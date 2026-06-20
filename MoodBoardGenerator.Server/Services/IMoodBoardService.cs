using MoodBoardGenerator.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodBoardGenerator.Server.Services
{
    public interface IMoodBoardService
    {
        Task<MoodBoard> GenerateMoodBoardAsync(string theme, string description, MoodStyle style);
        Task<MoodBoard?> GetMoodBoardAsync(Guid id);
        Task<IEnumerable<MoodBoard>> GetUserMoodBoardsAsync(string userId);
        Task<MoodBoard> SaveMoodBoardAsync(MoodBoard board, string userId);
        Task DeleteMoodBoardAsync(Guid id);
        Task<MoodBoard> AddItemToBoardAsync(Guid boardId, MoodItem item);
        Task<MoodBoard> RemoveItemFromBoardAsync(Guid boardId, Guid itemId);
    }
}