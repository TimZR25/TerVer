using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace TerVer_LB3
{
    public partial class Form1 : Form
    {
        private List<double> Numbers; // СВ
        private int Selection; // выборка
        private int Interval; // интервал
        private int MathExpectation; // мат ожидание
        private int StandardDeviation; // среднеквадратичное отклонение


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Numbers = new List<double>();
            numericUpDown1.Maximum = 500;
            numericUpDown1.Minimum = 50;
            numericUpDown2.Minimum = 5;
            numericUpDown2.Maximum = numericUpDown1.Minimum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNumbers();
        }

        private void GenerateNumbers()  // Сгенерировать СВ методом усечения
        {
            numericUpDown2.Maximum = numericUpDown1.Value;
            Numbers.Clear();

            Random random = new Random();
            double x, N = 20;
            for (int i = 0; i < Selection; i++) // генерация СВ методом усечения
            {
                double sum = 0;
                for (int j = 0; j < N; j++)
                {
                    sum += random.NextDouble();
                }
                x = (sum - N / 2) / Math.Sqrt(N / 12);
                x = x * StandardDeviation + MathExpectation;
                Math.Round(x, 2);
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

            Numbers.Sort();

            int numsInColumn = Numbers.Count / Interval;
            for (int i = 0; i < Interval; i++)
            {
                double sum = 0;
                for (int j = 0; j < numsInColumn; j++)
                {
                    sum += Numbers[j + i * numsInColumn];
                }
                double x = sum / numsInColumn;
                double y = 1 / (StandardDeviation * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(x - MathExpectation, 2) / (2 * Math.Pow(StandardDeviation, 2)));
                x = Math.Round(x, 3);
                chart1.Series[0].Points.AddXY(x, y);
            }
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

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            MathExpectation = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            StandardDeviation = (int)numericUpDown4.Value;
        }
    }
}
