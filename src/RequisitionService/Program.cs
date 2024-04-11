using Microsoft.EntityFrameworkCore;
using RequisitionService.Database;
using RequisitionService.Interfaces;
using RequisitionService.Repositories;
using RequisitionService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddScoped<IRequisitionRepository,RequisitionRepository>();
builder.Services.AddScoped<ICarPageRepository,CarPageRepository>();
builder.Services.AddMassTransit(busConfigurator=>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<CarPageCreatedConsumer>()
        .Endpoint(c => c.InstanceId = "requisition-service");
    busConfigurator.AddConsumer<CarPageUpdatedConsumer>()
        .Endpoint(c => c.InstanceId = "requisition-service");
    busConfigurator.AddConsumer<CarPageDeletedConsumer>()
        .Endpoint(c => c.InstanceId = "requisition-service");

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
