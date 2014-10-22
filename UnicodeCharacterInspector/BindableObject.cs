using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeCharacterInspector
{
	internal abstract class BindableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetValue<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(value, storage))
			{
				storage = value;
				NotifyPropertyChanged(propertyName);
				return true;
			}
			return false;
		}

		protected bool SetValue<T>(ref T storage, T value, IEqualityComparer<T> equalityComparer, [CallerMemberName] string propertyName = null)
		{
			if (!(equalityComparer ?? EqualityComparer<T>.Default).Equals(value, storage))
			{
				storage = value;
				NotifyPropertyChanged(propertyName);
				return true;
            }
			return false;
		}
	}
}
