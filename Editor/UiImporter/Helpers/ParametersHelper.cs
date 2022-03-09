using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Library.Editor
{
    public static class ParametersHelper
    {
        public static bool IsStringContainsParameter(string sourceString, string pattern)
        {
            return Regex.IsMatch(sourceString, pattern);
        }
        
        
        public static bool TryGetIntNumberFromParameters(string sourceString, string pattern, int numberIndex, out int number)
        {
            var stringNumber = GetStringNumberFromParameters(sourceString, pattern, numberIndex);
            if (stringNumber == string.Empty)
            {
                number = default;
                return false;
            }

            number = Convert.ToInt32(stringNumber, CultureInfo.InvariantCulture);
            return true;
        }


        public static bool TryGetFloatNumberFromParameters(string sourceString, string pattern, int numberIndex, out float number)
        {
            var stringNumber = GetStringNumberFromParameters(sourceString, pattern, numberIndex);
            if (stringNumber == default)
            {
                number = default;
                return false;
            }

            number = Convert.ToSingle(stringNumber, CultureInfo.InvariantCulture);
            return true;
        }
        
        
        private static string GetStringNumberFromParameters(string sourceString, string pattern, int numberIndex)
        {
            var matches = Regex.Matches(sourceString, pattern);
            if (matches.Count > 0)
            {
                var numbers = Regex.Matches(matches[0].Value, "[+-]?([0-9]*[.])?[0-9]+");
                if (numbers.Count > numberIndex)
                {
                    return numbers[numberIndex].Value;
                }
            }

            return default;
        }   
    }
}