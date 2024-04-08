using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.LoginService;
using Flight.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Flight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly LoginValidation validations;

        public LoginController(ILoginService loginService,LoginValidation validations) 
        {
            this.loginService = loginService;
            this.validations = validations;
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            try
            {
                var result = validations.Validate(model);
                if(!result.IsValid)
                {
                    return BadRequest(Repository<Dictionary<string, string>>.WithMessage(result.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage), 400));
                }
                var login = await loginService.SignIn(model);
                if(login==null)
                {
                    return BadRequest(Repository<string>.WithMessage("Sai mật khẩu hoặc email", 400));
                }
                return Ok(Repository<LoginDTO>.WithData(login,200));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
