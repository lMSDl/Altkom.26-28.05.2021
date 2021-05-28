using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class User : ICloneable, INotifyPropertyChanged
    {
        private string login;

        public virtual int Id { get; set; }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
            }
        }
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual object Clone()
        {
            return MemberwiseClone();
        }
    }
}
