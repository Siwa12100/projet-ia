from ultralytics import YOLO

# Load a model
model = YOLO("yolo11n-seg.pt")  # load a pretrained model (recommended for training)

# Train the model
results = model.train(data="person_segmentation/coco8-seg.yaml", epochs=100, imgsz=640)