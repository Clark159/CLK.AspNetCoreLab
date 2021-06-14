using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public class MockUserLoginRepository : UserLoginRepository
    {
        // Fields
        private readonly List<UserLogin> _userLoginList = new List<UserLogin>();


        // Constructors
        public MockUserLoginRepository()
        {
            // Default

        }


        // Methods
        public void Add(UserLogin userLogin)
        {
            #region Contracts

            if (userLogin == null) throw new ArgumentException(nameof(userLogin));

            #endregion

            // Remove
            _userLoginList.RemoveAll(o => o.UserId == userLogin.UserId && o.LoginType == userLogin.LoginType);

            // Add
            _userLoginList.Add(userLogin);            
        }

        public UserLogin FindByUserId(string userId, string loginType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));

            #endregion

            // Find
            return _userLoginList.FirstOrDefault(o => o.UserId == userId && o.LoginType == loginType);
        }

        public UserLogin FindByLoginType(string loginType, string loginCode)
        {
            #region Contracts

            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginCode) == true) throw new ArgumentException(nameof(loginCode));

            #endregion

            // Find
            return _userLoginList.FirstOrDefault(o => o.LoginType == loginType && o.LoginCode == loginCode);
        }
    }
}
