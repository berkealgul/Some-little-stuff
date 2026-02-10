cmake_minimum_required(VERSION 3.0.2)
project(k414)

## Compile as C++11, supported in ROS Kinetic and newer
# add_compile_options(-std=c++11)

catkin_package()

find_package(catkin REQUIRED COMPONENTS
  roscpp
  rospy
  std_msgs
  geometry_msgs
  sensor_msgs
  tf
  laser_geometry 
  message_runtime
  actionlib
  actionlib_msgs
  move_base
  move_base_msgs
)

include_directories(
# include
  ${catkin_INCLUDE_DIRS}
)

add_executable(hw1 src/hw1.cpp)
target_link_libraries(hw1 ${roscpp_LIBRARIES} ${geometry_msgs_LIBRARIES})

add_executable(scan_matcher src/scan_matcher_node.cpp)
target_link_libraries(scan_matcher ${roscpp_LIBRARIES} ${geometry_msgs_LIBRARIES} ${sensor_msgs_LIBRARIES} 
  ${tf_LIBRARIES}  ${laser_geometry_LIBRARIES} ${message_runtime_LIBRARIES})

add_executable(mission_planner src/mission_planner_node.cpp)
target_link_libraries(mission_planner ${roscpp_LIBRARIES} ${geometry_msgs_LIBRARIES} ${sensor_msgs_LIBRARIES} 
  ${tf_LIBRARIES}  ${laser_geometry_LIBRARIES} ${message_runtime_LIBRARIES} ${actionlib_LIBRARIES} ${actionlib_msgs_LIBRARIES} ${move_base_LIBRARIES} ${move_base_msgs_LIBRARIES})

install(TARGETS 
  RUNTIME DESTINATION ${CATKIN_PACKAGE_BIN_DESTINATION})
