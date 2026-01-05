using System;
using System.Data.SqlClient;
using System.Configuration; // 需引用此命名空间读取配置

namespace StudentManageSystem.Common
{
    /// <summary>
    /// 数据库连接测试类
    /// </summary>
    public class DBTest
    {
        /// <summary>
        /// 测试数据库连接是否成功
        /// </summary>
        public static void TestDBConnection()
        {
            // 1. 获取配置文件中的连接字符串（与SqlHelper共用同一配置）
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            // 2. 尝试打开连接
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open(); // 关键：打开数据库连接
                    // 若未抛出异常，说明连接成功
                    Console.WriteLine("✅ 数据库连接成功！");
                    Console.WriteLine("连接字符串：" + connStr); // 可查看实际使用的连接字符串
                }
            }
            catch (Exception ex)
            {
                // 捕获并输出错误信息（定位问题的核心）
                Console.WriteLine("❌ 数据库连接失败：");
                Console.WriteLine("错误原因：" + ex.Message);
                // 若为登录失败，可能是sa密码错误；若为数据库不存在，需检查Initial Catalog
            }
        }
    }
}