using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.DataModels.Entities
{
    public class UserEntity : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public AddressEntity? Address { get; set; }
    }
}
