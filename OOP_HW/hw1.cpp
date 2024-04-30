#include <iostream>
#include <iomanip>
#include <string>
#include <vector>

#define N 5

class Product
{
public:
	std::string name;
	float price;

	Product() {}
	Product(std::string n, float p)
	{
		name = n;
		price = p;
	}
};

class Customer
{
public:
	std::string customer_name;
	Product list_of_ordered_products[N];
	float credit_card_limit;
	int count_of_ordered_products = 0;

	Customer(std::string cname, float cclimit = 1000)
	{
		customer_name = cname;
		credit_card_limit = cclimit;
	}

	void operator+(Product P)
	{
		std::cout << "Add product : " << P.name << "  " << P.price << std::endl;
		if (count_of_ordered_products == N) {
			std::cout << "Count of ordered products exceeded limit" << std::endl;
			std::cout << "Product add operation is not done." << std::endl << std::endl;
		} else if (credit_card_limit < calculate_total_debt() + P.price) {
			std::cout << "Total debt exceeded the credit card limit" << std::endl;
			std::cout << "Product add operation is not done." << std::endl << std::endl;
		}
		else {
			list_of_ordered_products[count_of_ordered_products] = P;
			count_of_ordered_products++;
			std::cout << "Procudt is added to customer successfully" << std::endl << std::endl;
		}
	}

	float calculate_total_debt() {
		float debt = 0;
		for (int i = 0; i < count_of_ordered_products; i++) {
			debt += list_of_ordered_products[i].price;
		}
		return debt;
	}

	void print() {
		std::cout << "Customer name              : " << customer_name << std::endl;
		std::cout << "Credit card limit          : " << credit_card_limit << std::endl;
		std::cout << "Count of ordered products  : " << count_of_ordered_products << std::endl;
		std::cout << "List of Ordered Products   : " << std::endl;
		for (int i = 0; i < count_of_ordered_products; i++) {
			std::cout << i+1 << ". " << "Name: " << std::setw(20) << std::left << list_of_ordered_products[i].name << 
				" Price: " << list_of_ordered_products[i].price << std::endl;
		}
		std::cout << "TOTAL DEBT = " << calculate_total_debt() << std::endl << std::endl;
	}
};

int main()
{
	Customer c_john("JOHN FISHER", 2000);
	Customer c_ronald("RONALD CRAIG");
	Customer c_thomas("THOMAS AUSTIN", 750);

	std::cout << "PROGRAM STARTED" << std::endl << std::endl;

	c_john + Product("Panasonic Phone", 800);
	c_john + Product("Toshiba Battery", 300);
	c_john + Product("Kenwood DVD", 400);
	c_john.print();

	std::cout << "******************************************************************" << std::endl << std::endl;

	c_ronald + Product("Canon Battery", 500);
	c_ronald + Product("Nikon Accessory", 600);
	c_ronald + Product("Yamaha Subwoofer", 200);
	c_ronald.print();

	std::cout << "******************************************************************" << std::endl << std::endl;

	c_thomas + Product("Whirlpool Charger", 90);
	c_thomas + Product("Logitech Remote", 20);
	c_thomas + Product("LG Dryer", 100);
	c_thomas + Product("Linksys Router", 60);
	c_thomas + Product("Nintendo Portable", 150);
	c_thomas + Product("Mitsubishi Stand", 50);
	c_thomas.print();

	std::cout << "PROGRAM FINISHED";
	return 0;
}