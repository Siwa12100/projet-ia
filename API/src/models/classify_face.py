from ultralytics import YOLO
from PIL import Image


def classify(i):
    model = YOLO("pt_models/classif.pt")

    result = model(i)
    predicted_index = result[0].probs.top1

    return result[0].names[predicted_index]

if __name__ == '__main__':
    i = Image.open("../../../dataset/test/shanalafemme.jpg")
    print(classify(i))
    # classify(i)