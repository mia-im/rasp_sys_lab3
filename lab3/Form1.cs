using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab3
{
    public partial class Form1 : Form
    {
        private static ManualResetEvent _stopper = new ManualResetEvent(false);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int N = Convert.ToInt32(textBox2.Text); //переменная для количества вводимых строк

            //создание первого потока для добавления строк
            Thread thr1 = new Thread(() =>
            {
                for (int i=0; i<N; i++)
                {
                    richTextBox1.BeginInvoke(new Action(() =>
                    {
                        richTextBox1.Text += textBox1.Text+"\n";
                    }));
                    if (i==0) _stopper.Set(); //возобновление потока удаления после добавления одной строки
                }
            });

            //создание второго потока для удаления последней строки
            Thread thr2 = new Thread(() =>
            {
                _stopper.WaitOne(); //приостанавление потока для добавления строки
                richTextBox1.BeginInvoke(new Action(() => 
                {
                    string[] result = richTextBox1.Lines.Where((x, y) => y != richTextBox1.Lines.Length - 2).ToArray();
                    richTextBox1.Lines = result;
                }));
            });

            //определение приоритета первого потока
            string selectedPriority1 = comboBox1.SelectedItem.ToString();
            if (selectedPriority1 == "AboveNormal") { thr1.Priority = ThreadPriority.AboveNormal; }
            else if (selectedPriority1 == "BelowNormal") { thr1.Priority = ThreadPriority.BelowNormal; }
            else if (selectedPriority1 == "Highest") { thr1.Priority = ThreadPriority.Highest; }
            else if (selectedPriority1 == "Lowest") { thr1.Priority = ThreadPriority.Lowest; }
            else { thr1.Priority = ThreadPriority.Normal; }

            //определение приоритета второго потока
            string selectedPriority2 = comboBox2.SelectedItem.ToString();
            if (selectedPriority2 == "AboveNormal") { thr2.Priority = ThreadPriority.AboveNormal; }
            else if (selectedPriority2 == "BelowNormal") { thr2.Priority = ThreadPriority.BelowNormal; }
            else if (selectedPriority2 == "Highest") { thr2.Priority = ThreadPriority.Highest; }
            else if (selectedPriority2 == "Lowest") { thr2.Priority = ThreadPriority.Lowest; }
            else { thr2.Priority = ThreadPriority.Normal; }

            //запуск потоков
            thr1.Start();
            thr2.Start();
        }
    }
}