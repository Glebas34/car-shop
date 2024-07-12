using Microsoft.EntityFrameworkCore;
using ImageService.Interfaces;
using ImageService.Repositories;
using ImageService.Database;
using MassTransit;
using ImageService.Consumers;
using ImageService.Options;
using ImageService.MinimalAPI;
using ImageService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ICarPageRepository, CarPageRepository>();
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(nameof(StorageOptions)));
builder.Services.AddTransient<IStorageServiceCreator,StorageServiceCreator>();
builder.Services.AddMassTransit(busConfigurator=>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<CarPageCreatedConsumer>()
        .Endpoint(c => c.InstanceId = "image-service");
    busConfigurator.AddConsumer<CarPageDeletedConsumer>()
        .Endpoint(c => c.InstanceId = "image-service");

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

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapImageEndpoints();

app.Run();

