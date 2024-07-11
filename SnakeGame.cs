using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class SnakeGame
    {
        private const int InitialScore = 5;
        private const int GameOverFlag = 1;
        private int _screenWidth;
        private int _screenHeight;
        private int _score;
        private bool _isGameOver;
        private Pixel _head;
        private string _movementDirection;
        private List<int> _bodyXPositions;
        private List<int> _bodyYPositions;
        private int _berryXPosition;
        private int _berryYPosition;
        private Random _randomNumberGenerator;
        private DateTime _startTime;
        private DateTime _currentTime;
        private string _buttonPressed;

        public void StartGame()
        {
            InitializeGame();
            GameLoop();
            EndGame();
        }

        private void InitializeGame()
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            _screenWidth = Console.WindowWidth;
            _screenHeight = Console.WindowHeight;
            _randomNumberGenerator = new Random();
            _score = InitialScore;
            _isGameOver = false;

            _head = new Pixel
            {
                XPosition = _screenWidth / 2,
                YPosition = _screenHeight / 2,
                PixelColor = ConsoleColor.Red
            };

            _movementDirection = "RIGHT";
            _bodyXPositions = new List<int>();
            _bodyYPositions = new List<int>();
            _berryXPosition = _randomNumberGenerator.Next(1, _screenWidth - 2);
            _berryYPosition = _randomNumberGenerator.Next(1, _screenHeight - 2);
        }

        private void GameLoop()
        {
            while (!_isGameOver)
            {
                Console.Clear();
                CheckForCollisions();
                DrawGameField();
                UpdateSnakePosition();
                HandleUserInput();
            }
        }

        private void CheckForCollisions()
        {
            if (_head.XPosition == _screenWidth - 1 || _head.XPosition == 0 ||
                _head.YPosition == _screenHeight - 1 || _head.YPosition == 0)
            {
                _isGameOver = true;
            }

            for (int i = 0; i < _bodyXPositions.Count; i++)
            {
                if (_bodyXPositions[i] == _head.XPosition && _bodyYPositions[i] == _head.YPosition)
                {
                    _isGameOver = true;
                }
            }
        }

        private void DrawGameField()
        {
            DrawBorder();
            DrawBerry();
            DrawSnake();
        }

        private void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < _screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, _screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < _screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(_screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            Console.SetCursorPosition(_berryXPosition, _berryYPosition);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private void DrawSnake()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < _bodyXPositions.Count; i++)
            {
                Console.SetCursorPosition(_bodyXPositions[i], _bodyYPositions[i]);
                Console.Write("■");
            }

            Console.SetCursorPosition(_head.XPosition, _head.YPosition);
            Console.ForegroundColor = _head.PixelColor;
            Console.Write("■");

            if (_berryXPosition == _head.XPosition && _berryYPosition == _head.YPosition)
            {
                _score++;
                _berryXPosition = _randomNumberGenerator.Next(1, _screenWidth - 2);
                _berryYPosition = _randomNumberGenerator.Next(1, _screenHeight - 2);
            }
        }

        private void UpdateSnakePosition()
        {
            _bodyXPositions.Add(_head.XPosition);
            _bodyYPositions.Add(_head.YPosition);

            switch (_movementDirection)
            {
                case "UP":
                    _head.YPosition--;
                    break;
                case "DOWN":
                    _head.YPosition++;
                    break;
                case "LEFT":
                    _head.XPosition--;
                    break;
                case "RIGHT":
                    _head.XPosition++;
                    break;
            }

            if (_bodyXPositions.Count > _score)
            {
                _bodyXPositions.RemoveAt(0);
                _bodyYPositions.RemoveAt(0);
            }
        }

        private void HandleUserInput()
        {
            _startTime = DateTime.Now;
            _buttonPressed = "no";

            while (true)
            {
                _currentTime = DateTime.Now;
                if (_currentTime.Subtract(_startTime).TotalMilliseconds > 500) break;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.UpArrow && _movementDirection != "DOWN" && _buttonPressed == "no")
                    {
                        _movementDirection = "UP";
                        _buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.DownArrow && _movementDirection != "UP" && _buttonPressed == "no")
                    {
                        _movementDirection = "DOWN";
                        _buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.LeftArrow && _movementDirection != "RIGHT" && _buttonPressed == "no")
                    {
                        _movementDirection = "LEFT";
                        _buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.RightArrow && _movementDirection != "LEFT" && _buttonPressed == "no")
                    {
                        _movementDirection = "RIGHT";
                        _buttonPressed = "yes";
                    }
                }
            }
        }

        private void EndGame()
        {
            Console.SetCursorPosition(_screenWidth / 5, _screenHeight / 2);
            Console.WriteLine("Game over, Score: " + _score);
            Console.SetCursorPosition(_screenWidth / 5, _screenHeight / 2 + 1);
        }
    }
}
