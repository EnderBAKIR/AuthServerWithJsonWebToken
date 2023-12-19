using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Models
{
    public class UserApp : IdentityUser<string>
    {
        public string City { get; set; }
    }
}
