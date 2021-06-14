using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public interface UserRepository
    {
        // Methods
        void Add(User user);

        User FindByUserId(string userId);
    }
}
