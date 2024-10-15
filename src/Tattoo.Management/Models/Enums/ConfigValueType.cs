using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tattoo.Management.Models.Forms.Enum
{
    public enum CacheValueType
    {
        Tattoo,
        Reservation,
        TattooStyle,
        TattooLocation,
        TattooGender,
        AskAnswerFAQ,
        StudioLocation,
        EmailAddress,
        PhoneFacebook,
        TikTokInstagram,
        AskAnswerFAQEspannol,
        OfferChild,
        OfferWedding,
        Offer15,
        OfferPegnant,
        OfferCasual,
        OfferIndividual
    }

    public enum CacheValueTypeWithDescript : int
    {
        OfferChild = 11,
        OfferWedding = 12,
        OfferQuinceanera = 13,
        OfferPregnant = 14,
        OfferCasual = 15,
        OfferIndividual = 16
    }

    public enum ProductType
    {
        FixedAsset = 1,
        Merchandise = 2,
        RawMaterial = 3
    }

    public enum TransactionType
    {
        Expense = 1,
        Income = 2
    }


public enum TransactionGroup
    {
        [Description("Paquete de Fotografía")]
        PAQUETE_FOTOGRAFIA,

        [Description("Venta")]
        VENTA,

        [Description("Servicio de Impresión")]
        SERVICIO_IMPRESION,

        [Description("Servicio de Reparación")]
        SERVICIO_REPARACION,

        [Description("Servicio de Fotografía e Impresión")]
        SERVICIO_FOTOGRAFIA_IMPRESION,

        [Description("Servicio Plasticado")]
        SERVICIO_PLASTICADO,

        [Description("Servicio Engargolado")]
        SERVICIO_ENGARGOLADO,

        [Description("Otros")]
        OTROS,

        
    }

}
