    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
namespace StudentManageSystem.API
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // 启用属性路由（才能识别 [Route] 和 [RoutePrefix]）
            config.MapHttpAttributeRoutes();
            // 配置跨域：允许前端Vue项目（如http://localhost:5173）访问
            // 启用 CORS (允许所有来源、头、方法)
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            //var cors = new EnableCorsAttribute("http://localhost:5173", "*", "*");
            //config.EnableCors(cors);

            // 2. 新增：配置Web API默认路由（核心！确保控制器能被访问）
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}", // 路由模板：api/控制器/动作/参数
                defaults: new { id = RouteParameter.Optional } // id为可选参数
            );

            // 3. （可选）配置JSON序列化格式（避免前端接收日期格式异常）
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        }
}
}