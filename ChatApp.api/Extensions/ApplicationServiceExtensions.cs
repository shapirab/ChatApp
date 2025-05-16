using ChatApp.data.Services.Implementation.SqlServer;
using ChatApp.data.Services.Interface;

namespace ChatApp.api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
