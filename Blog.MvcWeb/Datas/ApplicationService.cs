using Blog.Core.Exceptions;
using Blog.Core.Profiles;
using Blog.FileStorage.Core;
using Blog.Repository;
using Blog.Service;
using SqlSugar;

namespace Blog.MvcWeb.Datas
{
    public static class ApplicationService
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ISqlSugarClient>(sp =>
            {
                // 从配置中读取连接字符串
                var connStr = configuration.GetConnectionString("MySqlConn");

                var sqlSugar = new SqlSugarClient(new ConnectionConfig()
                {
                    DbType = DbType.MySql, // 改为 MySQL
                    ConnectionString = connStr,
                    IsAutoCloseConnection = true, // 自动关闭连接
                    InitKeyType = InitKeyType.Attribute, // 从特性读取主键/表名
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        // 这里可以配置全局的命名转换规则等
                    }
                },
                db =>
                {
                    // --- AOP 配置区域 ---

                    // 获取 ILogger (用于记录 SQL 日志)
                    var logger = sp.GetService<ILogger<SqlSugarClient>>();

                    // 【AOP 1】执行前：打印 SQL (开发环境建议开启)
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        if (logger != null)
                        {
                            // 可以在这里格式化输出参数
                            logger.LogDebug("SQL Executing: {Sql}", sql);
                        }
                        Console.WriteLine(sql);
                    };

                    // 【AOP 2】执行后：可以记录执行时间等
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        // 可选：记录性能监控
                    };

                    // 【AOP 3】发生错误时：核心逻辑，将数据库异常转换为业务异常
                    db.Aop.OnError = (ex) =>
                    {
                        if (logger != null)
                        {
                            logger.LogError(ex, "数据库执行异常: {Message}", ex.Message);
                        }

                        string msg = ex.Message;

                        // 判断 MySQL 特定错误码或关键字
                        if (msg.Contains("Duplicate entry"))
                        {
                            // 唯一索引冲突 -> 409 Conflict
                            throw new BusinessException("数据已存在，请勿重复提交");
                        }
                        else if (msg.Contains("Foreign key constraint fails"))
                        {
                            // 外键约束失败 -> 400 Bad Request
                            throw new BusinessException("操作失败：存在关联数据，无法执行删除", 400);
                        }
                        else if (msg.Contains("Access denied") || msg.Contains("Connection refused"))
                        {
                            // 连接或权限问题 -> 503 Service Unavailable
                            throw new BusinessException("数据库服务不可用，请稍后重试");
                        }
                        else
                        {
                            // 其他未知错误 -> 500 Internal Server Error
                            // 注意：生产环境建议隐藏具体 SQL 错误信息，只返回通用提示
                            throw new BusinessException("数据库操作异常", 500, 500, ex);
                        }
                    };

                    // 【AOP 4】如果需要获取当前请求的用户信息 (示例)
                    // var httpContextAccessor = sp.GetService<IHttpContextAccessor>();
                    // var userId = httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
                    // 你可以在 OnLogExecuting 中把 userId 记录到日志里
                });

                return sqlSugar;
            });
        }

        /// <summary>
        /// 批量自动注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assemblyNames">要扫描的程序集名称列表 (例如: "Blog.Core", "Blog.Service")</param>
        public static void AddAutoRegisteredServices(this IServiceCollection services)
        {
            // 临时调试代码，加在方法最前面
            var repoAssembly = typeof(RepositoryProgram).Assembly;
            var serviceAssembly = typeof(ServiceProgram).Assembly;

            var repoCount = repoAssembly.GetTypes()
                .Count(t => t.Name.EndsWith("Repository") && !t.IsAbstract && t.IsClass);

            var serviceCount = serviceAssembly.GetTypes()
                .Count(t => t.Name.EndsWith("Service") && !t.Name.StartsWith("I") && !t.IsAbstract && t.IsClass);

            System.Diagnostics.Debug.WriteLine($"🔍 Repository 扫描结果: {repoCount} 个类");
            System.Diagnostics.Debug.WriteLine($"🔍 Service 扫描结果: {serviceCount} 个类");
            // 1. 批量注册 Repository
            services.Scan(scan => scan
                .FromAssemblyOf<RepositoryProgram>()
                .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository") && !c.IsAbstract))
                .AsImplementedInterfaces() // 注册所有接口 (包括 IRepository<T,K> 和 IUserRepository)
                .WithScopedLifetime());

            // 2. 批量注册 Service
            services.Scan(scan => scan
                .FromAssemblyOf<ServiceProgram>()
                .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service") && !c.Name.StartsWith("I") && !c.IsAbstract))
                .AsImplementedInterfaces() // 【关键修改】注册所有接口，不再强制要求 I+类名
                .WithScopedLifetime());

            services.AddAutoMapper(typeof(MappingProfile).Assembly); // 注册 AutoMapper 配置    
        }


        public static void AddFileStorage(this IServiceCollection services)
        {
            services.AddSingleton<FileUploadStrategy, LocalFileStrategy>(sp =>
            {
                return new LocalFileStrategy(sp.GetRequiredService<IWebHostEnvironment>());
            });

            //services.AddSingleton<FileUploadStrategy, MinioFileStrategy>(sp => new MinioFileStrategy("http://127.0.0.1:9000", "admin", "password", "mybucket"));

            services.AddSingleton<FileUploadContext>(sp =>
            {
                // 1. 手动从 sp 获取所有的策略列表
                var strategies = sp.GetServices<FileUploadStrategy>();

                // 2. 手动 new Context，把列表传进去
                return new FileUploadContext(strategies, "LocalFileStrategy");
            });
        }
    }
}
