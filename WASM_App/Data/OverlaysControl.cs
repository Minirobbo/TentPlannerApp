namespace WASM_App.Data
{
    public class OverlaysControl
    {
        public bool _settingsOpen { get; set; } = false;
        public bool _uploadFileOpen { get; set; } = false;
        public bool _editPersonOpen { get; set; } = false;
        public bool _editingContainer { get; set; } = false;

        public EventHandler OnRefresh { get; set; }

        public void Refresh()
        {
            OnRefresh.Invoke(this, null);
        }
    }
}
