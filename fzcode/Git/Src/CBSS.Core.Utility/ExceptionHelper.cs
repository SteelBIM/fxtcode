using System;
using System.Collections.Generic;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 异常帮助类
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// 判断异常是哪个异常类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Is<T>(this Exception source) where T : Exception
        {
            if (source is T)
            {
                return true;
            }
            else if (source.InnerException != null)
            {
                return source.InnerException.Is<T>();
            }
            else
            {
                return false;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“ExceptionHelper.GetInnerExceptionChain(Exception, IList<Exception>)”的 XML 注释
        public static IEnumerable<Exception> GetInnerExceptionChain(this Exception ex, IList<Exception> chain = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“ExceptionHelper.GetInnerExceptionChain(Exception, IList<Exception>)”的 XML 注释
        {
            if (chain == null)
            {
                chain = new List<Exception>();
            }

            var inner = ex.InnerException;
            if (inner != null)
            {
                chain.Add(inner);
                inner.GetInnerExceptionChain(chain);
            }

            return chain;
        }
    }
}
