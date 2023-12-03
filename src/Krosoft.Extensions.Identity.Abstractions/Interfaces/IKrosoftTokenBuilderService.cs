using Krosoft.Extensions.Core.Models;

namespace Krosoft.Extensions.Identity.Abstractions.Interfaces;

public interface IKrosoftTokenBuilderService
{
    KrosoftToken Build();
}