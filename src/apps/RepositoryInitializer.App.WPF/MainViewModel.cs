using System.Collections.ObjectModel;

namespace RepositoryInitializer.App.WPF
{
    public class MainViewModel
    {
        #region Properties

        public ObservableCollection<Variable> Variables { get; set; } = new ()
        {
            new ()
            {
                Key = "$PROJECT_NAME$",
                Value = "Name",
            },
            new()
            {
                Key = "$PROJECT_DESCRIPTION$",
                Value = "Description",
            },
        };

        public string Path { get; set; } = string.Empty;

        #endregion
    }
}