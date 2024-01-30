namespace MusicAPI.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        //added spotifyId for unique ID
        public string SpotifyId { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Artist> Artist { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
