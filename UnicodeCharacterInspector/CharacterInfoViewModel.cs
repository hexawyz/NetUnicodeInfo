using System;
using System.Globalization;
using System.Unicode;

namespace UnicodeCharacterInspector
{
	internal sealed class CharacterInfoViewModel : BindableObject
	{
		private string _character;
		private int _codePoint;
		private UnicodeCharInfo _characterInfo = UnicodeInfo.GetCharInfo(0);

		public CharacterInfoViewModel()
		{
		}

		public string Character
		{
			get => _character;
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

				if (value != _character)
				{
					if ((_character = value) != null)
					{
						_codePoint = char.ConvertToUtf32(_character, 0);
						_characterInfo = UnicodeInfo.GetCharInfo(_codePoint);
					}
					else
					{
						_codePoint = 0;
						_characterInfo = UnicodeInfo.GetCharInfo(0);
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
			string oldValue = DisplayText;

			DisplayText = _character != null ? UnicodeInfo.GetDisplayText(_characterInfo) : null;

			if (DisplayText != oldValue) NotifyPropertyChanged(nameof(DisplayText));
		}

		public string DisplayText { get; private set; }

		public int? CodePoint => _character != null ? _codePoint : null as int?;

		public string Name => _character != null ? _characterInfo.Name : null;
		public string OldName => _character != null ? _characterInfo.OldName : null;
		public UnicodeNameAliasCollection NameAliases => _character != null ? _characterInfo.NameAliases : UnicodeNameAliasCollection.Empty;

		public string Definition => _character != null ? _characterInfo.Definition : null;

		public UnicodeCategory? Category => _character != null ? _characterInfo.Category : null as UnicodeCategory?;
		public string Block => _character != null ? _characterInfo.Block : null;

		public CanonicalCombiningClass? CanonicalCombiningClass => _character != null ? _characterInfo.CanonicalCombiningClass : null as CanonicalCombiningClass?;
		public BidirectionalClass? BidirectionalClass => _character != null ? _characterInfo.BidirectionalClass : null as BidirectionalClass?;
		public CompatibilityFormattingTag? DecompositionType => _character != null && _characterInfo.DecompositionMapping != null ? _characterInfo.DecompositionType : null as CompatibilityFormattingTag?;

		public string DecompositionMapping => _character != null ? _characterInfo.DecompositionMapping : null;

		public UnicodeNumericType? NumericType => _character != null ? _characterInfo.NumericType : null as UnicodeNumericType?;
		public UnihanNumericType? UnihanNumericType => _character != null ? _characterInfo.UnihanNumericType : null as UnihanNumericType?;
		public UnicodeRationalNumber? NumericValue => _character != null && _characterInfo.NumericType != UnicodeNumericType.None ? _characterInfo.NumericValue : null as UnicodeRationalNumber?;

		public ContributoryProperties? ContributoryProperties => _character != null ? _characterInfo.ContributoryProperties : null as ContributoryProperties?;
		public CoreProperties? CoreProperties => _character != null ? _characterInfo.CoreProperties : null as CoreProperties?;
		public EmojiProperties? EmojiProperties => _character != null ? _characterInfo.EmojiProperties : null as EmojiProperties?;

		public UnicodeRadicalStrokeCountCollection RadicalStrokeCounts => _character != null ? _characterInfo.UnicodeRadicalStrokeCounts : UnicodeRadicalStrokeCountCollection.Empty;

		public UnicodeCrossReferenceCollection CrossReferences => _character != null ? _characterInfo.CrossRerefences : UnicodeCrossReferenceCollection.Empty;

		public string MandarinReading => _character != null ? _characterInfo.MandarinReading : null;
		public string CantoneseReading => _character != null ? _characterInfo.CantoneseReading : null;
		public string JapaneseKunReading => _character != null ? _characterInfo.JapaneseKunReading : null;
		public string JapaneseOnReading => _character != null ? _characterInfo.JapaneseOnReading : null;
		public string KoreanReading => _character != null ? _characterInfo.KoreanReading : null;
		public string HangulReading => _character != null ? _characterInfo.HangulReading : null;
		public string VietnameseReading => _character != null ? _characterInfo.VietnameseReading : null;

		public string SimplifiedVariant => _character != null ? _characterInfo.SimplifiedVariant : null;
		public string TraditionalVariant => _character != null ? _characterInfo.TraditionalVariant : null;
	}
}
