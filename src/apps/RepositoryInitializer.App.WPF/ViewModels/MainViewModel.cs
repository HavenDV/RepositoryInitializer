﻿using System.Collections.ObjectModel;

namespace RepositoryInitializer.App.WPF.ViewModels
{
    public class MainViewModel
    {
        #region Properties

        public string Path { get; set; } = string.Empty;
        public bool DeleteTemplateSettings { get; set; } = true;

        public ObservableCollection<Variable> Constants { get; } = new();
        public ObservableCollection<Variable> Variables { get; set; } = new ();
        public ObservableCollection<Condition> Conditions { get; set; } = new();

        #endregion
    }
}