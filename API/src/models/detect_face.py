from ultralytics import YOLO
from PIL import Image
import os




def detect(i):
    print("Current working directory:", os.getcwd())
    model = YOLO("pt_models/segment.pt")

    results = model(i)
    return results

if __name__ == '__main__':
    i = Image.open("../../../dataset/uncropped_images/gender_dataset/men/224.jpg")
    detect(i)