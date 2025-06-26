using AutoMapper;
using ChatApp.data.DataModels.DTOs.ChatItems;
using ChatApp.data.DataModels.Entities;
using ChatApp.data.Services.Implementation.SqlServer;
using ChatApp.data.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChatApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatItemController(IChatItemService chatItemService, IMapper mapper) : ControllerBase
    {
        private int maxPageSize = 20;

        [HttpGet]
        public async Task<ActionResult<ICollection<ChatItemToReturn>>> GetAllChatItemsAsync
            (string? searchQuery, string? userId, int pageSize = 10, int pageNumber = 1)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (chatItemEntities, paginationMetadata) =
                await chatItemService.GetChatItemsAsync(searchQuery, userId, pageSize, pageNumber);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(mapper.Map<ICollection<ChatItemToReturn>>(chatItemEntities));
        }

        [HttpGet("{id:int}", Name ="GetChatItem")]
        public async Task<ActionResult<ChatItemToReturn>> GetChatItemByIdAsync(int id)
        {
            var chatItemEntity = await chatItemService.GetChatItemByIdAsync(id);
            if (chatItemEntity == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ChatItemToReturn>(chatItemEntity));
        }

        [HttpPost]
        public async Task<ActionResult<ChatItemToReturn>> CreateChatItemAsync(ChatItemToAdd chatItemToAdd)
        {
            ChatItemEntity chatItemEntity = mapper.Map<ChatItemEntity>(chatItemToAdd);
            await chatItemService.CreateChatItemAsync(chatItemEntity);
            if (await chatItemService.SaveChangesAsync())
            {
                ChatItemToReturn chatItemToReturn = mapper.Map<ChatItemToReturn>(chatItemEntity);
                return CreatedAtRoute("GetChatItem", new { id = chatItemEntity.Id }, chatItemToReturn);
            }
            return BadRequest("Failed to create the chat item.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteChatItemAsync(int id)
        {
            ChatItemEntity? chatItemEntity = await chatItemService.GetChatItemByIdAsync(id);
            if (chatItemEntity == null)
            {
                return BadRequest("A message with provided id was not found");
            }
            await chatItemService.DeleteChatItemAsync(chatItemEntity.Id);
            return Ok(await chatItemService.SaveChangesAsync());
        }
    }
}
