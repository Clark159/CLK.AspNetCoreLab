using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public class UserContext
    {
        // Constructors
        public UserContext(UserRepository userRepository, UserLoginRepository userLoginRepository)
        {
            #region Contracts

            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));

            #endregion

            // Default
            this.UserRepository = userRepository;
            this.UserLoginRepository = userLoginRepository;
        }


        // Properties
        public UserRepository UserRepository { get; } = null;

        public UserLoginRepository UserLoginRepository { get; } = null;


        // Methods
        public User FindUser(string userId, string loginType, string loginCode)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginCode) == true) throw new ArgumentException(nameof(loginCode));

            #endregion

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByUserId(userId, loginType);
            if (userLogin == null) return null;
            if (userLogin.LoginCode != loginCode) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }

        public User FindUser(string loginType, string loginCode)
        {
            #region Contracts

            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginCode) == true) throw new ArgumentException(nameof(loginCode));

            #endregion

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(loginType, loginCode);
            if (userLogin == null) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }
    }
}
