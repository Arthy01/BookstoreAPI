
using BookstoreAPI.Controllers;

namespace BookstoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5010); // Erlaubt Verbindungen von überall
            });
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();            

            app.MapControllers();

            app.Run();

        }
    }
}
