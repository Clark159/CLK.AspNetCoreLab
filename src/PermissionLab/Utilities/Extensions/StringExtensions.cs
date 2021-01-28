using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PermissionLab
{
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
