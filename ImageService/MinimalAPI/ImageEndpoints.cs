using ImageService.Interfaces;
using ImageService.Dtos;
using MassTransit;
using Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using ImageService.Entities;

namespace ImageService.MinimalAPI
{
    public static class ImageEndpoints
    {
        public static void MapImageEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/Image")
                .WithOpenApi();

            endpoints.MapGet("/{id:required:guid}", GetAllAsync)
                .WithSummary("Gets all images of car by id except main image");
            endpoints.MapGet("/MainImage/{id:required:guid}", GetMainAsync)
                .WithSummary("Gets main image of car");
            endpoints.MapPost("/", PostAsync)
                .WithSummary("Posts image of car");
            endpoints.MapPost("/MainImage", PostMainAsync)
                .WithSummary("Posts main image of car");
            endpoints.MapDelete("/{id:required:guid}", DeleteAsync)
                .WithSummary("Deletes image of car");
        }
        
        public static async Task<Results<Ok<List<Image>>,NotFound>> GetAllAsync(Guid carPageId, ICarPageRepository carPageRepository, IImageRepository imageRepository)
        {
            var carPage = await carPageRepository.GetAsync(carPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            var images = await imageRepository.GetAllAsync(i => i.CarPageId == carPageId && i.IsMain == false);

            return TypedResults.Ok(images);
        }

        
        public static async Task<Results<Ok<Image>,NotFound>> GetMainAsync(Guid carPageId, ICarPageRepository carPageRepository, IImageRepository imageRepository)
        {
            var carPage = await carPageRepository.GetAsync(carPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            var image = await imageRepository.GetAsync(i => i.CarPageId == carPageId && i.IsMain == true);

            return TypedResults.Ok(image);
        }

        public static async Task<Results<Created,NotFound>> PostAsync(CreateImage createImage, ICarPageRepository carPageRepository, IImageRepository imageRepository, IPublishEndpoint publishEndpoint, IStorageServiceCreator storageServiceCreator)
        {   
            var createStorageServiceTask = storageServiceCreator.CreateStorageServiceAsync();

            var carPageId = createImage.CarPageId;
            var carPage = await carPageRepository.GetAsync(carPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            var storageService = await createStorageServiceTask;
            
            var image = await storageService.UploadAsync(createImage, carPageId, false);

            await imageRepository.CreateAsync(image);

            await publishEndpoint.Publish(new ImageCreated{Id = image.Id, Url = image.Url, CarPageId = carPageId, IsMain = false});

            return TypedResults.Created();
        }

        public static async Task<Results<Created,NotFound>> PostMainAsync(CreateImage createImage, ICarPageRepository carPageRepository, IImageRepository imageRepository, IStorageServiceCreator storageServiceCreator, IPublishEndpoint publishEndpoint)
        {   
            var createStorageServiceTask = storageServiceCreator.CreateStorageServiceAsync();

            var carPageId = createImage.CarPageId;
            var carPage = await carPageRepository.GetAsync(carPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            var storageService = await createStorageServiceTask;

            var image = await storageService.UploadAsync(createImage, carPageId, true);

            var mainImage = await imageRepository.GetAsync(i => i.CarPageId == carPageId && i.IsMain);

            if (mainImage is Image)
            {
                await imageRepository.DeleteAsync(mainImage.Id);
                await storageService.DeleteAsync(mainImage.Root);

                await publishEndpoint.Publish(new ImageDeleted{Id = mainImage.Id, IsMain = mainImage.IsMain, CarPageId = mainImage.CarPageId});
            }

            await imageRepository.CreateAsync(image);

            await publishEndpoint.Publish(new ImageCreated{Id = image.Id, Url = image.Url, CarPageId = carPageId, IsMain = true});

            return TypedResults.Created();
        }

        public static async Task<Results<NoContent,NotFound>> DeleteAsync(Guid id, IImageRepository imageRepository, IStorageServiceCreator storageServiceCreator, IPublishEndpoint publishEndpoint)
        {
            var createStorageServiceTask = storageServiceCreator.CreateStorageServiceAsync();

            var image = await imageRepository.GetAsync(id);

            if(image is null)
            {
                return TypedResults.NotFound();
            }

            var storageService = await createStorageServiceTask;

            await storageService.DeleteAsync(image.Root);
            await imageRepository.DeleteAsync(image.Id);

            await publishEndpoint.Publish(new ImageDeleted{Id = image.Id, IsMain = image.IsMain, CarPageId = image.CarPageId});

            return TypedResults.NoContent();
        }
    }
}