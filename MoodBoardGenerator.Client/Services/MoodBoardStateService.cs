using System;
using MoodBoardGenerator.Shared.Models;

namespace MoodBoardGenerator.Client.Services
{
    public class MoodBoardStateService : IMoodBoardStateService
    {
        private MoodBoard? _currentBoard;

        public MoodBoard? CurrentBoard
        {
            get => _currentBoard;
            set
            {
                _currentBoard = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();

        public void ClearBoard()
        {
            CurrentBoard = null;
        }
    }
}