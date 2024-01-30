namespace MusicAPI.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        //added spotifyId for unique ID
        public string SpotifyId { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
