using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationPermissionLab
{
    public class MockOperationPermissionRepository : OperationPermissionRepository
    {
        // Methods
        public List<OperationPermission> FindAllByUserName(string userName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userName) == true) throw new ArgumentException(nameof(userName));

            #endregion

            // Result
            var operationPermissionList = new List<OperationPermission>();

            // UserName
            switch (userName)
            {
                // Clark
                case "Clark":
                    operationPermissionList.Add(new OperationPermission() { UserName = "Clark", OperationName = "Home.Add" });
                    operationPermissionList.Add(new OperationPermission() { UserName = "Clark", OperationName = "Home.Remove" });
                    operationPermissionList.Add(new OperationPermission() { UserName = "Clark", OperationName = "Home.Update" });
                    break;

                // Jane
                case "Jane":
                    operationPermissionList.Add(new OperationPermission() { UserName = "Jane", OperationName = "Home.Add" });
                    break;
            }

            // Return
            return operationPermissionList;
        }
    }
}
