import cv2

img = None

def click_and_crop(event, x, y, flags, param):
	# grab references to the global variables
    global img
    b, g, r = img[y,x,:]
    print("x:", x, " y:", y, " r:", r, " g:", g, " b:", b)


cap = cv2.VideoCapture()
cap.open("video.mp4")
#cap.set(cv2.CAP_PROP_POS_FRAMES, 1200)

resume = True

cv2.namedWindow("lol")
cv2.setMouseCallback("lol", click_and_crop)

while True:

    if resume:
        c, img = cap.read()

        if c is False:
            break

    width = int(img.shape[1] / 2)
    height = int(img.shape[0] / 2)
    dim = (width, height)

    #img = cv2.resize(img, dim, interpolation = cv2.INTER_AREA)

    cv2.imshow("lol", img)

    if cv2.waitKey(1) == ord('a'):
        break

    if cv2.waitKey(5) == ord('b'):
        resume = not resume