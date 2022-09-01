using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        private int currentGeneration;
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

            currentGeneration = 0;
            Text = $"Generation {currentGeneration}";
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

            var newField = new bool[columns, rows];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neigboursCount = CountNeibours(x, y);

                    var hasLife = field[x, y]; // если в точке true то и haslife будет равно true

                    if (!hasLife && neigboursCount == 3)
                    {
                        newField[x, y] = true; // заполняем клетку
                    }
                    else if(hasLife && neigboursCount < 2 || neigboursCount > 3)
                    {
                        newField[x, y] = false; // убираем
                    }
                    else
                    {
                        newField[x, y] = field[x, y];  // оставляем клетку как было
                    }
                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution); //если true то в заданных координатах рисуем квадрат
                    }    
                }
            }

            field = newField;
            
            pictureBox1.Refresh(); //перерисовка поля
            Text = $"Generation {++currentGeneration}";
        }

        private int CountNeibours(int x, int y) // количество ближайших соседей
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + columns) % columns; //остаток это возможность посмотреть на другой край карты что там за значение
                    int row = (y + j + rows) % rows;
                    bool isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row];
                    if (hasLife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
            {
                return;
            }
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
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
            StopGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMousePosition(x, y);

                if (validationPassed)
                    field[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMousePosition(x, y);

                if (validationPassed)
                    field[x, y] = false;
            }
        }

        private bool ValidateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < columns && y < rows; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation {currentGeneration}";
        }
    }
}
