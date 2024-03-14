using System.Text.Json.Serialization;

namespace CountriesProject.Models
{
    public class Country {

        [JsonIgnore]
        public int? id { get; set; }
        public string? name { get; set; }
        public string[] borders { get; set; }
        public string? capital { get; set; }

    }
}
