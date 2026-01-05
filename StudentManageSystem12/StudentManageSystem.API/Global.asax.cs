using System;
using System.Web.Http;
using System.Data.Entity;
using StudentManageSystem.DAL;
using System.Linq;
using StudentManageSystem.API.App_Start;

namespace StudentManageSystem.API
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // 1. 标准 Web API 配置
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 2. EF 初始化策略配置 (开发阶段：模型变更则重建数据库)
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());
            using (var context = new AppDbContext())
            {
                var count = context.Students.Count(); // 触发数据库初始化
            }

            // 3. 🔥 Autofac 容器初始化配置
            // 调用单独提取的配置类，保持 Global.asax 整洁
            AutofacConfig.ConfigureContainer();
        }

    }
}