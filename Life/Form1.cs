using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Life
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows;
        private int columns;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value; // для изменения размеров точек
            rows = pictureBox1.Height / resolution;
            columns = pictureBox1.Width / resolution;
            field = new bool[columns, rows];
            
            Random random = new Random();

            for (int x = 0; x < columns; x++) // случайным образом генерируем первое поколение
            {
                for (int y = 0; y < rows; y++)
                {
                   field[x, y] = random.Next((int)nudDensity.Value) == 0;     // если сгенерированное число равно нулю то false и эта точка незакрашена
                }
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // создаем новую картинку(bitmap) с размерами pictureBox1
            graphics = Graphics.FromImage(pictureBox1.Image); // в обьект графика передаем image picturebox1
            timer1.Start();
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                   if (field[x, y])
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution); //если true то в заданных координатах рисуем квадрат
                    }    
                }
            }

            Random random = new Random();
            for (int x = 0; x < columns; x++) // случайным образом генерируем первое поколение
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;     // если сгенерированное число равно нулю то false и эта точка незакрашена
                }
            }
            pictureBox1.Refresh(); //перерисовка поля
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {

        }
    }
}
