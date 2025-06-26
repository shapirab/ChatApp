using AutoMapper;
using ChatApp.data.DataModels.DTOs.ChatItems;
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

        [HttpPost("members-register/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> AddRegisteredMemberToChatRoomAsync(int chatRoomId, [FromQuery]string memberEmail)
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
            chatRoomEntity.RegisteredMembers.Add(memberEntity);
            //await chatRoomService.AddRegisteredMemberToChatRoomAsync(chatRoomId, memberEmail);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("members-register/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> RemoveRegisteredMemberFromChatRoomAsync(int chatRoomId, [FromQuery]string memberEmail)
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

            var activeUser = chatRoomEntity.ActiveMembers.FirstOrDefault(user => user.Email == memberEntity.Email);
            if (activeUser != null)
            {
                return BadRequest("An active user cannot unregister. Please disactivate user");
            }

            var registeredUser = chatRoomEntity.RegisteredMembers.FirstOrDefault(user => user.Email == memberEntity.Email);
            if (registeredUser == null)
            {
                return BadRequest("User is already unregistered to the room");
            }

            await chatRoomService.RemoveRegisteredMemberFromChatRoomAsync(chatRoomId, memberEmail);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpPost("members-active/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> AddActiveMemberToChatRoomAsync(int chatRoomId, [FromQuery] string memberEmail)
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
            var registeredUser = chatRoomEntity.RegisteredMembers.FirstOrDefault(user => user.Email == memberEntity.Email);
            if (registeredUser == null)
            {
                return BadRequest("User must be registered to the chat room in order to become active");
            }
            chatRoomEntity.ActiveMembers.Add(registeredUser);
            //await chatRoomService.AddActiveMemberToChatRoomAsync(chatRoomId, memberEmail);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("members-active/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> RemoveActiveMemberFromChatRoomAsync(int chatRoomId, [FromQuery] string memberEmail)
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

            var activeUser = chatRoomEntity.ActiveMembers.FirstOrDefault(user => user.Email == memberEntity.Email);
            if (activeUser == null)
            {
                return BadRequest("User is already non active");
            }

            await chatRoomService.RemoveActiveMemberFromChatRoomAsync(chatRoomId, memberEmail);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpPost("messages/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> AddMessageToChatRoomAsync(int chatRoomId, [FromQuery]int chatItemId)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }

            ChatItemEntity? message = await chatItemService.GetChatItemByIdAsync(chatItemId);
            if( message == null)
            {
                return BadRequest("Message with provided id was not found");
            }

            UserEntity? user = chatRoomEntity.ActiveMembers.FirstOrDefault(member => member.Id == message.UserId);
            if(user == null || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User with provided id is not active in the chat room");
            }

            ChatItemEntity chatEntity = mapper.Map<ChatItemEntity>(message);

            await chatRoomService.AddMessageToChatRoomAsync(chatRoomId, chatEntity);
            return Ok(await chatRoomService.SaveChangesAsync());
        }

        [HttpDelete("messages/{chatRoomId:int}")]
        public async Task<ActionResult<bool>> RemoveMessageFromChatRoomAsync(int chatRoomId, [FromQuery]int chatItemId)
        {
            ChatRoomEntity? chatRoomEntity = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoomEntity == null)
            {
                return BadRequest("Chat room with provided id was not found");
            }
            ChatItemEntity? message = await chatItemService.GetChatItemByIdAsync(chatItemId);
            if (message == null)
            {
                return BadRequest("Message with provided id was not found");
            }
            await chatRoomService.RemoveMessageFromChatRoomAsync(chatRoomId, chatItemId);
            return Ok(await chatRoomService.SaveChangesAsync());
        }
    }
}
