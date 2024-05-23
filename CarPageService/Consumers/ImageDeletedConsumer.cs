using MassTransit;
using Contracts;
using CarPageService.Interfaces;

namespace CarPageService.Consumers
{
    public class ImageDeletedConsumer: IConsumer<ImageDeleted>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ImageDeletedConsumer(ICarPageRepository carPageRepository, IImageRepository imageRepository, IPublishEndpoint publishEndpoint)
        {
            _carPageRepository = carPageRepository;
            _imageRepository = imageRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ImageDeleted> context)
        {
            var message = context.Message;

            var carPage = await _carPageRepository.GetAsync(message.CarPageId);

            if(message.IsMain)
            {
                carPage.MainImage = "https://firebasestorage.googleapis.com/v0/b/car-shop-e3217.appspot.com/o/CarImages%2Fdefault.jpg?alt=media&token=3a120b4c-30f8-4518-8444-c6e752b6e00b";
                
                await _publishEndpoint.Publish(new CarPageCreated
                { 
                    Id = carPage.Id,
                    Manufacturer = carPage.Manufacturer,
                    Model = carPage.Model,
                    Price = carPage.Price,
                    Package = carPage.Package,
                    Color = carPage.Color,
                    Year = carPage.Year,
                    Image = carPage.MainImage
                });
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