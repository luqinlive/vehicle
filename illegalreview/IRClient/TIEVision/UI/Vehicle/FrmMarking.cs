using DevExpress.XtraEditors;
using IRVision.DAL;
using IRVision.Model;
using IRVision.UI.Vehicle;
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
        List<Point> TrafficLights1 = new List<Point>();
        List<Point> TrafficLights2 = new List<Point>();
        List<Point> TrafficLights3 = new List<Point>();
        List<Point> TrafficLights4= new List<Point>();
        double scaleX = 0.0f;
        double scaleY = 0.0f;
        private Image mCurOperateImage = null;
        public FrmMarkingSet frmMarkingSet = new FrmMarkingSet();
        public int nCurrentChannel = -1;

        public FrmMarking()
        {
            InitializeComponent();
        }

        private void FrmMarking_Load(object sender, EventArgs e)
        {
            FrmCrossing.ShowCrossInfo += new FrmCrossing.ShowCrossEventHandler(ShowMsgInfo);
            AddPointCursor = new Cursor(IRVision.Properties.Resources.add_point.GetHicon());
            //frmMarkingSet.Hide();
            InitParams();
            loadData();
        }

        private void ShowMsgInfo(string msg )
        {
            loadData();
        }

        private void InitParams()
        {
            frmMarkingSet.nScreenMode = 4;
            frmMarkingSet.nLaneNumber = 3;
            frmMarkingSet.nHaveLaneLine = 1;
            frmMarkingSet.nHaveStopLine = 1;
            frmMarkingSet.nHaveTrafficLights = 1;
            frmMarkingSet.nHaveZebra = 1;
            frmMarkingSet.nCropHeight = 0;

            frmMarkingSet.nTrafficNumber1 = 1;
            frmMarkingSet.nTrafficNumber2 = 0;
            frmMarkingSet.nTrafficNumber3 = 0;
            frmMarkingSet.nTrafficNumber4 = 0;


            frmMarkingSet.nLaneNumber1 = 1;
            frmMarkingSet.nLaneNumber2 = 0;
            frmMarkingSet.nLaneNumber3 = 0;
            frmMarkingSet.nLaneNumber4 = 0;

            frmMarkingSet.SetControlStatus();
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
            // Convert.ToInt32(textEdit_CropHeight.Text.ToString());
            int cropHeight = frmMarkingSet.nCropHeight;
            Image selectedImage = mCurOperateImage;
            if (null != selectedImage)
            {
                // 4分屏
                if (frmMarkingSet.nScreenMode == 4)
                {
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
                else
                {
                    //2分屏
                    int cloneWidth = selectedImage.Width / 2;
                    int cloneHeight = (selectedImage.Height - cropHeight);

                    Rectangle cropRect = new Rectangle(0, cropHeight, cloneWidth, cloneHeight);
                    Image img1 = cropImage(selectedImage, cropRect);
                    cropRect = new Rectangle(cloneWidth, cropHeight, cloneWidth, cloneHeight);
                    Image img2 = cropImage(selectedImage, cropRect);
                    pictureEdit1.Image = img1;
                    pictureEdit2.Image = img2;
                    pictureEdit3.Image = null;
                    pictureEdit4.Image = null;
                    scaleX = (double)img1.Width / (double)pictureEdit1.Width;
                    scaleY = (double)img1.Height / (double)pictureEdit1.Height;
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
        private const int object_radius = 5;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int over_dist_squared = object_radius * object_radius;

        // Each polygon is represented by a List<Point>.
        private List<List<Point>> Polygons = new List<List<Point>>();
        //Each polygon type 
        private List<string> PolygonsType = new List<string>();
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

        //TrafficLights
        private int _x, _y;
        Image _img = null;
        bool _selecting = false;
        Rectangle _selection;
        Rectangle selectionPic;
        private Point startPos;
        private Point currentPos;
        private bool drawing;


        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            // See what we're over.
            Point mouse_pt = e.Location;
            List<Point> hit_polygon;
            int hit_point, hit_point2;
            Point closest_point;

            if (checkEdit_TrafficLightsResize.Checked == true)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _selecting = true;
                    _selection = new Rectangle(new Point(e.X, e.Y), new Size());

                    currentPos = startPos = e.Location;
                    drawing = true;
                }
            }

            if (NewPolygon != null)
            {
                // We are already drawing a polygon.
                // If it's the right mouse button, finish this polygon.
                if (e.Button == MouseButtons.Right)
                {
                    // Finish this polygon.
                    //if (NewPolygon.Count >= 2) Polygons.Add(NewPolygon);
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

        private Rectangle GetRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y)
                );
        }

        FrmMarkingTraffic frmMarkingTraffic = new FrmMarkingTraffic();
        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            if(checkEdit_TrafficLightsResize.Checked == true)
            {
                if (drawing)
                {
                    drawing = false;
                    var rect = GetRectangle();
                    Rectangle clipRect = ScreenToPic(rect);
                    Image img1 = cropImage(pictureEdit1.Image, clipRect);
                    List<List<Point>> clipPolygons = new List<List<Point>>();
                    for (int i = 0; i < Polygons.Count; i++)
                    {

                        if (PolygonsType[i] == "TrafficLights")
                        {
                            List<Point> mClipPoints = new List<Point>();
                            List<Point> mTrafficLights = Polygons[i];
                            List<Point> mPicTrafficLightsPoints = ScreenPointToPicture(mTrafficLights);
                            foreach(var point1 in mPicTrafficLightsPoints)
                            {
                               Point pointClip =new Point(point1.X - clipRect.X,point1.Y - clipRect.Y);
                               mClipPoints.Add(pointClip);
                            }
                            clipPolygons.Add(mClipPoints);
                        }
                    }
                    frmMarkingTraffic.mClipPolygons = clipPolygons;
                    frmMarkingTraffic.SetClipImage(img1);
                    frmMarkingTraffic.ShowDialog();
                    if (frmMarkingTraffic.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        var items = frmMarkingTraffic.mClipPolygons;
                        List<List<Point>> clipPolygons1 = new List<List<Point>>();
                        foreach(var points in frmMarkingTraffic.mClipPolygons)
                        {
                            List<Point> mPicTrafficLightsPoints1 = new List<Point>();
                            foreach(var point in points)
                            {
                                Point pointClip1 = new Point(point.X + clipRect.X, point.Y + clipRect.Y);
                                mPicTrafficLightsPoints1.Add(pointClip1);
                            }
                            clipPolygons1.Add(mPicTrafficLightsPoints1);
                        }
                        Console.WriteLine(clipPolygons1.Count);
                        for (int i = 0; i < 4; i++ )
                        {
                            Polygons.RemoveAt(Polygons.Count - 1);

                        }
                        for (int i = 0; i < 4; i++)
                        {
                            Polygons.Add(PicturePointToScreen(clipPolygons1[i]));

                        }
                         
                    }

                }
            }
           
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

        private void pictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            //TrafficLightResize
            if (_selecting)
            {
                _selection.Width = (e.X - _selection.X);
                _selection.Height = (e.Y - _selection.Y);

                // Redraw the picturebox:
                pictureEdit1.Refresh();
            }

            currentPos = e.Location;
            if (drawing)
            {
                pictureEdit1.Invalidate();
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
                if (polygon.Count == 10)
                {
                    e.Graphics.DrawLine(Pens.Yellow, polygon[1], polygon[8]);
                    e.Graphics.DrawLine(Pens.Yellow, polygon[2], polygon[7]);
                    e.Graphics.DrawLine(Pens.Yellow, polygon[3], polygon[6]);
                }
                else if (polygon.Count == 8)
                {
                    e.Graphics.DrawLine(Pens.Yellow, polygon[1], polygon[6]);
                    e.Graphics.DrawLine(Pens.Yellow, polygon[2], polygon[5]);
                }
                else if (polygon.Count == 6)
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
            //TrafficLightResize
            Pen pen = new Pen(Color.Red, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            if(checkEdit_TrafficLightsResize.Checked == true)
            {
                if (drawing  )
                {
                    e.Graphics.DrawRectangle(pen, GetRectangle());
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
            if (frmMarkingSet.nScreenMode == 4)
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


        }

        public List<Point> ScreenPointToPicture(List<Point> points)
        {
            List<Point> mConvertPoint = new List<Point>();
            foreach (var item in points)
            {
                int _x = (int)(Math.Round(item.X * scaleX));
                int _y = (int)(Math.Round(item.Y * scaleY));
                mConvertPoint.Add(new Point(_x, _y));
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

        private Rectangle PicToScreen(Rectangle rect)
        {
            int _width = (int)(Math.Round(rect.Width / scaleX));
            int _height = (int)(Math.Round(rect.Height / scaleY));
            int _xp = (int)(Math.Round(rect.X / scaleX));
            int _yp = (int)(Math.Round(rect.Y / scaleY));
            return new Rectangle(new Point(_xp, _yp), new Size(_width, _height));
        }
        private Rectangle ScreenToPic(Rectangle rect)
        {
            int _width = (int)(Math.Round(rect.Width * scaleX));
            int _height = (int)(Math.Round(rect.Height * scaleY));
            int _xs = (int)(Math.Round(rect.X * scaleX));
            int _ys = (int)(Math.Round(rect.Y * scaleY));
            return new Rectangle(new Point(_xs, _ys), new Size(_width, _height));
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
            int cropHeight = 0;
            cropHeight = frmMarkingSet.nCropHeight;
            config.CropHeight = cropHeight;// Convert.ToInt32(textEdit_CropHeight.Text.ToString());
            config.ScreenMode = frmMarkingSet.nScreenMode;
            config.LaneNumber = frmMarkingSet.nLaneNumber;
            for (int i = 0; i < Polygons.Count; i++)
            {
                //LaneLine

                if (PolygonsType[i] == "LaneLine")
                {
                    List<Point> mLaneLine = Polygons[i];
                    Console.WriteLine(mLaneLine[0].X + "," + mLaneLine[0].Y);
                    if (frmMarkingSet.nHaveLaneLine == 1)
                    {
                        List<Point> mPicLanePoints = ScreenPointToPicture(mLaneLine);
                        if (mPicLanePoints.Count == 8)
                        {
                            //Three Lane 
                            config.LaneLine = new LaneLine();
                            config.LaneLine.HaveLine = 1;
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
                        else if (mPicLanePoints.Count == 10)
                        {
                            //Four Lane
                            config.LaneLine = new LaneLine();
                            config.LaneLine.HaveLine = 1;
                            config.LaneLine.LineNumber = 4;
                            config.LaneLine.LinePosition = new List<LinePosition>();

                            LinePosition line1 = new LinePosition();
                            line1.StartPoint = mPicLanePoints[0].X + "," + mPicLanePoints[0].Y;
                            line1.EndPoint = mPicLanePoints[9].X + "," + mPicLanePoints[9].Y;
                            config.LaneLine.LinePosition.Add(line1);

                            LinePosition line2 = new LinePosition();
                            line2.StartPoint = mPicLanePoints[1].X + "," + mPicLanePoints[1].Y;
                            line2.EndPoint = mPicLanePoints[8].X + "," + mPicLanePoints[8].Y;
                            config.LaneLine.LinePosition.Add(line2);

                            LinePosition line3 = new LinePosition();
                            line3.StartPoint = mPicLanePoints[2].X + "," + mPicLanePoints[2].Y;
                            line3.EndPoint = mPicLanePoints[7].X + "," + mPicLanePoints[7].Y;
                            config.LaneLine.LinePosition.Add(line3);

                            LinePosition line4 = new LinePosition();
                            line4.StartPoint = mPicLanePoints[3].X + "," + mPicLanePoints[3].Y;
                            line4.EndPoint = mPicLanePoints[6].X + "," + mPicLanePoints[6].Y;
                            config.LaneLine.LinePosition.Add(line4);

                            LinePosition line5 = new LinePosition();
                            line5.StartPoint = mPicLanePoints[4].X + "," + mPicLanePoints[4].Y;
                            line5.EndPoint = mPicLanePoints[5].X + "," + mPicLanePoints[5].Y;
                            config.LaneLine.LinePosition.Add(line5);

                        }
                        else if (mPicLanePoints.Count == 6)
                        {
                            //Two Lane 
                            config.LaneLine = new LaneLine();
                            config.LaneLine.HaveLine = 1;
                            config.LaneLine.LineNumber = 2;
                            config.LaneLine.LinePosition = new List<LinePosition>();

                            LinePosition line1 = new LinePosition();
                            line1.StartPoint = mPicLanePoints[0].X + "," + mPicLanePoints[0].Y;
                            line1.EndPoint = mPicLanePoints[5].X + "," + mPicLanePoints[5].Y;
                            config.LaneLine.LinePosition.Add(line1);

                            LinePosition line2 = new LinePosition();
                            line2.StartPoint = mPicLanePoints[1].X + "," + mPicLanePoints[1].Y;
                            line2.EndPoint = mPicLanePoints[4].X + "," + mPicLanePoints[4].Y;
                            config.LaneLine.LinePosition.Add(line2);

                            LinePosition line3 = new LinePosition();
                            line3.StartPoint = mPicLanePoints[2].X + "," + mPicLanePoints[2].Y;
                            line3.EndPoint = mPicLanePoints[3].X + "," + mPicLanePoints[3].Y;
                            config.LaneLine.LinePosition.Add(line3);

                        }
                        else if (mPicLanePoints.Count == 4)
                        {
                            //Two Lane
                            config.LaneLine = new LaneLine();
                            config.LaneLine.HaveLine = 1;
                            config.LaneLine.LineNumber = 1;
                            config.LaneLine.LinePosition = new List<LinePosition>();

                            LinePosition line1 = new LinePosition();
                            line1.StartPoint = mPicLanePoints[0].X + "," + mPicLanePoints[0].Y;
                            line1.EndPoint = mPicLanePoints[3].X + "," + mPicLanePoints[3].Y;
                            config.LaneLine.LinePosition.Add(line1);

                            LinePosition line2 = new LinePosition();
                            line2.StartPoint = mPicLanePoints[1].X + "," + mPicLanePoints[1].Y;
                            line2.EndPoint = mPicLanePoints[2].X + "," + mPicLanePoints[2].Y;
                            config.LaneLine.LinePosition.Add(line2);

                        }
                        if(null == config.LaneLine.LaneType)
                        {
                            config.LaneLine.LaneType = new List<string>();
                        }
                        config.LaneLine.LaneType.Add(frmMarkingSet.nLaneNumber1+"," + frmMarkingSet.mLaneType1);
                        config.LaneLine.LaneType.Add(frmMarkingSet.nLaneNumber2 + "," + frmMarkingSet.mLaneType2);
                        config.LaneLine.LaneType.Add(frmMarkingSet.nLaneNumber3 + "," + frmMarkingSet.mLaneType3);
                        config.LaneLine.LaneType.Add(frmMarkingSet.nLaneNumber4 + "," + frmMarkingSet.mLaneType4);
                    }
                    else
                    {
                        List<Point> mPicLanePoints = ScreenPointToPicture(mLaneLine);
                        //if (mPicLanePoints.Count == 8)
                        {
                            config.LaneLine = new LaneLine();
                            config.LaneLine.HaveLine = 0;
                            config.LaneLine.LineNumber = frmMarkingSet.nLaneNumber;
                            config.LaneLine.LinePosition = new List<LinePosition>();

                            LinePosition line1 = new LinePosition();
                            line1.StartPoint = "-10,-10";
                            line1.EndPoint = "-10,-10";
                            for (int n = 0; n < mPicLanePoints.Count / 2; n++)
                            {
                                config.LaneLine.LinePosition.Add(line1);
                            }
                            if (null == config.LaneLine.LaneType)
                            {
                                config.LaneLine.LaneType = new List<string>();
                            }
                            config.LaneLine.LaneType.Add("0,null,null");
                            config.LaneLine.LaneType.Add("0,null,null");
                            config.LaneLine.LaneType.Add("0,null,null");
                            config.LaneLine.LaneType.Add("0,null,null");

                        }
                    }

                }
                //StopLan

                if (PolygonsType[i] == "StopLine")
                {
                    List<Point> mStopLan = Polygons[i];
                    List<Point> mPicStopLanPoints = ScreenPointToPicture(mStopLan);
                    if (mPicStopLanPoints.Count == 2)
                    {
                        config.StopLine = new StopLine();
                        if (frmMarkingSet.nHaveStopLine == 1)
                        {
                            config.StopLine.HaveLine = 1;
                            config.StopLine.Points = new List<string>();
                            string point1 = mPicStopLanPoints[0].X + "," + mPicStopLanPoints[0].Y;
                            string point2 = mPicStopLanPoints[1].X + "," + mPicStopLanPoints[1].Y;
                            config.StopLine.Points.Add(point1);
                            config.StopLine.Points.Add(point2);
                        }
                        else
                        {
                            config.StopLine.HaveLine = 0;
                            config.StopLine.Points = new List<string>();
                            string point1 = "-10,-10";
                            string point2 = "-10,-10";
                            config.StopLine.Points.Add(point1);
                            config.StopLine.Points.Add(point2);
                        }

                    }

                }
                //ZebraCross 

                if (PolygonsType[i] == "ZebraCross")
                {
                    List<Point> mZebraCrossing = Polygons[i];

                    List<Point> mPicmZebraCrossingPoints = ScreenPointToPicture(mZebraCrossing);
                    if (mPicmZebraCrossingPoints.Count == 4)
                    {
                        config.ZebraCrossing = new ZebraCrossing();
                        config.ZebraCrossing.HaveLine = 1;
                        config.ZebraCrossing.TrafficPoints = new List<string>();
                        for (int n = 0; n < mPicmZebraCrossingPoints.Count; n++)
                        {
                            string point1 = mPicmZebraCrossingPoints[n].X + "," + mPicmZebraCrossingPoints[n].Y;

                            if (frmMarkingSet.nHaveZebra == 1)
                            {
                                config.ZebraCrossing.HaveLine = 1;
                                config.ZebraCrossing.TrafficPoints.Add(point1);
                            }
                            else
                            {
                                config.ZebraCrossing.HaveLine = 0;
                                config.ZebraCrossing.TrafficPoints.Add("-10,-10");
                            }

                        }
                    }
                }
                //TrafficLights

                if (PolygonsType[i] == "TrafficLights")
                {
                    List<Point> mTrafficLights = Polygons[i];
                    List<Point> mPicTrafficLightsPoints = ScreenPointToPicture(mTrafficLights);
                    if (mPicTrafficLightsPoints.Count == 4)
                    {
                        if (config.TrafficLight == null)
                        {
                            config.TrafficLight = new TrafficLight();
                        }
                        if (frmMarkingSet.nHaveTrafficLights == 1)
                        {
                            config.TrafficLight.HaveLine = 1;
                            if (config.TrafficLight.TrafficLine == null )
                            {
                                config.TrafficLight.TrafficLine = new List<TrafficLine>();
                            }
                            if(frmMarkingSet.nTrafficNumber1 ==1 && i == 3)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 1;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = mPicTrafficLightsPoints[n].X + "," + mPicTrafficLightsPoints[n].Y;
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }
                            else if(frmMarkingSet.nTrafficNumber1 ==0 && i == 3)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 1;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 =  "-10,-10"  ;
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }

                            if(frmMarkingSet.nTrafficNumber2 == 1 && i == 4)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 2;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = mPicTrafficLightsPoints[n].X + "," + mPicTrafficLightsPoints[n].Y;
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }
                            else if(frmMarkingSet.nTrafficNumber2 == 0 && i == 4)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 2;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = "-10,-10";
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }

                            if (frmMarkingSet.nTrafficNumber3 == 1 && i == 5)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 3;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = mPicTrafficLightsPoints[n].X + "," + mPicTrafficLightsPoints[n].Y;
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }else if (frmMarkingSet.nTrafficNumber3 == 0 && i == 5)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 3;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = "-10,-10";
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }

                            if (frmMarkingSet.nTrafficNumber4 == 1 && i == 6)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 4;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = mPicTrafficLightsPoints[n].X + "," + mPicTrafficLightsPoints[n].Y;
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);

                            }
                            else  if (frmMarkingSet.nTrafficNumber4 == 0 && i ==6)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = 4;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = "-10,-10";
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }


                        }
                        else
                        {
                            config.TrafficLight.HaveLine = 0;
                            if (config.TrafficLight.TrafficLine == null )
                            {
                                config.TrafficLight.TrafficLine = new List<TrafficLine>();
                            }

                            for(int k =0; k<4; k++)
                            {
                                TrafficLine trafficLine = new TrafficLine();
                                trafficLine.TraffcLightNumber = k + 1;
                                trafficLine.TraffcLightType = "";
                                trafficLine.Points = new List<string>();
                                for (int n = 0; n < mPicTrafficLightsPoints.Count; n++)
                                {
                                    string point1 = "-10,-10";
                                    trafficLine.Points.Add(point1);

                                }
                                config.TrafficLight.TrafficLine.Add(trafficLine);
                            }
                           
                        }

                    }
                }

            }
            String json = JsonConvert.SerializeObject(config);
            Console.WriteLine(json);
            int nIndex = comboBoxEditCross.SelectedIndex;
            if (nIndex >= 0)
            {
                var item = mCrossingList[nIndex];
                string base64Image = "";
                if (mCurOperateImage != null)
                {
                    base64Image = Base64Util.GetBase64FromImage(mCurOperateImage);
                }

                int ret = dal.UpdateCrossingInfo(item.CROSSING_ID, json, base64Image);
                if (ret == 1)
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
            PolygonsType.Clear();
            int nIndex = comboBoxEditCross.SelectedIndex;
            if (nIndex >= 0)
            {
                var crossInfo = mCrossingList[nIndex];
                nCurrentChannel = nIndex;
                if (!string.IsNullOrEmpty(crossInfo.CROSSING_CONFIG))
                {
                    CrossConfig dbConfig = JsonConvert.DeserializeObject<CrossConfig>(crossInfo.CROSSING_CONFIG);
                    if (null != dbConfig)
                    {
                        //ScreenMode 
                        frmMarkingSet.nScreenMode = Convert.ToInt32(dbConfig.ScreenMode);
                        frmMarkingSet.nLaneNumber = Convert.ToInt32(dbConfig.LaneNumber);

                        if (!string.IsNullOrEmpty(crossInfo.IMAGE_DATA))
                        {
                            Image decodeImage = Base64Util.GetImageFromBase64(crossInfo.IMAGE_DATA);
                            mCurOperateImage = decodeImage;
                            cutImage();
                        }
                        else
                        {
                            //Clear image from picture control
                            pictureEdit1.Image = null;
                            pictureEdit2.Image = null;
                            pictureEdit3.Image = null;
                            pictureEdit4.Image = null;
                        }

                        //LaneLine
                        //0
                        LaneLines.Clear();
                        if (null != dbConfig.LaneLine)
                        {
                            if (dbConfig.LaneLine.HaveLine == 1)
                            {
                                frmMarkingSet.nHaveLaneLine = 1;
                            }
                            else
                            {
                                frmMarkingSet.nHaveLaneLine = 0;
                            }
                            if (dbConfig.LaneLine.LineNumber == 3)
                            {
                                //Three Lane
                                string[] positionArr = dbConfig.LaneLine.LinePosition[0].StartPoint.Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //1
                                positionArr = dbConfig.LaneLine.LinePosition[1].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //2
                                positionArr = dbConfig.LaneLine.LinePosition[2].StartPoint.Split(',');
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
                                PolygonsType.Add("LaneLine");

                            }
                            else if (dbConfig.LaneLine.LineNumber == 4)
                            {
                                //Four Lane 
                                //0
                                string[] positionArr = dbConfig.LaneLine.LinePosition[0].StartPoint.Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //1
                                positionArr = dbConfig.LaneLine.LinePosition[1].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //2
                                positionArr = dbConfig.LaneLine.LinePosition[2].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //3
                                positionArr = dbConfig.LaneLine.LinePosition[3].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //4
                                positionArr = dbConfig.LaneLine.LinePosition[4].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //5
                                positionArr = dbConfig.LaneLine.LinePosition[4].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //6
                                positionArr = dbConfig.LaneLine.LinePosition[3].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //7
                                positionArr = dbConfig.LaneLine.LinePosition[2].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //8
                                positionArr = dbConfig.LaneLine.LinePosition[1].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //9
                                positionArr = dbConfig.LaneLine.LinePosition[0].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                Polygons.Add(LaneLines);
                                PolygonsType.Add("LaneLine");

                            }
                            else if (dbConfig.LaneLine.LineNumber == 2)
                            {
                                //Two Lane
                                //0
                                string[] positionArr = dbConfig.LaneLine.LinePosition[0].StartPoint.Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //1
                                positionArr = dbConfig.LaneLine.LinePosition[1].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //2
                                positionArr = dbConfig.LaneLine.LinePosition[2].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //3
                                positionArr = dbConfig.LaneLine.LinePosition[2].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //4
                                positionArr = dbConfig.LaneLine.LinePosition[1].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //5
                                positionArr = dbConfig.LaneLine.LinePosition[0].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                Polygons.Add(LaneLines);
                                PolygonsType.Add("LaneLine");
                            }
                            else if (dbConfig.LaneLine.LineNumber == 1)
                            {
                                //One Lane

                                //0
                                string[] positionArr = dbConfig.LaneLine.LinePosition[0].StartPoint.Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //1
                                positionArr = dbConfig.LaneLine.LinePosition[1].StartPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //2
                                positionArr = dbConfig.LaneLine.LinePosition[1].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                //3
                                positionArr = dbConfig.LaneLine.LinePosition[0].EndPoint.Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                LaneLines.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                Polygons.Add(LaneLines);
                                PolygonsType.Add("LaneLine");
                            }

                            //LaneType
                            if(dbConfig.LaneLine.LaneType != null)
                            {
                                for(int n =0;n < dbConfig.LaneLine.LaneType.Count;n++)
                                {
                                    string[] laneTypeParams =   dbConfig.LaneLine.LaneType[n].Split(',');
                                    if(laneTypeParams.Length >0)
                                    {
                                        if(n == 0)
                                        {
                                            frmMarkingSet.nLaneNumber1 = Convert.ToInt32(laneTypeParams[0]);
                                            frmMarkingSet.mLaneType1 = laneTypeParams[1] + "," + laneTypeParams[2];
                                        }
                                        if (n == 1)
                                        {
                                            frmMarkingSet.nLaneNumber2 = Convert.ToInt32(laneTypeParams[0]);
                                            frmMarkingSet.mLaneType2 = laneTypeParams[1] + "," + laneTypeParams[2];
                                        }
                                        if (n == 2)
                                        {
                                            frmMarkingSet.nLaneNumber3 = Convert.ToInt32(laneTypeParams[0]);
                                            frmMarkingSet.mLaneType3 = laneTypeParams[1] + "," + laneTypeParams[2];
                                        }
                                        if (n == 3)
                                        {
                                            frmMarkingSet.nLaneNumber4 = Convert.ToInt32(laneTypeParams[0]);
                                            frmMarkingSet.mLaneType4 = laneTypeParams[1] + "," + laneTypeParams[2];
                                        }
                                    }
                                }
                            }

                        }



                        //StopLine
                        if (null != dbConfig.StopLine)
                        {
                            if (dbConfig.StopLine.HaveLine == 1)
                            {
                                frmMarkingSet.nHaveStopLine = 1;
                            }
                            else
                            {
                                frmMarkingSet.nHaveStopLine = 0;
                            }
                            if (null != dbConfig.StopLine)
                            {
                                StopLine.Clear();
                                string[] positionArr = dbConfig.StopLine.Points[0].Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                StopLine.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                positionArr = dbConfig.StopLine.Points[1].Split(',');
                                position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                StopLine.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                Polygons.Add(StopLine);
                                PolygonsType.Add("StopLine");
                            }

                        }



                        //ZebraCross
                        ZebraCross.Clear();
                        if (null != dbConfig.ZebraCrossing)
                        {
                            if (dbConfig.ZebraCrossing.HaveLine == 1)
                            {
                                frmMarkingSet.nHaveZebra = 1;
                            }
                            else
                            {
                                frmMarkingSet.nHaveZebra = 0;
                            }
                            if (null != dbConfig.ZebraCrossing)
                            {
                                string[] positionArr = dbConfig.ZebraCrossing.TrafficPoints[0].Split(',');
                                Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
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
                                PolygonsType.Add("ZebraCross");
                            }
                        }


                        ////TrafficLights
                        TrafficLights1.Clear();
                        TrafficLights2.Clear();
                        TrafficLights3.Clear();
                        TrafficLights4.Clear();
                        if (null != dbConfig.TrafficLight)
                        {
                            if (dbConfig.TrafficLight.HaveLine == 1)
                            {
                                frmMarkingSet.nHaveTrafficLights = 1;
                            }
                            else
                            {
                                frmMarkingSet.nHaveTrafficLights = 0;
                            }
                            if (null != dbConfig.TrafficLight)
                            {
                                foreach (var traffic in dbConfig.TrafficLight.TrafficLine)
                                {

                                    if (traffic.TraffcLightNumber == 1)
                                    {
                                        frmMarkingSet.nTrafficNumber1 = 1;
                                        if(traffic.Points[0].Contains("-"))
                                        {
                                            frmMarkingSet.nTrafficNumber1 = 0;
                                        }
                                        string[] positionArr = traffic.Points[0].Split(',');
                                        Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights1.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[1].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights1.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[2].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights1.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[3].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights1.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                        Polygons.Add(TrafficLights1);
                                        PolygonsType.Add("TrafficLights");
                                    }
                                    if (traffic.TraffcLightNumber == 2) 
                                    { 
                                        frmMarkingSet.nTrafficNumber2 = 1;
                                        if (traffic.Points[0].Contains("-"))
                                        {
                                            frmMarkingSet.nTrafficNumber2 = 0;
                                        }
                                        string[] positionArr = traffic.Points[0].Split(',');
                                        Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights2.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[1].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights2.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[2].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights2.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[3].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights2.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                        Polygons.Add(TrafficLights2);
                                        PolygonsType.Add("TrafficLights");
                                    }
                                    if (traffic.TraffcLightNumber == 3)
                                    {
                                        frmMarkingSet.nTrafficNumber3= 1;
                                        if (traffic.Points[0].Contains("-"))
                                        {
                                            frmMarkingSet.nTrafficNumber3 = 0;
                                        }
                                        string[] positionArr = traffic.Points[0].Split(',');
                                        Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights3.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[1].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights3.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[2].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights3.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[3].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights3.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                        Polygons.Add(TrafficLights3);
                                        PolygonsType.Add("TrafficLights");
                                    }
                                    if (traffic.TraffcLightNumber == 4)
                                    { 
                                        frmMarkingSet.nTrafficNumber4= 1;
                                        if (traffic.Points[0].Contains("-"))
                                        {
                                            frmMarkingSet.nTrafficNumber4 = 0;
                                        }
                                        string[] positionArr = traffic.Points[0].Split(',');
                                        Point position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights4.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[1].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights4.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[2].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights4.Add(PicturePointToScreen(new Point(position.X, position.Y)));
                                        positionArr = traffic.Points[3].Split(',');
                                        position = new Point(Convert.ToInt32(positionArr[0]), Convert.ToInt32(positionArr[1]));
                                        TrafficLights4.Add(PicturePointToScreen(new Point(position.X, position.Y)));

                                        Polygons.Add(TrafficLights4);
                                        PolygonsType.Add("TrafficLights");
                                    }                        
                                    
                                    
                                }
                            }

                        }

                        //Change control status
                        frmMarkingSet.mCrossInfo = crossInfo;
                        frmMarkingSet.mCrossConfig = dbConfig;
                        frmMarkingSet.SetControlStatus();

                    }


                }
                if (!string.IsNullOrEmpty(crossInfo.IMAGE_DATA))
                {
                    Image decodeImage = Base64Util.GetImageFromBase64(crossInfo.IMAGE_DATA);
                    mCurOperateImage = decodeImage;
                    cutImage();
                }
                else
                {
                    //Clear image from picture control
                    pictureEdit1.Image = null;
                    pictureEdit2.Image = null;
                    pictureEdit3.Image = null;
                    pictureEdit4.Image = null;
                }
            }
        }

        private void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }


        private void simpleBtnParams_Click(object sender, EventArgs e)
        {
            //modify settings 
            int width = pictureEdit1.Width;
            int height = pictureEdit1.Height;
            int padding = 10;
            int lane_wrap = ((width - 20) / 3);

            frmMarkingSet.ShowDialog();
            if (frmMarkingSet.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //resize image 
                if (frmMarkingSet.nCropHeight >= 0)
                {
                    cutImage();
                }
                int nHaveLaneLine = frmMarkingSet.nHaveLaneLine;
                for (int i = 0; i < Polygons.Count; i++)
                {
                    //LaneLine

                    if (PolygonsType[i] == "LaneLine")
                    {
                        List<Point> mLaneLine = Polygons[i];
                        Console.WriteLine(mLaneLine[0].X + "," + mLaneLine[0].Y);
                        if (frmMarkingSet.nHaveLaneLine == 1)
                        {
                            List<Point> mPicLanePoints = ScreenPointToPicture(mLaneLine);
                            var crossInfo = mCrossingList[nCurrentChannel];
                            if (!string.IsNullOrEmpty(crossInfo.CROSSING_CONFIG))
                            {
                                CrossConfig dbConfig = JsonConvert.DeserializeObject<CrossConfig>(crossInfo.CROSSING_CONFIG);
                                if (dbConfig.LaneNumber != frmMarkingSet.nLaneNumber) ;
                                {
                                    ResetLanePoints(frmMarkingSet.nLaneNumber, i, width, height, padding, lane_wrap);



                                }
                                int point1 = mPicLanePoints[0].X;
                                if (point1 < 0)
                                {
                                    if (mPicLanePoints.Count == 8)
                                    {

                                        //Three 
                                        LaneLines.Clear();
                                        LaneLines.Add(new Point(padding, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap * 2, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap * 2, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap * 1, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));
                                        Polygons[i] = LaneLines;
                                    }
                                    else if (mPicLanePoints.Count == 6)
                                    {

                                        int lane_wrap2 = ((width - 20) / 2);
                                        LaneLines.Clear();
                                        LaneLines.Add(new Point(padding, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap2 * 1, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap2 * 2, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap2 * 2, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap2 * 1, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap2 * 0, height - padding));
                                        Polygons[i] = LaneLines;
                                    }
                                    else if (mPicLanePoints.Count == 4)
                                    {

                                        LaneLines.Clear();
                                        LaneLines.Add(new Point(padding, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));
                                        Polygons[i] = LaneLines;

                                    }
                                    else if (mPicLanePoints.Count == 10)
                                    {
                                        int lane_wrap4 = ((width - 20) / 4);
                                        LaneLines.Clear();
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 0, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 1, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 2, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 3, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 4, (height / 4) * 3));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 4, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 3, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 2, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 1, height - padding));
                                        LaneLines.Add(new Point(padding + lane_wrap4 * 0, height - padding));
                                        Polygons[i] = LaneLines;
                                    }
                                }

                            }

                        }
                        else
                        {
                            List<Point> mPicLanePoints = ScreenPointToPicture(mLaneLine);
                            int nLanePointsCount = mPicLanePoints.Count;
                            //if (mPicLanePoints.Count == 8)
                            {

                                LaneLines.Clear();
                                for (int j = 0; j < nLanePointsCount; j++)
                                {
                                    LaneLines.Add(new Point(-10, -10));
                                }
                                Polygons[i] = LaneLines;
                            }
                        }

                    }

                    //StopLan
                    if (PolygonsType[i] == "StopLine")
                    {
                        List<Point> mStopLan = Polygons[i];
                        List<Point> mPicStopLanPoints = ScreenPointToPicture(mStopLan);
                        if (mPicStopLanPoints.Count == 2)
                        {
                            if (frmMarkingSet.nHaveStopLine == 1)
                            {
                                int point1 = mPicStopLanPoints[0].X;
                                if (point1 < 0)
                                {
                                    //StopLine
                                    StopLine.Clear();
                                    StopLine.Add(new Point(padding, ((height / 4) * 3) - padding));
                                    StopLine.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3) - padding));
                                    Polygons[i] = StopLine;

                                }
                            }
                            else
                            {
                                StopLine.Clear();
                                StopLine.Add(new Point(-10, -10));
                                StopLine.Add(new Point(-10, -10));
                                Polygons[i] = StopLine;
                            }
                        }
                    }
                    //ZebraCross 
                    if (PolygonsType[i] == "ZebraCross")
                    {
                        List<Point> mZebraCrossing = Polygons[i];

                        List<Point> mPicmZebraCrossingPoints = ScreenPointToPicture(mZebraCrossing);
                        if (mPicmZebraCrossingPoints.Count == 4)
                        {
                            if (frmMarkingSet.nHaveZebra == 1)
                            {
                                int point1 = mPicmZebraCrossingPoints[0].X;
                                if (point1 < 0)
                                {
                                    ZebraCross.Clear();
                                    ZebraCross.Add(new Point(padding, ((height / 4) * 2)));
                                    ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 2)));
                                    ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3 - 20)));
                                    ZebraCross.Add(new Point(padding, ((height / 4) * 3 - 20)));
                                    Polygons[i] = ZebraCross;

                                }
                            }
                            else
                            {
                                ZebraCross.Clear();
                                ZebraCross.Add(new Point(-10, -10));
                                ZebraCross.Add(new Point(-10, -10));
                                ZebraCross.Add(new Point(-10, -10));
                                ZebraCross.Add(new Point(-10, -10));
                                Polygons[i] = ZebraCross;
                            }


                        }
                    }
                    //TrafficLights
                    if (PolygonsType[i] == "TrafficLights")
                    {
                        List<Point> mTrafficLights = Polygons[i];
                        List<Point> mPicTrafficLightsPoints = ScreenPointToPicture(mTrafficLights);
                        if (mPicTrafficLightsPoints.Count == 4)
                        {
                            if (frmMarkingSet.nHaveTrafficLights == 1)
                            {
                                if(frmMarkingSet.nTrafficNumber1 ==1 && i == 3)
                                {
                                    int point1 = mPicTrafficLightsPoints[0].X;
                                    if (point1 < 0)
                                    {

                                        TrafficLights1.Clear();
                                        TrafficLights1.Add(new Point(width / 5 - padding, padding));
                                        TrafficLights1.Add(new Point(width / 5 - padding + padding * 2, padding));
                                        TrafficLights1.Add(new Point(width / 5 - padding + padding * 2, padding * 4));
                                        TrafficLights1.Add(new Point(width / 5 - padding, padding * 4));
                                        Polygons[i] = TrafficLights1;
                                    }

                                }
                                else if (frmMarkingSet.nTrafficNumber1 ==0 && i == 3)
                                {

                                    TrafficLights1.Clear();
                                    TrafficLights1.Add(new Point(-10, -10));
                                    TrafficLights1.Add(new Point(-10, -10));
                                    TrafficLights1.Add(new Point(-10, -10));
                                    TrafficLights1.Add(new Point(-10, -10));
                                    Polygons[i] = TrafficLights1;
                                }

                                if (frmMarkingSet.nTrafficNumber2 == 1 && i == 4)
                                {
                                    int point1 = mPicTrafficLightsPoints[0].X;
                                    if (point1 < 0)
                                    {
                                        TrafficLights2.Clear();
                                        TrafficLights2 = new List<Point>();
                                        TrafficLights2.Add(new Point(width / 4 - padding + padding * 0, padding));
                                        TrafficLights2.Add(new Point(width / 4 - padding + padding * 2, padding));
                                        TrafficLights2.Add(new Point(width / 4 - padding + padding * 2, padding * 4));
                                        TrafficLights2.Add(new Point(width / 4 - padding + padding * 0, padding * 4));
                                        Polygons[i] = TrafficLights2;
                                    }


                                }
                                else if (frmMarkingSet.nTrafficNumber2 == 0 && i == 4)
                                {
                                    TrafficLights2.Clear();
                                    TrafficLights2.Add(new Point(-10, -10));
                                    TrafficLights2.Add(new Point(-10, -10));
                                    TrafficLights2.Add(new Point(-10, -10));
                                    TrafficLights2.Add(new Point(-10, -10));
                                    Polygons[i] = TrafficLights2;
                                }

                                if (frmMarkingSet.nTrafficNumber3 == 1 && i == 5)
                                {
                                    int point1 = mPicTrafficLightsPoints[0].X;
                                    if (point1 < 0)
                                    {
                                        TrafficLights3.Clear();
                                        TrafficLights3 = new List<Point>();
                                        TrafficLights3.Add(new Point(width / 3 - padding + padding * 0, padding));
                                        TrafficLights3.Add(new Point(width / 3 - padding + padding * 2, padding));
                                        TrafficLights3.Add(new Point(width / 3 - padding + padding * 2, padding * 4));
                                        TrafficLights3.Add(new Point(width / 3 - padding + padding * 0, padding * 4));
                                        Polygons[i] = TrafficLights3;

                                    }
                                }
                                else if (frmMarkingSet.nTrafficNumber3 == 0 && i == 5)
                                {
                                    TrafficLights3.Clear();
                                    TrafficLights3.Add(new Point(-10, -10));
                                    TrafficLights3.Add(new Point(-10, -10));
                                    TrafficLights3.Add(new Point(-10, -10));
                                    TrafficLights3.Add(new Point(-10, -10));
                                    Polygons[i] = TrafficLights3;
                                }

                                if (frmMarkingSet.nTrafficNumber4 == 1 && i ==6)
                                {
                                    int point1 = mPicTrafficLightsPoints[0].X;
                                    if (point1 < 0)
                                    {
                                        TrafficLights4.Clear();
                                        TrafficLights4 = new List<Point>();
                                        TrafficLights4.Add(new Point(width / 2 - padding + padding * 0, padding));
                                        TrafficLights4.Add(new Point(width / 2 - padding + padding * 2, padding));
                                        TrafficLights4.Add(new Point(width / 2 - padding + padding * 2, padding * 4));
                                        TrafficLights4.Add(new Point(width / 2 - padding + padding * 0, padding * 4));
                                        Polygons[i] = TrafficLights4;
                                    }
                                }else  if (frmMarkingSet.nTrafficNumber4 == 0 && i ==6)
                                {
                                    TrafficLights4.Clear();
                                    TrafficLights4.Add(new Point(-10, -10));
                                    TrafficLights4.Add(new Point(-10, -10));
                                    TrafficLights4.Add(new Point(-10, -10));
                                    TrafficLights4.Add(new Point(-10, -10));
                                    Polygons[i] = TrafficLights4;
                                }
                                

                            }
                            else
                            {


                            }

                        }
                    }

                }



                Console.WriteLine(nHaveLaneLine);
                //loadData();
            }
        }


        private void ResetLanePoints(int nLaneNumber, int i, int width, int height, int padding, int lane_wrap)
        {
            if (nLaneNumber == 3)
            {

                //Three 
                LaneLines.Clear();
                LaneLines.Add(new Point(padding, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap * 2, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap * 2, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap * 1, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));
                Polygons[i] = LaneLines;
            }
            else if (nLaneNumber == 2)
            {

                int lane_wrap2 = ((width - 20) / 2);
                LaneLines.Clear();
                LaneLines.Add(new Point(padding, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap2 * 1, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap2 * 2, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap2 * 2, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap2 * 1, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap2 * 0, height - padding));
                Polygons[i] = LaneLines;

            }
            else if (nLaneNumber == 1)
            {

                LaneLines.Clear();
                LaneLines.Add(new Point(padding, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));
                Polygons[i] = LaneLines;

            }
            else if (nLaneNumber == 4)
            {
                int lane_wrap4 = ((width - 20) / 4);
                LaneLines.Clear();
                LaneLines.Add(new Point(padding + lane_wrap4 * 0, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap4 * 1, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap4 * 2, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap4 * 3, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap4 * 4, (height / 4) * 3));
                LaneLines.Add(new Point(padding + lane_wrap4 * 4, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap4 * 3, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap4 * 2, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap4 * 1, height - padding));
                LaneLines.Add(new Point(padding + lane_wrap4 * 0, height - padding));
                Polygons[i] = LaneLines;
            }

        }

        private void simpleButtonChoosePic_Click(object sender, EventArgs e)
        {
            int nSelectedIdx = comboBoxEditCross.SelectedIndex;
            Console.WriteLine(nSelectedIdx);
            if (nSelectedIdx < 0)
            {
                XtraMessageBox.Show("请先选择配置通道!");
                return;
            }
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
                    PolygonsType.Clear();
                    int width = pictureEdit1.Width;
                    int height = pictureEdit1.Height;
                    int padding = 10;
                    int lane_wrap = ((width - 20) / 3);
                    //LaneLine
                    if (frmMarkingSet.nHaveLaneLine == 1)
                    {
                        //One Lane 
                        if (frmMarkingSet.nLaneNumber == 1)
                        {
                            LaneLines.Clear();
                            LaneLines.Add(new Point(padding, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));

                            Polygons.Add(LaneLines);
                            PolygonsType.Add("LaneLine");
                        }
                        //Two Lane
                        if (frmMarkingSet.nLaneNumber == 2)
                        {
                            int lane_wrap2 = ((width - 20) / 2);
                            LaneLines.Clear();
                            LaneLines.Add(new Point(padding, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap2 * 1, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap2 * 2, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap2 * 2, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap2 * 1, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap2 * 0, height - padding));

                            Polygons.Add(LaneLines);
                            PolygonsType.Add("LaneLine");

                        }
                        //Three Lane
                        if (frmMarkingSet.nLaneNumber == 3)
                        {
                            LaneLines.Clear();
                            LaneLines.Add(new Point(padding, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap * 2, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap * 3, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap * 3, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap * 2, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap * 1, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap * 0, height - padding));

                            Polygons.Add(LaneLines);
                            PolygonsType.Add("LaneLine");
                        }


                        //Four Lane
                        if (frmMarkingSet.nLaneNumber == 4)
                        {
                            int lane_wrap4 = ((width - 20) / 4);
                            LaneLines.Clear();
                            LaneLines.Add(new Point(padding + lane_wrap4 * 0, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 1, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 2, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 3, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 4, (height / 4) * 3));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 4, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 3, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 2, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 1, height - padding));
                            LaneLines.Add(new Point(padding + lane_wrap4 * 0, height - padding));

                            Polygons.Add(LaneLines);
                            PolygonsType.Add("LaneLine");
                        }
                    }
                    else
                    {
                        LaneLines.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            LaneLines.Add(new Point(-10, -10));
                        }

                        Polygons.Add(LaneLines);
                        PolygonsType.Add("LaneLine");
                    }


                    //StopLine
                    if (frmMarkingSet.nHaveStopLine == 1)
                    {
                        StopLine.Clear();
                        StopLine.Add(new Point(padding, ((height / 4) * 3) - padding));
                        StopLine.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3) - padding));

                        Polygons.Add(StopLine);
                        PolygonsType.Add("StopLine");

                    }
                    else
                    {
                        StopLine.Clear();
                        StopLine.Add(new Point(-10, -10));
                        StopLine.Add(new Point(-10, -10));
                        Polygons.Add(StopLine);
                        PolygonsType.Add("StopLine");
                    }


                    //ZebraCrossing

                    if (frmMarkingSet.nHaveZebra == 1)
                    {

                        ZebraCross.Clear();
                        ZebraCross.Add(new Point(padding, ((height / 4) * 2)));
                        ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 2)));
                        ZebraCross.Add(new Point(padding + lane_wrap * 3, ((height / 4) * 3 - 20)));
                        ZebraCross.Add(new Point(padding, ((height / 4) * 3 - 20)));
                        Polygons.Add(ZebraCross);
                        PolygonsType.Add("ZebraCross");
                    }
                    else
                    {
                        ZebraCross.Clear();
                        ZebraCross.Add(new Point(-10, -10));
                        ZebraCross.Add(new Point(-10, -10));
                        ZebraCross.Add(new Point(-10, -10));
                        ZebraCross.Add(new Point(-10, -10));
                        Polygons.Add(ZebraCross);
                        PolygonsType.Add("ZebraCross");
                    }

                    //TrafficLights
                    if (frmMarkingSet.nHaveTrafficLights == 1)
                    {
                        if(frmMarkingSet.nTrafficNumber1 ==1 )
                        {
                            TrafficLights1.Clear();
                            TrafficLights1 = new List<Point>();
                            TrafficLights1.Add(new Point(width / 5 - padding, padding));
                            TrafficLights1.Add(new Point(width / 5 - padding + padding * 2, padding));
                            TrafficLights1.Add(new Point(width / 5 - padding + padding * 2, padding * 4));
                            TrafficLights1.Add(new Point(width / 5 - padding, padding * 4));
                            Polygons.Add(TrafficLights1);
                            PolygonsType.Add("TrafficLights");
                        }
                        else
                        {
                            TrafficLights1.Clear();
                            TrafficLights1.Add(new Point(-10, -10));
                            TrafficLights1.Add(new Point(-10, -10));
                            TrafficLights1.Add(new Point(-10, -10));
                            TrafficLights1.Add(new Point(-10, -10));
                            Polygons.Add(TrafficLights1);
                            PolygonsType.Add("TrafficLights");
                        }

                        if (frmMarkingSet.nTrafficNumber2 == 1)
                        {
                            TrafficLights2.Clear();
                            TrafficLights2 = new List<Point>();
                            TrafficLights2.Add(new Point(width / 4 - padding + padding * 0, padding));
                            TrafficLights2.Add(new Point(width / 4 - padding + padding * 2, padding));
                            TrafficLights2.Add(new Point(width / 4 - padding + padding * 2, padding * 4));
                            TrafficLights2.Add(new Point(width / 4 - padding + padding * 0, padding * 4));
                            Polygons.Add(TrafficLights2);
                            PolygonsType.Add("TrafficLights");
                        }
                        else
                        {
                            TrafficLights2.Clear();
                            TrafficLights2.Add(new Point(-10, -10));
                            TrafficLights2.Add(new Point(-10, -10));
                            TrafficLights2.Add(new Point(-10, -10));
                            TrafficLights2.Add(new Point(-10, -10));
                            Polygons.Add(TrafficLights2);
                            PolygonsType.Add("TrafficLights");
                        }

                        if (frmMarkingSet.nTrafficNumber3 == 1)
                        {
                            TrafficLights3.Clear();
                            TrafficLights3 = new List<Point>();
                            TrafficLights3.Add(new Point(width / 3 - padding + padding * 0, padding));
                            TrafficLights3.Add(new Point(width / 3 - padding + padding * 2, padding));
                            TrafficLights3.Add(new Point(width / 3 - padding + padding * 2, padding * 4));
                            TrafficLights3.Add(new Point(width / 3 - padding + padding * 0, padding * 4));
                            Polygons.Add(TrafficLights3);
                            PolygonsType.Add("TrafficLights");
                        }
                        else
                        {
                            TrafficLights3.Clear();
                            TrafficLights3.Add(new Point(-10, -10));
                            TrafficLights3.Add(new Point(-10, -10));
                            TrafficLights3.Add(new Point(-10, -10));
                            TrafficLights3.Add(new Point(-10, -10));
                            Polygons.Add(TrafficLights3);
                            PolygonsType.Add("TrafficLights");
                        
                        }

                        if (frmMarkingSet.nTrafficNumber4 == 1)
                        {
                            TrafficLights4.Clear();
                            TrafficLights4 = new List<Point>();
                            TrafficLights4.Add(new Point(width / 2 - padding + padding * 0, padding));
                            TrafficLights4.Add(new Point(width / 2 - padding + padding * 2, padding));
                            TrafficLights4.Add(new Point(width / 2 - padding + padding * 2, padding * 4));
                            TrafficLights4.Add(new Point(width / 2 - padding + padding * 0, padding * 4));
                            Polygons.Add(TrafficLights4);
                            PolygonsType.Add("TrafficLights");
                        }
                        else
                        {
                            TrafficLights4.Clear();
                            TrafficLights4.Add(new Point(-10, -10));
                            TrafficLights4.Add(new Point(-10, -10));
                            TrafficLights4.Add(new Point(-10, -10));
                            TrafficLights4.Add(new Point(-10, -10));
                            Polygons.Add(TrafficLights4);
                            PolygonsType.Add("TrafficLights");

                        }
                      
                    }
                    else
                    {
                        
                    }

                }
            }
        }

        

    }
}
