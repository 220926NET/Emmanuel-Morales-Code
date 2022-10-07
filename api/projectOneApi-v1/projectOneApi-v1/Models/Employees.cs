using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectOneApi_v1.Models
{
    public class Employees
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public Logins Login { get; set; }

        public bool IsManager { get; set; } = false; 

    }
}
