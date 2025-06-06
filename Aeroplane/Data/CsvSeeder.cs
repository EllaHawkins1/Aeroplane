using System.Globalization;
using Aeroplane.Models;

namespace Aeroplane.Data
{
    public static class CsvSeeder
    {
        public static void SeedFromCsv(FlightDbContext context)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "FlightInformation.csv");

            if (!File.Exists(filePath))
                return;

            var lines = File.ReadAllLines(filePath).Skip(1); // skip header row

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length < 8) continue; // expecting 8 fields including Id

                if (!DateTime.TryParse(parts[5], CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureTime))
                    continue;

                if (!DateTime.TryParse(parts[6], CultureInfo.InvariantCulture, DateTimeStyles.None, out var arrivalTime))
                    continue;

                var flight = new Flight
                {
                    FlightNumber = parts[1],
                    Airline = parts[2],
                    DepartureAirport = parts[3],
                    ArrivalAirport = parts[4],
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    Status = Enum.TryParse<FlightStatus>(parts[7], true, out var status)
                             ? status
                             : FlightStatus.Scheduled
                };

                context.Flights.Add(flight);
            }

            context.SaveChanges();
        }
    }
}
