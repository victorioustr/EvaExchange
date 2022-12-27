using Business.Constants;
using Business.Handlers.UserPortfolioEvents.Commands;
using Business.Handlers.UserPortfolioEvents.Queries;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Business.Handlers.UserPortfolioEvents.Commands.CreateUserPortfolioEventCommand;
using static Business.Handlers.UserPortfolioEvents.Commands.DeleteUserPortfolioEventCommand;
using static Business.Handlers.UserPortfolioEvents.Commands.UpdateUserPortfolioEventCommand;
using static Business.Handlers.UserPortfolioEvents.Queries.GetUserPortfolioEventQuery;
using static Business.Handlers.UserPortfolioEvents.Queries.GetUserPortfolioEventsQuery;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class UserPortfolioEventHandlerTests
    {
        Mock<IUserPortfolioEventRepository> _userPortfolioEventRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _userPortfolioEventRepository = new Mock<IUserPortfolioEventRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task UserPortfolioEvent_GetQuery_Success()
        {
            //Arrange
            var query = new GetUserPortfolioEventQuery();

            _userPortfolioEventRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolioEvent, bool>>>())).ReturnsAsync(new UserPortfolioEvent()
//propertyler buraya yazılacak
//{																		
//UserPortfolioEventId = 1,
//UserPortfolioEventName = "Test"
//}
);

            var handler = new GetUserPortfolioEventQueryHandler(_userPortfolioEventRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.UserPortfolioEventId.Should().Be(1);

        }

        [Test]
        public async Task UserPortfolioEvent_GetQueries_Success()
        {
            //Arrange
            var query = new GetUserPortfolioEventsQuery();

            _userPortfolioEventRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserPortfolioEvent, bool>>>()))
                        .ReturnsAsync(new List<UserPortfolioEvent> { new UserPortfolioEvent { /*TODO:propertyler buraya yazılacak UserPortfolioEventId = 1, UserPortfolioEventName = "test"*/ } });

            var handler = new GetUserPortfolioEventsQueryHandler(_userPortfolioEventRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<UserPortfolioEvent>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task UserPortfolioEvent_CreateCommand_Success()
        {
            UserPortfolioEvent rt = null;
            //Arrange
            var command = new CreateUserPortfolioEventCommand();
            //propertyler buraya yazılacak
            //command.UserPortfolioEventName = "deneme";

            _userPortfolioEventRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolioEvent, bool>>>()))
                        .ReturnsAsync(rt);

            _userPortfolioEventRepository.Setup(x => x.Add(It.IsAny<UserPortfolioEvent>())).Returns(new UserPortfolioEvent());

            var handler = new CreateUserPortfolioEventCommandHandler(_userPortfolioEventRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioEventRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task UserPortfolioEvent_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateUserPortfolioEventCommand();
            //propertyler buraya yazılacak 
            //command.UserPortfolioEventName = "test";

            _userPortfolioEventRepository.Setup(x => x.Query())
                                           .Returns(new List<UserPortfolioEvent> { new UserPortfolioEvent { /*TODO:propertyler buraya yazılacak UserPortfolioEventId = 1, UserPortfolioEventName = "test"*/ } }.AsQueryable());

            _userPortfolioEventRepository.Setup(x => x.Add(It.IsAny<UserPortfolioEvent>())).Returns(new UserPortfolioEvent());

            var handler = new CreateUserPortfolioEventCommandHandler(_userPortfolioEventRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task UserPortfolioEvent_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateUserPortfolioEventCommand();
            //command.UserPortfolioEventName = "test";

            _userPortfolioEventRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolioEvent, bool>>>()))
                        .ReturnsAsync(new UserPortfolioEvent { /*TODO:propertyler buraya yazılacak UserPortfolioEventId = 1, UserPortfolioEventName = "deneme"*/ });

            _userPortfolioEventRepository.Setup(x => x.Update(It.IsAny<UserPortfolioEvent>())).Returns(new UserPortfolioEvent());

            var handler = new UpdateUserPortfolioEventCommandHandler(_userPortfolioEventRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioEventRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task UserPortfolioEvent_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteUserPortfolioEventCommand();

            _userPortfolioEventRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolioEvent, bool>>>()))
                        .ReturnsAsync(new UserPortfolioEvent { /*TODO:propertyler buraya yazılacak UserPortfolioEventId = 1, UserPortfolioEventName = "deneme"*/});

            _userPortfolioEventRepository.Setup(x => x.Delete(It.IsAny<UserPortfolioEvent>()));

            var handler = new DeleteUserPortfolioEventCommandHandler(_userPortfolioEventRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioEventRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

