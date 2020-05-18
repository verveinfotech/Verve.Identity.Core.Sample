using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Verve.Identity.Core.Service.Abstraction;
using Verve.Identity.Core.Service.Configuration;
using Verve.Identity.Core.Sample.Entity;
using Verve.Identity.Core.Sample.Web.Request;
using Verve.Identity.Core.Sample.Web.Response;

namespace Verve.Identity.Core.Sample.Web.Service
{


    public class UserService : IUserService
    {
        private readonly IVerveIdentityService<UserAccount> _identityService;

        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IdentitySettings _identitySettings;
        public UserService(IVerveIdentityService<UserAccount> identityService,
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            IOptionsMonitor<IdentitySettings> identityOptions)
        {
            _identityService = identityService;
            _userManager = userManager;
            _signInManager = signInManager;
            _identitySettings = identityOptions.CurrentValue;
        }


        public async Task<UserRegisterationResponse> Register(UserRegistrationRequest registrationRequest)
        {
            var account = new UserAccount
            {
                Id = Guid.NewGuid(),
                UserName = registrationRequest.UserName,
                Email = registrationRequest.Email,
                PhoneNumber = registrationRequest.PhoneNo,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Name = registrationRequest.Name,
                Status = 0,

            };

            var cancellationToken = new CancellationToken();

            await _identityService.SetSecurityStampAsync(account, Guid.NewGuid().ToString(), cancellationToken);

            var newAccountResult = await _userManager.CreateAsync(account, registrationRequest.Password);

            if (!newAccountResult.Succeeded)
            {
                return new UserRegisterationResponse
                {
                    ErrorMessage = string.Join("; ", newAccountResult.Errors.Select(x=>x.Description))
                };
            }
            
            if (newAccountResult.Succeeded)
            {
                return new UserRegisterationResponse
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Email = account.Email,
                    PhoneNo = account.PhoneNumber,
                    Success = true
                };
            }

            return null;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var userAccount = await _userManager.FindByNameAsync(loginRequest.Username);

            if (userAccount == null)
            {
                return null;
            }


            var signIn = await _signInManager.PasswordSignInAsync(userAccount, loginRequest.Password, false, true);

            if (!signIn.Succeeded)
            {
                return null;
            }


            var loginResponse = new LoginResponse
            {
                Email = userAccount.Email,
                Username = userAccount.UserName,
                Roles = await _userManager.GetRolesAsync(userAccount),
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_identitySettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userAccount.UserName),
                        new Claim(ClaimTypes.Role, string.Join(",", loginResponse.Roles)),
                        new Claim(ClaimTypes.Email, loginResponse.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            loginResponse.AccessToken = tokenHandler.WriteToken(token);

            return loginResponse;
        }
    }
}