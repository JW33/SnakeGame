using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            //set Settings to default
            new Settings();

            // set game speed and start timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //start new game
            StartGame();
        }

        private void StartGame()
        {
            //reset for each new game
            lblGameOver.Visible = false;

            //set Settings to default
            new Settings();

            //create new player object (snake)
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        //place random food object
        private void GenerateFood()
        {
            int maxXPosition = pbCanvas.Size.Width / Settings.Width;
            int maxYPosition = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();

            food = new Circle();

            food.X = random.Next(0, maxXPosition);
            food.Y = random.Next(0, maxYPosition);
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //check for game over
            if (Settings.GameOver == true)
            {
                //check if 'Enter' is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }

                MovePlayer();
            }

            //refreshes pbCanvas in real time
            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (Settings.GameOver != true) //or == false
            {
                //set color of snake
                Brush snakeColor;

                //draw snake
                for( int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        //draw snake head
                        snakeColor = Brushes.Black;
                    }
                    else
                    {
                        //draw snake body (everything but the head)
                        snakeColor = Brushes.Green;
                    }

                    //draw snake
                    canvas.FillEllipse(snakeColor, 
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //draw food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                                      food.Y * Settings.Height,
                                      Settings.Width,
                                      Settings.Height));
                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress enter to try again.";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            //i = current circle
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //move snake head
                if (i == 0)
                {
                    if (Settings.direction == Direction.Right)
                    {
                        Snake[i].X++;
                    }
                    else if (Settings.direction == Direction.Left)
                    {
                        Snake[i].X--;
                    }
                    else if (Settings.direction == Direction.Up)
                    {
                        Snake[i].Y--;
                    }
                    else if (Settings.direction == Direction.Down)
                    {
                        Snake[i].Y++;
                    }
                    
                    //switch (Settings.direction)
                    //{
                    //    case Direction.Right:
                    //        Snake[i].X++;
                    //        break;
                    //    case Direction.Left:
                    //        Snake[i].X--;
                    //        break;
                    //    case Direction.Up:
                    //        Snake[i].Y--;
                    //        break;
                    //    case Direction.Down:
                    //        Snake[i].Y++;
                    //        break;
                    //}

                    //get maximum X and Y positions
                    int maxXPosition = pbCanvas.Size.Width / Settings.Width;
                    int maxYPosition = pbCanvas.Size.Height / Settings.Height;

                    //detect collision with game borders
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPosition || Snake[i].Y >= maxXPosition)
                    {
                        Die();
                    }

                    //detect collision with body
                    for (int j = 1; j <Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Grow();
                    }
                }
                else
                {
                    //move snake body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Grow()
        {
            //add circle to body
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;

            Snake.Add(food);

            //update score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangedState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangedState(e.KeyCode, false);
        }
    }
}
