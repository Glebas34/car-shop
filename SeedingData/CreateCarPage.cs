using System.Text.Json.Serialization;
namespace SeedingData.Entities
{
    public record CreateCarPage
    {
        [JsonPropertyName("manufacturer")]
        public string Manufacturer {get;set;}
        [JsonPropertyName("model")]
        public string Model {get;set;}
        [JsonPropertyName("price")]
        public decimal Price {get;set;}
        [JsonPropertyName("Color")]
        public string Color {get;set;}
        [JsonPropertyName("warranty")]
        public string Warranty {get;set;}
        [JsonPropertyName("speed")]
        public string Speed {get;set;}
        [JsonPropertyName("power")]
        public string Power {get;set;}
        [JsonPropertyName("acceleration")]
        public string Acceleration {get;set;}
        [JsonPropertyName("fuelConsumption")]
        public string FuelConsumption {get;set;}
        [JsonPropertyName("package")]
        public string Package {get;set;}
        [JsonPropertyName("year")]
        public int Year {get;set;}
    }
}