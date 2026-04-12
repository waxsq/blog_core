using Blog.Core.Utils;

namespace Blog.Core
{
    public class CoreProgram
    {
        public static void Main()
        {
            // 示例连接字符串（MySqlConnector）
            string connectionStr = "server=127.0.0.1;port=3306;database=blog;user id=root;password=123456;CharSet=utf8mb4;AllowPublicKeyRetrieval=True;SslMode=None;";
            string outputPath = "D:\\Code\\Blog\\Blog.Core\\Entities";
            EntityGeneratorUtil.GenerateEntity(connectionStr, outputPath, SqlSugar.DbType.MySqlConnector);

        }
    }
}
