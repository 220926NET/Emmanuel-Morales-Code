﻿namespace projectOneApi_v1.Models
{
    public class Tickets
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public Employees Employee { get; set; }


    }
}
