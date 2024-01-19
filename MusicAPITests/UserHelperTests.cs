using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPITests
{
    [TestClass]
    public class UserHelperTests
    {

        [TestMethod]
        public void AddUser_AddsNewUserToDb()
        {
            // Arrange
            DbContextOptions<ApplicationContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            ApplicationContext context = new ApplicationContext(dbContextOptions);
            UserHelper userHelper = new UserHelper(context);

            // Act
            userHelper.AddUser(new UserDto() { Name = "TestUser" });

            // Assert
            Assert.AreEqual(1, context.Users.Count());
            Assert.AreEqual("TestUser", context.Users.Last().Name);
        }


    }
}
