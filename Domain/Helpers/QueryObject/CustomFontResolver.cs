using PdfSharp.Fonts;
using System;
using System.IO;

namespace Domain.Helpers.QueryObject
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            var fontPath = "C:/Users/Terrin Tin/Downloads/cour.ttf"; // Убедитесь, что путь правильный
            if (!File.Exists(fontPath))
                throw new InvalidOperationException($"Font file not found: {fontPath}");

            return File.ReadAllBytes(fontPath);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Courier New", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Courier");
            }

            return null; // Если запрашивается другой шрифт, оставляем null (PdfSharp может выбрать дефолтный)
        }

        public string DefaultFontName => "Courier New";
    }
}
