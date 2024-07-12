using CarPageService.Dtos;
using CarPageService.Entities;
using CarPageService.Extensions;
using CarPageService.Interfaces;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CarPageService.MinimalAPI
{
    public static class CarPageEndpoints
    {
        public static void MapCarPageEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/CarPage")
                .WithOpenApi();

            endpoints.MapGet("/", GetAllAsync)
                .WithSummary("Gets all car pages");
            endpoints.MapGet("/{id:required:guid}",GetAsync)
                .WithSummary("Gets car page");
            endpoints.MapPost("/", PostAsync)
                .WithSummary("Posts car page");
            endpoints.MapPut("/", PutAsync)
                .WithSummary("Updates car page");
            endpoints.MapDelete("/{id:required:guid}", DeleteAsync)
                .WithSummary("Deletes car page");
        }

        public static async Task<IResult> GetAllAsync(ICarPageRepository carPageRepository)
        {
            var carPages = await carPageRepository.GetAllAsync();

            return TypedResults.Ok(carPages);
        }

        public static async Task<Results<Ok<CarPageDto>,NotFound>> GetAsync(Guid id, ICarPageRepository carPageRepository)
        {
            var carPage = await carPageRepository.GetAsync(id);

            return carPage is CarPage?
                TypedResults.Ok(carPage.AsDto())
                : TypedResults.NotFound();
        }

        public static async Task<IResult> PostAsync(CreateCarPage createCarPage, ICarPageRepository carPageRepository, IPublishEndpoint publishEndpoint)
        {
            var carPage = new CarPage 
            {
                Id = new Guid(),
                Warranty = createCarPage.Warranty,
                Manufacturer = createCarPage.Manufacturer,
                Model = createCarPage.Model,
                Price = createCarPage.Price,
                Speed = createCarPage.Speed,
                Power = createCarPage.Power,
                Acceleration = createCarPage.Acceleration,
                FuelConsumption = createCarPage.FuelConsumption,
                Package = createCarPage.Package,
                Year = createCarPage.Year,
                Color =  createCarPage.Color,
                Images = new List<string>()
            };

            await carPageRepository.CreateAsync(carPage);

            await publishEndpoint.Publish(new CarPageCreated
            {
                Id = carPage.Id,
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Price = carPage.Price,
                Package = carPage.Package,
                Color = carPage.Color,
                Year = carPage.Year,
                Image = carPage.MainImage
            });

            return TypedResults.Created();
        }

        public static async Task<Results<NoContent,NotFound>> PutAsync(UpdateCarPage updateCarPage, ICarPageRepository carPageRepository, IPublishEndpoint publishEndpoint)
        {
            var existingCarPage = await carPageRepository.GetAsync(updateCarPage.Id);

            if (existingCarPage is null)
            {
                return TypedResults.NotFound();
            }

            existingCarPage.Manufacturer = updateCarPage.Manufacturer;
            existingCarPage.Model = updateCarPage.Model;
            existingCarPage.Package = updateCarPage.Package;
            existingCarPage.Warranty = updateCarPage.Warranty;
            existingCarPage.Price = updateCarPage.Price;
            existingCarPage.Speed = updateCarPage.Speed;
            existingCarPage.Power = updateCarPage.Power;
            existingCarPage.Acceleration = updateCarPage.Acceleration;
            existingCarPage.FuelConsumption = updateCarPage.FuelConsumption;
            existingCarPage.Year = updateCarPage.Year;
            existingCarPage.Color = updateCarPage.Color;

            await carPageRepository.UpdateAsync(existingCarPage);

            await publishEndpoint.Publish(new CarPageUpdated
            {
                Id = existingCarPage.Id,
                Manufacturer = existingCarPage.Manufacturer,
                Model = existingCarPage.Model,
                Price = existingCarPage.Price,
                Package = existingCarPage.Package,
                Color = existingCarPage.Color,
                Year = existingCarPage.Year,
                Image = existingCarPage.MainImage
            });

            return TypedResults.NoContent();
        }

        public static async Task<Results<NoContent,NotFound>> DeleteAsync(Guid id, ICarPageRepository carPageRepository, IPublishEndpoint publishEndpoint)
        {
            var carPage = await carPageRepository.GetAsync(id);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            await carPageRepository.DeleteAsync(id);

            await publishEndpoint.Publish(new CarPageDeleted
            {
                Id = id,
            });

            return TypedResults.NoContent();
        }
    }
}