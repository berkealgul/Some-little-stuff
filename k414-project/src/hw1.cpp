#include "ros/ros.h" 
#include "geometry_msgs/Twist.h"

int main(int argc, char **argv) 
{
    ros::init(argc, argv, "hw1");

    ros::NodeHandle n;
    ros::Rate loop_rate(10);
    ros::Publisher vel_pub = n.advertise<geometry_msgs::Twist>("/cmd_vel", 1000);

    while (ros::ok()) 
    {
        geometry_msgs::Twist msg;
        msg.linear.x = 1;
        msg.angular.z = 0.5;

        vel_pub.publish(msg);
        ros::spinOnce();    
        loop_rate.sleep();
    }

    return 0;
}