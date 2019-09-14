#include <vector>
#include<time.h>
#include"vmath.h"


class Boid
{

public:
	Vector2f position;
	Vector2f velocity;
	Vector2f acceleration;
	CircleShape shape;

	static float deltaTimeInSecs; 

	Boid(float x, float y);
	void update(std::vector<Boid> flock);

private:
	static float neighborhoodRadius;
	static float hitRadius;
	static int maxSpeed;

	void aling(std::vector<Boid> flock);
	void avoid(std::vector<Boid> flock);
	void cohesion(std::vector<Boid> flock);
	bool isNeighbor(Boid* candidate);
};
