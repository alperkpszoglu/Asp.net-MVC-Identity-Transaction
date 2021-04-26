using Microsoft.AspNet.Identity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentityExample.Identity
{
    public class CustomValidator:PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);

            if (password.Contains("12345")) //for example
            {
                var errors = result.Errors.ToList();
                errors.Add("Parolaya ardışık sayılar girmeyiniz");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}