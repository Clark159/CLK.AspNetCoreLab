using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public class MockUserRepository : UserRepository
    {
        // Fields
        private readonly List<User> _userList = new List<User>();


        // Constructors
        public MockUserRepository()
        {
            // Default

        }


        // Methods
        public void Add(User user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion

            // Remove
            _userList.RemoveAll(o => o.UserId == user.UserId);

            // Add
            _userList.Add(user);            
        }

        public User FindByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Find
            return _userList.FirstOrDefault(o => o.UserId == userId);
        }
    }
}
