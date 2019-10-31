using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class MatrixSystem
    {
        internal static string EquationLine(Point _p1, Point _p2)
        {
            int Vx = _p1.posY - _p2.posY;
            int Vy = _p2.posX - _p1.posX;

            if ((Vx == 0) && (Vy == 0))
                return null; // A и B одновременно не равны нулю.

            int Vz = ((_p1.posX * _p2.posY) - (_p2.posX * _p1.posY));
            string str = Vx + "x + " + Vy + "y + " + Vz + " = 0";
            return str;
        }
        internal static string EquationLine(Line _line)
        {
            int Vx = _line.Point1.posY - _line.Point2.posY;
            int Vy = _line.Point2.posX - _line.Point1.posX;

            if ((Vx == 0) && (Vy == 0))
                return null; // A и B одновременно не равны нулю.

            int Vz = ((_line.Point1.posX * _line.Point2.posY) - (_line.Point2.posX * _line.Point1.posY));
            string str = Vx + "x + " + Vy + "y + " + Vz + " = 0";
            return str;
        }
    }
}
