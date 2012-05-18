using System;

namespace CheapSkate
{
    public class OptionAttribute : Attribute
    {
        public OptionAttribute(string name, string description, bool required)
        {
            Required = required;
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Required { get; set; }
    }
}