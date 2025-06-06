using System;
using System.ComponentModel.DataAnnotations;

//init everything we need 

namespace Aeroplane.Models
{
    public class Flight
    {
        public int Id { get; set; }

        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public string Airline { get; set; }

        [Required]
        public string DepartureAirport { get; set; }

        [Required]
        public string ArrivalAirport { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        [Required]
        public FlightStatus Status { get; set; }
    }

    public enum FlightStatus
    {
        Scheduled,
        Delayed,
        Cancelled,
        InAir,
        Landed
    }
}
 