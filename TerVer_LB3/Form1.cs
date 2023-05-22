using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TerVer_LB3
{
    public partial class Form1 : Form
    {
        private List<double> Numbers; // СВ
        private int Selection; // выборка
        private int MathExpectation; // мат ожидание
        private int StandardDeviation; // среднеквадратичное отклонение


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Numbers = new List<double>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNumbers();
            CalculateParameters();
            Microsoft.Office.Interop.Excel.Application _ex = new Microsoft.Office.Interop.Excel.Application();
            textBox1.Text = _ex.WorksheetFunction.ChiInv(1 - 0.95 / 2, Selection).ToString();
        }

        private void GenerateNumbers()  // Сгенерировать СВ методом усечения
        {
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

            int interval = (int)Math.Ceiling(1 + 3.322 * Math.Log10(Numbers.Count)); // интервал определенный по правилу Стѐрджеса

            int numsInColumn = Numbers.Count / interval; // кол-во чисел на один столбик

            for (int i = 0; i < interval; i++)
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

        private void CalculateParameters()
        {
            if (MathExpectation == 0 || StandardDeviation == 0) return;

            double min, max, temp, S;
            switch (Selection)
            {
                case 50:
                    // Мера надежности 0.95
                    min = Math.Round(Numbers.Average() - Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.95996, 3);
                    max = Math.Round(Numbers.Average() + Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.95996, 3);
                    textBox1.Text = "При известной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = Math.Sqrt(temp / (Selection - 1));
                    min = Math.Round(Numbers.Average() - S / Math.Sqrt(Selection) * 2.00957, 3);
                    max = Math.Round(Numbers.Average() + S / Math.Sqrt(Selection) * 2.00957, 3);
                    textBox2.Text = "При неизвестной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - MathExpectation, 2);
                    min = Math.Round(temp / 71.42019, 3);
                    max = Math.Round(temp / 32.35736, 3);
                    textBox3.Text = "При известном мат ожидании: " + min + " < D < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = temp / (Selection - 1);
                    min = Math.Round((Selection - 1) * S / 70.22241, 3);
                    max = Math.Round((Selection - 1) * S / 31.55491, 3);
                    textBox4.Text = "При неизвестном мат ожидании: " + min + " < D < " + max;



                    // Мера надежности 0.85
                    min = Math.Round(Numbers.Average() - Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.43953, 3);
                    max = Math.Round(Numbers.Average() + Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.43953, 3);
                    textBox8.Text = "При известной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = Math.Sqrt(temp / (Selection - 1));
                    min = Math.Round(Numbers.Average() - S / Math.Sqrt(Selection) * 1.46246, 3);
                    max = Math.Round(Numbers.Average() + S / Math.Sqrt(Selection) * 1.46246, 3);
                    textBox7.Text = "При неизвестной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - MathExpectation, 2);
                    min = Math.Round(temp / 65.03027, 3);
                    max = Math.Round(temp / 36.39710, 3);
                    textBox6.Text = "При известном мат ожидании: " + min + " < D < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = temp / (Selection - 1);
                    min = Math.Round((Selection - 1) * S / 63.88477, 3);
                    max = Math.Round((Selection - 1) * S / 35.54256, 3);
                    textBox5.Text = "При неизвестном мат ожидании: " + min + " < D < " + max;
                    break;
                case 500:
                    // Мера надежности 0.95
                    min = Math.Round(Numbers.Average() - Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.95996, 3);
                    max = Math.Round(Numbers.Average() + Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.95996, 3);
                    textBox1.Text = "При известной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = Math.Sqrt(temp / (Selection - 1));
                    min = Math.Round(Numbers.Average() - S / Math.Sqrt(Selection) * 1.96473, 3);
                    max = Math.Round(Numbers.Average() + S / Math.Sqrt(Selection) * 1.96473, 3);
                    textBox2.Text = "При неизвестной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - MathExpectation, 2);
                    min = Math.Round(temp / 563.85153, 3);
                    max = Math.Round(temp / 439.93599, 3);
                    textBox3.Text = "При известном мат ожидании: " + min + " < D < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = temp / (Selection - 1);
                    min = Math.Round((Selection - 1) * S / 562.78949, 3);
                    max = Math.Round((Selection - 1) * S / 438.99802, 3);
                    textBox4.Text = "При неизвестном мат ожидании: " + min + " < D < " + max;



                    // Мера надежности 0.85
                    min = Math.Round(Numbers.Average() - Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.43953, 3);
                    max = Math.Round(Numbers.Average() + Math.Sqrt(Math.Pow(StandardDeviation, 2) / Selection) * 1.43953, 3);
                    textBox8.Text = "При известной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = Math.Sqrt(temp / (Selection - 1));
                    min = Math.Round(Numbers.Average() - S / Math.Sqrt(Selection) * 1.44175, 3);
                    max = Math.Round(Numbers.Average() + S / Math.Sqrt(Selection) * 1.44175, 3);
                    textBox7.Text = "При неизвестной дисперсии: " + min + " < m < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - MathExpectation, 2);
                    min = Math.Round(temp / 546.21178, 3);
                    max = Math.Round(temp / 455.21766, 3);
                    textBox6.Text = "При известном мат ожидании: " + min + " < D < " + max;

                    temp = 0;
                    for (int i = 0; i < Selection; i++) temp += Math.Pow(Numbers[i] - Numbers.Average(), 2);
                    S = temp / (Selection - 1);
                    min = Math.Round((Selection - 1) * S / 545.16621, 3);
                    max = Math.Round((Selection - 1) * S / 454.26323, 3);
                    textBox5.Text = "При неизвестном мат ожидании: " + min + " < D < " + max;
                    break;
                default:
                    break;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            MathExpectation = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            StandardDeviation = (int)numericUpDown4.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Selection = 50;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Selection = 500;
        }
    }
}
