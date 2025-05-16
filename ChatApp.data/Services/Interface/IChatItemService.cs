using ChatApp.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Services.Interface
{
    public interface IChatItemService
    {
        Task<ICollection<ChatItemEntity>> GetChatItemsAsync();
        Task<(ICollection<ChatItemEntity>, PaginationMetaData)> GetChatItemsAsync
            (string? searchQuery, string? userId, int pageSize, int pageNumber);
        Task<ChatItemEntity?> GetChatItemByIdAsync(int chatItemId);
        Task CreateChatItemAsync(ChatItemEntity chatItem);
        Task DeleteChatItemAsync(int chatItemId);
        Task<bool> SaveChangesAsync();
    }
}
