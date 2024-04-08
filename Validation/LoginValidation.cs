using Flight.Model;
using FluentValidation;
using System.Data;

namespace Flight.Validation
{
    public class LoginValidation:AbstractValidator<SignInModel>
    {
        public LoginValidation() 
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Không được trống")
                .Must(IsEmail).WithMessage("Phải đúng định dạng @vietjetair.com");
            RuleFor(x => x.Password).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Không được trống")
                .Must(IsLengthEightChar).WithMessage("Phải đủ 8 kí tự trở lên");
        }
        private bool IsLengthEightChar(string password)
        {
            if(password.Length>=8)
            {
                return true;
            }
            return false;
        }
        private bool IsEmail(string email)
        {
            if (email.EndsWith("@vietjetair.com"))
            {
                return true;
            }
            return false;
        }
    }
}
