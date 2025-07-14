using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnLoanController : ControllerBase
    {
        private readonly ILogger<OnLoanController> _logger;
        private readonly IOnLoanRepository _onLoanRepository;

        public OnLoanController(ILogger<OnLoanController> logger, IOnLoanRepository onLoanRepository)
        {
            _logger = logger;
            _onLoanRepository = onLoanRepository;
        }

        [HttpGet]
        [Route("GetOnLoans")]
        public IList<OnLoan> Get()
        {
            _logger.LogInformation("Fetching all borrowers with active loan(s).");

            var result = _onLoanRepository.GetOnLoans();

            _logger.LogInformation("Found {Count} borrowers with active loan(s).", result.Count);

            return result;
        }

        [HttpPost]
        [Route("ReturnBook")]
        public IActionResult Post(Guid bookId)
        {
            _logger.LogInformation("Attempting to return book with ID: {BookId}", bookId);

            var success = _onLoanRepository.ReturnBook(bookId);

            if (success)
            {
                _logger.LogInformation("Book returned successfully. ID: {BookId}", bookId);
                return Ok();
            }
            else
            {
                _logger.LogWarning("Book return failed. Book with ID {BookId} not found.", bookId);
                return NotFound($"Book with ID {bookId} not found or not on loan.");
            }
        }
    }
}
