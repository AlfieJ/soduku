using System;
using System.Collections.Generic;

namespace soduku
{
    public static class Extensions
    {
        public static bool IsValid(this int v)
        {
            return v >= 1 && v <= 9;
        }

        public static void Fill<T>(this List<T> list, T value)
        {
            list.Clear();
            for (int i = 0; i < list.Capacity; ++i)
                list.Add(value);
        }

        public static List<T> Randomize<T>(this List<T> list)
        {
            // Don't want to change the original list, and we'll be creating
            // a second--randomized--list anway, so let's duplicate the original
            // list and move things to the random list. That way we won't
            // use too much additional memory.
            List<T> dup = new List<T>(list);
            List<T> rand = new List<T>();
            while(dup.Count > 0)
            {
                int i = _rand.Next(dup.Count);
                rand.Add(dup[i]);
                dup.RemoveAt(i);
            }
            return rand;
        }

        public static Random _rand = new Random();
    }
}