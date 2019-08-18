using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRVision.UI.Vehicle
{
    public partial class FrmMarkingTraffic : XtraForm
    {
        public List<List<Point>> mClipPolygons { get; set; }
        double scaleX = 0.0f;
        double scaleY = 0.0f;
        public FrmMarkingTraffic()
        {
            InitializeComponent();
        }

        private void FrmMarkingTraffic_Load(object sender, EventArgs e)
        {

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
       
        public void SetClipImage(Image img )
        {
            pictureEdit1.Image = img;
            scaleX = (double)img.Width / (double)pictureEdit1.Width;
            scaleY = (double)img.Height / (double)pictureEdit1.Height;
            Polygons.Clear();
            foreach (var point in mClipPolygons)
            {
                //pic to screen 
                Console.WriteLine(point[0].X + " " + pictureEdit1.Image.Width);
                var points =   PicturePointToScreen(point);
                Polygons.Add(points);
            }
        }

        
        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            mClipPolygons.Clear();
            foreach(var poins in Polygons)
            {
                List<Point> mPicTrafficLightsPoints = ScreenPointToPicture(poins);
                mClipPolygons.Add(mPicTrafficLightsPoints);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void simpleBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
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

    }
}
