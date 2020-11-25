using System.Collections.Generic;

namespace RepositoryInitializer.App.WPF
{
    public class Settings
    {
        #region Properties

        public List<Variable> Variables { get; set; } = new();
        public List<Condition> Conditions { get; set; } = new();

        #endregion
    }
}
