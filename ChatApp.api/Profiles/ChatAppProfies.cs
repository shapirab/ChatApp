using AutoMapper;
using ChatApp.data.DataModels.DTOs.ChatItems;
using ChatApp.data.DataModels.DTOs.ChatRooms;
using ChatApp.data.DataModels.DTOs.Users;
using ChatApp.data.DataModels.Entities;

namespace ChatApp.api.Profiles
{
    public class ChatAppProfies : Profile
    {
        public ChatAppProfies()
        {
            CreateMap<ChatItemEntity, ChatItemToReturn>();
            CreateMap<ChatItemToReturn, ChatItemEntity>();

            CreateMap<ChatItemEntity, ChatItemToAdd>();
            CreateMap<ChatItemToAdd, ChatItemEntity>();

            CreateMap<ChatRoomEntity, ChatRoomToAdd>();
            CreateMap<ChatRoomToAdd, ChatRoomEntity>();

            CreateMap<RegisterDto, UserEntity>();
            CreateMap<UserEntity, RegisterDto>();

            CreateMap<LoginDto, UserEntity>();
            CreateMap<UserEntity, LoginDto>();
        }
    }
}
