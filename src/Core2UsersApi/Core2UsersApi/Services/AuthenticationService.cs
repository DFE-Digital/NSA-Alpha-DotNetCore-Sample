using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2UsersApi.Services
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private static readonly UserWithPassword[] ValidUsers = new[]
        {
            new UserWithPassword
            {
                Email = "peter.pan@neverland.com",
                DisplayName = "Peter Pan",
                Password = "SecondStar2Right"
            }
        };

        public Task<User> AuthenticateAsync(string username, string password)
        {
            var user = ValidUsers.SingleOrDefault(x => x.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            if (user == null || user.Password != password)
            {
                return Task.FromResult<User>(null);
            }
            return Task.FromResult(user.DuplicateWithoutPassword());
        }

        private class UserWithPassword : User
        {
            public string Password { get; set; }

            public User DuplicateWithoutPassword()
            {
                return new User
                {
                    Email = Email,
                    DisplayName = DisplayName
                };
            }
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}
