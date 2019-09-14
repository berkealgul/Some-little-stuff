#include"Boid.h"

float Boid::deltaTimeInSecs = 0;
float Boid::neighborhoodRadius = 70;
float Boid::hitRadius = 30;
int Boid::maxSpeed = 100;

float alingDis = 1;
float cohesionDis = 1;
float avoidDis = 1;

void Boid::aling(std::vector<Boid> flock)
{
	Vector2f avgV(0, 0);
	float neigborC = 0;

	for (auto& flockmate : flock)
	{
		if (isNeighbor(&flockmate))
		{
			avgV += flockmate.velocity;
			neigborC++;
		}
	}
	avgV /= neigborC;
	acceleration += (avgV - velocity) * alingDis;
}

void Boid::avoid(std::vector<Boid> flock)
{
	Vector2f force;

	for (auto& flockmate : flock)
	{
		Vector2f deltaPos = position - flockmate.position;
		float distance = lenght(&deltaPos);

		if (distance < hitRadius && distance > 0)
		{
			setMag(&deltaPos, hitRadius - distance);
			force = force + deltaPos;
		}
	}

	acceleration += force * avoidDis;
}

void Boid::cohesion(std::vector<Boid> flock)
{
	Vector2f avgPos;
	float neighborC = 0;

	for (auto& flockmate : flock)
	{
		if (isNeighbor(&flockmate))
		{
			avgPos += flockmate.position;
			neighborC++;
		}
	}
	avgPos /= neighborC;

	Vector2f force = avgPos - position;
	acceleration += force * cohesionDis;
}


Boid::Boid(float x, float y)
{
	float vx = rand() % maxSpeed + 1;
	float vy = rand() % maxSpeed + 1;

	velocity = Vector2f(vx, vy);
	position = Vector2f(x, y);
	
	shape = CircleShape(5, 3);
	shape.setFillColor(Color(188, 192, 194));
	shape.setPosition(position);
	shape.setRotation(atan(vy / vx) * 180 / 3.14);
}

void Boid::update(std::vector<Boid> flock)
{
	acceleration = Vector2f(0, 0);

	aling(flock);
	cohesion(flock);
 	avoid(flock);

	velocity += acceleration * deltaTimeInSecs;
	position += velocity * deltaTimeInSecs;

	//hýzý sýnýrlamak için
	if (lenght(&velocity) > maxSpeed)
		setMag(&velocity, maxSpeed);

	shape.setPosition(position);
	shape.setRotation(atan(velocity.y / velocity.x) * 180 / 3.14);
}


bool Boid::isNeighbor(Boid* candidate)
{
	Vector2f deltaPos = position - candidate->position;

	float len = lenght(&deltaPos);
	/* eðer sürü arkadaþý(flockmate) kendisi ise aralarýndaki uzaklýk 0 olacaktýr
		   bu sebeple if kýsmýna 0 eklemeyi tercih ettim */

	if (len < neighborhoodRadius && len > 0)
		return true;
	else
		return false;
}