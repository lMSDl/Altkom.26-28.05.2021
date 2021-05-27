using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleApp.Tests.Unit
{
    public class ExceptionLoggerTest 
    {
        [Fact]
        //public void Log_ValidAggregateException_LogMessage()
        public void Log_PassValidAggregateException_ReturnLogMessage()
        {
            //TODO
        }

        [Fact]
        public void Log_PassNullAggregateException_ThrowsArgumentNullException()
        {
            //TODO
        }

        [Fact]
        public void Log_PassEmptyAggregateException_ThrowsArgumentException()
        {
            //TODO
        }

        public static IEnumerable<object[]> Exceptions = 
            new[] { new object[] {new Exception("message")},
                    new object[] {new Exception("abc")}};

        [Theory]
        //[InlineData("message")]
        //[InlineData("message")]
        //public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage(string input)
        [MemberData(nameof(Exceptions))]
        public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage(Exception input)
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            //var exception = new Exception(input);

            //Act
            exceptionLogger.Log(input);
            exceptionLogger.End();

            //Assert
            exceptionLogger.LogHistory.Single().Value.Should().Contain(input.Message);
            //Assert.Contains(input.Message, exceptionLogger.LogHistory.Single().Value);
        }

        [Fact]
        public void Log_PassException_ThrowsOverflowException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            string tooLongString = string.Join(null,Enumerable.Repeat(" ", 101).ToArray());
            //var exception = new Exception("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
            var exception = new Exception(tooLongString);

            //Act&Assert
            exceptionLogger.Invoking(x => x.Log(exception)).Should().ThrowExactly<OverflowException>();
            //Assert.Throws<OverflowException>(() => exceptionLogger.Log(exception));
        }

        [Fact]
        public void Log_CallWithoutBegin_ThrowsNullReferenceException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
           
            var exception = new Exception();

            //Act&Assert
            exceptionLogger.Invoking(x => x.Log(exception)).Should().ThrowExactly<NullReferenceException>();
            //Assert.Throws<NullReferenceException>(() => exceptionLogger.Log(exception));
        }

        [Fact]
        public void End_ReturnsLoggedMessage()
        {
            //TODO
        }

        [Fact]
        public void End_MessageLoggedEvent_MessageEqualResult()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            string message = null;
            exceptionLogger.MessageLogged += (sender, args) => message = args.Message;
            //Act
            var result = exceptionLogger.End();

            //Assert
            result.Should().Be(message);
            //Assert.Equal(result, message);
        }

        [Fact]
        public void End_MessageLoggedEvent_SenderIsExceptionLogger()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            ExceptionLogger eventSender = null;
            exceptionLogger.MessageLogged += (sender, args) => { eventSender = sender as ExceptionLogger; };
            //Act
            var result = exceptionLogger.End();

            //Assert
            eventSender.Should().NotBeNull().And.Be(exceptionLogger);
            //Assert.Equal(eventSender, exceptionLogger);
        }

        [Fact]
        public void End_WasMessageLoggedEventInvoked()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            bool result = false;
            exceptionLogger.MessageLogged += (sender, args) => result = true;
            //Act
            exceptionLogger.End();

            //Assert
            result.Should().BeTrue();
            //Assert.True(result);
        }

        [Fact]
        public void Begin_InvokedTwice_ThrowsException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();

            //Act
            //Action action = () => exceptionLogger.Begin();

            //Assert
            //Assert.Throws<Exception>(action);

            //Act&Assert
            exceptionLogger.Invoking(x => x.Begin()).Should().Throw<Exception>().WithMessage("Already began");
            //var exception = Assert.Throws<Exception>(() => exceptionLogger.Begin());
            //Assert.Equal("Already began", exception.Message);
        }

        [Fact]
        public void Begin_AddsEmptySingleHistoryLog()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            var dateTime = DateTime.UtcNow;

            //Act
            exceptionLogger.Begin();

            //Assert    

            exceptionLogger.LogHistory.Should().HaveCount(1).And.ContainValue(null);
            exceptionLogger.LogHistory.Single().Key.Should().BeOnOrAfter(dateTime).And.BeOnOrBefore(DateTime.UtcNow);

            //Assert.Single(exceptionLogger.LogHistory);
            //Assert.Null(exceptionLogger.LogHistory.Single().Value);
            //Assert.Equal(dateTime, exceptionLogger.LogHistory.Single().Key, TimeSpan.FromMinutes(1));
            //Assert.InRange(exceptionLogger.LogHistory.Single().Key, dateTime, DateTime.UtcNow);
        }

        [Fact]
        public void Begin_LogsDateAndTime()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            
            //Act
            exceptionLogger.Begin();
            var result = exceptionLogger.End();

            //Assert
            DateTime.TryParseExact(result, "dd.MM.yyyy HH:mm:", CultureInfo.InvariantCulture, DateTimeStyles.None, out _).Should().BeTrue();
           // Assert.True(DateTime.TryParseExact(result, "dd.MM.yyyy HH:mm:", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        [Fact]
        public async Task LoadAsync_TaskCreated()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            var exception = new Exception();

            //Act
            var task = exceptionLogger.LogAsync(exception);
            await task;

            //Asert
            task.IsCompleted.Should().BeTrue();
            //Assert.True(task.IsCompleted);
        }

        [Fact]
        public async Task LoadAsync_WasLogCalled()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            var exception = new Exception();

            //Act
            await exceptionLogger.LogAsync(exception);
            exceptionLogger.End();

            //Asert
            exceptionLogger.LogHistory.Should().HaveCount(1).And.NotContainValue(null);
            //Assert.NotNull(exceptionLogger.LogHistory.Single().Value);
        }
    }
}
