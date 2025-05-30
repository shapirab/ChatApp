﻿using ChatApp.data.Config;
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
        public DbSet<ChatItemEntity> ChatItems { get; set; }
        public DbSet<ChatRoomEntity> ChatRooms { get; set; }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ChatRoomConfiguration).Assembly);
        }
    }
}
