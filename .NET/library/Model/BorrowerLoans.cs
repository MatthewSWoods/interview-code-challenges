namespace OneBeyondApi.Model
{
    public class BorrowerLoans
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public List<string> BookNames { get; set; }
    }
}
