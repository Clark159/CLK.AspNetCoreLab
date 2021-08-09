using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PathPermissionLab
{
    public class PathPermission
    {
        // Properties
        public string UserName { get; set; }

        public string PathPatten { get; set; }


        // Methods
        public bool IsMatch(string path = null)
        {
            // Require
            if (string.IsNullOrEmpty(path) == true) path = string.Empty;
            if (string.IsNullOrEmpty(this.PathPatten) == true) this.PathPatten = string.Empty;

            // RegularPattern
            var regularPattern = "^" + Regex.Escape(this.PathPatten).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            if (string.IsNullOrEmpty(regularPattern) == true) throw new InvalidOperationException();

            // IsMatch
            return new Regex
            (
                pattern: regularPattern,
                options: RegexOptions.IgnoreCase | RegexOptions.Singleline
            )
            .IsMatch(path);
        }
    }
}
