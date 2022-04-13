# Berke Algül 030180166 12.4.2022

from math import atan2, sin, cos


# constants
p = 1.225
As = 5.4106e-5
L = 0.8
m = 30 + 1
jp = 0.003
g = -9.81


def Mp(v, Cpm):
    return 0.5*p*Cpm*As*v*v*L

def Fd(v, Cd):
    return 0.5*p*Cd*As*v*v

def Fl(v, Cl):
    return 0.5*p*Cl*As*v*v

def CL(a):
    return 0.73*a

def CPM(a):
    return 0.325*a


#part d

def V(t):
    return 

dt = 0.001
t = 0
ax = 0
ay = 0
v0 = 0
vi = 8.29
theta_i = 35 * (3.14/180)
theta_0 = 0

x0 = 0
y0 = 1.5

while y0 > 0:
    ax = ((vi-v0)/dt)*cos(theta_i) - vi*sin(theta_i)*((theta_i-theta_0)/dt)
    ay = ((vi-v0)/dt)*sin(theta_i) - vi*cos(theta_i)*((theta_i-theta_0)/dt) + g

    v0 = vi
    vi = vi - ((Fd(vi, Cd)*cos(theta_i) + Fl(v, Cl)*sin(theta_i))/m)*dt

    theta_0 = theta_i
    theta_i = theta_i + (Mp(v, Cpm)/jp)*dt

    x0 = x0 + vi*cos(theta_i)*dt
    y0 = y0 + vi*cos(theta_i)*dt


# e part
# :(