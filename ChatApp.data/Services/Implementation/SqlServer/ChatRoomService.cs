using AutoMapper.Execution;
using ChatApp.data.DataModels.Entities;
using ChatApp.data.DbContexts;
using ChatApp.data.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.data.Services.Implementation.SqlServer
{
    public class ChatRoomService(AppDbContext db, IUserService userService) : IChatRoomService
    {
        public async Task<ICollection<ChatRoomEntity>> GetChatRoomsAsync()
        {
            return await db.ChatRooms
                .OrderBy(chatRoom => chatRoom.Id)
                .Include(chatRoom => chatRoom.ChatItems)
                .Include(chatRoom => chatRoom.RegisteredMembers)
                .Include(chatRoom => chatRoom.ActiveMembers)
                .ToListAsync();
        }

        public async Task<(ICollection<ChatRoomEntity>, PaginationMetaData)> GetChatRoomsAsync
            (string? searchQuery, string? registeredMemberId, string? activeMemberId, int pageSize, int pageNumber)
        {
            IQueryable<ChatRoomEntity> collection = db.ChatRooms as IQueryable<ChatRoomEntity>;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection
                    .Where(chatRoom => chatRoom.ChatItems
                        .Any(chatItem => chatItem.Message != null && chatItem.Message.Contains(searchQuery)));
            }
            if(!string.IsNullOrEmpty(registeredMemberId))
            {
                collection = collection
                    .Where(chatRoom => chatRoom.RegisteredMembers
                    .Any(registeredMember => String.Equals(registeredMember.Id, registeredMemberId)));
            }
            if (!string.IsNullOrEmpty(activeMemberId))
            {
                collection = collection
                    .Where(chatRoom => chatRoom.RegisteredMembers.Any(activeMember => String.Equals(activeMember.Id, activeMemberId)));
            }

            int totalCount = collection.Count();
            PaginationMetaData paginationMetaData = new PaginationMetaData(totalCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                        .OrderBy(chatRoom => chatRoom.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Include(chatRoom => chatRoom.ChatItems)
                        .Include(chatRoom => chatRoom.RegisteredMembers)
                        .Include(chatRoom => chatRoom.ActiveMembers)
                        .Take(pageSize)
                        .ToListAsync();
            return(collectionToReturn, paginationMetaData);
        }

        public async Task<ChatRoomEntity?> GetChatRoomByIdAsync(int chatRoomId)
        {
            return await db.ChatRooms
                .Include(chatRoom => chatRoom.ChatItems)
                .Include(chatRoom => chatRoom.RegisteredMembers)
                .Include(chatRoom => chatRoom.ActiveMembers)
                .FirstOrDefaultAsync(chatRoom => chatRoom.Id == chatRoomId);
        }

        public async Task CreateChatRoomAsync(ChatRoomEntity chatRoom)
        {
            await db.ChatRooms.AddAsync(chatRoom);
        }

        public async Task DeleteChatRoomAsync(int chatRoomId)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if(chatRoom != null && (chatRoom.ActiveMembers.Count > 0 || chatRoom.RegisteredMembers.Count > 0))
            {
                throw new InvalidOperationException("Cannot delete chat room with active members");
            }
            if(chatRoom != null)
            {
                db.ChatRooms.Remove(chatRoom);
            }
        }

        public async Task AddRegisteredMemberToChatRoomAsync(int chatRoomId, string userEmail)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if(chatRoom != null)
            {
                UserEntity? member = await userService.GetUserByEmailAsync(userEmail);
                if (member != null)
                {
                    chatRoom.RegisteredMembers.Add(member);
                }
            }
        }

        public async Task RemoveRegisteredMemberFromChatRoomAsync(int chatRoomId, string userEmail)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom != null)
            {
                UserEntity? member = await userService.GetUserByEmailAsync(userEmail);
                if (member != null)
                {
                    chatRoom.RegisteredMembers.Remove(member);
                }
            }
        }

        public async Task AddActiveMemberToChatRoomAsync(int chatRoomId, string userEmail)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom != null)
            {
                UserEntity? member = await userService.GetUserByEmailAsync(userEmail);
                if (member != null)
                {
                    chatRoom.ActiveMembers.Add(member);
                }
            }
        }

        public async Task RemoveActiveMemberFromChatRoomAsync(int chatRoomId, string userEmail)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom != null)
            {
                UserEntity? member = await userService.GetUserByEmailAsync(userEmail);
                if (member != null)
                {
                    chatRoom.ActiveMembers.Remove(member);
                }
            }
        }

        public async Task AddMessageToChatRoomAsync(int chatRoomId, ChatItemEntity message)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            if (chatRoom != null)
            {
                chatRoom.ChatItems.Add(message);
            }
        }

        public async Task RemoveMessageFromChatRoomAsync(int chatRoomId, int messageId)
        {
            ChatRoomEntity? chatRoom = await db.ChatRooms.FindAsync(chatRoomId);
            ChatItemEntity? message = await db.ChatItems.FindAsync(messageId);

            if (chatRoom != null && message != null)
            {
                chatRoom.ChatItems.Remove(message);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
