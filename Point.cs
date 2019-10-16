using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lab1
{
    public class Point
    {
        public int posX { get; set; }
        public int posY { get; set; }
        public string Name { get; set; }

        public Point()
        {
            posX = -1;
            posY = -1;
        }
        public Point(string name, int x, int y)
        {
            Name = name;
            posX = x;
            posY = y;
        }
    }
}
