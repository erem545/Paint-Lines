using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        bool ok = true; // Разрешать рисовать ось координат
        /// <summary>
        /// Отступ от начала экрана
        /// </summary>
        const int indent = 30;
        /// <summary>
        /// Размер деления
        /// </summary>
        const int sizeDiv = 20; 

        List<List<object>> LinesList; // Список прямых (Список в списке)
        List<object> tempList; // Дополнительный список
        Bitmap snapshot, tempDraw; // Снимки
        Color foreColor; // Цвет основной линии
        Brush FirstChar; // Цвет первого символа
        Brush SecondChar; // Цвет второго символа             
        int lineWight; // Ширина линии

        /// <summary>
        /// Вставить объект Line в список tempList
        /// </summary>
        /// <param name="_line">Объект класса Line</param>
        private void InsertInList(Line _line)
        {
            if (tempList == null)
                tempList = new List<object>();
            tempList.Add(_line.NameLine);
            tempList.Add(_line.Point1.posX);
            tempList.Add(_line.Point1.posY);
            tempList.Add(_line.Point2.posX);
            tempList.Add(_line.Point2.posY);
            InsertInList(tempList);
            tempList = null;
        }
        /// <summary>
        /// Вставить список tempList в список LinesList
        /// </summary>
        /// <param name="_list">Список со списками List</param>
        private void InsertInList(List<object> _list)
        {
            if (LinesList == null)
                LinesList = new List<List<object>>();
            LinesList.Add(_list);
            FillTableWithList(LinesList);
        }
        /// <summary>
        /// Заполнение datagridview данными из списка
        /// </summary>
        /// <param name="mainList">Список</param>
        private void FillTableWithList(List<List<object>> mainList)
        {
            dataGridView1.Rows.Clear();            
            for (int i = 0; i < mainList.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1[0, i].Value = mainList.ElementAt(i).ElementAt(0);
                dataGridView1[1, i].Value = mainList.ElementAt(i).ElementAt(1);
                dataGridView1[2, i].Value = mainList.ElementAt(i).ElementAt(2);
                dataGridView1[3, i].Value = mainList.ElementAt(i).ElementAt(3);
                dataGridView1[4, i].Value = mainList.ElementAt(i).ElementAt(4);
            }
            FillListWithTable();
        }
        /// <summary>
        /// Заполнение списка данными из datagridview
        /// </summary>
        private void FillListWithTable()
        {
            List<List<object>> listSafe = new List<List<object>>();
            List<object> tmpList = new List<object>();
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                tmpList.Add(dataGridView1.Rows[i].Cells[0].Value);
                tmpList.Add(dataGridView1.Rows[i].Cells[1].Value);
                tmpList.Add(dataGridView1.Rows[i].Cells[2].Value);
                tmpList.Add(dataGridView1.Rows[i].Cells[3].Value);
                tmpList.Add(dataGridView1.Rows[i].Cells[4].Value);
                listSafe.Add(new List<object>(tmpList));
                tmpList.Clear();
            }
            LinesList = listSafe;  
        }
        /// <summary>
        /// Рисование линий по данным datagridview
        /// </summary>
        /// <param name="list"></param>
        private void DrawLinesOnList(List<List<object>> list)
        {
            List<object> line = new List<object>();
            try
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    line = list[i];
                    Line _t = new Line
                        (
                            line[0].ToString()[0].ToString(),
                            new Point(Convert.ToInt32(line[1]),
                                      Convert.ToInt32(line[2])),

                            line[0].ToString()[1].ToString(),
                            new Point(Convert.ToInt32(line[3]),
                                      Convert.ToInt32(line[4]))
                       );
                    PaintLine(_t);
                }
            }
            catch (NullReferenceException)
            {                             
                LinesList.RemoveAt(LinesList.Count - 1);
                dataGridView1.Rows.RemoveAt(LinesList.Count);
            }
        }

        /// <summary>
        /// Рисует линию из объекта Line
        /// </summary>
        private void PaintLine(Line _line)
        {         
            int X1 = Convert.ToInt32(_line.Point1.posX ) * sizeDiv + indent;
            int Y1 = Convert.ToInt32(_line.Point1.posY ) * sizeDiv + indent;
            int X2 = Convert.ToInt32(_line.Point2.posX) * sizeDiv + indent;
            int Y2 = Convert.ToInt32(_line.Point2.posY) * sizeDiv + indent;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();

            // Цвета
            foreColor = selectColor();

            // Инструменты
            Pen pen = new Pen(foreColor, lineWight);
            Pen point1 = new Pen(FirstChar, 1);
            Pen point2 = new Pen(SecondChar, 1);
            Pen point11 = new Pen(FirstChar, 2);
            Pen point12 = new Pen(SecondChar, 2);

            // Рисовать точки 
            g.DrawRectangle(point11, X1, Y1, 2, 2);
            g.DrawRectangle(point12, X2, Y2, 2, 2);
            g.DrawString(X1.ToString(), new Font("Arial", 10), Brushes.Black, X1 - 5, indent - 20);
            g.DrawString(Y1.ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y1 - 5);
            g.DrawString(X2.ToString(), new Font("Arial", 10), Brushes.Black, X2 - 5, indent - 20);
            g.DrawString(Y2.ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y2 - 5);

            // Рисовать основной линии
            if (checkBox3.Checked == true)
            {
                g.DrawLine(pen, X1, Y1, X2, Y2);
            }

            // Рисовать линии к осям OX OY
            if (checkBox1.Checked == true)
            {
                // Циклы для рисования прерывестых линий
                // Константа размера прочерков
                const int length = 4;

                for (int i1 = Y1; i1 > indent; i1 -= length)
                {   
                    g.DrawLine(point1, X1, i1 - length, X1, i1);
                    i1 -= length;
                }
                for (int i2 = X1; i2 > indent; i2 -= length)
                {
                    g.DrawLine(point1, i2 - length, Y1, i2, Y1);
                    i2 -= length;
                }
                for (int i3 = Y2; i3 > indent; i3 -= length)
                {
                    g.DrawLine(point2, X2, i3 , X2, i3 - length);
                    i3 -= length;
                }
                for (int i4 = X2; i4 > indent; i4 -= length)
                {
                    g.DrawLine(point2, i4, Y2, i4 - length, Y2);
                    i4 -= length;
                }
            }

            // Рисовать наименования точек
            if (checkBox2.Checked == true)
            {            
                g.DrawString(_line.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), FirstChar, X1, Y1);
                g.DrawString(_line.NameLine[1].ToString(), new Font("Arial", 12, FontStyle.Bold), SecondChar, X2, Y2);
            }
        }

        private void PaintRectangle(Line _line1, Line _line2, Line _line3, Line _line4)
        {
            int X1 = Convert.ToInt32(_line1.Point1.posX) * sizeDiv + indent;
            int Y1 = Convert.ToInt32(_line1.Point1.posY) * sizeDiv + indent;
            int X2 = Convert.ToInt32(_line2.Point1.posX) * sizeDiv + indent;
            int Y2 = Convert.ToInt32(_line2.Point1.posY) * sizeDiv + indent;
            int X3 = Convert.ToInt32(_line3.Point1.posX) * sizeDiv + indent;
            int Y3 = Convert.ToInt32(_line3.Point1.posY) * sizeDiv + indent;
            int X4 = Convert.ToInt32(_line4.Point1.posX) * sizeDiv + indent;
            int Y4 = Convert.ToInt32(_line4.Point1.posY) * sizeDiv + indent;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();

            // Цвета
            foreColor = selectColor();

            // Инструменты
            Pen pen = new Pen(foreColor, lineWight);

            Pen point1 = new Pen(Brushes.Red, 2);
            Pen point2 = new Pen(Brushes.Green, 2);
            Pen point3 = new Pen(Brushes.Blue, 2);
            Pen point4 = new Pen(Brushes.DarkGoldenrod, 2);

            // Рисовать точки 
            g.DrawRectangle(point1, X1, Y1, 2, 2);
            g.DrawRectangle(point2, X2, Y2, 2, 2);
            g.DrawRectangle(point3, X3, Y3, 2, 2);
            g.DrawRectangle(point4, X4, Y4, 2, 2);

            g.DrawString(X1.ToString(), new Font("Arial", 10), Brushes.Black, X1, indent - 20);
            g.DrawString(X2.ToString(), new Font("Arial", 10), Brushes.Black, X2, indent - 20);
            g.DrawString(Y2.ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y2);
            g.DrawString(Y4.ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y4);

            // Рисовать основных линиий
            if (checkBox3.Checked == true)
            {
                g.DrawLine(pen, X1, Y1, X2, Y2);
                g.DrawLine(pen, X2, Y2, X3, Y3);
                g.DrawLine(pen, X3, Y3, X4, Y4);
                g.DrawLine(pen, X4, Y4, X1, Y1);
            }

            // Рисовать линии к осям OX OY
            if (checkBox1.Checked == true)
            {
                // Циклы для рисования прерывестых линий
                // Константа размера прочерков
                const int length = 4;

                for (int i1 = Y1; i1 > indent; i1 -= length)
                {
                    g.DrawLine(point1, X1, i1 - length, X1, i1);
                    i1 -= length;
                }
                for (int i2 = X1; i2 > indent; i2 -= length)
                {
                    g.DrawLine(point1, i2 - length, Y1, i2, Y1);
                    i2 -= length;
                }

                for (int i3 = Y2; i3 > indent; i3 -= length)
                {
                    g.DrawLine(point2, X2, i3, X2, i3 - length);
                    i3 -= length;
                }
                for (int i4 = X2; i4 > indent; i4 -= length)
                {
                    g.DrawLine(point2, i4, Y2, i4 - length, Y2);
                    i4 -= length;
                }

                for (int i1 = Y3; i1 > indent; i1 -= length)
                {
                    g.DrawLine(point3, X3, i1 - length, X3, i1);
                    i1 -= length;
                }
                for (int i2 = X3; i2 > indent; i2 -= length)
                {
                    g.DrawLine(point3, i2 - length, Y3, i2, Y3);
                    i2 -= length;
                }
                
                for (int i3 = Y4; i3 > indent; i3 -= length)
                {
                    g.DrawLine(point4, X4, i3, X4, i3 - length);
                    i3 -= length;
                }
                for (int i4 = X4; i4 > indent; i4 -= length)
                {
                    g.DrawLine(point4, i4, Y4, i4 - length, Y4);
                    i4 -= length;
                }
            }

            // Рисовать наименования точек
            if (checkBox2.Checked == true)
            {
                g.DrawString(_line1.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Red, X1, Y1);
                g.DrawString(_line2.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Green, X2, Y2);
                g.DrawString(_line3.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Blue, X3, Y3);
                g.DrawString(_line4.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.DarkGoldenrod, X4, Y4);

            }
        }


        /// <summary>
        /// Координатная ось
        /// </summary>
        private void Os_XY()
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            ok = false;
            const int size = 30 * sizeDiv; // Размер оси координат          
            

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 3);
            Pen line = new Pen(Brushes.LightGray, 1);
            Pen line1 = new Pen(Brushes.Black, 1);

            //const int lenght = 7; // Длинна прочерков
            // // Рисовать разметку по X
            // for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            //     for (int j = indent; j <= size + sizeDiv; j += lenght - 4)
            //         g.DrawLine(line, i, j, i, j += lenght);
            // // Рисовать разметку по Y
            // for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            //     for (int j = indent; j <= size + sizeDiv; j += lenght - 4)
            //         g.DrawLine(line, j, i, j += lenght, i);


            // Ось OX
            g.DrawString((size / sizeDiv).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent);
            g.DrawString("X", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent - 20);
            g.DrawLine(pen, indent, indent, indent + size, indent);

            // Ось OY
            g.DrawString((size / sizeDiv).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent, indent + size);
            g.DrawString("Y",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, indent + size);
            g.DrawLine(pen, indent, indent, indent, indent + size);

            // Рисовать даления на линии оси X
            for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            {
                g.DrawLine(line1, i, indent - 3, i, indent + 3);
            }
            // Рисовать даления на линии оси Y
            for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            {
                g.DrawLine(line1, indent - 3, i, indent + 3, i);
            }


            // Подписать точку начала
            g.DrawString("0", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 15, indent - 20);

        }

        #region Change Color
        /// <summary>
        /// Смена цвета первой точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    FirstChar = Brushes.Red;
                    break;
                case 1:
                    FirstChar = Brushes.Orange;
                    break;
                case 2:
                    FirstChar = Brushes.DarkGoldenrod;
                    break;
                case 3:
                    FirstChar = Brushes.Green;
                    break;
                case 4:
                    FirstChar = Brushes.DeepSkyBlue;
                    break;
                case 5:
                    FirstChar = Brushes.Blue;
                    break;
                case 6:
                    FirstChar = Brushes.BlueViolet;
                    break;
            }
        }
        /// <summary>
        /// Смена цвета второй точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    SecondChar = Brushes.Red;
                    break;
                case 1:
                    SecondChar = Brushes.Orange;
                    break;
                case 2:
                    SecondChar = Brushes.DarkGoldenrod;
                    break;
                case 3:
                    SecondChar = Brushes.Green;
                    break;
                case 4:
                    SecondChar = Brushes.DeepSkyBlue;
                    break;
                case 5:
                    SecondChar = Brushes.Blue;
                    break;
                case 6:
                    SecondChar = Brushes.BlueViolet;
                    break;
            }
        }
        /// <summary>
        /// Цвет основной линии
        /// </summary>
        /// <returns></returns>
        private Color selectColor()
        {
            Color col = new Color();
            switch (numericUpDown2.Value)
            {
                case 0:
                    col = ColorTranslator.FromHtml("#000000");
                    break;
                case 1:
                    col = ColorTranslator.FromHtml("#1a1a1a");
                    break;
                case 2:
                    col = ColorTranslator.FromHtml("#333333");
                    break;
                case 3:
                    col = ColorTranslator.FromHtml("#4d4d4d");
                    break;
                case 4:
                    col = ColorTranslator.FromHtml("#595959");
                    break;
                case 5:
                    col = ColorTranslator.FromHtml("#737373");
                    break;
                case 6:
                    col = ColorTranslator.FromHtml("#808080");
                    break;
                case 7:
                    col = ColorTranslator.FromHtml("#8c8c8c");
                    break;
                case 8:
                    col = ColorTranslator.FromHtml("#999999");
                    break;
                case 9:
                    col = ColorTranslator.FromHtml("#a6a6a6");
                    break;
                case 10:
                    col = ColorTranslator.FromHtml("#b3b3b3");
                    break;
            }
            return col;
        }
        #endregion

        #region General button for draw lines

        /// <summary>
        /// Рисование линии
        /// </summary>
        private void CreateLine()
        {
            pictureBox1.Update();
            Line _t = new Line(
                         firstName_txt.Text,
                         new Point(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text)),
                         secondName_txt.Text,
                         new Point(Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text)));
            PaintLine(_t);
            InsertInList(_t);
        }

        private void CreateRectangle()
        {
            pictureBox1.Update();

            var name = "ABCD";

            Line _t1 = new Line(
              name[0].ToString(),
              new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)),
              name[1].ToString(),
              new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)));
            InsertInList(_t1);

            Line _t2 = new Line(
             name[1].ToString(),
             new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)),
             name[2].ToString(),
             new Point(Convert.ToInt32(posX3r_textbox.Text), Convert.ToInt32(posY3r_textbox.Text)));
            InsertInList(_t2);

            Line _t3 = new Line(
             name[2].ToString(),
             new Point(Convert.ToInt32(posX3r_textbox.Text), Convert.ToInt32(posY3r_textbox.Text)),
             name[3].ToString(),
             new Point(Convert.ToInt32(posX4r_textbox.Text), Convert.ToInt32(posY4r_textbox.Text)));
            InsertInList(_t3);

            Line _t4 = new Line(
             name[3].ToString(),
             new Point(Convert.ToInt32(posX4r_textbox.Text), Convert.ToInt32(posY4r_textbox.Text)),
             name[0].ToString(),
             new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)));
            InsertInList(_t4);

            PaintRectangle(_t1,_t2,_t3,_t4);

        }

        /// <summary>
        /// Кнопка Изобразить 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (ok)
                Os_XY();
            CreateLine();
        }
        /// <summary>
        /// Кнопка Отчистить все (в т.ч. datagridview)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            dataGridView1.Rows.Clear();
            LinesList = null;
            Os_XY();
        }
        /// <summary>
        /// Кнопка Случайная линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (ok)
                Os_XY();
            RandomLine();
            CreateLine();
        }

        private void RandomLine()
        {
            Random rnd1 = new Random(DateTime.Now.Millisecond + 1111 + DateTime.Now.Minute);
            char char1 = (char)rnd1.Next(0x0041, 0x005A);
            Random rnd2 = new Random(DateTime.Now.Millisecond + 2222 + DateTime.Now.Minute);
            char char2 = (char)rnd2.Next(0x0041, 0x005A);
            firstName_txt.Text = char1.ToString();
            secondName_txt.Text = char2.ToString();
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 15 * 999).Next(5, 25).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 25 * 1999).Next(5, 25).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 35 * 2999).Next(5, 25).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 45 * 3999).Next(5, 25).ToString();
            comboBox1.SelectedIndex = new Random(DateTime.Now.Millisecond + 4949).Next(0, 7);
            comboBox2.SelectedIndex = new Random(DateTime.Now.Millisecond + 5959).Next(0, 7);         
        }

        private void RandomRectangle()
        {
            Random rnd1 = new Random(DateTime.Now.Millisecond + 1111 + DateTime.Now.Minute);
            char char1 = (char)rnd1.Next(0x0041, 0x005A);
            Random rnd2 = new Random(DateTime.Now.Millisecond + 2222 + DateTime.Now.Minute);
            char char2 = (char)rnd2.Next(0x0041, 0x005A);
            Random rnd3 = new Random(DateTime.Now.Millisecond + 3333 + DateTime.Now.Minute);
            char char3 = (char)rnd1.Next(0x0041, 0x005A);
            Random rnd4 = new Random(DateTime.Now.Millisecond + 4444 + DateTime.Now.Minute);
            char char4 = (char)rnd2.Next(0x0041, 0x005A);

            textBox1.Text = (char1 + char2 + char3 + char4).ToString();
            posX1r_textbox.Text = new Random(DateTime.Now.Millisecond + 15 * 999).Next(5, 10).ToString();
            posY1r_textbox.Text = new Random(DateTime.Now.Millisecond + 25 * 1999).Next(5, 10).ToString();
            posX2r_textbox.Text = new Random(DateTime.Now.Millisecond + 35 * 2999).Next(5, 25).ToString();
            posY2r_textbox.Text = new Random(DateTime.Now.Millisecond + 45 * 3999).Next(5, 25).ToString();
            posX3r_textbox.Text = new Random(DateTime.Now.Millisecond + 55 * 999).Next(10, 25).ToString();
            posY3r_textbox.Text = new Random(DateTime.Now.Millisecond + 65 * 1999).Next(10, 25).ToString();
            posX4r_textbox.Text = new Random(DateTime.Now.Millisecond + 75 * 2999).Next(10, 25).ToString();
            posY4r_textbox.Text = new Random(DateTime.Now.Millisecond + 85 * 3999).Next(10, 25).ToString();
        }

        #endregion

        /// <summary>
        /// Кнопка Справка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"УПРАВЛЕНИЕ ПРЯМОЙ " + Environment.NewLine + 
                              "Случайные - создает случайную прямую" + Environment.NewLine + 
                              "Изобразить прямую - создает прямую по заданным параметрам" + Environment.NewLine +
                              "РАБОТА С ДАННЫМИ" + Environment.NewLine +
                              "Изобразить - изобразить все прямые из списка" + Environment.NewLine +
                              "Удалить - удалить прямую из списка" + Environment.NewLine +
                              "Отчистить все - отчищает все данные" + Environment.NewLine +
                              "НАСТРОЙКА" + Environment.NewLine +
                              "Показать линии" + Environment.NewLine +
                              "Рисовать линии к осям - отображает линии к осям OX OY" + Environment.NewLine +
                              " - - - - - - - - - - - - - - - - - - - - - - - - - -" + Environment.NewLine +
                              "Используйте кнопку изобразить для построения фигур из прямых."
                              , "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Кнопка Отчистить (Поле)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            Os_XY();
        }

        /// <summary>
        /// Изобразить линии по данным datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //if (ok)
                Os_XY();
            FillListWithTable();
            DrawLinesOnList(LinesList);
        }

        /// <summary>
        /// Удалить прямую из datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(index);
            dataGridView1.Refresh();
            FillListWithTable();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (LinesList != null)
            {
                //if (ok)
                    Os_XY();
                DrawLinesOnList(LinesList);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (LinesList != null)
            {
                //if (ok)
                    Os_XY();
                DrawLinesOnList(LinesList);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox4.Visible = true;
        }


        private void button9_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox4.Visible = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
            {
            if (LinesList != null)
            {
                //if (ok)
                    Os_XY();
                DrawLinesOnList(LinesList);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lineWight = Convert.ToInt32(numericUpDown1.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            foreColor = selectColor();
        }

        /// <summary>
        /// Изобразить квадрат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (ok)
                Os_XY();
            CreateRectangle();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (ok)
                Os_XY();
            RandomRectangle();
            CreateRectangle();
        }

        public Form1()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            foreColor = Color.LightSlateGray;
            lineWight = 2;
            FirstChar = Brushes.Red;
            SecondChar = Brushes.Green;
        }
    }
}
