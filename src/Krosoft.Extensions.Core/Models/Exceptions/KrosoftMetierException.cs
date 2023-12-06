using System.Net;
using Krosoft.Extensions.Core.Models.Exceptions.Http;

namespace Krosoft.Extensions.Core.Models.Exceptions;

/// <summary>
/// Exception à retourner en cas d'erreur technique soulevé par le code de l'application.
/// </summary>
public class KrosoftMetierException : HttpException
{
    public KrosoftMetierException(ISet<string> erreurs) : base(HttpStatusCode.BadRequest, erreurs.FirstOrDefault())
    {
        Erreurs = erreurs;
    }

    public KrosoftMetierException(string erreur) : this(new HashSet<string> { erreur })
    {
    }

    public IEnumerable<string> Erreurs { get; }
}