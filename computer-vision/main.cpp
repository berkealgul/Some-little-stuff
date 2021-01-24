#include <iostream>
#include <string>
#include <vector>
#include <opencv2/opencv.hpp>

using namespace cv;

/*
// 1366x768
std::string RES = "1366_768";
Rect ROI_RECT(Point(826, 14), Point(1050, 57));
int ICON_SIZE = 28;
*/


// 1280x960
std::string RES = "1280_960";
Rect ROI_RECT(Point(822, 17), Point(1104, 68)); //1280_960
int ICON_SIZE = 36;


/*
// 1280x720
std::string RES = "1280_720";
Rect ROI_RECT(Point(771, 16), Point(990, 52)); //1280_720
int ICON_SIZE = 27;
*/

float CONFIDANCE = 0.5;


class Agent
{
public:
    std::string name;
    Mat icon;

    Agent(std::string name, std::string imgPath)
    {
        this->name = name;
        icon = imread(imgPath, 0);
        flip(icon, icon, 1);
        resize(icon, icon, Size(ICON_SIZE, ICON_SIZE));
        //imwrite(name+"_icon.png", icon);
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

void detect(Mat frame, Mat temp, Rect& detected, double &confidance)
{
    int result_cols =  frame.cols - temp.cols + 1;
    int result_rows = frame.rows - temp.rows + 1;

    Mat result;
    result.create(result_cols, result_rows, CV_32FC1);

    cvtColor(frame, frame, COLOR_BGR2GRAY);

    imshow("roi", frame);

    matchTemplate(frame, temp, result, TM_CCOEFF_NORMED);

    double minVal, maxVal;
    Point minLoc, maxLoc;
    Point matchLoc;

    minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());

    matchLoc = maxLoc;
    confidance = maxVal;

    detected = Rect(matchLoc.x, matchLoc.y, temp.cols, temp.rows);

    std::cout << maxVal << "\n";
}

int find_index(Rect& detected)
{
    return  ceil((5*detected.x) / ROI_RECT.width);
}

int main(int argc, char **argv)
{
    std::string imgPath = "/home/berkealgul/Masaüstü/volorant/ingame_dataset/" + RES;

    std::vector<Agent> agents;
    initialize_agents(agents, "/home/berkealgul/Masaüstü/volorant/icons/");

    std::vector<std::string> imageNames;
    glob(imgPath+"/*.png", imageNames);

    for(std::string imgName : imageNames)
    {
        Mat frame = imread(imgName);
        Mat roi = frame(ROI_RECT);

        Rect detected;
        double confidance;

        std::string names[] = {"_", "_", "_", "_", "_"};
        Rect rects[] = {Rect(), Rect(), Rect(), Rect(), Rect()};
        double confs[] = {0, 0, 0, 0, 0};

        for(Agent a : agents)
        {
            detect(roi, a.icon, detected, confidance);

            if(confidance < CONFIDANCE)
                continue;

            int idx = find_index(detected);

            // if we have better result
            if(confs[idx] < confidance)
            {
                confs[idx] = confidance;
                names[idx] = a.name;

                Rect global_rect(detected.x + ROI_RECT.x, detected.y + ROI_RECT.y,
                                 detected.width, detected.height);

                rects[idx] = global_rect;
            }
        }

        std::string nameText = names[0] + " " + names[1] + " " + names[2]+ " " + names[3]+ " " + names[4];

        putText(frame, nameText, Point(ROI_RECT.x-30,ROI_RECT.y+ROI_RECT.height+50), FONT_HERSHEY_SIMPLEX, 1, Scalar::all(0), 2);

        // display all found rects and roi rect
        for(Rect r : rects)
        {
            rectangle(frame, r, Scalar::all(0), 2, 8, 0 );
        }
        rectangle(frame, ROI_RECT, Scalar::all(0), 2, 8, 0 );

        std::cout << nameText << "\n";

        if(frame.cols != 0)
            imshow("frame", frame);
        else
            break;

        waitKey(100);
    }

    return 0;
}

