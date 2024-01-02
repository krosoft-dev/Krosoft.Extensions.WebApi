//using Krosoft.Extensions.Core.Interfaces;
//using Krosoft.Extensions.Data.EntityFramework.Audits.Services;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;
//using Krosoft.Extensions.Identity.Abstractions.Interfaces;

//namespace Krosoft.Extensions.Data.EntityFramework.Identity.Services;

//public class HttpDbContextSettingsProvider : HttpDbContextSettingsProvider2, IDbContextSettingsProvider
//{
//    private readonly IIdentityService _identityService;

//    public HttpDbContextSettingsProvider(IIdentityService identityService,
//                                         IDateTimeService dateTimeService) : base(dateTimeService)
//    {
//        _identityService = identityService;
//    }

//    public string GetTenantId() => _identityService.GetTenantId()!;

//    public string GetUtilisateurId() => _identityService.GetId()!;
//}