using System.Data;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;

namespace MusicAPI.Data
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres{ get; set; }
        public DbSet<Song> Songs{ get; set; }
        public DbSet<User> Users{ get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public ApplicationContext()
        {
        }
    }
}
