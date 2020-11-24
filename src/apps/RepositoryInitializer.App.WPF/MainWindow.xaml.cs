using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using RepositoryInitializer.App.WPF.Properties;

namespace RepositoryInitializer.App.WPF
{
    public partial class MainWindow
    {
        #region Properties

        public MainViewModel ViewModel { get; } = new();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                ViewModel = Load();
                DataContext = ViewModel;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Methods

        private static void Save(MainViewModel viewModel)
        {
            var settings = Settings.Default;
            settings.Path = viewModel.Path;
            settings.VariableNames = new StringCollection();
            settings.VariableNames.AddRange(viewModel.Variables.Select(i => i.Key ?? string.Empty).ToArray());
            settings.VariableValues = new StringCollection();
            settings.VariableValues.AddRange(viewModel.Variables.Select(i => i.Value ?? string.Empty).ToArray());

            settings.Save();
        }

        private static MainViewModel Load()
        {
            var settings = Settings.Default;
            var viewModel = new MainViewModel
            {
                Path = settings.Path
            };

            var names = settings.VariableNames?.Cast<string>().ToArray() ?? Array.Empty<string>();
            var values = settings.VariableValues?.Cast<string>().ToArray() ?? Array.Empty<string>();
            foreach (var (name, value) in names.Zip(values))
            {
                viewModel.Variables.Add(new Variable
                {
                    Key = name,
                    Value = value,
                });
            }

            if (!viewModel.Variables.Any())
            {
                viewModel.Variables.Add(new()
                {
                    Key = "$PROJECT_NAME$",
                    Value = "Name",
                });
                viewModel.Variables.Add(new()
                {
                    Key = "$PROJECT_DESCRIPTION$",
                    Value = "Description",
                });
            }

            return viewModel;
        }

        #endregion

        #region Event Handlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save(ViewModel);

                var path = ViewModel.Path;
                var variables = ViewModel.Variables
                    .ToDictionary(
                        pair => pair.Key ?? string.Empty,
                        pair => pair.Value ?? string.Empty);

                Replacer.ReplaceFileNames(path, variables, StringComparison.Ordinal);
                Replacer.ReplaceContents(path, variables, StringComparison.Ordinal);
                Replacer.DeleteEmptyDirs(path, variables, StringComparison.Ordinal);

                MessageBox.Show("Done!", "Message:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}", "Exception:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
