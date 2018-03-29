using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System;

namespace CAS.Entity.BaseDAModels
{
    public static class ReadWriteLock
    {

        /// <summary>
        /// used to lock the thread if two threads trying to write to same file or same key
        /// </summary>
        private static Dictionary<string, ReaderWriterLockSlim> syncLock = new Dictionary<string, ReaderWriterLockSlim>();

        /// <summary>
        /// used as a lock on synclock dictionary object
        /// </summary>
        private static ReaderWriterLockSlim syncLockLock = new ReaderWriterLockSlim();

        const int blocktimeout = 5000; // 5 seconds is the max time that a thread should be block. If lock cannot be acquired, then throw Exception


        private static ReaderWriterLockSlim getSyncLockObj(string key)
        {

            if (syncLockLock.TryEnterUpgradeableReadLock(blocktimeout))
            {
                // get read lock before reading dictionary
                try
                {
                    if (!syncLock.ContainsKey(key))
                    {
                        syncLockLock.EnterWriteLock();
                        try
                        {
                            syncLock.Add(key, new ReaderWriterLockSlim());
                        }
                        finally
                        {
                            syncLockLock.ExitWriteLock();
                        }
                    }

                    return syncLock[key]; // release lock in [finally]
                }
                finally
                {
                    // release all locks upon exit
                    syncLockLock.ExitUpgradeableReadLock();
                }
            }
            else
            {
                throw new Exception("Cannot getSyncLockObj for " + key);
            }
        }


        /// <summary>
        /// This method will get the write lock for the given "key"
        /// If there is a parallel write, it will wait for the write to finish, then return false.
        /// or else it will wait and acquire the write lock.
        /// This method is used for specific case when any write thread will achieve the same output,
        /// so the one that cannot write can just give up.
        /// Typical usage:
        ///     if (ReadWriteLock.TryAcquireWriterLock(newfile))
        ///     {
        ///         Test if newfile exists
        ///         If not, write to newfile
        ///         unlock(newfile)
        ///     } else
        ///     {
        ///         do nothing because newfile has been written by another thread already
        ///     }
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if writer locked, false if another writer has finished</returns>
        public static bool TryAcquireWriterLock(string key)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(key);


            if (mylock.TryEnterUpgradeableReadLock(0)) // test if any writer lock or UpgradeableReadLock, non-blocking
            {
                try
                {
                    // need to wait till all other reads finish
                    // this is blocking but should not deadlock
                    mylock.EnterWriteLock();
                    return true;
                }
                finally
                {
                    mylock.ExitUpgradeableReadLock();
                }
            }
            else
            {
                // if another writer thread is active, then wait for that to finish first
                mylock.EnterReadLock();
                mylock.ExitReadLock();
                return false; // no need to write!
            }

        }

        /// <summary>
        /// Block until acquiring the writer lock
        /// </summary>
        /// <param name="filename"></param>
        public static void AcquireWriterLock(string key)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(key);

            if (!mylock.TryEnterWriteLock(blocktimeout))
            {
                throw new Exception("Cannot AcquireWriterLock for " + key);
            }
        }


        public static bool AcquireReaderLock(string key)
        {
            List<string> keys = new List<string>();
            keys.Add(key);
            return AcquireReaderLocks(keys);
        }

        public static bool AcquireReaderLocks(List<String> keys)
        {
            List<string> acquired = new List<string>();
            try
            {
                foreach (string key in keys)
                {
                    ReaderWriterLockSlim mylock = getSyncLockObj(key);
                    if (!mylock.TryEnterReadLock(blocktimeout))
                    {
                        throw new Exception();
                    }
                    acquired.Add(key);
                }
            }
            catch (Exception )
            {
                ReleaseReaderLocks(acquired);
                throw new Exception("Cannot AcquireReaderLocks " + String.Join(",", keys.ToArray()));
            }
            return true;
        }

        public static bool ReleaseReaderLock(string key)
        {
            List<string> keys = new List<string>();
            keys.Add(key);
            return ReleaseReaderLocks(keys);
        }

        public static bool ReleaseReaderLocks(List<string> keys)
        {
            foreach (string key in keys)
            {
                ReaderWriterLockSlim mylock = getSyncLockObj(key);
                mylock.ExitReadLock();
            }
            return true;
        }

        public static void ReleaseWriterLock(string key)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(key);
            mylock.ExitWriteLock();
        }

     
        public delegate V GetDictValueDelegate<K, V>(K key);
        public delegate V TransformDictValueDelegate<K, V>(K key, V value);

        public static V ThreadSafeReadAndAddIfNotExists<K, V>(this Dictionary<K, V> dict, K key, GetDictValueDelegate<K, V> getDictValue, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);
            if (mylock.TryEnterReadLock(blocktimeout))
            {
                // get read lock before reading dictionary
                try
                {
                    if (!dict.ContainsKey(key))
                    {
                        mylock.ExitReadLock();
                        mylock.EnterWriteLock();
                        try
                        {
                            if (!dict.ContainsKey(key))
                            {
                                dict.Add(key, getDictValue(key));
                            }
                        }
                        finally
                        {
                            mylock.ExitWriteLock();
                            mylock.EnterReadLock();
                        }
                    }
                    return dict[key]; // release lock in [finally]
                }
                finally
                {
                    // release all locks upon exit
                    if (mylock.IsReadLockHeld)
                    {
                        mylock.ExitReadLock();
                    }
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeReadAndAddIfNotExists for Dict:" + lockname);
            }
        }

        public static V ThreadSafeReadAndAddIfNotExists<K, V>(this Dictionary<K, V> dict, K key, V initValue, string lockname)
        {
            return ThreadSafeReadAndAddIfNotExists(dict, key,
                new GetDictValueDelegate<K, V>(delegate(K dKey)
                {
                    return initValue;
                }
                ), lockname);
        }

        public static V ThreadSafeRead<K, V>(this Dictionary<K, V> dict, K key, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);

            if (mylock.TryEnterReadLock(blocktimeout))
            {
                // get read lock before reading dictionary
                try
                {
                    V value;
                    dict.TryGetValue(key, out value);
                    return value;
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeRead for Dict:" + lockname);
            }
        }

        public static void ThreadSafeWrite<K, V>(this Dictionary<K, V> dict, K key, V value, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);

            if (mylock.TryEnterWriteLock(blocktimeout))
            {
                try
                {
                    dict[key] = value;
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeWrite for Dict:" + lockname);
            }
        }

        public static V ThreadSafeWrite<K, V>(this Dictionary<K, V> dict, K key, TransformDictValueDelegate<K, V> transformDictValue, V initValue, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);
            if (mylock.TryEnterWriteLock(blocktimeout))
            {
                try
                {
                    V result;

                    if (dict.ContainsKey(key))
                    {
                        result = transformDictValue(key, dict[key]);
                        dict[key] = result;
                    }
                    else
                    {
                        result = transformDictValue(key, initValue);
                        dict.Add(key, result);
                    }

                    return result;
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeWrite for Dict:" + lockname);
            }

        }

        public static void ThreadSafeClear<K, V>(this Dictionary<K, V> dict, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);

            if (mylock.TryEnterWriteLock(blocktimeout))
            {
                try
                {
                    dict.Clear();
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeClear for Dict:" + lockname);
            }
        }


        public static K[] ThreadSafeGetKeysCopy<K, V>(this Dictionary<K, V> dict, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);
            K[] keys = null;
            if (mylock.TryEnterReadLock(blocktimeout))
            {
                try
                {
                    keys = new K[dict.Count];
                    dict.Keys.CopyTo(keys, 0);
                    return keys;
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitReadLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeGetKeysCopy for Dict:" + lockname);
            }
        }

        public static void ThreadSafeRemove<K, V>(this Dictionary<K, V> dict, K key, string lockname)
        {
            ReaderWriterLockSlim mylock = getSyncLockObj(lockname);

            if (mylock.TryEnterWriteLock(blocktimeout))
            {
                try
                {
                    dict.Remove(key);
                }
                finally
                {
                    // release all locks upon exit
                    mylock.ExitWriteLock();
                }
            }
            else
            {
                throw new Exception("Cannot ThreadSafeRemove for Dict:" + lockname);
            }
        }

    }

    static class ReadWriteLockExamples
    {
        static Dictionary<string, string> TestDic = new Dictionary<string, string>();

        static void Example1()
        {
            var v1_1 = TestDic.ThreadSafeRead("key1", "TestDic"); // should return null
            var v1_2 = TestDic.ThreadSafeReadAndAddIfNotExists("key1", "value1", "TestDic"); // should return "value1"
            TestDic.ThreadSafeWrite("key2", "value2", "TestDic"); // write value2 to key2
            var v1_3 = TestDic.ThreadSafeReadAndAddIfNotExists("key1", new ReadWriteLock.GetDictValueDelegate<string, string>(
                delegate(string key)
                {
                    return key + "test";
                }
            ),
            "TestDic"
            ); // this will return "value1
            var v3_1 = TestDic.ThreadSafeReadAndAddIfNotExists("key3", new ReadWriteLock.GetDictValueDelegate<string, string>(
                delegate(string key)
                {
                    return key + "test";
                }
            ),
            "TestDic"
            ); // this will return key3test

        }
    }
}
