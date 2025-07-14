using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IOnLoanRepository
    {
        public List<OnLoan> GetOnLoans();
        public bool ReturnBook(Guid bookId);
    }
}
