using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    class Settings
    {
        //determine width of circles within game
        public static int Width { get; set; }

        //determine width of circles within game
        public static int Height { get; set; }
        
        //determine how fast player character can move
        public static int Speed { get; set; }
        
        //total score of current game
        public static int Score { get; set; }
        
        //how many points will be added when a food object is 'eaten'
        public static int Points { get; set; }
        
        //decides if game is over; if true, game is over
        public static bool GameOver { get; set; }

        public static Direction direction { get; set; }


        public Settings()
        {
            Width = 16;
            Height = 16;
            Speed = 16;
            Score = 0;
            Points = 100;
            GameOver = false;
            direction = Direction.Down;
        }
    }
}
