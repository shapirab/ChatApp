using AutoMapper;
using ChatApp.data.DataModels.DTOs.ChatRooms;
using ChatApp.data.DataModels.Entities;
using ChatApp.data.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChatApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController(IChatRoomService chatRoomService, IChatItemService chatItemService,
        IUserService userService, IMapper mapper) : ControllerBase
    {
        private int maxPageSize = 20;

        [HttpGet]
        public async Task<ActionResult<ICollection<ChatRoomEntity>>> GetAllChatRoomsAsync
            (string? searchQuery, string? registeredMemberId, string? activeMemberId, int pageSize = 10, int pageNumber = 1)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (chatRoomEntities, paginationMetadata) =
                await chatRoomService.GetChatRoomsAsync(searchQuery, registeredMemberId, activeMemberId, pageSize, pageNumber);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(chatRoomEntities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChatRoomEntity>> GetChatRoomByIdAsync(int id)
        {
            var chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(id);
            if (chatRoomEntity == null)
            {
                return NotFound();
            }
            return Ok(chatRoomEntity);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateChatRoomAsync(ChatRoomToAdd chatRoomToAdd)
        {
            ChatRoomEntity chatRoom = mapper.Map<ChatRoomEntity>(chatRoomToAdd);
            await chatRoomService.CreateChatRoomAsync(chatRoom);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteChatRoomAsync(int id)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(id);
            if (chatRoomEntity == null)
            {
                return NotFound();
            }
            await chatRoomService.DeleteChatRoomAsync(chatRoomEntity.Id);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpPost("{chatRoomId:int}/members/${memberId:int}")]
        public async Task<ActionResult<bool>> AddMemberToChatRoomAsync(int chatRoomId, string memberEmail)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }

            UserEntity? memberEntity = await userService.GetUserByEmailAsync(memberEmail);
            if (memberEntity == null)
            {
                return BadRequest("User with provided email was not found");
            }

            await chatRoomService.AddMemberToChatRoomAsync(chatRoomId, memberEntity.Id);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("{chatRoomId:int}/members/${memberId:int}")]
        public async Task<ActionResult<bool>> removeMemberFromChatRoomAsync(int chatRoomId, string memberEmail)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }

            UserEntity? memberEntity = await userService.GetUserByEmailAsync(memberEmail);
            if (memberEntity == null)
            {
                return BadRequest("User with provided email was not found");
            }

            await chatRoomService.RemoveMemberFromChatRoomAsync(chatRoomId, memberEntity.Id);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpPost("{chatRoomId:int}/messages")]
        public async Task<ActionResult<bool>> AddMessageToChatRoomAsync(int chatRoomId, ChatItemEntity message)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }

            UserEntity? user = chatRoomEntity.RegisteredMembers.FirstOrDefault(member => member.Id == message.UserId);
            if(user == null)
            {
                return BadRequest("User with provided id is not registered in the chat room");
            }

            await chatRoomService.AddMessageToChatRoomAsync(chatRoomId, message);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("{chatRoomId:int}/messages/${messageId:int}")]
        public async Task<ActionResult<bool>> RemoveMessageFromChatRoomAsync(int chatRoomId, int messageId)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }
            ChatItemEntity? message = await chatItemService.GetChatItemByIdAsync(messageId);
            if (message == null)
            {
                return BadRequest("Message with provided id was not found");
            }
            await chatRoomService.RemoveMessageFromChatRoomAsync(chatRoomId, messageId);
            return Ok(await chatRoomService.SaveChangesAsync());
        }
    }
}
