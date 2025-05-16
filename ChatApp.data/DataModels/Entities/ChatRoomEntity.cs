using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.DataModels.Entities
{
    public class ChatRoomEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<UserEntity> RegisteredMembers { get; set; } = [];
        public ICollection<UserEntity> ActiveMembers { get; set; } = [];
        public ICollection<ChatItemEntity> ChatItems { get; set; } = [];
    }
}
