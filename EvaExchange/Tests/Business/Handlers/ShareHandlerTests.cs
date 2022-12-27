using AutoMapper;
using Business.Constants;
using Business.Handlers.Shares.Commands;
using Business.Handlers.Shares.Queries;
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
using static Business.Handlers.Shares.Commands.CreateShareCommand;
using static Business.Handlers.Shares.Commands.DeleteShareCommand;
using static Business.Handlers.Shares.Commands.UpdateShareCommand;
using static Business.Handlers.Shares.Queries.GetShareQuery;
using static Business.Handlers.Shares.Queries.GetSharesQuery;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ShareHandlerTests
    {
        Mock<IShareRepository> _shareRepository;
        Mock<IMediator> _mediator;
        Mock<IMapper> _mapper;
        [SetUp]
        public void Setup()
        {
            _shareRepository = new Mock<IShareRepository>();
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Share_GetQuery_Success()
        {
            //Arrange
            var query = new GetShareQuery();

            var id = Guid.NewGuid();

            query.Id = id;

            _shareRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Share, bool>>>())).ReturnsAsync(new Share
            {
                Id = id,
                Code = "ABC",
                Name = "Test",
                Rate = 100.5m,
                CreatedDate = DateTime.Now,
                CreatedUser = 1
            }
);

            var handler = new GetShareQueryHandler(_shareRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            x.Data.Id.Should().Be(id);

        }

        [Test]
        public async Task Share_GetQueries_Success()
        {
            //Arrange
            var query = new GetSharesQuery();

            _shareRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Share, bool>>>()))
                        .ReturnsAsync(new List<Share> {
                            new () {
                                Id = Guid.NewGuid(),
                                Code = "ABC",
                                Name = "Test",
                                Rate = 100.5m,
                            },
                            new () {
                                Id = Guid.NewGuid(),
                                Code = "CBA",
                                Name = "Test",
                                Rate = 200.5m,
                            }
                        });

            var handler = new GetSharesQueryHandler(_shareRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Share>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Share_CreateCommand_Success()
        {
            Share rt = null;
            //Arrange
            var command = new CreateShareCommand();
            command.Code = "ABC";
            command.Name = "Test";
            command.Rate = 300.5m;

            _shareRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Share, bool>>>()))
                        .ReturnsAsync(rt);

            _shareRepository.Setup(x => x.Add(It.IsAny<Share>())).Returns(new Share());

            var handler = new CreateShareCommandHandler(_shareRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Share_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateShareCommand();
            command.Code = "ABC";
            command.Name = "Test";
            command.Rate = 300.5m;

            _shareRepository.Setup(x => x.Query())
                                           .Returns(new List<Share> { new Share {
                                               Id = Guid.NewGuid(),
                                               Code = "ABC",
                                               Name = "Test",
                                               Rate = 100.5m, } }.AsQueryable());

            _shareRepository.Setup(x => x.Add(It.IsAny<Share>())).Returns(new Share());

            var handler = new CreateShareCommandHandler(_shareRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Share_UpdateCommand_Success()
        {
            //Arrange
            var id = Guid.NewGuid();

            var command = new UpdateShareCommand();
            command.Id = id;
            command.Code = "ABC";
            command.Name = "New Name";
            command.Rate = 300.5m;

            _shareRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Share, bool>>>()))
                        .ReturnsAsync(new Share
                        {
                            Id = id,
                            Code = "ABC",
                            Name = "Old Name",
                            Rate = 100.5m,
                        });

            _shareRepository.Setup(x => x.Update(It.IsAny<Share>())).Returns(new Share());

            var handler = new UpdateShareCommandHandler(_shareRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Share_DeleteCommand_Success()
        {
            //Arrange
            var id = Guid.NewGuid();

            var command = new DeleteShareCommand();
            command.Id = id;

            _shareRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Share, bool>>>()))
                        .ReturnsAsync(new Share
                        {
                            Id = id,
                            Code = "ABC",
                            Name = "Test",
                            Rate = 100.5m,
                        });

            _shareRepository.Setup(x => x.Delete(It.IsAny<Share>()));

            var handler = new DeleteShareCommandHandler(_shareRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _shareRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

