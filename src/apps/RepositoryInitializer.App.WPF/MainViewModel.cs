using System.Collections.ObjectModel;

namespace RepositoryInitializer.App.WPF
{
    public class MainViewModel
    {
        #region Properties

        public ObservableCollection<Variable> Variables { get; set; } = new ();

        public string Path { get; set; } = string.Empty;

        #endregion
    }
}