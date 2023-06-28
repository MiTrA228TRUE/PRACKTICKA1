using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // задаем в массивы x и y соответсвующие значения 
        double[] x = { -2.0, -1.4, -0.8, -0.2, 0.4, 1.0, 1.6, 2.2, 2.8, 3.4, 4.0, 4.6, 5.2, 5.8, 6.4 };
        double[] y = { 10.21, 9.35, 8.67, 6.12, 4.95, 3.64, 2.11, 1.05, -0.53, -1.89, -2.47, -3.28, -4.75, -6.04, -7.41 };
        public Form1()
        {
            InitializeComponent();
            // во время запуска программы на графике chart1 отмечает точки с координатами  из массивов x и y 
            for (int i = 0; i < x.Length; i++)
            {
                chart1.Series[0].Points.Add(new DataPoint(x[i], y[i]));
            }
        }
        // создаем событие для ввода данных tbX, позволяющее вводить в поле лишь цифры,знак минуса и запятую
        private void tb_number_start_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number1 = e.KeyChar;
            if (!Char.IsDigit(number1) && number1 != 8 && number1 != 45 && number1 != ',')
                e.Handled = true;     
        }
        // метод полинома Лагранжа для вводимого пользователем xValue
        public static double LagrangePolynomial(double[] x, double[] y, double xValue)
        {
            double result = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double term = y[i];
                for (int j = 0; j < y.Length; j++)
                {
                    if (i != j)
                    {
                        // при условии i!=j вычисляем Li(xValue) = ∏(xValue - xj) / (xi - xj) для каждой точки 
                        term *= (xValue - x[j]) / (x[i] - x[j]);
                    }
                }
                //Вычисляем значения полинома Лагранжа в точке xValue по формуле: L(xValue) = ∑(yj * Li(xValue))
                result += term;
            }
            // Возвращаем значение функции в точке xValue, полученное с помощью интерполяции
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // создаем еще одну проверку ввода xValue в поле tbX
            // falsch истина если строка не пуста
            bool falsch = !string.IsNullOrEmpty(tbX.Text);
            // если falsch истина, проверяем позицию запятой, если на первой позиции возвращаем ложь
            if (falsch == true)
                if (tbX.Text[0] == ',')
                    falsch = false;
            // проходим по всем символам строки, начиная со второго символа
            for (int i = 1; i < tbX.Text.Length; i++)
            {
                // если находим знак минус возвращаем ложь
                if (tbX.Text[i] == '-')
                   falsch = false;
                // если находим запятую, перед которой не цифра, возвращаем ложб
                if ((tbX.Text[i] == ',' && !char.IsDigit(tbX.Text[i-1])))
                    falsch = false;
            }
            // если falsch истина, то выполняем код, иначе вывод ошибки в окне MessageBox
            if (falsch == true)
            {
                // конвертируем в тип Double значение строки поля tbX
                double xValue = Double.Parse(tbX.Text);
                //в result присваеваем значение функции в точке xValue, полученное с помощью интерполяции
                double result = LagrangePolynomial(x, y, xValue);
                // очищаем поле tbY и передаем значение result
                tbY.Clear();
                tbY.Text +=
                    String.Format("{0}" + Environment.NewLine, result);
            }
            else 
            {
                MessageBox.Show("ОШИБКА ВВОДА Х!",
                               "ВНИМАНИЕ!",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Asterisk,
                               MessageBoxDefaultButton.Button1);
            }
        } 
    }
}
