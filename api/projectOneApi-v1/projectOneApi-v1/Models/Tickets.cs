namespace projectOneApi_v1.Models
{
    public class Tickets
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; } = "Pending";

        public int Amount { get; set; }

        public int employeeId { get; set; }


    }
}
