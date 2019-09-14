#include"Boid.h"
#include<chrono>

#define screenW 1000
#define screenH 700
#define flockSize 300

RenderWindow window(VideoMode(screenW, screenH), "xxx");
auto tp1 = std::chrono::system_clock::now();
auto tp2 = std::chrono::system_clock::now();

void handleEvent();
float calculateDeltaTime();

int main()
{
	srand(time(0));

	std::vector<Boid> flock;
	for(int i = 0; i < flockSize; i++)
	{
		float x = rand() % screenW;
		float y = rand() % screenH;

		flock.push_back(Boid(x, y));
	}

	while (window.isOpen())
	{
		Boid::deltaTimeInSecs = calculateDeltaTime();
		handleEvent();

		window.clear(Color(0, 0, 0));
		for (auto& boid : flock)
		{
			window.draw(boid.shape);
			boid.update(flock);

			//sýnýr kontrolü
			if (boid.position.x < 0)
				boid.position.x = screenW;
			else if (boid.position.x > screenW)
				boid.position.x = 0;
			
			if (boid.position.y < 0)
				boid.position.y = screenH;
			else if (boid.position.y > screenH)
				boid.position.y = 0;

		}
		window.display();
	}

	return 0;
}

void handleEvent()
{
	Event event;
	while (window.pollEvent(event))
	{
		if (event.type == sf::Event::Closed)
			window.close();
	}
}

float calculateDeltaTime()
{
	tp2 = std::chrono::system_clock::now();
	std::chrono::duration<float> elapsedTime = tp2 - tp1;
	tp1 = tp2;
	return elapsedTime.count();
}