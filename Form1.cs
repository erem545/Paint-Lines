﻿using System;
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
        const int indent = 20;

        Image mainScreen;
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
            Os_XY();
            if (mainScreen !=  null)
                pictureBox1.Image = mainScreen;
            int X1 = Convert.ToInt32(posX1_textbox.Text) * 10 + indent;
            int Y1 = Convert.ToInt32(posY1_textbox.Text) * 10 + indent;
            int X2 = Convert.ToInt32(posX2_textbox.Text) * 10 + indent;
            int Y2 = Convert.ToInt32(posY2_textbox.Text) * 10 + indent;

            textBox1.Text = posX1_textbox.Text;
            textBox2.Text = posY1_textbox.Text;
            textBox3.Text = posX2_textbox.Text;
            textBox4.Text = posY2_textbox.Text;

            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();

            Pen pen = new Pen(foreColor, lineWight);
            Pen point1 = new Pen(FirstChar, 1);
            Pen point2 = new Pen(SecondChar, 1);


            // Рисование основной линии
            if ((X1 > indent) && (Y1 > indent))
            {
                g.DrawLine(pen, X1, Y1, X2, Y2);
                g.DrawString(posX1_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, X1 - 5, indent - 20);
                g.DrawString(posY1_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, Y1 - 5);
                g.DrawString(posX2_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, X2 - 5, indent - 20);
                g.DrawString(posY2_textbox.Text, new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, Y2 - 5);
            }

            g.DrawString(equation_txt.Text, new Font("Arial", 7, FontStyle.Bold), Brushes.Black, X1 + 10, Y2 + 10);
            
            // Рисовать линии к осям OX OY
            if (checkBox1.Checked == true)
            {                
                g.DrawLine(point1, X1, Y1, X1, indent);
                g.DrawLine(point1, X1, Y1, indent, Y1);                        
                g.DrawLine(point2, X2, Y2, X2, indent);
                g.DrawLine(point2, X2, Y2, indent, Y2);
            }

            // Точки
            g.DrawString(textBox7.Text, new Font("Arial", 10, FontStyle.Bold), FirstChar, X1, Y1);
            g.DrawString(textBox8.Text, new Font("Arial", 10, FontStyle.Bold), SecondChar, X2, Y2);
            g.DrawRectangle(point1, X1, Y1, 2, 2);
            g.DrawRectangle(point2, X2, Y2, 2, 2);


        }

        private void Os_XY()
        {
            const int size = 400;
            tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, 3);

            g.DrawString("40", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent, indent + size);
            g.DrawString("Y",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 20, indent + size);
            g.DrawLine(pen, indent, indent, indent, indent + size);

            g.DrawString("0", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent - 15, indent - 20);

            g.DrawString("40", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent);
            g.DrawString("X",  new Font("Arial", 10, FontStyle.Bold), Brushes.Black, indent + size, indent - 20);
            g.DrawLine(pen, indent, indent, indent + size, indent);
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

        /// <summary>
        /// Изобразить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Update();
            Calculate(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            PaintLine();
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
            Os_XY();
        }

        /// <summary>
        /// Случайная линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = mainScreen;
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999).Next(5, 15).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1919).Next(5, 15).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2929).Next(5, 15).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3939).Next(5, 15).ToString();
            comboBox1.SelectedIndex = new Random(DateTime.Now.Millisecond + 4949).Next(0, 7);
            comboBox2.SelectedIndex = new Random(DateTime.Now.Millisecond + 5959).Next(0, 7);
            pictureBox1.Update();
            Os_XY();
            Calculate(Convert.ToInt32(posX1_textbox.Text), Convert.ToInt32(posY1_textbox.Text), Convert.ToInt32(posX2_textbox.Text), Convert.ToInt32(posY2_textbox.Text));
            PaintLine();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox1.Image = mainScreen;
            PaintLine();
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

            Os_XY();
            posX1_textbox.Text = new Random(DateTime.Now.Millisecond + 999).Next(5, 15).ToString();
            posY1_textbox.Text = new Random(DateTime.Now.Millisecond + 1999).Next(5, 15).ToString();
            posX2_textbox.Text = new Random(DateTime.Now.Millisecond + 2999).Next(5, 15).ToString();
            posY2_textbox.Text = new Random(DateTime.Now.Millisecond + 3999).Next(5, 15).ToString();
        }
    }
}
