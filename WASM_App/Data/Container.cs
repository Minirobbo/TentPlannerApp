﻿namespace WASM_App.Data
{
    public class Container
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public bool Expanded { get; set; } = true;

        public Container(string baseName, int number)
        {
            Id = Guid.NewGuid().ToString();
            Name = baseName + " " + number;
        }

        public Container() {}
    }
}
