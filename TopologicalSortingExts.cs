using ManageServerClient.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public interface IKeyClass
    {
        string Key { get; }
    }
    public static class TopologicalSortingExts
    {
        /// <summary>
        /// 拓扑排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="getDependencies"></param>
        /// <returns></returns>
        public static List<T> SortTopological<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
            where T : IKeyClass
        {
            var sorted = new List<T>();
            var visited = new Dictionary<string, bool>();


            foreach (var item in source)
            {
                int level = 1;
                Visit(source, item, getDependencies, sorted, visited, ref level);
            }

            return sorted;
        }

        public static void Visit<T>(IEnumerable<T> source, T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<string, bool> visited, ref int level)
             where T : IKeyClass
        {


            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item.Key, out inProcess);
             
            // 如果已经访问该顶点，则直接返回
            if (alreadyVisited)
            {

                // 如果处理的为当前节点，则说明存在循环引用
                if (inProcess)
                {
                    //throw new ArgumentException("Cyclic dependency found.");
                    Console.WriteLine("Cyclic dependency found.");
                }
            }
            else
            {
                // 正在处理当前顶点
                visited[item.Key] = true;

                // 获得所有依赖项
                var dependencies = getDependencies(item);
                if (!dependencies.HasItem())
                {
                    var itemsort = source.Where(a => a.Key == item.Key).FirstOrDefault();
                    if (itemsort != null)
                    {
                        dependencies = getDependencies(itemsort);

                        if (dependencies.HasItem())
                        {
                            item = itemsort;
                        } 
                    }
                }

                // 如果依赖项集合不为空，遍历访问其依赖节点
                if (dependencies.HasItem())
                {
                    level++;

                    Console.WriteLine(item + ":" + level);

                    foreach (var dependency in dependencies)
                    {
                        Console.WriteLine(item + "->" + dependency + "->:" + level);
                        // 递归遍历访问
                        Visit(source, dependency, getDependencies, sorted, visited, ref level);

                    }

                }

                // 处理完成置为 false
                visited[item.Key] = false;
                sorted.Add(item);

            }
        }
    }
}
