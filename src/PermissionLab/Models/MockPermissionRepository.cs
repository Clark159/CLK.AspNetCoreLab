using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionLab
{
    public class MockPermissionRepository : PermissionRepository
    {
        // Methods
        public List<Permission> FindAllByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Result
            var permissionList = new List<Permission>();

            // Clark
            switch(userId)
            {
                // Clark
                case "0001":
                    permissionList.Add(new Permission() { PathPatten = @"/Home/*" });
                    break;

                // Jane
                case "0002":
                    permissionList.Add(new Permission() { PathPatten = @"/Home/GetUser001" });
                    break;
            }

            // Return
            return permissionList;
        }
    }
}
