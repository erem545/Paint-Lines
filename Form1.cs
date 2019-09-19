using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        Bitmap mainScreen;
        Bitmap snapshot, tempDraw;  // Снимки
        Color foreColor;            // Цвет
        Brush FirstChar;
        Brush SecondChar; // Цвет букв
        int lineWight;              // Ширина

        private void Calculate(int posX1, int posY1, int posX2, int posY2)
        {
            int _i = posY1 - posY2;
            int _j = posX2 - posX1;
            int _k = (posX1 * posY2) - (posX2 * posY1);
            equation_txt.Text = $"{_i}x {_j}y {_k}";
        }

        private void PaintLine()
        {

            int X1 = Convert.ToInt32(posX1_textbox.Text) * 10 + 100;
            int Y1 = Convert.ToInt32(posY1_textbox.Text) * 10 + 100;
            int X2 = Convert.ToInt32(posX2_textbox.Text) * 10 + 100;
            int Y2 = Convert.ToInt32(posY2_textbox.Text) * 10 + 100;

            textBox1.Text = posX1_textbox.Text;
            textBox2.Text = posY1_textbox.Text;
            textBox3.Text = posX2_textbox.Text;
            textBox4.Text = posY2_textbox.Text;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();

            Pen pen = new Pen(foreColor, lineWight);
            Pen point1 = new Pen(FirstChar, lineWight);
            Pen point2 = new Pen(SecondChar, lineWight);

            if (tempDraw != null)
                g.DrawLine(pen, X1, Y1, X2, Y2);
            g.DrawString(textBox7.Text, new Font("Arial", 10, FontStyle.Bold), FirstChar, X1, Y1);
            g.DrawRectangle(point1, X1, Y1, 2, 2);
            g.DrawString(textBox8.Text, new Font("Arial", 10, FontStyle.Bold), SecondChar, X2, Y2);
            g.DrawRectangle(point2, X2, Y2, 2, 2);

            if ((X1 > 100) && (Y1 > 100))
            {
                Graphics graph = pictureBox1.CreateGraphics();
                Pen _line1 = new Pen(FirstChar, 1);
                graph.DrawLine(_line1, X1, Y1, X1, 100);
                graph.DrawLine(_line1, X1, Y1, 100, Y1);
                graph.DrawString(posX1_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, X1 - 5, 80);
                graph.DrawString(posY1_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 80, Y1 - 5);

                Pen _line2 = new Pen(SecondChar, 1);
                graph.DrawLine(_line2, X2, Y2, X2, 100);
                graph.DrawLine(_line2, X2, Y2, 100, Y2);
                graph.DrawString(posX2_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, X2 - 5, 80);
                graph.DrawString(posY2_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 80, Y2 - 5);
            }

            pen.Dispose();
            point1.Dispose();
            point2.Dispose();
            g.Dispose();
            
        }

        private void Os_XY()
        {
            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 3);

            g.DrawString("20", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 100, 300);
            g.DrawString("Y",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 80, 300);
            g.DrawLine(pen, 100, 100, 100, 300);

            g.DrawString("20", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 300, 100);
            g.DrawString("X",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 300, 80);
            g.DrawLine(pen, 100, 100, 300, 100);

            g.DrawString("-5", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 30, 100);
            g.DrawString("Y", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 80, 40);
            g.DrawLine(pen, 100, 50, 100, 100);

            g.DrawString("-5", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 100, 30);
            g.DrawString("X", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 40, 80);
            g.DrawLine(pen, 50, 100, 100, 100);

            pen.Dispose();
            g.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Os_XY();
            Calculate(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            //PaintLine();
        }

        private void MovePoint()
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            Calculate(Convert.ToInt32(posX1_textbox.Text) + 10, Convert.ToInt32(posY1_textbox.Text) + 10, Convert.ToInt32(posX2_textbox.Text) + 10, Convert.ToInt32(posY2_textbox.Text) + 10);
            PaintLine();
            Os_XY();

        }
        private void posX1_textbox_Click(object sender, EventArgs e)
        {
            posX1_textbox.Text = "";
        }
        private void posY1_textbox_Click(object sender, EventArgs e)
        {
            posY1_textbox.Text = "";
        }
        private void posX2_textbox_Click(object sender, EventArgs e)
        {
            posX2_textbox.Text = "";
        }
        private void posY2_textbox_Click(object sender, EventArgs e)
        {
            posY2_textbox.Text = "";
        }
        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
        }


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
        private void textBox8_Click(object sender, EventArgs e)
        {
            textBox8.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            Os_XY();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999).Next(5, 15).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1999).Next(5, 15).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2999).Next(5, 15).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3999).Next(5, 15).ToString();
        }

        private void posX1_textbox_ValueChanged(object sender, EventArgs e)
        {
            MovePoint();
        }

        private void posX2_textbox_ValueChanged(object sender, EventArgs e)
        {
            MovePoint();
        }

        private void posY1_textbox_ValueChanged(object sender, EventArgs e)
        {
            MovePoint();
        }

        private void posY2_textbox_ValueChanged(object sender, EventArgs e)
        {
            MovePoint();
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999).Next(5, 15).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1999).Next(5, 15).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2999).Next(5, 15).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3999).Next(5, 15).ToString();
            
        }

        public Form1()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox1.ClientRectangle.Width, pictureBox1.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            foreColor = Color.Gray;
            FirstChar = Brushes.Red;
            SecondChar = Brushes.Green;
            lineWight = 2;
        }
    }
}
