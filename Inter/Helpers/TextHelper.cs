using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inter.Helpers
{
    public static class TextHelper
    {
        public static string GenerateRandomWord()
        {
            const int length = 8;
            
            var random = new Random();
            var sb = new StringBuilder();

            for (var i = 0; i < length; ++i)
                sb.Append((char)('a' + random.Next(0, 26)));

            return sb.ToString();
        }

        public static string EditPostText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var patterns = new [] { "\\t+", "  +", "(\\r\\n )+|( \\r\\n)+", "\\r\\n(\\r\\n)+" };
            var insertSymbol = new[] { "", " ", "\r\n", "\r\n" };
            
            for (var i = 0; i < patterns.Length; ++i)
            {
                var regex = new Regex(patterns[i], RegexOptions.Singleline);
                var match = regex.Match(text);
                
                while (match.Success)
                {
                    text = match.Index + match.Length + 1 < text.Length 
                        ? text[..match.Index] + insertSymbol[i] + text[(match.Index + match.Length)..]
                        : text[..match.Index];
                    match = regex.Match(text, match.Index);
                }
            }

            text = text.Trim();

            return text;
        }

        public static string EditThreadName(string name, string text = null)
        {
            var nullOrEmptyName = string.IsNullOrEmpty(name);

            if (nullOrEmptyName)
                name = string.Empty;
            
            name = name.Trim();
            
            if (name.Length > ConstHelper.MaxNameLength)
                return name[..(ConstHelper.MaxNameLength - 3)] + "...";

            return nullOrEmptyName switch
            {
                true when !string.IsNullOrEmpty(text) => text.Length > ConstHelper.MaxNameLength 
                    ? text[..(ConstHelper.MaxNameLength - 3)] + "..."
                    : text,
                true => ConstHelper.RandomThreadName,
                _ => name
            };
        }
    }
}