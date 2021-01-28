using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionLab
{
    public interface PermissionRepository
    {
        // Methods
        public List<Permission> FindAllByUserId(string userId);
    }
}
