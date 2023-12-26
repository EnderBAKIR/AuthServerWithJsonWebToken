using Auth.CoreLayer.DTOs;
using Auth.CoreLayer.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.ServiceLayer.MapProfiles
{
    public class MapProfile : Profile
    {
        

        public MapProfile() 
        {
         CreateMap<ProductDto , Product >().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        
        
        }



    }
}
