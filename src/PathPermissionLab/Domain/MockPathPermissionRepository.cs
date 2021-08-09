using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathPermissionLab
{
    public class MockPathPermissionRepository : PathPermissionRepository
    {
        // Methods
        public List<PathPermission> FindAllByUserName(string userName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userName) == true) throw new ArgumentException(nameof(userName));

            #endregion

            // Result
            var pathPermissionList = new List<PathPermission>();
            pathPermissionList.Add(new PathPermission() { UserName = userName, PathPatten = @"/" });
            pathPermissionList.Add(new PathPermission() { UserName = userName, PathPatten = @"/Home" });
            pathPermissionList.Add(new PathPermission() { UserName = userName, PathPatten = @"/Home/Index" });

            // UserName
            switch (userName)
            {
                // Clark
                case "Clark":                    
                    pathPermissionList.Add(new PathPermission() { UserName = "Clark", PathPatten = @"/Home/*" });
                    break;

                // Jane
                case "Jane":
                    pathPermissionList.Add(new PathPermission() { UserName = "Jane", PathPatten = @"/Home/Add" });
                    break;
            }

            // Return
            return pathPermissionList;
        }
    }
}
