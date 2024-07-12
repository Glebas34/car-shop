using CardService.Entities;
using CardService.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CardService.MinimalAPI
{
    public static class CardEndpoints
    {
        public static void MapCardEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/Card")
                .WithOpenApi();

            endpoints.MapGet("/{id:required:guid}", GetAsync)
                .WithSummary("Gets card");
            endpoints.MapGet("/Filtered/{maxPrice:required:decimal}/{manufacturer?}/{model?}", GetFilteredAsync)
                .WithSummary("Gets cards by filter");
            endpoints.MapGet("/", GetAllAsync)
                .WithSummary("Gets all cards");
            endpoints.MapPost("/{id:required:guid}", PostAsync)
                .WithSummary("Posts card");
            endpoints.MapDelete("/{id:required:guid}", DeleteAsync)
                .WithSummary("Deletes card");
        }

        public static async Task<Results<Ok<Card>,NotFound>> GetAsync(Guid id, ICardRepository cardRepository)
        {
            var card = await cardRepository.GetAsync(id);

            return card is Card?
                TypedResults.Ok(card)
                :TypedResults.NotFound();
        }

        public static async Task<IResult> GetFilteredAsync(ICardRepository cardRepository, decimal maxPrice, string manufacturer="", string model="")
        {
            var cards = await cardRepository
                .GetAllAsync(c => (c.Manufacturer == manufacturer || manufacturer == "") && (c.Model == model || model=="") && (c.Price <= maxPrice));

            return TypedResults.Ok(cards);
        }


        public static async Task<IResult> GetAllAsync(ICardRepository cardRepository)
        {
            var cards = await cardRepository.GetAllAsync();

            return TypedResults.Ok(cards);
        }

        public static async Task<Results<Created, NotFound, BadRequest>> PostAsync(Guid carPageId, ICarPageRepository carPageRepository, ICardRepository cardRepository)
        {   
            var carPage = await carPageRepository.GetAsync(carPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }

            var card = await cardRepository.GetAsync(c => c.CarPageId == carPageId);

            if(card is Card)
            {
                return TypedResults.BadRequest();
            }

            card = new Card 
            {
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Price = carPage.Price,
                Package = carPage.Package,
                CarPageId = carPageId,
                Color = carPage.Color,
                Year = carPage.Year,
                Image = carPage.Image
            };

            await cardRepository.CreateAsync(card);

            return TypedResults.Created();
        }

        public static async Task<Results<NoContent, NotFound>> DeleteAsync(Guid id, ICardRepository cardRepository)
        {
            var card = await cardRepository.GetAsync(id);

            if(card is null)
            {
                return TypedResults.NotFound();
            }

            await cardRepository.DeleteAsync(id);

            return TypedResults.NoContent();
        }
    }
}