using MassTransit;
using Contracts;
using CarPageService.Interfaces;
using CarPageService.Entities;

namespace CarPageService.Consumers
{
    public class ImageDeletedConsumer: IConsumer<ImageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IImageRepository _imageRepository;

        public ImageDeletedConsumer(ICarPageRepository carPageRepository, IImageRepository imageRepository)
        {
            _carPageRepository = carPageRepository;
            _imageRepository = imageRepository;
        }

        public async Task Consume(ConsumeContext<ImageDeleted> context)
        {
            var message = context.Message;

            var carPage = await _carPageRepository.GetAsync(message.CarPageId);

            if(message.IsMain)
            {
                carPage.MainImage = "";
            }
            else
            {
                carPage.Images.Remove(message.Url);
            }

            await _carPageRepository.UpdateAsync(carPage);
            await _imageRepository.DeleteAsync(message.Id);
        }
    }
}