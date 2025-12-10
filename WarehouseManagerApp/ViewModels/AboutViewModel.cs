using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Markdig.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace WarehouseManagerApp.ViewModels
{
    public partial class AboutViewModel : ObservableObject
    {
        [ObservableProperty]
        private string readmeContent = string.Empty;

        public AboutViewModel()
        {
            LoadReadme();
        }

        private void LoadReadme()
        {
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var readmePath = Path.Combine(baseDir, "..", "..", "..", "..", "README.md");
                readmePath = Path.GetFullPath(readmePath);

                if (File.Exists(readmePath))
                {
                    ReadmeContent = File.ReadAllText(readmePath);
                }
                else
                {
                    ReadmeContent = "# README.dm not found";
                }
            }
            catch (Exception e)
            {
                ReadmeContent = $"#An Error occured while loading README: {e}";
            }
        }

        
    }
}
