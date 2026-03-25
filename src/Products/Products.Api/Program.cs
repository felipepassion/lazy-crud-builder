using Lazy.Crud.Builder.Api.Middlewares;
                                using Lazy.Crud.Products.Api;

                                var builder = WebApplication.CreateBuilder(args);

                                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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