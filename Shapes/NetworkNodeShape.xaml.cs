using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TopologicalWPF.Shapes
{
    /// <summary>
    /// NetworkNodeShape.xaml 的交互逻辑
    /// </summary>
    public partial class NetworkNodeShape : UserControl
    {
        public string Key { get; set; }
        public Point PointNow { get; set; }

        #region 发出的线
        /// <summary>
        /// 所有连线
        /// </summary>
        public Dictionary<string, string> Lines { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 连线始发及终点
        /// </summary>
        public Dictionary<string, string> LinePoints { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 对应显示信息
        /// </summary>
        public Dictionary<string, string> ShowMsgArrows { get; set; } = new Dictionary<string, string>();

        #endregion
        #region 收到的线
        /// <summary>
        /// 所有连线
        /// </summary>
        public Dictionary<string, string> LinesRef { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 连线始发及终点
        /// </summary>
        public Dictionary<string, string> LinePointsRef { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 对应显示信息
        /// </summary>
        public Dictionary<string, string> ShowMsgArrowsRef { get; set; } = new Dictionary<string, string>();

        #endregion 

        public NetworkNodeShape()
        {
            InitializeComponent();

            initFirst();
        }

        public void initFirst()
        {
            this.txtDesc.Text = "";
        }
    }
}
