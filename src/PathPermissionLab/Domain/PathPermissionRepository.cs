using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathPermissionLab
{
    public interface PathPermissionRepository
    {
        // Methods
        public List<PathPermission> FindAllByUserName(string userName);
    }
}
