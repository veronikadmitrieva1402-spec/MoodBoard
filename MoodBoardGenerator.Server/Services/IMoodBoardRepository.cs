using MoodBoardGenerator.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodBoardGenerator.Server.Services
{
    public interface IMoodBoardRepository
    {
        Task<MoodBoard?> GetMoodBoardAsync(Guid id);
        Task<IEnumerable<MoodBoard>> GetUserMoodBoardsAsync(string userId);
        Task<MoodBoard> SaveMoodBoardAsync(MoodBoard board, string userId);
        Task DeleteMoodBoardAsync(Guid id);
    }
}