using System;
using System.Collections.Generic;
using System.Text;

namespace ManageServerClient.Shared.Entity
{
    /// <summary>
    /// 用于返回参数
    /// </summary>
    public class NodesResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Node> node { get; set; }
    }
    /// <summary>
    /// 后台服务监控节点
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 
        /// </summary>
        public NodeInfo nodeinfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NodeInfo> linkinfo { get; set; }
    }

    /// <summary>
    /// 后台服务监控节点拓扑排序（Topological Sorting）
    /// </summary>
    public class NodeTopological : IKeyClass
    {
        /// <summary>
        /// 
        /// </summary>
        public NodeInfo NodeInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NodeTopological> Dependencies { get; set; }

        public string Key
        {
            get
            {
                return NodeInfo.Key;
            }
        }

        public override string ToString()
        {
            return NodeInfo.Key;
        }
    }
}
