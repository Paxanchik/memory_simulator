using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace memory_simulator
{
    public partial class Form1 : Form
    {
        private int numTests;
        private int numDigits;
        private int timeInterval;
        private int currentTest;
        private int correctAnswers;
        private int incorrectAnswers;
        private string targetNumber;
        private string userNumber;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Обработчик события нажатия на кнопку "Старт"
            numTests = (int)numUpDownTests.Value;
            numDigits = (int)numUpDownDigits.Value;
            timeInterval = (int)numUpDownInterval.Value;
            ClearResults();
            currentTest = 0;
            correctAnswers = 0;
            incorrectAnswers = 0;
            progressBarAttempts.Maximum = numTests;
            progressBarCorrect.Maximum = numTests;

            timer1.Interval = timeInterval * 1000; // Преобразуем интервал в миллисекунды
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Задаем значения по умолчанию
            numTests = 5;
            numDigits = 5;
            timeInterval = 5;
            currentTest = 0;
            correctAnswers = 0;
            incorrectAnswers = 0;

            // Запускаем датчик и очищаем компоненты Label и Edit
            GenerateRandomNumber();
            ClearResults();
        }
        private void GenerateRandomNumber()
        {
            // Генерируем случайное число заданного количества цифр
            Random random = new Random();
            targetNumber = "";
            for (int i = 0; i < numDigits; i++)
            {
                targetNumber += random.Next(0, 9).ToString();
            }
        }
        private void ClearResults() 
        {
            // Очищаем результаты предыдущего теста
            lblNumber.Text = "";
            userNumber = "";
            progressBarAttempts.Value = 0;
            progressBarCorrect.Value = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Обработчик события таймера, отображающий случайное число в Label
            timer1.Interval = timeInterval * 1000;
            lblNumber.Text = targetNumber;
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Обработчик события нажатия на кнопку "Отправить"
            userNumber = tbInput.Text.Trim();

            if (userNumber == targetNumber)
            {
                correctAnswers++;
            }
            else
            {
                incorrectAnswers++;
            }

            currentTest++;
            progressBarAttempts.Value = currentTest;
            progressBarCorrect.Value = correctAnswers;
            progressBarAttempts.Value = 2;
            ClearResults();
            tbInput.Text = "";

            if (currentTest >= numTests)
            {
                // Тестирование завершено
                double accuracy = (double)correctAnswers / numTests * 100;
                string username = tbUsername.Text.Trim();
                SaveResultsToFile(username, accuracy);

                MessageBox.Show($"Тестирование завершено!\n\nИмя: {username}\nПроцент качества памяти: {accuracy.ToString("0.00")}%");

                // Сбрасываем значения по умолчанию
                numTests = 5;
                numDigits = 3;
                timeInterval = 5;

                // Очищаем компоненты Label и Edit
                lblNumber.Text = "";
                tbInput.Text = "";
                tbUsername.Text = "";
            }
            else
            {
                GenerateRandomNumber();
            }
        }
        private void SaveResultsToFile(string username, double accuracy)
        {
            // Сохраняем результаты тестирования в файл
            string fileName = "MemoryTestResults.txt";
            using (StreamWriter writer = File.AppendText(fileName))
            {
                writer.WriteLine($"Имя: {username}");
                writer.WriteLine($"Процент качества памяти: {accuracy.ToString("0.00")}%");
                writer.WriteLine();
            }
        }
    }
}
