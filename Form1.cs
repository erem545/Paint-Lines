using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
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

        List<List<object>> LinesListGroup;
        List<object> tempListGroup;
        Bitmap snapshot, tempDraw; // Снимки
        //Bitmap linesScreen, tempLinesScreen; // Главный экран с линиями
        Bitmap groupDraw, tempGroupDraw; // группировка
        Color foreColor; // Цвет основной линии
        int lineWight; // Ширина линии



        #region Lists

        /// <summary>
        /// Создать список объектов из объекта линии
        /// </summary>
        /// <param name="_line">Объект</param>
        /// <returns></returns>
        private List<object> CreateObjFromLine(Line _line)
        {
            tempList = new List<object>();
            tempList.Add(_line.Point1.Name + _line.Point2.Name);
            tempList.Add(_line.Point1.posX);
            tempList.Add(_line.Point1.posY);
            tempList.Add(_line.Point2.posX);
            tempList.Add(_line.Point2.posY);
            return tempList;
        }

        /// <summary>
        /// Создает новый объект линию из списка объектов
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        private Line CreateLineFromObj(List<object> _list)
        {
            try
            {
                Line line = new Line(
                    new Point(_list[0].ToString()[0].ToString(), Convert.ToInt32(_list[1]), Convert.ToInt32(_list[2])),
                    new Point(_list[0].ToString()[1].ToString(), Convert.ToInt32(_list[3]), Convert.ToInt32(_list[4]))
                    );
                return line;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Вставить объект Line в список tempList
        /// </summary>
        /// <param name="_line">Объект класса Line</param>
        private void InsertInList(Line _line, List<object> _list)
        {
            InsertInList(CreateObjFromLine(_line), ref LinesList);
        }
        /// <summary>
        /// Вставить список tempList в список LinesList
        /// </summary>
        /// <param name="_list">Список со списками List</param>
        private void InsertInList(List<object> _list, ref List<List<object>> _listA)
        {
            if (_listA == null)
                _listA = new List<List<object>>();
            _listA.Add(_list);
            dataGridView1.Rows.Add();
            FillTableWithList(_listA);
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
                    PaintLine(ref snapshot, CreateLineFromObj(line), selectColor(), lineWight, 0);
                }
            }
            catch (NullReferenceException)
            {
                try
                {
                    LinesList.RemoveAt(LinesList.Count - 1);
                    dataGridView1.Rows.RemoveAt(LinesList.Count);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
            }
        }
        #endregion
        
        /// <summary>
        /// Вычислить длину выбранной линии
        /// </summary>
        /// <param name="_line">Линия</param>
        private void CalculateLengthLine(Line _line)
        {
            double l1 = Math.Abs(_line.Point1.posX - _line.Point2.posX);
            double l2 = Math.Abs(_line.Point1.posY - _line.Point2.posY);
            double length = Math.Sqrt((l1 * l1) + (l2 * l2));
            lengthLine_txt.Text = $"Длина {_line.ToString()}: {length}";
        }
        private void PullMatrix(Line _line)
        {
            matrix11_textbox.Text = _line.Matrix[0, 0].ToString();
            matrix12_textbox.Text = _line.Matrix[0, 1].ToString();
            //matrix13_textbox.Text = _line.Matrix[0, 2].ToString();
            matrix21_textbox.Text = _line.Matrix[1, 0].ToString();
            matrix22_textbox.Text = _line.Matrix[1, 1].ToString();
            //matrix23_textbox.Text = _line.Matrix[1, 2].ToString();
        }

        private void RefreshPicture()
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            FillListWithTable();
            snapshot.MakeTransparent(Color.White);
            DrawLinesOnList(LinesList);
            pictureBox1.Image = snapshot;
        }

        #region Draw

        /// <summary>
        /// Изобразить линию по указанным параметрам
        /// </summary>
        /// <param name="screen">Экран</param>
        /// <param name="_line">Линия</param>
        /// <param name="color">Цвет линии</param>
        /// <param name="_lineWeight">Толщина линии</param>
        /// <param name="_pointSize">Размер точки (0 - стандартно (_lineWeight), 1, 2, 3,... - Увеличение точки)</param>
        private void PaintLine(ref Bitmap _screen, Line _line, Color _color, int _lineWeight, int _pointSize)
        {           
            tempDraw = _screen;
            Graphics g = Graphics.FromImage(tempDraw);

            // Инструменты
            Pen pen = new Pen(_color, _lineWeight);

            try
            {
                // Параметры первой точки
                string name1 = _line.Point1.Name;
                int X1 = Convert.ToInt32(_line.Point1.posX) * sizeDiv + indent;
                int Y1 = Convert.ToInt32(_line.Point1.posY) * sizeDiv + indent;

                // Параметры второй точки
                string name2 = _line.Point2.Name;
                int X2 = Convert.ToInt32(_line.Point2.posX) * sizeDiv + indent;
                int Y2 = Convert.ToInt32(_line.Point2.posY) * sizeDiv + indent;

                // Рисовать точки 
                PaintPoint(ref snapshot, name1, X1, Y1, _color, _lineWeight + _pointSize - 1);
                PaintPoint(ref snapshot, name2, X2, Y2, _color, _lineWeight + _pointSize - 1);

                // Рисовать основной линии
                if (checkBox3.Checked == true)
                    g.DrawLine(pen, X1, Y1, X2, Y2);

                _screen = (Bitmap)tempDraw.Clone();
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        /// <summary>
        /// Изобразить координатную ось
        /// </summary>
        private void Os_XY()
        {
            const int maxValue = 30;
            const int size = maxValue * sizeDiv; // Размер оси координат          
            
            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = Graphics.FromImage(tempDraw);

            Pen pen = new Pen(Color.Black, 3);
            Pen line = new Pen(Brushes.LightGray, 1);
            Pen line1 = new Pen(Brushes.Black, 1);

            //if (checkBox4.Checked == true)
            //{
            //    const int lenght = 6; // Длинна прочерков
            //                          // Рисовать разметку по X
            //    for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            //        for (int j = indent; j <= size + sizeDiv; j += lenght - 2)
            //            g.DrawLine(line, i, j, i, j += lenght);
            //    // Рисовать разметку по Y
            //    for (int i = indent + sizeDiv; i <= size + sizeDiv; i += sizeDiv)
            //        for (int j = indent; j <= size + sizeDiv; j += lenght - 2)
            //            g.DrawLine(line, j, i, j += lenght, i);
            //}

            if (checkBox4.Checked == true)
            {
                // Рисовать разметку по X
                for (int i = indent + sizeDiv; i <= size + (sizeDiv * 2); i += sizeDiv)
                    g.DrawLine(line, i, indent, i, sizeDiv * maxValue + maxValue);
                // Рисовать разметку по Y
                for (int i = indent + sizeDiv; i <= size + (sizeDiv * 2); i += sizeDiv)
                    g.DrawLine(line, indent, i, sizeDiv * maxValue + maxValue, i);
            }

            // Ось OX
            g.DrawString((size / sizeDiv).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent);
            g.DrawString("X", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent - 20);
            g.DrawLine(pen, indent, indent, indent + size, indent);

            // Ось OY
            g.DrawString((size / sizeDiv).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent, indent + size);
            g.DrawString("Y",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, indent + size);
            g.DrawLine(pen, indent, indent, indent, indent + size);

            // Подписать координаты на оси
            int index = 1;
            while (index < 30)
            {
                g.DrawString(Convert.ToString(index), new Font("Arial", 10), Brushes.Black, ((index + 1) * sizeDiv) - 7, indent - 20);
                g.DrawString(Convert.ToString(index), new Font("Arial", 10), Brushes.Black, indent - 20, ((index + 1) * sizeDiv) - 7);
                index++;
            }

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

        /// <summary>
        /// Изобразить точку по указанным параметрам
        /// </summary>
        /// <param name="_screen">Экран</param>
        /// <param name="_name">Наименование точки</param>
        /// <param name="_x">Координата X</param>
        /// <param name="_y">Координата Y</param>
        /// <param name="_color">Цвет</param>
        /// <param name="_pointSize">Размер точки</param>
        private void PaintPoint(ref Bitmap _screen, string _name, int _x, int _y,  Color _color, int _pointSize)
        {
            tempDraw = _screen;
            Graphics g = Graphics.FromImage(tempDraw);
            Pen point = new Pen(_color, _pointSize);
            Pen linesXY = new Pen(_color, 1);
            g.DrawRectangle(point, _x, _y, _pointSize, _pointSize);

            // Рисовать наименование точки
            if (checkBox2.Checked == true)        
                g.DrawString(_name.ToString(), new Font("Arial", 12, FontStyle.Bold), Brushes.Black, _x, _y);

            // Рисование прерывестых линий к началу осям OX и OY
            if (checkBox1.Checked == true)
            {
                // Циклы для рисования прерывестых линий к началу осям OX и OY
                // Константа размера прочерков
                const int length = 4;

                for (int i1 = _y; i1 > indent; i1 -= length)
                {
                    g.DrawLine(linesXY, _x, i1 - length, _x, i1);
                    i1 -= length;
                }
                for (int i2 = _x; i2 > indent; i2 -= length)
                {
                    g.DrawLine(linesXY, i2 - length, _y, i2, _y);
                    i2 -= length;
                }
            }
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
        /// Создание линии по точкам с последующим рисованием
        /// </summary>
        private void CreateLine()
        {
            Point p1 = new Point(firstName_txt.Text, Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text));
            Point p2 = new Point(secondName_txt.Text, Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            Line _t = new Line(p1, p2);
            PaintLine(ref tempDraw,_t, selectColor(), lineWight, 0);
            InsertInList(_t, tempList);
            snapshot = (Bitmap)tempDraw.Clone();
            pictureBox1.Image = snapshot;
        }

        /// <summary>
        /// Создать квадрат из линий по точкам с последующим рисованием
        /// </summary>
        private void CreateRectangle()
        {
            string name = rectanglName_textbox.Text.ToString();
            Point p1 = new Point(name[0].ToString(), Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text));
            Point p2 = new Point(name[1].ToString(), Convert.ToInt32(posX1r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text));
            Point p3 = new Point(name[2].ToString(), Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY2r_textbox.Text));
            Point p4 = new Point(name[3].ToString(), Convert.ToInt32(posX2r_textbox.Text), Convert.ToInt32(posY1r_textbox.Text));

            Line _t1 = new Line(p1, p2);
            Line _t2 = new Line(p2, p3);
            Line _t3 = new Line(p3, p4);
            Line _t4 = new Line(p1, p4);

            InsertInList(_t1, tempList);
            PaintLine(ref snapshot, _t1, selectColor(), lineWight, 0);

            InsertInList(_t2, tempList);
            PaintLine(ref snapshot, _t2, selectColor(), lineWight, 0);

            InsertInList(_t3, tempList);
            PaintLine(ref snapshot, _t3, selectColor(), lineWight, 0);

            InsertInList(_t4, tempList);
            PaintLine(ref snapshot, _t4, selectColor(), lineWight, 0);
           
            snapshot = (Bitmap)tempDraw.Clone();
            pictureBox1.Image = snapshot;
        }

        /// <summary>
        /// Кнопка Изобразить (Линию)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            CreateLine();
            pictureBox1.Image = snapshot;
        }

        /// <summary>
        /// Кнопка Изобразить (Квадрат)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            CreateRectangle();
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
            //groupPicturebox.Image = null;
            //groupPicturebox.Update();

            Os_XY();
            dataGridView1.Rows.Clear();

            LinesList = null;
            Graphics g1 = Graphics.FromImage(snapshot);
            g1.Clear(Color.White);
            tempDraw = snapshot;
            groupDraw = snapshot;
            tempGroupDraw = snapshot;
        }
        /// <summary>
        /// Кнопка Случайная линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            snapshot.MakeTransparent(Color.White);
            RandomLine();
            CreateLine();
            DrawLinesOnList(LinesList);
            pictureBox1.Image = snapshot;
        }

        /// <summary>
        /// Задать случайные параметры для линии
        /// </summary>
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

        /// <summary>
        /// Задать случайные параметры для квадрата
        /// </summary>
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
            StringBuilder sb = new StringBuilder();
            sb.Append(char1);
            sb.Append(char2);
            sb.Append(char3);
            sb.Append(char4);
            rectanglName_textbox.Text = sb.ToString();
            posX1r_textbox.Text = new Random(DateTime.Now.Millisecond + 15 * 999).Next(5, 15).ToString();
            posY1r_textbox.Text = new Random(DateTime.Now.Millisecond + 25 * 1999).Next(5, 15).ToString();
            posX2r_textbox.Text = new Random(DateTime.Now.Millisecond + 35 * 2999).Next(16, 25).ToString();
            posY2r_textbox.Text = new Random(DateTime.Now.Millisecond + 45 * 3999).Next(16, 25).ToString();
        }

        #endregion

        #region Grouping

        private void SelectLine(Line _line, Color _color)
        {
            Line selectedLine = _line;
            DrawLinesOnList(LinesList);
            PaintLine(ref snapshot, selectedLine, _color, lineWight + 1, 0);
            if (LinesListGroup != null)
                LinesListGroup.Add(CreateObjFromLine(_line));

            CalculateLengthLine(_line);
            eduqationText_txt.Text = _line.Equation_val.ToString();
            PullMatrix(_line);
            pictureBox1.Image = snapshot;
        }
        private bool CheckSelected()
        {
            if (LinesList == null)
                return false;
            else
                return true;
        }
        private void PaintSelectLineList(List<List<object>> _list)
        {
            //groupPicturebox.Image = null;
            //groupPicturebox.Update();

            Graphics g = Graphics.FromImage(tempGroupDraw);
            g.Clear(Color.White);
            groupDraw = tempGroupDraw;

            foreColor = selectColor();
            Pen pen = new Pen(Color.Red, 2);
            Pen point = new Pen(Color.Black, 3);
            int start = 20;
            int size;
            if (_list.Count != 0)
                size = 600 / _list.Count;
            else
                size = 600;

            int startY = 0; // = groupPicturebox.ClientRectangle.Height / 2;

            for (int i = 0; i < _list.Count; i++)
            {
                int startA = start + (size * i);
                int endA = start + (size * (i + 1));
                g.DrawLine(pen, startA, startY, endA, startY);

                // Рисовать точки 
                g.DrawRectangle(point, startA, startY - 2, 3, 3);
                g.DrawRectangle(point, endA, startY - 2, 3, 3);

                // Подписать название
                g.DrawString(Convert.ToString(_list[i][0].ToString()[0].ToString()), new Font("Arial", 8), Brushes.Black, startA - 10, startY + 5);

                // Подписать координаты
                g.DrawString(Convert.ToString(_list[i][1] + ";" + _list[i][2]), new Font("Arial", 8), Brushes.Black, startA - 10, startY - 20);

                if (i == LinesListGroup.Count - 1)
                {
                    g.DrawString(Convert.ToString(_list[i][0].ToString()[1].ToString()), new Font("Arial", 8), Brushes.Black, endA - 10, startY + 5);
                    g.DrawString(Convert.ToString(_list[i][3] + ";" + _list[i][4]), new Font("Arial", 8), Brushes.Black, endA - 10, startY - 20);
                }
            }
            groupDraw = (Bitmap)tempGroupDraw.Clone();
            //groupPicturebox.Image = groupDraw;
        }
        private void CheckCheckedInTable() // Криво работает, не удаляет элементы, неккоректно добавляет.
                                           // Если элементы не связаны между собой - рисовать разрыв вместо соединения.
        {
            int index = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[5];
                if (chk.Value == chk.FalseValue || chk.Value == null)
                {
                    
                    
                }
                else
                {       
                    List<object> obj = new List<object>();
                    obj.Add(dataGridView1.Rows[index].Cells[0].Value);
                    obj.Add(dataGridView1.Rows[index].Cells[1].Value);
                    obj.Add(dataGridView1.Rows[index].Cells[2].Value);
                    obj.Add(dataGridView1.Rows[index].Cells[3].Value);
                    obj.Add(dataGridView1.Rows[index].Cells[4].Value);
                    LinesListGroup.Add(obj);
                    index++;
                }

            }
            PaintSelectLineList(LinesListGroup);
            LinesListGroup = new List<List<object>>();
        }

        #endregion

        #region Buttons
        /// <summary>
        /// Изобразить линии по данным datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            RefreshPicture();
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

        /// <summary>
        /// Изменить толщину линий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lineWight = Convert.ToInt32(numericUpDown1.Value);
            RefreshPicture();
        }
        /// <summary>
        /// Изменить насыщенность линий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            foreColor = selectColor();
            RefreshPicture();
        }

        /// <summary>
        /// Изобразить случайный квадрат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            snapshot.MakeTransparent(Color.White);
            RandomRectangle();
            CreateRectangle();
            pictureBox1.Image = snapshot;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(@"*** РАБОТА С ДАННЫМИ ***" + Environment.NewLine +
                             "Изобразить - изобразить все прямые из списка" + Environment.NewLine +
                             "Удалить - удалить прямую из списка" + Environment.NewLine +
                             "Отчистить все - отчищает все данные" + Environment.NewLine + Environment.NewLine +
                             "*** НАСТРОЙКА ***" + Environment.NewLine +
                             "Показать линии - скрывает/отображает линии" + Environment.NewLine +
                             "Линии к осям - скрывает/отображает линии к осям OX OY" + Environment.NewLine +
                             "Наименование точке - скрывает/отображает наименования точек" + Environment.NewLine +
                             "Разметка - скрывает/отображает разметку" + Environment.NewLine +
                             "Толщина линии - изменение толщены линий" + Environment.NewLine +
                             "Насыщенность линии - изменение цвета (насыщенности) линий" + Environment.NewLine +
                             " - - - - - - - - - - - - - - - - - - - - - - - - - -", 
                             "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SelectLine_btn_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox4.Visible = false;
        }

        private void SelectRectangle_btn_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox4.Visible = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            snapshot.MakeTransparent(Color.White);
            Os_XY();
            //pictureBox1.Image = snapshot;
            pictureBox1.Update();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int _X = e.X;
            int _Y = e.Y;

            _X = _X / 10;
            _Y = _Y / 10;

            // Выравнивание точки по разметки OX
            if (_X % 2 != 0)
            {
                _X *= 10;
            }
            else
            {       
                while (_X % 2 == 0)
                    _X++;
                _X *= 10;
            }
            // Выравнивание точки по разметки OY
            if (_Y % 2 != 0)
            {
                _Y *= 10;
            }
            else
            {
                while (_Y % 2 == 0)
                    _Y++;
                _Y *= 10;
            }
            RefreshPicture();
            PaintPoint(ref tempDraw, "", _X, _Y, Color.Red, lineWight);
            snapshot = (Bitmap)tempDraw.Clone();
            textBox1.Text = $"{_X / sizeDiv - 1}:{_Y / sizeDiv - 1}";
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Image = snapshot;        
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Выбранный элемент таблицы

            try
            {
                int index = dataGridView1.CurrentRow.Index;
                if (index < LinesList.Count)
                {
                    Line _line = CreateLineFromObj(LinesList.ElementAt(index));
                    EditingCapability.StretchShrink(ref _line, 1.05f);
                    LinesList[0] = CreateObjFromLine(_line);

                    FillTableWithList(LinesList);
                    RefreshPicture();
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        #endregion

        /// <summary>
        /// Выбор данных в datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            // Выбранный элемент таблицы
            int index = dataGridView1.CurrentRow.Index;
            try
            {
                if (index < LinesList.Count)
                {
                    List<object> obj = LinesList.ElementAt(index);
                    SelectLine(CreateLineFromObj(obj), Color.Blue);
                    LinesListGroup = new List<List<object>>();
                    LinesListGroup.Add(obj);         
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        public Form1()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            //groupDraw = new Bitmap(groupPicturebox.ClientRectangle.Width, groupPicturebox.ClientRectangle.Height);
            //tempGroupDraw = (Bitmap)groupDraw.Clone();
            foreColor = selectColor();
            lineWight = 2;
        }
    }
}
