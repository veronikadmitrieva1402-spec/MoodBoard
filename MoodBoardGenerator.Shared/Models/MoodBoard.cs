using System;
using System.Collections.Generic;

namespace MoodBoardGenerator.Shared.Models
{
    public class MoodBoard
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<MoodItem> Items { get; set; } = new();
        public string? ColorPalette { get; set; }
        public MoodStyle Style { get; set; } = MoodStyle.Minimalist;
    }

    public class MoodItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? ColorCode { get; set; }
        public MoodItemType Type { get; set; }
    }

    public enum MoodItemType
    {
        Image,
        Color,
        Text,
        Icon,
        Font
    }

    public enum MoodStyle
    {
        Minimalist,
        Bohemian,
        Industrial,
        Scandinavian,
        Vintage,
        Modern,
        Eclectic
    }
}