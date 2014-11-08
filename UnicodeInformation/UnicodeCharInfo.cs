using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeCharInfo
	{
		private readonly int codePoint;
		private readonly UnicodeCharacterData characterData;
		private readonly string block;

		public int CodePoint { get { return codePoint; } }

		public string Name
		{
			get
			{
				return characterData.Name == null || characterData.CodePointRange.IsSingleCodePoint ?
					characterData.Name :
					characterData.Name + "-" + codePoint.ToString("X4");
            }
		}

        public UnicodeCategory Category { get { return characterData.Category; } }
        public string Block { get { return block; } }
		public CanonicalCombiningClass CanonicalCombiningClass { get { return characterData.CanonicalCombiningClass; } }
		public BidirectionalClass BidirectionalClass { get { return characterData.BidirectionalClass; } }
		public CompatibilityFormattingTag DecompositionType { get { return characterData.DecompositionType; } }
		public string DecompositionMapping { get { return characterData.DecompositionMapping; } }
		public UnicodeNumericType NumericType { get { return characterData.NumericType; } }
		public UnicodeRationalNumber? NumericValue { get { return characterData.NumericValue; } }
		public bool BidirectionalMirrored { get { return characterData.BidirectionalMirrored; } }
		public string OldName { get { return characterData.OldName; } }
		public string SimpleUpperCaseMapping { get { return characterData.SimpleUpperCaseMapping; } }
		public string SimpleLowerCaseMapping { get { return characterData.SimpleLowerCaseMapping; } }
		public string SimpleTitleCaseMapping { get { return characterData.SimpleTitleCaseMapping; } }
		public ContributoryProperties ContributoryProperties { get { return characterData.ContributoryProperties; } }
		public CoreProperties CoreProperties { get { return characterData.CoreProperties; } }

		internal UnicodeCharInfo(int codePoint, UnicodeCharacterData characterData, string block)
		{
			this.codePoint = codePoint;
			this.characterData = characterData;
			this.block = block;
		}
	}
}
