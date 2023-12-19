using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.CoreLayer.Configuration
{
    public class Client
    {
        public int Id { get; set; }

        public string Secret { get; set; }

        public List<string> Audiences { get; set; }
    }
}
