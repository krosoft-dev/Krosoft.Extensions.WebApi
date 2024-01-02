//using Krosoft.Extensions.Core.Interfaces;
//using Krosoft.Extensions.Data.EntityFramework.Interfaces;

//namespace Krosoft.Extensions.Data.EntityFramework.Audits.Services;

//public class HttpAuditableDbContextProvider : IAuditableDbContextProvider
//{
//    private readonly IDateTimeService _dateTimeService;

//    public HttpDbContextSettingsProvider2(IDateTimeService dateTimeService)
//    {
//        _dateTimeService = dateTimeService;
//    }

//    public DateTime GetNow() => _dateTimeService.Now;
//    public string GetUtilisateurId() => throw new NotImplementedException();
//}