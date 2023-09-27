using DataAccessLayer;
using DataService;
using DbInteractionService;
using InteractionService;

namespace DatabaseAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddBaseServices(builder.Services);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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

        public static void AddBaseServices(IServiceCollection services)
        {
            services.AddScoped<IDbInteraction, DbInteractionRealize>();
            services.AddScoped<IDataModelInteraction, FirstInteractionRealization>();

        }
    }
}