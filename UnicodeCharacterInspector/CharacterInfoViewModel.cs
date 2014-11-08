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
		private UnicodeCharInfo characterInfo = UnicodeInfo.Default.GetCharInfo(0);

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
						characterInfo = UnicodeInfo.Default.GetCharInfo(codePoint);
					}
					else
					{
						codePoint = 0;
						characterInfo = UnicodeInfo.Default.GetCharInfo(0);
                    }

					NotifyPropertyChanged();
					NotifyPropertyChanged("CodePoint");
					NotifyPropertyChanged("Name");
					NotifyPropertyChanged("OldName");
					NotifyPropertyChanged("Category");
					NotifyPropertyChanged("Block");
					NotifyPropertyChanged("CanonicalCombiningClass");
					NotifyPropertyChanged("BidirectionalClass");
					NotifyPropertyChanged("DecompositionType");
					NotifyPropertyChanged("DecompositionMapping");
					NotifyPropertyChanged("NumericType");
					NotifyPropertyChanged("NumericValue");
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
			get { return character != null ? characterInfo.Name : null; }
		}

		public string OldName
		{
			get { return character != null ? characterInfo.OldName : null; }
		}

		public UnicodeCategory? Category
		{
			get { return character != null ? characterInfo.Category : null as UnicodeCategory?; }
		}

		public string Block
		{
			get { return character != null ? characterInfo.Block : null; }
		}

		public CanonicalCombiningClass? CanonicalCombiningClass
		{
			get { return character != null ? characterInfo.CanonicalCombiningClass : null as CanonicalCombiningClass?; }
		}

		public BidirectionalClass? BidirectionalClass
		{
			get { return character != null ? characterInfo.BidirectionalClass : null as BidirectionalClass?; }
		}

		public CompatibilityFormattingTag? DecompositionType
		{
			get { return character != null && characterInfo.DecompositionMapping != null ? characterInfo.DecompositionType : null as CompatibilityFormattingTag?; }
		}

		public string DecompositionMapping
		{
			get { return character != null ? characterInfo.DecompositionMapping : null; }
		}

		public UnicodeNumericType? NumericType
		{
			get { return character != null ? characterInfo.NumericType : null as UnicodeNumericType?; }
		}

		public UnicodeRationalNumber? NumericValue
		{
			get { return character != null && characterInfo.NumericType != UnicodeNumericType.None ? characterInfo.NumericValue : null as UnicodeRationalNumber?; }
		}

		public ContributoryProperties? ContributoryProperties
		{
			get { return character != null ? characterInfo.ContributoryProperties : null as ContributoryProperties?; }
		}
	}
}
