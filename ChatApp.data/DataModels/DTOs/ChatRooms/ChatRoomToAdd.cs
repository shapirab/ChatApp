using ChatApp.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.DataModels.DTOs.ChatRooms
{
    public class ChatRoomToAdd
    {
        public ICollection<UserEntity> RegisteredMembers { get; set; } = [];
        public ICollection<UserEntity> ActiveMembers { get; set; } = [];
        public ICollection<ChatItemEntity> ChatItems { get; set; } = [];
    }
}
