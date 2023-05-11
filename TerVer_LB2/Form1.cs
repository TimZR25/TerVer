using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TerVer_LB2
{
    public partial class Form1 : Form
    {
        private int Selection; // выборка
        private List<double> Numbers; // СВ
        private int Interval; // интервал

        private double _average;
        public double Average   // среднее
        {
            get => _average;
            set
            {
                _average = Math.Round(value, 2);
                textBox2.Text = _average.ToString();
            }
        }

        private double _mode;
        public double Mode  // мода
        {
            get => _mode;
            set
            {
                _mode = Math.Round(value, 2);
                textBox3.Text = _mode.ToString();
            }
        }

        private double _median;
        public double Median    // медиана
        {
            get => _median;
            set
            {
                _median = Math.Round(value, 2);
                textBox4.Text = _median.ToString();
            }
        }

        private double _sampleVariance;
        public double SampleVariance    //выборочная дисперсия
        {
            get => _sampleVariance;
            set
            {
                _sampleVariance = Math.Round(value, 2);
                textBox5.Text = _sampleVariance.ToString();
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Numbers = new List<double>();
            numericUpDown1.Maximum = 10000;
            numericUpDown1.Minimum = 100;
            numericUpDown2.Minimum = 5;
            numericUpDown2.Maximum = numericUpDown1.Minimum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNumbers();
            CalculateParameters();
        }

        private void GenerateNumbers()  // Сгенерировать СВ методом усечения
        {
            numericUpDown2.Maximum = numericUpDown1.Value;
            Numbers.Clear();

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
                double y = 1.5 - 0.5 * x;
                x = Math.Round(x, 3);
                chart1.Series[0].Points.AddXY(x, y);
            }
        }

        private void CalculateParameters() // Находит параметры СВ
        {
            Average = Numbers.Sum() / Numbers.Count; // среднее


            Median = Numbers.Count % 2 == 0 ?
                (Numbers[Numbers.Count / 2] + Numbers[Numbers.Count / 2 - 1]) / 2
                : Numbers[Numbers.Count / 2]; // медиана


            List<double> numbers = Numbers;
            numbers.Sort();
            double num = numbers[0];
            double maxNum = num;
            int maxCount = 0;
            int count = 0;
            for (int i = 0; i < numbers.Count; i += count + 1)
            {
                count = 0;
                num = numbers[i];
                for (int j = i; j < numbers.Count && num == numbers[j]; j++)
                    count++;

                if (count > maxCount)
                {
                    maxCount = count;
                    maxNum = num;
                }
            }
            Mode = maxNum; // мода


            double sum = 0;
            for (int i = 0; i < Numbers.Count; i++)
            {
                sum += Math.Pow(Numbers[i] - Average, 2);
            }
            SampleVariance = sum / Numbers.Count; //выборочная дисперсия

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
