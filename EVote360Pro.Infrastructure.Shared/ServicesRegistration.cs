using EVote360Pro.Infrastructure.Shared.Services;
using EVote360Pro.Infrastructure.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVote360Pro.Infrastructure.Shared
{
    public static class ServicesRegistration
    {
        public static void AddSharedLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<EmailService>();
        }
    }
}