using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public interface UserLoginRepository
    {
        // Methods
        void Add(UserLogin userLogin);

        UserLogin FindByUserId(string userId, string loginType);

        UserLogin FindByLoginType(string loginType, string loginCode);
    }
}
