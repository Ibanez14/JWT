using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JasonWebToken.Options
{
    public class AuthOptions
    {
        private static readonly SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        public const string ISSUER = "DemoIssuer";
        public const string AUDIENCE = "http://localhost:5000";
        const string KEY = "secret_key THAT SHOULD BE GREATER THAN 128 BYTES!";
        public const int LIFETIME = 1;

        public static string Algorithm
        {
            get => SecurityAlgorithms.HmacSha256;
        }
        public static SymmetricSecurityKey SymmetricSecurityKey
        {
            get => key;
        }
    }
}
