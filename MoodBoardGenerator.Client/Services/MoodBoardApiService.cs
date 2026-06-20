using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MoodBoardGenerator.Shared.Models;

namespace MoodBoardGenerator.Client.Services
{
    public class MoodBoardApiService : IMoodBoardApiService
    {
        private readonly HttpClient _httpClient;

        public MoodBoardApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MoodBoard> GenerateMoodBoardAsync(string theme, string description, MoodStyle style)
        {
            var response = await _httpClient.PostAsJsonAsync("api/moodboard/generate", new
            {
                theme,
                description,
                style
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoodBoard>() ?? new MoodBoard();
        }

        public async Task<MoodBoard?> GetMoodBoardAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<MoodBoard>($"api/moodboard/{id}");
        }

        public async Task<IEnumerable<MoodBoard>> GetUserMoodBoardsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<MoodBoard>>($"api/moodboard/user/{userId}") 
                   ?? new List<MoodBoard>();
        }

        public async Task<MoodBoard> SaveMoodBoardAsync(MoodBoard board, string userId)
        {
            var response = await _httpClient.PostAsJsonAsync("api/moodboard/save", new
            {
                board,
                userId
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoodBoard>() ?? new MoodBoard();
        }

        public async Task DeleteMoodBoardAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"api/moodboard/{id}");
        }

        public async Task<MoodBoard> AddItemToBoardAsync(Guid boardId, MoodItem item)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/moodboard/{boardId}/items", item);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoodBoard>() ?? new MoodBoard();
        }

        public async Task<MoodBoard> RemoveItemFromBoardAsync(Guid boardId, Guid itemId)
        {
            var response = await _httpClient.DeleteAsync($"api/moodboard/{boardId}/items/{itemId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<MoodBoard>() ?? new MoodBoard();
        }
    }
}