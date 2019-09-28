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
        public string NameLine { get; set; }
        public string Equation { get; set; }

        public Line()
        {
            Point1 = null;
            Point2 = null;
        }
        public Line(string name1 ,Point _point1, string name2, Point _point2)
        {
            Point1 = _point1;
            Point2 = _point2;
            NameLine = name1 + name2;
            int _i = _point1.posY - _point2.posY;
            int _j = _point2.posX - _point1.posX;
            int _k = (_point1.posX * _point2.posY) - (_point2.posX * _point1.posY);
        }
    }
}
