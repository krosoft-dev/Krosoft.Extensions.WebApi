using System.Data;
using Krosoft.Extensions.Samples.Library.Models;
using Krosoft.Extensions.Testing.Extensions;
using Krosoft.Extensions.Testing.Tests.Models;

namespace Krosoft.Extensions.Testing.Tests.Extensions;

[TestClass]
public class MockExtensionTests
{
    [TestMethod]
    public void VerifyNeverCallTest()
    {
        var foo = new Mock<IFoo>(MockBehavior.Loose);

        Check.That(foo).Verify(m => m.Call1(), Times.Never);
        Check.That(foo).Verify(m => m.Call2(), Times.Never);
    }

    [TestMethod]
    public void VerifyTest()
    {
        var foo = new Mock<IFoo>(MockBehavior.Loose);

        foo.Object.Call1();

        Check.That(foo).Verify(m => m.Call1(), Times.Once);
        Check.That(foo).Verify(m => m.Call2(), Times.Never);
    }

    [TestMethod]
    public void SetupWithData_Object()
    {
        var data = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Doe"
            },
            new User
            {
                Id = 2,
                FirstName = "Steve",
                LastName = "Smith"
            }
        };

        var mock = new Mock<IDbCommand>(MockBehavior.Strict);
        mock.SetupWithData(data);

        var resultats = GetResultats(mock.Object).ToList();

        Check.That(resultats).HasSize(2);
        Check.That(resultats.Select(x => x.Id)).ContainsExactly(1, 2);
        Check.That(resultats.Select(x => x.FirstName)).ContainsExactly("Joe", "Steve");
        Check.That(resultats.Select(x => x.LastName)).ContainsExactly("Doe", "Smith");
    }

    [TestMethod]
    public void SetupWithData_KeyValuePair()
    {
        var data = new List<List<KeyValuePair<string, object?>>>
        {
            new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("Id", 1),
                new KeyValuePair<string, object?>("FirstName", "Joe"),
                new KeyValuePair<string, object?>("LastName", "Doe")
            },
            new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("Id", 2),
                new KeyValuePair<string, object?>("FirstName", "Steve"),
                new KeyValuePair<string, object?>("LastName", "Smith")
            }
        };

        var mock = new Mock<IDbCommand>(MockBehavior.Strict);
        mock.SetupWithData(data);

        var resultats = GetResultats(mock.Object).ToList();

        Check.That(resultats).HasSize(2);
        Check.That(resultats.Select(x => x.Id)).ContainsExactly(1, 2);
        Check.That(resultats.Select(x => x.FirstName)).ContainsExactly("Joe", "Steve");
        Check.That(resultats.Select(x => x.LastName)).ContainsExactly("Doe", "Smith");
    }

    /// <summary>
    /// Représente un service métier faisant appel à la BDD.
    /// </summary>
    private static IEnumerable<User> GetResultats(IDbCommand cmd)
    {
        var movies = new List<User>();

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var movie = new User
            {
                Id = Convert.ToInt32(reader["Id"]),
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"]
            };

            movies.Add(movie);
        }

        return movies;
    }
}