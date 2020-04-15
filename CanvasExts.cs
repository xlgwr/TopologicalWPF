using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace System.Windows.Shapes
{
    public static class CanvasExts
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
        public static Line DrawLine(this Canvas canvas, Point point1, Point point2, Brush brushStroke, bool addArrow = false, double thickness = 2, string showMsg = "", DoubleCollection strokeDashArray = null, HorizontalAlignment horizontal = HorizontalAlignment.Left, VerticalAlignment vertical = VerticalAlignment.Center)
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

            //剑头
            if (addArrow)
            {
                canvas.DrawTextArrow(point1, point2, showMsg);
            }
            return newLine;
        }
        /// <summary>
        /// 画面剑头
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="showMsg"></param>
        public static TextBlock DrawTextArrow(this Canvas canvas, Point point1, Point point2, string showMsg, double fontSize = 20)
        {
            ////剑头三个点
            //Point leftArrow1 = new Point();
            //Point topArrow2 = new Point();
            //Point bomArrow3 = new Point();
            ////剑头
            //int arrowSizeint = 3;//剑头宽度
            //int arrowSizeHeightint = 10;//高度

            //leftArrow1 = new Point(point2.X - arrowSizeint, point2.Y);
            //topArrow2 = new Point(point2.X + arrowSizeint, point2.Y);
            //bomArrow3 = new Point(point2.X, point2.Y - arrowSizeHeightint);
            //画剑头
            //Path pathArrowX = canvas.DrawArrow(Brushes.Black, leftArrow1, topArrow2, bomArrow3);

            double angleOfLine = point1.GetAngle(point2);
            double distinctLeng = point1.GetDistance(point2);
            var endPointText = point1.GetEndPointByTrigonometric(angleOfLine, (distinctLeng * 2 / 3));

            var textblock = canvas.DrawTextBlock(endPointText, "->", fontSize);
            textblock.ToolTip = showMsg;
            textblock.FontSize = fontSize;
            canvas.SetLeftTop(textblock, new Point(endPointText.X, endPointText.Y));
            textblock.RotateTransformUI(angleOfLine);

            return textblock;
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
        /// 一起实现旋转和缩放效果
        /// </summary>
        /// <param name="uIElement"></param>
        /// <param name="angleNext">角度</param>
        /// <param name="scaleX"></param>
        public static void RotateTransformUI(this UIElement uIElement, double angleNext = 30, double scaleX = 1)
        {

            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = scaleX;
            transformGroup.Children.Add(scaleTransform);
            RotateTransform rotateTransform = new RotateTransform(angleNext);
            transformGroup.Children.Add(rotateTransform);
            uIElement.RenderTransform = transformGroup;
        }
        public static void RotateTransformUI(this Shape uIElement, double angleNext = 30, double scaleX = 1)
        {

            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = scaleX;
            transformGroup.Children.Add(scaleTransform);
            RotateTransform rotateTransform = new RotateTransform(angleNext);
            transformGroup.Children.Add(rotateTransform);
            uIElement.RenderTransform = transformGroup;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartCanvas"></param>
        /// <param name="point"></param>
        /// <param name="strname"></param>
        /// <returns></returns>
        public static TextBlock DrawTextBlock(this Canvas chartCanvas, Point newpoint, string strname, double ellipseSize = 20, bool isAddtoCanvas = true)
        {
            TextBlock textBlock = new TextBlock() { Text = strname };

            double strleng = strname.Length * textBlock.FontSize;

            Canvas.SetLeft(textBlock, Math.Round(newpoint.X - strleng / 3));
            Canvas.SetTop(textBlock, Math.Round(newpoint.Y + ellipseSize / 2));

            if (isAddtoCanvas)
            {
                chartCanvas.Children.Add(textBlock);
            }

            return textBlock;
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

            //newEllipse.ToolTip = $"{newpoint.X}_{newpoint.Y}_{tag.toJsonStr()}";

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
        /// 两点直线角度
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double GetAngle(this Point pt1, Point pt2)
        {
            return Math.Atan2((pt2.Y - pt1.Y), (pt2.X - pt1.X)) * 180 / Math.PI;
        }
        /// <summary>
        /// 两点距离
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double GetDistance(this Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2));
        }
        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="StartPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static Point GetEndPointByTrigonometric(this Point StartPoint, double angle, double distance)
        {
            Point EndPoint = new Point();

            //角度转弧度
            var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            EndPoint.X = StartPoint.X + distance * Math.Cos(radian);
            EndPoint.Y = StartPoint.Y + distance * Math.Sin(radian);

            return EndPoint;
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
        /// 显示层级
        /// canvas.Children.Count + 1
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="uIElement"></param>
        public static void SetZIndex(this Canvas canvas, UIElement uIElement, int zindex)
        {
            Canvas.SetZIndex(uIElement, zindex);
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
        public static void initWidthHeight(this Window window, WindowState windowState = WindowState.Maximized, double scale = 0.9)
        {
            double x = SystemParameters.WorkArea.Width * scale;//得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height * scale;//得到屏幕工作区域高度
            //double x1 = SystemParameters.PrimaryScreenWidth * 0.9;//得到屏幕整体宽度
            //double y1 = SystemParameters.PrimaryScreenHeight * 0.9;//得到屏幕整体高度   
            window.Width = x;
            window.Height = y;
            window.WindowState = windowState;
        }
    }
}
