using Microsoft.EntityFrameworkCore;
using MusicAPI.Handlers;
using MusicAPI.Services;
using MusicAPI.Models;
using MusicAPI.Data;

namespace MusicAPITests
{
    [TestClass]
    public class UserHelperTests
    {

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsersInDb()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersInDatabase")
                .Options;

            ApplicationContext context = new ApplicationContext(dbContextOptions);
            UserHelper userHelper = new UserHelper(context);

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
            UserHelper userHelper = new UserHelper(context);

            var testUser = new User { Id = 1, Name = "TestUser" };
            context.Users.Add(testUser);
            context.SaveChanges();

            // Act
            var user = userHelper.GetUser(testUser.Id);

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
            UserHelper userHelper = new UserHelper(context);

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
    }
}
