using ManageServerClient.Shared.Entity;
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
using ManageServerClient.Shared.Common;
using TopologicalWPF.Shapes;

namespace TopologicalWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 其它功能
        /// <summary>
        /// 缩放
        /// </summary>
        public Double scaleLevel = 1;
        public ScaleTransform totalScale = new ScaleTransform();
        #endregion

        List<NetworkNodeShape> networkNodeShapesList = new List<NetworkNodeShape>();

        #region 线记录
        /// <summary>
        /// 所有连线
        /// </summary>
        public Dictionary<string, List<UIElement>> Lines { get; set; } = new Dictionary<string, List<UIElement>>();
        /// <summary>
        /// 连线始发及终点
        /// </summary>
        public Dictionary<string, Tuple<Point, Point>> LinePoints { get; set; } = new Dictionary<string, Tuple<Point, Point>>();
        /// <summary>
        /// 对应显示信息
        /// </summary>
        public Dictionary<string, string> ShowMsgArrows { get; set; } = new Dictionary<string, string>();

        #endregion

        #region 点位置保存
        /// <summary>
        /// 路径
        /// </summary>
        public string configDirectory { get; set; } = "TopologicalConfig";
        /// <summary>
        /// 目录
        /// </summary>
        public string projectFilePath { get; set; }

        #endregion
        public MainWindow()
        {
            InitializeComponent();

            this.MouseWheel += MainWindow_MouseWheel;

            this.initWidthHeight();

            Canvas canvas = new Canvas() { Width = this.Width, Height = this.Height };
            this.Content = canvas;

            ToDo(canvas);

            toDoAddMenu();

            saveDefault();
        }


        #region 布局功能

        private void toDoAddMenu()
        {
            projectFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configDirectory);
            projectFilePath.FileDirectoryCreateDirectory();

            var menu = new ContextMenu();

            var menu1 = new MenuItem() { Header = "使用默认布局" };
            menu1.Click += Menu1_Click;

            var menu2 = new MenuItem() { Header = "使用自定义布局" };
            menu2.Click += Menu1_Click2;

            var menu3 = new MenuItem() { Header = "保存自定义布局" };
            menu3.Click += Menu1_Click3;

            menu.Items.Add(menu1);
            menu.Items.Add(menu2);
            menu.Items.Add(menu3);

            this.ContextMenu = menu;
        }
        private void saveDefault()
        {
            try
            {
                var toJson = networkNodeShapesList.ToDictionary(a => a.Key, a => a.PointNow).toJsonStr();
                var tmpPath = System.IO.Path.Combine(projectFilePath, $"{configDirectory}Default.json");
                tmpPath.FilePathSaveContent(toJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Menu1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmpPath = System.IO.Path.Combine(projectFilePath, $"{configDirectory}Default.json");
                loadProjectByFilePath(tmpPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Menu1_Click2(object sender, RoutedEventArgs e)
        {
            try
            {

                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "json选择布局 (*.json)|*.json";
                openFileDialog.Title = "选择布局";
                openFileDialog.InitialDirectory = projectFilePath;
                if ((bool)openFileDialog.ShowDialog())
                {
                    string tmpPath = openFileDialog.FileName;
                    loadProjectByFilePath(tmpPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadProjectByFilePath(string tmpPath)
        {
            var getContent = tmpPath.FilePathGetContent(Encoding.UTF8);
            if (getContent.IsNullOrWhiteSpace())
            {
                return;
            }
            var getlistCheckList = getContent.JsonTo<Dictionary<string, Point>>();
            foreach (var item in networkNodeShapesList)
            {
                if (getlistCheckList.ContainsKey(item.Key))
                {
                    item.PointNow = getlistCheckList[item.Key];
                }
                //刷新位置
                NetworkNodeShapePlaceChangeEnd(item, true);
            }
        }

        private void Menu1_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                var toJson = networkNodeShapesList.ToDictionary(a => a.Key, a => a.PointNow).toJsonStr();

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "json布局文件 (*.json)|*.json";
                saveFileDialog.Title = "保存当前选择布局";
                saveFileDialog.InitialDirectory = projectFilePath;
                var saveName = projectFilePath.Replace(@"\", "`").Replace(@":", "`").Replace(@"``", "`").Split('`');

                var saveNameLast = saveName[saveName.Length - 1];
                if (saveName.Length > 2)
                {
                    saveNameLast = saveName[0] + "_m_" + saveName[saveName.Length - 2] + "_" + saveName[saveName.Length - 1];
                }
                saveFileDialog.FileName = $"布局_{saveNameLast}_" + DateTime.Now.Date.ToString("yyyyMMdd");
                if ((bool)(saveFileDialog.ShowDialog()))
                {
                    //获得文件路径
                    var localFilePath = saveFileDialog.FileName.ToString();
                    localFilePath.FilePathSaveContent(toJson, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            var canvas = (Canvas)this.Content;
            if (e.Delta > 0)
            {
                scaleLevel *= 1.08;
            }
            else
            {
                scaleLevel /= 1.08;
            }

            var pointStart = e.GetPosition(this);
            canvas.adjustScale(pointStart, ref scaleLevel, ref totalScale);
        }
        public void ToDo(Canvas canvas)
        {
            var todoData = initData();
            //排序，依赖节点优先显示
            var sorted = todoData.SortTopological((x) => x.Dependencies);

            foreach (var item in sorted)
            {
                Console.WriteLine(item.NodeInfo.type.GetDescription() + ":" + item);
            }
            //计算图层级，初始位置
            var getDiclevel = CalcTopological(sorted);
            //画图点
            DrawTopological(canvas, getDiclevel);
            //画线
            DrawLine(canvas, sorted, true);
            //处理拖动
            MoveDragAll(canvas);

            Console.ReadLine();
        }
        private void DrawLine(Canvas canvas, List<NodeTopological> sorted, bool addLine = true)
        {
            foreach (var item in sorted)
            {
                if (!item.Dependencies.HasItem())
                {
                    continue;
                }
                var getCurrNetworkPorintStart = networkNodeShapesList.Where(a => a.Key == item.NodeInfo.Key).FirstOrDefault();
                foreach (var item2 in item.Dependencies)
                {
                    var getPoint = sorted.Where(a => a.NodeInfo.Key == item2.NodeInfo.Key).FirstOrDefault();
                    if (getPoint != null)
                    {
                        var point1 = item.NodeInfo.pointPlace;
                        var point2 = getPoint.NodeInfo.pointPlace;
                        var showmsg = item.NodeInfo.ShowName + "->" + getPoint.NodeInfo.ShowName;
                        var showmsgKey = item.NodeInfo.Key + "->" + getPoint.NodeInfo.Key;
                        if (addLine)
                        {
                            //线
                            var currLine = canvas.DrawLine(point1, point2, Brushes.Black, false, 1, showmsg);
                            Canvas.SetZIndex(currLine, -1);
                            //画箭头
                            var currArrow = canvas.DrawTextArrow(point1, point2, showmsg);

                            LinePoints[showmsgKey] = new Tuple<Point, Point>(point1, point2);
                            Lines[showmsgKey] = new List<UIElement>() { currLine, currArrow };
                            ShowMsgArrows[showmsgKey] = showmsg;

                            //记录线及箭头
                            if (getCurrNetworkPorintStart != null)
                            {
                                getCurrNetworkPorintStart.LinePoints[showmsgKey] = showmsg;
                                getCurrNetworkPorintStart.Lines[showmsgKey] = showmsg;
                                getCurrNetworkPorintStart.ShowMsgArrows[showmsgKey] = showmsg;

                                var getCurrNetworkPorintEnd = networkNodeShapesList.Where(a => a.Key == item2.NodeInfo.Key).FirstOrDefault();
                                if (getCurrNetworkPorintEnd != null)
                                {
                                    getCurrNetworkPorintEnd.LinePointsRef[showmsgKey] = showmsg;
                                    getCurrNetworkPorintEnd.LinesRef[showmsgKey] = showmsg;
                                    getCurrNetworkPorintEnd.ShowMsgArrowsRef[showmsgKey] = showmsg;
                                }
                            }
                        }
                        else
                        {
                            //获得取中点，x 移30
                            double angleOfLine = point1.GetAngle(point2);
                            double distinctLeng = point1.GetDistance(point2);
                            var endPointText = point1.GetEndPointByTrigonometric(angleOfLine, (distinctLeng / 2));
                            var DataPoints = new PointCollection() {
                                item.NodeInfo.pointPlace, endPointText,  getPoint.NodeInfo.pointPlace };
                            canvas.DrawPathBezierSegment(DataPoints, Colors.Green, 20);
                        }
                    }
                }
            }
        }

        public void DrawTopological(Canvas canvas, Dictionary<int, List<NodeTopological>> nodeTopologicals)
        {
            var evenHeight = canvas.Height / nodeTopologicals.Count;//平均高
            var evenWidth = canvas.Width / nodeTopologicals.Values.Max(a => a.Count);//平均宽

            foreach (var item in nodeTopologicals)
            {
                var pointPlace = new Point(0, canvas.Height - (evenHeight / 2 + item.Key * evenHeight) - evenHeight / 3);
                foreach (var item2 in item.Value)
                {
                    pointPlace.X = evenWidth / 2 + item2.NodeInfo.place * evenWidth;

                    if (item.Key > 0)
                    {
                        pointPlace.X += (item.Key % 2) * (evenWidth / 3);
                    }

                    item2.NodeInfo.pointPlace = pointPlace;

                    //DrawEllipse(canvas, pointPlace, item, item2);
                    DrawNodeShape(canvas, pointPlace, item, item2);
                }

            }

        }

        private void DrawEllipse(Canvas canvas, Point pointPlace, KeyValuePair<int, List<NodeTopological>> levelNode, NodeTopological currNode)
        {
            canvas.DrawEllipse(pointPlace, 20, Colors.Red, currNode);
            Point pointText = new Point(pointPlace.X, pointPlace.Y);
            string toShow = currNode.NodeInfo.typeDesc + ":" + currNode.NodeInfo.Key;

            var textblock = canvas.DrawTextBlock(pointText, toShow);
            if (levelNode.Key > 0)
            {
                pointText.X = pointPlace.X + 20;
                pointText.Y -= 20;
                canvas.SetLeftTop(textblock, pointText);
            }
        }
        private void DrawNodeShape(Canvas canvas, Point pointPlace, KeyValuePair<int, List<NodeTopological>> levelNode, NodeTopological currNode)
        {
            NetworkNodeShape networkNodeShape = new NetworkNodeShape() { Key = currNode.Key };

            Point pointText = new Point(pointPlace.X - networkNodeShape.Width / 2, pointPlace.Y - networkNodeShape.Height / 2);

            string toShowDesc = currNode.NodeInfo.Key;
            string toShowName = currNode.NodeInfo.typeDesc;
            networkNodeShape.rShowNode.Fill = new SolidColorBrush(Colors.Red);
            networkNodeShape.txtDesc.Text = toShowDesc;
            networkNodeShape.txtName.Text = toShowName;
            networkNodeShape.PointNow = pointText;

            canvas.Children.Add(networkNodeShape);
            canvas.SetLeftTop(networkNodeShape, pointText);
            networkNodeShapesList.Add(networkNodeShape);

        }

        /// <summary>
        /// 计算图层级，初始位置
        /// </summary>
        /// <param name="nodeTopologicals"></param>
        public Dictionary<int, List<NodeTopological>> CalcTopological(List<NodeTopological> nodeTopologicals)
        {
            //记录已画
            var hasCalc = new Dictionary<string, NodeInfo>();
            var levelCalc = new Dictionary<int, List<NodeTopological>>();

            //层级初始位置
            var actionPlace = new Action<NodeTopological>((item) =>
            {
                if (levelCalc.ContainsKey(item.NodeInfo.level))
                {
                    item.NodeInfo.place = levelCalc[item.NodeInfo.level].Count;
                    levelCalc[item.NodeInfo.level].Add(item);
                }
                else
                {
                    item.NodeInfo.place = 0;
                    levelCalc[item.NodeInfo.level] = new List<NodeTopological>() { item };
                }
            });


            //计算图层级
            foreach (var item in nodeTopologicals)
            {

                if (!item.Dependencies.HasItem())
                {
                    hasCalc[item.Key] = item.NodeInfo;

                    //层级初始位置
                    actionPlace.Invoke(item);
                    continue;
                }
                var allKeyDep = item.Dependencies.Select(a => a.Key);

                //层级
                int level = hasCalc.Where(a => allKeyDep.Contains(a.Key)).Select(a => a.Value.level).Max();
                item.NodeInfo.level = level + 1;
                hasCalc[item.Key] = item.NodeInfo;

                //层级初始位置
                actionPlace.Invoke(item);
            }
            //
            return levelCalc;
        }
        /// <summary>
        /// 生成拓扑数据
        /// 用于拓扑排序，
        /// 加载依赖节点
        /// </summary>
        /// <returns></returns>
        public List<NodeTopological> initData()
        {
            //交易服务
            var server6 = new Node()
            {
                nodeinfo = new NodeInfo() { num = "007", ip = "0.0.0.0", port = 10011, type = ClientType.TRADE_SERVER },
                linkinfo = new List<NodeInfo>()
                {
                    new NodeInfo(){ip = "0.0.0.0", port = 10012, type = ClientType.TRADE_FRONT},
                    new NodeInfo(){ip = "0.0.0.0", port = 10013, type = ClientType.TRADE_GATEWAY},
                    new NodeInfo(){ip = "0.0.0.0", port = 10014, type = ClientType.NEWS_SERVER},
                    new NodeInfo(){ip = "0.0.0.0", port = 10015, type = ClientType.SMS_SERVER},

                }
            };

            //交易前置
            var server5 = new Node()
            {
                nodeinfo = new NodeInfo() { num = "006", ip = "0.0.0.0", port = 10012, type = ClientType.TRADE_FRONT },
                linkinfo = new List<NodeInfo>()
                {
                    new NodeInfo(){ip = "0.0.0.0", port = 10011, type = ClientType.TRADE_SERVER},
                    new NodeInfo(){ip = "0.0.0.0", port = 10016, type = ClientType.CONFIRM_SERVER},
                    new NodeInfo(){ip = "0.0.0.0", port = 10014, type = ClientType.NEWS_SERVER},
                    new NodeInfo(){ip = "0.0.0.0", port = 10015, type = ClientType.SMS_SERVER},

                }
            };

            //认证服务
            var server16 = new Node()
            {
                nodeinfo = new NodeInfo() { num = "005", ip = "0.0.0.0", port = 10016, type = ClientType.CONFIRM_SERVER },
                linkinfo = new List<NodeInfo>()
                {
                    new NodeInfo(){ip = "0.0.0.0", port = 10014, type = ClientType.NEWS_SERVER},
                    new NodeInfo(){ip = "0.0.0.0", port = 10015, type = ClientType.SMS_SERVER},

                }
            };
            //交易网关
            var server7 = new Node()
            {
                nodeinfo = new NodeInfo() { ip = "0.0.0.0", port = 10013, type = ClientType.TRADE_GATEWAY }
            };
            //消息服务
            var server17 = new Node()
            {
                nodeinfo = new NodeInfo() { ip = "0.0.0.0", port = 10014, type = ClientType.NEWS_SERVER }
            };
            //短信服务
            var server19 = new Node()
            {
                nodeinfo = new NodeInfo() { ip = "0.0.0.0", port = 10015, type = ClientType.SMS_SERVER },
            };

            var realData = new List<Node>() { server16, server5, server7, server6, server17, server19 };
            var toTopData = new List<NodeTopological>();

            //生成拓扑数据
            foreach (var item in realData)
            {
                var currNodeTop = new NodeTopological() { NodeInfo = item.nodeinfo };
                currNodeTop.Dependencies = new List<NodeTopological>();

                if (item.linkinfo != null)
                {
                    foreach (var item2 in item.linkinfo)
                    {
                        currNodeTop.Dependencies.Add(new NodeTopological() { NodeInfo = item2 });
                    }
                }

                toTopData.Add(currNodeTop);
            }

            return toTopData;
        }

        /// <summary>
        /// 实现拖动
        /// </summary>
        /// <param name="canvas"></param>
        public void MoveDragAll(Canvas canvas)
        {
            foreach (UIElement uiEle in canvas.Children)
            {
                //WPF设计上的问题,Button.Clicked事件Supress掉了Mouse.MouseLeftButtonDown附加事件等.
                //不加这个Button、TextBox等无法拖动
                if (uiEle is Button || uiEle is TextBox || uiEle is NetworkNodeShape)
                {
                    uiEle.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeftButtonDown), true);
                    uiEle.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(Element_MouseMove), true);
                    uiEle.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Element_MouseLeftButtonUp), true);
                    continue;
                }
                //
                uiEle.MouseMove += new MouseEventHandler(Element_MouseMove);
                uiEle.MouseLeftButtonDown += new MouseButtonEventHandler(Element_MouseLeftButtonDown);
                uiEle.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            }
        }
        bool isDragDropInEffect = false;
        Point pos = new Point();
        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement ele = sender as FrameworkElement;
                isDragDropInEffect = false;
                ele.ReleaseMouseCapture();
                if (ele is NetworkNodeShape)
                {
                    Canvas canvas = (Canvas)this.Content;
                    var currnetwork = (NetworkNodeShape)ele;
                    currnetwork.PointNow = currnetwork.TranslatePoint(new Point(), canvas);
                    NetworkNodeShapePlaceChangeEnd(currnetwork);
                }
            }
        }
        /// <summary>
        /// 刷新线及点
        /// </summary>
        /// <param name="networkNode"></param>
        public void NetworkNodeShapePlaceChangeEnd(NetworkNodeShape networkNode, bool isRefPlace = false)
        {
            try
            {
                Canvas canvas = (Canvas)this.Content;
                Point pointEnd = new Point(networkNode.PointNow.X + networkNode.Width / 2, networkNode.PointNow.Y + networkNode.Height / 2);

                if (isRefPlace)
                {
                    Point pointText = new Point(networkNode.PointNow.X - networkNode.Width / 2, networkNode.PointNow.Y - networkNode.Height / 2);
                    pointEnd = networkNode.PointNow;

                    canvas.SetLeftTop(networkNode, pointText);
                }

                DrawLineAndArrow(canvas, networkNode, pointEnd, false);
                DrawLineAndArrow(canvas, networkNode, pointEnd, true);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 更新线及箭头
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="networkNode"></param>
        /// <param name="pointEnd"></param>
        /// <param name="isNoRef"></param>
        /// <returns></returns>
        private void DrawLineAndArrow(Canvas canvas, NetworkNodeShape networkNode, Point pointEnd, bool isNoRef = false)
        {
            Dictionary<string, Tuple<Point, Point>> LinePointsTmp = new Dictionary<string, Tuple<Point, Point>>();
            Dictionary<string, List<UIElement>> LinesTmp = new Dictionary<string, List<UIElement>>();


            var currLinePoints = isNoRef ? this.LinePoints.Where(a => networkNode.LinePoints.ContainsKey(a.Key)) : this.LinePoints.Where(a => networkNode.LinePointsRef.ContainsKey(a.Key));
            var currShowMsgArrows = isNoRef ? this.ShowMsgArrows.Where(a => networkNode.ShowMsgArrows.ContainsKey(a.Key)) : this.ShowMsgArrows.Where(a => networkNode.ShowMsgArrowsRef.ContainsKey(a.Key));
            var currlines = isNoRef ? this.Lines.Where(a => networkNode.Lines.ContainsKey(a.Key)) : this.Lines.Where(a => networkNode.LinesRef.ContainsKey(a.Key));

            //清理线及点
            foreach (var item in currlines)
            {
                canvas.Clear(item.Value);
            }

            //重新画线-收到的
            foreach (var item in currLinePoints)
            {
                var point1 = pointEnd;
                var point2 = item.Value.Item2;

                if (!isNoRef)
                {
                    point1 = item.Value.Item1;
                    point2 = pointEnd;
                }
                string showmsg = currShowMsgArrows.Where(a => a.Key == item.Key).First().Value;
                //线
                var currLine = canvas.DrawLine(point1, point2, Brushes.Black, false, 1, showmsg);
                Canvas.SetZIndex(currLine, -1);
                //画箭头
                var currArrow = canvas.DrawTextArrow(point1, point2, showmsg);

                LinePointsTmp[item.Key] = new Tuple<Point, Point>(point1, point2);
                LinesTmp[item.Key] = new List<UIElement>() { currLine, currArrow };
            }

            foreach (var item in LinesTmp)
            {
                this.Lines[item.Key] = item.Value;
            }
            foreach (var item in LinePointsTmp)
            {
                this.LinePoints[item.Key] = item.Value;
            }
        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragDropInEffect)
            {
                FrameworkElement currEle = sender as FrameworkElement;
                double xPos = e.GetPosition(null).X - pos.X + (double)currEle.GetValue(Canvas.LeftProperty);
                double yPos = e.GetPosition(null).Y - pos.Y + (double)currEle.GetValue(Canvas.TopProperty);
                currEle.SetValue(Canvas.LeftProperty, xPos);
                currEle.SetValue(Canvas.TopProperty, yPos);
                pos = e.GetPosition(null);
            }
        }

        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fEle = sender as FrameworkElement;
            isDragDropInEffect = true;
            pos = e.GetPosition(null);
            fEle.CaptureMouse();
            fEle.Cursor = Cursors.Hand;
        }
    }
}
