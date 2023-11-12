using Krosoft.Extensions.Samples.Library.Models;

namespace Krosoft.Extensions.Samples.Library.Factories;

public static class UtilisateurFactory
{
    public static Utilisateur CreateUtilisateur(string nom, string prenom) =>
        new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            DateModification = DateTime.Now
        };
}