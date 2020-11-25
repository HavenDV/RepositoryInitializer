using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RepositoryInitializer
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

        internal static bool ContainsVariables(this string text, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            return variables.Keys.Any(key => text.Contains(key, comparison));
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

        public static IEnumerable<string> Filter(this IEnumerable<string> enumerable, string folder)
        {
            return enumerable
                .Where(path => !IsIgnored(path, folder));
        }

        internal static IEnumerable<string> GetPaths(string folder)
        {
            return Directory
                .EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                .Filter(folder);
        }

        public static bool IsEmptyDirectory(this string folder)
        {
            return !Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories).Any();
        }

        public static void ReplaceFileNames(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in GetPaths(folder)
                .Where(path => path.ContainsVariables(variables)))
            {
                var to = path.ReplaceWithVariables(variables, comparison);

                var directory = Path.GetDirectoryName(to) ?? string.Empty;
                Directory.CreateDirectory(directory);

                File.Move(path, to);

                while (!string.IsNullOrWhiteSpace(directory) && directory.IsEmptyDirectory())
                {
                    Directory.Delete(directory);

                    directory = Path.GetDirectoryName(directory) ?? string.Empty;
                } 
            }
        }

        public static void ReplaceContents(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in GetPaths(folder))
            {
                var contents = File.ReadAllText(path);
                if (!contents.ContainsVariables(variables))
                {
                    continue;
                }

                File.WriteAllText(path, contents.ReplaceWithVariables(variables, comparison));
            }
        }

        public static void DeleteEmptyDirs(string folder, IDictionary<string, string> variables, StringComparison comparison = StringComparison.InvariantCulture)
        {
            foreach (var path in Directory
                .EnumerateDirectories(folder, "*", SearchOption.AllDirectories)
                .Where(path => !IsIgnored(path, folder) && path.ContainsVariables(variables, comparison)))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
