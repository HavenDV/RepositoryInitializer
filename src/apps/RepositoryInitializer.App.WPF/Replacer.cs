using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RepositoryInitializer.App.WPF
{
    public static class Replacer
    {
        internal static string ReplaceWithVariables(this string text, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var pair in variables)
            {
                text = text.Replace(pair.Key, pair.Value, comparison);
            }

            return text;
        }

        internal static IEnumerable<string> GetPaths(string folder)
        {
            return Directory
                .EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(path => !path.StartsWith(folder.TrimEnd('\\', '/') + "\\.git"));
        }

        public static void ReplaceFileNames(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in GetPaths(folder))
            {
                var to = path.ReplaceWithVariables(variables, comparison);

                var directory = Path.GetDirectoryName(to);
                Directory.CreateDirectory(directory ?? string.Empty);

                File.Move(path, to);
            }
        }

        public static void ReplaceContents(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in GetPaths(folder))
            {
                var contents = File.ReadAllText(path);

                File.WriteAllText(path, contents.ReplaceWithVariables(variables, comparison));
            }
        }
    }
}
