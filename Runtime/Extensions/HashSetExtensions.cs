﻿using System.Collections.Generic;
using System.Reflection;

namespace Library
{
    public static class HashSetExtensions
    {
        private static class HashSetDelegateHolder<T>
        {
            private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
            public static MethodInfo InitializeMethod { get; } = typeof(HashSet<T>).GetMethod("Initialize", Flags);
        }

        public static void SetCapacity<T>(this HashSet<T> hashSet, int capacity)
        {
            HashSetDelegateHolder<T>.InitializeMethod.Invoke(hashSet, new object[] { capacity });
        }

        public static HashSet<T> GetHashSet<T>(int capacity)
        {
            var hashSet = new HashSet<T>();
            hashSet.SetCapacity(capacity);
            return hashSet;
        }
    }
}