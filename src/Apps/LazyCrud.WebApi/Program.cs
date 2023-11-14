using LazyCrud.Marketplace.WebApi;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    //options.SerializerSettings.Converters.Add(new StringEnumConverter());
    // Use the default property (Pascal) casing
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    var settings = options.SerializerSettings;
    settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
    settings.NullValueHandling = NullValueHandling.Ignore;
    settings.DefaultValueHandling = DefaultValueHandling.Ignore;
    settings.Formatting = Formatting.Indented;
    settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
    options.AllowInputFormatterExceptionMessages = false;
    //settings.Converters.Add(new TiimeOnlyJsonConverter());
});

Environment.SetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "0");

CultureInfo invariantCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

IoCFactory.InjectDependencies(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Migrate();
//Task.Run(app.SeedAdministratorUser);

app.Run();
