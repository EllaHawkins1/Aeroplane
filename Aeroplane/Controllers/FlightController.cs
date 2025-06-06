using Aeroplane.Controllers;
using Aeroplane.Data;
using Aeroplane.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

/*
Implement the following API endpoints:
-GET / api / flights: Retrieve all flights
- GET /api/flights/{id}: Retrieve a specific flight by ID
- POST /api/flights: Create a new flight
- PUT / api / flights /{ id}: Update an existing flight
- DELETE /api/flights/{id}: Delete a flight
- GET /api/flights/search: Search flights by various criteria (e.g., airline, departure/arrival
airport, date range)
*/


namespace Aeroplane.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {

        private readonly ILogger<FlightController> _logger;
        private readonly FlightDbContext _context;

       

        public FlightController(FlightDbContext context, ILogger<FlightController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /**
         * Retrieves all flight records from the database.
         * Similar to a Java GET endpoint returning a list of entities.
         */

        [HttpGet("get Dataset")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            _logger.LogInformation("Fetching all flights from the database.");

            try
            {
                var flights = await _context.Flights.ToListAsync();
                _logger.LogInformation("Successfully retrieved {Count} flights.", flights.Count);
                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving flights.");
                return StatusCode(500, "An unexpected error occurred while retrieving flights.");
            }
        }


        /**
         * Fetches a single flight by its unique ID.
         * 
         */


        [HttpGet("get by ID")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            _logger.LogInformation("Attempting to fetch flight with ID {Id}.", id);

            try
            {
                var flight = await _context.Flights.FindAsync(id);
                if (flight == null)
                {
                    _logger.LogWarning("Flight with ID {Id} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved flight with ID {Id}.", id);
                return flight;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight with ID {Id}.", id);
                return StatusCode(500, "An unexpected error occurred while retrieving the flight.");
            }
        }


        /**
       * Creates a new flight entry in the in-memory database.
       * Accepts a full Flight object 
       */


        [HttpPost("Create New Flight")]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _logger.LogInformation("Attempting to create a new flight with flight number {FlightNumber}.", flight.FlightNumber);

            try
            {
                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Flight created successfully with ID {Id}.", flight.Id);
                return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the flight with flight number {FlightNumber}.", flight.FlightNumber);
                return StatusCode(500, "An unexpected error occurred while creating the flight.");
            }
        }


        /**
        * Searches for flights using optional filters like airline, airports, and date range.
        * Acts like a dynamic query builder
        */

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Flight>>> SearchFlights(
      [FromQuery] string? airline,
      [FromQuery] string? departureAirport,
      [FromQuery] string? arrivalAirport,
      [FromQuery] DateTime? DepartureTime,
      [FromQuery] DateTime? ArrivalTime)
        {
            _logger.LogInformation("Searching flights with filters - Airline: {Airline}, DepartureAirport: {DepartureAirport}, ArrivalAirport: {ArrivalAirport}, DepartureTime: {DepartureTime}, ArrivalTime: {ArrivalTime}",
                airline, departureAirport, arrivalAirport, DepartureTime, ArrivalTime);

            try
            {
                var query = _context.Flights.AsQueryable();

                if (!string.IsNullOrEmpty(airline))
                    query = query.Where(f => f.Airline.ToLower().Contains(airline.ToLower()));

                if (!string.IsNullOrEmpty(departureAirport))
                    query = query.Where(f => f.DepartureAirport.ToLower().Contains(departureAirport.ToLower()));

                if (!string.IsNullOrEmpty(arrivalAirport))
                    query = query.Where(f => f.ArrivalAirport.ToLower().Contains(arrivalAirport.ToLower()));

                if (DepartureTime.HasValue)
                    query = query.Where(f => f.DepartureTime >= DepartureTime.Value);

                if (ArrivalTime.HasValue)
                    query = query.Where(f => f.DepartureTime <= ArrivalTime.Value);

                var result = await query.ToListAsync();
                _logger.LogInformation("Found {Count} flights matching the criteria.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for flights.");
                return StatusCode(500, "An error occurred while searching for flights.");
            }
        }

        /**
          * Updates an existing flight record using URL query parameters.
        */
        [HttpPut("Edit Flight")]
        public async Task<IActionResult> EditFlightViaQuery(
            [FromQuery] int id,
            [FromQuery] string? flightNumber,
            [FromQuery] string? airline,
            [FromQuery] string? departureAirport,
            [FromQuery] string? arrivalAirport,
            [FromQuery] DateTime? departureTime,
            [FromQuery] DateTime? arrivalTime,
            [FromQuery] FlightStatus? status)
        {
            _logger.LogInformation("Attempting to edit flight with ID {Id}", id);

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                _logger.LogWarning("Flight with ID {Id} not found", id);
                return NotFound();
            }

            if (flightNumber != null) flight.FlightNumber = flightNumber;
            if (airline != null) flight.Airline = airline;
            if (departureAirport != null) flight.DepartureAirport = departureAirport;
            if (arrivalAirport != null) flight.ArrivalAirport = arrivalAirport;
            if (departureTime.HasValue) flight.DepartureTime = departureTime.Value;
            if (arrivalTime.HasValue) flight.ArrivalTime = arrivalTime.Value;
            if (status.HasValue) flight.Status = status.Value;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully updated flight with ID {Id}", id);
                return Ok(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating flight with ID {Id}", id);
                return StatusCode(500, "An unexpected error occurred while updating the flight.");
            }
        }


        [HttpDelete("Delete by ID")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            _logger.LogInformation("Attempting to delete flight with ID {Id}", id);

            try
            {
                var flight = await _context.Flights.FindAsync(id);
                if (flight == null)
                {
                    _logger.LogWarning("Flight with ID {Id} not found", id);
                    return NotFound();
                }

                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted flight with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the flight with ID {Id}", id);
                return StatusCode(500, "An unexpected error occurred while deleting the flight.");
            }
        }

    }
}
