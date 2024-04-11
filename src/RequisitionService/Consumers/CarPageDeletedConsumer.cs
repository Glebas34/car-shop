using Contracts;
using MassTransit;
using RequisitionService.Interfaces;

namespace RequisitionService.Consumers
{
    public class CarPageDeletedConsumer: IConsumer<CarPageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IRequisitionRepository _requisitionRepository;

        public CarPageDeletedConsumer(ICarPageRepository carPageRepository, IRequisitionRepository requisitionRepository)
        {
            _carPageRepository = carPageRepository;
            _requisitionRepository = requisitionRepository;
        }
        
        public async Task Consume(ConsumeContext<CarPageDeleted> context)
        {
            var message = context.Message;

            var requisition = await _requisitionRepository.GetAsync(c=>c.CarPageId==message.Id);

            await _carPageRepository.DeleteAsync(message.Id);
            await _requisitionRepository.DeleteAsync(requisition.Id);    
        }
    }
}