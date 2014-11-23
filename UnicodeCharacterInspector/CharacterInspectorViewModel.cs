using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Unicode;

namespace UnicodeCharacterInspector
{
	internal sealed class CharacterInspectorViewModel : BindableObject
	{
		public sealed class CharacterViewModel
		{
			public int CodePoint { get; }
			public string Character { get; }
			public string DisplayText { get; }

			public CharacterViewModel(int codePoint)
			{
				CodePoint = codePoint;
				Character = char.ConvertFromUtf32(codePoint);
				DisplayText = UnicodeInfo.GetDisplayText(codePoint);
			}
		}

		private class CharacterCollection : INotifyCollectionChanged, IList<CharacterViewModel>
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

			public CharacterViewModel this[int index]
			{
				get
				{
					if (index < 0 || index >= owner.characterCount) throw new IndexOutOfRangeException();

					return new CharacterViewModel(owner.codePoints[index]);
				}
			}

			CharacterViewModel IList<CharacterViewModel>.this[int index]
			{
				get { return this[index]; }
				set { throw new NotSupportedException(); }
			}

			public int Count { get { return owner.characterCount; } }

			bool ICollection<CharacterViewModel>.IsReadOnly { get { return true; } }

			void ICollection<CharacterViewModel>.Add(CharacterViewModel item) { throw new NotSupportedException(); }
			void IList<CharacterViewModel>.Insert(int index, CharacterViewModel item) { throw new NotSupportedException(); }

			bool ICollection<CharacterViewModel>.Remove(CharacterViewModel item) { throw new NotSupportedException(); }
			void IList<CharacterViewModel>.RemoveAt(int index) { throw new NotSupportedException(); }

			void ICollection<CharacterViewModel>.Clear() { throw new NotSupportedException(); }

			public int IndexOf(CharacterViewModel item) { return Array.IndexOf(owner.codePoints, item); }
			public bool Contains(CharacterViewModel item) { return Array.IndexOf(owner.codePoints, item) >= 0; }

			public void CopyTo(CharacterViewModel[] array, int arrayIndex)
			{
				if (array == null) throw new ArgumentNullException(nameof(array));
				if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length < arrayIndex + owner.codePoints.Length) throw new ArgumentException();

				for (int i = 0, j = arrayIndex; i < owner.codePoints.Length; ++i, ++j)
				{
					array[j] = new CharacterViewModel(owner.codePoints[i]);
				}
			}

			public IEnumerator<CharacterViewModel> GetEnumerator()
			{
				int length = owner.characterCount;
				var array = owner.codePoints;

				for (int i = 0; i < length; ++i)
					yield return new CharacterViewModel(array[i]);
			}

			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		}

		private event NotifyCollectionChangedEventHandler CollectionChanged;

		private string text;
		private int selectedCharacterIndex = -1;
		private int[] codePoints = new int[1024];
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
					foreach (int codePoint in value.AsCodePointEnumerable())
					{
						if (index >= codePoints.Length) Array.Resize(ref codePoints, codePoints.Length * 2);

						codePoints[index++] = codePoint;
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
					NotifyPropertyChanged(nameof(SelectedCharacterIndex));
					NotifyPropertyChanged(nameof(SelectedCharacter));
					selectedCharacterInfo.Character = null;
				}
			}
		}

		public ICollection<CharacterViewModel> Characters { get { return characterCollection; } }

		public int SelectedCharacterIndex
		{
			get { return selectedCharacterIndex; }
			set
			{
				if (selectedCharacterIndex < -1 || selectedCharacterIndex >= codePoints.Length)
					throw new ArgumentOutOfRangeException(nameof(value));

				if (value != selectedCharacterIndex)
				{
					string oldSelectedCharacter = SelectedCharacter;

					selectedCharacterIndex = value;

					string newSelectedCharacter = SelectedCharacter;

					NotifyPropertyChanged();
					if (newSelectedCharacter != oldSelectedCharacter)
					{
						selectedCharacterInfo.Character = newSelectedCharacter;
						NotifyPropertyChanged(nameof(SelectedCharacter));
					}
				}
			}
		}

		public string SelectedCharacter
		{
			get { return selectedCharacterIndex >= 0 ? char.ConvertFromUtf32(codePoints[selectedCharacterIndex]) : null; }
		}

		public CharacterInfoViewModel SelectedCharacterInfo
		{
			get { return selectedCharacterInfo; }
		}
	}
}
