using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace System.Windows.Shapes
{
    /// <summary>
    /// 主要用于基本图形生成扩展
    /// </summary>
    public static class BaseShapsExt
    {

        /// <summary>
        /// 画箭头 
        /// 扩展方法
        /// 也就是三角形
        /// </summary>
        /// <param name="colorBrush">填充颜色 Color.FromRgb(0xff, 0, 0)</param>
        /// <param name="startPoint">路径的起点</param>
        /// <param name="twoPoint">第2个点</param>
        /// <param name="threePoint">第3个点</param>
        public static Path DrawArrow(this Canvas canvas,
            Brush colorBrush,
            Point startPoint,
            Point twoPoint,
            Point threePoint
            )
        {
            Path pathArrow = new Path();
            pathArrow.Fill = colorBrush;

            PathFigure pathFigureArrow = new PathFigure();
            pathFigureArrow.IsClosed = true;
            pathFigureArrow.StartPoint = startPoint;
            pathFigureArrow.Segments.Add(new LineSegment(twoPoint, false));
            pathFigureArrow.Segments.Add(new LineSegment(threePoint, false));

            PathGeometry pathGeometryArrow = new PathGeometry();
            pathGeometryArrow.Figures.Add(pathFigureArrow);

            pathArrow.Data = pathGeometryArrow;

            canvas.Children.Add(pathArrow);

            return pathArrow;
        }

        /// <summary>
        /// 曲线绘制，并填充色
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="colorBrush"></param>
        /// <param name="fillBrush">并填充色</param>
        /// <param name="startPoint"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Path DrawPolylineAndFill(this Canvas canvas,
           Color colorBrush,
           Brush fillBrush,
           Point startPoint,
           PointCollection points)
        {
            Path currPath = new Path();
            if (!points.Any())
            {
                return currPath;
            }
            currPath.Stroke = Brushes.Black;

            var endpoint = new Point() { X = points.Last().X + 10, Y = startPoint.Y };
            var newStartPoint = new Point() { X = startPoint.X, Y = startPoint.Y };

            var maxX = points.Max(a => a.X);
            var maxY = points.Max(a => a.Y);

            GradientStopCollection gradientStops = new GradientStopCollection()
            {
               new GradientStop(){ Color= Colors.LightSeaGreen, Offset=1},
               new GradientStop(){ Color= Colors.LightPink, Offset=0.8},
               new GradientStop(){ Color= Colors.Black, Offset=0.2}

            };
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(gradientStops, 90);

            PathFigure pathFigureArrow = new PathFigure();
            pathFigureArrow.IsClosed = true;
            pathFigureArrow.StartPoint = newStartPoint;
            foreach (var item in points)
            {
                var toadd = new Point(item.X + 10, item.Y + 5);
                pathFigureArrow.Segments.Add(new LineSegment(toadd, false));
            }
            pathFigureArrow.Segments.Add(new LineSegment(endpoint, false));

            PathGeometry pathGeometryArrow = new PathGeometry();
            pathGeometryArrow.Figures.Add(pathFigureArrow);

            currPath.Data = pathGeometryArrow;

            currPath.Fill = fillBrush == null ? linearGradientBrush : fillBrush;

            Canvas.SetZIndex(currPath, -99);

            canvas.Children.Add(currPath);

            return currPath;
        }
        /// <summary>
        /// 画坐标中点到X,Y线的直线
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point">要画线的点</param>
        /// <param name="pointStart">XY的起始点</param>
        /// <param name="isTen">是否画十字虚线</param>
        /// <returns></returns>
        public static List<Line> DrawLineXYPoint(this Canvas canvas, Point point, Point startPoint, bool isTen = false)
        {
            var result = new List<Line>();
            double dTenLing1 = point.X;
            double dTenLing2 = point.Y;

            Brush brushColor = Brushes.LightGray;
            int lineWidth = 1;
            //十字线
            if (isTen)
            {
                brushColor = Brushes.LightGray;
                dTenLing1 = canvas.Width;//X线长终点
                dTenLing2 = -startPoint.X;//Y线长终点
                lineWidth = 2;
            }
            var strokeDashArray = new DoubleCollection() { 2, 3 };
            var line1 = DrawLine(canvas, new Point() { X = startPoint.X, Y = point.Y }, new Point() { X = dTenLing1, Y = point.Y }, brushColor, lineWidth, strokeDashArray);
            var line2 = DrawLine(canvas, new Point() { X = point.X, Y = startPoint.Y }, new Point() { X = point.X, Y = dTenLing2 }, brushColor, lineWidth, strokeDashArray);

            Canvas.SetZIndex(line1, -1);
            Canvas.SetZIndex(line2, -1);

            result.Add(line1);
            result.Add(line2);

            return result;
        }
        public static TextBlock DrawTextBlock(this Canvas canvas, Point point1, string msg, Color foregroundColor, double fontsize = 20, Visibility visibility = Visibility.Visible, Brush brushBK = null, bool isHorizontalShow = false)
        {
            Brush brushes = new SolidColorBrush(foregroundColor);
            return DrawTextBlock(canvas, point1, msg, brushes, fontsize, visibility, brushBK, isHorizontalShow);
        }
        public static TextBlock DrawTextBlock(this Canvas canvas, Point point1, string msg, Brush foregroundColor, double fontsize = 20, Visibility visibility = Visibility.Visible, Brush brushBK = null, bool isHorizontalShow = false)
        {
            Brush brushes = foregroundColor;
            TextBlock newtextBlock = new TextBlock() { Text = msg, FontSize = fontsize, Background = brushBK, Foreground = brushes == null ? Brushes.Black : brushes, Visibility = visibility };
            if (isHorizontalShow)
            {
                newtextBlock.HorizontalShow(newtextBlock.FontSize);
            }

            Canvas.SetTop(newtextBlock, point1.Y);
            Canvas.SetLeft(newtextBlock, point1.X);

            canvas.Children.Add(newtextBlock);

            return newtextBlock;
        }

        /// <summary>
        /// 基本画线
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point1">起点</param>
        /// <param name="point2">终点</param>
        /// <param name="brushStroke">颜色 System.Windows.Media.Brushes.Black</param>
        /// <param name="thickness">粗细</param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public static Line DrawLine(this Canvas canvas, Point point1, Point point2, Brush brushStroke, double thickness = 2, DoubleCollection strokeDashArray = null, HorizontalAlignment horizontal = HorizontalAlignment.Left, VerticalAlignment vertical = VerticalAlignment.Center)
        {
            Line newLine = new Line();
            newLine.Stroke = brushStroke == null ? System.Windows.Media.Brushes.Black : brushStroke;
            newLine.X1 = point1.X;
            newLine.Y1 = point1.Y;
            newLine.X2 = point2.X;
            newLine.Y2 = point2.Y;
            newLine.HorizontalAlignment = horizontal;
            newLine.VerticalAlignment = vertical;
            newLine.StrokeThickness = thickness;
            newLine.StrokeDashArray = strokeDashArray;
            newLine.Uid = Guid.NewGuid().ToString();
            var getIndex = canvas.Children.Add(newLine);
            newLine.Tag = getIndex;

            return newLine;
        }
        /// <summary>
        /// 图形缩放
        ///  Double scaleLevel = 1;
        ///  ScaleTransform totalScale = new ScaleTransform();
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pointStart">图形缩放中心点</param>
        /// <param name="scaleLevel">比例大小</param>
        /// <param name="totalScale">比例实体</param>
        public static void adjustScale(this Canvas canvas, Point pointStart, ref double scaleLevel, ref ScaleTransform totalScale)
        {
            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = pointStart.X;
            totalScale.CenterY = pointStart.Y;
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(totalScale);
            canvas.RenderTransform = transform;
        }

        /// <summary>
        /// 生成n个随机点，X轴坐标是每隔x px出现一次，Y轴坐标的大小是随机生成的
        /// 及对应点画圆点
        /// </summary>
        /// <param name="chartCanvas"></param>
        public static Tuple<List<Ellipse>, PointCollection> DrawPointGen<T1, T2>(this Canvas canvas,
            ScaleBase<T1> scaleBaseX, ScaleBase<T2> scaleBaseY,
            MouseButtonEventHandler mouseDownHandler,
            MouseEventHandler mouseLeaveHandler,
            MouseEventHandler MouseEnterHandler,
            Color colorEllipse,
            int genCount = 10, int ellipseSize = 10, double leftX = 10)
            where T1 : struct
            where T2 : struct
        {
            PointCollection points = new PointCollection();
            List<Ellipse> pointEllipses = new List<Ellipse>();
            //随机生成n个点 
            Random rPoint = new Random(DateTime.Now.Millisecond);
            int ranY = Convert.ToInt32(scaleBaseY.AllScaleNumber - 2);

            for (int i = 0; i < scaleBaseX.AllScaleNumber; i++)
            {
                double x_point = leftX + Math.Round(i * scaleBaseX.minScale * scaleBaseX.ScaleRate);//ellipseSize是为了补偿圆点的大小，到精确的位置
                double y_point = Math.Round(rPoint.Next(ranY) * scaleBaseY.minScale * scaleBaseY.ScaleRate);


                var newpoint = new Point(x_point, y_point);

                points.Add(newpoint);

                //生成圆
                #region 生成圆
                Ellipse newEllipse = canvas.DrawEllipse(newpoint, 10, colorEllipse, i.ToString(), false);

                newEllipse.MouseDown += mouseDownHandler;
                newEllipse.MouseLeave += mouseLeaveHandler;
                newEllipse.MouseEnter += MouseEnterHandler;

                pointEllipses.Add(newEllipse);

                #endregion
            }
            return new Tuple<List<Ellipse>, PointCollection>(pointEllipses, points);
        }
        /// <summary>
        /// 真实数据生成点
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="canvas"></param>
        /// <param name="xyScaleBase"></param>
        /// <param name="keyListName"></param>
        /// <param name="colorEllipse"></param>
        /// <param name="ellipseSize"></param>
        /// <param name="showEllipse"></param>
        /// <param name="mouseDownHandler"></param>
        /// <param name="mouseLeaveHandler"></param>
        /// <param name="MouseEnterHandler"></param>
        /// <returns></returns>
        public static Tuple<List<Ellipse>, PointCollection> DrawPointByPointData<T1, T2>(this Canvas canvas,
            XYScaleBase<T1, T2> xyScaleBase, string keyListName,
            Color colorEllipse,
            int ellipseSize = 10,
            bool showEllipse = false,
            MouseButtonEventHandler mouseDownHandler = null,
            MouseEventHandler mouseLeaveHandler = null,
            MouseEventHandler MouseEnterHandler = null)
            where T1 : struct
            where T2 : struct
        {
            PointCollection points = new PointCollection();
            List<Ellipse> pointEllipses = new List<Ellipse>();
            var pointDatas = xyScaleBase.PointDataValuesDic[keyListName];

            foreach (var item in pointDatas)
            {
                var newpoint = item.GetPoint(xyScaleBase);
                points.Add(newpoint);

                //生成圆
                #region 生成圆
                Ellipse newEllipse = canvas.DrawEllipse(newpoint, 10, colorEllipse, item, showEllipse, mouseDownHandler, mouseLeaveHandler, MouseEnterHandler);

                pointEllipses.Add(newEllipse);

                #endregion
            }
            return new Tuple<List<Ellipse>, PointCollection>(pointEllipses, points);
        }
        /// <summary>
        /// 生成圆点
        /// </summary>
        /// <param name="chartCanvas"></param>
        /// <param name="newpoint"></param>
        /// <param name="ellipseSize"></param>
        /// <param name="color"></param>
        /// <param name="tag"></param>
        /// <param name="isAddtoCanvas"></param>
        /// <returns></returns>
        public static Ellipse DrawEllipse(this Canvas chartCanvas, Point newpoint, double ellipseSize, Color color, object tag, bool isAddtoCanvas = true,
            MouseButtonEventHandler mouseDownHandler = null,
            MouseEventHandler mouseLeaveHandler = null,
            MouseEventHandler MouseEnterHandler = null)
        {
            //生成圆
            #region 生成圆
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = new SolidColorBrush(color);//Color.FromRgb(0, 0, 0xff)
            newEllipse.Width = ellipseSize;
            newEllipse.Height = ellipseSize;

            Canvas.SetLeft(newEllipse, Math.Round(newpoint.X - ellipseSize / 2));
            Canvas.SetTop(newEllipse, Math.Round(newpoint.Y - ellipseSize / 2));

            newEllipse.Tag = tag;

            //newEllipse.ToolTip = $"{newpoint.X}_{newpoint.Y}";

            newEllipse.Uid = Guid.NewGuid().ToString();

            if (mouseDownHandler != null)
            {
                newEllipse.MouseDown += mouseDownHandler;
            }
            if (mouseLeaveHandler != null)
            {
                newEllipse.MouseLeave += mouseLeaveHandler;
            }
            if (MouseEnterHandler != null)
            {
                newEllipse.MouseEnter += MouseEnterHandler;
            }

            if (isAddtoCanvas)
            {
                chartCanvas.Children.Add(newEllipse);
            }
            return newEllipse;
            #endregion
        }


        /// <summary>
        /// 曲线绘制
        /// </summary>
        public static Path DrawCurve(this Canvas chartCanvas, PointCollection points, Color color, int ellipseSize = 10)
        {
            Polyline curvePolyline = new Polyline();

            curvePolyline.Stroke = new SolidColorBrush(color);
            curvePolyline.StrokeThickness = 1;

            //虚线
            //curvePolyline.StrokeDashArray = new DoubleCollection() { 1, 1, 2 };
            //curvePolyline.StrokeDashOffset = 1;
            //curvePolyline.StrokeDashCap = PenLineCap.Round;
            //curvePolyline.StrokeStartLineCap = PenLineCap.Round;
            //curvePolyline.StrokeEndLineCap = PenLineCap.Round;

            curvePolyline.Points = points;
            chartCanvas.Children.Add(curvePolyline);

            return new Path();

        }
        /// <summary>
        /// 使用了RotateTransform和DoubleAnimation实现转动动画。动画的时间长度根据角度大小分配，1度8个毫秒
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="point">圆心</param>
        /// <param name="angleNext">下个角度</param>
        /// <param name="angelCurrent">当前角度</param>
        public static void RotateTransformUI(this UIElement uIElement, Point point, double angleNext, double angelCurrent = 0)
        {
            RotateTransform rt = new RotateTransform();
            rt.CenterX = point.X;
            rt.CenterY = point.Y;
            uIElement.RenderTransform = rt;

            double timeAnimation = Math.Abs(angelCurrent - angleNext) * 8;
            DoubleAnimation da = new DoubleAnimation(angelCurrent, angleNext, new Duration(TimeSpan.FromMilliseconds(timeAnimation)));
            da.AccelerationRatio = 1;

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }
        /// <summary>
        /// 一起实现旋转和缩放效果
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="angleNext"></param>
        /// <param name="scaleX"></param>
        public static void RotateTransformUI(this UIElement uIElement, double angleNext = 30, double scaleX = 1)
        {

            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = 1;
            transformGroup.Children.Add(scaleTransform);
            RotateTransform rotateTransform = new RotateTransform(angleNext);
            transformGroup.Children.Add(rotateTransform);
            uIElement.RenderTransform = transformGroup;
        }

        /// <summary>
        /// 画贝塞尔曲线
        /// 曲线需要（开始点，结束点，控制点1，控制点2）
        /// </summary>
        /// <param name="canvas"></param>
        public static Path DrawPathBezierSegment(this Canvas canvas, PointCollection points, Color color, int ellipseSize = 10, bool isAddPoint = false)
        {
            if (!points.Any())
            {
                return new Path();
            }
            var toDrawPoint = points.OrderBy(a => a.X).ToList();
            var pf = new PathFigure { StartPoint = toDrawPoint[0] };

            var toForCount = toDrawPoint.Count - 1;

            for (var i = 0; i < toForCount; i++)
            {
                int current = i, last = i - 1, next = i + 1, next2 = i + 2;

                if (last == -1)
                {
                    last = 0;
                }
                if (next == toDrawPoint.Count)
                {
                    next = toForCount;
                }
                if (next2 == toDrawPoint.Count)
                {
                    next2 = toForCount;
                }
                var bzs = canvas.GetBezierSegment(toDrawPoint[current], toDrawPoint[last], toDrawPoint[next], toDrawPoint[next2], 0.7);
                pf.Segments.Add(bzs);
            }
            if (isAddPoint)
            {
                //画点的圆圈 
                foreach (var lipt in toDrawPoint)
                {
                    var ellipse = new Ellipse
                    {
                        Width = ellipseSize,
                        Height = ellipseSize,
                        Margin = new Thickness(lipt.X - ellipseSize / 2, lipt.Y, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Fill = Brushes.Red,
                        ToolTip = string.Format("x:{0},y:{1} ", lipt.X, lipt.Y)
                    };
                    canvas.Children.Add(ellipse);
                };
            }

            //添加曲线到图上
            var pfc = new PathFigureCollection { pf };
            var pg = new PathGeometry(pfc);
            var path = new Path { StrokeThickness = 1, Stroke = new SolidColorBrush(color), Data = pg };

            canvas.Children.Add(path);
            return path;

        }

        /// <summary>
        /// 获得贝塞尔曲线 
        /// 曲线需要（开始点，结束点，控制点1，控制点2）
        /// 代码主要是计算两个红色的控制点。
        /// 先计算相邻点的中点【橙色】。
        /// 再将中点的连线平移到相邻的位置【蓝色点】，取得虚线，得到虚线的端点【红色】。
        /// 红色，即为控制点 
        /// </summary>
        /// <param name="currentPt">当前点</param>
        /// <param name="lastPt">上一个点</param>
        /// <param name="nextPt1">下一个点1</param>
        /// <param name="nextPt2">下一个点2</param>
        /// <param name="coefficient">曲率0-1,控制点向当前点靠近一定的系数</param>
        /// <returns></returns>
        public static BezierSegment GetBezierSegment(this Canvas canvas, Point currentPt, Point lastPt, Point nextPt1, Point nextPt2, double coefficient = 0.5)
        {
            //计算中点
            var lastC = GetCenterPoint(lastPt, currentPt);
            var nextC1 = GetCenterPoint(currentPt, nextPt1); //贝塞尔控制点
            var nextC2 = GetCenterPoint(nextPt1, nextPt2);

            //计算相邻中点连线跟目的点的垂足
            //效果并不算太好，因为可能点在两个线上或者线的延长线上，计算会有误差
            //所以就直接使用中点平移方法。
            //var C1 = GetFootPoint(lastC, nextC1, currentPt);
            //var C2 = GetFootPoint(nextC1, nextC2, nextPt1); 

            //计算“相邻中点”的中点
            var c1 = GetCenterPoint(lastC, nextC1);
            var c2 = GetCenterPoint(nextC1, nextC2);

            //计算【"中点"的中点】需要的点位移
            var controlPtOffset1 = currentPt - c1;
            var controlPtOffset2 = nextPt1 - c2;

            //移动控制点
            var controlPt1 = nextC1 + controlPtOffset1;
            var controlPt2 = nextC1 + controlPtOffset2;

            //如果觉得曲线幅度太大，可以将控制点向当前点靠近一定的系数。
            controlPt1 = controlPt1 + coefficient * (currentPt - controlPt1);
            controlPt2 = controlPt2 + coefficient * (nextPt1 - controlPt2);

            var bzs = new BezierSegment(controlPt1, controlPt2, nextPt1, true);
            return bzs;
        }
        /// <summary>
        /// 计算“相邻中点”的中点
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static Point GetCenterPoint(this Point pt1, Point pt2)
        {
            return new Point((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
        }
        /// <summary>
        /// 过c点做A和B连线的垂足
        /// </summary>
        /// <param name="aPoint"></param>
        /// <param name="bPoint"></param>
        /// <param name="cPoint"></param>
        /// <returns></returns>
        public static Point GetFootPoint(this Point aPoint, Point bPoint, Point cPoint)
        {
            //设三点坐标是A，B，C，AB构成直线，C是线外的点
            //三点对边距离是a,b,c,垂足为D，
            //根据距离推导公式得：AD距离是（b平方-a平方+c平方）/2c
            //可能没考虑点c在线ab上的情况
            var offsetADist = (Math.Pow(cPoint.X - aPoint.X, 2) + Math.Pow(cPoint.Y - aPoint.Y, 2) - Math.Pow(bPoint.X - cPoint.X, 2) - Math.Pow(bPoint.Y - cPoint.Y, 2) + Math.Pow(aPoint.X - bPoint.X, 2) + Math.Pow(aPoint.Y - bPoint.Y, 2)) / (2 * GetDistance(aPoint, bPoint));

            var v = bPoint - aPoint;
            var distab = GetDistance(aPoint, bPoint);
            var offsetVector = v * offsetADist / distab;
            return aPoint + offsetVector;
        }
        public static double GetDistance(this Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        public static void AddList(this Canvas canvas, List<UIElement> uIElements)
        {
            foreach (var uI in uIElements)
            {
                canvas.Children.Add(uI);
            }
        }
        /// <summary>
        /// 显示层级
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="uIElement"></param>
        public static void SetZIndex(this Canvas canvas, UIElement uIElement)
        {
            Canvas.SetZIndex(uIElement, canvas.Children.Count + 1);
        }
        /// <summary>
        /// 显示位置
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="uIElement"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public static void SetLeftTop(this Canvas canvas, UIElement uIElement, double left, double top, bool isAdd = false)
        {
            Canvas.SetLeft(uIElement, left);
            Canvas.SetTop(uIElement, top);
            if (isAdd)
            {
                canvas.Children.Add(uIElement);
            }
        }
        public static void SetLeftTop(this Canvas canvas, UIElement uIElement, Point point, bool isAdd = false)
        {
            Canvas.SetLeft(uIElement, point.X);
            Canvas.SetTop(uIElement, point.Y);
            if (isAdd)
            {
                canvas.Children.Add(uIElement);
            }
        }
        /// <summary>
        /// 批量清除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="canvas"></param>
        /// <param name="element"></param>
        public static void Clear<T>(this Canvas canvas, List<T> element)
            where T : UIElement
        {
            if (element == null || !element.Any())
            {
                return;
            }
            foreach (var item in element)
            {
                canvas.Children.Remove(item);
            }
            element.Clear();
        }
    }
}
