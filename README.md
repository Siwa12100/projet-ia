
# Projet IA - Classification de genre

Notre projet concentre deux IA différentes : un modèle d'IA en CNN (convolutional neural network) et deux modèles d'IA de classification.

# Guide d'utilisation
### Prérequis

- Python3+
- Dernière version de PIP
- Docker / DockerDesktop

### Installation des dépendances Python
Vous devez juste executer ces 3 commandes :

```bash
git clone <URL du projet>
cd projet-ia
pip install -r API/requirements.txt
```
L'installation peut prendre plusieurs minutes en fonction de votre accès à internet.

### Lancer l'API
Pour lancer l'API :
```bash
cd API/
sh ./lancement-projet.sh
```
Vous aurez dans le terminal une log du port utilisé.

Pour arrêter l'API et supprimer l'image :
```bash
cd API/
sh ./arret.sh
sh ./supprimer-image.sh
```

### Documentation

Ultralytics / YOLO : https://docs.ultralytics.com/fr

URLs DataSet:
- https://www.kaggle.com/datasets/cashutosh/gender-classification-dataset
- https://www.kaggle.com/datasets/snmahsa/human-images-dataset-men-and-women

# Auteurs

- [Antoine PINAGOT](https://github.com/YalIhow)
- [Jean MARCILLAC](https://github.com/Siwa12100)
