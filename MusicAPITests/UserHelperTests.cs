using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;

namespace MusicAPITests
{
    [TestClass]
    public class UserHelperTests
    {

        [TestMethod]
        public void AddUser_AddsNewUserToDb()
        {
            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_AddUser_1")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            UserHelper userHelper = new UserHelper(context);

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
            UserHelper userHelper = new UserHelper(context);

            // Act
            userHelper.ConnectSongToUser(1, 1);

            // Assert
            Assert.AreEqual(1, context.Users.SingleOrDefault().Songs.Count());
            Assert.AreEqual("TestUser", context.Users.SingleOrDefault().Name);
            Assert.AreEqual("TestSong", context.Users.SingleOrDefault().Songs.SingleOrDefault().Name);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectSongToUser_ThrowsNullExceptionWhenNoSuchUser()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectSongToUser_2")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            UserHelper userHelper = new UserHelper(context);

            // Act
            userHelper.ConnectSongToUser(1, 1);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectSongToUser_ThrowsNullExceptionWhenNoSuchSong()
        {

            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectSongToUser_3")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User { Name = "TestName" });
            context.SaveChanges();
            UserHelper userHelper = new UserHelper(context);

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
            UserHelper userHelper = new UserHelper(context);

            // Act
            userHelper.ConnectArtistToUser(1, 1);

            // Assert
            Assert.AreEqual(1, context.Users.SingleOrDefault().Artists.Count());
            Assert.AreEqual("TestUser", context.Users.SingleOrDefault().Name);
            Assert.AreEqual("TestArtist", context.Users.SingleOrDefault().Artists.SingleOrDefault().Name);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectArtistToUser_ThrowsNullExceptionWhenNoSuchUser()
        {
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectArtistToUser_2")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            UserHelper userHelper = new UserHelper(context);

            // Act
            userHelper.ConnectArtistToUser(1, 1);


        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConnectArtistToUser_ThrowsNullExceptionWhenNoSuchArtist()
        {

            // Arrange
            DbContextOptions<ApplicationContext> options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb_ConnectArtistToUser_3")
                .Options;
            ApplicationContext context = new ApplicationContext(options);
            context.Users.Add(new User { Name = "TestName" });
            context.SaveChanges();
            UserHelper userHelper = new UserHelper(context);

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
