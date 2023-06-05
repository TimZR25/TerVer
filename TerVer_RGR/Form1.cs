using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Windows.Forms;
using Chart = System.Windows.Forms.DataVisualization.Charting.Chart;
using Label = System.Windows.Forms.Label;

namespace TerVer_RGR
{
    public partial class Form1 : Form
    {
        private int Selection; // выборка
        private List<double> Numbers1; // СВ 1
        private List<double> Numbers2; // СВ 2
        private int Interval; // интервал
        private Random random;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Numbers1 = new List<double>();
            Numbers2 = new List<double>();
            random = new Random();
            numericUpDown1.Maximum = 10000;
            numericUpDown1.Minimum = 50;
            numericUpDown2.Minimum = 5;
            numericUpDown2.Maximum = numericUpDown1.Minimum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNumbers(Numbers1);
            GenerateNumbers(Numbers2);
            NumbersToFile(Numbers1, 1);
            NumbersToFile(Numbers2, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawHistogram(Numbers1, chart1, label3);
            DrawHistogram(Numbers2, chart2, label4);
        }

        private void GenerateNumbers(List<double> numbers)  // Сгенерировать СВ методом усечения
        {
            numbers.Clear();

            if (Selection <= 0) return;

            double x, y;
            for (int i = 0; i < Selection; i++) // генерация СВ методом усечения
            {
                do
                {
                    y = random.NextDouble();
                    x = random.NextDouble() * 2 + 1;
                } while (y > (1.5 - 0.5 * x));
                numbers.Add(x);
            }
        }

        private void NumbersToFile(List<double> numbers, int n)
        {
            if (Selection <= 0) return;

            string str = "";
            for (int i = 0; i < numbers.Count; i++)
            {
                str += numbers[i].ToString() + Environment.NewLine;
            }
            File.WriteAllText("Выборка" + "№" + n.ToString() + ".txt", str);
        }

        private void DrawHistogram(List<double> numbers, Chart chart, Label label) // Нарисовать гистограмму 
        {
            chart.Series[0].Points.Clear();

            if (numbers.Count == 0) return;

            if (numbers.Count < Interval) return;

            numbers.Sort();

            double min = numbers.Min();
            double max = numbers.Max();

            double intervalLength = (max - min) / Interval;

            int j = 0;
            double xi = 0;
            for (int i = 0; i < Interval; i++)
            {
                int numsInColumn = 0;
                double rightBorder = min + (i + 1) * intervalLength;

                for (; j < numbers.Count && numbers[j] <= rightBorder; j++)
                {
                    numsInColumn++;
                }


                //xi += Math.Pow(numsInColumn - numbers.Count / Interval, 2) / (numbers.Count / Interval);
                xi += Math.Pow(numsInColumn - numbers.Count * (rightBorder - min + i * intervalLength), 2) / (numbers.Count * (rightBorder - min + i * intervalLength));

                chart.Series[0].Points.AddXY(min + (i + 0.5) * intervalLength, numsInColumn / (numbers.Count * intervalLength));
            }
            label.Text = "χ^2: " + xi.ToString();
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
