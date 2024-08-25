using Tattoo.InkVibesTattoo.Models.Forms;

namespace Tattoo.InkVibesTattoo.Helpers
{
    public interface IGlobalState
    {
        void SetGlobalBasicValues(ConfigValueDto globaltiktokInstagram, ConfigValueDto globalphoneFacebook, ConfigValueDto globalemailAdress, ConfigValueDto globalLocation);

    }
}
