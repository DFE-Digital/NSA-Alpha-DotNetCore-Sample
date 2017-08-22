using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core2UsersApi.Models;
using Core2UsersApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Core2UsersApi.Controllers
{
    [Route("[controller]")]
    public class TokenController : Controller
    {
        public const string Issuer = "Core2UserApi";
        public const string Key = "new-secure-access";

        private readonly IAuthenticationService _authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> GetToken([FromBody]UserCredentialsModel model)
        {
            var user = await _authenticationService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.DisplayName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((string)Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Issuer,
                Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}