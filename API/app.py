import io
import os
import zipfile
from flask import Flask, jsonify, request, send_file
import logging
import json
from src.models.classify_face import classify
from src.constants.file_extensions import allowed_extensions
from src.exception.exception import *
from src.models.crop import crop_images
from src.converts.filestorage_to_cv2img import file_storage_to_opencv_image



app = Flask(__name__)

@app.errorhandler(Exception)
def handle_exception(error):
    logging.critical(error, exc_info=True)

    if isinstance(error, AnalysisException):
        response = jsonify(message=error.message, description=error.description)
        return response, error.code
    
    return jsonify(message="server-error", description='Internal server error'), 500

@app.route("/api/segmentation", methods=['POST'])
def detect_and_crop():
    input_data = request.files['image']

    # Gestion d'erreur de l'API
    if input_data.filename == '':
        raise EmptyDataException
    
    file_ext = os.path.splitext(input_data.filename)[1].lower()
    if file_ext not in allowed_extensions:
        raise InvalidFileException
    
    # Convertion et detection
    file = file_storage_to_opencv_image(input_data)
    saved_faces = crop_images(file)

    # Ajout du ou des résultats dans un zip et envoi du zip
    zip_buffer = io.BytesIO()
    with zipfile.ZipFile(zip_buffer, "w", zipfile.ZIP_DEFLATED) as zip_file:
        for image_path in saved_faces: 
            filename = os.path.basename(image_path)
            zip_file.write(image_path, arcname=filename)
    zip_buffer.seek(0)

    return send_file(zip_buffer, mimetype="application/zip", as_attachment=True, download_name="images.zip")

@app.route("/api/gender-classify", methods=['POST'])
def gender_classify():
    input_data = request.files['image']

    # Gestion d'erreur de l'API
    if input_data.filename == '':
        raise EmptyDataException
    
    file_ext = os.path.splitext(input_data.filename)[1].lower()
    if file_ext not in allowed_extensions:
        raise InvalidFileException
    
    # Convertion et detection
    file = file_storage_to_opencv_image(input_data)
    saved_faces = crop_images(file)

    results = {}
    results['gender'] = classify(saved_faces[0])

    return jsonify(results), 200


# @app.route("/api/web1-classify")
# def detect_and_crop():
#     return "<p>Hello, World!</p>"

if __name__ == '__main__':
    app.run(port=5000, debug=True)