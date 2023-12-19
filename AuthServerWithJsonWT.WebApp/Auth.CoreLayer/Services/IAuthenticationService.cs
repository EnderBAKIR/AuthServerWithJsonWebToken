using Auth.CoreLayer.DTOs;
using Auth.SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(SignInDto signInDto);

        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshTOken);

        Task<Response<ClientTokenDto>> CreateTokenByClient(ClientSignInDto clientSignInDto);
    }
}
