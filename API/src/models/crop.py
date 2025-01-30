import os
import cv2
from PIL import Image

from src.constants.crop_dimensions import HEIGHT, WIDTH
from .detect_face import detect

def crop_images(image):
    results = detect(image)
    boxes = results[0].boxes  

    output_folder = "../crop_results/"
    os.makedirs(output_folder, exist_ok=True)

    saved_faces = [] 

    for i, box in enumerate(boxes.xyxy):
        x1, y1, x2, y2 = map(int, box.tolist())  

        face_crop = image[y1:y2, x1:x2]

        face_resized = cv2.resize(face_crop, (WIDTH, HEIGHT))  

        face_filename = f"face_{i}.jpg"
        face_path = os.path.join(output_folder, face_filename)
        cv2.imwrite(face_path, face_resized)

        saved_faces.append(face_path)

    return saved_faces

if __name__ == '__main__':
    # With a good picture
    # i =  cv2.imread("../../../dataset/uncropped_images/gender_dataset/men/224.jpg")
    # crop_images(i)

    # With a bad picture
    i =  cv2.imread("../../../dataset/test/singe.jpg")
    crop_images(i)