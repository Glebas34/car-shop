using Contracts;
using MassTransit;
using ImageService.Interfaces;
using ImageService.Services;

namespace ImageService.Consumers
{
    public class CarPageDeletedConsumer: IConsumer<CarPageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageServiceCreator _storageServiceCreator;

        public CarPageDeletedConsumer(ICarPageRepository carPageRepository, IImageRepository imageRepository)
        {
            _carPageRepository = carPageRepository;
            _imageRepository = imageRepository;
            _storageServiceCreator = new StorageServiceCreator();
        }
        
        public async Task Consume(ConsumeContext<CarPageDeleted> context)
        {
            var message = context.Message;

            var images = await _imageRepository.GetAllAsync(i=>i.CarPageId==message.Id);
            var storageService = await _storageServiceCreator.CreateStorageService();

            await _carPageRepository.DeleteAsync(message.Id);
            foreach (var image in images)
            {
                await storageService.DeleteAsync(image.Root);
                await _imageRepository.DeleteAsync(image.Id);
            }    
        }
    }
}