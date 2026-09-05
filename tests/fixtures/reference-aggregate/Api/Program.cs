using Lazy.Crud.Builder.Api.Middlewares;
using Lazy.Crud.{Aggregate}.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.InjectDependencies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.OnAppInitialized();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
