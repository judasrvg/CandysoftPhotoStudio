using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Enum
{
    public enum TransactionType
    {
        Expense = 1,
        Income = 2
    }

    public enum TransactionGroup
        {
            [Description("Paquete Foto")]
            PAQUETE_FOTOGRAFIA,
    
            [Description("Venta")]
            VENTA,
    
            [Description("Servicio Impresión")]
            SERVICIO_IMPRESION,
    
            [Description("Servicio Reparación")]
            SERVICIO_REPARACION,
    
            [Description("Servicio Fotografía_Impresión")]
            SERVICIO_FOTOGRAFIA_IMPRESION,
    
            [Description("Servicio Fotografía_Digital")]
            SERVICIO_FOTOGRAFIA_DIGITAL,
    
            [Description("Otros")]
            OTROS,
             
            [Description("Ajuste")]
            AJUSTE,
        
            [Description("Ajuste Reparacion")]
            AJUSTE_REPARACION,

            [Description("Ajuste VENTA")]
            AJUSTE_VENTA
    }

}
