using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeCharacterInspector
{
	internal sealed class CharacterInspectorViewModel : BindableObject
	{
		private class CharacterCollection : INotifyCollectionChanged, IList<string>
		{
			public event NotifyCollectionChangedEventHandler CollectionChanged
			{
				add { owner.CollectionChanged += value; }
				remove { owner.CollectionChanged -= value; }
			}

			private CharacterInspectorViewModel owner;

			internal CharacterCollection(CharacterInspectorViewModel owner)
			{
				this.owner = owner;
			}

			public string this[int index]
			{
				get
				{
					if (index < 0 || index >= owner.characterCount) throw new IndexOutOfRangeException();

					return owner.characters[index];
				}
			}

			string IList<string>.this[int index]
			{
				get { return this[index]; }
				set { throw new NotSupportedException(); }
			}

			public int Count { get { return owner.characterCount; } }

			bool ICollection<string>.IsReadOnly { get { return true; } }

			void ICollection<string>.Add(string item) { throw new NotSupportedException(); }
			void IList<string>.Insert(int index, string item) { throw new NotSupportedException(); }

			bool ICollection<string>.Remove(string item) { throw new NotSupportedException(); }
			void IList<string>.RemoveAt(int index) { throw new NotSupportedException(); }

			void ICollection<string>.Clear() { throw new NotSupportedException(); }

			public int IndexOf(string item) { return Array.IndexOf(owner.characters, item); }
			public bool Contains(string item) { return Array.IndexOf(owner.characters, item) >= 0; }

			public void CopyTo(string[] array, int arrayIndex) { owner.characters.CopyTo(array, arrayIndex); }

			public IEnumerator<string> GetEnumerator()
			{
				int length = owner.characterCount;
				var array = owner.characters;

				for (int i = 0; i < length; ++i)
					yield return array[i];
			}

			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}

		private event NotifyCollectionChangedEventHandler CollectionChanged;

		private string text;
		private int selectedCharacterIndex = -1;
		private string[] characters = new string[1024];
		private int characterCount;
		private CharacterInfoViewModel selectedCharacterInfo = new CharacterInfoViewModel();
		private readonly CharacterCollection characterCollection;

		public CharacterInspectorViewModel()
		{
			characterCollection = new CharacterCollection(this);
        }

		private void NotifyCollectionChanged(NotifyCollectionChangedAction action)
		{
			var handler = CollectionChanged;

			if (handler != null)
				handler(this, new NotifyCollectionChangedEventArgs(action));
		}

		public string Text
		{
			get { return text; }
			set
			{
				// Analyze the string to detect if there are any similarities between the new and old values.
				bool keepSelectedIndex = value != null
					&& text != null
					&&
					(
						value.Length <= text.Length ?
							text.StartsWith(value, StringComparison.Ordinal) :
							value.StartsWith(text, StringComparison.Ordinal)
					);

				// Here, we can deduce the information on string equality and simply return if needed.
				if (keepSelectedIndex && value.Length == text.Length) return;

				// We don't need the old value anymore.
				text = value;

				// We always need to reparse the text, as it is not possible to directly map between UTF-16 elements and their respective code point indexes.
				int index = 0;
				if (!string.IsNullOrEmpty(value))
				{
					var enumerator = StringInfo.GetTextElementEnumerator(value);

					while (enumerator.MoveNext())
					{
						if (index >= characters.Length) Array.Resize(ref characters, characters.Length * 2);

						// We don't replace pre-existing elements if they already have the correct value.
						// This should help a bit with GC, by making new string elements as short-lived as possible.
						string element = enumerator.GetTextElement();
						if (characters[index] != element) characters[index] = element;

						++index;
					}
				}
				characterCount = index;

				// Now that we parsed the text, we really know whether the previous selected index should be left untouched.
				if (!(keepSelectedIndex = keepSelectedIndex && selectedCharacterIndex < characterCount || selectedCharacterIndex == -1)) selectedCharacterIndex = -1;

				// After all this, raise the change notifications as needed. 😊
				NotifyPropertyChanged();
				NotifyCollectionChanged(NotifyCollectionChangedAction.Reset);
				if (!keepSelectedIndex)
				{
					NotifyPropertyChanged("SelectedCharacterIndex");
					NotifyPropertyChanged("SelectedCharacter");
					selectedCharacterInfo.Character = null;
				}
            }
		}

		public ICollection<string> Characters { get { return characterCollection; } }

		public int SelectedCharacterIndex
		{
			get { return selectedCharacterIndex; }
			set
			{
				if (selectedCharacterIndex < -1 || selectedCharacterIndex >= characters.Length)
					throw new ArgumentOutOfRangeException("value");

				if (value != selectedCharacterIndex)
				{
					string oldSelectedCharacter = SelectedCharacter;

					selectedCharacterIndex = value;

					string newSelectedCharacter = SelectedCharacter;

					NotifyPropertyChanged();
					if (newSelectedCharacter != oldSelectedCharacter) NotifyPropertyChanged("SelectedCharacter");
					selectedCharacterInfo.Character = newSelectedCharacter;
				}
			}
		}

		public string SelectedCharacter
		{
			get { return selectedCharacterIndex >= 0 ? characters[selectedCharacterIndex] : null; }
		}

		public CharacterInfoViewModel SelectedCharacterInfo
		{
			get { return selectedCharacterInfo; }
		}
	}
}
