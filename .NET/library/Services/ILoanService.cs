using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public interface ILoanService
    {
        public Dictionary<Borrower, List<string>> GetOnLoanDetails();
    }
}
