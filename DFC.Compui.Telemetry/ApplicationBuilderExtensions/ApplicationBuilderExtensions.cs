using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace DFC.Compui.Telemetry.ApplicationBuilderExtensions
{
    public static class ApplicationBuilderExtensions
    {
        private const string OperationId = "OperationId";

        public static IApplicationBuilder AddOperationIdToRequests(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                if (!context.Response.Headers.ContainsKey(OperationId))
                {
                    context.Response.Headers.Add(OperationId, Activity.Current.TraceId.ToString());
                }

                return next.Invoke();
            });

            return app;
        }
    }
}
