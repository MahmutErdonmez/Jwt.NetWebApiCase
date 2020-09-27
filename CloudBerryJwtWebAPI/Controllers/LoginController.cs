using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CloudBerryJwtWebAPI.Model;
using CloudBerryJwtWebAPI.Operation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CloudBerryJwtWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private IDataLayer _dataLayer;
        public LoginController(IConfiguration config, IDataLayer dataLayer)
        {
            _config = config;
            _dataLayer = dataLayer;
        }

        [HttpGet]
        public IActionResult Login(string email, string pass)
        {
            IActionResult response = Unauthorized();

            var loginRequest = new AdminModel
            {
                Email = email,
                Password = pass
            };

            var userResult = _dataLayer.CheckAdmin(loginRequest);
            if (userResult.Key)
            {
                var tokenStr = GenerateJSONWebToken(userResult.Value);
                response = Ok(new { token = tokenStr });
            }
            return response;
        }

        private string GenerateJSONWebToken(AdminModel userinfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub,userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email,userinfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120), //time
                signingCredentials: credentials
                );

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        [Authorize]
        [HttpPost("Post")]
        public ActionResult<string> Post()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var adminName = claim[0].Value;
            return "Welcome To :" + adminName;
        }

        [Authorize]
        [HttpGet("List")]
        public ActionResult<IEnumerable<AdminModel>> List()
        {
            return _dataLayer.AdminList();
        }

        [Authorize]
        [HttpPost("Create")]
        public ActionResult<string> Create(string firstName, string lastName, string email, string phone)
        {
            var createRequest = new UserModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            };
            var result = _dataLayer.CreateUser(createRequest);
            return result.Value;
        }

        [Authorize]
        [HttpDelete("Delete")]
        public ActionResult<string> Delete(int id)
        {
            var result = _dataLayer.DeleteUser(id);
            return result.Value;
        }

        [Authorize]
        [HttpPut("Update")]
        public ActionResult<string> Update(UserModel userModel)
        {
            var result = _dataLayer.UpdateUser(userModel);
            return result.Value;
        }
    }
}
