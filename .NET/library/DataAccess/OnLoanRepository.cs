using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class OnLoanRepository : IOnLoanRepository
    {
        private static readonly int FINES_FOR_LATE_RETURNS = 500;

        private readonly LibraryContext _context;

        public OnLoanRepository(LibraryContext context)
        {
            _context = context;
        }

        public List<OnLoan> GetOnLoans()
        {
            var list = _context.Catalogue
                .Include(x => x.Book)
                .Include(x => x.OnLoanTo)
                .Where(x => x.OnLoanTo != null)
                .GroupBy(x => x.OnLoanTo)
                .Select(g => new OnLoan
                {
                    Borrower = g.Key!,
                    BookTitles = g.Select(bs => bs.Book.Name).ToList()
                })
                .ToList();

            return list;
        }

        public bool ReturnBook(Guid bookId)
        {
            var bookStock = _context.Catalogue
                .Include(x => x.Book)
                .Include(x => x.OnLoanTo)
                .Where(x => x.Book.Id == bookId)
                .FirstOrDefault();

            if (bookStock == null || bookStock.OnLoanTo == null)
            {
                return false;
            }

            if (bookStock.LoanEndDate < DateTime.UtcNow)
            {
                bookStock.OnLoanTo.Fine += FINES_FOR_LATE_RETURNS;
            }

            bookStock.LoanEndDate = null;
            bookStock.OnLoanTo = null;

            _context.SaveChanges();

            return true;
        }
    }
}
