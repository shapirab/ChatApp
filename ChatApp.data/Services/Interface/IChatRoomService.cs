using ChatApp.data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Services.Interface
{
    public interface IChatRoomService
    {
        Task<ICollection<ChatRoomEntity>> GetChatRoomsAsync();
        Task<(ICollection<ChatRoomEntity>, PaginationMetaData)> GetChatRoomsAsync
            (string? searchQuery, string? registeredMemberId, string? activeMemberId, int pageSize, int pageNumber);
        Task<ChatRoomEntity?> GetChatRoomByIdAsync(int chatRoomId);
        Task CreateChatRoomAsync(ChatRoomEntity chatRoom);
        Task DeleteChatRoomAsync(int chatRoomId);
        Task AddMemberToChatRoomAsync(int chatRoomId, string username);
        Task RemoveMemberFromChatRoomAsync(int chatRoomId, string username);
        Task AddMessageToChatRoomAsync(int chatRoomId, ChatItemEntity message);
        Task RemoveMessageFromChatRoomAsync(int chatRoomId, int messageId);
        Task<bool> SaveChangesAsync();
    }
}
