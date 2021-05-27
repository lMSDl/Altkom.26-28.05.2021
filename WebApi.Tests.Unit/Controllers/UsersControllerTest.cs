using Microsoft.AspNetCore.Mvc;
using Moq;
using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests.Unit.Controllers
{
    public class UsersControllerTest
    {

        [Fact]
        public void Get_ReturnsAllUsers()
        {
            //Arrage
            var userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            //var counter = 0;
            userServiceMock.Setup(x => x.Read()).Returns(new List<User>());//.Verifiable()//.Callback(() => counter++);

            var controller = new UsersController(userServiceMock.Object);

            //Act
            var result = controller.Get();

            //Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(okObjectResult.Value);
            userServiceMock.Verify(x => x.Read(), Times.Once);
            //userServiceMock.VerifyAll();
            //userServiceMock.Verify();
            //Assert.True(counter > 0);
        }

        [Fact]
        public void Get_PassValidId_ReturnsUser()
        {
            //Arrage
            Mock<IUserService> userServiceMock = GetUserServiceMock();

            var controller = new UsersController(userServiceMock.Object);

            //Act
            var result = controller.Get(1);

            //Assert
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.IsAssignableFrom<User>(okObjectResult.Value);
        }

        [Fact]
        public void Get_PassInvalidId_ReturnsNotFoundResult()
        {
            //Arrage
            Mock<IUserService> userServiceMock = GetUserServiceMock();

            var controller = new UsersController(userServiceMock.Object);

            //Act
            var result = controller.Get(0);

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        private static Mock<IUserService> GetUserServiceMock()
        {
            var userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            userServiceMock.Setup(x => x.Read(It.Is<int>(x => x > 0))).Returns(Mock.Of<User>());
            userServiceMock.Setup(x => x.Read(It.Is<int>(x => x <= 0))).Returns(default(User)/*It.IsAny<User>()*/);
            return userServiceMock;
        }
    }
}
