using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MS.ApplicationCore;
using MS.ApplicationCore.Authorization;
using MS.ApplicationCore.Interface.Service;
using MS.ApplicationCore.Interfaces;
using MS.ApplicationCore.Services;
using MS.Infrastructure;
using MS.Infrastructure.Data;
using MS.Infrastructure.UnitOfWork;
using MySqlConnector;
using System.Data;

namespace MS.eContact.Web
{
    public static class ServiceExtenstions
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddApplicationCore();
            //service.AddTransient<IUnitOfWork, UnitOfWork>(); //Chú ý sử dụng AddTransient
            service.AddScoped<MySqlDbContext>();// Chú ý phải sử dụng AddScoped để đảm bảo không mở nhiều kết nối (các kết nối được tạo ra sẽ được duy trì và sử dụng lại)
            //service.AddScoped<IDbConnection>(db => new MySqlConnection(""));
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddTransient(typeof(IRepository<>), typeof(DapperRepository<>));
            service.AddTransient(typeof(IAsyncRepository<>), typeof(DapperRepository<>));
            service.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
            //service.AddScoped(typeof(IAsyncService<>), typeof(BaseService<>));
            service.AddTransient<IFileTransfer, FileTransfer>();
            service.AddTransient<IEventRepository, EventRepository>();
            service.AddTransient<IEventService, EventService>();
            service.AddTransient<IEventDetailService, EventDetailService>();
            service.AddTransient<IEventDetailRepository, EventDetailRepository>();
            service.AddTransient<IPictureService, PictureService>();
            service.AddTransient<IPictureRepository, PictureRepository>();
            service.AddTransient<IAlbumService, AlbumService>();
            service.AddTransient<IAlbumRepository, AlbumRepository>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IUserRepository, UserRepository>();
            service.AddTransient<IUserRolesRepository, UserRolesRepository>();

            service.AddTransient<IJwtUtils, JwtUtils>();
            service.AddHttpContextAccessor();


        }
    }
}
