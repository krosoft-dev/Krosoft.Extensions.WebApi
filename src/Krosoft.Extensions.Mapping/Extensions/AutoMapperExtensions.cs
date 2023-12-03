using AutoMapper;
#if NET7_0_OR_GREATER
using System.Reflection;
using AutoMapper.Configuration;
#endif

namespace Krosoft.Extensions.Mapping.Extensions;

public static class AutoMapperExtensions
{
    public static async Task<TDestination> Map<TSource, TDestination>(this Task<TSource> task,
                                                                      IMapper mapper)
    {
        var item = await task;

        return mapper.Map<TDestination>(item);
    }
#if NET7_0_OR_GREATER
    private static readonly PropertyInfo TypeMapActionsProperty = typeof(TypeMapConfiguration).GetProperty("TypeMapActions", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException();

    public static void ForAllOtherMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Action<IMemberConfigurationExpression<TSource, TDestination, object>> memberOptions)
    {
        var typeMapConfiguration = (TypeMapConfiguration)expression;

        var typeMapActions = TypeMapActionsProperty.GetValue(typeMapConfiguration) as List<Action<TypeMap>>;
        if (typeMapActions != null)
        {
            typeMapActions.Add(typeMap =>
            {
                var destinationTypeDetails = typeMap.DestinationTypeDetails;
                if (destinationTypeDetails != null)
                {
                    foreach (var accessor in destinationTypeDetails.WriteAccessors.Where(m => typeMapConfiguration.GetDestinationMemberConfiguration(m) == null))
                    {
                        expression.ForMember(accessor.Name, memberOptions);
                    }
                }
            });
        }
    }
#endif
}