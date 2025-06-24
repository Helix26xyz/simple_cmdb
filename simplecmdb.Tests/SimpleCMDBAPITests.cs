using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplecmdb.ApiService;
using simplecmdb.ApiService.src;
using simplecmdb.SharedModels.models;
using Xunit;
using simplecmdb.SharedModels.storage;


namespace simplecmdb.ApiService.Tests
{
    public class SimpleCMDBsControllerTests
    {
        private readonly AppDbContext _context;
        private readonly SimpleCMDBsController _controller;

        public SimpleCMDBsControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _controller = new SimpleCMDBsController(_context);

            // Seed the database with test data
            _context.SimpleCMDBs.AddRange(new List<SimpleCMDB>
                {
                    new SimpleCMDB { Id = Guid.NewGuid(), Name = "SimpleCMDB1" },
                    new SimpleCMDB { Id = Guid.NewGuid(), Name = "SimpleCMDB2" }
                });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetSimpleCMDBs_ReturnsAllSimpleCMDBs()
        {
            // Act
            var result = await _controller.GetSimpleCMDBs();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SimpleCMDB>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SimpleCMDB>>(actionResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetSimpleCMDB_ReturnsSimpleCMDB()
        {
            // Arrange
            var webhookId = _context.SimpleCMDBs.First().Id;

            // Act
            var result = await _controller.GetSimpleCMDB(webhookId);

            // Assert
            //var actionResult = Assert.IsType<ActionResult<SimpleCMDB>>(result);
            //var model = Assert.IsAssignableFrom<IEnumerable<SimpleCMDB>>(actionResult.Value);
            Assert.Equal(webhookId, result.Value.Id);
        }

        [Fact]
        public async Task GetSimpleCMDB_ReturnsNotFound_WhenSimpleCMDBDoesNotExist()
        {
            // Arrange
            var webhookId = Guid.NewGuid();

            // Act
            var result = await _controller.GetSimpleCMDB(webhookId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SimpleCMDB>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostSimpleCMDB_CreatesSimpleCMDB()
        {
            // Arrange
            var webhook = new SimpleCMDB { Id = Guid.NewGuid(), Name = "SimpleCMDB3" };
            var previousCount = _context.SimpleCMDBs.Count();
            // Act
            var result = await _controller.PostSimpleCMDB(webhook);

            // Assert
            var actionResult = Assert.IsType<ActionResult<SimpleCMDB>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var model = Assert.IsAssignableFrom<SimpleCMDB>(createdAtActionResult.Value);
            Assert.Equal(webhook.Id, model.Id);
            Assert.Equal(previousCount + 1, _context.SimpleCMDBs.Count());
        }

        [Fact]
        public async Task PutSimpleCMDB_UpdatesSimpleCMDB()
        {
            // Arrange
            var webhookId = _context.SimpleCMDBs.First().Id;
            var webhook = new SimpleCMDB { Id = webhookId, Name = "UpdatedSimpleCMDB" };

            // Act
            var result = await _controller.PutSimpleCMDB(webhookId, webhook);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedSimpleCMDB = await _context.SimpleCMDBs.FindAsync(webhookId);
            Assert.Equal("UpdatedSimpleCMDB", updatedSimpleCMDB.Name);
        }

        [Fact]
        public async Task DeleteSimpleCMDB_DeletesSimpleCMDB()
        {
            // Arrange
            var webhookId = _context.SimpleCMDBs.First().Id;
            var previousCount = _context.SimpleCMDBs.Count();

            // Act
            var result = await _controller.DeleteSimpleCMDB(webhookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.SimpleCMDBs.FindAsync(webhookId));
            Assert.Equal(previousCount - 1, _context.SimpleCMDBs.Count());
        }
    }
}
