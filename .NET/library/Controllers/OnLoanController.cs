using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.Model;
using OneBeyondApi.Services;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnLoanController : ControllerBase
    {
        private readonly ILogger<OnLoanController> _logger;
        private readonly ILoanService _loanService;

        public OnLoanController(
            ILogger<OnLoanController> logger,
            ILoanService loanService)
        {
            _logger = logger;
            _loanService = loanService;
        }

        [HttpGet]
        [Route("GetOnLoan")]
        public Dictionary<Borrower, List<string>> GetOnLoan()
        {
            return _loanService.GetOnLoanDetails();
        }
    }
}
