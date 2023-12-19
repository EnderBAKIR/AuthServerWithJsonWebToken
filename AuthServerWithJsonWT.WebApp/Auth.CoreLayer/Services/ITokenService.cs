using Auth.CoreLayer.Configuration;
using Auth.CoreLayer.DTOs;
using Auth.CoreLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Services
{
   public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);

        ClientTokenDto CreateTokenByClient(Client client);

    }
}
