#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicWebAPIFactory
* 项目描述 ：
* 类 名 称 ：DynamicWebApiServiceExtensions
* 类 描 述 ：
* 命名空间 ：DynamicWebAPIFactory
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.Loader;

namespace DynamicWebAPIFactory
{
    /* ============================================================================== 
* 功能描述：DynamicWebApiServiceExtensions 扩展方法，注入程序
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public static  class DynamicWebApiServiceExtensions
    {

        /// <summary>
        /// 添加动态API中间件
        /// </summary>
        /// <param name="services">注入容器</param>
        /// <param name="options">配置</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, DynamicWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();
            //内部配置设置
            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.ControllerMapName = options.ControllerMapName;
            AppConsts.ControllerVersion = options.ControllerVersion;
            AppConsts.ControllerArea = options.ControllerArea;
            //
            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new CustomWebApiControllerFeatureProvider(options.ControllerFeature));

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new DynamicWebApiConvention(services));
            });

            return services;
        }


        /// <summary>
        /// 注入控制器程序集
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dlls">程序集文件</param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiAssemblys(this IServiceCollection services,string[] dlls)
        {
           if(dlls==null||dlls.Length==0)
            {
                return services;
            }
            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }
            foreach (string dll in dlls)
            {
                services.AddWebApiAssembly(dll);
            }
            return services;
        }

        /// <summary>
        /// 程序集加载
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dlls"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiAssembly(this IServiceCollection services, string file)
        {
            if (file.IsNullOrEmpty())
            {
                return services;
            }
            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }
                AssemblyPart part = new AssemblyPart(AssemblyLoadContext.Default.LoadFromAssemblyPath(file));
                partManager.ApplicationParts.Add(part);
            
            return services;
        }

        /// <summary>
        /// 注入控制器目录查找程序集
        /// 默认筛选：*.dll
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dirs">目录</param>
        /// <param name="dllfilter">程序集文件筛选条件,正则表达式</param>
        /// <returns></returns>
        public static IServiceCollection AddWebApiDirectory(this IServiceCollection services, string[] dirs=null,string filter=null)
        {
            if(filter==null)
            {
                filter = "*.dll";
            }
            if(dirs==null||dirs.Length==0)
            {
                dirs = new string[] { AppDomain.CurrentDomain.BaseDirectory };
            }
            foreach(string dir in dirs)
            {
                var files = Directory.EnumerateFiles(dir, filter);
                foreach (string file in files)
                {
                    services.AddWebApiAssembly(file);
                }
            }
            return services;
        }


        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
        {
            return AddDynamicWebApi(services, new DynamicWebApiOptions());
        }
    }
}
