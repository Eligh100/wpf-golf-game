using GalaSoft.MvvmLight.Command;
using Puttshack_API.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Puttshack_Project.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        static readonly HttpClient client = new HttpClient();

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }


        private string _scoreText;
        public string ScoreText
        {
            get { return _scoreText; }
            set
            {
                _scoreText = value;
                OnPropertyChanged();
            }
        }

        private string _moveText;
        public string MoveText
        {
            get { return _moveText; }
            set
            {
                _moveText = value;
                OnPropertyChanged();
            }
        }

        private int _numberOfMoves;
        public int NumberOfMoves
        {
            get { return _numberOfMoves; }
            set
            {
                _numberOfMoves = value;
                OnPropertyChanged();
            }
        }


        private bool _showStartSplash;
        public bool ShowStartSplash
        {
            get { return _showStartSplash; }
            set
            {
                _showStartSplash = value;
                OnPropertyChanged();
            }
        }

        private bool _canGameBeStarted;
        public bool CanGameBeStarted
        {
            get { return _canGameBeStarted; }
            set
            {
                _canGameBeStarted = value;
                OnPropertyChanged();
            }
        }

        private bool _isGameStarted;
        public bool IsGameStarted
        {
            get { return _isGameStarted; }
            set
            {
                _isGameStarted = value;
                OnPropertyChanged();
            }
        }

        private bool _isGameFinished;
        public bool IsGameFinished
        {
            get { return _isGameFinished; }
            set
            {
                _isGameFinished = value;
                OnPropertyChanged();
            }
        }

        private string _currentMove;
        public string CurrentMove
        {
            get { return _currentMove; }
            set
            {
                _currentMove = value;
                OnPropertyChanged();
            }
        }

        private ICommand _startGameCommand;
        public ICommand StartGameCommand
        {
            get { return _startGameCommand; }
            set
            {
                _startGameCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _nextMoveCommand;
        public ICommand NextMoveCommand
        {
            get { return _nextMoveCommand; }
            set
            {
                _nextMoveCommand = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            StartGameCommand = new RelayCommand(() => StartGame(), () => true);
            NextMoveCommand = new RelayCommand(() => NextMove(), () => true);

            CurrentMove = "";
            ShowStartSplash = true;
            CanGameBeStarted = true;
            IsGameStarted = false;
            IsGameFinished = false;
        }

        private async void StartGame()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:5001/api/game/start");
                response.EnsureSuccessStatusCode();
                string jsonResponseBody = await response.Content.ReadAsStringAsync();

                GameMove nextMove = JsonSerializer.Deserialize<GameMove>(jsonResponseBody);

                CurrentMove = nextMove.MoveName;
                Score = 50;
                ScoreText = $"Score: {Score}";
                NumberOfMoves = 0;
                MoveText = $"Moves: {NumberOfMoves}";
                IsGameStarted = true;
                ShowStartSplash = false;
                CanGameBeStarted = false;
                IsGameFinished = false;
            }
            catch (HttpRequestException e)
            {
                Trace.WriteLine("Error :{0} ", e.Message);
            }
        }

        private async void NextMove()
        {
            try
            {
                NumberOfMoves++;

                MoveText = $"Moves: {NumberOfMoves}";

                HttpResponseMessage response = await client.GetAsync($"https://localhost:5001/api/game/next?currentMoveNumber={NumberOfMoves}");
                response.EnsureSuccessStatusCode();
                string jsonResponseBody = await response.Content.ReadAsStringAsync();

                GameMove nextMove = JsonSerializer.Deserialize<GameMove>(jsonResponseBody);

                Score += nextMove.MoveScore;

                int deltaScore = nextMove.MoveScore;

                if (deltaScore >= 0)
                {
                    if (nextMove.MoveId == GameState.BallInHole.ToString())
                    {
                        ScoreText = $"Score: {Score}";
                    }
                    else
                    {
                        ScoreText = $"Score: {Score} (+{nextMove.MoveScore})";
                    }
                }
                else
                {
                    ScoreText = $"Score: {Score} ({nextMove.MoveScore})";
                }

                CurrentMove = nextMove.MoveName;

                if (nextMove.MoveId == GameState.BallInHole.ToString() || nextMove.MoveId == GameState.SuperTubeActivated.ToString())
                {
                    IsGameStarted = false;
                    CanGameBeStarted = true;
                    IsGameFinished = true;
                }
            }
            catch (HttpRequestException e)
            {
                Trace.WriteLine("Error :{0} ", e.Message);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
