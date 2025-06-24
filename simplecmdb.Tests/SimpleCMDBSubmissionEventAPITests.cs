using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplecmdb.ApiService.src;
using simplecmdb.SharedModels.models;
using simplecmdb.SharedModels.storage;
using Xunit;

namespace simplecmdb.ApiService.Tests
{
    public class SimpleCMDBSubmissionEventAPITests
    {
        private readonly AppDbContext _context;
        private readonly SimpleCMDBEventsSubmissionController _controller;

        public SimpleCMDBSubmissionEventAPITests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _controller = new SimpleCMDBEventsSubmissionController(_context);

            // Seed the database with test data
            var webhook = new SimpleCMDB
            {
                Id = Guid.NewGuid(),
                Name = "TestSimpleCMDB",
                Slug = "test-webhook",
                Owner = "test-org",
                Project = "test-project",
                Status = SimpleCMDBStatus.Enabled
            };
            _context.SimpleCMDBs.Add(webhook);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetSimpleCMDBEvent_CreatesAndReturnsSimpleCMDBEvent()
        {
            // Act
            var result = await _controller.GetSimpleCMDBEvent("test-org", "test-project", "test-webhook");

            // Assert
            var actionResult = Assert.IsType<ActionResult<SimpleCMDBEvent>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var webhookEvent = Assert.IsType<SimpleCMDBEvent>(createdResult.Value);
            Assert.Equal(SimpleCMDBEventStatus.New, webhookEvent.Status);
            Assert.Equal(SimpleCMDBEventSubStatus.Pending, webhookEvent.SubStatus);
        }

        [Fact]
        public async Task GetSimpleCMDBEvent_ReturnsNotFound_WhenSimpleCMDBDoesNotExist()
        {
            // Act
            var result = await _controller.GetSimpleCMDBEvent("invalid-org", "invalid-project", "invalid-webhook");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostSimpleCMDBEvent_CreatesAndReturnsSimpleCMDBEvent()
        {
            // Arrange
            var payload = new { message = "Test payload" };

            // Act
            var result = await _controller.PostSimpleCMDBEvent("test-org", "test-project", "test-webhook", payload);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SimpleCMDBEvent>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var webhookEvent = Assert.IsType<SimpleCMDBEvent>(createdResult.Value);
            Assert.Equal(SimpleCMDBEventStatus.New, webhookEvent.Status);
            Assert.Equal(SimpleCMDBEventSubStatus.Pending, webhookEvent.SubStatus);
            Assert.Contains("Test payload", webhookEvent.Payload);
        }

        [Fact]
        public async Task PostSimpleCMDBEvent_ReturnsNotFound_WhenSimpleCMDBDoesNotExist()
        {
            // Arrange
            var payload = new { message = "Test payload" };

            // Act
            var result = await _controller.PostSimpleCMDBEvent("invalid-org", "invalid-project", "invalid-webhook", payload);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
