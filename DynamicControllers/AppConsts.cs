#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicControllersFactory
* 项目描述 ：
* 类 名 称 ：AppConsts
* 类 描 述 ：
* 命名空间 ：DynamicControllersFactory
* CLR 版本 ：4.0.30319.42000
* 作    者 ：jinyu
* 创建时间 ：2019
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ jinyu 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion



using System;
using System.Collections.Generic;

namespace DynamicControllersFactory
{
    /* ============================================================================== 
* 功能描述：AppConsts  公共配置项谓词,该程序集使用，外部设置来自DynamicWebApiOptions
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    internal static class AppConsts
    {
        /// <summary>
        /// 默认谓词：Post
        /// </summary>
        public static string DefaultHttpVerb { get; set; }

        /// <summary>
        /// 默认域名称：NULL
        /// </summary>
        public static string DefaultAreaName { get; set; }

        /// <summary>
        /// 默认前缀：APi
        /// </summary>
        public static string DefaultApiPreFix { get; set; }

        /// <summary>
        /// 控制器前缀
        /// </summary>
        public static List<string> ControllerPostfixes { get; set; }
        
        /// <summary>
        /// 映射控制器名称的特性，常量，静态字段或者属性名称（URL）
        /// 默认名称：BilName;
        /// </summary>
        public static string ControllerMapName { get; set; }

        /// <summary>
        /// 控制器版本；
        /// 会在路由中添加；
        /// 默认：Version
        /// </summary>
        public static string ControllerVersion { get; set; }

        /// <summary>
        /// 控制器所在域
        /// </summary>
        public static string ControllerArea { get; set; }

        /// <summary>
        /// 方法前缀
        /// </summary>
        public static List<string> ActionPostfixes { get; set; }

        /// <summary>
        /// 绑定 特性FormBody的类型
        /// </summary>
        public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

        /// <summary>
        /// 谓词替换集合
        /// </summary>
        public static Dictionary<string, string> HttpVerbs { get; }

        static AppConsts()
        {
            ControllerMapName = "BilName";
            ControllerVersion = "Version";
           HttpVerbs = new Dictionary<string, string>()
            {
                ["Add"] = "POST",
                ["create"] = "POST",
                ["post"] = "POST",

                ["get"] = "GET",
                ["find"] = "GET",
                ["fetch"] = "GET",
                ["query"] = "GET",

                ["update"] = "PUT",
                ["put"] = "PUT",

                ["delete"] = "DELETE",
                ["remove"] = "DELETE",
            };
        }
    }
}
