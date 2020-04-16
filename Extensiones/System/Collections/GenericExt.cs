using ManageServerClient.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class GenericExt
    {
        /// <summary>
        /// 字符包含
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="tovaule"></param>
        /// <returns></returns>
        public static bool ContainsReAll<T>(this ICollection<T> m, string allvaule)
        {
            if (m == null || !m.Any() || string.IsNullOrEmpty(allvaule))
            {
                return false;
            }
            var toLower = allvaule.ToLower();
            foreach (var item in m)
            {
                if (item == null)
                {
                    continue;
                }
                if (toLower.Contains(item.ToString().ToLower()) || toLower == item.NullToStr().ToLower()) return true;
            }
            return false;
        }
    }
}
