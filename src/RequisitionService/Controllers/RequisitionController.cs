using Microsoft.AspNetCore.Mvc;
using RequisitionService.Interfaces;
using RequisitionService.Entities;
using RequisitionService.Dtos;

namespace RequisitionService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RequisitionController: ControllerBase
    {
        private readonly IRequisitionRepository _requisitionRepository;
        private readonly ICarPageRepository _carPageRepository;

        public RequisitionController(IRequisitionRepository requisitionRepository, ICarPageRepository carPageRepository)
        {
            _requisitionRepository = requisitionRepository;
            _carPageRepository = carPageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var requisitions = await _requisitionRepository.GetAllAsync();

            return Ok(requisitions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            
            var requisition = await _requisitionRepository.GetAsync(id);

            if(requisition==null)
            {
                return NotFound();
            }

            return Ok(requisition);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateRequisition createRequisition)
        {
            var carPage = await _carPageRepository.GetAsync(createRequisition.CarPageId);

            if(carPage==null)
            {
                return NotFound();
            }
            
            var requisition = new Requisition
            {
                Id = new Guid(),
                FullName = createRequisition.FullName,
                PhoneNumber = createRequisition.PhoneNumber,
                CarPageId = createRequisition.CarPageId,
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Price = carPage.Price,
                Color = carPage.Color,
                Package = carPage.Package,
                Year = carPage.Year
            };

            await _requisitionRepository.CreateAsync(requisition);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var requisition = await _requisitionRepository.GetAsync(id);

            if(requisition==null)
            {
                return NotFound();
            }

            await _requisitionRepository.DeleteAsync(id);
            
            return NoContent();
        }
    }
}