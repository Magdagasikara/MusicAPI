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
using MusicAPI.Repositories;

namespace MusicAPITests
{
    [TestClass]
    public class ArtistRepositoryTests
    {
        [TestMethod]
        public void AddSong_CorrectlyAddsSongsToDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddSong")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

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
        }

        [TestMethod]
        public void AddGenre_CorrectlyAddsGenreToDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddGenres")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            GenreDto genre = new GenreDto() { Title = "Test_Genre" };

            // Act
            artistHelper.AddGenre(genre);

            // Assert
            Assert.AreEqual(1, context.Genres.Count());
            Assert.IsTrue(context.Genres.Any(g => g.Title == genre.Title));
        }

        [TestMethod]
        public void AddArtist_CorrectlyAddsArtistsToDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddArtist")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            ArtistDto artist = new ArtistDto() { Name = "Test_Artist", Description = "Test_Description" };

            // Act
            artistHelper.AddArtist(artist);

            // Assert
            Assert.AreEqual(1, context.Artists.Count());
            Assert.IsTrue(context.Artists.Any(a => a.Name == artist.Name));
        }

        [TestMethod]
        public void GetArtistsForUser_GetsArtistsFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetArtistsForUser")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            User user = new User() { Id = 1, Name = "Test_User", Artists = new List<Artist>() };

            Artist artist1 = new Artist() { Id = 1, Name = "Test_Artist1", Description = "Test_Description1", Users = new List<User>() };
            Artist artist2 = new Artist() { Id = 2, Name = "Test_Artist2", Description = "Test_Description2", Users = new List<User>() };

            user.Artists.Add(artist1);
            user.Artists.Add(artist2);

            context.Artists.AddRange(artist1, artist2);
            context.Users.Add(user);

            context.SaveChanges();

            // Act
            var result = artistHelper.GetArtistsForUser(user.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Name == artist1.Name));
            Assert.IsTrue(result.Any(a => a.Name == artist2.Name));
        }

        [TestMethod]
        public void GetGenresForUser_GetsGenresFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetGenresForUser")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            User user = new User() { Id = 1, Name = "Test_User", Genres = new List<Genre>() };

            Genre genre1 = new Genre() { Id = 1, Title = "Test_Title1", Users = new List<User>() };
            Genre genre2 = new Genre() { Id = 2, Title = "Test_Title2", Users = new List<User>() };

            user.Genres.Add(genre1);
            user.Genres.Add(genre2);

            context.Genres.AddRange(genre1, genre2);
            context.Users.Add(user);

            context.SaveChanges();

            // Act
            var result = artistHelper.GetGenresForUser(user.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(g => g.Title == genre1.Title));
            Assert.IsTrue(result.Any(g => g.Title == genre2.Title));
        }

        [TestMethod]
        public void GetSongsForUser_GetsSongsFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetSongs1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            User user = new User() { Name = "Test_User", Songs = new List<Song>() };
            Artist artist = new Artist() { Name = "Test_Artist", Description = "Test_ArtistDescription" };
            Genre genre = new Genre() { Title = "Test_Genre" };
            Song song1 = new Song() { Name = "Test_Song1", Artist = artist, Genre = genre };
            Song song2 = new Song() { Name = "Test_Song2", Artist = artist, Genre = genre };

            context.Users.Add(user);
            user.Songs.Add(song1);
            user.Songs.Add(song2);

            context.SaveChanges();

            // Act
            var result = artistHelper.GetSongsForUser(user.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(s => s.Name == song1.Name));
            Assert.IsTrue(result.Any(s => s.Name == song2.Name));
        }

        [TestMethod]
        public void GetArtists_GetsArtistsFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetArtists")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Artist artist1 = new Artist() { Name = "Test_Artist1", Description = "Test_Description1" };
            Artist artist2 = new Artist() { Name = "Test_Artist2", Description = "Test_Description2" };

            context.Artists.AddRange(artist1, artist2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetArtists("", "", "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Name == artist1.Name));
            Assert.IsTrue(result.Any(a => a.Name == artist2.Name));
            Assert.IsTrue(result.Any(a => a.Description == artist1.Description));
            Assert.IsTrue(result.Any(a => a.Description == artist2.Description));
        }
        [TestMethod]
        public void GetArtists_GetsArtistsFromDbWithSearchQuery()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetArtistsWithSearchQuery")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Artist artist1 = new Artist() { Name = "Test_Artist1", Description = "Test_Description1" };
            Artist artist2 = new Artist() { Name = "Test_Artist2", Description = "Test_Description2" };

            context.Artists.AddRange(artist1, artist2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetArtists("T", "2", "1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Name == artist1.Name));
            Assert.IsTrue(result.Any(a => a.Name == artist2.Name));
            Assert.IsTrue(result.Any(a => a.Description == artist1.Description));
            Assert.IsTrue(result.Any(a => a.Description == artist2.Description));
        }

        [TestMethod]
        public void GetGenres_GetsGenresFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetGenres")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Genre genre1 = new Genre() { Title = "Test_genre1" };
            Genre genre2 = new Genre() { Title = "Test_genre2" };

            context.Genres.AddRange(genre1, genre2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetGenres("", "", "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Title == genre1.Title));
            Assert.IsTrue(result.Any(a => a.Title == genre2.Title));

        }

        [TestMethod]
        public void GetGenres_GetsGenresFromDbWithSearchQuery()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetGenresWithSearchQuery")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Genre genre1 = new Genre() { Title = "Test_genre1" };
            Genre genre2 = new Genre() { Title = "Test_genre2" };

            context.Genres.AddRange(genre1, genre2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetGenres("T", "2", "1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Title == genre1.Title));
            Assert.IsTrue(result.Any(a => a.Title == genre2.Title));

        }

        [TestMethod]
        public void GetSongs_GetsSongsFromDb()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetSongs")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Song song1 = new Song()
            {
                Name = "Test_song1",
                Artist = new Artist { Name = "TestArtist1", Description = "TestDesc1" },
                Genre = new Genre { Title = "TestGenre1" }
            };
            Song song2 = new Song()
            {
                Name = "Test_song2",
                Artist = new Artist { Name = "TestArtist2", Description = "TestDesc2" },
                Genre = new Genre { Title = "TestGenre2" }
            };

            context.Songs.AddRange(song1, song2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetSongs("", "", "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Name == song1.Name));
            Assert.IsTrue(result.Any(a => a.Name == song2.Name));

        }
        [TestMethod]
        public void GetSongs_GetsSongsFromDbWithSearchQuery()
        {
            // Arrange 
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_GetSongsWithSearchQuery")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbArtistRepository artistHelper = new DbArtistRepository(context);

            Song song1 = new Song()
            {
                Name = "Test_song1",
                Artist = new Artist { Name = "TestArtist1", Description = "TestDesc1" },
                Genre = new Genre { Title = "TestGenre1" }
            };
            Song song2 = new Song()
            {
                Name = "Test_song2",
                Artist = new Artist { Name = "TestArtist2", Description = "TestDesc2" },
                Genre = new Genre { Title = "TestGenre2" }
            };

            context.Songs.AddRange(song1, song2);
            context.SaveChanges();

            // Act
            var result = artistHelper.GetSongs("T", "2", "1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(a => a.Name == song1.Name));
            Assert.IsTrue(result.Any(a => a.Name == song2.Name));

        }
        
    }
}
