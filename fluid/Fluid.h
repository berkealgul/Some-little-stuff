#ifndef  FLUID_H
#define  FLUID_H

#define IX(i,j) i + N * j

class Fluid
{
	public:
		//deðiþkenler
		int size;
		float dt;
		float diff;
		float visc;

		float *s;
		float *density;

		float *Vx;
		float *Vy;

		float *Vx0;
		float *Vy0;

		//fonksiyonlar
		Fluid(int _size, float diffusion, float viscosity, float _dt);
		~Fluid();

		//ana fonksiyon
		void update();
		
		// öncül operasyonlar
		void diffuse(int b, float *x, float *x0, float diff, float dt, int iter, int N);
		void project(float *velocX, float *velocY, float *p, float *div, int iter, int N);
		void advect(int b, float *d, float *d0, float *velocX, float *velocY, float dt, int N);

		//alt fonkiyonlar
		void addVelocity(int x, int y, float amountX, float amountY);
		void addDensity(int x, int y, float amount);

		void lin_solve(int b, float *x, float *x0, float a, float c, int iter, int N);
		void set_bnd(int b, float *x, int N);

};

#endif // ! FLUID_H