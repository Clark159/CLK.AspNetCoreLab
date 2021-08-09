using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperationPermissionLab
{
    public interface OperationPermissionRepository
    {
        // Methods
        public List<OperationPermission> FindAllByUserName(string userName);
    }
}
