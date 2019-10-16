using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Lab1
{
    class Line
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }

        public Line()
        {
            Point1 = null;
            Point2 = null;
        }
        public Line(Point _point1, Point _point2)
        {
            Point1 = _point1;
            Point2 = _point2;
        }
        public override string ToString()
        {
            return Point1.Name + Point2.Name;
        }
    }
}
