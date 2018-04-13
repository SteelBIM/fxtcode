using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry”的 XML 注释
    public static class Retry
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry.Do(Action, TimeSpan, int, Func<Exception, bool>)”的 XML 注释
        public static void Do(
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry.Do(Action, TimeSpan, int, Func<Exception, bool>)”的 XML 注释
            Action action,
            TimeSpan retryInterval,
            int retryCount = int.MaxValue,
            Func<Exception, bool> retryConditions = null)
        {
            Do<object, Exception>(() =>
            {
                action();
                return null;
            }, retryInterval, retryCount, retryConditions);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry.Do<T, TException>(Func<T>, TimeSpan, int, Func<TException, bool>, Func<TException, bool>, Action<TException>, string)”的 XML 注释
        public static T Do<T, TException>(
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry.Do<T, TException>(Func<T>, TimeSpan, int, Func<TException, bool>, Func<TException, bool>, Action<TException>, string)”的 XML 注释
            Func<T> action,
            TimeSpan retryInterval,
            int retryCount = int.MaxValue,
            Func<TException, bool> retryConditions = null,
            Func<TException, bool> reDoConditions = null,
            Action<TException> reDo = null,
            string name = null) where TException : Exception
        {
            var e = new Exception(string.Format("[{0}]Retry count has reached the maxium, quit retry logic!", name));

            try
            {
                for (int retry = 0; retry < retryCount; retry++)
                {
                    try
                    {
                        return action();
                    }
                    catch (TException ex)
                    {
                        if (retryConditions != null)
                        {
                            if (!retryConditions(ex))
                            {
                                throw;
                            }
                        }
                        else
                        {
                            throw;
                        }

                        Trace.TraceInformation("[{2}]Exception \"{0}\", watit for {1} seconds", ex.Message, retryInterval, name);
                        Thread.Sleep(retryInterval);
                    }
                }

                throw e;
            }
            catch (TException reTryEx)
            {
                if (reDoConditions != null)
                {
                    if (!reDoConditions(reTryEx))
                    {
                        throw;
                    }
                    else if (reDo != null)
                    {
                        reDo(reTryEx);
                        return default(T);
                    }
                }

                throw;
            }

            throw e;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry.Do<T>(Func<T>, TimeSpan, int, Func<T, bool>, string)”的 XML 注释
        public static T Do<T>(
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry.Do<T>(Func<T>, TimeSpan, int, Func<T, bool>, string)”的 XML 注释
            Func<T> action,
            TimeSpan retryInterval,
            int retryCount = int.MaxValue,
            Func<T, bool> retryConditions = null, string name = null)
        {
            var result = default(T);
            for (int retry = 0; retry < retryCount; retry++)
            {
                result = action();

                if (retryConditions(result))
                {
                    Trace.TraceInformation("[{1}]retry condition met, watit for {0} seconds", retryInterval, name);
                    Thread.Sleep(retryInterval);
                }
                else
                {
                    break;
                }
            }

            return result;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry.Do<TException>(object, Action, TimeSpan, int, Action<TException, int>)”的 XML 注释
        public static void Do<TException>(this object o, Action action, TimeSpan retryInterval, int retryCount = int.MaxValue, Action<TException, int> onException = null) where TException : Exception
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry.Do<TException>(object, Action, TimeSpan, int, Action<TException, int>)”的 XML 注释
        {
            var throwEx = default(TException);
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    action();
                    return;
                }
                catch (TException ex)
                {
                    throwEx = ex;

                    if (onException != null)
                    {
                        onException(ex, retry + 1);
                    }

                    Thread.Sleep(retryInterval);
                }
            }

            throw throwEx;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Retry.Do<TException>(object, Action, TimeSpan, int, Func<TException, int, bool>, Action<TException, int>)”的 XML 注释
        public static void Do<TException>(this object o, Action action, TimeSpan retryInterval, int retryCount = int.MaxValue, Func<TException, int, bool> retryConditions = null, Action<TException, int> onException = null) where TException : Exception
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Retry.Do<TException>(object, Action, TimeSpan, int, Func<TException, int, bool>, Action<TException, int>)”的 XML 注释
        {
            var throwEx = default(TException);
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    action();
                    return;
                }
                catch (TException ex)
                {
                    throwEx = ex;
                    if (retryConditions != null)
                    {
                        if (retryConditions(ex, retry + 1))
                        {
                            Thread.Sleep(retryInterval);
                            continue;
                        }
                    }
                    if (onException != null)
                    {
                        onException(ex, retry + 1);
                    }

                    Thread.Sleep(retryInterval);
                }
            }

            throw throwEx;
        }
    }
}
