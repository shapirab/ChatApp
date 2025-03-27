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
        public IEnumerable<UserEntity> RegisteredMembers { get; set; } = [];
        public IEnumerable<UserEntity> ActiveMembers { get; set; } = [];
        public IEnumerable<ChatItemEntity> ChatItems { get; set; } = [];
    }
}
