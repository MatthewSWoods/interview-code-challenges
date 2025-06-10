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
        private readonly IBorrowerRepository _borrowerRepository;

        public OnLoanController(
            ILogger<OnLoanController> logger,
            IBorrowerRepository borrowerRepository)
        {
            _logger = logger;
            _borrowerRepository = borrowerRepository;
        }

        [HttpGet]
        [Route("GetOnLoan")]
        public List<Borrower> GetOnLoan()
        {
            throw new NotImplementedException();
        }
    }
}
