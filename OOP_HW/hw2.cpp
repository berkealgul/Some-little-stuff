#include <iostream>
#include <iomanip>
#include <sstream>
#include <string>
#include <typeinfo>

class Plant
{
protected:
	std::string name;
	int megawatts;

public:
	int year;

	Plant(std::string name_, int year_, int megawatts_) :
		name(name_),
		year(year_),
		megawatts(megawatts_)
	{ }

	virtual void plot() = 0;
};

class Coal : public Plant
{
private:
	float carbon;

public:
	Coal(std::string name_, int year_, int megawatts_, float carbon_) :
		Plant(name_, year_, megawatts_),
		carbon(carbon_)
	{ }

	void plot()
	{
		std::cout << std::setw(15) << std::left << name << std::setw(5) << std::left << megawatts;
		for (int i = 0, n = megawatts / 10; i < n; i++) {
			std::cout << char(219);
		}
		std::cout << std::endl;
	}

	inline float get_carbon() { return carbon; }
};

class Hydro : public Plant
{
private:
	int height;

public:
	Hydro(std::string name_, int year_, int megawatts_, int height_) :
		Plant(name_, year_, megawatts_),
		height(height_)
	{ }

	void plot()
	{
		std::cout << std::setw(15) << std::left << name << std::setw(5) << std::left << megawatts;
		for (int i = 0, n = megawatts / 100; i < n; i++) {
			std::cout << char(177);
		}
		std::cout << std::endl << std::endl;
	}

	inline int get_height() { return height; }
};

int main()
{
	Plant* plants[13];

	plants[0] = new Coal("Beihetan",    2024, 450, 6.21);
	plants[1] = new Coal("Xiangji" ,    2022, 51,  5.37);
	plants[2] = new Coal("Jaisalmer",   2023, 210, 5.32);
	plants[3] = new Coal("Nyabarongo",  2024, 365, 5.51);
	plants[4] = new Coal("Walney",      2023, 660, 4.00);
	plants[5] = new Coal("Gateway",     2023, 405, 5.18);
	plants[6] = new Coal("Vindhyachal", 2023, 520, 5.92);
	plants[7] = new Coal("Taichung",    2024, 70,  6.13);
	
	plants[8]  = new Hydro("Yeongheung",  2023, 1800, 173);
	plants[9]  = new Hydro("Kashiwazaki", 2022, 1330, 210);
	plants[10] = new Hydro("Coulee",      2023, 1209, 135);
	plants[11] = new Hydro("Hongyanhe",   2024, 702,  195);
	plants[12] = new Hydro("Polaniec",    2022, 672,  63 );

	// establish types
	Coal dummy_coal("", 0, 0, 0);
	Hydro dummy_hydro("", 0, 0, 0);
	auto coal_type  = typeid(dummy_coal).name();
	auto hydro_type = typeid(dummy_hydro).name();


	int year_counts[3] = { 0, 0, 0 }; // 0-2022 1-2023 2-2024

	float hydro_plant_count = 0;
	float total_dam_height = 0;
	float coal_plant_count = 0;
	float total_carbon_rate = 0;

	std::cout << "HISTOGRAMS FOR ELECTRIC POWER PLANTS (MEGAWATTS):" << std::endl << std::endl;
	std::cout << "COAL ELECTRIC PLANTS:" << std::endl;

	for (int i = 0; i < 13; i++)
	{
		if (typeid(*plants[i]).name() == coal_type) 
		{
			plants[i]->plot();
			coal_plant_count++;
			total_carbon_rate += dynamic_cast<Coal*>(plants[i])->get_carbon();
		}

		year_counts[plants[i]->year - 2022]++; //single loop is enough
	}

	std::cout << std::endl << "Avarage Carbon Rate: " << total_carbon_rate / coal_plant_count;
	std::cout << std::endl << std::endl << std::endl;

	std::cout << "HYDRO ELECTRIC PLANTS:" << std::endl << std::endl;

	for (int i = 0; i < 13; i++)
	{
		if (typeid(*plants[i]).name() == hydro_type)
		{
			plants[i]->plot();
			hydro_plant_count++;
			total_dam_height += dynamic_cast<Hydro*>(plants[i])->get_height();
		}
	}

	std::cout << std::endl << "Avarage Dam Height: " << total_dam_height / hydro_plant_count << std::endl << std::endl << std::endl;


	std::cout << "HISTOGRAMS FOR YEAR DISTRIBUTIONS:" << std::endl;
	std::cout << "YEAR 2022 (" << year_counts[0] << ")  ";
	for (int i = 0, n = year_counts[0]; i < n; i++) { std::cout << char(219);}
	std::cout << std::endl << std::endl;
	std::cout << "YEAR 2023 (" << year_counts[1] << ")  ";
	for (int i = 0, n = year_counts[1]; i < n; i++) { std::cout << char(219); }
	std::cout << std::endl << std::endl;
	std::cout << "YEAR 2024 (" << year_counts[2] << ")  ";
	for (int i = 0, n = year_counts[2]; i < n; i++) { std::cout << char(219); }
	std::cout << std::endl << std::endl;

	return 0;
}