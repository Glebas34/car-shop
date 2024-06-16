using System.Threading;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Text.Json;
using SeedingData.Entities;
using System.Text;

static async Task<string> GetData(HttpClient client, string url)
{
    var response = await client.GetAsync("car-page-service/CarPage");
    return await response.Content.ReadAsStringAsync();
}

HttpClient client = new()
{
    BaseAddress = new Uri("http://localhost:4043/")
};

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
var carPages = JsonSerializer.Deserialize<List<CarPage>>(await GetData(client,"car-page-service/CarPage"), options);

if (carPages.Count != 0)
{
    return;
}

var data = new List<CreateCarPage>
{
    new CreateCarPage
    {
        Manufacturer = "BMW",
        Model = "420d xDrive Gran Coupe",
        Price = 6896800,
        Color = "Синий Танзанит металлик",
        Warranty = "5 лет",
        Speed = "300 км/ч",
        Power = "384 л.с.",
        Acceleration = "4,7 с",
        FuelConsumption = "8,2л/100км",
        Package = "M Sport Pure",
        Year = 2023
    },
    new CreateCarPage
    {
        Manufacturer = "BMW",
        Model = "M340i xDrive",
        Price = 6750000,
        Color = "Морозный Черный металлик",
        Warranty = "5 лет",
        Speed = "300 км/ч",
        Power = "340 л.с.",
        Acceleration = "5,5 с",
        FuelConsumption = "7,7л/100км",
        Package = "BMW M 50 Years Special Edition",
        Year = 2023
    },
    new CreateCarPage
    {
        Manufacturer = "BMW",
        Model = "X6 xDrive30d",
        Price = 8400000,
        Color = "Искрящийся Коричневый металлик",
        Warranty = "5 лет",
        Speed = "250 км/ч",
        Power = "290 л.с.",
        Acceleration = "5,5 с",
        FuelConsumption = "6,9л/100км",
        Package = "Base",
        Year = 2023
    },
    new CreateCarPage
    {
        Manufacturer = "Porsche",
        Model = "718 Cayman GT4",
        Price = 15990000,
        Color = "Желтый",
        Warranty = "5 лет",
        Speed = "300 км/ч",
        Power = "420 л.с.",
        Acceleration = "4,4 с",
        FuelConsumption = "10,4л/100км",
        Package = "Sport",
        Year = 2023
    },
    new CreateCarPage
    {
        Manufacturer = "Porsche",
        Model = "Panamera Turbo S Executive",
        Price = 17990000,
        Color = "Синий",
        Warranty = "5 лет",
        Speed = "315 км/ч",
        Power = "630 л.с.",
        Acceleration = "3,2 с",
        FuelConsumption = "11,8л/100км",
        Package = "Sport",
        Year = 2023
    },
    new CreateCarPage
    {
        Manufacturer= "Porsche",
        Model = "Cayenne Turbo Coupe",
        Price = 17990000,
        Color = "Серый",
        Warranty = "5 лет",
        Speed = "286 км/ч",
        Power = "550 л.с.",
        Acceleration = "3,9 с",
        FuelConsumption = "12,3л/100км",
        Package = "Sport",
        Year = 2023
    },
       new CreateCarPage
    {
        Manufacturer = "Mercedes-Benz",
        Model = "S-Class S 580 4MATIC",
        Price = 15000000,
        Color = "Черный",
        Warranty = "5 лет",
        Speed = "250 км/ч",
        Power = "496 л.с.",
        Acceleration = "4,4 с",
        FuelConsumption = "9,0л/100км", 
        Package = "Luxury",
        Year = 2024,

    },
      new CreateCarPage
    {
        Manufacturer = "Mercedes-Benz",
        Model = "G-Class G 63 AMG",
        Price = 18000000,
        Color = "Полярно-белый",
        Warranty = "5 лет",
        Speed = "240 км/ч",
        Power = "577 л.с.",
        Acceleration = "4,5 с",
        FuelConsumption = "13,1л/100км",
        Package = "AMG",
        Year = 2023
    },
      new CreateCarPage
    {
        Manufacturer = "Mercedes-Benz",
        Model = "E-Class E 53 AMG 4MATIC+",
        Price = 13000000,
        Color = "Серый",
        Warranty = "5 лет",
        Speed = "250 км/ч",
        Power = "429 л.с.",
        Acceleration = "4,4 с",
        FuelConsumption = "8,9л/100км",
        Package = "AMG",
        Year = 2023
    },
    };

foreach(var carPage in data)
{
    var content = new StringContent(JsonSerializer.Serialize(carPage),Encoding.UTF8, "application/json");
    var response = await client.PostAsync("car-page-service/CarPage", content);
}

Thread.Sleep(5000);

carPages = JsonSerializer.Deserialize<List<CarPage>>(await GetData(client,"car-page-service/CarPage"), options);

foreach(var carPage in carPages)
{
    string path = $"Images/{carPage.Manufacturer}/{carPage.Model}/";

    using (var formData = new MultipartFormDataContent())
    {
        var imageContent = new StreamContent(File.OpenRead(path+"MainImage.png"));

        formData.Add(new StringContent(carPage.Id.ToString()),"CarPageId");
        formData.Add(imageContent, "Image", "MainImage.png");

        await client.PostAsync("image-service/Image/MainImage", formData);
    }

    for(int i = 1; i <=3; i++)
    {
        using (var formData = new MultipartFormDataContent())
        {

            var imageContent = new StreamContent(File.OpenRead(path+$"{i}.png"));

            formData.Add(new StringContent(carPage.Id.ToString()),"CarPageId");
            formData.Add(imageContent, "Image", $"{i}.png");

            await client.PostAsync("image-service/Image", formData);
        }
    }

    await client.PostAsync($"card-service/Card/{carPage.Id}",null);
}





