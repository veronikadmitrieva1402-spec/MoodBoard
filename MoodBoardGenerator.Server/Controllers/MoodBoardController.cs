using Microsoft.AspNetCore.Mvc;
using MoodBoardGenerator.Shared.Models;
using MoodBoardGenerator.Server.Services;

namespace MoodBoardGenerator.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodBoardController : ControllerBase
    {
        private readonly IMoodBoardService _moodBoardService;
        private readonly ILogger<MoodBoardController> _logger;

        public MoodBoardController(IMoodBoardService moodBoardService, ILogger<MoodBoardController> logger)
        {
            _moodBoardService = moodBoardService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMoodBoard([FromBody] GenerateMoodBoardRequest request)
        {
            try
            {
                var board = await _moodBoardService.GenerateMoodBoardAsync(
                    request.Theme,
                    request.Description ?? string.Empty,
                    request.Style
                );
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating mood board");
                return StatusCode(500, "An error occurred while generating the mood board");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMoodBoard(Guid id)
        {
            try
            {
                var board = await _moodBoardService.GetMoodBoardAsync(id);
                if (board == null) return NotFound($"Mood board with id {id} not found");
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting mood board");
                return StatusCode(500, "An error occurred while retrieving the mood board");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMoodBoards(string userId)
        {
            try
            {
                var boards = await _moodBoardService.GetUserMoodBoardsAsync(userId);
                return Ok(boards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user mood boards");
                return StatusCode(500, "An error occurred while retrieving user mood boards");
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveMoodBoard([FromBody] SaveMoodBoardRequest request)
        {
            try
            {
                var board = await _moodBoardService.SaveMoodBoardAsync(request.Board, request.UserId);
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving mood board");
                return StatusCode(500, "An error occurred while saving the mood board");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoodBoard(Guid id)
        {
            try
            {
                await _moodBoardService.DeleteMoodBoardAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting mood board");
                return StatusCode(500, "An error occurred while deleting the mood board");
            }
        }

        [HttpPost("{boardId}/items")]
        public async Task<IActionResult> AddItem(Guid boardId, [FromBody] MoodItem item)
        {
            try
            {
                var board = await _moodBoardService.AddItemToBoardAsync(boardId, item);
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to mood board");
                return StatusCode(500, "An error occurred while adding the item");
            }
        }

        [HttpDelete("{boardId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid boardId, Guid itemId)
        {
            try
            {
                var board = await _moodBoardService.RemoveItemFromBoardAsync(boardId, itemId);
                return Ok(board);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from mood board");
                return StatusCode(500, "An error occurred while removing the item");
            }
        }
    }

    public class GenerateMoodBoardRequest
    {
        public string Theme { get; set; } = string.Empty;
        public string? Description { get; set; }
        public MoodStyle Style { get; set; } = MoodStyle.Minimalist;
    }

    public class SaveMoodBoardRequest
    {
        public MoodBoard Board { get; set; } = new();
        public string UserId { get; set; } = string.Empty;
    }
}