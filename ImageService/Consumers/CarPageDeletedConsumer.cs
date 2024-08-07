using Contracts;
using MassTransit;
using ImageService.Interfaces;
using ImageService.Services;
using MassTransit.Configuration;
using ImageService.Options;
using Microsoft.Extensions.Options;

namespace ImageService.Consumers
{
    public class CarPageDeletedConsumer: IConsumer<CarPageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IStorageServiceCreator _storageServiceCreator;

        public CarPageDeletedConsumer(ICarPageRepository carPageRepository, IImageRepository imageRepository, IOptions<StorageOptions> options)
        {
            _carPageRepository = carPageRepository;
            _imageRepository = imageRepository;
            _storageServiceCreator = new StorageServiceCreator(options);
        }
        
        public async Task Consume(ConsumeContext<CarPageDeleted> context)
        {
            var message = context.Message;

            var images = await _imageRepository.GetAllAsync(i=>i.CarPageId==message.Id);
            var storageService = await _storageServiceCreator.CreateStorageServiceAsync();

            await _carPageRepository.DeleteAsync(message.Id);
            
            foreach (var image in images)
            {
                await storageService.DeleteAsync(image.Root);
                await _imageRepository.DeleteAsync(image.Id);
            }    
        }
    }
}