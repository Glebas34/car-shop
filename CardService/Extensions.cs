using CardService.Entities;
using CardService.Dtos;

namespace CardService
{
    public static class Extensions
    {
        public static CardDto AsDto(this Card card)
        {
            return new CardDto
            {
                Manufacturer=card.Manufacturer,
                Model=card.Model, 
                CarPageId=card.CarPageId, 
                Price=card.Price,
                Package=card.Package, 
                Color = card.Color,
                Year = card.Year,
                Image = card.Image
            };
        }
    }
}