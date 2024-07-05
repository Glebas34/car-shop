using ImageService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ImageService.Dtos;
using MassTransit;
using Contracts;
using ImageService.Services;
using ImageService.Options;
using Microsoft.Extensions.Options;

namespace ImageService.Controllers
{
    [ApiController]
    [Route("Image")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICarPageRepository _carPageRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IStorageServiceCreator _storageServiceCreator;

        public ImageController(IImageRepository imageRepository, ICarPageRepository carPageRepository, IPublishEndpoint publishEndpoint, IOptions<StorageOptions> options)
        {
            _imageRepository = imageRepository;
            _carPageRepository = carPageRepository;
            _publishEndpoint = publishEndpoint;
            _storageServiceCreator = new StorageServiceCreator(options);
        }

        [HttpGet("{carPageId}")]
        public async Task<IActionResult> GetAllAsync(Guid carPageId)
        {
            var carPage = await _carPageRepository.GetAsync(carPageId);

            if(carPage == null)
            {
                return NotFound();
            }

            var images = await _imageRepository.GetAllAsync(i => i.CarPageId == carPageId && i.IsMain == false);

            return Ok(images);
        }

        [HttpGet("MainImage/{carPageId}")]
        public async Task<IActionResult> GetMainAsync(Guid carPageId)
        {
            var carPage = await _carPageRepository.GetAsync(carPageId);

            if(carPage == null)
            {
                return NotFound();
            }

            var images = await _imageRepository.GetAsync(i => i.CarPageId == carPageId && i.IsMain == true);

            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateImage createImage)
        {   
            var createStorageServiceTask = _storageServiceCreator.CreateStorageServiceAsync();

            var carPageId = createImage.CarPageId;
            var carPage = await _carPageRepository.GetAsync(carPageId);

            if(carPage == null)
            {
                return NotFound();
            }

            var storageService = await createStorageServiceTask;
            
            var image = await storageService.UploadAsync(createImage, carPageId, false);

            await _imageRepository.CreateAsync(image);

            await _publishEndpoint.Publish(new ImageCreated{Id = image.Id, Url = image.Url, CarPageId = carPageId, IsMain = false});

            return Ok();
        }

        [HttpPost("MainImage")]
        public async Task<IActionResult> PostMainAsync(CreateImage createImage)
        {   
            var createStorageServiceTask = _storageServiceCreator.CreateStorageServiceAsync();

            var carPageId = createImage.CarPageId;
            var carPage = await _carPageRepository.GetAsync(carPageId);

            if(carPage == null)
            {
                return NotFound();
            }

            var storageService = await createStorageServiceTask;

            var image = await storageService.UploadAsync(createImage, carPageId, true);

            var mainImage = await _imageRepository.GetAsync(i => i.CarPageId == carPageId && i.IsMain);

            if (mainImage != null)
            {
                await _imageRepository.DeleteAsync(mainImage.Id);
                await storageService.DeleteAsync(mainImage.Root);

                await _publishEndpoint.Publish(new ImageDeleted{Id = mainImage.Id, IsMain = mainImage.IsMain, CarPageId = mainImage.CarPageId});
            }

            await _imageRepository.CreateAsync(image);

            await _publishEndpoint.Publish(new ImageCreated{Id = image.Id, Url = image.Url, CarPageId = carPageId, IsMain = true});

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var createStorageServiceTask = _storageServiceCreator.CreateStorageServiceAsync();

            var image = await _imageRepository.GetAsync(id);

            if(image == null)
            {
                return NotFound();
            }

            var storageService = await createStorageServiceTask;

            await storageService.DeleteAsync(image.Root);
            await _imageRepository.DeleteAsync(image.Id);

            await _publishEndpoint.Publish(new ImageDeleted{Id = image.Id, IsMain = image.IsMain, CarPageId = image.CarPageId});

            return NoContent();
        }
    }
}