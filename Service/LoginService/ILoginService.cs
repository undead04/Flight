using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Service.LoginService
{
    public interface ILoginService
    {
        Task<LoginDTO?> SignIn(SignInModel model);
        Task<string> CreateToken(SignInModel model);
    }
}
