using Amazon.SQS;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Services;


namespace Common.Extensions
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddSqsCustomerService(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("ServiceConfiguration");
            services.AddAWSService<IAmazonSQS>();
            services.Configure<ServiceConfiguration>(appSettingsSection);
            services.AddSingleton<SqsCustomerService>();
            return services;
        }




    }
}
