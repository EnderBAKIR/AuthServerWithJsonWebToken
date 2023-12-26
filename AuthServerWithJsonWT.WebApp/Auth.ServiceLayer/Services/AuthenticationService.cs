using Auth.CoreLayer.Configuration;
using Auth.CoreLayer.DTOs;
using Auth.CoreLayer.Models;
using Auth.CoreLayer.Repositories;
using Auth.CoreLayer.Services;
using Auth.CoreLayer.UnitOfWork;
using Auth.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshtoken;

        public AuthenticationService(IOptions< List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshtoken)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshtoken = userRefreshtoken;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(SignInDto signInDto)//Kullanıcı için token , token for user.
        {
            if (signInDto==null) throw new NotImplementedException(nameof(signInDto));

            var user = await _userManager.FindByEmailAsync(signInDto.Email);//check mail

            if (user == null) return Response<TokenDto>.Fail("Email or Password wrong" , 400 , true);

            if (!await _userManager.CheckPasswordAsync(user , signInDto.Password))//check password
                {

                return Response<TokenDto>.Fail("Email or Password wrong", 400, true);
            }

            var token = _tokenService.CreateToken(user);//mail ve şifre doğru ise token oluştur , if mail and pw true creare token.

            var userRefreshToken = await _userRefreshtoken.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();//kullanıcının refresh tokeni varmı diye bak
                                                                                                                   //check user have resfreh token
            if (userRefreshToken == null)//yok ise yeni token oluştur , if not create new token
            {
                await _userRefreshtoken.AddAsync(new UserRefreshToken
                {
                    UserId = user.Id,
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration

                });
            }
            else 
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientSignInDto clientSignInDto)//Client için token , token for client
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientSignInDto.ClientId && x.Secret == clientSignInDto.ClientSecret);//check token

            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("Client or ClientSecret not found", 404, true);
                
            }
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)//Kullanıcıya refreshtoken oluştur
        {
            var existrefreshToken = await _userRefreshtoken.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();//check refresh token

            if (existrefreshToken == null)
            {
                return Response<TokenDto>.Fail("Refresh Token Not Found", 404, true);
            }

            var user = await _userManager.FindByIdAsync(existrefreshToken.UserId);//check user authrezation

            if (user == null)
            {
                return Response<TokenDto>.Fail("User Id Not Found", 404, true);
            }


            var tokenDto =_tokenService.CreateToken(user);//ceate refresh token
            existrefreshToken.Code = tokenDto.RefreshToken;
            existrefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);


        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshTOken)//çıkış yapan kullanıcının refreshtokenini sil , if user logout delete refreshtoken
        {
            var existrefreshToken = await _userRefreshtoken.Where(x=>x.Code == refreshTOken).SingleOrDefaultAsync();//check refreshtoken

            if (existrefreshToken==null)
            {
                return Response<NoDataDto>.Fail("Refres Token Not Found", 404, true);
            }
            _userRefreshtoken.Remove(existrefreshToken);//delete refresh token
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);

        }
    }
}
