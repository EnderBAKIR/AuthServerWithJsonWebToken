using Auth.CoreLayer.Configuration;
using Auth.CoreLayer.DTOs;
using Auth.CoreLayer.Models;
using Auth.CoreLayer.Services;
using Auth.SharedLibrary.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceLayer.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOptions _customTokenOptions;

        public TokenService(UserManager<UserApp> userManager, IOptions< CustomTokenOptions> customTokenOptions)
        {
            _userManager = userManager;
            _customTokenOptions = customTokenOptions.Value;
        }

        private string CreateRehfreshToken()
        {
            var numberByte= new byte[32];
            using var rnd = RandomNumberGenerator.Create();
           
            rnd.GetBytes(numberByte);
          
            return Convert.ToBase64String(numberByte);
            
            

        }

        private IEnumerable<Claim> GetClaims(UserApp userApp , List<string> auidences)
        {
            var userList = new List<Claim>();
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id);
                new Claim(JwtRegisteredClaimNames.Email, userApp.Email);
                new Claim(ClaimTypes.Email, userApp.Email);
                new Claim(ClaimTypes.Name, userApp.UserName);
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString());
            };
            userList.AddRange(auidences.Select(x=> new Claim(JwtRegisteredClaimNames.Aud, x)));

            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();

            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());
           
            return claims;
        }


        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccesTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
            
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(userApp, _customTokenOptions.Audience),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRehfreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,


            };
            return tokenDto;
                

        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccesTokenExpiration);
            
            var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _customTokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaimsByClient(client),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new ClientTokenDto
            {
                AccessToken = token,
               
                AccessTokenExpiration = accessTokenExpiration,
                


            };
            return tokenDto;
        }
    }
}
