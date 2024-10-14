
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Tattoo.Management.Helpers
{

    
    /// <summary>
    /// Formatter helper for transform UI string element to defined formats
    /// </summary>
    public static class FormatterHelper
    {
        public static string ConvertToSuperscriptDecimal(decimal number)
        {
            // Diccionario para convertir números a superíndices
            var superScript = new Dictionary<char, char>
            {
                {'0', '⁰'}, {'1', '¹'}, {'2', '²'}, {'3', '³'}, {'4', '⁴'},
                {'5', '⁵'}, {'6', '⁶'}, {'7', '⁷'}, {'8', '⁸'}, {'9', '⁹'},
                {'.', '⋅'} // Usamos '⋅' como representación pequeña del punto decimal, si prefieres otro carácter ajusta aquí.
            };

            // Convertir el número a string para poder manipularlo
            string numberStr = number.ToString("0.##");

            // Encontrar la posición del punto decimal para separar la parte entera de la decimal
            int decimalPointIndex = numberStr.IndexOf('.');
            StringBuilder sb = new StringBuilder();

            if (decimalPointIndex == -1)
            {
                // Si no hay parte decimal, añadir ".⁰⁰"
                sb.Append(numberStr); // Parte entera
                sb.Append(superScript['.']); // Punto decimal pequeño
                sb.Append(superScript['0']); // Cero superíndice
                sb.Append(superScript['0']); // Cero superíndice
            }
            else
            {
                // Si hay parte decimal
                string integerPart = numberStr.Substring(0, decimalPointIndex);
                string decimalPart = numberStr.Substring(decimalPointIndex + 1);

                sb.Append(integerPart); // Parte entera normal
                sb.Append(superScript['.']); // Punto decimal pequeño

                // Convertir cada dígito de la parte decimal a superíndice y añadirlo al StringBuilder
                foreach (char digit in decimalPart)
                {
                    if (superScript.ContainsKey(digit))
                    {
                        sb.Append(superScript[digit]);
                    }
                }
            }

            return sb.ToString();
        }
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        //Adapt for this simple send message

        //public static List<MessageInfoDto> BuildProductOrderTable(List<ProductOrder> orders, string userName, string userPhone)
        //{
        //    var messageProviderList = new List<MessageInfo>();
        //    var groupedOrders = orders.GroupBy(o => o.Product.ProviderId);

        //    foreach (var group in groupedOrders)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendLine($"******************************");
        //        sb.AppendLine($"Notificación de Compra para Proveedor");
        //        sb.AppendLine($"Usuario: {userName}");
        //        sb.AppendLine($"Teléfono: {userPhone}");
        //        sb.AppendLine("");
        //        sb.AppendLine("Detalles de la Compra:");
        //        sb.AppendLine($"{"Producto",-20} {"#",10} {"Total",15}");

        //        decimal totalPorProveedor = 0M;

        //        foreach (var order in group)
        //        {
        //            string productName = order.Product.Name;
        //            int units = order.Units;
        //            decimal totalCost = order.Product.Price * units;
        //            totalPorProveedor += totalCost;

        //            sb.AppendLine($"{productName,-20} {units,5} {"$" + totalCost,15}");
        //        }

        //        sb.AppendLine("");
        //        sb.AppendLine($"Monto Total por Proveedor: {totalPorProveedor} CUP");
        //        var currentDate = DateTime.Now;
        //        var culture = new CultureInfo("es-ES");
        //        sb.AppendLine($"Fecha: {currentDate.ToString("dddd, dd MMMM yyyy", culture)}");
        //        sb.AppendLine($"******************************");

        //        messageProviderList.Add(new MessageInfo() { ProviderId = group.Key, Message = sb.ToString() });
        //    }

        //    return messageProviderList;
        //}


    }
}
