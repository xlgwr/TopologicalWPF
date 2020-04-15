using ManageServerClient.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ManageServerClient.Shared.Entity
{
    /// <summary>
    /// 后台服务连接的其他服务配置信息
    /// </summary>
    public class LinkInfo : IKeyClass
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        [Description("服务类型")]
        public ClientType type { get; set; }
        /// <summary>
        /// 服务ip
        /// </summary>
        [Description("服务ip")]
        public string ip { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        [Description("端口")]
        public int port { get; set; }

        /// <summary>
        /// $"{ip}:{port}"
        /// </summary>
        public string Key
        {
            get
            {
                return $"{ip}:{port}";
            }
        }
        public string typeDesc
        {
            get
            {
                return type.GetDescription();
            }
        }
        public string ShowName
        {
            get
            {
                return $"{typeDesc}:{Key}";
            }
        }
        public override string ToString()
        {
            return Key;
        }
    }

}
