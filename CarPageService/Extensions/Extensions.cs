using CarPageService.Entities;
using CarPageService.Dtos;

namespace CarPageService.Extensions
{
    public static class Extensions
    {
        public static CarPageDto AsDto(this CarPage carPage)
        {
            return new CarPageDto 
            {
                Manufacturer = carPage.Manufacturer,
                Model = carPage.Model,
                Warranty = carPage.Warranty, 
                Price = carPage.Price,
                Speed = carPage.Speed,
                Power = carPage.Power,
                Acceleration = carPage.Acceleration,
                FuelConsumption = carPage.FuelConsumption,
                Package = carPage.Package,
                MainImage = carPage.MainImage,
                Images = carPage.Images,
                Year = carPage.Year,
                Color = carPage.Color
            };
        }
    }
}