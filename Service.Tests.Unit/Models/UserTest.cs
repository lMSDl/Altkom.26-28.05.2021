using FluentAssertions;
using Moq;
using Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Service.Tests.Unit.Models
{
    public class UserTest
    {
        [Fact]
        public void Clone_ReturnsClone()
        {
            //Arrange
            var user = new User();

            //Act
            var clone = user.Clone();

            //Assert
            clone.Should().BeOfType<User>()
                .And.NotBeSameAs(user)
                .And.Match(x => typeof(User).GetProperties().ToList().TrueForAll(y => object.Equals(y.GetValue(user), y.GetValue(x))));
        }

        [Fact]
        public void Login_OnSetter_RaisePropertyChangedEvent()
        {
            //Arrange
            var user = new User();
            PropertyChangedEventArgs eventArgs = null;
            user.PropertyChanged += (sender, args) => eventArgs = args;
            var expectedLogin = "login";

            //Act
            user.Login = expectedLogin;
            var login = user.Login;

            //Assert
            eventArgs.Should().NotBeNull();
            eventArgs.PropertyName.Should().Be(nameof(User.Login));
            login.Should().Be(expectedLogin);
        }

        [Fact]
        public void Login_AfterSet_GetKeepsValue()
        {
            //Arrange
            var user = new User();
            var expectedLogin = "login";

            //Act
            user.Login = expectedLogin;
            var login = user.Login;

            //Assert
            login.Should().Be(expectedLogin);
        }

    }
}
