using System.Text.Json;
using Azure.Core.Serialization;
using Blog.Core.Commons;
using Blog.MvcWeb.Datas;
using SqlSugar;

namespace Blog.MvcWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            SnowFlakeSingle.WorkId = 1;

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<Filters.GlobalExceptionFilter>(); // 全局异常过滤器
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddAutoRegisteredServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<Middlewares.GlobalExceptionMiddleware>(); // 全局异常处理中间件
            app.MapControllers();

            app.UseAuthorization();
            app.MapAreaControllerRoute(
               name: "AdminArea",
               areaName: "Admin",
               pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
