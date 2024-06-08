#include <algorithm>
#include <iostream>
#include <iomanip>
#include <string>
#include <map>
#include <fstream>
#include <sstream>

// global variable
std::map<int, std::string> authors;

class Book
{
public:
	int book_id;
	int publication_year;
	int author_ID;
	int publisher_ID;
	std::string book_title;

	Book() {}
	Book(int book_id_, int publication_year_, int author_ID_, int publisher_ID_, std::string book_title_) :
		book_id(book_id_),
		publication_year(publication_year_),
		author_ID(author_ID_),
		publisher_ID(publisher_ID_),
		book_title(book_title_)
	{}

	void print()
	{
		std::cout << std::left << std::setw(9) << book_id << std::left << std::setw(18) << publication_year << std::left << std::setw(19) << authors[author_ID] << book_title << std::endl;

	}
};

/*
	NOTE: Code assumes following file structure

	main.cpp
	DATA_FILES
		|
		|--- PUBLISHERS.TXT
		|--- AUTHORS.TXT
		|--- BOOKS.TXT

*/ 

int main()
{
	std::map<int, std::string> publishers;
	std::map<int, Book> books;

	std::ifstream infile;

	// read publishers
	infile.open("DATA_FILES/PUBLISHERS.TXT");
	while (infile.good()) 
	{
		std::string line;
		std::getline(infile, line);
		std::stringstream sline(line);

		int publisher_id;
		std::string publisher_name;

		if ((sline >> publisher_id)) {
			std::getline(sline, publisher_name);
			publisher_name.erase(0, 2);
			std::transform(publisher_name.begin(), publisher_name.end(), publisher_name.begin(), ::toupper); // convert publisher names to upper case
			publishers[publisher_id] = publisher_name;
		}
	}
	infile.close();

	// read authors
	infile.open("DATA_FILES/AUTHORS.TXT");
	while (infile.good())
	{
		std::string line;
		std::getline(infile, line);
		std::stringstream sline(line);

		int author_id;
		std::string author_name;

		if ((sline >> author_id)) {
			std::getline(sline, author_name);
			author_name.erase(0, 2);
			authors[author_id] = author_name;
		}
	}
	infile.close();

	// read books
	infile.open("DATA_FILES/BOOKS.TXT");
	while (infile.good())
	{
		std::string line;
		std::getline(infile, line);
		std::stringstream sline(line);

		int book_id;
		int publication_year;
		int author_id;
		int publisher_id;
		std::string title;

		if ((sline >> book_id >> publication_year >> author_id >> publisher_id)) {
			std::getline(sline, title);
			title.erase(0, 2);
			books[book_id] = Book(book_id, publication_year, author_id, publisher_id, title);
		}
	}
	infile.close();

	std::cout << "LISTS OF ALL BOOKS GROUPED BY PUBISHERS" << std::endl;
	std::cout << "=======================================" << std::endl << std::endl;
	for (auto& pub_pair : publishers) {
		// count publisher books
		int pub_book_count = 0;
		for (auto& book_pair : books) {
			if (pub_pair.first == book_pair.second.publisher_ID) {
				pub_book_count++;
			}
		}
		// print publisher books
		std::cout << "PUBLISHER_ID    : " << std::right << std::setw(3) << std::setfill('0') << pub_pair.first << std::endl;
		std::cout << "PUBLISHER_NAME  : " << pub_pair.second << std::endl;
		std::cout << "NUMBER OF BOOKS : " << pub_book_count  << std::endl << std::endl;
		std::cout << std::setfill(' ');
		std::cout << "BOOK_ID  PUBLICATION_YEAR  AUTHOR_FULLNAME    BOOK_TITLE" << std::endl;
		for (auto& book_pair : books) {
			if (pub_pair.first == book_pair.second.publisher_ID) {
				book_pair.second.print();
			}
		}
		std::cout << "---------------------------------------------------------------------" << std::endl << std::endl;
	}

	std::cout << "TOTAL NUMBER OF ALL BOOKS : " << books.size() << std::endl << std::endl << "Program finished.";

	return 0;
}