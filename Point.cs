using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lab1
{
    public class Point
    {
        public int posX { get; set; }
        public int posY { get; set; }

        public Point()
        {
            posX = -1;
            posY = -1;
        }
        public Point(int x, int y)
        {
            posX = x;
            posY = y;
        }
    }
}
