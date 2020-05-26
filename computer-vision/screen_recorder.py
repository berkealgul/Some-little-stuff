import time
import cv2
import numpy as np
from PIL import ImageGrab


def edit_bbox():
	global roi, bbox, is_roi_chosen

	winName = "frame_edit"
	cv2.namedWindow(winName)
	cv2.setMouseCallback(winName, mouse_event_handler)

	img = cap_screen()

	while 1:
		img_speciman = img.copy()

		if len(roi) == 2:
			cv2.rectangle(img_speciman, roi[0], roi[1], (0, 255, 0), 2)

		cv2.imshow(winName, img_speciman)

		if cv2.waitKey(10) == 27:
			break

		if is_roi_chosen == True:
			x = roi[0][0]
			y = roi[0][1]
			w = roi[1][0] - x
			h = roi[1][1] - y
			bbox = (x, y, w, h)
			break

	cv2.destroyAllWindows()


def mouse_event_handler(event, x, y, flags, param):
	global roi, is_roi_chosen

	if event == cv2.EVENT_LBUTTONDOWN:
		if len(roi) == 2:
			is_roi_chosen = True
			return

		roi.append((x, y))
		roi.append((0, 0))

	elif event == cv2.EVENT_MOUSEMOVE:
		if len(roi) == 2:
			roi[1] = (x, y)


def cap_screen(bbox=None):
	img = ImageGrab.grab(bbox=bbox)
	img = np.array(img)
	img = img[..., ::-1].copy()
	return img


def record():
	global out, bbox

	while 1:
		img = cap_screen(bbox)
		print(img.shape)
		out.write(img)
		if cv2.waitKey(10) == 27:
			break


output_name = "output.avi"
fourcc = cv2.VideoWriter_fourcc(*'XVID')
fps = 30

shape = None
roi = []
bbox = None
is_roi_chosen = False

print("Capturing in ")
for i in range(3):
	print(3 - i)
	time.sleep(1)


edit_bbox()
shape = cap_screen(bbox).shape
out = cv2.VideoWriter(output_name, fourcc, fps, (shape[1], shape[0]))

print("Recording in ")
for i in range(3):
	print(3 - i)
	time.sleep(1)

record()

out.release()
cv2.destroyAllWindows()
