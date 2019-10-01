using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JasonWebToken.Models;
using JasonWebToken.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace JasonWebToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private List<Person> people = new List<Person>()
        {
            new Person(){Login = "mann@gmail.com", Password = "123123", Role = "admin"},
            new Person(){Login = "ivanov@gmail.com", Password = "234234", Role = "superadmin"},
            new Person(){Login = "ferber@gmail.com" , Password = "345345" , Role = "user"}
        };



        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Testing is succeeded");
        }


        [HttpPost]
        public IActionResult GetToken()
        {
            // This should be true so that to show personla information in logging
            if (IdentityModelEventSource.ShowPII == false)
                IdentityModelEventSource.ShowPII = true;

            var username = Request.Form["username"];
            var password = Request.Form["password"];


            ClaimsIdentity identity = GetIdentity(username, password);

            if (identity == null)
            {
                return NotFound();
            }

            var now = DateTime.UtcNow;

            var jwtToken = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(key: AuthOptions.SymmetricSecurityKey,
                                                     algorithm: AuthOptions.Algorithm));

            string encdodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var responseData = new
            {
                accessToken = encdodedJwt,
                username = identity.Name
            };

            return Ok(responseData);
        }





        private ClaimsIdentity GetIdentity(string username, string password)
        {
            // simulate fetching user from database
            var person = people.FirstOrDefault(p => p.Password == password &&
                                                    p.Login == username);

            if (person != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType) ;
                return identity;
            }

            // if user is not found in database
            return null;

        }

    }
}
