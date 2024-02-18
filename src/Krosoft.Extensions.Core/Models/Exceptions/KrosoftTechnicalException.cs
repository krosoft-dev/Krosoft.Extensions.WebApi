using System.Net;
using Krosoft.Extensions.Core.Models.Exceptions.Http;

namespace Krosoft.Extensions.Core.Models.Exceptions;

/// <summary>
/// Exception à retourner en cas d'erreur technique soulevé par le code de l'application.
/// </summary>
public class KrosoftTechnicalException : HttpException
{ 
    public KrosoftTechnicalException(HashSet<string> erreurs) : base(HttpStatusCode.InternalServerError, erreurs.FirstOrDefault())
    {
        Erreurs = erreurs;
    }

    public KrosoftTechnicalException(string erreur) : this(new HashSet<string> { erreur })
    {
    }

    public IEnumerable<string> Erreurs { get; }
}