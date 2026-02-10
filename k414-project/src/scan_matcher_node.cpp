#include <ros/ros.h>
#include <std_srvs/Empty.h>
#include <geometry_msgs/Pose2D.h>
#include <sensor_msgs/LaserScan.h>
#include <sensor_msgs/PointCloud.h>
#include <tf/transform_listener.h>
#include <laser_geometry/laser_geometry.h>

using namespace std;

class ScansMerger
{
public:
  ScansMerger(ros::NodeHandle& nh, ros::NodeHandle& nh_local);
  ~ScansMerger();

private:
  bool updateParams(std_srvs::Empty::Request& req, std_srvs::Empty::Response& res);
  void frontScanCallback(const sensor_msgs::LaserScan::ConstPtr front_scan);
  void rearScanCallback(const sensor_msgs::LaserScan::ConstPtr rear_scan);

  void initialize() { std_srvs::Empty empt; updateParams(empt.request, empt.response); }

  void publishMessages();

  ros::NodeHandle nh_;
  ros::NodeHandle nh_local_;

  ros::ServiceServer params_srv_;

  ros::Subscriber front_scan_sub_;
  ros::Subscriber rear_scan_sub_;
  ros::Publisher scan_pub_;
  ros::Publisher pcl_pub_;

  tf::TransformListener tf_ls_;
  laser_geometry::LaserProjection projector_;

  bool front_scan_received_;
  bool rear_scan_received_;
  bool front_scan_error_;
  bool rear_scan_error_;

  sensor_msgs::PointCloud front_pcl_;
  sensor_msgs::PointCloud rear_pcl_;

  // Parameters
  bool p_active_;
  bool p_publish_scan_;
  bool p_publish_pcl_;

  int p_ranges_num_;

  double p_min_scanner_range_;
  double p_max_scanner_range_;
  double p_min_x_range_;
  double p_max_x_range_;
  double p_min_y_range_;
  double p_max_y_range_;

  std::string p_fixed_frame_id_;
  std::string p_target_frame_id_;
};


ScansMerger::ScansMerger(ros::NodeHandle& nh, ros::NodeHandle& nh_local) : nh_(nh), nh_local_(nh_local) {
  p_active_ = false;

  front_scan_received_ = false;
  rear_scan_received_ = false;

  front_scan_error_ = false;
  rear_scan_error_ = false;

  params_srv_ = nh_local_.advertiseService("params", &ScansMerger::updateParams, this);

  initialize();
}

ScansMerger::~ScansMerger() {
  nh_local_.deleteParam("active");
  nh_local_.deleteParam("publish_scan");
  nh_local_.deleteParam("publish_pcl");

  nh_local_.deleteParam("ranges_num");

  nh_local_.deleteParam("min_scanner_range");
  nh_local_.deleteParam("max_scanner_range");

  nh_local_.deleteParam("min_x_range");
  nh_local_.deleteParam("max_x_range");
  nh_local_.deleteParam("min_y_range");
  nh_local_.deleteParam("max_y_range");

  nh_local_.deleteParam("fixed_frame_id");
  nh_local_.deleteParam("target_frame_id");
}

bool ScansMerger::updateParams(std_srvs::Empty::Request &req, std_srvs::Empty::Response &res) {
  bool prev_active = p_active_;

  nh_local_.param<bool>("active", p_active_, true);
  nh_local_.param<bool>("publish_scan", p_publish_scan_, false);
  nh_local_.param<bool>("publish_pcl", p_publish_pcl_, true);

  nh_local_.param<int>("ranges_num", p_ranges_num_, 1000);

  nh_local_.param<double>("min_scanner_range", p_min_scanner_range_, 0.05);
  nh_local_.param<double>("max_scanner_range", p_max_scanner_range_, 10.0);

  nh_local_.param<double>("min_x_range", p_min_x_range_, -10.0);
  nh_local_.param<double>("max_x_range", p_max_x_range_,  10.0);
  nh_local_.param<double>("min_y_range", p_min_y_range_, -10.0);
  nh_local_.param<double>("max_y_range", p_max_y_range_,  10.0);

  nh_local_.param<string>("fixed_frame_id", p_fixed_frame_id_, "map");
  nh_local_.param<string>("target_frame_id", p_target_frame_id_, "robot");

  if (p_active_ != prev_active) {
    if (p_active_) {
      front_scan_sub_ = nh_.subscribe("front_scan", 10, &ScansMerger::frontScanCallback, this);
      rear_scan_sub_ = nh_.subscribe("rear_scan", 10, &ScansMerger::rearScanCallback, this);
      scan_pub_ = nh_.advertise<sensor_msgs::LaserScan>("scan", 10);
      pcl_pub_ = nh_.advertise<sensor_msgs::PointCloud>("pcl", 10);
    }
    else {
      front_scan_sub_.shutdown();
      rear_scan_sub_.shutdown();
      scan_pub_.shutdown();
      pcl_pub_.shutdown();
    }
  }

  return true;
}

void ScansMerger::frontScanCallback(const sensor_msgs::LaserScan::ConstPtr front_scan) {
  projector_.projectLaser(*front_scan, front_pcl_);

  front_scan_received_ = true;
  front_scan_error_ = false;

  if (rear_scan_received_ || rear_scan_error_)
    publishMessages();
  else
    rear_scan_error_ = true;
}

void ScansMerger::rearScanCallback(const sensor_msgs::LaserScan::ConstPtr rear_scan) {
  projector_.projectLaser(*rear_scan, rear_pcl_);

  rear_scan_received_ = true;
  rear_scan_error_ = false;

  if (front_scan_received_ || front_scan_error_)
    publishMessages();
  else
    front_scan_error_ = true;
}

void ScansMerger::publishMessages() {
  ros::Time now = ros::Time::now();

  vector<float> ranges;
  vector<geometry_msgs::Point32> points;
  vector<float> range_values;
  sensor_msgs::ChannelFloat32 range_channel;
  range_channel.name = "range";
  sensor_msgs::PointCloud new_front_pcl, new_rear_pcl;

  ranges.assign(p_ranges_num_, nanf("")); // Assign nan values

  if (!front_scan_error_) {
    tf::StampedTransform target_to_lidar;
    try {
      tf_ls_.waitForTransform(p_target_frame_id_, now, front_pcl_.header.frame_id, front_pcl_.header.stamp, p_fixed_frame_id_, ros::Duration(0.05));
      tf_ls_.lookupTransform(p_target_frame_id_, now, front_pcl_.header.frame_id, front_pcl_.header.stamp, p_fixed_frame_id_, target_to_lidar);
      tf_ls_.transformPointCloud(p_target_frame_id_, now, front_pcl_, p_fixed_frame_id_, new_front_pcl);
    }
    catch (tf::TransformException& ex) {
      return;
    }
    const tf::Vector3 target_to_lidar_origin = target_to_lidar.getOrigin();
    ROS_INFO_STREAM_ONCE("Front lidar origin (frame " << front_pcl_.header.frame_id << ") is at (" << target_to_lidar_origin.getX() << ", " << target_to_lidar_origin.getY() << ") w.r.t. frame " << p_target_frame_id_);

    for (auto& point : new_front_pcl.points) {
      const double range_x = point.x - target_to_lidar_origin.getX();
      const double range_y = point.y - target_to_lidar_origin.getY();
      const double range = sqrt(pow(range_x, 2.0) + pow(range_y, 2.0));

      if (range_x > p_min_x_range_ && range_x < p_max_x_range_ &&
          range_y > p_min_y_range_ && range_y < p_max_y_range_ &&
          range > p_min_scanner_range_ && range < p_max_scanner_range_) {
        if (p_publish_pcl_) {
          points.push_back(point);
          range_channel.values.push_back(range);
        }

        if (p_publish_scan_) {
          double angle = atan2(range_y, range_x);

          size_t idx = static_cast<int>(p_ranges_num_ * (angle + M_PI) / (2.0 * M_PI));
          if (isnan(ranges[idx]) || range < ranges[idx])
            ranges[idx] = range;
        }
      }
    }
  }

  if (!rear_scan_error_) {
    tf::StampedTransform target_to_lidar;
    try {
      tf_ls_.waitForTransform(p_target_frame_id_, now, rear_pcl_.header.frame_id, rear_pcl_.header.stamp, p_fixed_frame_id_, ros::Duration(0.05));
      tf_ls_.lookupTransform(p_target_frame_id_, now, rear_pcl_.header.frame_id, rear_pcl_.header.stamp, p_fixed_frame_id_, target_to_lidar);
      tf_ls_.transformPointCloud(p_target_frame_id_, now, rear_pcl_,  p_fixed_frame_id_, new_rear_pcl);
    }
    catch (tf::TransformException& ex) {
      return;
    }
    const tf::Vector3 target_to_lidar_origin = target_to_lidar.getOrigin();
    ROS_INFO_STREAM_ONCE("Rear lidar origin (frame " << rear_pcl_.header.frame_id << ") is at (" << target_to_lidar_origin.getX() << ", " << target_to_lidar_origin.getY() << ") w.r.t. frame " << p_target_frame_id_);

    for (auto& point : new_rear_pcl.points) {
      const double range_x = point.x - target_to_lidar_origin.getX();
      const double range_y = point.y - target_to_lidar_origin.getY();
      const double range = sqrt(pow(range_x, 2.0) + pow(range_y, 2.0));

      if (range_x > p_min_x_range_ && range_x < p_max_x_range_ &&
          range_y > p_min_y_range_ && range_y < p_max_y_range_ &&
          range > p_min_scanner_range_ && range < p_max_scanner_range_) {
        if (p_publish_pcl_) {
          points.push_back(point);
          range_channel.values.push_back(range);
        }

        if (p_publish_scan_) {
          double angle = atan2(range_y, range_x);

          size_t idx = static_cast<int>(p_ranges_num_ * (angle + M_PI) / (2.0 * M_PI));
          if (isnan(ranges[idx]) || range < ranges[idx])
            ranges[idx] = range;
        }
      }
    }
  }

  if (p_publish_scan_) {
    sensor_msgs::LaserScanPtr scan_msg(new sensor_msgs::LaserScan);

    scan_msg->header.frame_id = p_target_frame_id_;
    scan_msg->header.stamp = now;
    scan_msg->angle_min = -M_PI;
    scan_msg->angle_max = M_PI;
    scan_msg->angle_increment = 2.0 * M_PI / (p_ranges_num_ - 1);
    scan_msg->time_increment = 0.0;
    scan_msg->scan_time = 0.1;
    scan_msg->range_min = p_min_scanner_range_;
    scan_msg->range_max = p_max_scanner_range_;
    scan_msg->ranges.assign(ranges.begin(), ranges.end());

    scan_pub_.publish(scan_msg);
  }

  if (p_publish_pcl_) {
    sensor_msgs::PointCloudPtr pcl_msg(new sensor_msgs::PointCloud);

    pcl_msg->header.frame_id = p_target_frame_id_;
    pcl_msg->header.stamp = now;
    pcl_msg->points.assign(points.begin(), points.end());
    pcl_msg->channels.push_back(range_channel);
    assert(range_channel.values.size() == points.size());

    pcl_pub_.publish(pcl_msg);
  }

  front_scan_received_ = false;
  rear_scan_received_ = false;
}

int main(int argc, char** argv) {
  ros::init(argc, argv, "scan_matcher", ros::init_options::NoRosout);
  ros::NodeHandle nh("");
  ros::NodeHandle nh_local("~");

  try {
    ROS_INFO("[Scans Merger]: Initializing node");
    ScansMerger sm(nh, nh_local);
    ros::spin();
  }
  catch (const char* s) {
    ROS_FATAL_STREAM("[Scans Merger]: " << s);
  }
  catch (...) {
    ROS_FATAL_STREAM("[Scans Merger]: Unexpected error");
  }

  return 0;
}