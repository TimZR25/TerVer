using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
            DrawHistogram();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (Selection)
            {
                case 50:
                    NumbersToFile("50.txt");
                    break;
                case 500:
                    NumbersToFile("500.txt");
                    break;
                default:
                    break;
            }
        }

        private void GenerateNumbers()  // Сгенерировать СВ
        {
            if (Selection <= 0 || StandardDeviation <= 0 || MathExpectation <= 0) return;

            Numbers.Clear();

            Random random = new Random();
            double x, N = 20;
            for (int i = 0; i < Selection; i++) // генерация СВ
            {
                double sum = 0;
                for (int j = 0; j < N; j++)
                {
                    sum += random.NextDouble();
                }
                x = (sum - N / 2) / Math.Sqrt(N / 12);
                x = x * StandardDeviation + MathExpectation;
                Numbers.Add(x);
            }
        }

        private void NumbersToFile(string path)
        {
            string str = "";
            for (int i = 0; i < Numbers.Count; i++)
            {
                str += Numbers[i].ToString() + Environment.NewLine;
            }
            File.WriteAllText(path, str);
        }

        private void DrawHistogram() // Нарисовать гистограмму 
        {
            if (Numbers.Count == 0) return;

            chart1.Series[0].Points.Clear();

            List<double> numbers = new List<double>();
            foreach (double item in Numbers)
            {
                numbers.Add(item);
            }
            numbers.Sort();


            int interval = (int)Math.Ceiling(1 + 3.322 * Math.Log10(numbers.Count)); // интервал определенный по правилу Стѐрджеса

            double min = numbers.Min();
            double max = numbers.Max();

            double intervalLength = (max - min) / interval;

            int j = 0;
            for (int i = 0; i < interval; i++)
            {
                int numsInColumn = 0;
                double rightBorder = min + (i + 1) * intervalLength;

                for (; j < numbers.Count && numbers[j] <= rightBorder; j++)
                {
                    numsInColumn++;
                }

                chart1.Series[0].Points.AddXY(min + (i + 0.5) * intervalLength, numsInColumn / (numbers.Count * intervalLength));
            }
        }

        private void CalculateParameters()
        {
            if (MathExpectation == 0 || StandardDeviation == 0) return;

            double min, max, S, delta, gamma, chi1, chi2;
            Microsoft.Office.Interop.Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();


            gamma = 0.95; // Мера надежности 0.95
            //---------------Доверительный интервал для мат ожидания---------------
            delta = ex.WorksheetFunction.NormSInv(gamma / 2 + 0.5) * StandardDeviation / Math.Sqrt(Selection);
            min = Numbers.Average() - delta;
            max = Numbers.Average() + delta;
            textBox1.Text = "Мат ожидание при известной дисперсии:" + Environment.NewLine + min + " < m < " + max;

            S = Math.Pow(StandardDeviation, 2) * (Selection / (Selection - 1)); // выборочная дисперсия
            delta = ex.WorksheetFunction.TInv(1 - gamma, Selection - 1) * Math.Sqrt(S / Selection);
            min = Numbers.Average() - delta;
            max = Numbers.Average() + delta;
            textBox2.Text = "Мат ожидание при неизвестной дисперсии:" + Environment.NewLine + min + " < m < " + max;
            

            //---------------Доверительный интервал для дисперсии---------------
            chi1 = ex.WorksheetFunction.ChiInv((1 - gamma) / 2, Selection);
            chi2 = ex.WorksheetFunction.ChiInv(1 - (1 - gamma) / 2, Selection);
            min = Selection * Math.Pow(StandardDeviation, 2) / chi1;
            max = Selection * Math.Pow(StandardDeviation, 2) / chi2;
            textBox3.Text = "Дисперсия при известном мат ожидании:" + Environment.NewLine + min + " < D < " + max;

            S = Math.Pow(StandardDeviation, 2) * (Selection / (Selection - 1)); // выборочная дисперсия
            chi1 = ex.WorksheetFunction.ChiInv((1 - gamma) / 2, Selection - 1);
            chi2 = ex.WorksheetFunction.ChiInv(1 - (1 - gamma) / 2, Selection - 1);
            min = (Selection - 1) * S / chi1;
            max = (Selection - 1) * S / chi2;
            textBox4.Text = "Дисперсия при неизвестном мат ожидании:" + Environment.NewLine + min + " < D < " + max;



            gamma = 0.85; // Мера надежности 0.85
            //---------------Доверительный интервал для мат ожидания---------------
            delta = ex.WorksheetFunction.NormSInv(gamma / 2 + 0.5) * StandardDeviation / Math.Sqrt(Selection);
            min = Numbers.Average() - delta;
            max =Numbers.Average() + delta;
            textBox8.Text = "Мат ожидание при известной дисперсии:" + Environment.NewLine + min + " < m < " + max;

            S = Math.Pow(StandardDeviation, 2) * (Selection / (Selection - 1)); // выборочная дисперсия
            delta = ex.WorksheetFunction.TInv(1 - gamma, Selection - 1) * Math.Sqrt(S / Selection);
            min = Numbers.Average() - delta;
            max = Numbers.Average() + delta;
            textBox7.Text = "Мат ожидание при неизвестной дисперсии:" + Environment.NewLine + min + " < m < " + max;


            //---------------Доверительный интервал для дисперсии---------------
            chi1 = ex.WorksheetFunction.ChiInv((1 - gamma) / 2, Selection);
            chi2 = ex.WorksheetFunction.ChiInv(1 - (1 - gamma) / 2, Selection);
            min = Selection * Math.Pow(StandardDeviation, 2) / chi1;
            max = Selection * Math.Pow(StandardDeviation, 2) / chi2;
            textBox6.Text = "Дисперсия при известном мат ожидании:" + Environment.NewLine + min + " < D < " + max;

            S = Math.Pow(StandardDeviation, 2) * (Selection / (Selection - 1)); // выборочная дисперсия
            chi1 = ex.WorksheetFunction.ChiInv((1 - gamma) / 2, Selection - 1);
            chi2 = ex.WorksheetFunction.ChiInv(1 - (1 - gamma) / 2, Selection - 1);
            min = (Selection - 1) * S / chi1;
            max = (Selection - 1) * S / chi2;
            textBox5.Text = "Дисперсия при неизвестном мат ожидании:" + Environment.NewLine + min + " < D < " + max;
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
