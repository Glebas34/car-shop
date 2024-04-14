using Contracts;
using MassTransit;
using CardService.Interfaces;

namespace CardService.Consumers
{
    public class CarPageDeletedConsumer: IConsumer<CarPageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly ICardRepository _cardRepository;

        public CarPageDeletedConsumer(ICarPageRepository carPageRepository, ICardRepository cardRepository)
        {
            _carPageRepository = carPageRepository;
            _cardRepository = cardRepository;
        }
        
        public async Task Consume(ConsumeContext<CarPageDeleted> context)
        {
            var message = context.Message;

            var card = await _cardRepository.GetAsync(c=>c.CarPageId==message.Id);

            await _carPageRepository.DeleteAsync(message.Id);
            await _cardRepository.DeleteAsync(card.Id);    
        }
    }
}