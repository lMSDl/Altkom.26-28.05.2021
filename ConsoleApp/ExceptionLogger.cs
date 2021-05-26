using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ExceptionLogger
    {
        private StringBuilder stringBuilder;
        public EventHandler<MessageEventArgs> MessageLogged { get; set; }
        public Dictionary<DateTime, string> LogHistory { get; } = new Dictionary<DateTime, string>();

        public void Begin()
        {
            if (stringBuilder != null)
                throw new Exception("Already began");

            var date = DateTime.UtcNow;
            LogHistory[date] = null;
            stringBuilder = new StringBuilder($"{date.ToShortDateString()} {date.ToShortTimeString()}:");
        }

        public string End()
        {
            var result = stringBuilder.ToString();
            stringBuilder = null;
            MessageLogged?.Invoke(this, new MessageEventArgs { Message = result });
            LogHistory[LogHistory.Single(x => x.Value == null).Key] = result;
            return result;
        }

        public Task LogAsync(Exception e)
        {
            return Task.Run(() => Log(e));
        }
        public void Log(Exception e)
        {
            if (e.Message.Length > 100)
            {
                throw new OverflowException();
            }
            AppendException(e);
            LogInner(e.InnerException);
        }

        private void AppendException(Exception e)
        {
            stringBuilder.Append($"{e.GetType().Name}: ");
            stringBuilder.AppendLine(e.Message);
        }

        private void LogInner(Exception innerException, int counter = 1)
        {
            if (innerException == null)
                return;
            for (var i = 0; i < counter; i++)
                stringBuilder.Append("\t");
            AppendException(innerException);

            LogInner(innerException.InnerException, ++counter);
        }


        public Task<string> LogAsync(AggregateException aggregateException)
        {
            return Task.Run(() => Log(aggregateException));
        }
        public string Log(AggregateException aggregateException)
        {
            if (aggregateException == null)
                throw new ArgumentNullException(nameof(aggregateException));
            if (aggregateException.InnerExceptions.Count == 0)
                throw new ArgumentException("No inner exceptions", nameof(aggregateException));

            Begin();
            foreach(var e in aggregateException.InnerExceptions)
            {
                Log(e);
            }
            return End();
        }

        public class MessageEventArgs : EventArgs
        {
            public string Message { get; set; }
        }
    }
}
