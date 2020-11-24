using System;
using System.Collections.Generic;
using System.IO;

namespace RepositoryInitializer.App.WPF
{
    public static class Replacer
    {
        public static string ReplaceWithVariables(this string text, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var pair in variables)
            {
                text = text.Replace(pair.Key, pair.Value, comparison);
            }

            return text;
        }

        public static void ReplaceFileNames(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var paths = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);
            foreach (var path in paths)
            {
                File.Move(path, path.ReplaceWithVariables(variables, comparison));
            }
        }

        public static void ReplaceContents(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var paths = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories);
            foreach (var path in paths)
            {
                var contents = File.ReadAllText(path);

                File.WriteAllText(path, contents.ReplaceWithVariables(variables, comparison));
            }
        }
    }
}
