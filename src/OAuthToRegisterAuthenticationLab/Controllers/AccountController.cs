using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OAuthToRegisterAuthenticationLab
{
    public partial class AccountController : Controller
    {
        // Fields
        private readonly UserContext _userContext = null;


        // Constructors
        public AccountController(UserContext userContext)
        {
            #region Contracts

            if (userContext == null) throw new ArgumentException(nameof(userContext));

            #endregion

            // Default
            _userContext = userContext;
        }


        // Methods
        private string ComputeHash(string input = null, HashAlgorithm hashAlgorithm = null)
        {
            // HashAlgorithm
            if (hashAlgorithm == null) hashAlgorithm = SHA256.Create();

            // InputBytes
            byte[] inputBytes = null;
            if (string.IsNullOrEmpty(input) == true) inputBytes = new byte[0];
            if (string.IsNullOrEmpty(input) == false) inputBytes = Encoding.UTF8.GetBytes(input);

            // HashBytes
            var hashBytes = hashAlgorithm.ComputeHash(inputBytes);
            if (hashBytes == null) throw new InvalidOperationException($"{nameof(hashBytes)}=null");

            // HashString
            var hashStringBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashStringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            // Return
            return hashStringBuilder.ToString();
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl = @"/")
        {
            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // SignOut
            await this.HttpContext.SignOutAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme);
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // View
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            // Require
            if (this.User.Identity.IsAuthenticated == false) return this.Redirect(@"/");

            // SignOut
            await this.HttpContext.SignOutAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme);
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect
            return this.Redirect(@"/");
        }


        // Class
        public class LoginViewModel
        {
            // Properties
            public string UserId { get; set; } = "User001";

            public string Password { get; set; } = null;

            public string Message { get; set; } = null;
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLogin(string authenticationScheme, string returnUrl = @"/")
        {
            #region Contracts

            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);
            if (authenticationScheme == ApplicationCookieAuthenticationDefaults.AuthenticationScheme) throw new InvalidOperationException($"{nameof(authenticationScheme)}={ApplicationCookieAuthenticationDefaults.AuthenticationScheme}");
            if (authenticationScheme == ExternalCookieAuthenticationDefaults.AuthenticationScheme) throw new InvalidOperationException($"{nameof(authenticationScheme)}={ExternalCookieAuthenticationDefaults.AuthenticationScheme}");

            // SignOut
            await this.HttpContext.SignOutAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme);
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // RedirectUri
            var redirectUri = this.Url.Action(nameof(ExternalSignIn), new { returnUrl = returnUrl });
            if (string.IsNullOrEmpty(redirectUri) == true) throw new InvalidOperationException($"{nameof(redirectUri)}=null");

            // Challenge
            return this.Challenge(new AuthenticationProperties() { RedirectUri = redirectUri }, authenticationScheme);
        }

        [Authorize(AuthenticationSchemes = ExternalCookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ExternalSignIn(string returnUrl = @"/")
        {
            // ClaimsIdentity
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}==null");

            // Validate
            User user = null;
            {
                // LoginType
                var loginType = claimsIdentity.AuthenticationType;
                if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

                // LoginCode
                var loginCode = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(loginCode) == true) throw new InvalidOperationException($"{nameof(loginCode)}=null");

                // Find
                user = _userContext.FindUser(loginType, loginCode);
                if (user == null)
                {
                    // Register
                    return this.RedirectToAction(nameof(Register), new { returnUrl = returnUrl });
                }
            }

            // ApplicationIdentity
            var applicationIdentity = new ApplicationIdentity(claimsIdentity.AuthenticationType);
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserIdType, user.UserId));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserNameType, user.UserName));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.NickNameType, user.NickName));

            // SignIn
            await this.HttpContext.SignInAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(applicationIdentity));
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect
            return this.Redirect(returnUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> PasswordSignIn(string userId, string password = null, string returnUrl = @"/")
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Validate
            User user = null;
            {
                // LoginType
                var loginType = PasswordAuthenticationDefaults.AuthenticationScheme;
                if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

                // LoginCode
                var loginCode = this.ComputeHash(password);
                if (string.IsNullOrEmpty(loginCode) == true) throw new InvalidOperationException($"{nameof(loginCode)}=null");

                // Find
                user = _userContext.FindUser(userId, loginType, loginCode);
                if (user == null)
                {
                    // ActionModel
                    var actionModel = new LoginViewModel();
                    actionModel.UserId = userId;
                    actionModel.Password = null;
                    actionModel.Message = "Login Failure";

                    // Login
                    return this.View(nameof(Login), actionModel);
                }
            }

            // ApplicationIdentity
            var applicationIdentity = new ApplicationIdentity(PasswordAuthenticationDefaults.AuthenticationScheme);
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserIdType, user.UserId));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserNameType, user.UserName));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.NickNameType, user.NickName));

            // SignIn
            await this.HttpContext.SignInAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(applicationIdentity));
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect
            return this.Redirect(returnUrl);
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(string returnUrl = @"/")
        {
            // UserIndex => ForTest
            int userIndex = 1;
            for (userIndex = 1; userIndex <= int.MaxValue; userIndex++)
            {
                // User
                var user = _userContext.UserRepository.FindByUserId(string.Format("User{0:000}", userIndex));
                if (user == null) break;
            }

            // ViewModel
            var viewModel = new RegisterViewModel();
            viewModel.UserId = string.Format("User{0:000}", userIndex);
            viewModel.UserName = string.Format("Clark{0:000}", userIndex);
            viewModel.NickName = string.Format("昏睡少尉{0:000}", userIndex);

            // View
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(string userId = null, string userName = null, string nickName = null, string password = null, string returnUrl = @"/")
        {
            // ViewModel
            var viewModel = new RegisterViewModel();
            viewModel.UserId = userId;
            viewModel.UserName = userName;
            viewModel.NickName = nickName;
            viewModel.Password = null;

            // Validation
            if (string.IsNullOrEmpty(userId)==true) { viewModel.Message = $"{nameof(userId)}=null"; return View(viewModel); }
            if (string.IsNullOrEmpty(userName) == true) { viewModel.Message = $"{nameof(userName)}=null"; return View(viewModel); }
            if (string.IsNullOrEmpty(nickName) == true) { viewModel.Message = $"{nameof(nickName)}=null"; return View(viewModel); }
            if (_userContext.UserRepository.FindByUserId(userId) != null) { viewModel.Message = "UserId Existed"; return View(viewModel); }

            // User
            User user = null;
            {
                // Create
                user = new User();
                user.UserId = userId;
                user.UserName = userName;
                user.NickName = nickName;

                // Add
                _userContext.UserRepository.Add(user);
            }

            // PasswordUserLogin
            UserLogin passwordUserLogin = null;
            {
                // Create
                passwordUserLogin = new UserLogin();
                passwordUserLogin.UserId = userId;
                passwordUserLogin.LoginType = PasswordAuthenticationDefaults.AuthenticationScheme;
                passwordUserLogin.LoginCode = this.ComputeHash(password);

                // Add
                _userContext.UserLoginRepository.Add(passwordUserLogin);
            }

            // ExternalUserLogin
            UserLogin externalUserLogin = null;
            {
                // ExternalSignIn
                var authenticateResult = await this.HttpContext.AuthenticateAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
                if (authenticateResult?.Succeeded == true && authenticateResult?.Principal?.Identity?.IsAuthenticated == true)
                {
                    // ClaimsIdentity
                    var claimsIdentity = authenticateResult?.Principal?.Identity as ClaimsIdentity;
                    if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}=null");

                    // LoginType
                    var loginType = claimsIdentity.AuthenticationType;
                    if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

                    // LoginCode
                    var loginCode = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(loginCode) == true) throw new InvalidOperationException($"{nameof(loginCode)}=null");

                    // Create
                    externalUserLogin = new UserLogin();
                    externalUserLogin.UserId = userId;
                    externalUserLogin.LoginType = loginType;
                    externalUserLogin.LoginCode = loginCode;

                    // Add
                    _userContext.UserLoginRepository.Add(externalUserLogin);
                }
            }

            // AuthenticationType
            string authenticationType = null;
            if (passwordUserLogin != null) authenticationType = passwordUserLogin.LoginType;
            if (externalUserLogin != null) authenticationType = externalUserLogin.LoginType;
            if (string.IsNullOrEmpty(authenticationType) == true) throw new InvalidOperationException($"{nameof(authenticationType)}=null");

            // ApplicationIdentity
            var applicationIdentity = new ApplicationIdentity(authenticationType);
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserIdType, user.UserId));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.UserNameType, user.UserName));
            applicationIdentity.AddClaim(new Claim(applicationIdentity.NickNameType, user.NickName));

            // SignIn
            await this.HttpContext.SignInAsync(ApplicationCookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(applicationIdentity));
            await this.HttpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect
            return this.Redirect(returnUrl);
        }


        // Class
        public class RegisterViewModel
        {
            // Properties
            public string UserId { get; set; } = null;

            public string UserName { get; set; } = null;

            public string NickName { get; set; } = null;

            public string Password { get; set; } = null;

            public string Message { get; set; } = null;
        }
    }
}
