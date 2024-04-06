using DataCrawling_Web.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DataCrawling_Web.BSL.Authentication
{
    public class ClsJWT
    {
        private string Secretkey = "252faa7c70a38ea09e34ca76fc9cb97656ac4b52e734145f809e7f7b233db0cc";

        public string GenerateToken(string Url, string UserID, string MemberType, string Name, string Phone, string Gender, int ExpiresDay)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secretkey));

            var myIssuer = "https://myplatformkorea.co.kr"; // 토큰 발급자
            var myAudience = Url; // 토큰 대상자

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserID),
                    new Claim("MemberType", MemberType),
                    new Claim("Name", Name),
                    new Claim("Phone", Phone),
                    new Claim("Gender", Gender)
                }),
                Expires = DateTime.UtcNow.AddDays(ExpiresDay),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserInfo ValidateCurrentToken(string url, string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secretkey));

            var myIssuer = "https://myplatformkorea.co.kr"; // 토큰 발급자
            var myAudience = url; // 토큰 대상자

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                UserInfo userInfo = new UserInfo()
                {
                    User_ID = jwtToken.Claims.First(x => x.Type == "UserID").Value,
                    MemberType = jwtToken.Claims.First(x => x.Type == "MemberType").Value,
                    User_Name = jwtToken.Claims.First(x => x.Type == "Name").Value,
                    Phone = jwtToken.Claims.First(x => x.Type == "Phone").Value,
                    Gender = jwtToken.Claims.First(x => x.Type == "Gender").Value
                };
                var exp = int.Parse(jwtToken.Claims.First(x => x.Type == "exp").Value);
                DateTime dt = TimeStampToDateTime(exp);
                if (dt > DateTime.Now) return userInfo;
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public DateTime TimeStampToDateTime(long value)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(value).ToLocalTime();
            return dt;
        }
    }
}