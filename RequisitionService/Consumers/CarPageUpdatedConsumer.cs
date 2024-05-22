using Contracts;
using MassTransit;
using RequisitionService.Interfaces;

namespace RequisitionService.Consumers
{
    public class CarPageUpdatedConsumer: IConsumer<CarPageUpdated>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IRequisitionRepository _requisitionRepository;
        
        public CarPageUpdatedConsumer(ICarPageRepository carPageRepository, IRequisitionRepository requisitionRepository)
        {
            _carPageRepository = carPageRepository;
            _requisitionRepository = requisitionRepository;
        }

        public async Task Consume(ConsumeContext<CarPageUpdated> context)
        {
            var message = context.Message;

            var existingCarPage = await _carPageRepository.GetAsync(message.Id);
            var existingRequisition = await _requisitionRepository.GetAsync(r=>r.CarPageId==message.Id);
            
            existingCarPage.Manufacturer = message.Manufacturer;
            existingCarPage.Model = message.Model;
            existingCarPage.Price = message.Price;
            existingCarPage.Package = message.Package;
            existingCarPage.Color = message.Color;
            existingCarPage.Year = message.Year;
            existingCarPage.Image = message.Image;

            await _carPageRepository.UpdateAsync(existingCarPage);

            if(existingRequisition!=null)
            {
                existingRequisition.Manufacturer = existingCarPage.Manufacturer;
                existingRequisition.Model = existingCarPage.Model;
                existingRequisition.Price = existingCarPage.Price;
                existingRequisition.Package = existingCarPage.Package;
                existingRequisition.Color = existingCarPage.Color;
                existingRequisition.Year = existingCarPage.Year;

                await _requisitionRepository.UpdateAsync(existingRequisition);
            }
        }
    }
}