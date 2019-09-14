#include"vmath.h"

float lenght(Vector2f* v)
{
	return sqrt(v->x * v->x + v->y * v->y);
}

void normalize(Vector2f* vp)
{
	float len = lenght(vp);
	*vp /= len;
}

void setMag(Vector2f* vp, float mag)
{
	normalize(vp);
	*vp *= mag;
}