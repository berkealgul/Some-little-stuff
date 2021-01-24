#include <iostream>
#include <string>
#include <vector>
#include <opencv2/opencv.hpp>


using namespace cv;


int main( int argc, char* argv[] )
{
    Mat img1 = imread("/home/berkealgul/Masaüstü/volorant/icons/Sage_icon.png",0 );
    Mat img2 = imread( "/home/berkealgul/Masaüstü/volorant/1607667456.png", 0);
    //GaussianBlur(img2, img2, Size(5, 5),0);
    //img2 = img2(Rect(819, 22, 60, 60));
    img2= img2(Rect(Point(810, 15), Point(1110, 75))); //1280_960); //1280_720)
    resize(img2, img2, Size(img2.cols*2, img2.rows*2));
    imshow("2", img2);
    //-- Step 1: Detect the keypoints using SURF Detector, compute the descriptors
    Ptr<FeatureDetector> detector = ORB::create();
    std::vector<KeyPoint> keypoints1, keypoints2;
    Mat descriptors1, descriptors2;
    detector->detectAndCompute( img1, noArray(), keypoints1, descriptors1 );
    detector->detectAndCompute( img2, noArray(), keypoints2, descriptors2 );

    //-- Step 2: Matching descriptor vectors with a FLANN based matcher
    // Since SURF is a floating-point descriptor NORM_L2 is used
    Ptr<DescriptorMatcher> matcher = DescriptorMatcher::create(DescriptorMatcher::BRUTEFORCE);
    std::vector< std::vector<DMatch> > knn_matches;
    matcher->knnMatch( descriptors1, descriptors2, knn_matches, 2 );
    //-- Filter matches using the Lowe's ratio test
    const float ratio_thresh = 0.75f;
    std::vector<DMatch> good_matches;
    for (size_t i = 0; i < knn_matches.size(); i++)
    {
        if (knn_matches[i][0].distance < ratio_thresh * knn_matches[i][1].distance)
        {
            good_matches.push_back(knn_matches[i][0]);
        }
    }
    //-- Draw matches
    Mat img_matches;
    drawMatches( img1, keypoints1, img2, keypoints2, good_matches, img_matches, Scalar::all(-1),
                 Scalar::all(-1), std::vector<char>(), DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS );
    //-- Show detected matches
    imshow("Good Matches", img_matches );
    waitKey();
    return 0;
}