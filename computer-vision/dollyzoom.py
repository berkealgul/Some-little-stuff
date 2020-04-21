import cv2
import numpy as np
from cv2 import ORB


def findMatchesBetweenImages(image_1, image_2, num_matches, mask_offset, mask_size):
    matches = None
    image_1_kp = None
    image_1_desc = None
    image_2_kp = None
    image_2_desc = None

    orb = ORB()

    offset = (mask_offset[0] * image_1.shape[0], mask_offset[1] * image_1.shape[1])
    size = (mask_size[0] * image_1.shape[0], mask_size[1] * image_1.shape[1])
    mask = np.zeros(image_1.shape[:2], dtype=np.uint8)
    mask[offset[0] - size[0] / 2:offset[0] + size[0] / 2, offset[1] - size[1] / 2:offset[1] + size[1] / 2] = 255

    image_1_kp, image_1_desc = orb.detectAndCompute(image_1, mask)
    image_2_kp, image_2_desc = orb.detectAndCompute(image_2, mask)

    bf = cv2.BFMatcher(cv2.NORM_HAMMING, crossCheck=True)
    matches = bf.match(image_1_desc, image_2_desc)
    matches = sorted(matches, key=lambda x: x.distance)[:num_matches]

    return image_1_kp, image_2_kp, matches


def findAffineTransform(image_1_kp, image_2_kp, matches):
    image_1_points = np.zeros((len(matches), 1, 2), dtype=np.float32)
    image_2_points = np.zeros((len(matches), 1, 2), dtype=np.float32)

    for idx, match in enumerate(matches):
        image_1_points[idx, 0, :] = image_1_kp[match.queryIdx].pt
        image_2_points[idx, 0, :] = image_2_kp[match.trainIdx].pt

    transform = cv2.estimateRigidTransform(image_1_points, image_2_points, fullAffine=False)

    return transform


def applyTransform(next_img, transform, img):
    return cv2.warpAffine(next_img, transform, img.shape[1::-1])


def main():
    mask_offset = [0.5, 0.5]
    mask_size   = [640, 480]
    cap = cv2.VideoCapture(1)
    _, img = cap.read()

    while 1:
        _, next_img = cap.read()
        image1_kp, image2_kp, matches = findMatchesBetweenImages(next_img, img, 50, mask_offset, mask_size)
        transform = findAffineTransform(image1_kp, image2_kp, matches)
        result = applyTransform(next_img, transform, img)
        img = next_img

        cv2.imshow("frame", result)
        if cv2.waitKey(30) == 27:
            break


if __name__ == "__main__":
    main()
