using System.ComponentModel.DataAnnotations;

namespace System.Unicode
{
	/// <summary>Represent known values for the Canonical_Combining_Class property.</summary>
	public enum CanonicalCombiningClass : byte
	{
		/// <summary>Represents the value Not_Reordered.</summary>
		[ValueName("Not_Reordered"), Display(Name = "Not_Reordered", Description = "Spacing and enclosing marks; also many vowel and consonant signs, even if nonspacing")]
		NotReordered = 0,
		/// <summary>Represents the value Overlay.</summary>
		[ValueName("Overlay"), Display(Name = "Overlay", Description = "Marks which overlay a base letter or symbol")]
		Overlay = 1,
		/// <summary>Represents the value Nukta.</summary>
		[ValueName("Nukta"), Display(Name = "Nukta", Description = "Diacritic nukta marks in Brahmi-derived scripts")]
		Nukta = 7,
		/// <summary>Represents the value Kana_Voicing.</summary>
		[ValueName("Kana_Voicing"), Display(Name = "Kana_Voicing", Description = "Hiragana/Katakana voicing marks")]
		KanaVoicing = 8,
		/// <summary>Represents the value Virama.</summary>
		[ValueName("Virama"), Display(Name = "Virama", Description = "Viramas")]
		Virama = 9,
		/// <summary>Represents the value Ccc10.</summary>
		[ValueName("Ccc10"), Display(Name = "Ccc10", Description = "Start of fixed position classes")]
		Ccc10 = 10,
		/// <summary>Represents the value Ccc11.</summary>
		[ValueName("Ccc11"), Display(Name = "Ccc11")]
		Ccc11 = 11,
		/// <summary>Represents the value Ccc12.</summary>
		[ValueName("Ccc12"), Display(Name = "Ccc12")]
		Ccc12 = 12,
		/// <summary>Represents the value Ccc13.</summary>
		[ValueName("Ccc13"), Display(Name = "Ccc13")]
		Ccc13 = 13,
		/// <summary>Represents the value Ccc14.</summary>
		[ValueName("Ccc14"), Display(Name = "Ccc14")]
		Ccc14 = 14,
		/// <summary>Represents the value Ccc15.</summary>
		[ValueName("Ccc15"), Display(Name = "Ccc15")]
		Ccc15 = 15,
		/// <summary>Represents the value Ccc16.</summary>
		[ValueName("Ccc16"), Display(Name = "Ccc16")]
		Ccc16 = 16,
		/// <summary>Represents the value Ccc17.</summary>
		[ValueName("Ccc17"), Display(Name = "Ccc17")]
		Ccc17 = 17,
		/// <summary>Represents the value Ccc18.</summary>
		[ValueName("Ccc18"), Display(Name = "Ccc18")]
		Ccc18 = 18,
		/// <summary>Represents the value Ccc19.</summary>
		[ValueName("Ccc19"), Display(Name = "Ccc19")]
		Ccc19 = 19,
		/// <summary>Represents the value Ccc20.</summary>
		[ValueName("Ccc20"), Display(Name = "Ccc20")]
		Ccc20 = 20,
		/// <summary>Represents the value Ccc21.</summary>
		[ValueName("Ccc21"), Display(Name = "Ccc21")]
		Ccc21 = 21,
		/// <summary>Represents the value Ccc22.</summary>
		[ValueName("Ccc22"), Display(Name = "Ccc22")]
		Ccc22 = 22,
		/// <summary>Represents the value Ccc23.</summary>
		[ValueName("Ccc23"), Display(Name = "Ccc23")]
		Ccc23 = 23,
		/// <summary>Represents the value Ccc24.</summary>
		[ValueName("Ccc24"), Display(Name = "Ccc24")]
		Ccc24 = 24,
		/// <summary>Represents the value Ccc25.</summary>
		[ValueName("Ccc25"), Display(Name = "Ccc25")]
		Ccc25 = 25,
		/// <summary>Represents the value Ccc26.</summary>
		[ValueName("Ccc26"), Display(Name = "Ccc26")]
		Ccc26 = 26,
		/// <summary>Represents the value Ccc27.</summary>
		[ValueName("Ccc27"), Display(Name = "Ccc27")]
		Ccc27 = 27,
		/// <summary>Represents the value Ccc28.</summary>
		[ValueName("Ccc28"), Display(Name = "Ccc28")]
		Ccc28 = 28,
		/// <summary>Represents the value Ccc29.</summary>
		[ValueName("Ccc29"), Display(Name = "Ccc29")]
		Ccc29 = 29,
		/// <summary>Represents the value Ccc30.</summary>
		[ValueName("Ccc30"), Display(Name = "Ccc30")]
		Ccc30 = 30,
		/// <summary>Represents the value Ccc31.</summary>
		[ValueName("Ccc31"), Display(Name = "Ccc31")]
		Ccc31 = 31,
		/// <summary>Represents the value Ccc32.</summary>
		[ValueName("Ccc32"), Display(Name = "Ccc32")]
		Ccc32 = 32,
		/// <summary>Represents the value Ccc33.</summary>
		[ValueName("Ccc33"), Display(Name = "Ccc33")]
		Ccc33 = 33,
		/// <summary>Represents the value Ccc34.</summary>
		[ValueName("Ccc34"), Display(Name = "Ccc34")]
		Ccc34 = 34,
		/// <summary>Represents the value Ccc35.</summary>
		[ValueName("Ccc35"), Display(Name = "Ccc35")]
		Ccc35 = 35,
		/// <summary>Represents the value Ccc36.</summary>
		[ValueName("Ccc36"), Display(Name = "Ccc36")]
		Ccc36 = 36,
		/// <summary>Represents the value Ccc37.</summary>
		[ValueName("Ccc37"), Display(Name = "Ccc37")]
		Ccc37 = 37,
		/// <summary>Represents the value Ccc38.</summary>
		[ValueName("Ccc38"), Display(Name = "Ccc38")]
		Ccc38 = 38,
		/// <summary>Represents the value Ccc39.</summary>
		[ValueName("Ccc39"), Display(Name = "Ccc39")]
		Ccc39 = 39,
		/// <summary>Represents the value Ccc40.</summary>
		[ValueName("Ccc40"), Display(Name = "Ccc40")]
		Ccc40 = 40,
		/// <summary>Represents the value Ccc41.</summary>
		[ValueName("Ccc41"), Display(Name = "Ccc41")]
		Ccc41 = 41,
		/// <summary>Represents the value Ccc42.</summary>
		[ValueName("Ccc42"), Display(Name = "Ccc42")]
		Ccc42 = 42,
		/// <summary>Represents the value Ccc43.</summary>
		[ValueName("Ccc43"), Display(Name = "Ccc43")]
		Ccc43 = 43,
		/// <summary>Represents the value Ccc44.</summary>
		[ValueName("Ccc44"), Display(Name = "Ccc44")]
		Ccc44 = 44,
		/// <summary>Represents the value Ccc45.</summary>
		[ValueName("Ccc45"), Display(Name = "Ccc45")]
		Ccc45 = 45,
		/// <summary>Represents the value Ccc46.</summary>
		[ValueName("Ccc46"), Display(Name = "Ccc46")]
		Ccc46 = 46,
		/// <summary>Represents the value Ccc47.</summary>
		[ValueName("Ccc47"), Display(Name = "Ccc47")]
		Ccc47 = 47,
		/// <summary>Represents the value Ccc48.</summary>
		[ValueName("Ccc48"), Display(Name = "Ccc48")]
		Ccc48 = 48,
		/// <summary>Represents the value Ccc49.</summary>
		[ValueName("Ccc49"), Display(Name = "Ccc49")]
		Ccc49 = 49,
		/// <summary>Represents the value Ccc50.</summary>
		[ValueName("Ccc50"), Display(Name = "Ccc50")]
		Ccc50 = 50,
		/// <summary>Represents the value Ccc51.</summary>
		[ValueName("Ccc51"), Display(Name = "Ccc51")]
		Ccc51 = 51,
		/// <summary>Represents the value Ccc52.</summary>
		[ValueName("Ccc52"), Display(Name = "Ccc52")]
		Ccc52 = 52,
		/// <summary>Represents the value Ccc53.</summary>
		[ValueName("Ccc53"), Display(Name = "Ccc53")]
		Ccc53 = 53,
		/// <summary>Represents the value Ccc54.</summary>
		[ValueName("Ccc54"), Display(Name = "Ccc54")]
		Ccc54 = 54,
		/// <summary>Represents the value Ccc55.</summary>
		[ValueName("Ccc55"), Display(Name = "Ccc55")]
		Ccc55 = 55,
		/// <summary>Represents the value Ccc56.</summary>
		[ValueName("Ccc56"), Display(Name = "Ccc56")]
		Ccc56 = 56,
		/// <summary>Represents the value Ccc57.</summary>
		[ValueName("Ccc57"), Display(Name = "Ccc57")]
		Ccc57 = 57,
		/// <summary>Represents the value Ccc58.</summary>
		[ValueName("Ccc58"), Display(Name = "Ccc58")]
		Ccc58 = 58,
		/// <summary>Represents the value Ccc59.</summary>
		[ValueName("Ccc59"), Display(Name = "Ccc59")]
		Ccc59 = 59,
		/// <summary>Represents the value Ccc60.</summary>
		[ValueName("Ccc60"), Display(Name = "Ccc60")]
		Ccc60 = 60,
		/// <summary>Represents the value Ccc61.</summary>
		[ValueName("Ccc61"), Display(Name = "Ccc61")]
		Ccc61 = 61,
		/// <summary>Represents the value Ccc62.</summary>
		[ValueName("Ccc62"), Display(Name = "Ccc62")]
		Ccc62 = 62,
		/// <summary>Represents the value Ccc63.</summary>
		[ValueName("Ccc63"), Display(Name = "Ccc63")]
		Ccc63 = 63,
		/// <summary>Represents the value Ccc64.</summary>
		[ValueName("Ccc64"), Display(Name = "Ccc64")]
		Ccc64 = 64,
		/// <summary>Represents the value Ccc65.</summary>
		[ValueName("Ccc65"), Display(Name = "Ccc65")]
		Ccc65 = 65,
		/// <summary>Represents the value Ccc66.</summary>
		[ValueName("Ccc66"), Display(Name = "Ccc66")]
		Ccc66 = 66,
		/// <summary>Represents the value Ccc67.</summary>
		[ValueName("Ccc67"), Display(Name = "Ccc67")]
		Ccc67 = 67,
		/// <summary>Represents the value Ccc68.</summary>
		[ValueName("Ccc68"), Display(Name = "Ccc68")]
		Ccc68 = 68,
		/// <summary>Represents the value Ccc69.</summary>
		[ValueName("Ccc69"), Display(Name = "Ccc69")]
		Ccc69 = 69,
		/// <summary>Represents the value Ccc70.</summary>
		[ValueName("Ccc70"), Display(Name = "Ccc70")]
		Ccc70 = 70,
		/// <summary>Represents the value Ccc71.</summary>
		[ValueName("Ccc71"), Display(Name = "Ccc71")]
		Ccc71 = 71,
		/// <summary>Represents the value Ccc72.</summary>
		[ValueName("Ccc72"), Display(Name = "Ccc72")]
		Ccc72 = 72,
		/// <summary>Represents the value Ccc73.</summary>
		[ValueName("Ccc73"), Display(Name = "Ccc73")]
		Ccc73 = 73,
		/// <summary>Represents the value Ccc74.</summary>
		[ValueName("Ccc74"), Display(Name = "Ccc74")]
		Ccc74 = 74,
		/// <summary>Represents the value Ccc75.</summary>
		[ValueName("Ccc75"), Display(Name = "Ccc75")]
		Ccc75 = 75,
		/// <summary>Represents the value Ccc76.</summary>
		[ValueName("Ccc76"), Display(Name = "Ccc76")]
		Ccc76 = 76,
		/// <summary>Represents the value Ccc77.</summary>
		[ValueName("Ccc77"), Display(Name = "Ccc77")]
		Ccc77 = 77,
		/// <summary>Represents the value Ccc78.</summary>
		[ValueName("Ccc78"), Display(Name = "Ccc78")]
		Ccc78 = 78,
		/// <summary>Represents the value Ccc79.</summary>
		[ValueName("Ccc79"), Display(Name = "Ccc79")]
		Ccc79 = 79,
		/// <summary>Represents the value Ccc80.</summary>
		[ValueName("Ccc80"), Display(Name = "Ccc80")]
		Ccc80 = 80,
		/// <summary>Represents the value Ccc81.</summary>
		[ValueName("Ccc81"), Display(Name = "Ccc81")]
		Ccc81 = 81,
		/// <summary>Represents the value Ccc82.</summary>
		[ValueName("Ccc82"), Display(Name = "Ccc82")]
		Ccc82 = 82,
		/// <summary>Represents the value Ccc83.</summary>
		[ValueName("Ccc83"), Display(Name = "Ccc83")]
		Ccc83 = 83,
		/// <summary>Represents the value Ccc84.</summary>
		[ValueName("Ccc84"), Display(Name = "Ccc84")]
		Ccc84 = 84,
		/// <summary>Represents the value Ccc85.</summary>
		[ValueName("Ccc85"), Display(Name = "Ccc85")]
		Ccc85 = 85,
		/// <summary>Represents the value Ccc86.</summary>
		[ValueName("Ccc86"), Display(Name = "Ccc86")]
		Ccc86 = 86,
		/// <summary>Represents the value Ccc87.</summary>
		[ValueName("Ccc87"), Display(Name = "Ccc87")]
		Ccc87 = 87,
		/// <summary>Represents the value Ccc88.</summary>
		[ValueName("Ccc88"), Display(Name = "Ccc88")]
		Ccc88 = 88,
		/// <summary>Represents the value Ccc89.</summary>
		[ValueName("Ccc89"), Display(Name = "Ccc89")]
		Ccc89 = 89,
		/// <summary>Represents the value Ccc90.</summary>
		[ValueName("Ccc90"), Display(Name = "Ccc90")]
		Ccc90 = 90,
		/// <summary>Represents the value Ccc91.</summary>
		[ValueName("Ccc91"), Display(Name = "Ccc91")]
		Ccc91 = 91,
		/// <summary>Represents the value Ccc92.</summary>
		[ValueName("Ccc92"), Display(Name = "Ccc92")]
		Ccc92 = 92,
		/// <summary>Represents the value Ccc93.</summary>
		[ValueName("Ccc93"), Display(Name = "Ccc93")]
		Ccc93 = 93,
		/// <summary>Represents the value Ccc94.</summary>
		[ValueName("Ccc94"), Display(Name = "Ccc94")]
		Ccc94 = 94,
		/// <summary>Represents the value Ccc95.</summary>
		[ValueName("Ccc95"), Display(Name = "Ccc95")]
		Ccc95 = 95,
		/// <summary>Represents the value Ccc96.</summary>
		[ValueName("Ccc96"), Display(Name = "Ccc96")]
		Ccc96 = 96,
		/// <summary>Represents the value Ccc97.</summary>
		[ValueName("Ccc97"), Display(Name = "Ccc97")]
		Ccc97 = 97,
		/// <summary>Represents the value Ccc98.</summary>
		[ValueName("Ccc98"), Display(Name = "Ccc98")]
		Ccc98 = 98,
		/// <summary>Represents the value Ccc99.</summary>
		[ValueName("Ccc99"), Display(Name = "Ccc99")]
		Ccc99 = 99,
		/// <summary>Represents the value Ccc100.</summary>
		[ValueName("Ccc100"), Display(Name = "Ccc100")]
		Ccc100 = 100,
		/// <summary>Represents the value Ccc101.</summary>
		[ValueName("Ccc101"), Display(Name = "Ccc101")]
		Ccc101 = 101,
		/// <summary>Represents the value Ccc102.</summary>
		[ValueName("Ccc102"), Display(Name = "Ccc102")]
		Ccc102 = 102,
		/// <summary>Represents the value Ccc103.</summary>
		[ValueName("Ccc103"), Display(Name = "Ccc103")]
		Ccc103 = 103,
		/// <summary>Represents the value Ccc104.</summary>
		[ValueName("Ccc104"), Display(Name = "Ccc104")]
		Ccc104 = 104,
		/// <summary>Represents the value Ccc105.</summary>
		[ValueName("Ccc105"), Display(Name = "Ccc105")]
		Ccc105 = 105,
		/// <summary>Represents the value Ccc106.</summary>
		[ValueName("Ccc106"), Display(Name = "Ccc106")]
		Ccc106 = 106,
		/// <summary>Represents the value Ccc107.</summary>
		[ValueName("Ccc107"), Display(Name = "Ccc107")]
		Ccc107 = 107,
		/// <summary>Represents the value Ccc108.</summary>
		[ValueName("Ccc108"), Display(Name = "Ccc108")]
		Ccc108 = 108,
		/// <summary>Represents the value Ccc109.</summary>
		[ValueName("Ccc109"), Display(Name = "Ccc109")]
		Ccc109 = 109,
		/// <summary>Represents the value Ccc110.</summary>
		[ValueName("Ccc110"), Display(Name = "Ccc110")]
		Ccc110 = 110,
		/// <summary>Represents the value Ccc111.</summary>
		[ValueName("Ccc111"), Display(Name = "Ccc111")]
		Ccc111 = 111,
		/// <summary>Represents the value Ccc112.</summary>
		[ValueName("Ccc112"), Display(Name = "Ccc112")]
		Ccc112 = 112,
		/// <summary>Represents the value Ccc113.</summary>
		[ValueName("Ccc113"), Display(Name = "Ccc113")]
		Ccc113 = 113,
		/// <summary>Represents the value Ccc114.</summary>
		[ValueName("Ccc114"), Display(Name = "Ccc114")]
		Ccc114 = 114,
		/// <summary>Represents the value Ccc115.</summary>
		[ValueName("Ccc115"), Display(Name = "Ccc115")]
		Ccc115 = 115,
		/// <summary>Represents the value Ccc116.</summary>
		[ValueName("Ccc116"), Display(Name = "Ccc116")]
		Ccc116 = 116,
		/// <summary>Represents the value Ccc117.</summary>
		[ValueName("Ccc117"), Display(Name = "Ccc117")]
		Ccc117 = 117,
		/// <summary>Represents the value Ccc118.</summary>
		[ValueName("Ccc118"), Display(Name = "Ccc118")]
		Ccc118 = 118,
		/// <summary>Represents the value Ccc119.</summary>
		[ValueName("Ccc119"), Display(Name = "Ccc119")]
		Ccc119 = 119,
		/// <summary>Represents the value Ccc120.</summary>
		[ValueName("Ccc120"), Display(Name = "Ccc120")]
		Ccc120 = 120,
		/// <summary>Represents the value Ccc121.</summary>
		[ValueName("Ccc121"), Display(Name = "Ccc121")]
		Ccc121 = 121,
		/// <summary>Represents the value Ccc122.</summary>
		[ValueName("Ccc122"), Display(Name = "Ccc122")]
		Ccc122 = 122,
		/// <summary>Represents the value Ccc123.</summary>
		[ValueName("Ccc123"), Display(Name = "Ccc123")]
		Ccc123 = 123,
		/// <summary>Represents the value Ccc124.</summary>
		[ValueName("Ccc124"), Display(Name = "Ccc124")]
		Ccc124 = 124,
		/// <summary>Represents the value Ccc125.</summary>
		[ValueName("Ccc125"), Display(Name = "Ccc125")]
		Ccc125 = 125,
		/// <summary>Represents the value Ccc126.</summary>
		[ValueName("Ccc126"), Display(Name = "Ccc126")]
		Ccc126 = 126,
		/// <summary>Represents the value Ccc127.</summary>
		[ValueName("Ccc127"), Display(Name = "Ccc127")]
		Ccc127 = 127,
		/// <summary>Represents the value Ccc128.</summary>
		[ValueName("Ccc128"), Display(Name = "Ccc128")]
		Ccc128 = 128,
		/// <summary>Represents the value Ccc129.</summary>
		[ValueName("Ccc129"), Display(Name = "Ccc129")]
		Ccc129 = 129,
		/// <summary>Represents the value Ccc130.</summary>
		[ValueName("Ccc130"), Display(Name = "Ccc130")]
		Ccc130 = 130,
		/// <summary>Represents the value Ccc131.</summary>
		[ValueName("Ccc131"), Display(Name = "Ccc131")]
		Ccc131 = 131,
		/// <summary>Represents the value Ccc132.</summary>
		[ValueName("Ccc132"), Display(Name = "Ccc132")]
		Ccc132 = 132,
		/// <summary>Represents the value Ccc133.</summary>
		[ValueName("Ccc133"), Display(Name = "Ccc133")]
		Ccc133 = 133,
		/// <summary>Represents the value Ccc134.</summary>
		[ValueName("Ccc134"), Display(Name = "Ccc134")]
		Ccc134 = 134,
		/// <summary>Represents the value Ccc135.</summary>
		[ValueName("Ccc135"), Display(Name = "Ccc135")]
		Ccc135 = 135,
		/// <summary>Represents the value Ccc136.</summary>
		[ValueName("Ccc136"), Display(Name = "Ccc136")]
		Ccc136 = 136,
		/// <summary>Represents the value Ccc137.</summary>
		[ValueName("Ccc137"), Display(Name = "Ccc137")]
		Ccc137 = 137,
		/// <summary>Represents the value Ccc138.</summary>
		[ValueName("Ccc138"), Display(Name = "Ccc138")]
		Ccc138 = 138,
		/// <summary>Represents the value Ccc139.</summary>
		[ValueName("Ccc139"), Display(Name = "Ccc139")]
		Ccc139 = 139,
		/// <summary>Represents the value Ccc140.</summary>
		[ValueName("Ccc140"), Display(Name = "Ccc140")]
		Ccc140 = 140,
		/// <summary>Represents the value Ccc141.</summary>
		[ValueName("Ccc141"), Display(Name = "Ccc141")]
		Ccc141 = 141,
		/// <summary>Represents the value Ccc142.</summary>
		[ValueName("Ccc142"), Display(Name = "Ccc142")]
		Ccc142 = 142,
		/// <summary>Represents the value Ccc143.</summary>
		[ValueName("Ccc143"), Display(Name = "Ccc143")]
		Ccc143 = 143,
		/// <summary>Represents the value Ccc144.</summary>
		[ValueName("Ccc144"), Display(Name = "Ccc144")]
		Ccc144 = 144,
		/// <summary>Represents the value Ccc145.</summary>
		[ValueName("Ccc145"), Display(Name = "Ccc145")]
		Ccc145 = 145,
		/// <summary>Represents the value Ccc146.</summary>
		[ValueName("Ccc146"), Display(Name = "Ccc146")]
		Ccc146 = 146,
		/// <summary>Represents the value Ccc147.</summary>
		[ValueName("Ccc147"), Display(Name = "Ccc147")]
		Ccc147 = 147,
		/// <summary>Represents the value Ccc148.</summary>
		[ValueName("Ccc148"), Display(Name = "Ccc148")]
		Ccc148 = 148,
		/// <summary>Represents the value Ccc149.</summary>
		[ValueName("Ccc149"), Display(Name = "Ccc149")]
		Ccc149 = 149,
		/// <summary>Represents the value Ccc150.</summary>
		[ValueName("Ccc150"), Display(Name = "Ccc150")]
		Ccc150 = 150,
		/// <summary>Represents the value Ccc151.</summary>
		[ValueName("Ccc151"), Display(Name = "Ccc151")]
		Ccc151 = 151,
		/// <summary>Represents the value Ccc152.</summary>
		[ValueName("Ccc152"), Display(Name = "Ccc152")]
		Ccc152 = 152,
		/// <summary>Represents the value Ccc153.</summary>
		[ValueName("Ccc153"), Display(Name = "Ccc153")]
		Ccc153 = 153,
		/// <summary>Represents the value Ccc154.</summary>
		[ValueName("Ccc154"), Display(Name = "Ccc154")]
		Ccc154 = 154,
		/// <summary>Represents the value Ccc155.</summary>
		[ValueName("Ccc155"), Display(Name = "Ccc155")]
		Ccc155 = 155,
		/// <summary>Represents the value Ccc156.</summary>
		[ValueName("Ccc156"), Display(Name = "Ccc156")]
		Ccc156 = 156,
		/// <summary>Represents the value Ccc157.</summary>
		[ValueName("Ccc157"), Display(Name = "Ccc157")]
		Ccc157 = 157,
		/// <summary>Represents the value Ccc158.</summary>
		[ValueName("Ccc158"), Display(Name = "Ccc158")]
		Ccc158 = 158,
		/// <summary>Represents the value Ccc159.</summary>
		[ValueName("Ccc159"), Display(Name = "Ccc159")]
		Ccc159 = 159,
		/// <summary>Represents the value Ccc160.</summary>
		[ValueName("Ccc160"), Display(Name = "Ccc160")]
		Ccc160 = 160,
		/// <summary>Represents the value Ccc161.</summary>
		[ValueName("Ccc161"), Display(Name = "Ccc161")]
		Ccc161 = 161,
		/// <summary>Represents the value Ccc162.</summary>
		[ValueName("Ccc162"), Display(Name = "Ccc162")]
		Ccc162 = 162,
		/// <summary>Represents the value Ccc163.</summary>
		[ValueName("Ccc163"), Display(Name = "Ccc163")]
		Ccc163 = 163,
		/// <summary>Represents the value Ccc164.</summary>
		[ValueName("Ccc164"), Display(Name = "Ccc164")]
		Ccc164 = 164,
		/// <summary>Represents the value Ccc165.</summary>
		[ValueName("Ccc165"), Display(Name = "Ccc165")]
		Ccc165 = 165,
		/// <summary>Represents the value Ccc166.</summary>
		[ValueName("Ccc166"), Display(Name = "Ccc166")]
		Ccc166 = 166,
		/// <summary>Represents the value Ccc167.</summary>
		[ValueName("Ccc167"), Display(Name = "Ccc167")]
		Ccc167 = 167,
		/// <summary>Represents the value Ccc168.</summary>
		[ValueName("Ccc168"), Display(Name = "Ccc168")]
		Ccc168 = 168,
		/// <summary>Represents the value Ccc169.</summary>
		[ValueName("Ccc169"), Display(Name = "Ccc169")]
		Ccc169 = 169,
		/// <summary>Represents the value Ccc170.</summary>
		[ValueName("Ccc170"), Display(Name = "Ccc170")]
		Ccc170 = 170,
		/// <summary>Represents the value Ccc171.</summary>
		[ValueName("Ccc171"), Display(Name = "Ccc171")]
		Ccc171 = 171,
		/// <summary>Represents the value Ccc172.</summary>
		[ValueName("Ccc172"), Display(Name = "Ccc172")]
		Ccc172 = 172,
		/// <summary>Represents the value Ccc173.</summary>
		[ValueName("Ccc173"), Display(Name = "Ccc173")]
		Ccc173 = 173,
		/// <summary>Represents the value Ccc174.</summary>
		[ValueName("Ccc174"), Display(Name = "Ccc174")]
		Ccc174 = 174,
		/// <summary>Represents the value Ccc175.</summary>
		[ValueName("Ccc175"), Display(Name = "Ccc175")]
		Ccc175 = 175,
		/// <summary>Represents the value Ccc176.</summary>
		[ValueName("Ccc176"), Display(Name = "Ccc176")]
		Ccc176 = 176,
		/// <summary>Represents the value Ccc177.</summary>
		[ValueName("Ccc177"), Display(Name = "Ccc177")]
		Ccc177 = 177,
		/// <summary>Represents the value Ccc178.</summary>
		[ValueName("Ccc178"), Display(Name = "Ccc178")]
		Ccc178 = 178,
		/// <summary>Represents the value Ccc179.</summary>
		[ValueName("Ccc179"), Display(Name = "Ccc179")]
		Ccc179 = 179,
		/// <summary>Represents the value Ccc180.</summary>
		[ValueName("Ccc180"), Display(Name = "Ccc180")]
		Ccc180 = 180,
		/// <summary>Represents the value Ccc181.</summary>
		[ValueName("Ccc181"), Display(Name = "Ccc181")]
		Ccc181 = 181,
		/// <summary>Represents the value Ccc182.</summary>
		[ValueName("Ccc182"), Display(Name = "Ccc182")]
		Ccc182 = 182,
		/// <summary>Represents the value Ccc183.</summary>
		[ValueName("Ccc183"), Display(Name = "Ccc183")]
		Ccc183 = 183,
		/// <summary>Represents the value Ccc184.</summary>
		[ValueName("Ccc184"), Display(Name = "Ccc184")]
		Ccc184 = 184,
		/// <summary>Represents the value Ccc185.</summary>
		[ValueName("Ccc185"), Display(Name = "Ccc185")]
		Ccc185 = 185,
		/// <summary>Represents the value Ccc186.</summary>
		[ValueName("Ccc186"), Display(Name = "Ccc186")]
		Ccc186 = 186,
		/// <summary>Represents the value Ccc187.</summary>
		[ValueName("Ccc187"), Display(Name = "Ccc187")]
		Ccc187 = 187,
		/// <summary>Represents the value Ccc188.</summary>
		[ValueName("Ccc188"), Display(Name = "Ccc188")]
		Ccc188 = 188,
		/// <summary>Represents the value Ccc189.</summary>
		[ValueName("Ccc189"), Display(Name = "Ccc189")]
		Ccc189 = 189,
		/// <summary>Represents the value Ccc190.</summary>
		[ValueName("Ccc190"), Display(Name = "Ccc190")]
		Ccc190 = 190,
		/// <summary>Represents the value Ccc191.</summary>
		[ValueName("Ccc191"), Display(Name = "Ccc191")]
		Ccc191 = 191,
		/// <summary>Represents the value Ccc192.</summary>
		[ValueName("Ccc192"), Display(Name = "Ccc192")]
		Ccc192 = 192,
		/// <summary>Represents the value Ccc193.</summary>
		[ValueName("Ccc193"), Display(Name = "Ccc193")]
		Ccc193 = 193,
		/// <summary>Represents the value Ccc194.</summary>
		[ValueName("Ccc194"), Display(Name = "Ccc194")]
		Ccc194 = 194,
		/// <summary>Represents the value Ccc195.</summary>
		[ValueName("Ccc195"), Display(Name = "Ccc195")]
		Ccc195 = 195,
		/// <summary>Represents the value Ccc196.</summary>
		[ValueName("Ccc196"), Display(Name = "Ccc196")]
		Ccc196 = 196,
		/// <summary>Represents the value Ccc197.</summary>
		[ValueName("Ccc197"), Display(Name = "Ccc197")]
		Ccc197 = 197,
		/// <summary>Represents the value Ccc198.</summary>
		[ValueName("Ccc198"), Display(Name = "Ccc198")]
		Ccc198 = 198,
		/// <summary>Represents the value Ccc199.</summary>
		[ValueName("Ccc199"), Display(Name = "Ccc199", Description = "End of fixed position classes")]
		Ccc199 = 199,
		/// <summary>Represents the value Attached_Below_Left.</summary>
		[ValueName("Attached_Below_Left"), Display(Name = "Attached_Below_Left", Description = "Marks attached at the bottom left")]
		AttachedBelowLeft = 200,
		/// <summary>Represents the value Attached_Below.</summary>
		[ValueName("Attached_Below"), Display(Name = "Attached_Below", Description = "Marks attached directly below")]
		AttachedBelow = 202,
		/// <summary>Represents the value Attached_Below_Right.</summary>
		[ValueName("Attached_Below_Right"), Display(Name = "Attached_Below_Right", Description = "Marks attached at the bottom right")]
		AttachedBelowRight = 204,
		/// <summary>Represents the value Attached_Left.</summary>
		[ValueName("Attached_Left"), Display(Name = "Attached_Left", Description = "Marks attached to the left")]
		AttachedLeft = 208,
		/// <summary>Represents the value Attached_Right.</summary>
		[ValueName("Attached_Right"), Display(Name = "Attached_Right", Description = "Marks attached to the right")]
		AttachedRight = 210,
		/// <summary>Represents the value Attached_Above_Left.</summary>
		[ValueName("Attached_Above_Left"), Display(Name = "Attached_Above_Left", Description = "Marks attached at the top left")]
		AttachedAboveLeft = 212,
		/// <summary>Represents the value Attached_Above.</summary>
		[ValueName("Attached_Above"), Display(Name = "Attached_Above", Description = "Marks attached directly above")]
		AttachedAbove = 214,
		/// <summary>Represents the value Attached_Above_Right.</summary>
		[ValueName("Attached_Above_Right"), Display(Name = "Attached_Above_Right", Description = "Marks attached at the top right")]
		AttachedAboveRight = 216,
		/// <summary>Represents the value Below_Left.</summary>
		[ValueName("Below_Left"), Display(Name = "Below_Left", Description = "Distinct marks at the bottom left")]
		BelowLeft = 218,
		/// <summary>Represents the value Below.</summary>
		[ValueName("Below"), Display(Name = "Below", Description = "Distinct marks directly below")]
		Below = 220,
		/// <summary>Represents the value Below_Right.</summary>
		[ValueName("Below_Right"), Display(Name = "Below_Right", Description = "Distinct marks at the bottom right")]
		BelowRight = 222,
		/// <summary>Represents the value Left.</summary>
		[ValueName("Left"), Display(Name = "Left", Description = "Distinct marks to the left")]
		Left = 224,
		/// <summary>Represents the value Right.</summary>
		[ValueName("Right"), Display(Name = "Right", Description = "Distinct marks to the right")]
		Right = 226,
		/// <summary>Represents the value Above_Left.</summary>
		[ValueName("Above_Left"), Display(Name = "Above_Left", Description = "Distinct marks at the top left")]
		AboveLeft = 228,
		/// <summary>Represents the value Above.</summary>
		[ValueName("Above"), Display(Name = "Above", Description = "Distinct marks directly above")]
		Above = 230,
		/// <summary>Represents the value Above_Right.</summary>
		[ValueName("Above_Right"), Display(Name = "Above_Right", Description = "Distinct marks at the top right")]
		AboveRight = 232,
		/// <summary>Represents the value Double_Below.</summary>
		[ValueName("Double_Below"), Display(Name = "Double_Below", Description = "Distinct marks subtending two bases")]
		DoubleBelow = 233,
		/// <summary>Represents the value Double_Above.</summary>
		[ValueName("Double_Above"), Display(Name = "Double_Above", Description = "Distinct marks extending above two bases")]
		DoubleAbove = 234,
		/// <summary>Represents the value Iota_Subscript.</summary>
		[ValueName("Iota_Subscript"), Display(Name = "Iota_Subscript", Description = "Greek iota subscript only")]
		IotaSubscript = 240,
	}
}
