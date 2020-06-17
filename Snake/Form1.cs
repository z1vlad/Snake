using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private int rI, rJ; //переменные для рандомизирования координат
        private PictureBox fruit; //переменная фрукта которую ест змейка (фрукт)
        private PictureBox[] snake = new PictureBox[400]; //переменная змейки
        private Label labelScore; //переменная рекорда
        private int dirX, dirY; //переменнаяя отвечающая за движения по Х и по Y
        private int _width = 900;  //переменная ширины (чтобы каждый раз не вводить)
        private int _height = 800;  //переменная длины (чтобы каждый раз не вводить)
        private int _sizeOfSides = 40; //переменная размера
        private int score = 0; //переменная движение элементов
        public Form1()
        {
            InitializeComponent();
            this.Text = "ЗМЕЙКА";
            this.Width = 900; //размер окна по ширине с игрой
            this.Height = 880; //размер окна по длине с игрой
            dirX = 1;
            dirY = 0;
            labelScore = new Label();
            labelScore.Text = "Score: 0";
            labelScore.Location = new Point(810, 10);
            this.Controls.Add(labelScore);
            snake[0] = new PictureBox();
            snake[0].Location = new Point(201, 201); 
            snake[0].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            snake[0].BackColor = Color.ForestGreen;
            this.Controls.Add(snake[0]);
            fruit = new PictureBox();
            fruit.BackColor = Color.Red;
            fruit.Size = new Size(_sizeOfSides, _sizeOfSides);
            _generateMap();
            _generateFruit();
            timer.Tick += new EventHandler(_update);
            timer.Interval = 200;
            timer.Start();
            this.KeyDown += new KeyEventHandler(OKP);
        }

        private void _generateFruit() // генерация фрукта по карте (рандомно)
        {
            Random r = new Random(); 
            rI = r.Next(0, _height - _sizeOfSides);
            int tempI = rI % _sizeOfSides;
            rI -= tempI;
            rJ = r.Next(0, _height - _sizeOfSides);
            int tempJ = rJ % _sizeOfSides; // важная страка отвечающая за то чтобы фрукт генерировался только на карте
            rJ -= tempJ;
            rI++;
            rJ++;
            fruit.Location = new Point(rI, rJ); //размер фрукта 
            this.Controls.Add(fruit);
        }

        private void _checkBorders() //
        {
            if (snake[0].Location.X < 0)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = 1;
            }
            if (snake[0].Location.X > _height)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirX = -1;
            }
            if (snake[0].Location.Y < 0)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > _height)
            {
                for (int _i = 1; _i <= score; _i++)
                {
                    this.Controls.Remove(snake[_i]);
                }
                score = 0;
                labelScore.Text = "Score: " + score;
                dirY = -1;
            }
        }

        private void _eatItself() //
        {
            for (int _i = 1; _i < score; _i++)
            {
                if (snake[0].Location == snake[_i].Location)
                {
                    for (int _j = _i; _j <= score; _j++)
                        this.Controls.Remove(snake[_j]);
                    score = score - (score - _i + 1);
                    labelScore.Text = "Score: " + score;
                }
            }
        }

        private void _eatFruit() //
        {
            if (snake[0].Location.X == rI && snake[0].Location.Y == rJ)
            {
                labelScore.Text = "Score: " + ++score;
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 40 * dirX, snake[score - 1].Location.Y - 40 * dirY);
                snake[score].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
                snake[score].BackColor = Color.Green;
                this.Controls.Add(snake[score]);
                _generateFruit();
            }
        }

        private void _generateMap() //генерация клеточек при помощи пик. боксов (палочек)
        {
            for (int i = 0; i < _width / _sizeOfSides; i++)  // линии по горизонтале 
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, _sizeOfSides * i);
                pic.Size = new Size(_width - 100, 1); //расположение и размер
                this.Controls.Add(pic);
            }
            for (int i = 0; i <= _height / _sizeOfSides; i++) // линии по вертикале
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(_sizeOfSides * i, 0);
                pic.Size = new Size(1, _width ); //расположение и размер
                this.Controls.Add(pic);
            }
        }

        private void _moveSnake() //
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point(snake[0].Location.X + dirX * (_sizeOfSides), snake[0].Location.Y + dirY * (_sizeOfSides));
            _eatItself();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void _update(Object myObject, EventArgs eventsArgs)
        {
            _checkBorders();
            _eatFruit();
            _moveSnake();
            //cube.Location = new Point(cube.Location.X + dirX * _sizeOfSides, cube.Location.Y + dirY * _sizeOfSides);
        }

        private void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString()) //управление стрелочками
            {
                case "Right": //вправо
                    dirX = 1;
                    dirY = 0; //обнуление направления в обработчике клавиш, чтобы змейка не двигалась по диогонали  
                    break;
                case "Left": //влево
                    dirX = -1;
                    dirY = 0;
                    break;
                case "Up": //вверх
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Down": //вниз
                    dirY = 1;
                    dirX = 0;
                    break;
            }
        }
    }
}
