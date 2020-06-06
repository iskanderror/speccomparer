using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Comparator.Extensions
{
    public class NotifiedEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return;
            }
            field = value;
            OnPropertyChanged(propertyName);
        }
    }
}
