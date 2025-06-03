using ChatApp.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.DataModels.DTOs.ChatItems
{
    public class ChatItemToReturn
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
