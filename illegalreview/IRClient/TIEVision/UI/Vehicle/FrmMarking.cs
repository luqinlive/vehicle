using DevExpress.XtraEditors;
using IRVision.DAL;
using IRVision.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TIEVison.COMMON;

namespace TIEVision.UI.Vehicle
{
    public partial class FrmMarking : XtraForm
    {
        private string mFileName = "";
        private Thread thLoadData = null;
        public delegate void DelegateBindHandler();
        CrossingInfoDAL dal = new CrossingInfoDAL();
        List<CrossingInfo> mCrossingList = new List<CrossingInfo>();
        List<Point> LaneLines = new List<Point>();
        List<Point> StopLine = new List<Point>();
        List<Point> ZebraCross = new List<Point>();
        List<Point> TrafficLights = new List<Point>();
        double scaleX =  0.0f;
        double scaleY = 0.0f;
        private Image mCurOperateImage = null;

        public FrmMarking()
        {
            InitializeComponent();
        }

        private void FrmMarking_Load(object sender, EventArgs e)
        {
            AddPointCursor = new Cursor(IRVision.Properties.Resources.add_point.GetHicon());
            loadData();
        }

        private void loadData()
        {
            try
            {
                if (thLoadData != null)
                {
                    thLoadData.Abort();
                }
                thLoadData = new Thread(new ThreadStart(GetListData));
                thLoadData.Start();
            }
            catch
            {

            }
        }

        public void GetListData()
        {
            mCrossingList = dal.GetCrossingInfos();
            try
            {
                this.Invoke(new DelegateBindHandler(BindDataSource));
            }
            catch
            { }
        }

        private void BindDataSource()
        {
            comboBoxEditCross.Properties.Items.Clear();
            foreach (var item in mCrossingList)
            {
                comboBoxEditCross.Properties.Items.Add(item.CROSSING_NAME);
            }
            
        }

        private void cutImage()
        {
            int cropHeight = Convert.ToInt32(textEdit_CropHeight.Text.ToString());
            Image selectedImage = mCurOperateImage;
            int cloneWidth = selectedImage.Width / 2;
            int cloneHeight = (selectedImage.Height - cropHeight) / 2;

            Rectangle cropRect = new Rectangle(0, cropHeight, cloneWidth, cloneHeight);
            Image img1 = cropImage(selectedImage, cropRect);
            cropRect = new Rectangle(cloneWidth, cropHeight, cloneWidth, cloneHeight);
            Image img2 = cropImage(selectedImage, cropRect);
            cropRect = new Rectangle(0, cloneHeight + cropHeight, cloneWidth, cloneHeight);
            Image img3 = cropImage(selectedImage, cropRect);
            cropRect = new Rectangle(cloneWidth, cloneHeight + cropHeight, cloneWidth, cloneHeight);
            Image img4 = cropImage(selectedImage, cropRect);
            pictureEdit1.Image = img1;
            pictureEdit2.Image = img2;
            pictureEdit3.Image = img3;
            pictureEdit4.Image = img4;

            scaleX = (double)img1.Width / (double)pictureEdit1.Width;
            scaleY = (double)img1.Height / (double)pictureEdit1.Height;
        }
        private void simpleButtonChoosePic_Click(object sender, EventArgs e)
        {
            
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "请选择要图片";
                ofd.Filter = "JPG图片|*.jpg|PNG图片|*.png|BMP图片|*.bmp|Gif图片|*.gif";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    mFileName = ofd.FileName;
                    mCurOperateImage = Image.FromFile(mFileName);
                   
                    cutImage();
                    //PicturePointToScreen
                    //int _width = (int)(Math.Round(rect.Width / scaleX));
                    //int _height = (int)(Math.Round(rect.Height / scaleY));


                    Polygons.Clear();
                    int width =pictureEdit1.Width;
                    int height = pictureEdit1.Height;
                    int padding = 10;
                    LaneLines.Clear();
                    //LaneLine
                    LaneLines.Add(new Point(padding, (height / 4) * 3));
                    int lane_wrap = ((width - 20) / 3);
                    LaneLines.Add(new Point(padding + lane_wrap, (height / 4)*3));
                    LaneLines.Add(new Point(padding + lane_wrap * 2, (height / 4) * 3));
                    LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));

                    LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                    LaneLines.Add(new Point(padding + lane_wrap * 2, height - padding));
                    LaneLines.Add(new Point(padding + lane_wrap * 1, height - padding));
                    LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));

                    Polygons.Add(LaneLines);

                    //StopLine
                    StopLine.Clear();
                    StopLine.Add(new Point(padding, ((height / 4) * 3) - padding));
                    StopLine.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3) - padding));

                    Polygons.Add(StopLine);

                    //ZebraCrossing
                   
                    if (checkEditZebra.Checked != true)
                    {
                        ZebraCross.Clear();
                        //ZebraCross.Add(new Point(0,0));
                        //ZebraCross.Add(new Point(0, 0));
                        //ZebraCross.Add(new Point(0, 0));
                        //ZebraCross.Add(new Point(0, 0));
                        ZebraCross.Add(new Point(-10, -1));
                        ZebraCross.Add(new Point(-10, -1));
                        ZebraCross.Add(new Point(-10, -1));
                        ZebraCross.Add(new Point(-10, -1));
                        Polygons.Add(ZebraCross);
                    }
                    else
                    {
                        ZebraCross.Clear();
                        ZebraCross.Add(new Point(padding, ((height / 4) * 2)));
                        ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 2)));
                        ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3 - 20)));
                        ZebraCross.Add(new Point(padding, ((height / 4) * 3 - 20)));
                        Polygons.Add(ZebraCross);
                    }

                    //TrafficLights
                    TrafficLights.Clear();
                    TrafficLights.Add(new Point(width / 2 - padding, padding));
                    TrafficLights.Add(new Point(width / 2 - padding+padding*2, padding));
                    TrafficLights.Add(new Point(width / 2 - padding + padding * 2, padding * 4));
                    TrafficLights.Add(new Point(width / 2 - padding  , padding * 4));
                    Polygons.Add(TrafficLights);


                }
            }
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private void panelControl1_Resize(object sender, EventArgs e)
        {
            int picPanelWidth = (panelControl1.Width - 15) / 2;
            int picPanelHeight = (panelControl1.Height - 15 - pictureEdit1.Location.Y) / 2;
            pictureEdit1.Width = picPanelWidth; pictureEdit1.Height = picPanelHeight;
            pictureEdit1.Location = new Point(5, pictureEdit1.Location.Y);

            pictureEdit2.Width = picPanelWidth; pictureEdit2.Height = picPanelHeight;
            pictureEdit2.Location = new Point(10 + picPanelWidth, pictureEdit1.Location.Y);

            pictureEdit3.Width = picPanelWidth; pictureEdit3.Height = picPanelHeight;
            pictureEdit3.Location = new Point(5, 5 + pictureEdit1.Location.Y + picPanelHeight);

            pictureEdit4.Width = picPanelWidth; pictureEdit4.Height = picPanelHeight;
            pictureEdit4.Location = new Point(10 + picPanelWidth, 5 + pictureEdit1.Location.Y + picPanelHeight);
        }

        // The "size" of an object for mouse over purposes.
        private const int object_radius = 4;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int over_dist_squared = object_radius * object_radius;

        // Each polygon is represented by a List<Point>.
        private List<List<Point>> Polygons = new List<List<Point>>();

        // Points for the new polygon.
        private List<Point> NewPolygon = null;

        // The current mouse position while drawing a new polygon.
        private Point NewPoint;

        // The polygon and index of the corner we are moving.
        private List<Point> MovingPolygon = null;
        private int MovingPoint = -1;
        private int OffsetX, OffsetY;

        // The add point cursor.
        private Cursor AddPointCursor;

        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            // See what we're over.
            Point mouse_pt = e.Location;
            List<Point> hit_polygon;
            int hit_point, hit_point2;
            Point closest_point;

            if (NewPolygon != null)
            {
                // We are already drawing a polygon.
                // If it's the right mouse button, finish this polygon.
                if (e.Button == MouseButtons.Right)
                {
                    // Finish this polygon.
                    if (NewPolygon.Count >= 2) Polygons.Add(NewPolygon);
                    NewPolygon = null;

                    // We no longer are drawing.
                    pictureEdit1.MouseMove += picCanvas_MouseMove_NotDrawing;
                    pictureEdit1.MouseMove -= picCanvas_MouseMove_Drawing;
                }
                else
                {
                    // Add a point to this polygon.
                    if (NewPolygon[NewPolygon.Count - 1] != e.Location)
                    {
                        NewPolygon.Add(e.Location);
                    }
                }
            }
            else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            {
                // Start dragging this corner.
                pictureEdit1.MouseMove -= picCanvas_MouseMove_NotDrawing;
                pictureEdit1.MouseMove += picCanvas_MouseMove_MovingCorner;
                pictureEdit1.MouseUp += picCanvas_MouseUp_MovingCorner;

                // Remember the polygon and point number.
                MovingPolygon = hit_polygon;
                MovingPoint = hit_point;

                // Remember the offset from the mouse to the point.
                OffsetX = hit_polygon[hit_point].X - e.X;
                OffsetY = hit_polygon[hit_point].Y - e.Y;
            }
            else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                out hit_point, out hit_point2, out closest_point))
            {
                // Add a point.
                //hit_polygon.Insert(hit_point + 1, closest_point);
            }
            else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
            {
                // Start moving this polygon.
                pictureEdit1.MouseMove -= picCanvas_MouseMove_NotDrawing;
                pictureEdit1.MouseMove += picCanvas_MouseMove_MovingPolygon;
                pictureEdit1.MouseUp += picCanvas_MouseUp_MovingPolygon;

                // Remember the polygon.
                MovingPolygon = hit_polygon;

                // Remember the offset from the mouse to the segment's first point.
                OffsetX = hit_polygon[0].X - e.X;
                OffsetY = hit_polygon[0].Y - e.Y;
            }
            else
            {
                // Start a new polygon.
                NewPolygon = new List<Point>();
                NewPoint = e.Location;
                NewPolygon.Add(e.Location);

                // Get ready to work on the new polygon.
                pictureEdit1.MouseMove -= picCanvas_MouseMove_NotDrawing;
                pictureEdit1.MouseMove += picCanvas_MouseMove_Drawing;
            }

            // Redraw.
            pictureEdit1.Invalidate();
            pictureEdit2.Invalidate();
            pictureEdit3.Invalidate();
        }

        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureEdit2.Invalidate();
            pictureEdit3.Invalidate();
        }

        // Move the next point in the new polygon.
        private void picCanvas_MouseMove_Drawing(object sender, MouseEventArgs e)
        {
            NewPoint = e.Location;
            pictureEdit1.Invalidate();
        }

        // Move the selected corner.
        private void picCanvas_MouseMove_MovingCorner(object sender, MouseEventArgs e)
        {
            // Move the point.
            MovingPolygon[MovingPoint] = new Point(e.X + OffsetX, e.Y + OffsetY);

            // Redraw.
            pictureEdit1.Invalidate();
        }

        // Finish moving the selected corner.
        private void picCanvas_MouseUp_MovingCorner(object sender, MouseEventArgs e)
        {
            pictureEdit1.MouseMove += picCanvas_MouseMove_NotDrawing;
            pictureEdit1.MouseMove -= picCanvas_MouseMove_MovingCorner;
            pictureEdit1.MouseUp -= picCanvas_MouseUp_MovingCorner;
        }

        // Move the selected polygon.
        private void picCanvas_MouseMove_MovingPolygon(object sender, MouseEventArgs e)
        {
            // See how far the first point will move.
            int new_x1 = e.X + OffsetX;
            int new_y1 = e.Y + OffsetY;

            int dx = new_x1 - MovingPolygon[0].X;
            int dy = new_y1 - MovingPolygon[0].Y;

            if (dx == 0 && dy == 0) return;

            // Move the polygon.
            for (int i = 0; i < MovingPolygon.Count; i++)
            {
                MovingPolygon[i] = new Point(
                    MovingPolygon[i].X + dx,
                    MovingPolygon[i].Y + dy);
            }

            // Redraw.
            pictureEdit1.Invalidate();
        }

        // Finish moving the selected polygon.
        private void picCanvas_MouseUp_MovingPolygon(object sender, MouseEventArgs e)
        {
            pictureEdit1.MouseMove += picCanvas_MouseMove_NotDrawing;
            pictureEdit1.MouseMove -= picCanvas_MouseMove_MovingPolygon;
            pictureEdit1.MouseUp -= picCanvas_MouseUp_MovingPolygon;
        }

        // See if we're over a polygon or corner point.
        private void picCanvas_MouseMove_NotDrawing(object sender, MouseEventArgs e)
        {
            Cursor new_cursor = Cursors.Cross;

            // See what we're over.
            Point mouse_pt = e.Location;
            List<Point> hit_polygon;
            int hit_point, hit_point2;
            Point closest_point;

            if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            {
                new_cursor = Cursors.Arrow;
            }
            else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                out hit_point, out hit_point2, out closest_point))
            {
                new_cursor = AddPointCursor;
            }
            else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
            {
                new_cursor = Cursors.Hand;
            }

            // Set the new cursor.
            if (pictureEdit1.Cursor != new_cursor)
            {
                pictureEdit1.Cursor = new_cursor;
            }
        }

        private void pictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            //var g2 = pictureEdit2.CreateGraphics();
            //var g3 = pictureEdit3.CreateGraphics();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.Clear(pictureEdit1.BackColor);

            // Draw the old polygons.
            foreach (List<Point> polygon in Polygons)
            {
                // Draw the polygon.
                e.Graphics.FillPolygon(Brushes.Transparent, polygon.ToArray());
                e.Graphics.DrawPolygon(Pens.Blue, polygon.ToArray());
                 //draw Lane line
                if (polygon.Count==8)
                {
                    e.Graphics.DrawLine(Pens.Yellow, polygon[1], polygon[6]);
                    e.Graphics.DrawLine(Pens.Yellow, polygon[2], polygon[5]);
                }
                else if(polygon.Count == 6)
                {
                    e.Graphics.DrawLine(Pens.Yellow, polygon[1], polygon[4]);
                }
                else if (polygon.Count == 2)
                {
                    e.Graphics.DrawLine(Pens.Blue, polygon[0], polygon[1]);
                }
               
                // Draw the corners.
                foreach (Point corner in polygon)
                {
                    Rectangle rect = new Rectangle(
                        corner.X - object_radius, corner.Y - object_radius,
                        2 * object_radius + 1, 2 * object_radius + 1);
                    //e.Graphics.FillEllipse(Brushes.White, rect);
                    //e.Graphics.DrawEllipse(Pens.Black, rect);
                    e.Graphics.FillRectangle(Brushes.White, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);

                  
                }
            }

            

           

            // Draw the new polygon.
            if (NewPolygon != null)
            {
                // Draw the new polygon.
                if (NewPolygon.Count > 1)
                {
                    e.Graphics.DrawLines(Pens.Green, NewPolygon.ToArray());
                }

                // Draw the newest edge.
                if (NewPolygon.Count > 0)
                {
                    using (Pen dashed_pen = new Pen(Color.Green))
                    {
                        dashed_pen.DashPattern = new float[] { 3, 3 };
                        e.Graphics.DrawLine(dashed_pen,
                            NewPolygon[NewPolygon.Count - 1],
                            NewPoint);
                    }
                }
            }
        }

        // See if the mouse is over a corner point.
        private bool MouseIsOverCornerPoint(Point mouse_pt, out List<Point> hit_polygon, out int hit_pt)
        {
            // See if we're over a corner point.
            foreach (List<Point> polygon in Polygons)
            {
                // See if we're over one of the polygon's corner points.
                for (int i = 0; i < polygon.Count; i++)
                {
                    // See if we're over this point.
                    if (FindDistanceToPointSquared(polygon[i], mouse_pt) < over_dist_squared)
                    {
                        // We're over this point.
                        hit_polygon = polygon;
                        hit_pt = i;
                        return true;
                    }
                }
            }

            hit_polygon = null;
            hit_pt = -1;
            return false;
        }

        // See if the mouse is over a polygon's edge.
        private bool MouseIsOverEdge(Point mouse_pt, out List<Point> hit_polygon, out int hit_pt1, out int hit_pt2, out Point closest_point)
        {
            // Examine each polygon.
            // Examine them in reverse order to check the ones on top first.
            for (int pgon = Polygons.Count - 1; pgon >= 0; pgon--)
            {
                List<Point> polygon = Polygons[pgon];

                // See if we're over one of the polygon's segments.
                for (int p1 = 0; p1 < polygon.Count; p1++)
                {
                    // Get the index of the polygon's next point.
                    int p2 = (p1 + 1) % polygon.Count;

                    // See if we're over the segment between these points.
                    PointF closest;
                    if (FindDistanceToSegmentSquared(mouse_pt,
                        polygon[p1], polygon[p2], out closest) < over_dist_squared)
                    {
                        // We're over this segment.
                        hit_polygon = polygon;
                        hit_pt1 = p1;
                        hit_pt2 = p2;
                        closest_point = Point.Round(closest);
                        return true;
                    }
                }
            }

            hit_polygon = null;
            hit_pt1 = -1;
            hit_pt2 = -1;
            closest_point = new Point(0, 0);
            return false;
        }

        // See if the mouse is over a polygon's body.
        private bool MouseIsOverPolygon(Point mouse_pt, out List<Point> hit_polygon)
        {
            // Examine each polygon.
            // Examine them in reverse order to check the ones on top first.
            try
            {
                for (int i = Polygons.Count - 1; i >= 0; i--)
                {
                    // Make a GraphicsPath representing the polygon.
                    GraphicsPath path = new GraphicsPath();
                    path.AddPolygon(Polygons[i].ToArray());

                    // See if the point is inside the GraphicsPath.
                    if (path.IsVisible(mouse_pt))
                    {
                        hit_polygon = Polygons[i];
                        return true;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            

            hit_polygon = null;
            return false;
        }

        // Calculate the distance squared between two points.
        private int FindDistanceToPointSquared(Point pt1, Point pt2)
        {
            int dx = pt1.X - pt2.X;
            int dy = pt1.Y - pt2.Y;
            return dx * dx + dy * dy;
        }

        // Calculate the distance squared between
        // point pt and the segment p1 --> p2.
        private double FindDistanceToSegmentSquared(PointF pt, PointF p1, PointF p2, out PointF closest)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            // return Math.Sqrt(dx * dx + dy * dy);
            return dx * dx + dy * dy;
        }

        private void pictureEdit2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.Clear(pictureEdit1.BackColor);

            // Draw the old polygons.
            foreach (List<Point> polygon in Polygons)
            {
                // Draw the polygon.
                e.Graphics.FillPolygon(Brushes.Transparent, polygon.ToArray());
                e.Graphics.DrawPolygon(Pens.Blue, polygon.ToArray());


                // Draw the corners.
                foreach (Point corner in polygon)
                {
                    Rectangle rect = new Rectangle(
                        corner.X - object_radius, corner.Y - object_radius,
                        2 * object_radius + 1, 2 * object_radius + 1);
                    //e.Graphics.FillEllipse(Brushes.White, rect);
                    //e.Graphics.DrawEllipse(Pens.Black, rect);
                    e.Graphics.FillRectangle(Brushes.White, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);

                }
            }

        }

        private void pictureEdit3_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.Clear(pictureEdit1.BackColor);

            // Draw the old polygons.
            foreach (List<Point> polygon in Polygons)
            {
                // Draw the polygon.
                e.Graphics.FillPolygon(Brushes.Transparent, polygon.ToArray());
                e.Graphics.DrawPolygon(Pens.Blue, polygon.ToArray());


                // Draw the corners.
                foreach (Point corner in polygon)
                {
                    Rectangle rect = new Rectangle(
                        corner.X - object_radius, corner.Y - object_radius,
                        2 * object_radius + 1, 2 * object_radius + 1);
                    //e.Graphics.FillEllipse(Brushes.White, rect);
                    //e.Graphics.DrawEllipse(Pens.Black, rect);
                    e.Graphics.FillRectangle(Brushes.White, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);

                }
            }

        }

        public List<Point> ScreenPointToPicture(List<Point> points)
        {
            List<Point> mConvertPoint = new List<Point>();
            foreach(var item in points)
            {
                int _x = (int)(Math.Round(item.X * scaleX));
                int _y = (int)(Math.Round(item.Y * scaleY));
                mConvertPoint.Add(new Point(_x,_y));
            }
            return mConvertPoint;
        }

        public List<Point> PicturePointToScreen(List<Point> points)
        {
            List<Point> mConvertPoint = new List<Point>();
            foreach (var item in points)
            {
                int _x = (int)(Math.Round(item.X / scaleX));
                int _y = (int)(Math.Round(item.Y / scaleY));
                mConvertPoint.Add(new Point(_x, _y));
            }
            return mConvertPoint;
        }

        public Point PicturePointToScreen(Point points)
        {
            Point mConvertPoint = new Point();
            int _x = (int)(Math.Round(points.X / scaleX));
            int _y = (int)(Math.Round(points.Y / scaleY));
            mConvertPoint.X = _x;
            mConvertPoint.Y = _y;
            return mConvertPoint;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CrossingInfo crossing = new CrossingInfo();
            CrossConfig config = new CrossConfig();
            config.CropHeight = Convert.ToInt32(textEdit_CropHeight.Text.ToString());
            for(int i=0; i< Polygons.Count ;i++)
            {
                //LaneLine
                if(i == 0)
                {
                    List<Point> mLaneLine= Polygons[i];
                    Console.WriteLine(mLaneLine[0].X + "," + mLaneLine[0].Y);
                    List<Point> mPicLanePoints = ScreenPointToPicture(mLaneLine);
                    if(mPicLanePoints.Count == 8)
                    {
                        
                        config.LaneLine = new LaneLine();
                        config.LaneLine.LineNumber = 3;
                        config.LaneLine.LinePosition = new List<LinePosition>();

                        LinePosition line1 = new LinePosition();
                        line1.StartPoint = mPicLanePoints[0].X + "," + mPicLanePoints[0].Y;
                        line1.EndPoint = mPicLanePoints[7].X + "," + mPicLanePoints[7].Y;
                        config.LaneLine.LinePosition.Add(line1);

                        LinePosition line2 = new LinePosition();
                        line2.StartPoint = mPicLanePoints[1].X + "," + mPicLanePoints[1].Y;
                        line2.EndPoint = mPicLanePoints[6].X + "," + mPicLanePoints[6].Y;
                        config.LaneLine.LinePosition.Add(line2);

                        LinePosition line3 = new LinePosition();
                        line3.StartPoint = mPicLanePoints[2].X + "," + mPicLanePoints[2].Y;
                        line3.EndPoint = mPicLanePoints[5].X + "," + mPicLanePoints[5].Y;
                        config.LaneLine.LinePosition.Add(line3);

                        LinePosition line4 = new LinePosition();
                        line4.StartPoint = mPicLanePoints[3].X + "," + mPicLanePoints[3].Y;
                        line4.EndPoint = mPicLanePoints[4].X + "," + mPicLanePoints[4].Y;
                        config.LaneLine.LinePosition.Add(line4);
                    }
                }
                //StopLan
                if(i ==1 )
                {
                    List<Point> mStopLan = Polygons[i];
                    List<Point> mPicStopLanPoints = ScreenPointToPicture(mStopLan);
                    if(mPicStopLanPoints.Count ==2 )
                    {
                        config.StopLine = new List<string>();
                        string point1 = mPicStopLanPoints[0].X +"," + mPicStopLanPoints[0].Y;
                        string point2 = mPicStopLanPoints[1].X +"," + mPicStopLanPoints[1].Y;
                        config.StopLine.Add(point1);
                        config.StopLine.Add(point2);
                    }

                }
                //ZebraCrossing 
                if(i ==2 )
                {
                    List<Point> mZebraCrossing = Polygons[i];

                    List<Point> mPicmZebraCrossingPoints = ScreenPointToPicture(mZebraCrossing);
                    if(mPicmZebraCrossingPoints.Count == 4 )
                    {
                        config.ZebraCrossing = new ZebraCrossing();
                        config.ZebraCrossing.HaveLine = 1;
                        config.ZebraCrossing.TrafficPoints=  new List<string>();
                        for(int n = 0;n < mPicmZebraCrossingPoints.Count ;n++)
                        {
                            string point1 = mPicmZebraCrossingPoints[n].X + "," + mPicmZebraCrossingPoints[n].Y;
                            if (checkEditZebra.Checked ==true)
                            {
                                config.ZebraCrossing.HaveLine = 1;
                                config.ZebraCrossing.TrafficPoints.Add(point1);
                            }
                            else
                            {
                                config.ZebraCrossing.HaveLine = 0;
                                config.ZebraCrossing.TrafficPoints.Add("0,0");
                            }
                           
                        }
                    }
                }
                //TrafficLights
                if(i ==3 )
                {
                    List<Point> mTrafficLights = Polygons[i];
                    List<Point> mPicTrafficLightsPoints = ScreenPointToPicture(mTrafficLights);
                    if(mPicTrafficLightsPoints.Count == 4 )
                    {
                        config.TrafficLight = new List<string>();
                        for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                        {
                            string point1 = mPicTrafficLightsPoints[n].X + "," + mPicTrafficLightsPoints[n].Y;
                            config.TrafficLight.Add(point1);
                        }
                    }
                }

            }
            String json = JsonConvert.SerializeObject(config);
            Console.WriteLine(json);
            int nIndex = comboBoxEditCross.SelectedIndex;
            if(nIndex>=0)
            {
                var item =  mCrossingList[nIndex];
                string base64Image = "";
                if(mCurOperateImage != null )
                {
                     base64Image = Base64Util.GetBase64FromImage(mCurOperateImage);
                }

                int ret = dal.UpdateCrossingInfo(item.CROSSING_ID,json, base64Image);
                if(ret == 1 )
                {
                    //Reload Update Data
                    loadData();
                    XtraMessageBox.Show("保存成功!");
                }
                
            }

        }

        private void comboBoxEditCross_SelectedIndexChanged(object sender, EventArgs e)
        {
            Polygons.Clear();
            int nIndex = comboBoxEditCross.SelectedIndex;
            if (nIndex >= 0)
            {

                var crossInfo = mCrossingList[nIndex];


                if (!string.IsNullOrEmpty(crossInfo.IMAGE_DATA))
                {
                    Image decodeImage = Base64Util.GetImageFromBase64(crossInfo.IMAGE_DATA);
                    mCurOperateImage = decodeImage;
                    cutImage();
                }


                if (!string.IsNullOrEmpty(crossInfo.CROSSING_CONFIG))
                {
                    CrossConfig dbConfig = JsonConvert.DeserializeObject<CrossConfig>(crossInfo.CROSSING_CONFIG);
                    if (null != dbConfig)
                    {
                        //LaneLine
                        //0
                        LaneLines.Clear();
                        string [] positionArr = dbConfig.LaneLine.LinePosition[0].StartPoint.Split(',');
                        Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //1
                         positionArr = dbConfig.LaneLine.LinePosition[1].StartPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //2
                         positionArr= dbConfig.LaneLine.LinePosition[2].StartPoint.Split(',');
                         position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //3
                        positionArr = dbConfig.LaneLine.LinePosition[3].StartPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //4
                        positionArr = dbConfig.LaneLine.LinePosition[3].EndPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //5
                        positionArr = dbConfig.LaneLine.LinePosition[2].EndPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //6
                        positionArr = dbConfig.LaneLine.LinePosition[1].EndPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        //7
                        positionArr = dbConfig.LaneLine.LinePosition[0].EndPoint.Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        Polygons.Add(LaneLines);
                      


                        //StopLine
                        StopLine.Clear();
                        positionArr = dbConfig.StopLine[0].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        StopLine.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        positionArr = dbConfig.StopLine[1].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        StopLine.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        Polygons.Add(StopLine);

                        //ZebraCrossing
                        ZebraCross.Clear();
                        positionArr = dbConfig.ZebraCrossing.TrafficPoints[0].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        ZebraCross.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        positionArr = dbConfig.ZebraCrossing.TrafficPoints[1].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        ZebraCross.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        positionArr = dbConfig.ZebraCrossing.TrafficPoints[2].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        ZebraCross.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        positionArr = dbConfig.ZebraCrossing.TrafficPoints[3].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        ZebraCross.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                        Polygons.Add(ZebraCross);

                        if (dbConfig.ZebraCrossing.HaveLine == 1)
                        {
                            checkEditZebra.Checked = true;
                        }
                        else
                        {
                            checkEditZebra.Checked = false;
                        }

                        ////TrafficLights
                        TrafficLights.Clear();
                        positionArr = dbConfig.TrafficLight[0].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        TrafficLights.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        positionArr = dbConfig.TrafficLight[1].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        TrafficLights.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        positionArr = dbConfig.TrafficLight[2].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        TrafficLights.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                        positionArr = dbConfig.TrafficLight[3].Split(',');
                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                        TrafficLights.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                       
                        Polygons.Add(TrafficLights);

                    }
                }
                
            }
        }

        private void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

       

    }
}
