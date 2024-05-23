using CardService.Interfaces;
using CardService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CardService.Controllers
{
    [ApiController]
    [Route("Card")]
    public class CardController: ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICarPageRepository _carPageRepository;

        public CardController(ICardRepository cardRepository, ICarPageRepository carPageRepository)
        {
            _cardRepository = cardRepository;
            _carPageRepository = carPageRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var card = await _cardRepository.GetAsync(id);

            return Ok(card);
        }

        [Route("Filtered")]
        [HttpGet]
        public async Task<IActionResult> GetFilteredAsync(decimal maxPrice,string manufacturer="", string model="")
        {
            var cardDtos = await _cardRepository.
                GetAllAsync(c=>(c.Manufacturer==manufacturer||manufacturer=="")&&(c.Model==model||model=="")&&(c.Price<=maxPrice));

            return Ok(cardDtos);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cards = await _cardRepository.GetAllAsync();

            return Ok(cards);
        }

        [HttpPost("{carPageId}")]
        public async Task<IActionResult> PostAsync(Guid carPageId)
        {   
            var carPage = await _carPageRepository.GetAsync(carPageId);

            if(carPage==null)
            {
                return NotFound();
            }

            var card = new Card {
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Price = carPage.Price,
                Package = carPage.Package,
                CarPageId = carPageId,
                Color = carPage.Color,
                Year = carPage.Year,
                Image = carPage.Image
            };

            await _cardRepository.CreateAsync(card);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var card = await _cardRepository.GetAsync(id);

            if(card==null){
                return NotFound();
            }

            await _cardRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}