#include "ros/ros.h" 
#include "geometry_msgs/Pose.h"
#include "geometry_msgs/PoseStamped.h"
#include "move_base_msgs/MoveBaseAction.h"
#include "actionlib/client/simple_action_client.h"
#include "visualization_msgs/MarkerArray.h"

#include <queue>

typedef actionlib::SimpleActionClient<move_base_msgs::MoveBaseAction> MoveBaseClient;

class MissionPlanner
{
public:
    ros::Publisher *marker_pub_;

    std::queue<geometry_msgs::Pose> goal_que_;

    visualization_msgs::MarkerArray markers_;

    MoveBaseClient ac_;

    int waypoint_counter = 1;

    const int state_check_interval = 15;

    MissionPlanner(ros::Publisher *pb) : ac_("move_base", true)
    {   
        marker_pub_ = pb;
        geometry_msgs::Pose pose;

        pose.position.x = 2.76;
        pose.position.y = -3.04;
        pose.orientation.z = -0.99;
        pose.orientation.w = 0.035;
        goal_que_.push(pose);
        PushMarker(pose, 0);

        pose.position.x = -2.82;
        pose.position.y = -5.99;
        pose.orientation.z = -0.99;
        pose.orientation.w = 0.013;
        goal_que_.push(pose);
        PushMarker(pose, 1);

        pose.position.x = -3.90;
        pose.position.y = -3.20;
        pose.orientation.z = 0.94;
        pose.orientation.w = 0.32;
        goal_que_.push(pose);
        PushMarker(pose, 2);

        pose.position.x = -5.75;
        pose.position.y = 6.02;
        pose.orientation.z = 0.02;
        pose.orientation.w = 0.99;
        goal_que_.push(pose);
        PushMarker(pose, 3);

        pose.position.x = -1.83;
        pose.position.y = 5.12;
        pose.orientation.z = -0.33;
        pose.orientation.w = 0.94;
        goal_que_.push(pose);
        PushMarker(pose, 4);

        pose.position.x = 6.04;
        pose.position.y = 0.98;
        pose.orientation.z = -0.88;
        pose.orientation.w = 0.46;
        goal_que_.push(pose);
        PushMarker(pose, 5);

        pose.position.x = 3.07;
        pose.position.y = -2.58;
        pose.orientation.z = 0.99;
        pose.orientation.w = 0.0;
        goal_que_.push(pose);
        PushMarker(pose, 6);

        pose.position.x = 2.68;
        pose.position.y = -6.13;
        pose.orientation.z = 0.99;
        pose.orientation.w = 0.06;
        goal_que_.push(pose);
        PushMarker(pose, 7);

        pose.position.x = 0;
        pose.position.y = 0;
        pose.orientation.z = 0.0;
        pose.orientation.w = 1.0;
        goal_que_.push(pose);
        PushMarker(pose, 8);

        Run();
    }

    void PushMarker(const geometry_msgs::Pose &pose, int id)
    {
        float sphere_rad = 0.2;
        visualization_msgs::Marker marker;
        marker.header.frame_id = "map";
        marker.header.stamp = ros::Time::now();
        marker.pose = pose;
        marker.pose.position.z = 0.3;
        marker.pose.orientation.x = 0;
        marker.pose.orientation.y = 0;
        marker.pose.orientation.z = 0;
        marker.pose.orientation.w = 1;
        marker.id = id;
        marker.type = 2;
        marker.scale.x = sphere_rad;
        marker.scale.y = sphere_rad;
        marker.scale.z = sphere_rad;
        marker.color.r = 0;
        marker.color.b = 1;
        marker.color.g = 0;
        marker.color.a = 1;
        markers_.markers.push_back(marker);
    }

    void Run()
    {
        ac_.waitForServer();
        ROS_INFO("[Mission Planner] Mission Planner Established");
        SendGoalFromQue();
        
        while(true)
        {
            ac_.waitForResult(ros::Duration(state_check_interval));
            if(ac_.getState() == actionlib::SimpleClientGoalState::SUCCEEDED) 
            {
                ROS_INFO("[Mission Planner] Goal Reached");
                if(!SendGoalFromQue())
                {
                    ROS_INFO("[Mission Planner] Mission Accomplished!");
                    break;
                }
            }
            ros::Duration(0.5).sleep();  
        }
    }

    bool SendGoalFromQue()
    {
        if(!goal_que_.empty())
        {
            ROS_INFO("[Mission Planner] Ready to send waypoint: %d", waypoint_counter);
            move_base_msgs::MoveBaseGoal goal;
            goal.target_pose.header.frame_id = "map";
            goal.target_pose.header.stamp = ros::Time::now();
            goal.target_pose.pose = goal_que_.front();
            // ac_.sendGoal(goal,
            // boost::bind(&MissionPlanner::DoneCb, this, _1, _2),
            // boost::bind(&MissionPlanner::ActiveCb, this),
            // boost::bind(&MissionPlanner::FeedbackCb, this, _1));
            ac_.sendGoal(goal);
            goal_que_.pop();
            // highlight target waypoint
            markers_.markers[waypoint_counter-1].color.r = 0; 
            markers_.markers[waypoint_counter-1].color.b = 0;
            markers_.markers[waypoint_counter-1].color.g = 1;
            // highlight previous waypoint if it is not first one
            if(waypoint_counter > 1)
            {
                markers_.markers[waypoint_counter-2].color.r = 1;
                markers_.markers[waypoint_counter-2].color.b = 0;
                markers_.markers[waypoint_counter-2].color.g = 0;
            }
            marker_pub_->publish(markers_);
            waypoint_counter++;
            return true;
        }
        // highlight last waypoint
        markers_.markers[waypoint_counter-1].color.r = 1;
        markers_.markers[waypoint_counter-1].color.b = 0;
        markers_.markers[waypoint_counter-1].color.g = 0;
        return false;
    }

    void DoneCb(const actionlib::SimpleClientGoalState& state, const move_base_msgs::MoveBaseResultConstPtr& result) 
    {
        if(state == actionlib::SimpleClientGoalState::SUCCEEDED) 
        {
            ROS_INFO("[Mission Planner] Goal Reached");
            if(!SendGoalFromQue())
            {
                ROS_INFO("[Mission Planner] Mission Accomplished!");
            }
        }
        else
        {
            ROS_WARN("[Mission Planner] Mission did not succeed at waypoint: %d", waypoint_counter);
        }
    }

    void ActiveCb() 
    {
        ROS_INFO("[Mission Planner] Move Base just went active");
    }

    void FeedbackCb(const move_base_msgs::MoveBaseFeedbackConstPtr& feedback)
    {
        // ROS_INFO("[Mission Planner] Current Position: [X: %f, Y: %f]", 
        //         feedback->base_position.pose.position.x, 
        //         feedback->base_position.pose.position.y);
    }
};


void pose_cb(const geometry_msgs::Pose& msg)
{
    geometry_msgs::PoseStamped pose;
    pose.header.frame_id = "odom";
    pose.header.stamp = ros::Time::now();
    pose.pose = msg;
}

int main(int argc, char **argv) 
{
    ros::init(argc, argv, "mission_planner_node");
    
    ros::NodeHandle nh;
    ros::Publisher pose_pub = nh.advertise<geometry_msgs::PoseStamped>("/initalpose", 1000);
    ros::Publisher marker_pub = nh.advertise<visualization_msgs::MarkerArray>("/waypoint_markers", 1000);
    ros::Subscriber sub = nh.subscribe("gt_odom", 1000, pose_cb);
    
    MissionPlanner mp(&marker_pub);

    ros::spin();
    return 0;
}