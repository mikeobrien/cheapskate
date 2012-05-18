using System;
using System.Collections.Generic;
using System.Linq;

namespace CheapSkate
{
    public class MissingArgumentsException : Exception { public MissingArgumentsException(IEnumerable<string> arguments) : 
        base("The following required arguments were not specified: " + arguments.Select(x => "-" + x).Aggregate((a, i) => a + ", " + i)) {}}

    public static class OptionParser
    {
        public static T Parse<T>(string[] args) where T : new()
        {
            var model = new T();
            var arguments = Enumerable.Range(0, args.Length).Where(x => x % 2 == 0)
                .ToDictionary(x => args[x].Replace("-", "").ToLower(), x => args[x + 1]);
            var properties = typeof(T).GetProperties()
                .Select(x => new { Property = x, Attribute = x.GetCustomAttributes(typeof(OptionAttribute), true).Cast<OptionAttribute>().FirstOrDefault() })
                .Where(x => x.Attribute != null);
            var missingRequiredArgs = properties.Where(x => x.Attribute.Required && !arguments.Any(y => y.Key == x.Attribute.Name));
            if (missingRequiredArgs.Any()) throw new MissingArgumentsException(missingRequiredArgs.Select(x => x.Attribute.Name));
            properties.Join(arguments, x => x.Attribute.Name.ToLower(), 
                      x => x.Key, (p, v) => new { p.Property, v.Value})
                .ToList().ForEach(x => x.Property.SetValue(model, x.Value, null));
            return model;
        }

        public static Dictionary<string, string> GetOptions<T>()
        {
            return typeof(T).GetProperties()
                .Select(x => x.GetCustomAttributes(typeof(OptionAttribute), true).Cast<OptionAttribute>().FirstOrDefault())
                .Where(x => x != null)
                .ToDictionary(x => x.Name, x => string.Format("{0} [{1}]", x.Description, x.Required ? "required" : "optional"));
        }
    }
}