using Autofac;
using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using OrderManagement.Api.StartupInfra;

var builder = WebApplication.CreateSlimBuilder(args);
//var builder = WebApplication.CreateBuilder(args); // TODO: try this alternative!

builder.Host
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new ApplicationModule());
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddSingleDatabase(builder.Configuration)
    .AddFastEndpoints()
    .AddOpenApiSpecs()
    .AddHttpGlobalExceptionHandler();

var app = builder.Build();

app.UseFastEndpoints();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
}

app.Run();