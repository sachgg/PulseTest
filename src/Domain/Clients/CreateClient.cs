namespace Domain.Clients
{
    public class CreateClient
    {
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}