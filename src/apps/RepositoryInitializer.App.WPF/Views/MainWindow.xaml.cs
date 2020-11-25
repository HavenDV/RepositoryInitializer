using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using RepositoryInitializer.App.WPF.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace RepositoryInitializer.App.WPF.Views
{
    public partial class MainWindow
    {
        #region Properties

        public MainViewModel ViewModel { get; set; } = new();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void Save(MainViewModel viewModel)
        {
            var settings = Properties.Settings.Default;
            settings.Path = viewModel.Path;
            settings.DeleteTemplateSettings = viewModel.DeleteTemplateSettings;

            settings.Save();
        }

        private static MainViewModel Load()
        {
            var settings = Properties.Settings.Default;
            if (settings.UpgradeRequired)
            {
                settings.Upgrade();
                settings.UpgradeRequired = false;
                settings.Save();
            }

            var viewModel = new MainViewModel
            {
                Path = settings.Path,
                DeleteTemplateSettings = settings.DeleteTemplateSettings,
            };

            return viewModel;
        }

        private static string GetSettingsPath(string path) => Path.Combine(path, "template.settings.json");

        private static void SaveRepositorySettings(string repositoryPath, MainViewModel viewModel)
        {
            var settings = new Settings
            {
                Variables = viewModel.Variables.ToList(),
                Conditions = viewModel.Conditions.ToList(),
            };
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            var path = GetSettingsPath(repositoryPath);

            File.WriteAllText(path, json);
        }

        private static MainViewModel LoadRepositorySettings(string repositoryPath)
        {
            var path = GetSettingsPath(repositoryPath);
            var json = File.Exists(path)
                ? File.ReadAllText(path)
                : "{}";
            var settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();

            var viewModel = new MainViewModel
            {
                Path = repositoryPath,
                Variables = new ObservableCollection<Variable>(settings.Variables),
                Conditions = new ObservableCollection<Condition>(settings.Conditions)
            };

            if (!viewModel.Variables.Any() && !viewModel.Conditions.Any())
            {
                var name = Path.GetFileName(repositoryPath.TrimEnd('\\', '/'));
                viewModel.Variables.Add(new("$PROJECT_NAME$", name));
                viewModel.Variables.Add(new("$PROJECT_DESCRIPTION$", name));

                viewModel.Conditions.Add(new("?CONDITION?", true));
            }

            return viewModel;
        }

        private void Open(string path)
        {
            ViewModel = LoadRepositorySettings(path);
            DataContext = ViewModel;

            Save(ViewModel);
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel = Load();
                DataContext = ViewModel;

                if (Directory.Exists(ViewModel.Path))
                {
                    Open(ViewModel.Path);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var dialog = new FolderBrowserDialog
                {
                    AutoUpgradeEnabled = true,
                };
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                var path = dialog.SelectedPath;

                Open(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = ViewModel.Path;
                var variables = ViewModel.Variables
                    .ToDictionary(
                        pair => pair.Key ?? string.Empty,
                        pair => pair.Value ?? string.Empty);

                Replacer.ReplaceFileNames(path, variables, StringComparison.Ordinal);
                Replacer.ReplaceContents(path, variables, StringComparison.Ordinal);
                //Replacer.DeleteEmptyDirs(path, variables, StringComparison.Ordinal);

                Save(ViewModel);

                if (ViewModel.DeleteTemplateSettings)
                {
                    var settingsPath = GetSettingsPath(path);

                    File.Delete(settingsPath);
                }

                MessageBox.Show("Done!", "Message:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveRepositorySettings(ViewModel.Path, ViewModel);
                Save(ViewModel);

                MessageBox.Show("Saved!", "Message:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
