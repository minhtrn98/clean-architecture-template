using FastEndpoints.Swagger;

namespace Web.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerGen();

        return app;
    }
}
