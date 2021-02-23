using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using M = System.Windows.Media;

namespace Opulos.Core.UI {

public static partial class FontEx {

	// e.g. new Font("Segoe UI Symbol", 10).HasGlyph('\u23F0') == true; // alarm clock
	public static bool HasGlyph(this Font font, String glyph) {
		return HasGlyph(font.FontFamily.Name, glyph);
	}

	public static bool HasGlyph(String fontFamily, String glyph) {
		var family = new M::FontFamily(fontFamily);
		return HasGlyph(family, glyph);
	}

	public static bool HasGlyph(M::FontFamily family, String glyph) {
		var typefaces = family.GetTypefaces();
		return HasGlyph(typefaces, glyph);
	}

	public static bool HasGlyph(ICollection<M::Typeface> typefaces, String glyph) {
		int unicodeValue = Char.ConvertToUtf32(glyph, 0);
		return HasGlyph(typefaces, unicodeValue);
	}

//----------

	// e.g. new Font("Segoe UI Symbol", 10).HasGlyph('\u23F0') == true; // alarm clock
	public static bool HasGlyph(this Font font, char c) {
		return HasGlyph(font.FontFamily.Name, c);
	}

	public static bool HasGlyphs(this Font font, String text) {
		return HasGlyphs(new M::FontFamily(font.FontFamily.Name), text);
	}

	public static bool HasGlyph(String fontFamily, char c) {
		var family = new M::FontFamily(fontFamily);
		return HasGlyph(family, c);
	}

	public static bool HasGlyph(M::FontFamily family, char c) {
		var typefaces = family.GetTypefaces();
		return HasGlyph(typefaces, c);
	}

	public static bool HasGlyph(ICollection<M::Typeface> typefaces, char c) {
		int unicodeValue = Convert.ToUInt16(c);
		return HasGlyph(typefaces, unicodeValue);
	}

	public static bool HasGlyph(ICollection<M::Typeface> typefaces, int unicodeValue) {
		M::GlyphTypeface glyph;
		ushort glyphIndex;
		foreach (var typeface in typefaces) {
			if (typeface.TryGetGlyphTypeface(out glyph)) {
				if (glyph.CharacterToGlyphMap.TryGetValue(unicodeValue, out glyphIndex))
					return true;
			}
		}
		return false;
	}

	public static List<FontFamily> FindSupportingFontFamilies(String text) {
		List<FontFamily> list2 = new List<FontFamily>();
		foreach (FontFamily f in FontFamily.Families) {
			bool hasAll = true;
			var family = new M::FontFamily(f.Name);
			var typefaces = family.GetTypefaces();
			for (int i = 0; i < text.Length; i++) {
				char c = text[i];
				bool b = false;
				if (char.IsHighSurrogate(c)) {
					char c2 = text[i+1];
					int unicode = char.ConvertToUtf32(c, c2);
					b = FontEx.HasGlyph(typefaces, unicode);
				}
				else {
					b = FontEx.HasGlyph(typefaces, c);
				}
				if (!b) {
					hasAll = false;
					break;
				}
			}
			if (hasAll)
				list2.Add(f);
		}
		return list2;
	}

	public static bool HasGlyphs(this M::FontFamily family, String text) {
		bool hasAll = true;
		//var family = new System.Windows.Media.FontFamily(f.Name);
		var typefaces = family.GetTypefaces();
		for (int i = 0; i < text.Length; i++) {
			char c = text[i];
			bool b = false;
			if (char.IsHighSurrogate(c)) {
				if (i+1 < text.Length) {
					char c2 = text[i+1];
					if (Char.IsLowSurrogate(c2)) {
						int unicode = char.ConvertToUtf32(c, c2);
						b = FontEx.HasGlyph(typefaces, unicode);
						if (b)
							i++;
					}
				}
			}
			else {
				b = FontEx.HasGlyph(typefaces, c);
			}
			if (!b) {
				hasAll = false;
				break;
			}
		}
		return hasAll;
	}


	public static bool HasGlyph(this M::FontFamily family, int charPoint) {

		var typefaces = family.GetTypefaces();
		foreach (var typeface in typefaces) { // e.g. bold, oblique, italic, regular/normal, etc.
			M::GlyphTypeface glyph;
			typeface.TryGetGlyphTypeface(out glyph);
			IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;

			ushort glyphIndex = 0;
			characterMap.TryGetValue(charPoint, out glyphIndex); // 0x23F0 (alarm clock)
			if (glyphIndex != 0)
				return true;
		}

		return false;
	}

	// e.g. var families = System.Windows.Media.Fonts.GetFontFamilies(@"C:\WINDOWS\Fonts\seguisym.ttf");
	public static M::FontFamily HasGlyph(ICollection<M::FontFamily> families, int charPoint) {
		foreach (var family in families) {
			if (HasGlyph(family, charPoint))
				return family;
		}
		return null;
	}

	///<summary>
	///This method is the first step in creating a image from a Font and some text. The inputs are parallel arrays in order of preference.
	///The first font that can be created and supports a glyph for each character in the text are in the 'out' variables.
	///</summary>
	public static void CreateFont(String[] fontNames, float[] fontSizes, FontStyle[] fontStyles, String[] texts, out Font font, out String text, int startIndex = 0) {
		FontGlyph fg = CreateFont(fontNames, texts, fontSizes, fontStyles, startIndex);
		font = fg.Font;
		text = fg.Text;
	}

	private static readonly Hashtable htUnsupportedFontNames = new Hashtable(StringComparer.OrdinalIgnoreCase);
	public static FontGlyph CreateFont(String[] fontNames, String[] texts, float[] fontSizes, FontStyle[] fontStyles, int startIndex = 0) {
		Font font = null;
		String fontName = "";
		String fontText = "";
		float fontSize = 10f;
		FontStyle fontStyle = FontStyle.Regular;
		int max = Math.Max(fontNames.Length, texts.Length);

		for (int i = startIndex; i < max; i++) {
			if (i < fontNames.Length)
				fontName = fontNames[i];
			if (i < texts.Length)
				fontText = texts[i];
			if (fontSizes != null && i < fontSizes.Length)
				fontSize = fontSizes[i];
			if (fontStyles != null && i < fontStyles.Length)
				fontStyle = fontStyles[i];

			try {
				if (!htUnsupportedFontNames.ContainsKey(fontName)) {
					Font f = FontEx.New(fontName, fontSize, fontStyle);
					if (f.HasGlyphs(fontText)) {
						font = f;
						break;
					}
				}
			} catch {
				htUnsupportedFontNames[fontName] = "";
			}
		}
		return new FontGlyph(font, fontText);
	}
}

public class FontGlyph {
	public readonly String Text;
	public readonly Font Font;

	public FontGlyph(Font font, String text) {
		this.Font = font;
		this.Text = text;
	}

	public override String ToString() {
		return Text;
	}
}
}