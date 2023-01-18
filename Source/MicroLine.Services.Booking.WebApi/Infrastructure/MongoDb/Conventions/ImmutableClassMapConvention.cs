using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace MicroLine.Services.Booking.WebApi.Infrastructure.MongoDb.Conventions;

/// <summary>
/// Maps a fully immutable type. This will include anonymous types.
/// This class is refactored version of <see cref="ImmutableTypeClassMapConvention"/> to support private constructors
/// </summary>
public class ImmutableClassMapConvention : ConventionBase, IClassMapConvention
{
    /// <inheritdoc />
    public void Apply(BsonClassMap classMap)
    {
        var typeInfo = classMap.ClassType.GetTypeInfo();

        if (typeInfo.IsAbstract)
            return;

        if (typeInfo.GetConstructor(Type.EmptyTypes) != null)
            return;

        var propertyBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        var properties = typeInfo.GetProperties(propertyBindingFlags);

        if (properties.Any(CanWrite))
            return;

        var constructors = GetMappableConstructors(typeInfo, properties);

        if (constructors.Count == 0)
            return;

        foreach (var ctor in constructors)
            classMap.MapConstructor(ctor);


        MapProperties(classMap, properties);
    }

    private void MapProperties(BsonClassMap classMap, PropertyInfo[] properties)
    {
        foreach (var property in properties)
        {
            if (property.DeclaringType != classMap.ClassType)
                continue;

            if (!PropertyMatchesSomeCreatorParameter(classMap, property))
                continue;

            var memberMap = classMap.MapMember(property);

            if (classMap.IsAnonymous)
            {
                var defaultValue = memberMap.DefaultValue;
                memberMap.SetDefaultValue(defaultValue);
            }
        }
    }


    private bool CanWrite(PropertyInfo propertyInfo)
    {
        return propertyInfo.CanWrite && (propertyInfo.SetMethod?.IsPublic ?? false);
    }

    private ConstructorInfo[] GetUsableConstructors(TypeInfo typeInfo)
    {
        var constructorBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        return typeInfo.GetConstructors(constructorBindingFlags);
    }

    private List<ConstructorInfo> GetMappableConstructors(TypeInfo typeInfo, PropertyInfo[] properties)
    {
        var usableConstructors = GetUsableConstructors(typeInfo);

        var mappableConstructors = new List<ConstructorInfo>();

        foreach (var ctor in usableConstructors)
        {
            var parameters = ctor.GetParameters();

            var matches = parameters
                .GroupJoin(properties,
                    parameter => parameter.Name,
                    property => property.Name,
                    (parameter, props) => new { Parameter = parameter, Properties = props },
                    StringComparer.OrdinalIgnoreCase);

            if (matches.Any(m => m.Properties.Count() != 1))
                continue;


            mappableConstructors.Add(ctor);
        }

        return mappableConstructors;
    }

    private bool PropertyMatchesSomeCreatorParameter(BsonClassMap classMap, PropertyInfo propertyInfo)
    {
        foreach (var creatorMap in classMap.CreatorMaps)
        {
            if (creatorMap.MemberInfo is not ConstructorInfo constructorInfo)
                continue;

            if (PropertyMatchesSomeConstructorParameter(constructorInfo, propertyInfo))
                return true;
        }


        var classTypeInfo = classMap.ClassType.GetTypeInfo();
        var constructors = GetUsableConstructors(classTypeInfo);

        return constructors
            .Where(constructorInfo => classTypeInfo.IsAbstract ||
                                      constructorInfo.IsFamily || /* protected */
                                      constructorInfo.IsFamilyOrAssembly /* protected internal */)
            .Any(constructorInfo => PropertyMatchesSomeConstructorParameter(constructorInfo, propertyInfo));

    }

    private bool PropertyMatchesSomeConstructorParameter(ConstructorInfo constructorInfo, PropertyInfo propertyInfo)
    {
        return constructorInfo
            .GetParameters()
            .Any(parameter => string.Equals(propertyInfo.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

    }
}