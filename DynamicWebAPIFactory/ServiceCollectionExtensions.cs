#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicWebAPIFactory
* 项目描述 ：
* 类 名 称 ：ServiceCollectionExtensions
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



using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DynamicWebAPIFactory
{
    /* ============================================================================== 
* 功能描述：ServiceCollectionExtensions ServiceCollection扩展，可以替换注入容器
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    internal static class ServiceCollectionExtensions
    {

        /// <summary>
        /// 获取注入的类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }

       /// <summary>
       /// 扩展容器，用于容器替换
       /// </summary>
       /// <param name="services"></param>
       /// <returns></returns>
        public static IServiceProvider BuildServiceProviderFromFactory(this IServiceCollection services)
        {
            foreach (var service in services)
            {
                var factoryInterface = service.ImplementationInstance?.GetType()
                    .GetTypeInfo()
                    .GetInterfaces()
                    .FirstOrDefault(i => i.GetTypeInfo().IsGenericType &&
                                         i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>));

                if (factoryInterface == null)
                {
                    continue;
                }

                var containerBuilderType = factoryInterface.GenericTypeArguments[0];
                return (IServiceProvider)typeof(ServiceCollectionExtensions)
                    .GetTypeInfo()
                    .GetMethods()
                    .Single(m => m.Name == nameof(BuildServiceProviderFromFactory) && m.IsGenericMethod)
                    .MakeGenericMethod(containerBuilderType)
                    .Invoke(null, new object[] { services, null });
            }

            return services.BuildServiceProvider();
        }

        public static IServiceProvider BuildServiceProviderFromFactory<TContainerBuilder>(this IServiceCollection services, Action<TContainerBuilder> builderAction = null)
        {

            var serviceProviderFactory = services.GetSingletonInstanceOrNull<IServiceProviderFactory<TContainerBuilder>>();
            if (serviceProviderFactory == null)
            {
                throw new Exception($"Could not find {typeof(IServiceProviderFactory<TContainerBuilder>).FullName} in {services}.");
            }

            var builder = serviceProviderFactory.CreateBuilder(services);
            builderAction?.Invoke(builder);
            return serviceProviderFactory.CreateServiceProvider(builder);
        }

    }
}
