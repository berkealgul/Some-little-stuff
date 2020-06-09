#include<stdio.h>
#include<stdlib.h>
#include<string.h>


// Define states of the program
enum state{INTERFACE, TERMINATE, ADD_STUDENT, EDIT_STUDENT, PRINT_STUDENTS, READ_FILE, WRITE_FILE, DELETE_STUDENT};

/*
	 They are global variables so every function can access 
	 it without any extra argument.
 */
enum state state = INTERFACE;


struct course
{
	char name[100];
	char code[10];
	char instructor[100];

    int homeworks[5];
    int midterms[3];
    int quizes[5];
	int project;
    int final;

	struct course *next;
};

struct course *find_by_code(struct course *root, char *code)
{
	if(root == 0)
		return root;

	if(strcmp(root->code, code) == 0)
		return root;
	else
		return root->next;
}

struct course *create_course()
{
    struct course *course = (struct course*)malloc(sizeof(struct course));
	
    if(course == 0)
	{
		printf("Allocation failed...");
		return 0;
	}

	char *pos;
	fgets(course->name, 100, stdin);

	//get name
	printf("\nEnter name of course\n");
	fgets(course->name, 100, stdin);
	if ((pos=strchr(course->name, '\n')) != 0)
    		*pos = '\0';
	
	//get code
	printf("\nEnter code of course\n");
	fgets(course->code, 10, stdin);
	if ((pos=strchr(course->code, '\n')) != 0)
    		*pos = '\0';
	
	//get instructor
    printf("\nEnter instructor of course\n");
    fgets(course->instructor, 100, stdin);
    if ((pos=strchr(course->instructor, '\n')) != 0)
    		*pos = '\0';

	course->next = 0;

	return course;
}

void print_courses(struct course *course)
{	
	if(course == 0)
		return;
		
	int i, n;

	printf("\n\t---------------\n");
	
	printf("\n\t %s",course->name);
	printf("\n\t %s",course->code);
	printf("\n\t %s",course->instructor);

	printf("\n\n\tHomeworks");
	for(i = 0, n = (int)(sizeof(course->homeworks)/sizeof(int)); i < n; i++)
		printf("\n\t\t%d- %d", i+1, course->homeworks[i]);		
	
	printf("\n\tQuizes");	
	for(i = 0, n = (int)(sizeof(course->quizes)/sizeof(int)); i < n; i++)
		printf("\n\t\t%d- %d", i+1, course->quizes[i]);
	
	printf("\n\tMidterms");
	for(i = 0, n = (int)(sizeof(course->midterms)/sizeof(int)); i < n; i++)
		 printf("\n\t\t%d- %d", i+1, course->midterms[i]);
	
	printf("\n\tProject- %d", course->project);
	printf("\n\tFinal-   %d", course->final);

	printf("\n\t--------------\n");

	print_courses(course->next);
}

struct course *append_course(struct course *root, struct course *course)
{
	if(root == 0)
		return course;
	else
		root->next = append_course(root->next, course);

	return root;
}

struct course *add_course(struct course *root)
{
	struct course *course = create_course();
	root = append_course(root, course);
	return root;
}


struct student
{
	char id[50];
	char tr_id[50];

	char name[50];
	char surname[50];
	char address[200];

	struct course *course;
	struct student *next;
};

struct student *find_by_id(struct student *root, char *id)
{
	if(root == 0)
		return root;

	if(strcmp(root->id, id) == 0)
		return root;
	else
		return root->next;

}

struct student *get_last(struct student *root)
{
	if(root->next == 0)
		return root;
	else
		return get_last(root->next);
} 
 
struct student *create_student()
{
	struct student *student = (struct student*)malloc(sizeof(struct student));
	
	if(student == 0)
	{
		printf("allocation failed...");
		return 0;
	}

	char *pos, flag[5];

	printf("\nStudent addition form: \n");
	fgets(flag, 50, stdin);

	//get name
	printf("\nEnter the name of student\n");
	fgets(student->name, 50, stdin);
	if ((pos=strchr(student->name, '\n')) != 0)
    	*pos = '\0';
	
	//get surname
	printf("\nEnter surname of student\n");
	fgets(student->surname, 50, stdin);
	if ((pos=strchr(student->surname, '\n')) != 0)
    	*pos = '\0';

	//get id
	printf("\nEnter the id of student\n");
	fgets(student->id, 50, stdin);
	if ((pos=strchr(student->id, '\n')) != 0)
    	*pos = '\0';
	
	//get tr id 
	printf("\nEnter the turkish republic identation number\n");
	fgets(student->tr_id, 50, stdin);
	if ((pos=strchr(student->tr_id, '\n')) != 0)
    	*pos = '\0';
	
	//get address
	printf("\nEnter adress of student\n");
	fgets(student->address, 200, stdin);
	if ((pos=strchr(student->address, '\n')) != 0)
    	*pos = '\0';
	 
	student->next = 0;
	student->course = 0;


	int x = 0;
	while(1)
	{
		printf("\nPress 1 to add course, 2 otherwise\n");
		scanf("%d", &x);

		if(x == 2)
			break;

		if(x == 1)
			student->course = add_course(student->course);
	}

	return student;
}

void print_student_content(struct student *student)
{
	printf("\nSTUDENT");
	printf("\n%s %s", student->name, student->surname);
	printf("\nid: %s", student->id);
	printf("\nTR id: %s", student->tr_id);
	printf("\nAddress: %s", student->address);
	printf("\n\n\tCourses");
	print_courses(student->course);
}

void print_students(struct student* student)
{	
	if(student == 0)
	{
		state = INTERFACE;
		return;	
	}

	print_student_content(student);

	print_students(student->next);
}


struct student *append_student(struct student *root, struct student *student)
{
	if(root == 0)
		return student;
	else
		root->next = append_student(root->next, student);

	return root;
}

struct student *add_student(struct student *root)
{
	struct student *student = create_student();
	root = append_student(root, student);
	state = INTERFACE;
	return root;
}

void greet()
{
	printf("\nBerke Algül 030180166");
	printf("\nMAK104E homework\n");
	printf("\nHello user");
	printf("\nData base file will be name: database.txt\n");
	printf("\nPlease be aware that wrong input format can cause errors on program\n");
}

void interface()
{
	printf("\n-------------------------------------------\n");
	printf("\nChoose avaible actions bellow\n");
	printf("\n Enter 1 to terminate");
	printf("\n Enter 2 to add student");
	printf("\n Enter 3 to print all students");
	printf("\n Enter 4 to edit student");
	printf("\n Enter 5 to read file");
	printf("\n Enter 6 to save students to file");
	printf("\n Enter 7 to delete student");
	printf("\n\n-------------------------------------------\n");

	int action = 0;
	scanf("%d", &action);

	switch(action)
	{
		case 1:
			state = TERMINATE;
			break;
			
		case 2:
			state = ADD_STUDENT;
			break;

		case 3:
			state = PRINT_STUDENTS;
			break;

		case 4:
			state = EDIT_STUDENT;
			break;

		case 5:
			state = READ_FILE;
			break;

		case 6:
			state = WRITE_FILE;
			break;

		case 7:
			state = DELETE_STUDENT;
			break;

		default:
			printf("\nUnavaible action detected...");
	}
}

void edit_student(struct student *root)
{
	state = INTERFACE;
	printf("\nEnter id of desired student:\n");
	char id[50], code[50];
	scanf("%s", id);
	struct student *student = find_by_id(root, id);

	if(student == 0)
	{
		printf("\n Desired student is not exist\n");
		return;
	}

	print_student_content(student);

	printf("enter desired course code");
	scanf("%s", code);
	struct course *course = find_by_code(student->course, code);

	if(course == 0)
	{
		printf("\n Desired course is not exist\n");
		return;
	}

	printf("Enter homeworks like this example: 100 90 85 95 100: ");
	scanf("%d %d %d %d %d", &(course->homeworks[0]),&(course->homeworks[1]),&(course->homeworks[2]),&(course->homeworks[3]),&(course->homeworks[4]));
	printf("Enter quizes like this example: 100 90 85 95 100: ");
	scanf("%d %d %d %d %d", &(course->quizes[0]),&(course->quizes[1]),&(course->quizes[2]),&(course->quizes[3]),&(course->quizes[4]));
	printf("Enter midterms(3) like this example: 100 90 85: ");
	scanf("%d %d %d %d %d", &(course->quizes[0]),&(course->quizes[1]),&(course->quizes[2]),&(course->quizes[3]),&(course->quizes[4]));
	printf("Enter final: ");
	scanf("%d", &(course->final));
	printf("Enter project: ");
	scanf("%d", &(course->project));

	printf("\nedited");
}


void write_course_to_file(FILE *fp, struct course *course)
{
	if(course == 0)
		return;

	fprintf(fp, "-c-\n");
	
	fprintf(fp, "%s\n",course->name);
	fprintf(fp, "%s\n",course->code);
	fprintf(fp, "%s\n",course->instructor);

	int i, n;
	for(i = 0, n = (int)(sizeof(course->homeworks)/sizeof(int)); i < n; i++)
		fprintf(fp, "%d\n", course->homeworks[i]);		
		
	for(i = 0, n = (int)(sizeof(course->quizes)/sizeof(int)); i < n; i++)
		fprintf(fp, "%d\n", course->quizes[i]);
	
	for(i = 0, n = (int)(sizeof(course->midterms)/sizeof(int)); i < n; i++)
		fprintf(fp, "%d\n", course->midterms[i]);
	
	fprintf(fp, "%d\n", course->project);
	fprintf(fp, "%d\n", course->final);

	write_course_to_file(fp, course->next);
}

void write_student_to_file(FILE *fp, struct student *student)
{
	if(student == 0)
		return;

	fprintf(fp, "-s-\n");
	fprintf(fp, "%s\n", student->name);
	fprintf(fp, "%s\n", student->surname);
	fprintf(fp, "%s\n", student->id);
	fprintf(fp, "%s\n", student->tr_id);
	fprintf(fp, "%s\n", student->address);

	write_course_to_file(fp, student->course);

	write_student_to_file(fp, student->next);
}

void write_file(struct student *student)
{
	FILE *fp = fopen("database.txt", "w");

	if(fp == 0)
		printf("\nFile is not opened");

	write_student_to_file(fp, student);
	fclose(fp);

	printf("\nFile saved.");
	state = INTERFACE;
}


struct course *read_course_from_file(FILE *fp)
{	
	struct course *course = (struct course*)malloc(sizeof(struct course));

	fgets(fp, 100, course->name);
	char *pos;
	if ((pos=strchr(course->name, '\n')) != 0)
    		*pos = '\0';
	fscanf(fp, "%s",course->code);
	fgets(fp, 100, course->instructor);
	if ((pos=strchr(course->instructor, '\n')) != 0)
    		*pos = '\0';


	int i, n;
	for(i = 0, n = (int)(sizeof(course->homeworks)/sizeof(int)); i < n; i++)
		fscanf(fp, "%d", &(course->homeworks[i]));		
		
	for(i = 0, n = (int)(sizeof(course->quizes)/sizeof(int)); i < n; i++)
		fscanf(fp, "%d", &(course->quizes[i]));
	
	for(i = 0, n = (int)(sizeof(course->midterms)/sizeof(int)); i < n; i++)
		fscanf(fp, "%d", &(course->midterms[i]));
	
	fscanf(fp, "%d", &(course->project));
	fscanf(fp, "%d", &(course->final));

	return course;
}

struct student *read_student_from_file(FILE *fp)
{
	struct student *student = (struct student*)malloc(sizeof(struct student));

	fscanf(fp, "%s", student->name);
	fscanf(fp, "%s", student->surname);
	fscanf(fp, "%s", student->id);
	fscanf(fp, "%s", student->tr_id);
	fgets(student->address, 500, fp);
	char *pos;
	if ((pos=strchr(student->address, '\n')) != 0)
    		*pos = '\0';

	student->next = 0;
	student->course = 0;

	return student;
}

struct student *read_file(struct student *root)
{
	state = INTERFACE;

	FILE *fp;
	fp = fopen("./database.txt", "r");

	if(fp == 0)
	{
		printf("\nError: File is not opened\n");
		return root;
	}
	else
		printf("\nFile opened");

	char *pos, buff[100];
	while(fgets(buff, 100, fp) != 0)
	{
		if ((pos=strchr(buff, '\n')) != 0)
    		*pos = '\0';

    	//New student detected in file 
		if(strcmp(buff, "-s-\0") == 0)
		{
			struct student *student = read_student_from_file(fp);
			root = append_student(root, student);
		}

		if(strcmp(buff, "-c-\0") == 0)
		{
			struct course *course = read_course_from_file(fp);
			struct student *student = get_last(root);
			append_course(student->course, course);
		}
	}

	fclose(fp);
	return root;
}

void delete_student(struct student *root)
{
	state = INTERFACE;
	char id[50];
	printf("\nEnter deleted student id: ");
	scanf("%s", id);

	struct student *student = find_by_id(root, id);

	if(student == 0);
	{
		printf("\nStudent is not exist");
		return;
	}

	struct student *student_next = student->next;
	free(student);
	append_student(root, student_next);
	printf("deleted");

}


int main()
{		
	greet();

	struct student *root = 0;

	//Go to top of the code to learn more about "state"
	while(state != TERMINATE)
	{
		switch(state)
		{
			case INTERFACE:
				interface();
				break;

			case ADD_STUDENT:
				root = add_student(root);
				break;

			case PRINT_STUDENTS:
				print_students(root);
				break;

			case READ_FILE:
				root = read_file(root);
				break;

			case WRITE_FILE:
				write_file(root);
				break;

			case EDIT_STUDENT:
				edit_student(root);
				break;

			case DELETE_STUDENT:
				delete_student(root);
				break;

		}
	} 
	
	printf("Program terminated\n");
	return 0;
}
