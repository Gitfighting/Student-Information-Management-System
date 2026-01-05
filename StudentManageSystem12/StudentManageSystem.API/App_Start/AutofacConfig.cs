using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;
using StudentManageSystem.API.Filters;
using StudentManageSystem.BLL;
using StudentManageSystem.Common;
using StudentManageSystem.DAL;

namespace StudentManageSystem.API.App_Start
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 配置 Autofac DI 容器
        /// </summary>
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // ============================================================
            // 1. 基础架构层注册
            // ============================================================

            // 注册 EF DbContext
            // 关键：InstancePerRequest 保证每个 HTTP 请求使用独立的 Context 实例
            // 原理：避免多线程下 Context 并发访问错误，同时确保通过 SaveChanges 时事务的一致性
            builder.RegisterType<AppDbContext>()
                   .As<DbContext>()
                   .InstancePerRequest();

            // 注册 UnitOfWork (工作单元)
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerRequest();

            // ============================================================
            // 2. 数据访问层 (DAL) 批量注册
            // ============================================================

            // 扫描 StudentManageSystem.DAL 程序集
            // 查找所有名称以 "Repository" 结尾的具体类 (如 StudentRepository)
            // 自动将其注册为对应的接口 (如 IStudentRepository)
            builder.RegisterAssemblyTypes(typeof(StudentRepository).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();

            // ============================================================
            // 3. 业务逻辑层 (BLL) 批量注册
            // ============================================================

            // 扫描 StudentManageSystem.BLL 程序集
            builder.RegisterAssemblyTypes(typeof(StudentBll).Assembly)
                   .Where(t => t.Name.EndsWith("Bll"))
                   .AsImplementedInterfaces()  // 注册为接口，供 Controller 使用
                   .AsSelf()                  // 也注册为自身类，方便内部调用（可选）
                   .InstancePerRequest();

            // ============================================================
            // 4. 表现层 (Controller & Filter) 注册
            // ============================================================

            // 注册当前程序集中的所有 ApiController
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // 注册过滤器 (Filter)，使其支持属性注入
            builder.RegisterType<ModelValidationFilterAttribute>().AsSelf().InstancePerDependency();
            builder.RegisterType<AdminAuthorizationFilterAttribute>().AsSelf().InstancePerDependency();
            builder.RegisterType<ActionLogFilterAttribute>().AsSelf().InstancePerDependency();
            builder.RegisterType<GlobalExceptionFilterAttribute>().AsSelf().InstancePerDependency();
            builder.RegisterType<LoginLogFilterAttribute>().AsSelf().InstancePerDependency();
            builder.RegisterType<NullParameterCheckFilterAttribute>().AsSelf().InstancePerDependency();

            // ============================================================
            // 5. 构建容器并接管 Web API 依赖解析器
            // ============================================================
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}