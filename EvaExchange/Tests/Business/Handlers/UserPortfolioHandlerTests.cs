using Business.Constants;
using Business.Handlers.UserPortfolios.Commands;
using Business.Handlers.UserPortfolios.Queries;
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
using static Business.Handlers.UserPortfolios.Commands.CreateUserPortfolioCommand;
using static Business.Handlers.UserPortfolios.Commands.DeleteUserPortfolioCommand;
using static Business.Handlers.UserPortfolios.Commands.UpdateUserPortfolioCommand;
using static Business.Handlers.UserPortfolios.Queries.GetUserPortfolioQuery;
using static Business.Handlers.UserPortfolios.Queries.GetUserPortfoliosQuery;

namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class UserPortfolioHandlerTests
    {
        Mock<IUserPortfolioRepository> _userPortfolioRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _userPortfolioRepository = new Mock<IUserPortfolioRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task UserPortfolio_GetQuery_Success()
        {
            //Arrange
            var query = new GetUserPortfolioQuery();

            _userPortfolioRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolio, bool>>>())).ReturnsAsync(new UserPortfolio()
//propertyler buraya yazılacak
//{																		
//UserPortfolioId = 1,
//UserPortfolioName = "Test"
//}
);

            var handler = new GetUserPortfolioQueryHandler(_userPortfolioRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.UserPortfolioId.Should().Be(1);

        }

        [Test]
        public async Task UserPortfolio_GetQueries_Success()
        {
            //Arrange
            var query = new GetUserPortfoliosQuery();

            _userPortfolioRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserPortfolio, bool>>>()))
                        .ReturnsAsync(new List<UserPortfolio> { new UserPortfolio { /*TODO:propertyler buraya yazılacak UserPortfolioId = 1, UserPortfolioName = "test"*/ } });

            var handler = new GetUserPortfoliosQueryHandler(_userPortfolioRepository.Object);

            //Act
            var x = await handler.Handle(query, new CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<UserPortfolio>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task UserPortfolio_CreateCommand_Success()
        {
            UserPortfolio rt = null;
            //Arrange
            var command = new CreateUserPortfolioCommand();
            //propertyler buraya yazılacak
            //command.UserPortfolioName = "deneme";

            _userPortfolioRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolio, bool>>>()))
                        .ReturnsAsync(rt);

            _userPortfolioRepository.Setup(x => x.Add(It.IsAny<UserPortfolio>())).Returns(new UserPortfolio());

            var handler = new CreateUserPortfolioCommandHandler(_userPortfolioRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task UserPortfolio_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateUserPortfolioCommand();
            //propertyler buraya yazılacak 
            //command.UserPortfolioName = "test";

            _userPortfolioRepository.Setup(x => x.Query())
                                           .Returns(new List<UserPortfolio> { new UserPortfolio { /*TODO:propertyler buraya yazılacak UserPortfolioId = 1, UserPortfolioName = "test"*/ } }.AsQueryable());

            _userPortfolioRepository.Setup(x => x.Add(It.IsAny<UserPortfolio>())).Returns(new UserPortfolio());

            var handler = new CreateUserPortfolioCommandHandler(_userPortfolioRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task UserPortfolio_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateUserPortfolioCommand();
            //command.UserPortfolioName = "test";

            _userPortfolioRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolio, bool>>>()))
                        .ReturnsAsync(new UserPortfolio { /*TODO:propertyler buraya yazılacak UserPortfolioId = 1, UserPortfolioName = "deneme"*/ });

            _userPortfolioRepository.Setup(x => x.Update(It.IsAny<UserPortfolio>())).Returns(new UserPortfolio());

            var handler = new UpdateUserPortfolioCommandHandler(_userPortfolioRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task UserPortfolio_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteUserPortfolioCommand();

            _userPortfolioRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPortfolio, bool>>>()))
                        .ReturnsAsync(new UserPortfolio { /*TODO:propertyler buraya yazılacak UserPortfolioId = 1, UserPortfolioName = "deneme"*/});

            _userPortfolioRepository.Setup(x => x.Delete(It.IsAny<UserPortfolio>()));

            var handler = new DeleteUserPortfolioCommandHandler(_userPortfolioRepository.Object);
            var x = await handler.Handle(command, new CancellationToken());

            _userPortfolioRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

