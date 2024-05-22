using Microsoft.AspNetCore.Mvc;
using CarPageService.Dtos;
using CarPageService.Interfaces;
using CarPageService.Entities;
using CarPageService.Extensions;
using MassTransit;
using Contracts;

namespace CarPageService.Controllers
{
    [ApiController]
    [Route("CarPage")]
    public class CarPageController: ControllerBase
    {
        private readonly ICarPageRepository _carPageRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public CarPageController(ICarPageRepository carPageRepository, IPublishEndpoint publishEndpoint)
        {
            _carPageRepository = carPageRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var carPages = await _carPageRepository.GetAllAsync();

            return Ok(carPages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var carPageDto = (await _carPageRepository.GetAsync(id)).AsDto();

            return Ok(carPageDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateCarPage createCarPage)
        {
            var carPage = new CarPage 
            {
                Id = new Guid(),
                Warranty = createCarPage.Warranty,
                Manufacturer = createCarPage.Manufacturer,
                Model = createCarPage.Model,
                Price = createCarPage.Price,
                Speed = createCarPage.Speed,
                Power = createCarPage.Power,
                Acceleration = createCarPage.Acceleration,
                FuelConsumption = createCarPage.FuelConsumption,
                Package = createCarPage.Package,
                Year = createCarPage.Year,
                Color =  createCarPage.Color,
                Images = new List<string>()
            };

            await _carPageRepository.CreateAsync(carPage);

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

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(UpdateCarPage updateCarPage)
        {
            var existingCarPage = await _carPageRepository.GetAsync(updateCarPage.Id);

            if (existingCarPage == null)
            {
                return NotFound();
            }

            existingCarPage.Manufacturer = updateCarPage.Manufacturer;
            existingCarPage.Model = updateCarPage.Model;
            existingCarPage.Package = updateCarPage.Package;
            existingCarPage.Warranty = updateCarPage.Warranty;
            existingCarPage.Price = updateCarPage.Price;
            existingCarPage.Speed = updateCarPage.Speed;
            existingCarPage.Power = updateCarPage.Power;
            existingCarPage.Acceleration = updateCarPage.Acceleration;
            existingCarPage.FuelConsumption = updateCarPage.FuelConsumption;
            existingCarPage.Year = updateCarPage.Year;
            existingCarPage.Color = updateCarPage.Color;

            await _carPageRepository.UpdateAsync(existingCarPage);

            await _publishEndpoint.Publish(new CarPageUpdated
            {
                Id = existingCarPage.Id,
                Manufacturer = existingCarPage.Manufacturer,
                Model = existingCarPage.Model,
                Price = existingCarPage.Price,
                Package = existingCarPage.Package,
                Color = existingCarPage.Color,
                Year = existingCarPage.Year,
                Image = existingCarPage.MainImage
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var carPage = await _carPageRepository.GetAsync(id);

            if(carPage==null)
            {
                return NotFound();
            }

            await _carPageRepository.DeleteAsync(id);

            await _publishEndpoint.Publish(new CarPageDeleted
            {
                Id = id,
            });

            return NoContent();
        }
    }
}