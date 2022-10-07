namespace projectOneApi_v1.Dtos
{
    public class GetPendingTicketDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Amount { get; set; }

        public string Status { get; set; }

        public GetPendingTicketDto(int id, string name, string description, int amount, string status)
        {
            Id = id;
            Name = name;
            Description = description;
            Amount = amount;
            Status = status;
        }
    }
}
