using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPlacer
{
    public class LanguageEntry
    {
        public readonly string sheet;
        public readonly string key;
        public readonly string text;

        public LanguageEntry(string _sheet, string _key, string _text)
        {
            sheet = _sheet;
            key = _key;
            text = _text;
        }
        public override string ToString()
        {
            return $"SHEET: {sheet};  KEY: {key};  TEXT: {text}";
        }
    }
}
