using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class EditingCapability
    {
        /// <summary>
        /// Коэффициенты на диагонали матрицы задают РАСТЯГИВАНИЕ/СЖАТИЕ плоскости.
        /// </summary>
        /// <param name="mlt">Коэффициент</param>
        public static void StretchShrink(ref Line _line, float k)
        {
            float[,] aMatrix = new float[,] { { k, 0 }, { 0, k } };
            float[,] lineMatrix = new float[,] { { _line.Matrix[0, 0], _line.Matrix[0, 1] }, { _line.Matrix[1, 0], _line.Matrix[1, 1] } };
            float[,] readyMatrix = MatrixClass.Multiplication(lineMatrix, aMatrix);


            int[,] arrInt = new int[2, 2];

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    arrInt[i, j] = (int)Math.Round(readyMatrix[i, j]);

            _line.RefreshLineOfMatrix(arrInt);
            


        }
    }
}
