from ultralytics import YOLO

# Load a model
model = YOLO("yolo11n-cls.pt")  # load a pretrained model (recommended for training)

# Train the model
results = model.train(data="gender_recognition/", epochs=50, imgsz=64, batch=2, patience=5, device="cpu", workers=1)
