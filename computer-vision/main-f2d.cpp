//
// Created by berkealgul on 20.01.2021.
//

#include <iostream>
#include <string>
#include <vector>
#include <opencv2/opencv.hpp>

using namespace cv;


/*
// 1366x768
std::string RES = "1366_768";
Rect ROI_RECT(Point(810, 15), Point(1110, 75)); //1280_960
int ICON_SIZE = 28;
*/

// 1280x960

std::string RES = "1280_960";
const Rect cells[] = {
        Rect(Point(819, 19), Point(871, 71)),
        Rect(Point(880, 19), Point(932, 71)),
        Rect(Point(936, 19), Point(988, 71)),
        Rect(Point(995, 19), Point(1047, 71)),
        Rect(Point(1053, 19), Point(1105, 71))
};


/*
// 1280x720
std::string RES = "1280_720";
Rect ROI_RECT(Point(771, 16), Point(990, 52)); //1280_720
int ICON_SIZE = 27;
*/

Ptr<FeatureDetector> detector = ORB::create();
Ptr<DescriptorMatcher> matcher = DescriptorMatcher::create(DescriptorMatcher::BRUTEFORCE);
const float RATIO_TRESH = .99;
const int minMatches = 2;

class Agent
{
public:
    std::string name;
    Mat icon;

    Agent(std::string name, std::string imgPath)
    {
        this->name = name;
        icon = imread(imgPath, 0);
        //pyrDown(icon, icon, Size(icon.cols/2, icon.rows/2));
        //imshow(name, icon);
    }
};

void initialize_agents(std::vector<Agent> &agents, std::string path)
{
    agents.emplace_back("Sage", path + "/Sage_icon.png");
    agents.emplace_back("Viper", path + "/Viper_icon.png");
    agents.emplace_back("Yoru", path + "/Yoru_icon.png");
    agents.emplace_back("Jett", path + "/Jett_icon.png");
    agents.emplace_back("Breach", path + "/Breach_icon.png");
    agents.emplace_back("Cypher", path + "/Cypher_icon.png");
    agents.emplace_back("Killjoy", path + "/Killjoy_icon.png");
    agents.emplace_back("Omen", path + "/Omen_icon.png");
    agents.emplace_back("Phoenix", path + "/Phoenix_icon.png");
    agents.emplace_back("Raze", path + "/Raze_icon.png");
    agents.emplace_back("Reyna", path + "/Reyna_icon.png");
    agents.emplace_back("Skye", path + "/Skye_icon.png");
    agents.emplace_back("Sova", path + "/Sova_icon.png");
}

void detect(Mat frame, Mat ref, int &matchCount)
{
    cvtColor(frame, frame, COLOR_BGR2GRAY);
    GaussianBlur(frame, frame, Size(5, 5),0);
    pyrUp(frame, frame, Size(frame.cols*2, frame.rows*2));
   // pyrUp(frame, frame, Size(frame.cols*2, frame.rows*2));
    
    std::vector<KeyPoint> keypoints1, keypoints2;
    Mat descriptors1, descriptors2;
    detector->detectAndCompute(frame, noArray(), keypoints1, descriptors1 );
    detector->detectAndCompute(ref, noArray(), keypoints2, descriptors2 );

    //descriptors1.convertTo(descriptors1, CV_32F);
    //descriptors1.convertTo(descriptors2, CV_32F);

    if(keypoints1.size() == 0 || keypoints2.size() == 0)
    {
        matchCount = 0;
        return;
    }

    std::vector< std::vector<DMatch> > knn_matches;
    matcher->knnMatch( descriptors1, descriptors2, knn_matches, 2 );
    //-- Filter matches using the Lowe's ratio test
    std::vector<DMatch> good_matches;
    for(auto& knn_matche : knn_matches)
    {
        if (knn_matche[0].distance < RATIO_TRESH * knn_matche[1].distance)
        {
            good_matches.push_back(knn_matche[0]);
        }
    }

    /*
    drawMatches(frame, keypoints1, ref, keypoints2, good_matches, img_matches, Scalar::all(-1),
                 Scalar::all(-1), std::vector<char>(), DrawMatchesFlags::NOT_DRAW_SINGLE_POINTS );*/

    matchCount = good_matches.size();

    /*
    imshow("f", frame);
    imshow("ref", ref);
    waitKey();*/
}

int main(int argc, char **argv)
{
    std::string imgPath = "/home/berkealgul/Masaüstü/volorant/ingame_dataset/" + RES;

    std::vector<Agent> agents;
    initialize_agents(agents, "/home/berkealgul/Masaüstü/volorant/icons");

    std::vector<std::string> imageNames;
    glob(imgPath+"/*.png", imageNames);

    // each image
    for(std::string imgName : imageNames)
    {
        //"/home/berkealgul/Masaüstü/volorant/1607667456.png"
        Mat frame = imread(imgName);
        std::string names[] = {"_", "_", "_", "_", "_"};

        // each cell
        for(int i = 0; i < 5; i++)
        {
            Mat roi = frame(cells[i]);
            int maxMatches = 0;

            // each agent
            for(Agent a : agents)
            {
                int matches;

                rectangle(frame, cells[i], Scalar::all(0), 2, 8, 0 );
                detect(roi, a.icon, matches);

                std::cout << matches << "\n";

                if(matches < minMatches)
                    continue;

                // if we have better result
                if(matches > maxMatches)
                {
                    maxMatches = matches;
                    names[i] = a.name;
                }
            }
        }

        std::string nameText = names[0] + " " + names[1] + " " + names[2]+ " " + names[3]+ " " + names[4];
        putText(frame, nameText, Point(cells[0].x-30,cells[0].y+cells[0].height+50), FONT_HERSHEY_SIMPLEX, 1, Scalar::all(0), 2);

        std::cout << nameText << "\n";

        imshow("frame", frame);
        waitKey(50);
    }

    return 0;
}