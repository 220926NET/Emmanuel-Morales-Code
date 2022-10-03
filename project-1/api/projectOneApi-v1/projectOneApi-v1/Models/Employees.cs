using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectOneApi_v1.Models
{
    public class Employees
    {
        public int id { get; set; }

        public string? name { get; set; }

        public User user { get; set; }



    }
}
