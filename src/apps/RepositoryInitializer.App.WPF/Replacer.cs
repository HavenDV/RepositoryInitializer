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
            foreach (var (key, value) in variables)
            {
                text = text.Replace(key, value, comparison);
            }

            return text;
        }

        internal static ICollection<string> IgnoredSubfolders { get; } = new List<string>
        {
            ".git",
            ".vs",
        };

        internal static bool IsIgnored(string path, string folder)
        {
            return IgnoredSubfolders.Any(subFolder => path.StartsWith(folder.TrimEnd('\\', '/') + "\\" + subFolder));
        }

        internal static IEnumerable<string> GetPaths(string folder)
        {
            return Directory
                .EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                .Where(path => !IsIgnored(path, folder));
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

        public static void DeleteEmptyDirs(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in Directory
                .EnumerateDirectories(folder, "*", SearchOption.AllDirectories)
                .Where(path => !IsIgnored(path, folder)))
            {
                foreach (var (key, _) in variables)
                {
                    if (path.Contains(key, comparison))
                    {
                        Directory.Delete(path, true);
                        break;
                    }
                }
            }
        }
    }
}
