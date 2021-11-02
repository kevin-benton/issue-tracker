using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

using IssueTracker.Core.Application.Commands;

namespace IssueTracker.Core.Tests.Application.Commands
{
    [TestClass]
    public class CreateIssueCommandValidatorTests
    {
        private CreateIssueCommandValidator TestObject { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            TestObject = new CreateIssueCommandValidator();
        }

        [TestMethod]
        public async Task
            CreateIssueCommandValidator_Should_Validate_When_Command_Has_Valid_Title_Description_And_Priority()
        {
            var command = new CreateIssueCommand
            {
                Title = "title",
                Description = "description",
                Priority = 4
            };

            var validationResult = await TestObject.ValidateAsync(command);

            validationResult.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public async Task
            CreateIssueCommandValidator_Should_Not_Validate_When_Command_Has_Invalid_Title()
        {
            var command = new CreateIssueCommand
            {
                Title = string.Empty,
                Description = "description",
                Priority = 4
            };

            var validationResult = await TestObject.ValidateAsync(command);

            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public async Task
            CreateIssueCommandValidator_Should_Not_Validate_When_Command_Has_Invalid_Description()
        {
            var command = new CreateIssueCommand
            {
                Title = "title",
                Description = string.Empty,
                Priority = 4
            };

            var validationResult = await TestObject.ValidateAsync(command);

            validationResult.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public async Task
            CreateIssueCommandValidator_Should_Not_Validate_When_Command_Has_Invalid_Priority()
        {
            var command = new CreateIssueCommand
            {
                Title = "title",
                Description = "description",
                Priority = 18
            };

            var validationResult = await TestObject.ValidateAsync(command);

            validationResult.IsValid.Should().BeFalse();
        }
    }

    [TestClass]
    public class CreateIssueCommandHandlerTests
    {
        private CreateIssueCommandHandler TestObject { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            TestObject = new CreateIssueCommandHandler();
        }

        [TestMethod]
        public async Task CreateIssueCommandHandler_Should_Create_An_IssueCreatedEvent_When_Handled()
        {
            var command = new CreateIssueCommand
            {
                AggregateId = Guid.NewGuid().ToString(),
                Title = "title",
                Description = "description",
                Priority = 18
            };

            var eventsEnumerable = await TestObject.Handle(command, CancellationToken.None);
            var events = eventsEnumerable.ToList();

            events.Count.Should().Be(1);
        }
    }
}
