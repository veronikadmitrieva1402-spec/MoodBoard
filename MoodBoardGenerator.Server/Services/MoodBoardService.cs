using MoodBoardGenerator.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodBoardGenerator.Server.Services
{
    public class MoodBoardService : IMoodBoardService
    {
        private readonly IMoodBoardRepository _repository;
        private readonly Random _random = new();

        private readonly string[] _inspirations = new[]
        {
            "Творчество - это интеллект, играющий воображением",
            "Красота начинается в тот момент, когда вы решаете быть собой",
            "Дизайн - это не то, как это выглядит, а то, как это работает",
            "Цвет - это улыбка природы",
            "Меньше - это больше",
            "Искусство - это ложь, которая говорит нам правду",
            "Вдохновение существует, но оно должно найти вас работающим",
            "Дизайн - это тихая революция"
        };

        private readonly Dictionary<MoodStyle, (string[] colors, string[] fonts)> _styleThemes = new()
        {
            [MoodStyle.Minimalist] = (
                new[] { "#FFFFFF", "#F5F5F5", "#E0E0E0", "#9E9E9E", "#424242" },
                new[] { "Inter", "Helvetica", "Arial" }
            ),
            [MoodStyle.Bohemian] = (
                new[] { "#FF6B6B", "#FFE66D", "#4ECDC4", "#FF9F43", "#2C3E50" },
                new[] { "Playfair Display", "Georgia", "Times New Roman" }
            ),
            [MoodStyle.Industrial] = (
                new[] { "#2C3E50", "#34495E", "#7F8C8D", "#BDC3C7", "#ECF0F1" },
                new[] { "Roboto Mono", "Courier New", "Consolas" }
            ),
            [MoodStyle.Scandinavian] = (
                new[] { "#F8F9FA", "#E9ECEF", "#DEE2E6", "#ADB5BD", "#6C757D" },
                new[] { "Nunito", "Open Sans", "Arial" }
            ),
            [MoodStyle.Vintage] = (
                new[] { "#8B4513", "#D2691E", "#CD853F", "#DEB887", "#F5DEB3" },
                new[] { "EB Garamond", "Garamond", "Times New Roman" }
            ),
            [MoodStyle.Modern] = (
                new[] { "#1A1A2E", "#16213E", "#0F3460", "#E94560", "#F5F5F5" },
                new[] { "Poppins", "Montserrat", "Arial" }
            ),
            [MoodStyle.Eclectic] = (
                new[] { "#FF006E", "#FFB703", "#8338EC", "#3A86FF", "#FB5607" },
                new[] { "Bangers", "Pacifico", "Comic Sans MS" }
            )
        };

        public MoodBoardService(IMoodBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task<MoodBoard> GenerateMoodBoardAsync(string theme, string description, MoodStyle style)
        {
            var board = new MoodBoard
            {
                Id = Guid.NewGuid(),
                Name = $"{style} Mood Board: {theme}",
                Theme = theme,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                Style = style
            };

            var (colors, fonts) = _styleThemes[style];
            board.ColorPalette = string.Join(", ", colors);

            board.Items = await GenerateBoardItemsAsync(theme, style, colors, fonts);
            return board;
        }

        private Task<List<MoodItem>> GenerateBoardItemsAsync(string theme, MoodStyle style, string[] colors, string[] fonts)
        {
            var items = new List<MoodItem>();
            var random = new Random();

            foreach (var color in colors.Take(3))
            {
                items.Add(new MoodItem
                {
                    Id = Guid.NewGuid(),
                    Title = $"Цвет {Array.IndexOf(colors, color) + 1}",
                    ColorCode = color,
                    Type = MoodItemType.Color,
                    Description = $"Оттенок из палитры {style}"
                });
            }

            var selectedQuotes = _inspirations.OrderBy(x => random.Next()).Take(3).ToArray();
            foreach (var quote in selectedQuotes)
            {
                items.Add(new MoodItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Вдохновение",
                    Description = quote,
                    Type = MoodItemType.Text
                });
            }

            foreach (var font in fonts)
            {
                items.Add(new MoodItem
                {
                    Id = Guid.NewGuid(),
                    Title = $"Шрифт {font}",
                    Description = $"Пример использования шрифта {font} для заголовков",
                    Type = MoodItemType.Font
                });
            }

            for (int i = 0; i < 3; i++)
            {
                var color = colors[new Random().Next(colors.Length)].Replace("#", "");
                items.Add(new MoodItem
                {
                    Id = Guid.NewGuid(),
                    Title = $"Иконка {i + 1}",
                    ImageUrl = $"https://via.placeholder.com/200x200/{color}/FFFFFF?text={i+1}",
                    Type = MoodItemType.Icon,
                    Description = $"Элемент для темы {theme}"
                });
            }

            return Task.FromResult(items);
        }

        public async Task<MoodBoard?> GetMoodBoardAsync(Guid id)
        {
            return await _repository.GetMoodBoardAsync(id);
        }

        public async Task<IEnumerable<MoodBoard>> GetUserMoodBoardsAsync(string userId)
        {
            return await _repository.GetUserMoodBoardsAsync(userId);
        }

        public async Task<MoodBoard> SaveMoodBoardAsync(MoodBoard board, string userId)
        {
            return await _repository.SaveMoodBoardAsync(board, userId);
        }

        public async Task DeleteMoodBoardAsync(Guid id)
        {
            await _repository.DeleteMoodBoardAsync(id);
        }

        public async Task<MoodBoard> AddItemToBoardAsync(Guid boardId, MoodItem item)
        {
            var board = await _repository.GetMoodBoardAsync(boardId);
            if (board == null) throw new KeyNotFoundException($"Mood board with id {boardId} not found");
            item.Id = Guid.NewGuid();
            board.Items.Add(item);
            return board;
        }

        public async Task<MoodBoard> RemoveItemFromBoardAsync(Guid boardId, Guid itemId)
        {
            var board = await _repository.GetMoodBoardAsync(boardId);
            if (board == null) throw new KeyNotFoundException($"Mood board with id {boardId} not found");
            var item = board.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null) board.Items.Remove(item);
            return board;
        }
    }
}