using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TerVer_RGR
{
    public partial class Form1 : Form
    {
        private int Selection; // выборка
        private List<double> Numbers; // СВ
        private int Interval; // интервал

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Numbers = new List<double>();
            numericUpDown1.Maximum = 10000;
            numericUpDown1.Minimum = 50;
            numericUpDown2.Minimum = 5;
            numericUpDown2.Maximum = numericUpDown1.Minimum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNumbers();
            DrawHistogram();
        }

        private void GenerateNumbers()  // Сгенерировать СВ методом усечения
        {
            Numbers.Clear();

            if (Selection <= 0) return;

            Random random = new Random();
            double x, y;
            for (int i = 0; i < Selection; i++) // генерация СВ методом усечения
            {
                do
                {
                    y = random.NextDouble();
                    x = random.NextDouble() * 2 + 1;
                } while (y > (1.5 - 0.5 * x));
                x = Math.Round(x, 2);
                Numbers.Add(x);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Numbers.Count == 0) return;

            DrawHistogram();
        }

        private void DrawHistogram() // Нарисовать гистограмму 
        {
            chart1.Series[0].Points.Clear();

            if (Numbers.Count == 0) return;

            Numbers.Sort();

            double min = Numbers.Min();
            double max = Numbers.Max();

            double intervalLength = (max - min) / Interval;

            int j = 0;
            double xi = 0;
            for (int i = 0; i < Interval; i++)
            {
                int numsInColumn = 0;
                double rightBorder = min + (i + 1) * intervalLength;

                for (; j < Numbers.Count && Numbers[j] <= rightBorder; j++)
                {
                    numsInColumn++;
                }

                
                xi += Math.Pow(numsInColumn - Numbers.Count / Interval, 2) / (Numbers.Count / Interval);
                chart1.Series[0].Points.AddXY(min + (i + 0.5) * intervalLength, numsInColumn / (Numbers.Count * intervalLength));
            }
            label1.Text = xi.ToString();
            Microsoft.Office.Interop.Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
            label2.Text = ex.WorksheetFunction.ChiInv(1-0.95, Numbers.Count - 1).ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Selection = (int)numericUpDown1.Value;
            numericUpDown2.Maximum = numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Interval = (int)numericUpDown2.Value;
        }
    }
}
