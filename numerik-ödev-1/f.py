
from math import sin, cos

theta = 0
theta_max = 3.14 / 2
theta_rmax = 0
d_theta = 0.01
dt = 0.001
g = 9.81

range_max = -1
v = 8.29

while theta < theta_max:
    y = 1.5
    x = 0
    t = 0
    
    while y > 0:
        y = y + v*sin(theta)*t - 0.5*g*t*t
        x = v*cos(theta)*t
        t = t + dt
        
    if(x > range_max):
        theta_rmax = theta

    theta = theta + d_theta

print("Angle for maximum range: ", theta_rmax)