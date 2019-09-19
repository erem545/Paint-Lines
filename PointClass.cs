using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab1
{
    public class PointClass
    {
        internal int posX { get; set; }
        internal int posY { get; set; }
        internal Point LeftPoint;
        internal Point RightPoint;

        public PointClass()
        {
            posX = -1;
            posY = -1;
            LeftPoint = null;
            RightPoint = null;
        }
        public PointClass(int x, int y)
        {
            posX = x;
            posY = y;
            LeftPoint = null;
            RightPoint = null;
        }
    }
    public class Point : PointClass
    {
        public Point() : base()
        {
            posX = -1;
            posY = -1;
            LeftPoint = null;
            RightPoint = null;
        }
        public Point(int x, int y) : base(x, y)
        {
            posX = x;
            posY = y;
            LeftPoint = null;
            RightPoint = null;
        }
        public void Add(Point point)
        {
            if (LeftPoint == null)
                LeftPoint = point;
            else if (RightPoint == null)
                RightPoint = point;
            else
                return;
        }
    }
}
