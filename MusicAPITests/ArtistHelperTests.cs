using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace MusicAPITests
{
    [TestClass]
    public class ArtistHelperTests
    {
        [TestMethod]
        public void GetArtists_GetsArtistsFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetArtists")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            ArtistHelper artistHelper = new ArtistHelper(context);

            User user = new User() { Id = 1, Name = "Test_User", Artists = new List<Artist>() };

            Artist artist1 = new Artist() { Id = 1, Name = "Test_Artist1", Description = "Test_Description1", Users = new List<User>() };
            Artist artist2 = new Artist() { Id = 2, Name = "Test_Artist2", Description = "Test_Description2", Users = new List<User>() };

            user.Artists.Add(artist1);
            user.Artists.Add(artist2);

            context.Artists.AddRange(artist1, artist2);
            context.Users.Add(user);

            context.SaveChanges();

            // Act
            var result = artistHelper.GetArtists(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(a => a.Users?.Any(u => u.Id == user.Id) == true));

            // Make sure the database did not save anything
            context.Database.EnsureDeleted(); 
        }

        [TestMethod]
        public void AddSong_CorrectlyAddsSongsToDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddSong")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            ArtistHelper artistHelper = new ArtistHelper(context);

            Artist artist = new Artist() { Id = 1, Name = "Test_Artist", Description = "Test_Description" };
            Genre genre = new Genre() { Id = 1, Title = "Test_Genre", };

            context.Artists.Add(artist);
            context.Genres.Add(genre);

            context.SaveChanges();

            // Act
            artistHelper.AddSong(new SongDto() { Name = "TestSong" }, artist.Id, genre.Id);
            
            // Assert
            Assert.AreEqual(1, context.Songs.Count());
            Assert.AreEqual("TestSong", context.Songs.SingleOrDefault().Name);

            // Make sure the database did not save anything
            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetGenres_GetsGenresFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddSong")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            ArtistHelper artistHelper = new ArtistHelper(context);

            User user = new User() { Id = 1, Name = "Test_User", Genres = new List<Genre>() };

            Genre genre1 = new Genre() { Id = 1, Title = "Test_Title1", Users = new List<User>() };
            Genre genre2 = new Genre() { Id = 2, Title = "Test_Title2", Users = new List<User>() };

            user.Genres.Add(genre1);
            user.Genres.Add(genre2);

            context.Genres.AddRange(genre1, genre2);
            context.Users.Add(user);

            context.SaveChanges();

            // Act
            var result = artistHelper.GetGenres(user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(g => g.Users?.Any(u => u.Id == user.Id) == true));

            // Make sure the database did not save anything
            context.Database.EnsureDeleted();
        }
    }
}
