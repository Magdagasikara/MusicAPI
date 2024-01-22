using Microsoft.EntityFrameworkCore;
using MusicAPI.Handlers;
using MusicAPI.Services;
using MusicAPI.Models;
using MusicAPI.Data;

namespace MusicAPITests
{
    [TestClass]
    internal class UserHelperTests
    {

        [TestMethod]
        public void GetAllUsers_ReturnsAllUsersInDb()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersInDatabase")
                .Options;

            using (var context = new ApplicationContext(dbContextOptions))
            {
                context.Users.Add(new User { Id = 1, Name = "User1" });
                context.Users.Add(new User { Id = 2, Name = "User2" });
                context.Users.Add(new User { Id = 3, Name = "User3" });
            }

            using (var context = new ApplicationContext(dbContextOptions))
            {
                var userHelper = new UserHelper(context);

                // Act
                List<User> result = userHelper.GetAllUsers();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.IsTrue(result.Any(user => user.Name == "User1"));
                Assert.IsTrue(result.Any(user => user.Name == "User2"));
                Assert.IsTrue(result.Any(user => user.Name == "User3"));
            }
        }

        [TestMethod]
        public void GetUser_ReturnsCorrectUser()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UserInDatabase")
                .Options;

            using (var context = new ApplicationContext(dbContextOptions))
            {
                var testUser = new User { Id = 1, Name = "TestUser" };
                context.Users.Add(testUser);
                context.SaveChanges();
            }

            using (var context = new ApplicationContext(dbContextOptions))
            {
                var userRepository = new UserHelper(context);

                // Act
                int userId = 1; 
                User result = userRepository.GetUser(userId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(userId, result.Id); 
                Assert.AreEqual("TestUser", result.Name);
            }
        }
    }
}
