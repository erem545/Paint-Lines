using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lab1
{
    class Line
    {
        internal class Equation
        {
            protected int i_val;
            protected int j_val;
            protected int k_val;
            public Equation(int _i, int _j, int _k)
            {
                i_val = _i;
                j_val = _j;
                k_val = _k;
            }
            public override string ToString()
            {
                return (i_val + "i + (" + j_val + "j) + (" + k_val + "k) = 0");
            }
        }
        public Equation Equation_val
        {
            get
            {
                return _equation = new Equation(
                    Matrix[1, 1] - Matrix[0, 1],
                    Matrix[1, 0] - Matrix[0, 0],
                    Matrix[0, 0] * Matrix[1, 1] - Matrix[1, 0] * Matrix[0, 1]
                    );
            }
        }
        Equation _equation;

        // Хранение наименований точек
        string p2name;
        string p1name;

        public Point Point1 
        { 
            get { return point1; }
            set { point1 = value; } 
        }
        Point point1;
        public Point Point2
        {
            get { return point2; }
            set { point2 = value; }
        }
        Point point2;

        /// <summary>
        /// Возвращает матрицу прямой
        /// </summary>
        public int[,]  Matrix 
        { 
            get 
            {
                return matrix = new int[,] { { Point1.posX, Point1.posY }, { Point2.posX, Point2.posY } };
            } 
            set
            {
                matrix = value;
            }
        }
        int[,] matrix;


        public Line()
        {
            Point1 = null;
            Point2 = null;
        }
        public Line(Point _point1, Point _point2)
        {
            Point1 = _point1;
            Point2 = _point2;
            p1name = _point1.Name;
            p2name = _point2.Name;
        }




        public void RefreshLineOfMatrix(int[,]_matrix)
        {
            Point p1 = new Point(Point1.Name, _matrix[0, 0], _matrix[0, 1]);
            Point p2 = new Point(Point2.Name, _matrix[1, 0], _matrix[1, 1]);
            Point1 = p1; 
            Point2 = p2;
        }

        public override string ToString()
        {
            return Point1.Name + Point2.Name;
        }
    }
}
