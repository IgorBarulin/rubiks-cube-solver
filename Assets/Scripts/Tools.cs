using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Model
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
                if (value.Equals(me[i])) { return i; }
            }

            return -1;
        }
    }
}