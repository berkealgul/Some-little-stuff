import cv2

cap = cv2.VideoCapture()
cap.open("video.mp4")
cap.set(cv2.CAP_PROP_POS_FRAMES, 1200)


coordinates = [(371,349),(372,349),(373,349),(374,349),(375,349)]
points = len(coordinates)

while True:

    _, img = cap.read()

    width = int(img.shape[1] / 2)
    height = int(img.shape[0] / 2)
    dim = (width, height)

    img = cv2.resize(img, dim, interpolation = cv2.INTER_AREA)

    total_r = 0
    total_g = 0
    total_b = 0 

    cv2.imshow("lol", img)

    for x, y in coordinates:
        b, g, r = img[y,x,:]

        total_b += b
        total_g += g
        total_r += r
        
    total_r /= points
    total_g /= points
    total_b /= points

    if(total_g > 200 and total_b > 150 and total_r < 120):
        print("ULTİM HAZIR  g-", total_g," b-",total_b," r-",total_r) 
    else:
        print(":(")

    if cv2.waitKey(10) == ord('a'):
        break
        
