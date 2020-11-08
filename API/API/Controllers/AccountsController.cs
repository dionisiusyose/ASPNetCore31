using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.DapperRepository;
using API.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IDapper dapper;

        public AccountsController(IConfiguration configuration, IDapper dapper)
        {
            this.configuration = configuration;
            this.dapper = dapper;
        }

        [HttpPost(nameof(Login))]
        public async Task<string> Login(UserRoleVM userRoleVM)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("@UserEmail", userRoleVM.UserEmail, DbType.String);
            dbparams.Add("@UserPassword", userRoleVM.UserPassword, DbType.String);
            var result = await Task.FromResult(dapper.Get<UserRoleVM>("[dbo].[SP_LoginUser]",
                dbparams, commandType: CommandType.StoredProcedure));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("RoleName", result.RoleName),
                    new Claim("UserEmail", result.UserEmail)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
            //return result;
        }
    }
}
