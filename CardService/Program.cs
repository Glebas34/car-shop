using CardService.Database;
using CardService.Interfaces;
using CardService.Repositories;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using CardService.Consumers;
using CardService.MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<ICardRepository,CardRepository>();
builder.Services.AddScoped<ICarPageRepository,CarPageRepository>();
builder.Services.AddMassTransit(busConfigurator=>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<CarPageCreatedConsumer>()
        .Endpoint(c => c.InstanceId = "card-service");
    busConfigurator.AddConsumer<CarPageUpdatedConsumer>()
        .Endpoint(c => c.InstanceId = "card-service");
    busConfigurator.AddConsumer<CarPageDeletedConsumer>()
        .Endpoint(c => c.InstanceId = "card-service");

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

//app.UseAuthorization();


app.MapCardEndpoints();

app.Run();
