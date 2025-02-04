import cv2
from ultralytics import YOLO
from PIL import Image


def iut_detection(i):
    i = cv2.resize(i, (640, 640))  
    model = YOLO("pt_models/iut_detection.pt")

    result = model(i)
    predicted_index = result[0].probs.top1

    return result[0].names[predicted_index]

if __name__ == '__main__':
    i = Image.open("../../../dataset/test/IMG_0288.jpg")
    print("Result:", iut_detection(i))
    # classify(i)