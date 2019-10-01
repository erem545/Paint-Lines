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
        //Bitmap linesScreen, tempLinesScreen; // Главный экран с линиями
        Bitmap groupDraw, tempGroupDraw; // группировка
        Color foreColor; // Цвет основной линии
        int lineWight; // Ширина линии



        #region Lists
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
            dataGridView1.Rows.Add();
            FillTableWithList(LinesList);
        }
        /// <summary>
        /// Заполнение datagridview данными из списка
        /// </summary>
        /// <param name="mainList">Список</param>
        private void FillTableWithList(List<List<object>> mainList)
        {
            if (mainList.Count > 0)
            {
                for (int i = 0; i < mainList.Count; i++)
                {
                    dataGridView1[0, i].Value = mainList.ElementAt(i).ElementAt(0);
                    dataGridView1[1, i].Value = mainList.ElementAt(i).ElementAt(1);
                    dataGridView1[2, i].Value = mainList.ElementAt(i).ElementAt(2);
                    dataGridView1[3, i].Value = mainList.ElementAt(i).ElementAt(3);
                    dataGridView1[4, i].Value = mainList.ElementAt(i).ElementAt(4);
                }
            }
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
            Graphics g = Graphics.FromImage(snapshot);
            g.Clear(Color.White);
            Os_XY();
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
                    PaintLine(ref snapshot, _t, selectColor(), 2);
                }
            }
            catch (NullReferenceException)
            {                             
                LinesList.RemoveAt(LinesList.Count - 1);
                dataGridView1.Rows.RemoveAt(LinesList.Count);
            }
        }
        #endregion

        #region Draw
        /// <summary>
        /// Рисует линию из объекта Line
        /// </summary>
        private void PaintLine(ref Bitmap screen, Line _line, Color color, int _lineWeight)
        {

            int X1 = Convert.ToInt32(_line.Point1.posX ) * sizeDiv + indent;
            int Y1 = Convert.ToInt32(_line.Point1.posY ) * sizeDiv + indent;
            int X2 = Convert.ToInt32(_line.Point2.posX) * sizeDiv + indent;
            int Y2 = Convert.ToInt32(_line.Point2.posY) * sizeDiv + indent;

            tempDraw = screen;
            Graphics g = Graphics.FromImage(tempDraw);
            // Цвета
            foreColor = selectColor();

            // Инструменты
            Pen pen = new Pen(color, _lineWeight);
            Pen point = new Pen(color, 3);
            Pen linesXY = new Pen(color, 1);

            // Рисовать точки 
            g.DrawRectangle(point, X1, Y1, 2, 2);
            g.DrawRectangle(point, X2, Y2, 2, 2);


            g.DrawString(Convert.ToInt32(_line.Point1.posX).ToString(), new Font("Arial", 10), Brushes.Black, X1 - 5, indent - 20);
            g.DrawString(Convert.ToInt32(_line.Point1.posY).ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y1 - 5);
            g.DrawString(Convert.ToInt32(_line.Point2.posX).ToString(), new Font("Arial", 10), Brushes.Black, X2 - 5, indent - 20);
            g.DrawString(Convert.ToInt32(_line.Point2.posY).ToString(), new Font("Arial", 10), Brushes.Black, indent - 20, Y2 - 5);

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
                    g.DrawLine(linesXY, X1, i1 - length, X1, i1);
                    i1 -= length;
                }
                for (int i2 = X1; i2 > indent; i2 -= length)
                {
                    g.DrawLine(linesXY, i2 - length, Y1, i2, Y1);
                    i2 -= length;
                }
                for (int i3 = Y2; i3 > indent; i3 -= length)
                {
                    g.DrawLine(linesXY, X2, i3 , X2, i3 - length);
                    i3 -= length;
                }
                for (int i4 = X2; i4 > indent; i4 -= length)
                {
                    g.DrawLine(linesXY, i4, Y2, i4 - length, Y2);
                    i4 -= length;
                }
            }

            // Рисовать наименования точек
            if (checkBox2.Checked == true)
            {            
                g.DrawString(_line.NameLine[0].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, X1, Y1);
                g.DrawString(_line.NameLine[1].ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, X2, Y2);
            }
            
            screen = (Bitmap)tempDraw.Clone();
        }
        private void PaintRectangle(Line _line1, Line _line2, Line _line3, Line _line4)
        {
            PaintLine(ref snapshot, _line1, selectColor(), 2);
            PaintLine(ref snapshot, _line2, selectColor(), 2);
            PaintLine(ref snapshot, _line3, selectColor(), 2);
            PaintLine(ref snapshot, _line4, selectColor(), 2);
        }
        /// <summary>
        /// Координатная ось
        /// </summary>
        private void Os_XY()
        {
            ok = false;
            const int size = 30 * sizeDiv; // Размер оси координат          
            
            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = Graphics.FromImage(tempDraw);

            Pen pen = new Pen(Color.Black, 3);
            Pen line = new Pen(Brushes.LightGray, 1);
            Pen line1 = new Pen(Brushes.Black, 1);

            if (checkBox4.Checked == true)
            {
                const int lenght = 7; // Длинна прочерков
                                      // Рисовать разметку по X
                for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
                    for (int j = indent; j <= size + sizeDiv; j += lenght - 4)
                        g.DrawLine(line, i, j, i, j += lenght);
                // Рисовать разметку по Y
                for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
                    for (int j = indent; j <= size + sizeDiv; j += lenght - 4)
                        g.DrawLine(line, j, i, j += lenght, i);
            }

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
                g.DrawLine(line1, i, indent - 3, i, indent + 3);
            // Рисовать даления на линии оси Y
            for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
                g.DrawLine(line1, indent - 3, i, indent + 3, i);
            // Подписать точку начала
            g.DrawString("0", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 15, indent - 20);
            snapshot = (Bitmap)tempDraw.Clone();
        }
        #endregion

        #region Change Color
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
            Line _t = new Line(
                         firstName_txt.Text,
                         new Point(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text)),
                         secondName_txt.Text,
                         new Point(Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text)));
            PaintLine(ref snapshot, _t, ForeColor, 2);
            InsertInList(_t);
            pictureBox1.Image = snapshot;
        }

        private void CreateRectangle()
        {
            var name = "ABCD";

            Line _t1 = new Line(
              name[0].ToString(),
              new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)),
              name[1].ToString(),
              new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)));
            InsertInList(_t1);
            PaintLine(ref tempDraw, _t1, Color.Black, 2);

            Line _t2 = new Line(
             name[1].ToString(),
             new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)),
             name[2].ToString(),
             new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)));
            InsertInList(_t2);
            PaintLine(ref tempDraw, _t2, Color.Black, 2);

            Line _t3 = new Line(
             name[2].ToString(),
             new Point(Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)),
             name[3].ToString(),
             new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)));
            InsertInList(_t3);
            PaintLine(ref tempDraw, _t3, Color.Black, 2);

            Line _t4 = new Line(
             name[3].ToString(),
             new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text)),
             name[0].ToString(),
             new Point(Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text)));
            InsertInList(_t4);
            PaintLine(ref tempDraw, _t4, Color.Black, 2);

            PaintRectangle(_t1,_t2,_t3,_t4);
            pictureBox1.Image = snapshot;
            snapshot = (Bitmap)tempDraw.Clone();
        }

        /// <summary>
        /// Кнопка Изобразить 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Os_XY();
            pictureBox1.Image = snapshot;
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
            groupPicturebox.Image = null;
            groupPicturebox.Update();

            Os_XY();
            dataGridView1.Rows.Clear();

            LinesList = null;
            Graphics g1 = Graphics.FromImage(snapshot);
            g1.Clear(Color.White);

            Graphics g2 = Graphics.FromImage(tempDraw);
            g2.Clear(Color.White);

            Graphics g3 = Graphics.FromImage(groupDraw);
            g3.Clear(Color.White);

            Graphics g4 = Graphics.FromImage(tempGroupDraw);
            g4.Clear(Color.White);

            g1.Dispose();
            g2.Dispose();
            g3.Dispose();
            g4.Dispose();
        }
        /// <summary>
        /// Кнопка Случайная линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
           // Os_XY();
            snapshot.MakeTransparent(Color.White);
            RandomLine();
            CreateLine();
            DrawLinesOnList(LinesList);
            pictureBox1.Image = snapshot;
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
            posX1r_textbox.Text = new Random(DateTime.Now.Millisecond + 15 * 999).Next(5, 15).ToString();
            posY1r_textbox.Text = new Random(DateTime.Now.Millisecond + 25 * 1999).Next(5, 15).ToString();
            posX2r_textbox.Text = new Random(DateTime.Now.Millisecond + 35 * 2999).Next(15, 25).ToString();
            posY2r_textbox.Text = new Random(DateTime.Now.Millisecond + 45 * 3999).Next(15, 25).ToString();
        }

        #endregion

        #region Grouping

        private void SelectLine(Line _line)
        {
            Line selectedLine = _line;
            DrawLinesOnList(LinesList);
            PaintLine(ref snapshot, selectedLine, Color.Red, 2);          
            DrawSelectLine(selectedLine);          
        }
        private bool CheckSelected()
        {
            if (LinesList == null)
                return false;
            else
                return true;
        }

        private void DrawSelectLine(Line _line)
        {
            groupPicturebox.Image = null;
            groupPicturebox.Update();
            int X1 = Convert.ToInt32(_line.Point1.posX);
            int Y1 = Convert.ToInt32(_line.Point1.posY) * sizeDiv + indent;
            int X2 = Convert.ToInt32(_line.Point2.posX) * sizeDiv + indent;
            int Y2 = Convert.ToInt32(_line.Point2.posY) * sizeDiv + indent;

            tempGroupDraw = (Bitmap)groupDraw.Clone();
            Graphics g = groupPicturebox.CreateGraphics();

            foreColor = selectColor();
            Pen pen = new Pen(Color.Red, 2);
            Pen point = new Pen(Color.Black, 3);

            int startX = 10;
            int startY = groupPicturebox.ClientRectangle.Height / 2;

            g.DrawLine(pen, startX, startY, startX + 100, startY);

            // Рисовать точки 
            g.DrawRectangle(point, startX, startY - 2, 3, 3);
            g.DrawRectangle(point, startX + 100, startY - 2, 3, 3);

            // Подписать название
            g.DrawString(Convert.ToString(_line.NameLine[0]), new Font("Arial", 10, FontStyle.Bold), Brushes.Red, startX, startY + 5);
            g.DrawString(Convert.ToString(_line.NameLine[1]), new Font("Arial", 10, FontStyle.Bold), Brushes.Red, startX + 100, startY + 5);

            // Подписать координаты
            g.DrawString(Convert.ToString(_line.Point1.posX + ";" + _line.Point1.posY), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, startX - 10, startY - 20);
            g.DrawString(Convert.ToString(_line.Point2.posX + ";" + _line.Point2.posY), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, startX + 100 - 10, startY - 20);
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
            pictureBox1.Image = null;
            pictureBox1.Update();
            //Os_XY();
            FillListWithTable();
            snapshot.MakeTransparent(Color.White);
            DrawLinesOnList(LinesList);
            pictureBox1.Image = snapshot;
        }
        /// <summary>
        /// Удалить прямую из datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            
            try
            {             
                int index = dataGridView1.CurrentRow.Index;
                dataGridView1.Rows.RemoveAt(index);
                dataGridView1.Refresh();
                FillListWithTable();                
                DrawLinesOnList(LinesList);
                pictureBox1.Image = snapshot;
            }
            catch (InvalidOperationException)
            {
                return;
            }
            catch (NullReferenceException)
            {
                return;
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
            //Os_XY();
            CreateRectangle();
            pictureBox1.Image = snapshot;
        }
        /// <summary>
        /// Изобразить случайный квадрат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            //Os_XY();
            snapshot.MakeTransparent(Color.White);
            RandomRectangle();
            CreateRectangle();
            pictureBox1.Image = snapshot;
        }

        
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Выбранный элемент таблицы
            int index = dataGridView1.CurrentRow.Index;
            if ((CheckSelected()) && (index < LinesList.Count))
            {                
                List<object> obj = LinesList.ElementAt(index);
                Line line = new Line
                    (
                    obj[0].ToString()[0].ToString(),
                    new Point(Convert.ToInt32(obj[1]), Convert.ToInt32(obj[2])),
                    obj[0].ToString()[1].ToString(),
                    new Point(Convert.ToInt32(obj[3]), Convert.ToInt32(obj[4]))
                );
                SelectLine(line);
            }
        }

        public Form1()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            groupDraw = new Bitmap(groupPicturebox.ClientRectangle.Width, groupPicturebox.ClientRectangle.Height);
            tempGroupDraw = (Bitmap)groupDraw.Clone();
            foreColor = Color.LightSlateGray;
            lineWight = 2;
        }
    }
}
