using MassTransit;
using Contracts;
using CarPageService.Interfaces;
using CarPageService.Entities;

namespace CarPageService.Consumers
{
    public class ImageCreatedConsumer: IConsumer<ImageCreated>
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ImageCreatedConsumer(ICarPageRepository carPageRepository, IImageRepository imageRepository, IPublishEndpoint publishEndpoint)
        {
            _carPageRepository = carPageRepository;
            _imageRepository = imageRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ImageCreated> context)
        {
            var message = context.Message;

            var carPage = await _carPageRepository.GetAsync(message.CarPageId);

            if(message.IsMain)
            {
                carPage.MainImage = message.Url;
            }
            else
            {
                carPage.Images.Add(message.Url);
            }

            await _carPageRepository.UpdateAsync(carPage);
            await _imageRepository.CreateAsync(new Image{Id = message.Id, Url = message.Url});
            
            if(message.IsMain){
                await _publishEndpoint.Publish(new CarPageUpdated
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
        }
    }
}