using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using projetIa.Service;
using System.Linq;

namespace projetIa.Pages.Index
{
    public partial class Index
    {
        private const string ClasseDragParDefaut = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string _classeDrag = ClasseDragParDefaut;
        private string? _nomFichier;
        private MudFileUpload<IBrowserFile>? _zoneFichier;
        private static readonly string[] ExtensionsAutorisees = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        protected string? sexe = null;

        [Inject]
        protected IService? IaService { get; set; }

        private async Task ViderAsync()
        {
            await (_zoneFichier?.ClearAsync() ?? Task.CompletedTask);
            _nomFichier = null;
            this.sexe = null;
            ReinitialiserClasseDrag();
        }

        private Task OuvrirSelectionFichierAsync()
            => _zoneFichier?.OpenFilePickerAsync() ?? Task.CompletedTask;

        // private void SurChangementFichier(InputFileChangeEventArgs e)
        // {
        //     ReinitialiserClasseDrag();
            
        //     var fichier = e.GetMultipleFiles(1).FirstOrDefault();
        //     if (fichier == null) return;
            
        //     if (_nomFichier != null)
        //     {
        //         Console.WriteLine("Un seul fichier est autorisé.");
        //         return;
        //     }
            
        //     if (!ExtensionsAutorisees.Contains(Path.GetExtension(fichier.Name).ToLower()))
        //     {
        //         Console.WriteLine("Format de fichier non autorisé.");
        //         return;
        //     }

        //     _nomFichier = fichier.Name;
        // }

        private IBrowserFile? _fichierSelectionne;

        private void SurChangementFichier(InputFileChangeEventArgs e)
        {
            ReinitialiserClasseDrag();

            var fichier = e.GetMultipleFiles(1).FirstOrDefault();
            if (fichier == null)
            {
                Console.WriteLine("Aucun fichier sélectionné.");
                return;
            }

            if (!ExtensionsAutorisees.Contains(Path.GetExtension(fichier.Name).ToLower()))
            {
                Console.WriteLine("Format de fichier non autorisé.");
                return;
            }

            _fichierSelectionne = fichier;
            _nomFichier = fichier.Name;
        }


        private async Task Envoyer()
        {
            if (_nomFichier != null)
            {
                Console.WriteLine($"Envoi du fichier : {_nomFichier}");
                await this.RecupererRetourAi();

            }
        }

        protected async Task RecupererRetourAi()
        {
            if (this.IaService == null)
            {
                Console.WriteLine("Service IA non disponible.");
                return;
            }

            if (_fichierSelectionne == null)
            {
                Console.WriteLine("Aucun fichier sélectionné.");
                return;
            }

            using var memoryStream = new MemoryStream();
            await _fichierSelectionne.OpenReadStream(maxAllowedSize:100000000).CopyToAsync(memoryStream);
            var fichierByteArray = memoryStream.ToArray();

            var resultatIa = await this.IaService.ClassifierParGenre(fichierByteArray);
            if (resultatIa == null)
            {
                Console.WriteLine("Erreur lors de la classification.");
                return;
            }

            this.sexe = resultatIa;
            Console.WriteLine($"Le sexe est : {this.sexe}");
        }

        private void ActiverClasseDrag()
            => _classeDrag = $"{ClasseDragParDefaut} mud-border-primary";

        private void ReinitialiserClasseDrag()
            => _classeDrag = ClasseDragParDefaut;
    }
}
