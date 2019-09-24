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
        const int indent = 30; // Отступ от начала экрана
        List<List<object>> LinesList; // Список прямых (Список в списке)
        List<object> tempList; // Дополнительный список
        Bitmap snapshot, tempDraw; // Снимки
        Color foreColor; // Цвет основной линии
        Brush FirstChar; // Цвет первого символа
        Brush SecondChar; // Цвет второго символа             
        int lineWight; // Ширина линии

        /// <summary>
        /// Считать уравнение
        /// </summary>
        /// <param name="posX1">Позиция X1</param>
        /// <param name="posY1">Позиция Y1</param>
        /// <param name="posX2">Позиция X2</param>
        /// <param name="posY2">Позиция Y2</param>
        private void Calculate(int posX1, int posY1, int posX2, int posY2)
        {
            int _i = posY1 - posY2;
            int _j = posX2 - posX1;
            int _k = (posX1 * posY2) - (posX2 * posY1);
            equation_txt.Text = $"{_i}x {_j}y {_k}";
        }

        /// <summary>
        /// Вставить объект Line в список tempList
        /// </summary>
        /// <param name="_line">Объект класса Line</param>
        private void InsertInList(Line _line)
        {
            if (tempList == null)
                tempList = new List<object>();
            tempList.Add(_line.NameLine + ": " + equation_txt.Text);
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

        private void DrawLine(List<object> _list)
        {
            foreach(Line el in _list)
            {
                PaintLine(el.Point1.posX, el.Point1.posY, el.Point2.posX, el.Point2.posY);
            }
        }
        private void DrawLineOnList(List<List<object>> _list)
        {
            foreach (List<object> el in _list)
            {
                DrawLine(el);
            }
        }


        /// <summary>
        /// Рисует линию по указанным параметрам
        /// </summary>
        private void PaintLine(int _x1, int _y1, int _x2, int _y2)
        {         
            Os_XY();

            // Создание объектов класса
            Point FirstPoint = new Point(_x1, _y1);
            Point SecondPoint = new Point(_x2, _y2);
            Line line = new Line(firstName_txt.Text, FirstPoint, secondName_txt.Text, SecondPoint);
            InsertInList(line);

            /* Конвертирование значений из textBox'ов
               Умножаем на 10 (Размер деления) 
            */
            int X1 = Convert.ToInt32(posX1_textbox.Text) * 10 + indent;
            int Y1 = Convert.ToInt32(posY1_textbox.Text) * 10 + indent;
            int X2 = Convert.ToInt32(posX2_textbox.Text) * 10 + indent;
            int Y2 = Convert.ToInt32(posY2_textbox.Text) * 10 + indent;

            // Отображение последних координат
            textBox1.Text = posX1_textbox.Text;
            textBox2.Text = posY1_textbox.Text;
            textBox3.Text = posX2_textbox.Text;
            textBox4.Text = posY2_textbox.Text;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();

            foreColor = selectColor();

            // Инструменты
            Pen pen = new Pen(foreColor, lineWight);
            Pen point1 = new Pen(FirstChar, 1);
            Pen point2 = new Pen(SecondChar, 1);
            
            // Рисовать уравнение
            g.DrawString(equation_txt.Text, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, X1 + 10, Y2 + 10);
            
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

            // Рисовать основной линии
            g.DrawLine(pen, X1, Y1, X2, Y2);
            g.DrawString(posX1_textbox.Text, new Font("Arial", 10), Brushes.Black, X1 - 5, indent - 20);
            g.DrawString(posY1_textbox.Text, new Font("Arial", 10), Brushes.Black, indent - 20, Y1 - 5);
            g.DrawString(posX2_textbox.Text, new Font("Arial", 10), Brushes.Black, X2 - 5, indent - 20);
            g.DrawString(posY2_textbox.Text, new Font("Arial", 10), Brushes.Black, indent - 20, Y2 - 5);

            // Точки
            g.DrawString(firstName_txt.Text, new Font("Arial", 12, FontStyle.Bold), FirstChar, X1, Y1);
            g.DrawString(secondName_txt.Text, new Font("Arial", 12, FontStyle.Bold), SecondChar, X2, Y2);
            g.DrawRectangle(point1, X1, Y1, 2, 2);
            g.DrawRectangle(point2, X2, Y2, 2, 2);
        }

        /// <summary>
        /// Координатная ось
        /// </summary>
        private void Os_XY()
        {
            // Размер оси координат
            const int size = 300;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 3);

            // Ось OX
            g.DrawString((size / 10).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent);
            g.DrawString("X", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent - 20);
            g.DrawLine(pen, indent, indent, indent + size, indent);

            // Ось OY
            g.DrawString((size / 10).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent, indent + size);
            g.DrawString("Y",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, indent + size);
            g.DrawLine(pen, indent, indent, indent, indent + size);

            // Начало
            g.DrawString("0", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 15, indent - 20);
        }

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
                    FirstChar = Brushes.YellowGreen;
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
                    SecondChar = Brushes.YellowGreen;
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
        /// Изобразить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Update();
            Calculate(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            PaintLine(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));          
        }

        /// <summary>
        /// Случайная линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {     
            Random rnd1 = new Random(DateTime.Now.Millisecond + 1111 + DateTime.Now.Minute);
            char char1 = (char)rnd1.Next(0x0041, 0x005A);
            Random rnd2 = new Random(DateTime.Now.Millisecond + 2222 + DateTime.Now.Minute);
            char char2 = (char)rnd2.Next(0x0041, 0x005A);

            firstName_txt.Text = char1.ToString();
            secondName_txt.Text = char2.ToString();

            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999).Next(5, 15).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1919).Next(5, 15).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2929).Next(5, 15).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3939).Next(5, 15).ToString();
            comboBox1.SelectedIndex = new Random(DateTime.Now.Millisecond + 4949).Next(0, 7);
            comboBox2.SelectedIndex = new Random(DateTime.Now.Millisecond + 5959).Next(0, 7);

            pictureBox1.Update();
            Calculate(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            PaintLine(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));           
        }

        /// <summary>
        /// Отчистить поле
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
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Случайные - создает случайную Прямую с заданными именами Точек" + Environment.NewLine + 
                              "Изобразить - создает прямую по заданным параметрам" + Environment.NewLine +
                              "Отчистить - отчищает рабочую область" + Environment.NewLine +
                              "Рисовать линии к осям - отображает линии к осям OX OY" + Environment.NewLine +
                              " - - - - - - - - - - - - - - - - - - - - - - - - - -" + Environment.NewLine +
                              "Используйте кнопку изобразить для построения фигур из прямых."
                              , "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lineWight = Convert.ToInt32(numericUpDown1.Value);
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            foreColor = selectColor();
        }
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

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            Os_XY();
        }

        private void button6_Click(object sender, EventArgs e)
        {

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
            Os_XY();
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999  - DateTime.Now.Millisecond*1).Next(5, 25).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1999 - DateTime.Now.Millisecond/2).Next(5, 25).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2999 - DateTime.Now.Millisecond*3).Next(5, 25).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3999 - DateTime.Now.Millisecond/4).Next(5, 25).ToString();
        }
    }
}
