import base64
import cv2
import numpy as np
from werkzeug.datastructures import FileStorage
from io import BytesIO


def file_storage_to_opencv_image(file: FileStorage):
    in_memory_file = BytesIO()
    file.save(in_memory_file)
    in_memory_file.seek(0)
    
    img_array = np.frombuffer(in_memory_file.read(), np.uint8)
    img = cv2.imdecode(img_array, cv2.IMREAD_COLOR)
    print(img.__class__)
    return img

def encode_image_to_base64(img):
    _, buffer = cv2.imencode('.jpg', img)
    return base64.b64encode(buffer).decode('utf-8')
