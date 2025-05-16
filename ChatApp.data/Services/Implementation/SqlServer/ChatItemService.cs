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
    public class ChatItemService(AppDbContext db) : IChatItemService
    {
        public async Task<ChatItemEntity?> GetChatItemByIdAsync(int chatItemId)
        {
            return await db.ChatItems.FindAsync(chatItemId);
        }

        public async Task<ICollection<ChatItemEntity>> GetChatItemsAsync()
        {
            return await db.ChatItems.OrderBy(chatItem => chatItem.UserId).ToListAsync();
        }

        public async Task<(ICollection<ChatItemEntity>, PaginationMetaData)> GetChatItemsAsync
            (string? searchQuery, string? userId, int pageSize, int pageNumber)
        {
            IQueryable<ChatItemEntity> collection = db.ChatItems as IQueryable<ChatItemEntity>;
            if(!string.IsNullOrEmpty(searchQuery))
            {
                collection = collection.Where(chatItem => chatItem.Message != null && chatItem.Message.Contains(searchQuery));
            }
            if(!string.IsNullOrEmpty(userId))
            {
                collection = collection.Where(chatItem => String.Equals(chatItem.UserId, userId));
            }

            int totalCount = collection.Count();
            PaginationMetaData paginationMetaData = new PaginationMetaData(totalCount, pageSize, pageNumber);

            var collectionToReturn = await collection
                        .OrderBy(chatItem => chatItem.UserId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task CreateChatItemAsync(ChatItemEntity chatItem)
        {
            await db.ChatItems.AddAsync(chatItem);
        }

        public async Task DeleteChatItemAsync(int chatItemId)
        {
            ChatItemEntity? chatItem = await db.ChatItems.FindAsync(chatItemId);
            if (chatItem != null)
            {
                db.ChatItems.Remove(chatItem);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
