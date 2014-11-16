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
		private string displayText;
		private int codePoint;
		private UnicodeCharInfo characterInfo = UnicodeInfo.GetCharInfo(0);

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
						characterInfo = UnicodeInfo.GetCharInfo(codePoint);
					}
					else
					{
						codePoint = 0;
						characterInfo = UnicodeInfo.GetCharInfo(0);
					}

					NotifyPropertyChanged();
					UpdateDisplayText();
					NotifyPropertyChanged(nameof(CodePoint));
					NotifyPropertyChanged(nameof(Name));
					NotifyPropertyChanged(nameof(OldName));
					NotifyPropertyChanged(nameof(Definition));
					NotifyPropertyChanged(nameof(Category));
					NotifyPropertyChanged(nameof(Block));
					NotifyPropertyChanged(nameof(CanonicalCombiningClass));
					NotifyPropertyChanged(nameof(BidirectionalClass));
					NotifyPropertyChanged(nameof(DecompositionType));
					NotifyPropertyChanged(nameof(DecompositionMapping));
					NotifyPropertyChanged(nameof(NumericType));
					NotifyPropertyChanged(nameof(UnihanNumericType));
					NotifyPropertyChanged(nameof(NumericValue));
					NotifyPropertyChanged(nameof(ContributoryProperties));
					NotifyPropertyChanged(nameof(CoreProperties));
					NotifyPropertyChanged(nameof(MandarinReading));
					NotifyPropertyChanged(nameof(CantoneseReading));
					NotifyPropertyChanged(nameof(JapaneseKunReading));
					NotifyPropertyChanged(nameof(JapaneseOnReading));
					NotifyPropertyChanged(nameof(KoreanReading));
					NotifyPropertyChanged(nameof(HangulReading));
					NotifyPropertyChanged(nameof(VietnameseReading));
					NotifyPropertyChanged(nameof(SimplifiedVariant));
					NotifyPropertyChanged(nameof(TraditionalVariant));
				}
			}
		}

		private void UpdateDisplayText()
		{
			string oldValue = displayText;

			displayText = character != null ? UnicodeInfo.GetDisplayText(characterInfo) : null;

			if (displayText != oldValue)
				NotifyPropertyChanged(nameof(DisplayText));
		}

		public string DisplayText { get { return displayText; } }

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

		public string Definition
		{
			get { return character != null ? characterInfo.Definition : null; }
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

		public UnihanNumericType? UnihanNumericType
		{
			get { return character != null ? characterInfo.UnihanNumericType : null as UnihanNumericType?; }
		}

		public UnicodeRationalNumber? NumericValue
		{
			get { return character != null && characterInfo.NumericType != UnicodeNumericType.None ? characterInfo.NumericValue : null as UnicodeRationalNumber?; }
		}

		public ContributoryProperties? ContributoryProperties
		{
			get { return character != null ? characterInfo.ContributoryProperties : null as ContributoryProperties?; }
		}

		public CoreProperties? CoreProperties
		{
			get { return character != null ? characterInfo.CoreProperties : null as CoreProperties?; }
		}

		public string MandarinReading
		{
			get { return character != null ? characterInfo.MandarinReading : null; }
		}

		public string CantoneseReading
		{
			get { return character != null ? characterInfo.CantoneseReading : null; }
		}

		public string JapaneseKunReading
		{
			get { return character != null ? characterInfo.JapaneseKunReading : null; }
		}

		public string JapaneseOnReading
		{
			get { return character != null ? characterInfo.JapaneseOnReading : null; }
		}

		public string KoreanReading
		{
			get { return character != null ? characterInfo.KoreanReading : null; }
		}

		public string HangulReading
		{
			get { return character != null ? characterInfo.HangulReading : null; }
		}

		public string VietnameseReading
		{
			get { return character != null ? characterInfo.VietnameseReading : null; }
		}

		public string SimplifiedVariant
		{
			get { return character != null ? characterInfo.SimplifiedVariant : null; }
		}

		public string TraditionalVariant
		{
			get { return character != null ? characterInfo.TraditionalVariant : null; }
		}
	}
}
