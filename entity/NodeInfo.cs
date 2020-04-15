using ManageServerClient.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ManageServerClient.Shared.Entity
{
    /// <summary>
    /// 后台服务基本信息
    /// </summary>
    public class NodeInfo : LinkInfo
    {
        public string num { get; set; }

        #region 画图相关
        /// <summary>
        /// 图层级，默认0级
        /// </summary>
        public int level { get; set; } = 0;
        /// <summary>
        /// 方格初始位置
        /// </summary>
        public int place { get; set; } = 0; 

        /// <summary>
        /// 显示位置
        /// </summary>
        public Point pointPlace { get; set; }

        #endregion
    }
}
