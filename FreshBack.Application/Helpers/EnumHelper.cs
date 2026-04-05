using System.ComponentModel;

namespace FreshBack.Application.Helpers;

public static class EnumHelper
{
    public static string GetDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString())!;
        var attribute = (DescriptionAttribute)Attribute
            .GetCustomAttribute(field, typeof(DescriptionAttribute))!;

        return attribute == null ? value.ToString() : attribute.Description;
    }
}
