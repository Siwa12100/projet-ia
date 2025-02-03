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
        private IBrowserFile? _fichierSelectionne;
        private MudFileUpload<IBrowserFile>? _zoneFichier;
        public  string? _imageUrl { get; set; }
        // private MudSnackbar? _snackbar;
        private static readonly string[] ExtensionsAutorisees = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        protected string? sexe = null;
        protected string? personne = null; 

        [Inject]
        protected IService? IaService { get; set; }

        [Inject]
        protected ISnackbar? Snackbar { get; set; }

        private async Task ViderAsync()
        {
            await (_zoneFichier?.ClearAsync() ?? Task.CompletedTask);
            _nomFichier = null;
            _fichierSelectionne = null;
            _imageUrl = null;
            sexe = null;
            personne = null;
            AfficherMessage("Fichier supprimé.", Severity.Info);
            ReinitialiserClasseDrag();
        }

        private Task OuvrirSelectionFichierAsync()
            => _zoneFichier?.OpenFilePickerAsync() ?? Task.CompletedTask;

        private async void SurChangementFichier(InputFileChangeEventArgs e)
        {
            ReinitialiserClasseDrag();

            var fichier = e.GetMultipleFiles(1).FirstOrDefault();
            if (fichier == null)
            {
                AfficherMessage("Aucun fichier sélectionné.", Severity.Warning);
                return;
            }

            if (!ExtensionsAutorisees.Contains(Path.GetExtension(fichier.Name).ToLower()))
            {
                AfficherMessage("Format de fichier non autorisé. Veuillez choisir une image.", Severity.Error);
                return;
            }

            _fichierSelectionne = fichier;
            _nomFichier = fichier.Name;

            await ChargerImageAsync(fichier);

            AfficherMessage("Fichier sélectionné avec succès.", Severity.Success);
        }

        private async Task ChargerImageAsync(IBrowserFile fichier)
        {
            try
            {
                var memoryStream = new MemoryStream();
                await fichier.OpenReadStream(maxAllowedSize: 100000000).CopyToAsync(memoryStream);
                _imageUrl = $"data:image/png;base64,{Convert.ToBase64String(memoryStream.ToArray())}";
                StateHasChanged();
            }
            catch (Exception ex)
            {
                AfficherMessage($"Erreur lors du chargement de l'image : {ex.Message}", Severity.Error);
            }
        }


        private async Task EnvoyerClassificationGenre()
        {
            if (_fichierSelectionne == null)
            {
                AfficherMessage("Aucun fichier à envoyer.", Severity.Warning);
                return;
            }

            using var memoryStream = new MemoryStream();
            await _fichierSelectionne.OpenReadStream(maxAllowedSize: 100000000).CopyToAsync(memoryStream);
            var fichierByteArray = memoryStream.ToArray();

            if (IaService == null)
            {
                AfficherMessage("Service IA non disponible.", Severity.Error);
                return;
            }

            var resultatIa = await IaService.ClassifierParGenre(fichierByteArray);
            if (resultatIa == null)
            {
                AfficherMessage("Erreur lors de la classification.", Severity.Error);
                return;
            }

            sexe = resultatIa;
            AfficherMessage($"Le sexe détecté est : {sexe}", Severity.Success);
        }

        protected async Task EnvoyerClassificationPersonne()
        {
            if (_fichierSelectionne == null)
            {
                AfficherMessage("Aucun fichier à envoyer.", Severity.Warning);
                return;
            }

            using var memoryStream = new MemoryStream();
            await _fichierSelectionne.OpenReadStream(maxAllowedSize: 100000000).CopyToAsync(memoryStream);
            var fichierByteArray = memoryStream.ToArray();

            if (IaService == null)
            {
                AfficherMessage("Service IA non disponible.", Severity.Error);
                return;
            }

            var resultatIa = await IaService.ClassifierParPersonne(fichierByteArray);
            if (resultatIa == null)
            {
                AfficherMessage("Erreur lors de la classification par personne.", Severity.Error);
                return;
            }

            this.personne = resultatIa;
            AfficherMessage($"La personne détecté est : {sexe}", Severity.Success);
        }

        private void ActiverClasseDrag() => _classeDrag = $"{ClasseDragParDefaut} mud-border-primary";
        private void ReinitialiserClasseDrag() => _classeDrag = ClasseDragParDefaut;

        private void AfficherMessage(string message, Severity niveau)
        {
            Snackbar?.Add(message, niveau);
        }
    }
}