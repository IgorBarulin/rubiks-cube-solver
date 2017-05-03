using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeModel
{
    public static class Tools
    {
        public static T[] Rotate<T>(this IEnumerable<T> me, int amount)
        {
            var newarr = new T[me.Count()];
            int i = 0;

            foreach (T item in me)
            {
                newarr[(i + amount) % me.Count()] = item;
                i++;
            }

            return newarr;
        }

        public static int Index<T>(this T[] me, T value) where T : IEquatable<T>
        {
            for (int i = 0; i < me.Length; i++)
            {
                if (value.Equals(me[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int Index<T>(this T[] me, T value, Func<T, T, bool> comparer)
        {
            for (int i = 0; i < me.Length; i++)
            {
                if (comparer(value, me[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool SetEquals<T>(this T[] a, T[] b)
        {
            var min = (a.Length >= b.Length) ? b : a;
            var max = (a.Length >= b.Length) ? a : b;

            foreach (T item in max)
            {
                if (!min.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public static int NChooseK(int n, int k)
        {
            int i, j, s;
            if (n < k)
                return 0;
            if (k > n / 2)
                k = n - k;
            for (s = 1, i = n, j = 1; i != n - k; i--, j++)
            {
                s *= i;
                s /= j;
            }
            return s;
        }
    }
}