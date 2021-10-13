using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryPackingUI
{
    class Drawer
    {
        private PictureBox pictureBox;
        private const int CIRCLE_DIAMETER = 10;

        public Drawer(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
        }
        public void drawRectangles(ref List<Rectangle> rectList)
        {
            Pen blackPen = new Pen(Color.Black, 3);
            SolidBrush redBrush = new SolidBrush(Color.Red);
            Graphics graphics = pictureBox.CreateGraphics();
            foreach (Rectangle rect in rectList)
            {
                graphics.DrawRectangle(blackPen, rect.posX, rect.posY, rect.width, rect.height);
                graphics.FillEllipse(redBrush, rect.center.X - CIRCLE_DIAMETER / 2, rect.center.Y - CIRCLE_DIAMETER / 2, CIRCLE_DIAMETER, CIRCLE_DIAMETER);
            }
            drawOverlay(rectList);
        }

        public void drawOverlay(List<Rectangle> rectList)
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 255));
            Graphics graphics = pictureBox.CreateGraphics();
            List<List<float>> toMove = new List<List<float>>();
            for (int first = 0; first < rectList.Count(); first++)
            {
                for (int second = first + 1; second < rectList.Count(); second++)
                {
                    
                    RectangleF tmp = new RectangleF(0, 0, 0, 0);
                    float x1First = rectList[first].posX;
                    float y1First = rectList[first].posY;
                    float x2First = rectList[first].posX + rectList[first].width;
                    float y2First = rectList[first].posY + rectList[first].height;

                    float x1Second = rectList[second].posX;
                    float y1Second = rectList[second].posY;
                    float x2Second = rectList[second].posX + rectList[second].width;
                    float y2Second = rectList[second].posY + rectList[second].height;


                    if (x1First > x2First) swap(ref x1First, ref x2First);
                    if (y1First > y2First) swap(ref y1First, ref y2First);
                    if (x1Second > x2Second) swap(ref x1Second, ref x2Second);
                    if (y1Second > y2Second) swap(ref y1Second, ref y2Second);

                    tmp.X = Math.Max(x1First, x1Second);
                    tmp.Y = Math.Max(y1First, y1Second);
                    tmp.Width = Math.Min(x2First, x2Second) - tmp.X;
                    tmp.Height = Math.Min(y2First, y2Second) - tmp.Y;
                    if (tmp.Width > 0 && tmp.Height > 0)
                    {
                        graphics.FillRectangle(blueBrush, tmp);

                        if (tmp.X + tmp.Width == rectList[first].posX + rectList[first].width &&
                            tmp.Y + tmp.Height == rectList[first].posY + rectList[first].height)
                        {
                            //rectList[first].moveRectangle(-tmp.Width / 4, -tmp.Height / 4);
                            //rectList[second].moveRectangle(tmp.Width / 4, tmp.Height / 4);
                            toMove.Add(new List<float>() { first, -tmp.Width / 4, -tmp.Height / 4 });
                            toMove.Add(new List<float>() { second, tmp.Width / 4, tmp.Height / 4 });
                        }
                        if (tmp.X == rectList[first].posX &&
                            tmp.Y + tmp.Height == rectList[first].posY + rectList[first].height)
                        {
                            //rectList[first].moveRectangle(tmp.Width / 4, -tmp.Height / 4);
                            //rectList[second].moveRectangle(-tmp.Width / 4, tmp.Height / 4);
                            toMove.Add(new List<float>() { first, tmp.Width / 4, -tmp.Height / 4 });
                            toMove.Add(new List<float>() { second, -tmp.Width / 4, tmp.Height / 4 });
                        }
                        if (tmp.X + tmp.Width == rectList[first].posX + rectList[first].width &&
                            tmp.Y == rectList[first].posY)
                        {
                            //rectList[first].moveRectangle(-tmp.Width / 4, tmp.Height / 4);
                            //rectList[second].moveRectangle(tmp.Width / 4, -tmp.Height / 4);
                            toMove.Add(new List<float>() { first, -tmp.Width / 4, tmp.Height / 4 });
                            toMove.Add(new List<float>() { second, tmp.Width / 4, -tmp.Height / 4 });
                        }
                        if (tmp.X == rectList[first].posX &&
                            tmp.Y == rectList[first].posY)
                        {
                            //rectList[first].moveRectangle(tmp.Width / 4, tmp.Height / 4);
                            //rectList[second].moveRectangle(-tmp.Width / 4, -tmp.Height / 4);
                            toMove.Add(new List<float>() { first, tmp.Width / 4, tmp.Height / 4 });
                            toMove.Add(new List<float>() { second, -tmp.Width / 4, -tmp.Height / 4 });
                        }
                    }
                }
            }
            foreach (List<float> curMove in toMove)
            {
                rectList[(int)curMove[0]].moveRectangle(curMove[1], curMove[2]);
            }
        }

        private void swap(ref float first, ref float second)
        {
            float tmp = first;
            first = second;
            second = tmp;
        }
    }
}