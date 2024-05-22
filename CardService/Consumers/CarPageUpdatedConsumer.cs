using Contracts;
using MassTransit;
using CardService.Interfaces;

namespace CardService.Consumers
{
    public class CarPageUpdatedConsumer: IConsumer<CarPageUpdated>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly ICardRepository _cardRepository;

        public CarPageUpdatedConsumer(ICarPageRepository carPageRepository, ICardRepository cardRepository)
        {
            _carPageRepository = carPageRepository;
            _cardRepository = cardRepository;
        }
        
        public async Task Consume(ConsumeContext<CarPageUpdated> context)
        {
            var message = context.Message;

            var existingCarPage = await _carPageRepository.GetAsync(message.Id);
            var existingCard = await _cardRepository.GetAsync(c=>c.CarPageId==message.Id);
            
            existingCarPage.Manufacturer = message.Manufacturer;
            existingCarPage.Model = message.Model;
            existingCarPage.Price = message.Price;
            existingCarPage.Package = message.Package;
            existingCarPage.Color = message.Color;
            existingCarPage.Year = message.Year;
            existingCarPage.Image = message.Image;

            await _carPageRepository.UpdateAsync(existingCarPage);

            if(existingCard!=null)
            {
                existingCard.Manufacturer = existingCarPage.Manufacturer;
                existingCard.Model = existingCarPage.Model;
                existingCard.Price = existingCard.Price;
                existingCard.Package = existingCarPage.Package;
                existingCard.Color = existingCarPage.Color;
                existingCard.Year = existingCarPage.Year;
                existingCard.Image = existingCarPage.Image;

                await _cardRepository.UpdateAsync(existingCard);
            }
        }
    }
}