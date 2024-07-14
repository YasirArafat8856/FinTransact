namespace AccountService.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
    }
}
