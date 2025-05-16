using ChatApp.data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Config
{
    public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoomEntity>
    {
        public void Configure(EntityTypeBuilder<ChatRoomEntity> builder)
        {
            builder.HasMany(chatRoom => chatRoom.RegisteredMembers).WithMany().UsingEntity(j => j.ToTable("ChatRoomMembers"));
            builder.HasMany(chatRoom => chatRoom.ActiveMembers).WithMany().UsingEntity(j => j.ToTable("ChatRoomActiveMembers"));
            builder.HasMany(chatRoom => chatRoom.ChatItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
