using System;
using System.Globalization;
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
					NotifyPropertyChanged(nameof(NameAliases));
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
					NotifyPropertyChanged(nameof(EmojiProperties));
					NotifyPropertyChanged(nameof(RadicalStrokeCounts));
					NotifyPropertyChanged(nameof(CrossReferences));
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

			if (displayText != oldValue) NotifyPropertyChanged(nameof(DisplayText));
		}

		public string DisplayText => displayText;

		public int? CodePoint => character != null ? codePoint : null as int?;

		public string Name => character != null ? characterInfo.Name : null;
		public string OldName => character != null ? characterInfo.OldName : null;
		public UnicodeNameAliasCollection NameAliases => character != null ? characterInfo.NameAliases : UnicodeNameAliasCollection.Empty;

		public string Definition => character != null ? characterInfo.Definition : null;

		public UnicodeCategory? Category => character != null ? characterInfo.Category : null as UnicodeCategory?;
		public string Block => character != null ? characterInfo.Block : null;

		public CanonicalCombiningClass? CanonicalCombiningClass => character != null ? characterInfo.CanonicalCombiningClass : null as CanonicalCombiningClass?;
		public BidirectionalClass? BidirectionalClass => character != null ? characterInfo.BidirectionalClass : null as BidirectionalClass?;
		public CompatibilityFormattingTag? DecompositionType => character != null && characterInfo.DecompositionMapping != null ? characterInfo.DecompositionType : null as CompatibilityFormattingTag?;

		public string DecompositionMapping => character != null ? characterInfo.DecompositionMapping : null;

		public UnicodeNumericType? NumericType => character != null ? characterInfo.NumericType : null as UnicodeNumericType?;
		public UnihanNumericType? UnihanNumericType => character != null ? characterInfo.UnihanNumericType : null as UnihanNumericType?;
		public UnicodeRationalNumber? NumericValue => character != null && characterInfo.NumericType != UnicodeNumericType.None ? characterInfo.NumericValue : null as UnicodeRationalNumber?;

		public ContributoryProperties? ContributoryProperties => character != null ? characterInfo.ContributoryProperties : null as ContributoryProperties?;
		public CoreProperties? CoreProperties => character != null ? characterInfo.CoreProperties : null as CoreProperties?;
		public EmojiProperties? EmojiProperties => character != null ? characterInfo.EmojiProperties : null as EmojiProperties?;

		public UnicodeRadicalStrokeCountCollection RadicalStrokeCounts => character != null ? characterInfo.UnicodeRadicalStrokeCounts : UnicodeRadicalStrokeCountCollection.Empty;

		public UnicodeCrossReferenceCollection CrossReferences => character != null ? characterInfo.CrossRerefences : UnicodeCrossReferenceCollection.Empty;

		public string MandarinReading => character != null ? characterInfo.MandarinReading : null;
		public string CantoneseReading => character != null ? characterInfo.CantoneseReading : null;
		public string JapaneseKunReading => character != null ? characterInfo.JapaneseKunReading : null;
		public string JapaneseOnReading => character != null ? characterInfo.JapaneseOnReading : null;
		public string KoreanReading => character != null ? characterInfo.KoreanReading : null;
		public string HangulReading => character != null ? characterInfo.HangulReading : null;
		public string VietnameseReading => character != null ? characterInfo.VietnameseReading : null;

		public string SimplifiedVariant => character != null ? characterInfo.SimplifiedVariant : null;
		public string TraditionalVariant => character != null ? characterInfo.TraditionalVariant : null;
	}
}
