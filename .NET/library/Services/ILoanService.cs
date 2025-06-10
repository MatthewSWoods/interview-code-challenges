using OneBeyondApi.Model;

namespace OneBeyondApi.Services
{
    public interface ILoanService
    {
        public Task<List<OnLoan>> GetOnLoanDetails();
    }
}
