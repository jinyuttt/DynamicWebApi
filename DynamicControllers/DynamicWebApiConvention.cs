#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicControllersFactory
* 项目描述 ：
* 类 名 称 ：DynamicWebApiConvention
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



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Internal;

namespace DynamicControllersFactory
{
    /* ============================================================================== 
* 功能描述：DynamicWebApiConvention 控制器约束修改设置
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    public class DynamicWebApiConvention : IApplicationModelConvention
    {
        private readonly IServiceCollection _services;

        public DynamicWebApiConvention(IServiceCollection services)
        {
            this._services = services;
        }
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
              
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                //没有Route
               if(matchedSelectors.Count==0)
                {
                    //
                    string SrvName = ConfigControllerInfo(controller, AppConsts.ControllerMapName);
                    if (string.IsNullOrEmpty(SrvName))
                    {
                        controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());
                    }
                    else
                    {
                        controller.ControllerName = SrvName;
                    }
                    ConfigureArea(controller);
                    ConfigureDynamicWebApi(controller);
                }
               
            }
        }

        /// <summary>
        /// 配置所有域
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="attr"></param>
        private void ConfigureArea(ControllerModel controller)
        {
            

            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (!string.IsNullOrEmpty(AppConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = AppConsts.DefaultAreaName;
                }
            }

        }

        /// <summary>
        /// 配置WEBAPI
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureDynamicWebApi(ControllerModel controller)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller);
            ConfigureParameters(controller);
        }

        /// <summary>
        /// 设置每个参数特性
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                foreach (var para in action.Parameters)
                {
                    if (para.BindingInfo != null)
                    {
                        continue;
                    }

                    if (!IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                    {
                        if (CanUseFormBodyBinding(action, para))
                        {
                            para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 设置FormBody特性判断
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            //没有返回
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {
                   
                    //没有方法设置HttpGet
                    var httpMethodActionConstraint = actionConstraint as IActionConstraint;

                   //  var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    //  var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }
                  
                    //if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    //{
                    //    return false;
                    //}
                }
            }

            return true;
        }

         
        #region ConfigureApiExplorer

        /// <summary>
        /// Api的描述信息ApiExplorer
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureApiExplorer(ControllerModel controller)
        {
            //
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }

        /// <summary>
        /// 设置对象建立
        /// </summary>
        /// <param name="action"></param>
        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion

        /// <summary>
        /// 设置控制器
        /// </summary>
        /// <param name="controller"></param>
        private void ConfigureSelector(ControllerModel controller)
        {
            RemoveEmptySelectors(controller.Selectors);

            //已经有路由就返回
            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }
            var areaName = ConfigControllerInfo(controller, AppConsts.ControllerArea);
            string version = ConfigControllerInfo(controller,AppConsts.ControllerVersion);
            foreach (var action in controller.Actions)
            {

                ConfigureSelector(version, areaName, controller.ControllerName, action);
            }
        }

        /// <summary>
        /// 配置选择
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="action"></param>
        private void ConfigureSelector(string version,string areaName, string controllerName, ActionModel action)
        {
            RemoveEmptySelectors(action.Selectors);
            if (!action.Selectors.Any())
            {
                AddAppServiceSelector(version,areaName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(version,areaName, controllerName, action);
            }
        }

        /// <summary>
        /// 配置ActionModel路由地址
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="action"></param>
        private void AddAppServiceSelector(string version,string areaName, string controllerName, ActionModel action)
        {
            try
            {
                string verb;
                var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();

                //查看是否有谓词对应
                verb = AppConsts.HttpVerbs.ContainsKey(verbKey) ? AppConsts.HttpVerbs[verbKey] : AppConsts.DefaultHttpVerb;

                action.ActionName = GetRestFulActionName(action.ActionName);

                var appServiceSelectorModel = new SelectorModel
                {
                    AttributeRouteModel = CreateActionRouteModel(version,areaName, controllerName, action.ActionName)
                };

                appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                appServiceSelectorModel.EndpointMetadata.Add(new HttpMethodMetadata(new[] { verb }));
                action.Selectors.Add(appServiceSelectorModel);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 处理Action的名称
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string GetRestFulActionName(string actionName)
        {
            // Remove Postfix
            actionName = actionName.RemovePostFix(AppConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();
            if (AppConsts.HttpVerbs.ContainsKey(verbKey))
            {
                if (actionName.Length == verbKey.Length)
                {
                    return "";
                }
                else
                {
                    return actionName.Substring(verbKey.Length);
                }
            }
            else
            {
                return actionName;
            }
        }

        /// <summary>
        /// 生成每个路由URL;
        /// 在原来路由上添加
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="action"></param>
        private static void NormalizeSelectorRoutes(string version,string areaName, string controllerName, ActionModel action)
        {
            action.ActionName = GetRestFulActionName(action.ActionName);
            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                    CreateActionRouteModel(version,areaName, controllerName, action.ActionName) :
                    AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(version,areaName, controllerName, ""), selector.AttributeRouteModel);
            }
        }

        /// <summary>
        /// 设置路由（控制器+Action）
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="areaName">域</param>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="actionName">方法</param>
        /// <returns></returns>
        private static AttributeRouteModel CreateActionRouteModel(string version,string areaName, string controllerName, string actionName)
        {
            var routeStr =
                $"{AppConsts.DefaultApiPreFix}/{version}/{areaName}/{controllerName}/{actionName}";
            routeStr = routeStr.RemoveRepeat("/");
            return new AttributeRouteModel(new RouteAttribute(routeStr));//注入路由
        }

        /// <summary>
        /// 设置action对应的URL
        /// </summary>
        /// <param name="selectors"></param>
        private static void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            selectors
                .Where(IsEmptySelector)
                .ToList()
                .ForEach(s => selectors.Remove(s));
        }
        
       /// <summary>
       /// 没有Route设置并且Action约束为空
       /// </summary>
       /// <param name="selector"></param>
       /// <returns></returns>
        private static bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null && selector.ActionConstraints.IsNullOrEmpty();
        }



        /// <summary>
        /// 判断数据类型是否是基础值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeEnums"></param>
        /// <returns></returns>
        public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
        {
            if (IsPrimitiveExtended(type, includeEnums))
            {
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);
            }

            return false;
        }

        private static bool IsPrimitiveExtended(Type type, bool includeEnums)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.GetTypeInfo().IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }

        /// <summary>
        /// 根据名称获取对应的值：
        /// 主要获取常量，静态变量，静态属性
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string ConfigControllerInfo(ControllerModel controller,string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            string value = string.Empty;
            var field = controller.ControllerType.GetDeclaredField(name);
            var property = controller.ControllerType.GetDeclaredProperty(name);
            if (field != null)
            {
                value = field.GetValue(null).ToString();
            }
            if (property != null)
            {
                value = property.GetValue(null, null).ToString();
            }
            return value;
        }

    }
}
