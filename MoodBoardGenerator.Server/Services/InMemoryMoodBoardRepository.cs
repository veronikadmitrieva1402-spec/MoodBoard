using MoodBoardGenerator.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodBoardGenerator.Server.Services
{
    public class InMemoryMoodBoardRepository : IMoodBoardRepository
    {
        private readonly ConcurrentDictionary<Guid, MoodBoard> _boards = new();
        private readonly ConcurrentDictionary<string, List<Guid>> _userBoards = new();

        public Task<MoodBoard> GetMoodBoardAsync(Guid id)
        {
            _boards.TryGetValue(id, out var board);
            return Task.FromResult(board);
        }

        public Task<IEnumerable<MoodBoard>> GetUserMoodBoardsAsync(string userId)
        {
            if (_userBoards.TryGetValue(userId, out var boardIds))
            {
                var boards = boardIds
                    .Select(id => _boards.TryGetValue(id, out var board) ? board : null)
                    .Where(b => b != null)
                    .Select(b => b!);
                return Task.FromResult(boards);
            }
            return Task.FromResult(Enumerable.Empty<MoodBoard>());
        }

        public Task<MoodBoard> SaveMoodBoardAsync(MoodBoard board, string userId)
        {
            _boards[board.Id] = board;

            if (!_userBoards.ContainsKey(userId))
                _userBoards[userId] = new List<Guid>();

            if (!_userBoards[userId].Contains(board.Id))
                _userBoards[userId].Add(board.Id);

            return Task.FromResult(board);
        }

        public Task DeleteMoodBoardAsync(Guid id)
        {
            _boards.TryRemove(id, out _);
            foreach (var userBoards in _userBoards.Values)
            {
                userBoards.Remove(id);
            }
            return Task.CompletedTask;
        }
    }
}