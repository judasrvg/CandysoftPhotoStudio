using Tattoo.InkVibesTattoo.Models.Forms;

namespace Tattoo.InkVibesTattoo.Helpers
{
    public partial class GlobalState : IGlobalState
    {
        public bool GlobalFirstLoad { get; private set; } = true;
        public int GlobalCountSolicited { get; private set; }
        public bool GlobalShowDetail { get; private set; }
        public bool GlobalIsSuccessDialogRequest { get; set; } = false;
        public bool GlobalIsPartialSuccessDialogRequest { get; set; } = false;
        public string GlobalDialogRequestMessage { get; set; } = "";
        public bool GlobalIsEventDeletedDialog { get; set; } = false;
        public string GlobalLanguage { get; set; } = "en";

        public event Action OnGlobalBasicValuesChanged;
        public event Action OnGlobalCountSolicitedChanged;
        public event Action OnGlobalLanguageChanged;
        public ConfigValueDto GlobaltiktokInstagram { get; private set; } = new ConfigValueDto();
        public ConfigValueDto GlobalphoneFacebook { get; private set; } = new ConfigValueDto();
        public ConfigValueDto GlobalemailAdress { get; private set; } = new ConfigValueDto();
        public ConfigValueDto GlobalLocation { get; private set; } = new ConfigValueDto();
        public void SetGlobalBasicValues(ConfigValueDto globaltiktokInstagram, ConfigValueDto  globalphoneFacebook,ConfigValueDto  globalemailAdress, ConfigValueDto globalLocation)
        {
            GlobaltiktokInstagram  = globaltiktokInstagram;
            GlobalphoneFacebook = globalphoneFacebook ;
            GlobalemailAdress  = globalemailAdress  ;
            GlobalLocation = globalLocation ;

            NotifyStateChanged();
        }

        public void SetGlobalFirstLoad(bool firstLoad)
        {
            GlobalFirstLoad = firstLoad;
        }

        public void SetGlobalCountSolicited(int count)
        {
            GlobalCountSolicited = count;
            NotifyCountSolicitedChanged();
        }

        public void SetGlobalLanguage(string lang)
        {
            GlobalLanguage = lang;
            NotifyLanguageChanged();
        }

        private void NotifyStateChanged() => OnGlobalBasicValuesChanged?.Invoke();
        private void NotifyCountSolicitedChanged() => OnGlobalCountSolicitedChanged?.Invoke();
        private void NotifyLanguageChanged() => OnGlobalLanguageChanged?.Invoke();
    }
}
