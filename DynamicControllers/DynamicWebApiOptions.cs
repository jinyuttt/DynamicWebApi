#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicControllersFactory
* 项目描述 ：
* 类 名 称 ：DynamicWebApiOptions
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



using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DynamicControllersFactory
{
    /* ============================================================================== 
* 功能描述：DynamicWebApiOptions 各项配置
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
  public  class DynamicWebApiOptions
    {
        /// <summary>
        /// 配置
        /// </summary>
        public DynamicWebApiOptions()
        {
            RemoveControllerPostfixes = new List<string>() { "AppService", "ApplicationService" };
            RemoveActionPostfixes = new List<string>() { "Async" };
            FormBodyBindingIgnoredTypes = new List<Type>() { typeof(IFormFile) };
            ControllerMapName = "BilName";
            ControllerVersion = "Version";
        }

        /// <summary>
        /// API HTTP Verb.
        /// <para></para>
        /// Default value is "POST".
        /// </summary>
        public string DefaultHttpVerb { get; set; } = "POST";

        /// <summary>
        /// 默认域
        /// </summary>
        public string DefaultAreaName { get; set; }

      
        /// <summary>
        /// 默认路由前缀：api
        /// </summary>
        public string DefaultApiPrefix { get; set; } = "api";
 
        /// <summary>
        /// 移除控制器后缀
        /// </summary>
        public List<string> RemoveControllerPostfixes { get; set; }

        /// <summary>
        /// 移除方法后缀，默认：Async
        /// </summary>
        public List<string> RemoveActionPostfixes { get; set; }

        /// <summary>
        /// 忽视绑定FormBody的参数类型，默认：IFormFile
        /// </summary>
        public List<Type> FormBodyBindingIgnoredTypes { get; set; }

        /// <summary>
        /// 判断控制器回调
        /// </summary>
        public Func<Type, bool> ControllerFeature { get; set; }

        /// <summary>
        /// 映射控制器路由名称，根据该名称的常量，静态属性的值
        /// 默认：BilName
        /// </summary>
        public string ControllerMapName { get; set; }

        /// <summary>
        /// 映射控制器版本信息，根据该名称的常量，静态属性的值
        /// 默认：Version
        /// </summary>
        public string ControllerVersion { get; set; }

        /// <summary>
        /// 控制器所在域设置，根据该名称的常量，静态属性的值
        /// </summary>
        public string ControllerArea { get; set; }

        /// <summary>
        /// Verify that all configurations are valid
        /// </summary>
        public void Valid()
        {
            if (string.IsNullOrEmpty(DefaultHttpVerb))
            {
                throw new ArgumentException($"{nameof(DefaultHttpVerb)} can not be empty.");
            }

            if (string.IsNullOrEmpty(DefaultAreaName))
            {
                DefaultAreaName = string.Empty;
            }

            if (string.IsNullOrEmpty(DefaultApiPrefix))
            {
                DefaultApiPrefix = string.Empty;
            }

            if (FormBodyBindingIgnoredTypes == null)
            {
                throw new ArgumentException($"{nameof(FormBodyBindingIgnoredTypes)} can not be null.");
            }

            if (RemoveControllerPostfixes == null)
            {
                throw new ArgumentException($"{nameof(RemoveControllerPostfixes)} can not be null.");
            }
        }
    }
}
