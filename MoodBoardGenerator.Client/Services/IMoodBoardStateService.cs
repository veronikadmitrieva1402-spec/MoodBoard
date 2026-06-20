using MoodBoardGenerator.Shared.Models;
using System;

namespace MoodBoardGenerator.Client.Services
{
    public interface IMoodBoardStateService
    {
        MoodBoard? CurrentBoard { get; set; }
        event Action? OnChange;
        void NotifyStateChanged();
        void ClearBoard();
    }
}