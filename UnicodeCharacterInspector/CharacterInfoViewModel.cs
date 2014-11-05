using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Unicode;

namespace UnicodeCharacterInspector
{
	internal sealed class CharacterInfoViewModel : BindableObject
	{
		private string character;
		private int codePoint;
		private UnicodeCharacterData characterData = UnicodeData.Default.Get(0);

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
						characterData = UnicodeData.Default.Get(codePoint);
					}
					else
					{
						codePoint = 0;
						characterData = UnicodeData.Default.Get(0);
                    }

					NotifyPropertyChanged();
					NotifyPropertyChanged("CodePoint");
					NotifyPropertyChanged("Name");
					NotifyPropertyChanged("OldName");
					NotifyPropertyChanged("Category");
					NotifyPropertyChanged("ContributoryProperties");
				}
			}
		}

		public int? CodePoint
		{
			get { return character != null ? codePoint : null as int?; }
		}

		public string Name
		{
			get { return character != null && characterData != null ? characterData.Name : null; }
		}

		public string OldName
		{
			get { return character != null && characterData != null ? characterData.OldName : null; }
		}

		public UnicodeCategory? Category
		{
			get { return character != null ? characterData != null ? characterData.Category : UnicodeCategory.OtherNotAssigned : null as UnicodeCategory?; }
		}

		public ContributoryProperties? ContributoryProperties
		{
			get { return character != null ? characterData != null ? characterData.ContributoryProperties : 0 : null as ContributoryProperties?; }
		}
	}
}
