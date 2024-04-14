using Contracts;
using CardService.Entities;
using MassTransit;
using CardService.Interfaces;

namespace CardService.Consumers
{
    public class CarPageCreatedConsumer: IConsumer<CarPageCreated>
    {
        private readonly ICarPageRepository _carPageRepository;

        public CarPageCreatedConsumer(ICarPageRepository carPageRepository)
        {
            _carPageRepository = carPageRepository;
        }
        
        public async Task Consume(ConsumeContext<CarPageCreated> context)
        {
            var message = context.Message;

            var carPage = await _carPageRepository.GetAsync(message.Id);
            
            if(carPage != null)
            {
                return;
            }

            carPage = new CarPage
            {
                Id = message.Id,
                Manufacturer = message.Manufacturer,
                Model = message.Model,
                Price = message.Price,
                Package = message.Package,
                Color = message.Color,
                Year = message.Year
            };

            await _carPageRepository.CreateAsync(carPage);
        }
    }
}