using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace DFC.Compui.Telemetry.ApplicationBuilderExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddOperationIdToRequests(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Response.Headers.Add("OperationId", Activity.Current.TraceId.ToString());
                return next.Invoke();
            });

            return app;
        }
    }
}
