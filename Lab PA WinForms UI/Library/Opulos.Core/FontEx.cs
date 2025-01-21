using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using Opulos.Core.Utils;

namespace Opulos.Core.UI {
public static partial class FontEx {

	private static List<Font> newFontsList = new List<Font>();
	private static Hashtable ht = new Hashtable();
	private static Hashtable htFamily = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

	public static Font NewSize(this Font f, float size, FontStyle? style = null) {
		size = SizeF(size);
		return makeFont(f.FontFamily, size, (style.HasValue ? style.Value : f.Style), f.Unit, f.GdiCharSet, f.GdiVerticalFont);
	}

	private static Font makeFont(FontFamily fontFamily, float size, FontStyle style, GraphicsUnit unit, byte charSet, bool gdiVerticalFont) {
		MultiKey mk = new MultiKey(size, style, fontFamily, unit, charSet, gdiVerticalFont);
		Font f = (Font) ht[mk];
		if (f == null) {
			f = new Font(fontFamily, size, style, unit, charSet, gdiVerticalFont);
			ht[mk] = f;
			newFontsList.Add(f);
		}
		return f;
	}

	private static FontFamily GetFamily(String name) {
		FontFamily ff = (FontFamily) htFamily[name];
		if (ff == null) {
			ff = new FontFamily(name);
			htFamily[name] = ff;
		}
		return ff;
	}

	public static Font NewFamily(this Font f, String family) {
		FontFamily ff = GetFamily(family);
		return makeFont(ff, f.Size, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
	}

	public static Font NewStyle(this Font f, FontStyle style) {
		return makeFont(f.FontFamily, f.Size, style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
	}

	public static Font NewUnit(this Font f, GraphicsUnit unit) {
		return makeFont(f.FontFamily, f.Size, f.Style, unit, f.GdiCharSet, f.GdiVerticalFont);
	}

	public static Font New(String fontFamily, float fontSize) {
		FontFamily ff = GetFamily(fontFamily);
		return makeFont(ff, fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
	}

	public static Font New(FontFamily fontFamily, float fontSize) {
		return makeFont(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Point, 1, false);
	}

	public static Font New(String fontName, float fontSize, FontStyle style) {
		return makeFont(GetFamily(fontName), fontSize, style, GraphicsUnit.Point, 1, false);
	}

	public static Font New(FontFamily fontFamily, float fontSize, FontStyle style) {
		return makeFont(fontFamily, fontSize, style, GraphicsUnit.Point, 1, false);
	}

	public static Font New(String fontName, float fontSize, FontStyle style, GraphicsUnit unit, byte charSet) {
		return makeFont(GetFamily(fontName), fontSize, style, unit, charSet, false);
	}

	// adding +1 or -1 to font values like 7.8 can result in precision errors.  Keep at most 3 decimal places.
	// anything after 3 decimal places shouldn't be noticable
	public static float SizeF(float size) {
		String s = size.ToString("#0.000", System.Globalization.CultureInfo.InvariantCulture);
		return float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
	}

	public static double SizeD(double size) {
		String s = size.ToString("#0.000", System.Globalization.CultureInfo.InvariantCulture);
		return double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
	}

	public static void Dispose() {
		foreach (Font f in newFontsList)
			f.Dispose();

		foreach (DictionaryEntry de in htFamily) {
			FontFamily ff = (FontFamily) de.Value;
			ff.Dispose();
		}
	}

	// converts \uXXXXX to \uXXXX\uXXXX
	public static String ToSurrogate(String uhex) {
		String s = uhex;
		if (s.StartsWith("\\u"))
			s = s.Substring(2);
		int val = 0;
		int.TryParse(s, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out val);
		String glyph = Char.ConvertFromUtf32(val);
		String output = "";
		foreach (char c in glyph) {
			String t = ((int) c).ToString("X").PadLeft(4, '0');
			output += "\\u" + t;
		}
		return output;
	}
}

}
