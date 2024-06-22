using Blazored.LocalStorage;
using TentPlannerApp.Components.Overlays;

namespace TentPlannerApp.Data
{
    public class DataService
    {
        public string SearchText = "";
        public List<Container> Containers = new();
        private List<Person> _items = new();
        public List<Person> DisplayItems = new();
        public Person? CurrentPersonEditing { get; private set; } = null;
        public Action PersonEditingChange { get; set; }
        public Action Update { get; set; }

        private SettingsService _settings;
        private ILocalStorageService _localStorageService;

        public DataService(SettingsService settingsService, ILocalStorageService localStorage) 
        {
            _settings = settingsService;
            _localStorageService = localStorage;
        }

        public async void LoadFromStorage()
        {
            _items = await _localStorageService.GetItemAsync<List<Person>>("items") ?? new();
            Containers = await _localStorageService.GetItemAsync<List<Container>>("containers") ?? new();

            RefreshItems();
            Update.Invoke();
        }

        private async void UpdatePeopleStorage()
        {
            await _localStorageService.SetItemAsync<List<Person>>("items", _items);
        }

        private async void UpdateContainerStorage()
        {
            await _localStorageService.SetItemAsync<List<Container>>("containers", Containers);
        }

        public void Reset()
        {
            StopEditing();
            _items.Clear();
            DisplayItems.Clear();
            Containers.Clear();
            Update.Invoke();
        }

        public void AddPerson(Person person)
        {
            if (!_items.Contains(person))
            {
                _items.Add(person);
            }

            UpdatePeopleStorage();
        }

        public void EditPerson(Person person)
        {
            PersonEditingChange?.Invoke();
            CurrentPersonEditing = person;

            UpdatePeopleStorage();
        }

        public void StopEditing()
        {
            CurrentPersonEditing = null;
        }

        public void RemovePerson(Person person)
        {
            _items.Remove(person);

            UpdatePeopleStorage();
        }

        public int Count(Func<Person, bool> predicate)
        {
            return _items.Count(predicate);
        }

        public void RefreshItems()
        {
            DisplayItems = _items.Where(p => p.Identifier != "Names" || p.Name.ToLower().Contains(SearchText.ToLower())).OrderBy(p => p.Name).ToList();
        }

        public bool CanDrop(Person item, string containerName)
        {
            if (_items.Count(p => p.Identifier == containerName) >= _settings.MaxOccupants)
            {
                return false;
            }

            if (_items.Where(p => p.Identifier == containerName).Any(p => p.Gender != item.Gender))
            {
                return false;
            }

            return true;
        }

        public void AddContainer()
        {
            Containers.Add(NewContainer(Containers.Count + 1));

            UpdateContainerStorage();
        }

        public void RemoveContainer()
        {
            int index = Containers.Count - 1;
            string id = Containers[index].Id;
            _items.ForEach(item => item.Identifier = item.Identifier == id ? "Names" : item.Identifier);
            Containers.RemoveAt(index);

            UpdateContainerStorage();
        }

        public Container NewContainer(int number)
        {
            return new Container(_settings.ContainerBaseName, number);
        }
    }
}
