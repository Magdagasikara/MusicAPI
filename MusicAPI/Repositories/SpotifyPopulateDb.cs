using MusicAPI.Data;

namespace MusicAPI.Repositories
{
    public class SpotifyPopulateDb
    {
        private ApplicationContext _context;
        public DbArtistRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}
