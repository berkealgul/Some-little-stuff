#include <AccelStepper.h>

#define motorInterfaceType 1

// Define pin connections & motor's steps per revolution
const int dirPin = 2;
const int stepPin = 3;
const int stepsPerRevolution = 200;
const int secondsForRevolution = 36;
const int stepsPerSecond = stepsPerRevolution / secondsForRevolution;

AccelStepper myStepper(motorInterfaceType, stepPin, dirPin);

void setup()
{
  // Declare pins as Outputs
  pinMode(stepPin, OUTPUT);
  pinMode(dirPin, OUTPUT);

  // Set motor direction clockwise
  digitalWrite(dirPin, HIGH);
}
void loop()
{
  digitalWrite(stepPin, HIGH);
  delayMicroseconds(2000);
  digitalWrite(stepPin, LOW);
  delayMicroseconds(2000);
}
