using Microsoft.AspNetCore.Http.HttpResults;
using RequisitionService.Interfaces;
using RequisitionService.Entities;
using RequisitionService.Dtos;

namespace RequisitionService.MinimalAPI
{
    public static class RequisitionEndpoints
    {
        public static void MapRequisitionEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("/Requisition")
                .WithOpenApi();
            
            endpoints.MapGet("/", GetAllAsync)
                .WithSummary("Gets all requisitions");
            endpoints.MapGet("/{id:required:guid}", GetAsync)
                .WithSummary("Gets requisition");
            endpoints.MapPost("/", PostAsync)
                .WithSummary("Posts requisition");
            endpoints.MapDelete("/{id:required:guid}", DeleteAsync)
                .WithSummary("Deletes requisition");
        }

        public static async Task<IResult> GetAllAsync(IRequisitionRepository requisitionRepository)
        {
            var requisitions = await requisitionRepository.GetAllAsync();

            return TypedResults.Ok(requisitions);
        }

        public static async Task<Results<Ok<Requisition>, NotFound>> GetAsync(Guid id, IRequisitionRepository requisitionRepository)
        {   
            var requisition = await requisitionRepository.GetAsync(id);

            return requisition is Requisition?
                TypedResults.Ok(requisition)
                : TypedResults.NotFound();
        }

        public static async Task<Results<Created,NotFound>> PostAsync(CreateRequisition createRequisition, ICarPageRepository carPageRepository, IRequisitionRepository requisitionRepository)
        {
            var carPage = await carPageRepository.GetAsync(createRequisition.CarPageId);

            if(carPage is null)
            {
                return TypedResults.NotFound();
            }
            
            var requisition = new Requisition
            {
                Id = new Guid(),
                FullName = createRequisition.FullName,
                PhoneNumber = createRequisition.PhoneNumber,
                CarPageId = createRequisition.CarPageId,
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Price = carPage.Price,
                Color = carPage.Color,
                Package = carPage.Package,
                Year = carPage.Year,
                CarImage = carPage.Image
            };

            await requisitionRepository.CreateAsync(requisition);

            return TypedResults.Created();
        }

        public static async Task<Results<NoContent,NotFound>> DeleteAsync(Guid id, IRequisitionRepository requisitionRepository)
        {
            var requisition = await requisitionRepository.GetAsync(id);

            if(requisition is null)
            {
                return TypedResults.NotFound();
            }

            await requisitionRepository.DeleteAsync(id);
            
            return TypedResults.NoContent();
        }
    }
}