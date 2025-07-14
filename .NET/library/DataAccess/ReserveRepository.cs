using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class ReserveRepository : IReserveRepository
    {

        public ReserveRepository()
        {
        }

        public DateTime AvailableFrom(Guid bookId)
        {
            using (var context = new LibraryContext())
            {
                var bookStock = context.Catalogue
                    .Include(x => x.Book)
                    .FirstOrDefault(x => x.Book.Id == bookId);

                return bookStock.LoanEndDate.Value.AddDays(1);
            }
        }

        public ReservationResult ReserveBook(Guid bookId, Guid borrowerId)
        {
            using (var context = new LibraryContext())
            {
                var bookStock = context.Catalogue
                    .Include(x => x.Book)
                    .Include(x => x.OnLoanTo)
                    .Where(x => x.Book.Id == bookId)
                    .FirstOrDefault();

                if (bookStock == null)
                {
                    return ReservationResult.BookNotFound;
                }

                if (bookStock.Reserved != null)
                {
                    return ReservationResult.AlreadyReserved;
                }

                var borrower = context.Borrowers
                    .FirstOrDefault(x => x.Id == borrowerId);

                if (borrower == null)
                {
                    return ReservationResult.BorrowerNotFound;
                }

                bookStock.Reserved = borrower;

                context.SaveChanges();
            }

            return ReservationResult.Success;
        }
    }
}
