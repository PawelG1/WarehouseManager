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
            LoadReleaseNotes();
        }

        private void LoadReleaseNotes()
        {
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var releaseNotesPath = Path.Combine(baseDir, "Resources", "release_notes.md");
                
                // Try relative path first (for development)
                if (!File.Exists(releaseNotesPath))
                {
                    releaseNotesPath = Path.Combine(baseDir, "..", "..", "..", "Resources", "release_notes.md");
                    releaseNotesPath = Path.GetFullPath(releaseNotesPath);
                }

                if (File.Exists(releaseNotesPath))
                {
                    ReadmeContent = File.ReadAllText(releaseNotesPath);
                }
                else
                {
                    ReadmeContent = "# Release Notes not found\n\nPlease ensure release_notes.md exists in the Resources folder.";
                }
            }
            catch (Exception e)
            {
                ReadmeContent = $"# An Error occurred while loading Release Notes\n\n{e.Message}";
            }
        }

        
    }
}
