using ChatApp.data.DataModels.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.DbContexts
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<UserEntity>(options)
    {
    }
}
