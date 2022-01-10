using System;
using System.Linq;
using System.Text.RegularExpressions;


namespace Library
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string sourceString)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(sourceString, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
        
        
        public static string[] SplitCamelCase(this string sourceString)
        {
            return sourceString.ToCamelCaseWithSpaces().Split(' ');
        }


        public static string ToCamelCaseWithSpaces(this string sourceString)
        {
            string temp = Regex.Replace(sourceString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
            return Regex.Replace(temp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
        }


        public static string AsNameInWords(this string sourceString)
        {
            var nameInWords = Regex.Replace(sourceString, "(\\B[A-Z])", " $1");
            if (nameInWords.Length > 0)
            {
                nameInWords = char.ToUpper(nameInWords[0]) + nameInWords.Substring(1);
            }
            return nameInWords;
        }


        public static int GetLevenshteinDistance(this string source, string variant)
        {
            int sourceLength = source.Length;
            int variantLength = variant.Length;
            int[,] matrix = new int[sourceLength + 1, variantLength + 1];

            if (sourceLength == 0)
            {
                return variantLength;
            }

            if (variantLength == 0)
            {
                return sourceLength;
            }

            for (int i = 0; i <= sourceLength; i++)
            {
                matrix[i, 0] = i;
            }

            for (int j = 0; j <= variantLength; j++)
            {
                matrix[0, j] = j;
            }

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= variantLength; j++)
                {
                    int cost = (variant[j - 1] == source[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[sourceLength, variantLength];
        }


        public static string FixLineEndings(this string source)
        {
            return Regex.Replace(source, "(?<!\r)\n", AssetsSettings.LineEnding);
        }
    }
}