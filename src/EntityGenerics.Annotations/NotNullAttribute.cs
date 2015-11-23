using System;

namespace EntityGenerics.Annotations
{
    /// <summary>
    /// Indicates that the value of marked element could never be <c>null</c>
    /// </summary>
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}