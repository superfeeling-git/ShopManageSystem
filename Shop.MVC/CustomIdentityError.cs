using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.MVC
{
    public class CustomIdentityError : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = "未知错误" };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "并发失败，对象已被修改。" };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(PasswordMismatch), Description = "密码错误" };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"密码至少{uniqueChars}个不同字符" };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError { Code = nameof(InvalidToken), Description = "无效令牌" };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "用户已存在" };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = nameof(InvalidUserName), Description = "用户名无效" };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(InvalidEmail), Description = "邮箱无效" };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(DuplicateUserName), Description = "用户名已被使用" };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError { Code = nameof(DuplicateRoleName), Description = "角色名无效" };
        }
    }
}
