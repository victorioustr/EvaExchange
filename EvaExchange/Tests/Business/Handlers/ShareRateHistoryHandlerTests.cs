using AutoMapper;
using Business.Constants;
using Business.Handlers.ShareRateHistories.Commands;
using Business.Handlers.ShareRateHistories.Queries;
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
using static Business.Handlers.ShareRateHistories.Commands.CreateShareRateHistoryCommand;
using static Business.Handlers.ShareRateHistories.Commands.DeleteShareRateHistoryCommand;
using static Business.Handlers.ShareRateHistories.Commands.UpdateShareRateHistoryCommand;
using static Business.Handlers.ShareRateHistories.Queries.GetShareRateHistoriesQuery;
using static Business.Handlers.ShareRateHistories.Queries.GetShareRateHistoryQuery;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ShareRateHistoryHandlerTests
    {
        Mock<IShareRateHistoryRepository> _shareRateHistoryRepository;
        Mock<IMediator> _mediator;
        Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _shareRateHistoryRepository = new Mock<IShareRateHistoryRepository>();
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShareRateHistory_GetQuery_Success()
        {
            //Arrange
            var query = new GetShareRateHistoryQuery();

            _shareRateHistoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ShareRateHistory, bool>>>())).ReturnsAsync(new ShareRateHistory()
//propertyler buraya yazılacak
//{																		
//ShareRateHistoryId = 1,
//ShareRateHistoryName = "Test"
//}
);

            var handler = new GetShareRateHistoryQueryHandler(_shareRateHistoryRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ShareRateHistoryId.Should().Be(1);

        }

        [Test]
        public async Task ShareRateHistory_GetQueries_Success()
        {
            //Arrange
            var query = new GetShareRateHistoriesQuery();

            _shareRateHistoryRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<ShareRateHistory, bool>>>()))
                        .ReturnsAsync(new List<ShareRateHistory> { new ShareRateHistory { /*TODO:propertyler buraya yazılacak ShareRateHistoryId = 1, ShareRateHistoryName = "test"*/ } });

            var handler = new GetShareRateHistoriesQueryHandler(_shareRateHistoryRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<ShareRateHistory>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task ShareRateHistory_CreateCommand_Success()
        {
            ShareRateHistory rt = null;
            //Arrange
            var command = new CreateShareRateHistoryCommand();
            //propertyler buraya yazılacak
            //command.ShareRateHistoryName = "deneme";

            _shareRateHistoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ShareRateHistory, bool>>>()))
                        .ReturnsAsync(rt);

            _shareRateHistoryRepository.Setup(x => x.Add(It.IsAny<ShareRateHistory>())).Returns(new ShareRateHistory());

            var handler = new CreateShareRateHistoryCommandHandler(_shareRateHistoryRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRateHistoryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task ShareRateHistory_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateShareRateHistoryCommand();
            //propertyler buraya yazılacak 
            //command.ShareRateHistoryName = "test";

            _shareRateHistoryRepository.Setup(x => x.Query())
                                           .Returns(new List<ShareRateHistory> { new ShareRateHistory { /*TODO:propertyler buraya yazılacak ShareRateHistoryId = 1, ShareRateHistoryName = "test"*/ } }.AsQueryable());

            _shareRateHistoryRepository.Setup(x => x.Add(It.IsAny<ShareRateHistory>())).Returns(new ShareRateHistory());

            var handler = new CreateShareRateHistoryCommandHandler(_shareRateHistoryRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task ShareRateHistory_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateShareRateHistoryCommand();
            //command.ShareRateHistoryName = "test";

            _shareRateHistoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ShareRateHistory, bool>>>()))
                        .ReturnsAsync(new ShareRateHistory { /*TODO:propertyler buraya yazılacak ShareRateHistoryId = 1, ShareRateHistoryName = "deneme"*/ });

            _shareRateHistoryRepository.Setup(x => x.Update(It.IsAny<ShareRateHistory>())).Returns(new ShareRateHistory());

            var handler = new UpdateShareRateHistoryCommandHandler(_shareRateHistoryRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRateHistoryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task ShareRateHistory_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteShareRateHistoryCommand();

            _shareRateHistoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ShareRateHistory, bool>>>()))
                        .ReturnsAsync(new ShareRateHistory { /*TODO:propertyler buraya yazılacak ShareRateHistoryId = 1, ShareRateHistoryName = "deneme"*/});

            _shareRateHistoryRepository.Setup(x => x.Delete(It.IsAny<ShareRateHistory>()));

            var handler = new DeleteShareRateHistoryCommandHandler(_shareRateHistoryRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRateHistoryRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

