namespace TentPlannerApp.Data
{
    public class Container
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public bool Expanded { get; set; } = true;

        public Container(string baseName, int number)
        {
            Id = "Container " + number;
            Name = baseName + " " + number;
        }

        public Container() {}
    }
}
