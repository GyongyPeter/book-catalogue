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
            return _onLoanRepository.GetOnLoans();
        }

        [HttpPost]
        [Route("ReturnBook")]
        public bool Post(Guid bookId)
        {
            return _onLoanRepository.ReturnBook(bookId);
        }
    }
}
