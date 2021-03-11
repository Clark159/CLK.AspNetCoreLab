using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WildcardMatchLab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Variables
            var source = @"https://tw.yahoo.com/api";

            // *
            Debug.Assert(Program.RunIsWildcardMatch(source, @"*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"h*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"*h") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"i*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"*i") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/api/*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/api/") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/api*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/ap*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/a*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*w.yahoo.com/api/*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*w.*ahoo.*om/*pi/*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://t*.yaho*.co*/ap*/*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.***/*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.*/*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.*/*/") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.*./*") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.*/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.yahoo.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.*.*/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.yahoo.*/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*.*.co/api") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://t*.yahoo.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*w.yahoo.com/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://w*.yahoo.com/api") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*t.yahoo.com/api") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*/api") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*/aaa") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://*/a") == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"*://tw.*.com/*") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yah*") == true);

            // ?
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.com/??i") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yahoo.co?/ap?") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yah??.co?/ap?") == true);
            Debug.Assert(Program.RunIsWildcardMatch(source, @"https://tw.yah???.co?/ap?") == false);

            // Empty
            Debug.Assert(Program.RunIsWildcardMatch(null, null) == true);
            Debug.Assert(Program.RunIsWildcardMatch(null, string.Empty) == true);
            Debug.Assert(Program.RunIsWildcardMatch(string.Empty, null) == true);
            Debug.Assert(Program.RunIsWildcardMatch(string.Empty, string.Empty) == true);

            Debug.Assert(Program.RunIsWildcardMatch(null, "*") == true);
            Debug.Assert(Program.RunIsWildcardMatch("*", null) == false);
            Debug.Assert(Program.RunIsWildcardMatch("*", "*") == true);

            Debug.Assert(Program.RunIsWildcardMatch(source, null) == false);
            Debug.Assert(Program.RunIsWildcardMatch(source, string.Empty) == false);
        }

        public static bool RunIsWildcardMatch(string input, string pattern)
        {
            // Require
            if (string.IsNullOrEmpty(input) == true) input = string.Empty;
            if (string.IsNullOrEmpty(pattern) == true) pattern = string.Empty;

            // IsWildcardMatch
            var result = input.IsWildcardMatch(pattern);

            // Display
            Console.WriteLine($"IsMatch={result}; pattern={pattern}; input={input};");

            // Return
            return result;
        }
    }

    public static class StringExtensions
    {
        // Methods
        public static bool IsWildcardMatch(this string input, string pattern)
        {
            // Require
            if (string.IsNullOrEmpty(input) == true) input = string.Empty;
            if (string.IsNullOrEmpty(pattern) == true) pattern = string.Empty;

            // RegularPattern
            var regularPattern = "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            if (string.IsNullOrEmpty(regularPattern) == true) throw new InvalidOperationException();

            // IsMatch
            return new Regex
            (
                pattern: regularPattern,
                options: RegexOptions.IgnoreCase | RegexOptions.Singleline
            )
            .IsMatch(input);
        }
    }
}

