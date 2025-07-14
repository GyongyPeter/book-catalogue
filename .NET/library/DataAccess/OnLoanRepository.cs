using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class OnLoanRepository : IOnLoanRepository
    {
        private static readonly int FINES_FOR_LATE_RETURNS = 500;

        public OnLoanRepository()
        {
        }

        public List<OnLoan> GetOnLoans()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Catalogue
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
        }

        public bool ReturnBook(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                var bookStock = context.Catalogue
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

                context.SaveChanges();
            }

            return true;
        }
    }
}
