using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using hr_master.Db;
using hr_master.Models.Form;
using hr_master.Models.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("cross")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public AuthController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost]
        public IActionResult EmployeeLogin([FromBody] EmployeeLogin form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "please provide correct informations"
                });

            var response = _context.EmployessUsers.Where(x => x.Employee_Email == form.Employee_NameOrMail || x.Employee_Name == form.Employee_NameOrMail).FirstOrDefault();
            if (response == null)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in login"
                });


            var role = _context.Teams.Where(x => x.Id == response.Employee_Team).FirstOrDefault();

            if (form.Employee_Password != response.Employee_Password)
            {
                return Unauthorized(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in username and/or password"
                });
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
                new Claim("UserName", response.Employee_Fullname),
                new Claim(ClaimTypes.Role, role.Team_Roles),
                new Claim("UserId" , response.Id.ToString()),
               

            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                notBefore: DateTime.UtcNow,
                audience: "Audience",
                issuer: "Issuer",
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-Hsau1")),
                    SecurityAlgorithms.HmacSha256)
            );
            return Ok(new Response
            {
                Error = false,
                Message = role.Team_Roles,
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        public IActionResult AdminLogin([FromBody] AdminLogin form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "please provide correct informations"
                });

            var response = _context.AdminUser.Where(x => x.User_Mail == form.Admin_NameOrMail || x.User_Name == form.Admin_NameOrMail).FirstOrDefault();
            if (response == null)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in login"
                });



            if (form.Admin_Password != response.User_Password)
            {
                return Unauthorized(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in username and/or password"
                });
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
                new Claim("UserName", response.User_Firstname),
                new Claim(ClaimTypes.Role, response.User_Level)
            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                notBefore: DateTime.UtcNow,
                audience: "Audience",
                issuer: "Issuer",
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-Hsau1")),
                    SecurityAlgorithms.HmacSha256)
            );
            return Ok(new Response
            {
                Error = false,
                Message = response.User_Level,
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }


        [HttpPost]
        public IActionResult InternetUserLogin([FromBody] InternetUserLogin form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "please provide correct informations"
                });

            var response = _context.InternetUsers.Where(x => x.User_Name == form.User_NameOrMail && x.IsDelete == false).FirstOrDefault();
            if (response == null)
                return BadRequest(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in login"
                });



            if (form.User_Password != response.User_Password)
            {
                return Unauthorized(new Response
                {
                    Data = null,
                    Error = true,
                    Message = "error in username and/or password"
                });
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
                new Claim("UserName", response.User_FullName),
                new Claim(ClaimTypes.Role, "InternetUser")
            };

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                notBefore: DateTime.UtcNow,
                audience: "Audience",
                issuer: "Issuer",
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-Hsau1")),
                    SecurityAlgorithms.HmacSha256)
            );
            return Ok(new AuthResponse
            {
                Error = false,
                external_id = response.Id.ToString(),
                Tower = response.User_Tower,
                Role = "InternetUser",
                Data = new JwtSecurityTokenHandler().WriteToken(token)
            }) ;
        }




    }
}
