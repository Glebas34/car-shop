using Microsoft.EntityFrameworkCore;
using CarPageService.Interfaces;
using CarPageService.Repositories;
using CarPageService.Database;
using MassTransit;
using CarPageService.Consumers;

System.Net.ServicePointManager.ServerCertificateValidationCallback += 
    (s, cert, chain, sslPolicyErrors) => true;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<ICarPageRepository,CarPageRepository>();
builder.Services.AddScoped<IImageRepository,ImageRepository>();
builder.Services.AddMassTransit(busConfigurator=>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

     busConfigurator.AddConsumer<ImageCreatedConsumer>()
        .Endpoint(c => c.InstanceId = "carpage-service");
    busConfigurator.AddConsumer<ImageDeletedConsumer>()
        .Endpoint(c => c.InstanceId = "carpage-service");

    busConfigurator.UsingRabbitMq((context, configurator)=>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h=> 
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

