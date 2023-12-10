namespace Domain.Clients
{
    public class Client
    {
        public Guid Id { get; set; }
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}