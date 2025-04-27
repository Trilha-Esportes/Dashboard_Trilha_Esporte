

using System;
using System.ComponentModel;
using System.Reflection;

namespace DashboardTrilhasEsporte.Enums
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }

}

