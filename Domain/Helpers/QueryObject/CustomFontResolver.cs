using PdfSharp.Fonts;
using System;
using System.IO;

namespace Domain.Helpers.QueryObject
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            var fontPath = "../fonts/cour.ttf";
            if (!File.Exists(fontPath))
                throw new InvalidOperationException($"Font file not found: {fontPath}");

            return File.ReadAllBytes(fontPath);
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Courier New", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Courier");
            }

            return null; 
        }

        public string DefaultFontName => "Courier New";
    }
}
