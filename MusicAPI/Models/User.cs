namespace MusicAPI.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Artist> Artists { get; set;}
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
