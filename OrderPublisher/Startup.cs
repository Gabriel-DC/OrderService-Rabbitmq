using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace OrderPublisher
{
    public class Startup
    {
        public IConfiguration ConfigRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            ConfigRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddMassTransit(busRegistrationConfig =>
            {
                busRegistrationConfig.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    const string RabbitmqUriString = "rabbitmq://localhost";
                    config.Host(new Uri(RabbitmqUriString), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });

            services.AddHealthChecks();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
