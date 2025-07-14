namespace OneBeyondApi.Model
{
    public class OnLoan
    {
        public Borrower Borrower { get; set; }
        public IList<string> BookTitles { get; set; }
    }
}
