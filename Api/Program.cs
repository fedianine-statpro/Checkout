using Api.DB.Payments.Commands;
using Api.DB.Payments.Queries;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<BankSimulator.IPayment, BankSimulator.Payment>();
            builder.Services.AddSingleton<PaymentDbContext>();
            builder.Services.AddSingleton<IPaymentReadRepository, PaymentReadRepository>();
            builder.Services.AddSingleton<IPaymentWriteRepository, PaymentWriteRepository>();

            // Add services to the container.
            builder.Services.AddControllers();

            // WebApi Versioning
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
    }
}