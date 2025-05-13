using System;
using System.ComponentModel;
using System.Reflection;


/* Class responsável por extender a classe Enum, permitindo que ela tenha uma descrição
para cada valor do enum. Isso é útil para exibir mensagens mais amigáveis ao usuário
em vez de exibir o valor bruto do enum.

O método GetDescription() verifica se o valor do enum possui um atributo de descrição
e, se sim, retorna essa descrição. Caso contrário, retorna o valor do enum como string.
*/ 

namespace DashboardTrilhaEsporte.Enums{
    public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field == null)
            return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}

}

