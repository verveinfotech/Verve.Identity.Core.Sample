using System.Threading.Tasks;
using Verve.Identity.Core.Sample.Web.Request;
using Verve.Identity.Core.Sample.Web.Response;

namespace Verve.Identity.Core.Sample.Web.Service
{
    public interface IUserService
    {
        Task<UserRegisterationResponse> Register(UserRegistrationRequest registrationRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    }
}