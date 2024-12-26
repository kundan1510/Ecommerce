using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Models
{
    public class User
    {
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; } // e.g., Admin, User
    }
}
