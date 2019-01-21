/*
    Simple Snake game made in C#
    I used some WindowsApi functions to make this game, so is very easy to make it in C++.
    Have some things can be better in this code,  but I probably I'll leave it like this.
    If you want to modify this code, feel free to do it.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WinApiConsole;
using System.Threading;

namespace SnakeGame
{
    class Game
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };
    
        public enum SnakeStatus
        {
            Alive,
            Death
        };

        private struct SnakeAnimationHandle
        {
            public Direction SnakeDirection { get; private set; }

            public int SnakeSize { get; set; }

            public int[] SnakePos { get; set; }

            static public int[] ShiftPos(int[] ArrayToShift, int Lenght)
            {
                int[] buffer = new int[ArrayToShift.Length];
                for (int x = 0; x < Lenght; x++)
                {
                    buffer[x + 1] = ArrayToShift[x];
                }
                buffer[Lenght + 1] = 0;
                return buffer;
            }

            public void ChangeDirection(Direction MoveDirection)
            {
                switch (MoveDirection)
                {
                    case Direction.Right:
                        if (SnakeDirection == Direction.Left)
                            return;
                        break;
                    case Direction.Left:
                        if (SnakeDirection == Direction.Right)
                            return;
                        break;
                    case Direction.Up:
                        if (SnakeDirection == Direction.Down)
                            return;
                        break;
                    case Direction.Down:
                        if (SnakeDirection == Direction.Up)
                            return;
                        break;
                }
                SnakeDirection = MoveDirection;
            }

            public SnakeAnimationHandle(int SnakeSize, int SnakeMaxSize, int InitialPos, Direction InitialDirection)
            {
                // Check Values
                if (SnakeSize > SnakeMaxSize)
                    throw new ArgumentException("SnakeSize can't be bigger than SnakeMaxSize", "SnakeSize");

                SnakePos = new int[SnakeMaxSize];
                this.SnakeSize = SnakeSize;
                SnakeDirection = InitialDirection;

                // InitialDirection Not Implemented
                switch (InitialDirection)
                {
                    case Direction.Right:
                        for (int x = 0, y = InitialPos; x <= SnakeSize; x++, y--)
                        {
                            SnakePos[x] = y;
                        }
                        break;
                    default:
                        throw new Exception("The game support only the initial direction as Right");
                }
            }
        }

        // Properties
        public int Score { get { return Snake.SnakeSize - SnakeStartLen; } }
        public Direction SnakeDirection { get { return Snake.SnakeDirection; } }
        public SnakeStatus Snake_Status { get; private set; }
        
        private Size ScreenSize { get; }
        private char[] ScreenBuffer { get; set; }
        
        // Variables
        private SnakeAnimationHandle Snake;
        private int FoodCount = 0;

        // Game Const
        private const int MaxFood = 7;
        private const char SnakeBody  = (char)178;
        private const char FoodChar = (char)64;
        private const int SnakeStartLen = 3;
        private const Direction StartDirection = Direction.Right;


        public Game(ref char[] ScreenBuffer, Size ScreenSize)
        {
            this.Snake_Status = SnakeStatus.Alive;
            this.ScreenSize = ScreenSize;
            this.ScreenBuffer = ScreenBuffer;
            int StartPos = ScreenSize.Width * (ScreenSize.Height / 2) - ScreenSize.Width / 2;
            this.Snake = new SnakeAnimationHandle(SnakeStartLen, ScreenBuffer.Length , StartPos, StartDirection);
        }
        
        private void UpdateDirection()
        {
            // Arrow Map
            if (_Console.GetAsyncKeyState(Buttons.VK_UP) != 0)
                Snake.ChangeDirection(Game.Direction.Up);
            else if (_Console.GetAsyncKeyState(Buttons.VK_DOWN) != 0)
                Snake.ChangeDirection(Game.Direction.Down);
            else if (_Console.GetAsyncKeyState(Buttons.VK_LEFT) != 0)
                Snake.ChangeDirection(Game.Direction.Left);
            else if (_Console.GetAsyncKeyState(Buttons.VK_RIGHT) != 0)
               Snake.ChangeDirection(Game.Direction.Right);
        }

        /// <summary>
        /// Update the game and write in the buffer
        /// </summary>
        public void UpdateGame()
        {
            UpdateDirection();

            // New Head Position
            int SnakeHead = Snake.SnakePos[0];
            int size = ScreenSize.Height * ScreenSize.Width;  // Compute the value a unique time
            switch (Snake.SnakeDirection)
            {

                case Direction.Right:
                    if (SnakeHead % ScreenSize.Width == ScreenSize.Width - 1)  // Check if the snake hits the Screen Corner                   
                        SnakeHead -= (ScreenSize.Width - 1);  // Set To the Left Side
                    else
                        SnakeHead++;  // Move to right
                    break;

                case Direction.Left:
                    if (SnakeHead % ScreenSize.Width == 0)  // Check if the snake hits the Screen Corner 
                        SnakeHead += (ScreenSize.Width - 1);  // Set To the Right Side
                    else
                        SnakeHead--;  // Move to Left
                    break;

                case Direction.Up:
                    if (SnakeHead >= 0 && SnakeHead <= ScreenSize.Width)  // Check if the snake hits the Screen Corner 
                        SnakeHead = (size - ScreenSize.Width) + SnakeHead % ScreenSize.Width;  // Set Snake to botton
                    else
                        SnakeHead -= (ScreenSize.Width);  // Move Up
                    break;

                case Direction.Down:
                    if (SnakeHead >= size - ScreenSize.Width && SnakeHead <= size)  // Check if the snake hits the Screen Corner 
                        SnakeHead = SnakeHead % ScreenSize.Width;  // Set Snake to Top
                    else
                        SnakeHead += (ScreenSize.Width);  // Move Down
                    break;
            }

            // Draw Food
            Random food = new Random();
            while(FoodCount < MaxFood)
            {
                int rnd = food.Next(0, ScreenBuffer.Length);
                if (ScreenBuffer[rnd] != SnakeBody)
                {
                    ScreenBuffer[rnd] = FoodChar;
                    FoodCount++;
                }
            }

            // Check Food
            if (ScreenBuffer[SnakeHead] == FoodChar)
            {
                Snake.SnakeSize++;
                FoodCount--;
            }

            // Check Self Hit
            if(ScreenBuffer[SnakeHead] == SnakeBody)
                this.Snake_Status = SnakeStatus.Death;

            // Draw the new Head
            ScreenBuffer[SnakeHead] = SnakeBody;
            ScreenBuffer[Snake.SnakePos[Snake.SnakeSize]] = '\0';
            
            // Update the Snake Position
            Snake.SnakePos = SnakeAnimationHandle.ShiftPos(Snake.SnakePos, Snake.SnakeSize);
            Snake.SnakePos[0] = SnakeHead;

        }

    }

    class Program
    {
        // Windown
        private static int nWidht = 120;
        private static int nHeight = 40;
        private static char[] ScreenBuffer = new char[nHeight * nWidht];
        private static IntPtr wHandle;

        //Game
        private static Game SnakeGame;

        static void Main(string[] args)
        {
            Console.Title = "SnakeGame :: Guila767";
            Console.SetWindowSize(nWidht, nHeight);
            Console.ForegroundColor = ConsoleColor.Green;
            wHandle = _Console.CreateConsoleScreenBuffer(_Console.GENERIC_READ | _Console.GENERIC_WRITE, 0, IntPtr.Zero, _Console.CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);            
            _Console.SetConsoleActiveScreenBuffer(wHandle);
            uint BytesWritten;

            // Game initialize
            SnakeGame = new Game(ref ScreenBuffer, new Size(nWidht, nHeight));

            // Game Loop
            while(true)
            {
                // Check The Console Size
                if (_Console.GetAsyncKeyState((int)ConsoleKey.M) != 0)
                    Console.SetWindowSize(nWidht, nHeight);  // Doesn't work

                // Update The Game and ScreenBuffer
                SnakeGame.UpdateGame();

                // Check if the Snake Die
                if(SnakeGame.Snake_Status == Game.SnakeStatus.Death)
                {
                    ScreenBuffer = new char[nHeight * nWidht];
                    ScreenBuffer = _Console.WriteText(ScreenBuffer, (nWidht * ((nHeight / 2)-1) - (nWidht/2))- (Properties.Resources.Lose1.Length / 2), Properties.Resources.Lose1);
                    ScreenBuffer = _Console.WriteText(ScreenBuffer, (nWidht * (nHeight / 2) - (nWidht / 2)) - ((Properties.Resources.Lose2.Length + SnakeGame.Score.ToString().Length + 1) / 2), Properties.Resources.Lose2 + $" {SnakeGame.Score}");
                    _Console.WriteConsoleOutputCharacter(wHandle, ScreenBuffer, (UInt32)ScreenBuffer.Length, new COORD(0, 0), out BytesWritten);
                    while (_Console.GetAsyncKeyState(Buttons.VK_RETURN) == 0) ;
                    ScreenBuffer = new char[nHeight * nWidht];
                    SnakeGame = new Game(ref ScreenBuffer, new Size(nWidht, nHeight));
                    continue;
                }

                // Easy way to dalay the game, probaly not the best one
                if (SnakeGame.SnakeDirection == Game.Direction.Right || SnakeGame.SnakeDirection == Game.Direction.Left)
                    Thread.Sleep(50);
                else
                    Thread.Sleep(66); // 66.64ms

                // Write The buffer in the screen
                _Console.WriteConsoleOutputCharacter(wHandle, ScreenBuffer, (UInt32)ScreenBuffer.Length, new COORD(0, 0), out BytesWritten);
            }
        }
    }
}
