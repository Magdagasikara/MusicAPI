using Microsoft.Data.SqlClient.Server;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Models.ViewModel;
using MusicAPI.Repositories;
using System.Text.Json;

namespace MusicAPI.Services
{
    public interface ITicketmasterHelper
    {
        Task<TicketmasterRootAttractionDto> FindArtistAsync(string artistName);
        Task<TicketmasterRootEventDto> FindConcertsByArtistAsync(string artistId);
        Task<TicketmasterArtistConcertsViewModel> ReturnConcertsForArtistAsync(string searchArtist);
    }


    public class TicketmasterHelper : ITicketmasterHelper
    {
        private HttpClient _httpClient;
       private string _ticketmasterKey;


        public TicketmasterHelper(HttpClient httpClient, string ticketmasterKey)
        {

            _httpClient = httpClient;
            _ticketmasterKey= ticketmasterKey;

        }
        public TicketmasterHelper(string ticketmasterKey):this(new HttpClient(), ticketmasterKey) {}

        public async Task<TicketmasterArtistConcertsViewModel> ReturnConcertsForArtistAsync(string searchArtist)
        {

            bool correctArtistName = false;
            while (!correctArtistName)
            {

                var artist = await FindArtistAsync(searchArtist);
                if (artist is null || artist.Embedded is null)
                {
                    throw new TicketmasterArtistNotFoundException();
                }

                foreach (var item in artist.Embedded.AttractionList)
                {
                    if (item.Name.ToUpper() == searchArtist.ToUpper())
                    {
                        correctArtistName = true;
                        var concert = await FindConcertsByArtistAsync(item.Id);
                        if (concert.Embedded is null)
                        {
                            throw new TicketmasterConcertsNotFoundException();
                        }

                        List<TicketmasterConcertViewModel> concerts = new List<TicketmasterConcertViewModel>();
                        foreach (var ev in concert.Embedded.Events)
                        {

                            foreach (var ven in ev.EmbeddedEvent.Venues)
                            {

                                concerts.Add(new TicketmasterConcertViewModel
                                {
                                    City = ven.City.Name,
                                    Country = ven.Country.Name,
                                    Date = ev.Dates.Start.LocalDate,
                                    Website = ev.Url
                                });
                            }
                        }
                        
                        // artist and concerts found - return result
                        return new TicketmasterArtistConcertsViewModel() { ArtistName = item.Name, Concerts = concerts };
                    }

                }
                // last option if no exact match on artist name
                throw new TicketmasterArtistNotFoundException();
            }
            throw new TicketmasterArtistNotFoundException();
        }
        public async Task<TicketmasterRootAttractionDto> FindArtistAsync(string artistName)
        {

            string url = $"https://app.ticketmaster.com/discovery/v2/attractions.json?apikey={_ticketmasterKey}&keyword={Uri.EscapeDataString(artistName)}";

            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                HttpResponseMessage response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();
                string artist = await response.Content.ReadAsStringAsync();
                TicketmasterRootAttractionDto result = JsonSerializer.Deserialize<TicketmasterRootAttractionDto>(artist);
                await Console.Out.WriteLineAsync(artist);
                return result;
            }
        }

        public async Task<TicketmasterRootEventDto> FindConcertsByArtistAsync(string artistId)
        {
            string url = $"https://app.ticketmaster.com/discovery/v2/events.json?apikey={_ticketmasterKey}&attractionId={artistId}";
           
            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                HttpResponseMessage response = await _httpClient.SendAsync(req);
                response.EnsureSuccessStatusCode();
                string events = await response.Content.ReadAsStringAsync();
                TicketmasterRootEventDto result = JsonSerializer.Deserialize<TicketmasterRootEventDto>(events);
                await Console.Out.WriteLineAsync(events);
                return result;
            }
        }


    }


}
