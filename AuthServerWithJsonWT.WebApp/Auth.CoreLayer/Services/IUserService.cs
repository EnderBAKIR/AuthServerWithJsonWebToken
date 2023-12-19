using Auth.CoreLayer.DTOs;
using Auth.SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>>CreateUserAsync(CreateUserDto createUserDto);
        
        Task<Response<UserAppDto>>GetUserByUserNameAsync(string userName);
    }
}
