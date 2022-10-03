using Microsoft.AspNetCore.Authentication;

namespace projectOneApi_v1.Models
{
    public class Employees
    {
        public int id { get; set; }

        public string? name { get; set; }

        public List<Tickets>? tickets { get; set; }


    }
}
