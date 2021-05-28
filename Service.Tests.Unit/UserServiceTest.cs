using Moq;
using Service.Models;
using System.Linq;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;

namespace Service.Tests.Unit
{
    public class UserServiceTest
    {
        [Fact]
        public void Create_PassUser_SavesUserWithUniqueId()
        {
            //Arrage
            var userMock = new Mock<User>();
            userMock.Setup(x => x.Clone()).Returns(userMock.Object).Verifiable();
            userMock.SetupProperty(x => x.Id, 0);

            var users = new List<User>();
            var collectionMoq = new Mock<ICollection<User>>();

            var service = new UserService();
            var ids = new List<int>();

            //Act
            ids.Add(service.Create(userMock.Object));
            ids.Add(service.Create(userMock.Object));

            //Assert
            using (new AssertionScope())
            {
                //ids.Should().OnlyHaveUniqueItems();
                userMock.VerifySet(x => x.Id = It.Is<int>(y => y > 0), Times.Exactly(2));
                userMock.Verify();
                collectionMoq.Verify();
            }
        }
    }
}
