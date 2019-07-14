#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicWebAPIFactory
* 项目描述 ：
* 类 名 称 ：CustomWebApiControllerFeatureProvider
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



using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Reflection;

namespace DynamicWebAPIFactory
{
    /* ============================================================================== 
* 功能描述：CustomWebApiControllerFeatureProvider 检查控制器
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public  class CustomWebApiControllerFeatureProvider: ControllerFeatureProvider
    {
        /// <summary>
        /// 判断是否是控制器
        /// </summary>
        private Func<Type,bool> customFeature = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature">自定义判断</param>
        public CustomWebApiControllerFeatureProvider(Func<Type,bool> feature=null)
        {
            customFeature = feature;
        }

        /// <summary>
        /// 判断类型是否是控制器
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        protected override bool IsController(TypeInfo typeInfo)
        {
            bool r= base.IsController(typeInfo);

            //如果不是原来的控制器，则继续判断
            if(!r&&customFeature!=null)
            {
               return customFeature(typeInfo);
            }
            return r;
        }
    }
}
