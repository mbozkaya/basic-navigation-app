using namespace std;
#include <conio.h>
#include <iostream>
#include <sstream>
#include <time.h> 
#include <windows.h>   // WinApi header
#include <stdlib.h>
#include <map>
#include <math.h>  

class Menu {
public:
	static void Print(int size);
	static int TakeNumber(string message);
};

class Map {
public:
	string Item[4] = { "#", "S", "H", "P" };
	int Size;
	string** MapArray;
	int Location[2];
	map<double, int[2]> distance;

	string** Create(int size);
	void Print();
	void Move(int x, int y);
	int* ArraySelection(int* a, int n);
	void Find();
	double CalculateEuclidian(int x, int y);
	void CurrentPlace();
	void Info();

	Map(int size)
	{
		Create(size);
	}
	Map(string** mapArray)
	{
		MapArray = mapArray;
		Size = sizeof mapArray / sizeof mapArray[0];
		Location[0] = Size / 2;
		Location[1] = Size / 2;
	}
};

string** Map::Create(int size) {
	Size = size;
	Location[0] = size / 2;
	Location[1] = size / 2;

	srand(time(NULL));
	string** array = new string * [size];

	for (int x = 0; x < size; x++) {
		array[x] = new string[size];
		for (int y = 0; y < size; y++) {
			int random = rand() % 100;
			array[x][y] = random < 90 ? "#" : Item[random % 4];
		}
	}
	MapArray = array;

	return array;
}

void Map::Print() {
	HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	for (size_t x = 0; x < Map::Size + 1; x++)
	{
		for (size_t y = 0; y < Map::Size + 1; y++)
		{
			string write;

			if (x == 0 && y == 0) {
				write = "X|Y";
			}
			else if (x == 0) {
				write = to_string(y - 1);
			}
			else {
				if (y == 0) {
					write = to_string(x - 1) + " ";
				}
				else {
					if (x == Location[0] && y == Location[1]) {
						SetConsoleTextAttribute(hConsole, 228);
					}
					else {
						SetConsoleTextAttribute(hConsole, 15);
					}

					write = MapArray[x - 1][y - 1];
				}
			}

			while (write.length() < 3) {
				write += " ";
			}

			std::cout << write;
		}
		std::cout << "\n";
	}
	std::cout << "\n";

}

void Map::Move(int x, int y) {
	int newX = Location[0] + x;
	int newY = Location[1] + y;

	if (newX < 1 || newX >= Size + 1 || newY < 1 || newY >= Size + 1) {
		std::cout << "You don't go to outside of map" << endl;
	}
	else {
		Location[0] = newX;
		Location[1] = newY;

		system("cls");

		Print();

		std::cout << endl;

		printf("+++++++++++++++++++++++++++++++++++++\n");
		printf("- - 1 LEFT - 2 UP - 3 RIGHT - 4 Down\n");
		printf("- - Go Back to Menu Press 0\n");
		printf("+++++++++++++++++++++++++++++++++++++\n");

	}
}

int* Map::ArraySelection(int* a, int n) {
	int i, j, min, temp;
	for (i = 0; i < n - 1; i++) {
		min = i;
		for (j = i + 1; j < n; j++)
			if (a[j] < a[min])
				min = j;
		temp = a[i];
		a[i] = a[min];
		a[min] = temp;

	}
	return a;
}

void Map::Find() {
	int lowestDistanceLocation[2];
	double lowestDistance = 0;

	for (size_t x = 0; x < Map::Size; x++)
	{
		for (size_t y = 0; y < Map::Size; y++)
		{
			double euclidianDistance = CalculateEuclidian(x, y);


			if (x == 0 && y == 0) {
				lowestDistance = euclidianDistance;
				lowestDistanceLocation[0] = x;
				lowestDistanceLocation[1] = y;
			}
			else if (MapArray[x][y] != "#" && euclidianDistance < lowestDistance) {
				lowestDistance = euclidianDistance;
				lowestDistanceLocation[0] = x;
				lowestDistanceLocation[1] = y;
			}
		}
	}

	std::cout << "Nearest Building is : " << MapArray[lowestDistanceLocation[0]][lowestDistanceLocation[1]] << endl;
	std::cout << "Distance is : " << lowestDistance << endl;
	std::cout << "Location X : " << lowestDistanceLocation[0] << " Y :" <<lowestDistanceLocation[1] << endl;

	bool exitMenu = true;

	do
	{
		int exit = Menu::TakeNumber("Go Back to Menu Press 0");
		if (exit == 0) {
			exitMenu = false;
		}

	} while (exitMenu);
}

void Map::CurrentPlace() {

	string place = MapArray[Location[0]][Location[1]];
	string placeName = "";

	if (place == "#") {
		placeName = "Default Place";
	}
	else if (place == "P") {
		placeName = "Police Station";
	}
	else if (place == "H") {
		placeName = "Hospital";
	}
	else if (place == "S") {
		placeName = "School";
	}
	else {
		placeName = "Unknown Place";
	}

	std::cout << "Current Place :" << placeName << endl;
}

void Map::Info() {
	CurrentPlace();

	std::cout << "# - Default Place" << endl;
	std::cout << "P - Police Station" << endl;
	std::cout << "H - Hospital" << endl;
	std::cout << "S - School" << endl;

	bool exit = true;

	do
	{
		int infoInput = Menu::TakeNumber("Go Back to Menu Press 0");

		if (infoInput == 0) {
			exit = false;
		}

	} while (exit);

}

double Map::CalculateEuclidian(int x, int y) {
	return pow(((x - (Location[0] - 1)) * (x - (Location[0] - 1))) + ((y - (Location[1] - 1)) * (y - (Location[1] - 1))), 2);
}

void Menu::Print(int size) {

	Map map(size);

	bool isMenuShow = true;

	while (isMenuShow) {
		system("cls");
		map.Print();

		std::cout << "1 - Print Map" << endl;
		std::cout << "2 - Find Nearest Building" << endl;
		std::cout << "3 - Go" << endl;
		std::cout << "4 - Info" << endl;
		std::cout << "5 - Close The Navigation System" << endl;

		int menuInput = Menu::TakeNumber("Choose an Option");

		if (menuInput == 1) {

			int mapSize = Menu::TakeNumber("Please enter map size");

			map.Create(mapSize);
		}
		else if (menuInput == 2) {
			map.Find();
		}
		else if (menuInput == 3) {
			printf("+++++++++++++++++++++++++++++++++++++\n");
			printf("- - 1 LEFT - 2 UP - 3 RIGHT - 4 Down\n");
			printf("- - Go Back to Menu Press 0\n");
			printf("+++++++++++++++++++++++++++++++++++++\n");

			bool stayGoMenu = true;

			while (stayGoMenu)
			{
				int goMenuInput = Menu::TakeNumber("Choose Option");

				if (goMenuInput == 0) {
					stayGoMenu = false;
				}
				else if (goMenuInput == 1) {
					map.Move(0, -1);
				}
				else if (goMenuInput == 2) {
					map.Move(-1, 0);
				}
				else if (goMenuInput == 3) {
					map.Move(0, 1);
				}
				else if (goMenuInput == 4) {
					map.Move(1, 0);
				}
				else {
					std::cout << "Please enter valid an option" << endl;
				}
			}


		}
		else if (menuInput == 4) {			
			map.Info();
		}
		else if (menuInput == 5) {
			std::cout << "GoodBye" << endl;

			isMenuShow = false;
		}
	}


}

int Menu::TakeNumber(string message) {
	std::cout << message << endl;

	int mapSize = 0;
	string line;
	while (getline(cin, line))
	{
		std::stringstream ss(line);
		if (ss >> mapSize)
		{
			if (ss.eof())
			{   // Success
				break;
			}
		}
		std::cout << "Please enter valid char:" << endl;
	}

	return mapSize;
}

int main()
{
	std::cout << "Welcome to The Navigation System v1.0.0" << endl;
	std::cout << endl;
	std::cout << endl;

	int mapSize = 101;

	while (mapSize >= 100 || mapSize < 1) {
		mapSize = Menu::TakeNumber("Plese Enter Map Size");

		if (mapSize >= 100) {
			std::cout << "Please enter number less than 100" << endl;
		}
		else if (mapSize < 1) {
			std::cout << "Please enter number greater than 0" << endl;
		}
	}
	Menu::Print(mapSize);
}



