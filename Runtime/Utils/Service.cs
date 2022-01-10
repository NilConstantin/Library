using System;
using System.Runtime.CompilerServices;


namespace Library
{
    public sealed class Service<T> where T : class
    {
        static T instance;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get (bool canBeNull = false) 
        {
            if (!canBeNull && instance == null)
            {
                throw new NullReferenceException($"Instance \"{typeof(T).Name}\" not set");
            }

            return instance;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Set (T newInstance)
        {
            instance = newInstance;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has()
        {
            return instance != null;
        }
    }
}