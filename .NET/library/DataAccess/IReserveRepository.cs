using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface IReserveRepository
    {
        public DateTime AvailableFrom(Guid bookId);
        public ReservationResult ReserveBook(Guid bookId, Guid borrowerId);
    }
}
