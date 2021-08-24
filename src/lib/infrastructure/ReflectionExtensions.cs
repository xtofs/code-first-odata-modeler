using System.Diagnostics.CodeAnalysis;

namespace System;

static class ReflectionExtensions
{
    public static Type Flatten(this Type type) => type.IsCollection(out var elem) ? elem : type;

    /// <summary>
    /// tests if the given type is a strongly typed collection
    /// i.e. if it derices from IEnumerable<T>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static bool IsCollection(this Type type, [MaybeNullWhen(false)] out Type elementType)
    {

        if (type.IsGenericType && type.IsAssignableTo(Base.MakeGenericType(type.GetGenericArguments())))
        {
            elementType = type.GetGenericArguments()[0];
            return true;
        }
        elementType = default;
        return false;
    }

    private static Type Base = typeof(IEnumerable<>);
}
