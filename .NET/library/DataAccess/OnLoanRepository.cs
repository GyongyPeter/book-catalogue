using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class OnLoanRepository : IOnLoanRepository
    {
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

                bookStock.LoanEndDate = null;
                bookStock.OnLoanTo = null;

                context.SaveChanges();
            }

            return true;
        }
    }
}
