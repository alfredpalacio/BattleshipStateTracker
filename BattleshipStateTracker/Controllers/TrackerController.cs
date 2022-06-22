using BattleshipStateTracker.Core.Services.Interfaces;
using BattleshipStateTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BattleshipStateTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        #region Local variables

        private readonly IMemoryCacheWrapper _memoryCacheWrapper;
        private readonly IBoardService _boardService;

        #endregion Local variables

        #region Constructor

        public TrackerController
        (
            IMemoryCacheWrapper memoryCacheWrapper,
            IBoardService boardService
        )
        {
            _memoryCacheWrapper = memoryCacheWrapper;
            _boardService = boardService;
        }

        #endregion Constructor

        #region Public methods

        [HttpPost("create")]
        public IActionResult Create()
        {
            try
            {
                _memoryCacheWrapper.SetCache(nameof(Board), new Board());
                return Ok("New board created");
            }
            catch (Exception e)
            {
                return BadRequest($"Unhandled exception: {e.Message}");
            }
        }

        [HttpPost("addRandom")]
        public IActionResult Add()
        {
            try
            {
                var result = _boardService.AddBattleship();
                var message = result ? "Battleship added successfully" : "Failed to add battleship";
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest($"Unhandled exception: {e.Message}");
            }
        }

        [HttpPost("add")]
        public IActionResult Add(Battleship battleship)
        {
            try
            {
                var result = _boardService.AddBattleship(battleship);
                var message = result ? "Battleship added successfully" : "Failed to add battleship";
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest($"Unhandled exception: {e.Message}");
            }
        }

        [HttpGet("attack")]
        public IActionResult Attack(int xCoordinate, int yCoordinate)
        {
            try
            {
                var result = _boardService.Attack(xCoordinate, yCoordinate);
                var message = result ? "hit" : "miss";
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest($"Unhandled exception: {e.Message}");
            }
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            try
            {
                var result = _boardService.IsGameOver();
                var message = result ? "All ships are sunk" : "The battle continues";
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest($"Unhandled exception: {e.Message}");
            }
        }

        #endregion Public methods
    }
}