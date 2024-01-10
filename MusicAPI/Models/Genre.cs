namespace MusicAPI.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
