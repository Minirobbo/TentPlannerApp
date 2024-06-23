using Blazored.LocalStorage;
using System;
using WASM_App.Overlays;

namespace WASM_App.Data
{
    public class DataService
    {
        public string SearchText = "";
        public List<Container> Containers = new();
        private List<Person> _items = new();
        public List<Person> DisplayItems = new();
        public Person? CurrentPersonEditing { get; private set; } = null;
        public Action Update { get; set; } = new Action(() => { });

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

        public async void UpdatePeopleStorage()
        {
            await _localStorageService.SetItemAsync<List<Person>>("items", _items);
        }

        public async void UpdateContainerStorage()
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
            CurrentPersonEditing = person;

            UpdatePeopleStorage();
            RefreshItems();
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
            DisplayItems = _items.Where(p => p.Identifier != "Names" || (SearchText.Trim() == "" || p.Name.ToLower().Contains(SearchText.ToLower()))).OrderBy(p => p.Name).ToList();
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

        public void RemoveLastContainer()
        {
            int index = Containers.Count - 1;
            RemoveContainer(Containers[index]);
        }

        public void RemoveContainer(Container container)
        {
            string id = container.Id;
            _items.ForEach(item => item.Identifier = item.Identifier == id ? "Names" : item.Identifier);
            Containers.Remove(container);

            UpdateContainerStorage();
        }

        public Container NewContainer(int number)
        {
            return new Container(_settings.ContainerBaseName, number);
        }
    }
}
