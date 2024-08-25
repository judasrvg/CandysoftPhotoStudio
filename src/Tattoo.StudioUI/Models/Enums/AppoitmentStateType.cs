using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tattoo.StudioUI.Models.Forms.Enum
{
    public enum AppoitmentStateType
    {
        [Description("Solicited")]
        Solicited = 0,
        [Description("Confirmed")]
        Confirmed = 1,
        //[Description("Executed")]
        //Executed = 2,
        [Description("Canceled")]
        Canceled = 3,
    }
}
