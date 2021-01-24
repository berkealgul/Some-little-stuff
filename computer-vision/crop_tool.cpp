//
// Created by berkealgul on 17.01.2021.
//

#include <string>
#include <opencv2/opencv.hpp>

using namespace cv;

Mat roi;
Point ul(0,0), lr(0,0);
int num=0;
int size = 27;

void CallBackFunc(int event, int x, int y, int flags, void* userdata)
{
    std::cout << "xd";
    if  ( event == EVENT_LBUTTONDOWN )
    {
        ul.x = x;
        ul.y = y;
    }
    else if  ( event == EVENT_RBUTTONDOWN )
    {

        imwrite(std::to_string(num)+".png", roi);
        std::cout << "saved";
    }
    else if  ( event == EVENT_MBUTTONDOWN )
    {

    }
    else if ( event == EVENT_MOUSEMOVE )
    {
        lr.x = x;
        lr.y = y;
    }
}


int main(int argc, char **argv)
{
    std::string res = "1280_960/";
    std::string baseDir = "/home/berkealgul/Masaüstü/volorant/ingame_dataset/" + res;

    namedWindow("frame", 1);
    setMouseCallback("frame", CallBackFunc, NULL);

    while(1)
    {
        Mat frame = imread(baseDir+"1607667808.png");
        roi = frame(Rect(ul, Size(size, size)));

        std::cout << "ul: " << ul << " lr: " << lr << "\n";

        //rectangle(frame, ul, lr, Scalar::all(0), 2, 8, 0 );

        imshow("frame", frame);
        imshow("roi", roi);
        if(waitKey(10) == 27) //27 == esc
            break;
    }


    return 0;
}
