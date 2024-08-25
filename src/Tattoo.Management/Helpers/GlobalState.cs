namespace Tattoo.Management.Helpers
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

        public event Action OnGlobalShowDetailChanged;
        public event Action OnGlobalCountSolicitedChanged;
        public bool IsAuthenticated { get; set; } = false;
        public void SetGlobalShowDetail(bool showDetail)
        {
            GlobalShowDetail = showDetail;
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

        private void NotifyStateChanged() => OnGlobalShowDetailChanged?.Invoke();
        private void NotifyCountSolicitedChanged() => OnGlobalCountSolicitedChanged?.Invoke();
    }
}
