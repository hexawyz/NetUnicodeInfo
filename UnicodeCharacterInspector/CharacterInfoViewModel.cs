using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicodeCharacterInspector
{
	internal sealed class CharacterInfoViewModel : BindableObject
	{
		private string character;
		private int codePoint;
		private UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory('\0');

		public CharacterInfoViewModel()
		{
		}

		public string Character
		{
			get { return character; }
			set
			{
				if (value != null)
				{
					if (value.Length == 0)
					{
						value = null;
					}
					else if (value.Length > 2 || value.Length == 2 && !char.IsSurrogatePair(value, 0))
					{
						throw new ArgumentException();
					}
				}

				if (value != character)
				{
					if ((character = value) != null)
					{
						codePoint = char.ConvertToUtf32(character, 0);
						category = CharUnicodeInfo.GetUnicodeCategory(character, 0);
					}
					else
					{
						codePoint = 0;
						category = CharUnicodeInfo.GetUnicodeCategory('\0');
                    }

					NotifyPropertyChanged();
					NotifyPropertyChanged("CodePoint");
					NotifyPropertyChanged("Category");
				}
			}
		}

		public int? CodePoint
		{
			get { return character != null ? codePoint : null as int?; }
		}

		public UnicodeCategory? Category
		{
			get { return character != null ? category : null as UnicodeCategory?; }
		}
	}
}
