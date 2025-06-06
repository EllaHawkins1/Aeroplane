using Xunit;
using Moq;
using Aeroplane.Controllers;
using Aeroplane.Data;
using Aeroplane.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Aeroplane.Tests
{
    public class FlightControllerTests
    {
        [Fact]
        public async Task GetFlights_ReturnsAllFlights()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<FlightDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using var context = new FlightDbContext(options);
            context.Flights.Add(new Flight { FlightNumber = "NZ101", Airline = "Air NZ" });
            context.Flights.Add(new Flight { FlightNumber = "QA202", Airline = "Qantas" });
            context.SaveChanges();

            var logger = new Mock<ILogger<FlightController>>();
            var controller = new FlightController(context, logger.Object);

            // Act
            var result = await controller.GetFlights();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Flight>>>(result);
            var flights = Assert.IsAssignableFrom<IEnumerable<Flight>>(okResult.Value!);
            Assert.Equal(2, ((List<Flight>)flights).Count);
        }
    }
}
