#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：DynamicControllersFactory
* 项目描述 ：
* 类 名 称 ：ExtensionMethods
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
using System.Text;
using System.Text.RegularExpressions;

namespace DynamicControllersFactory
{
    /* ============================================================================== 
* 功能描述：ExtensionMethods 本程序集字符串功能扩展
* 创 建 者：jinyu 
* 创建日期：2019 
* 更新时间 ：2019
* ==============================================================================*/
    internal static  class ExtensionMethods
    {

        /// <summary>
        /// 直接判断空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        /// 判断参数中是否有该字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public static bool IsIn(this string str, params string[] data)
        {
            foreach (var item in data)
            {
                if (str == item)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 移除字符串首部中的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="postFixes"></param>
        /// <returns></returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// 移除字符串尾部数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

       /// <summary>
       /// 移除首部字符串
       /// </summary>
       /// <param name="str"></param>
       /// <param name="len"></param>
       /// <returns></returns>
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

       /// <summary>
       /// 处理名称
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
        public static string GetCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 1)
            {
                return str;
            }
            else
            {
                return res[0];
            }
        }

        /// <summary>
        /// 根据命名规则获取词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPascalCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 2)
            {
                return str;
            }
            else
            {
                return res[1];
            }
        }

        /// <summary>
        /// 处理名称中的动词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPascalOrCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            if (str[0] >= 65 && str[0] <= 90)
            {
                return GetPascalCaseFirstWord(str);
            }
            else
            {
                return GetCamelCaseFirstWord(str);
            }
        }

        /// <summary>
        /// 去除重复字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="repeats"></param>
        /// <returns></returns>
        public static string RemoveRepeat(this string str,params string[] repeats)
        {
            foreach(string r in repeats)
            {
               str= Regex.Replace(str.Trim(), string.Format(@"[\{0}]+",r), r);
            }
            return str;
        }

       
    }
}
