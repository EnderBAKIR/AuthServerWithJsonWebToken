using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.SharedLibrary.Configurations
{
    public class CustomTokenOptions
    {
        public List<string> Audience { get; set; }

        public string Issuer { get; set; }

        public int AccesTokenExpiration { get; set; }

        public int RefreshTokenExpiration { get; set; }


        public string SecurityKey { get; set; }
    }
}
