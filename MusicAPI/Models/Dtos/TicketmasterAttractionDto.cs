using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json.Serialization;

namespace MusicAPI.Models.Dtos
{
    public class TicketmasterRootAttractionDto
    {
        [JsonPropertyName("_embedded")]
        public TicketmasterEmbeddedDto Embedded { get; set; }

    }
    public class TicketmasterAttractionDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class TicketmasterRootEventDto
    {
        [JsonPropertyName("_embedded")]
        public TicketmasterEmbeddedDto Embedded { get; set; }

    }
    public class TicketmasterEmbeddedDto
    {
        [JsonPropertyName("events")]
        public List<TicketmasterEventDto> Events { get; set; }

        [JsonPropertyName("venues")]
        public List<TicketmasterVenueDto> Venues { get; set; }

        [JsonPropertyName("attractions")]
        public List<TicketmasterAttractionDto> AttractionList { get; set; }
    }
    public class TicketmasterEventDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }


        [JsonPropertyName("dates")]
        public TicketmasterDatesDto Dates { get; set; }

        [JsonPropertyName("_embedded")]
        public TicketmasterEmbeddedDto EmbeddedEvent { get; set; }
    }

    public class TicketmasterDatesDto
    {
        [JsonPropertyName("start")]
        public TicketmasterStartDto Start { get; set; }

    }
    public class TicketmasterStartDto
    {
        [JsonPropertyName("localDate")]
        public string LocalDate { get; set; }


    }
    public class TicketmasterVenueDto
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("city")]
        public TicketmasterCityDto City { get; set; }

        [JsonPropertyName("country")]
        public TicketmasterCountryDto Country { get; set; }


    }
    public class TicketmasterCityDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }


    public class TicketmasterCountryDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

  

}
