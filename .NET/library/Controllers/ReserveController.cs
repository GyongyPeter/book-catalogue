using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReserveController : ControllerBase
    {
        private readonly ILogger<ReserveController> _logger;
        private readonly IReserveRepository _reserveRepository;

        public ReserveController(ILogger<ReserveController> logger, IReserveRepository reserveRepository)
        {
            _logger = logger;
            _reserveRepository = reserveRepository;
        }

        [HttpGet]
        [Route("AvailableFrom")]
        public DateTime Get(Guid bookId)
        {
            _logger.LogInformation("Fetching information about the book availability.");

            return _reserveRepository.AvailableFrom(bookId);
        }

        [HttpPost]
        [Route("ReserveBook")]
        public IActionResult Post(Guid bookId, Guid borrowerId)
        {
            _logger.LogInformation("Attempting to reserve book with ID: {BookId} and borrower with ID: {BorrowerId}", bookId, borrowerId);

            var result = _reserveRepository.ReserveBook(bookId, borrowerId);

            switch (result)
            {
                case ReservationResult.Success:
                    _logger.LogInformation("Book reserved successfully. Book ID: {BookId}, Borrower ID: {BorrowerId}", bookId, borrowerId);
                    return Ok();
                case ReservationResult.BookNotFound:
                    _logger.LogWarning("Book with ID {BookId} not found.", bookId);
                    return NotFound($"Book with ID {bookId} not found.");
                case ReservationResult.BorrowerNotFound:
                    _logger.LogWarning("Borrower with ID {BorrowerId} not found.", borrowerId);
                    return NotFound($"Borrower with ID {borrowerId} not found.");
                case ReservationResult.AlreadyReserved:
                    _logger.LogWarning("Book with ID {BookId} is already reserved.", bookId);
                    return Conflict($"Book with ID {bookId} is already reserved.");
                default:
                    _logger.LogError("Unknown error occurred while reserving book with ID {BookId}.", bookId);
                    return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
