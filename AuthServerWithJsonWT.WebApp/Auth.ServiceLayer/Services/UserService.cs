using Auth.CoreLayer.DTOs;
using Auth.CoreLayer.Models;
using Auth.CoreLayer.Services;
using Auth.ServiceLayer.MapProfiles;
using Auth.SharedLibrary.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }



        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp {Email = createUserDto.Email , UserName=createUserDto.UserName};

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (result.Succeeded) 
            {
             var errors =  result.Errors.Select(x=>x.Description).ToList();

                return Response<UserAppDto>.Fail(new ErrorDto(errors, true) , 404);  
            
            }
             return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

            
        }




        public async Task<Response<UserAppDto>> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName Not Found" , 404 , true);

            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
    }
}
