using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace Sample.App.Features;

public class ObjectViewer
{
    public string Print<T>(IEnumerable<T> source)
    {
        var flags = BindingFlags.Public | BindingFlags.GetField | BindingFlags.SetField;
        var properties = typeof(T).GetProperties(flags);
        var length = 0;
        properties.Aggregate(length, (current, p) => current + p.Name.Length + 3); // " name |"
        length += 1;

        var builder = new StringBuilder();
        builder.AppendLine("".PadLeft(length, '='));
        builder.Append("|");
        foreach (var property in properties)
        {
            builder.Append($" {property.Name} ");
        }
        builder.AppendLine("|");

        builder.AppendLine("".PadLeft(length, '='));


        foreach (var item in source)
        {
            builder.Append("|");
            var itemProperties = item.GetType().GetProperties(flags);
            foreach (var p in itemProperties)
            {
                // If item is enumerable ?? -_-)>
                // Ah... 

                var value = p.GetValue(item).ToString();
                var pLength = p.Name.Length;
                var actural = value.Length > pLength ? value.Substring(0, pLength + 3) : value;

                builder.Append($" {actural} ... ");
            }
            builder.AppendLine("|");
            builder.AppendLine("".PadLeft(length, '-'));
        }

        return builder.ToString();
    }

    public string PringJson<T>(T source)
    {
        if (source == null)
        {
            return "null";
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        return JsonSerializer.Serialize(source, options);
    }
}
