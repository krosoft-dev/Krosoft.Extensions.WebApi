//using AutoMapper;
//using Krosoft.Extensions.Core.Tools;
//using Krosoft.Extensions.Data.Abstractions.Interfaces;
//using Krosoft.Extensions.Samples.Library.Models.Commands;
//using Krosoft.Extensions.Samples.Library.Models.Entities;

//namespace Krosoft.Extensions.Samples.DotNet8.Api.Handlers.Commands.Positive.Extensions.Samples.Api.Handlers.Commands;

//internal class LogicielCreateCommandHandler : CreateCommandHandler<LogicielCreateCommand, Logiciel, Guid>
//{
//    private readonly IEmailService _emailService;
//    private readonly IEventService _eventService;

//    public LogicielCreateCommandHandler(ILogger<CreateCommandHandler<LogicielCreateCommand, Logiciel, Guid>> logger,
//                                        IWriteRepository<Logiciel> repository,
//                                        IMapper mapper,
//                                        IUnitOfWork unitOfWork,
//                                        IEmailService emailService,
//                                        IEventService eventService)
//        : base(logger, repository, mapper, unitOfWork)
//    {
//        _emailService = emailService;
//        _eventService = eventService;
//    }

//    protected override Task AfterMapAsync(Logiciel entity, CancellationToken cancellationToken)
//    {
//        entity.Id = SequentialGuid.NewGuid();
//        return base.AfterMapAsync(entity, cancellationToken);
//    }

//    protected override async Task AfterSaveChangesAsync(Logiciel entity, CancellationToken cancellationToken)
//    {
//        _eventService.Publish(o => new UpdateStatLogicielEvent(o), cancellationToken);
//        await _emailService.SendAsync("krobert@positivethinking.tech", "Création", "Votre logiciel a bien été créé.", cancellationToken);
//    }

//    protected override Guid GetId(Logiciel entity) => entity.Id;

//    protected override string GetMessage(LogicielCreateCommand request, string entityName) => $"Création du logiciel {request.Nom} par {request.UtilisateurCourantId}";
//}