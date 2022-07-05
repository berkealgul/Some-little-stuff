#include <AccelStepper.h>

#define motorInterfaceType 1

// Define pin connections & motor's steps per revolution
const int dirPin = 2;
const int stepPin = 3;
// delay = 200 / 36 * 1000 = 180

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
  delayMicroseconds(1000);
  digitalWrite(stepPin, LOW);
  delay(180);
}
