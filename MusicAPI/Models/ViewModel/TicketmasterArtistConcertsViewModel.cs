namespace MusicAPI.Models.ViewModel
{
    public class TicketmasterArtistConcertsViewModel
    {
        public string ArtistName { get; set; }
        public List<TicketmasterConcertViewModel> Concerts { get; set; }

    }

    public class TicketmasterConcertViewModel
    {
        public string Date { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
    }
}
