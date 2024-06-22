using Blazored.LocalStorage;

namespace TentPlannerApp.Data
{
    public class SettingsService
    {
        private string _containerBaseName = "Tent";
        public string ContainerBaseName
        {
            get { return _containerBaseName; }
            set { _containerBaseName = value; _storageService.SetItemAsStringAsync("containerName", value); }
        }

        private int _maxOccupants = 6;
        public int MaxOccupants
        {
            get { return _maxOccupants; }
            set { _maxOccupants = value; _storageService.SetItemAsync<int>("maxOccupants", value); }
        }

        private bool _printUnallocated = false;
        public bool PrintUnallocated
        {
            get { return _printUnallocated; }
            set { _printUnallocated = value; _storageService.SetItemAsync<bool>("printUnallocated", value); }
        }
        
        private ILocalStorageService _storageService;

        public SettingsService(ILocalStorageService localStorage)
        {
            _storageService = localStorage;
        }

        public async void LoadFromStorage()
        {
            _containerBaseName = await _storageService.GetItemAsStringAsync("containerName") ?? "Tent";
            _maxOccupants = await _storageService.ContainKeyAsync("maxOccupants") ? await _storageService.GetItemAsync<int>("maxOccupants") : 6;
            _printUnallocated = await _storageService.ContainKeyAsync("printUnallocated") ? await _storageService.GetItemAsync<bool>("printUnallocated") : false;
        }
    }
}
