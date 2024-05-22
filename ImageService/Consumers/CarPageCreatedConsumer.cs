using Contracts;
using ImageService.Entities;
using MassTransit;
using ImageService.Interfaces;

namespace ImageService.Consumers
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
            };

            await _carPageRepository.CreateAsync(carPage);
        }
    }
}