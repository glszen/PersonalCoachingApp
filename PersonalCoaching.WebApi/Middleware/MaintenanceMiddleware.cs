using PersonalCoachingApp.Business.Operations.Setting;

namespace PersonalCoaching.WebApi.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        // IServiceProvider'i enjekte ediyoruz
        public MaintenanceMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            // Scoped servisleri çözmek için yeni bir scope oluşturuyoruz
            using (var scope = _serviceProvider.CreateScope())
            {
                var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();

                bool maintenanceMode = settingService.GetMaintencenceState();

                if(context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
                {
                    await _next(context);
                    return;
                }
                if (maintenanceMode)
                {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsync("We are currently unable to provide service.");
                }
                else
                {
                    await _next(context);
                }
            }
        }
    }
}
