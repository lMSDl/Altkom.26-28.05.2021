
using ConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CosoleApp.Tests.NUnit
{
    [TestClass]
    public class ExceptionLoggerTest
    {
        public static IEnumerable<object[]> Exceptions =
            new[] { new object[] {new Exception("message")},
                    new object[] {new Exception("abc")}};

        //[DataRow("message")]
        //[DataRow("abc")]
        //public void Log_PassException_LogHistoryContainsSingleElementWithExceptionMessage(string input)
        [DataSource(nameof(Exceptions))]
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
            StringAssert.Contains(input.Message, exceptionLogger.LogHistory.Single().Value);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void Log_PassException_ThrowsOverflowException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            exceptionLogger.Begin();
            string tooLongString = string.Join(null, Enumerable.Repeat(" ", 101).ToArray());
            //var exception = new Exception("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901");
            var exception = new Exception(tooLongString);

            //Act
            exceptionLogger.Log(exception);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Log_CallWithoutBegin_ThrowsNullReferenceException()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();

            var exception = new Exception();

            //Act
             exceptionLogger.Log(exception);
        }

        [TestMethod]
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
            Assert.AreEqual(result, message);
        }

        [TestMethod]
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
            Assert.AreEqual(eventSender, exceptionLogger);
        }

        [TestMethod]
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
            Assert.IsTrue(result);
        }

        [TestMethod]
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
            var exception = Assert.ThrowsException<Exception>(() => exceptionLogger.Begin());
            Assert.AreSame("Already began", exception.Message);
        }

        [TestMethod]
        public void Begin_AddsEmptySingleHistoryLog()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();
            var dateTime = DateTime.UtcNow;

            //Act
            exceptionLogger.Begin();

            //Assert
            Assert.AreEqual(exceptionLogger.LogHistory.Count, 1);
            Assert.IsNull(exceptionLogger.LogHistory.Single().Value);
            Assert.IsTrue(dateTime < exceptionLogger.LogHistory.Single().Key);
            Assert.IsTrue(exceptionLogger.LogHistory.Single().Key < DateTime.UtcNow);
        }

        [TestMethod]
        public void Begin_LogsDateAndTime()
        {
            //Arrange
            var exceptionLogger = new ExceptionLogger();

            //Act
            exceptionLogger.Begin();
            var result = exceptionLogger.End();

            //Assert
            Assert.IsTrue(DateTime.TryParseExact(result, "dd.MM.yyyy HH:mm:", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        [TestMethod]
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
            Assert.IsTrue(task.IsCompleted);
        }

        [TestMethod]
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
            Assert.IsNotNull(exceptionLogger.LogHistory.Single().Value);
        }
    }
}
