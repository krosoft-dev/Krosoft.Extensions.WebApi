//using Positive.Extensions.Core.Models.Exceptions;
//using Positive.Extensions.Samples.Api.Models.Commands;
//using Positive.Extensions.Samples.Api.Tests.Core;

//namespace Positive.Extensions.Samples.Api.Tests.Unit.Handlers.Commands;

//[TestClass]
//public class LogicielsUpdateCommandHandlerTests : SampleBaseTest<Startup>
//{
//    [TestMethod]
//    public void HandleEmptyTest()
//    {
//        var serviceProvider = CreateServiceCollection();

//        var command = new LogicielUpdateCommand();

//        Check.ThatCode(async () => { await SendCommandAsync<MediatR.Unit>(serviceProvider, command); })
//             .Throws<PositiveMetierException>()
//             .WhichMember(c => c.Erreurs)
//             .ContainsExactly("'Id' is mandatory.");
//    }
//}

