using ConsoleApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CosoleApp.Tests.NUnit
{
    [TestFixture]
    public class ExceptionLoggerTest
    {
        public static IEnumerable<object[]> Exceptions =
            new[] { new object[] {new Exception("message")},
                    new object[] {new Exception("abc")}};

        [Theory]
        //[TestCase("message")]
        //[TestCase("message")]
        //public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage(string input)
        //[TestCaseSource(nameof(Exceptions))]
        //public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage(Exception input)
        public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage([Values("abc", "message")]string input)
        //[Range(...)] [Random(...)]
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            var exception = new Exception(input);

            //Act
            exceptionLogger.Log(exception);
            exceptionLogger.End();

            //Assert
            StringAssert.Contains(input, exceptionLogger.LogHistory.Single().Value);
        }

        [Test]
        public void Log_PassException_ThrowsOverflowException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            string tooLongString = string.Join(null, Enumerable.Repeat(" ", 101).ToArray());
            //var exception = new Exception("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
            var exception = new Exception(tooLongString);

            //Act&Assert
            //Assert.Throws<OverflowException>(() => exceptionLogger.Log(exception));
            Assert.That(() => exceptionLogger.Log(exception), Throws.TypeOf<OverflowException>());
        }

        [Test]
        public void Log_CallWithoutBegin_ThrowsNullReferenceException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();

            var exception = new Exception();

            //Act&Assert
            Assert.Throws<NullReferenceException>(() => exceptionLogger.Log(exception));
        }

        [Test]
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
            Assert.That(result, Is.EqualTo(message));
        }

        [Test]
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
            Assert.That(eventSender, Is.EqualTo(exceptionLogger));
        }

        [Test]
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
            Assert.True(result);
        }

        [Test]
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
            var exception = Assert.Throws<Exception>(() => exceptionLogger.Begin());
            StringAssert.AreEqualIgnoringCase("already began", exception.Message);
        }

        [Test]
        public void Begin_AddsEmptySingleHistoryLog()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            var dateTime = DateTime.UtcNow;

            //Act
            exceptionLogger.Begin();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(exceptionLogger.LogHistory, Has.Count.EqualTo(1));
                Assert.That(exceptionLogger.LogHistory.Single().Value, Is.Null);
                Assert.That(exceptionLogger.LogHistory.Single().Key, Is.InRange(dateTime, DateTime.UtcNow));
            });
        }

        [Test]
        public void Begin_LogsDateAndTime()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();

            //Act
            exceptionLogger.Begin();
            var result = exceptionLogger.End();

            //Assert
            Assert.True(DateTime.TryParseExact(result, "dd.MM.yyyy HH:mm:", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        [Test]
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
            Assert.True(task.IsCompleted);
        }

        [Test]
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
            Assert.NotNull(exceptionLogger.LogHistory.Single().Value);
        }
    }
}
