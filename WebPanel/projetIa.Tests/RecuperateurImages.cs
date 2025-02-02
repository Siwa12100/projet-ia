namespace projetIa.Tests
{
    public class RecuperateurImages
    {
        private readonly string CheminDossierPartage;

        public RecuperateurImages()
        {
            // Trouver le dossier parent du projet
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            if (currentDirectory == null || currentDirectory.Parent == null || currentDirectory.Parent.Parent == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }
            var projetRoot = currentDirectory.Parent.Parent.FullName;
            
            // Définir le chemin absolu vers "DossierPartage"
            this.CheminDossierPartage = Path.Combine(projetRoot, "DossierPartage");

            Console.WriteLine($"Chemin dossier partage : {this.CheminDossierPartage}");
            
            if (!Directory.Exists(this.CheminDossierPartage))
            {
                throw new DirectoryNotFoundException($"Le dossier partagé {this.CheminDossierPartage} n'existe pas.");
            }
        }

        public FileInfo? RecupererImage(string? dossier, string nomImage)
        {
            // Déterminer le chemin de base du dossier partagé
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            if (currentDirectory == null || currentDirectory.Parent == null || currentDirectory.Parent.Parent == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }
            string cheminBase = Path.Combine(currentDirectory.Parent.Parent.FullName, "DossierPartage");

            // Si un dossier est précisé, on l'ajoute au chemin
            if (!string.IsNullOrEmpty(dossier))
            {
                cheminBase = Path.Combine(cheminBase, dossier);
            }

            // Construire le chemin de l'image
            string cheminImage = Path.Combine(cheminBase, nomImage);

            // Vérifier si le fichier existe
            if (File.Exists(cheminImage))
            {
                return new FileInfo(cheminImage);
            }

            Console.WriteLine($"Fichier non trouvé : {cheminImage}");
            return null;
        }

        public List<FileInfo> ListerFichiersDossier(string? dossier)
        {
            // Déterminer le chemin de base du dossier partagé
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            if (currentDirectory == null || currentDirectory.Parent == null || currentDirectory.Parent.Parent == null)
            {
                throw new InvalidOperationException("Unable to determine the project root directory.");
            }
            string cheminBase = Path.Combine(currentDirectory.Parent.Parent.FullName, "DossierPartage");

            // Si un dossier est précisé, on l'ajoute au chemin
            if (!string.IsNullOrEmpty(dossier))
            {
                cheminBase = Path.Combine(cheminBase, dossier);
            }

            // Vérifier si le dossier existe
            if (!Directory.Exists(cheminBase))
            {
                Console.WriteLine($"Dossier non trouvé : {cheminBase}");
                return new List<FileInfo>();
            }

            // Retourner la liste des fichiers sous forme de FileInfo
            return Directory.GetFiles(cheminBase)
                            .Select(f => new FileInfo(f))
                            .ToList();
        }


    }
}