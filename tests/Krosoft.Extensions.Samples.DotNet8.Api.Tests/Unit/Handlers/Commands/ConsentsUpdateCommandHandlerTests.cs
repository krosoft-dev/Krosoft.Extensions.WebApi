//using Krosoft.Extensions.Core.Models.Exceptions;
//using Krosoft.Extensions.Samples.Api.Models.Commands;
//using Krosoft.Extensions.Samples.Api.Tests.Core;

//namespace Krosoft.Extensions.Samples.Api.Tests.Unit.Handlers.Commands;

//[TestClass]
//public class LogicielsUpdateCommandHandlerTests : SampleBaseTest<Startup>
//{
//    [TestMethod]
//    public void HandleEmptyTest()
//    {
//        var serviceProvider = CreateServiceCollection();

//        var command = new LogicielUpdateCommand();

//        Check.ThatCode(async () => { await SendCommandAsync<MediatR.Unit>(serviceProvider, command); })
//             .Throws<KrosoftMetierException>()
//             .WhichMember(c => c.Erreurs)
//             .ContainsExactly("'Id' is mandatory.");
//    }
//}

