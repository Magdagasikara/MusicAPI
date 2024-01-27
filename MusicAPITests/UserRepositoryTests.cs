using Microsoft.EntityFrameworkCore;
using MusicAPI.Handlers;
using MusicAPI.Services;
using MusicAPI.Models;
using MusicAPI.Data;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;

namespace MusicAPITests
{
    [TestClass]
    public class UserRepositoryTests
    {

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsersInDb()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersInDatabase")
                .Options;

            ApplicationContext context = new ApplicationContext(dbContextOptions);
            DbUserRepository userHelper = new DbUserRepository(context);

            var testUser1 = new User { Id = 1, Name = "User1" };
            var testUser2 = new User { Id = 2, Name = "User2" };
            var testUser3 = new User { Id = 3, Name = "User3" };

            context.Users.Add(testUser1);
            context.Users.Add(testUser2);
            context.Users.Add(testUser3);

            context.SaveChanges();

            // Act
            List<User> result = userHelper.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(testUser1 => testUser1.Name == "User1"));
            Assert.IsTrue(result.Any(testUser2 => testUser2.Name == "User2"));
            Assert.IsTrue(result.Any(testUser3 => testUser3.Name == "User3"));
        }

        [TestMethod]
        public void GetUser_ReturnsCorrectUser()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UserInDatabase")
                .Options;

            ApplicationContext context = new ApplicationContext(dbContextOptions);
            DbUserRepository userHelper = new DbUserRepository(context);

            var testUser = new User { Name = "TestUser" };
            context.Users.Add(testUser);
            context.SaveChanges();

            // Act
            var user = userHelper.GetUser(testUser.Name);

            // Assert
            Assert.AreEqual("TestUser", user.Name);
        }

        [TestMethod]
        public void ConnectGenreToUser_ShouldLinkUserWithGenre()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            ApplicationContext context = new ApplicationContext(dbContextOptions);
            DbUserRepository userHelper = new DbUserRepository(context);

            var testUser = new User { Id = 1, Name = "TestUser" };
            var testGenre = new Genre { Id = 1, Title = "TestGenre" };
            context.Users.Add(testUser);
            context.Genres.Add(testGenre);

            context.SaveChanges();

            // Act
            userHelper.ConnectGenreToUser(testUser.Id, testGenre.Id);

            // Assert
            Assert.IsTrue(context.Genres.SingleOrDefault().Id == testUser.Genres.SingleOrDefault().Id);
        }
      
        [TestMethod]
        public void AddUser_AddsNewUserToDb()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddUser_1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.AddUser(new UserDto() { Name = "TestUser" });

            // Assert
            Assert.AreEqual(1, context.Users.Count());
            Assert.AreEqual("TestUser", context.Users.Last().Name);
        }

        [TestMethod]
        public void ConnectSongToUser_AddsSongToUser()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectSongToUser_1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User() { Name = "TestUser" });
            context.Songs.Add(new Song() { Name = "TestSong" });
            context.SaveChanges();
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectSongToUser(1, 1);

            // Assert
            Assert.AreEqual(1, context.Users.SingleOrDefault().Songs.Count());
            Assert.AreEqual("TestUser", context.Users.SingleOrDefault().Name);
            Assert.AreEqual("TestSong", context.Users.SingleOrDefault().Songs.SingleOrDefault().Name);
            Assert.IsTrue(context.Songs.SingleOrDefault().Id == context.Users.SingleOrDefault().Songs.SingleOrDefault().Id);

        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ConnectSongToUser_ThrowsUserNotFoundException()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectSongToUser_2")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectSongToUser(1, 1);


        }

        [TestMethod]
        [ExpectedException(typeof(SongNotFoundException))]
        public void ConnectSongToUser_ThrowsSongNotFoundException()
        {

            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectSongToUser_3")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User { Name = "TestName" });
            context.SaveChanges();
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectSongToUser(1, 1);

        }

        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void ConnectSongToUser_????()
        //{
        //    // ska man testa sista exception ifall det inte gick med .SaveChanges?
        //    // hur kan man testa det, när faller den ut?
        //}

        [TestMethod]
        public void ConnectArtistToUser_AddsArtistToUser()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectArtistToUser_1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User() { Name = "TestUser" });
            context.Artists.Add(new Artist() { Name = "TestArtist", Description = "TestArtistDescription" });
            context.SaveChanges();
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectArtistToUser(1, 1);

            // Assert
            Assert.AreEqual(1, context.Users.SingleOrDefault().Artists.Count());
            Assert.AreEqual("TestUser", context.Users.SingleOrDefault().Name);
            Assert.AreEqual("TestArtist", context.Users.SingleOrDefault().Artists.SingleOrDefault().Name);

        }

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void ConnectArtistToUser_ThrowsUserNotFoundException()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectArtistToUser_2")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectArtistToUser(1, 1);


        }

        [TestMethod]
        [ExpectedException(typeof(ArtistNotFoundException))]
        public void ConnectArtistToUser_ThrowsArtistNotFoundException()
        {

            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectArtistToUser_3")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User { Name = "TestName" });
            context.SaveChanges();
            DbUserRepository userHelper = new DbUserRepository(context);

            // Act
            userHelper.ConnectArtistToUser(1, 1);

        }

        //[TestMethod]
        //[ExpectedException(typeof(Exception))]
        //public void ConnectArtistToUser_????()
        //{
        //    // ska man testa sista exception ifall det inte gick med .SaveChanges?
        //    // hur kan man testa det, när faller den ut?
        //}
    }
}
