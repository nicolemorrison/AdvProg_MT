//Nicole Morrison
//CITP4650 Midterm
//July 15, 2023
//Program prompts user to enter year for calculating current salary, then imports worker.txt and manager.txt and calculates curSalary and saves to two arrays
//Menu provides Option 1: view data for all employees within a user entered salary range sorted by curSalary, Option 2: View All Data for Workers, Option 3: View All Data for Managers, Option 4: Exit

//Using statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//Create namespace
namespace EmployeeProgram
{
    public static class Global
    {
        //CurrentYear used to calculate salary made a global variable
        public static int CurrentYear { get; set; }
    }
    //Employee Class creates Employee object and base CalcCurSalary() function
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkID { get; set; }
        public int YearStartedWked { get; set; }
        public int InitSalary { get; set; }
        //curSalary made private so only usable inside of the class
        private double curSalary;
        public double CurSalary
        {
            get => curSalary;
            //protected set makes curSalary read-only except when being written to by CalcSalary inside the class
            protected set => curSalary = value;
        }

        public Employee(string firstName, string lastName, string workID, int yearStartedWked, int initSalary)
        {
            FirstName = firstName;
            LastName = lastName;
            WorkID = workID;
            YearStartedWked = yearStartedWked;
            InitSalary = initSalary;
            CurSalary = CalcCurSalary();
        }
        //CalcCurSalary function sets curSalary to initSalary
        public virtual double CalcCurSalary()
        {
            return InitSalary;
        }
    }
    //Worker class inherits from Employee class provides override CalcCurSalary() funcion
    public class Worker : Employee
    {
        public int YearWorked { get; set; }

        public Worker(string firstName, string lastName, string workID, int yearStartedWked, int initSalary) : base(firstName, lastName, workID, yearStartedWked, initSalary)
        {
            //set variables via functions
            YearWorked = CalcYearWorked();
            CurSalary = CalcCurSalary();
        }
        //CaldYearWorked function
        public int CalcYearWorked()
        {
            return Global.CurrentYear - YearStartedWked;
        }
        //override CalcCurSalary inherited from Employee class
        public override double CalcCurSalary()
        {
            double curSalary = InitSalary;
            for (int i = 0; i < YearWorked; i++)
                curSalary *= 1.03;
            //return rounded to 2 decimals
            return Math.Round(curSalary, 2);
        }
    }
    //Manager class // inherits from worker class provides override CalcCurSalary() funcion
    public class Manager : Worker
    {
        //variables needed to claculate salary for years as worker and years as manager
        public int YearPromo { get; set; }
        public int YearsWorker { get; set; }
        public int YearsManager { get; set; }

        public Manager(string firstName, string lastName, string workID, int yearStartedWked, int initSalary, int yearPromo) : base(firstName, lastName, workID, yearStartedWked, initSalary)
        {
            YearPromo = yearPromo;
            YearsManager = CalcYearsManager();
            YearsWorker = CalcYearsWorker();
            CurSalary = CalcCurSalary();
        }
        //years as manager is CurrentYear - YearPromo
        public int CalcYearsManager()
        {
            return Global.CurrentYear - YearPromo;
        }

        //years as worker is total years worked - years as manager
        public int CalcYearsWorker()
        {
            return YearWorked - YearsManager;
        }

       
        //override the CalcCurSalary inherited from Employee class
        public override double CalcCurSalary()
        {
            //This could be done just using one variable but doing
            //workSalary and then Manage Salary makes it clearer to outside person 
            //that there are two steps being calculated
            double workSalary = InitSalary;

            for (int i = 0; i < YearsWorker; i++)
                workSalary *= 1.03;

            double manageSalary = workSalary;

            for (int i = 0; i < YearsManager; i++)
                manageSalary *= 1.05;

            return Math.Round(manageSalary * 1.10, 2);
        }
    }
    //EmployeeDemo class imports text files
    public class EmployeeDemo
    {
        private List<Worker> Work { get; set; } = new List<Worker>();
        private List<Manager> Manage { get; set; } = new List<Manager>();

        public void ReadData()
        {
            //import worker.txt file
            using (StreamReader sr = new StreamReader("worker.txt"))
            {
                var numWorkers = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < numWorkers; i++)
                {
                    var firstName = sr.ReadLine();
                    var lastName = sr.ReadLine();
                    var workID = sr.ReadLine();
                    var yearStartedWked = Convert.ToInt32(sr.ReadLine());
                    var initSalary = Convert.ToInt32(sr.ReadLine());
                    //Add new worker which through functions also calculates the years worked and current salary
                    Work.Add(new Worker(firstName, lastName, workID, yearStartedWked, initSalary));
                }
            }
            //import manager.txt file
            using (StreamReader sr = new StreamReader("manager.txt"))
            {
                var numManagers = Convert.ToInt32(sr.ReadLine());

                for (int i = 0; i < numManagers; i++)
                {
                    var firstName = sr.ReadLine();
                    var lastName = sr.ReadLine();
                    var workID = sr.ReadLine();
                    var yearStartedWked = Convert.ToInt32(sr.ReadLine());
                    var initSalary = Convert.ToInt32(sr.ReadLine());
                    var yearPromo = Convert.ToInt32(sr.ReadLine());
                    //Add new manager which through functions also calculates the years worked and current salary
                    Manage.Add(new Manager(firstName, lastName, workID, yearStartedWked, initSalary, yearPromo));
                }
            }
        }
        //ObjSort() function sorts array on curSalary decending
        public void ObjSort<T>(List<T> list) where T : Employee
        {
            list.Sort((x, y) => y.CurSalary.CompareTo(x.CurSalary));
        }
        //Main function
        static void Main(string[] args)
        {
            while (true)
            {
                //collect the year for calculating salaries and years worked, etc. 
                Console.Write("Enter the year for calculating Current Salary: ");
                Global.CurrentYear = Convert.ToInt32(Console.ReadLine());

                if (Global.CurrentYear >= 1990 && Global.CurrentYear <= 2200)
                    break;
                else
                    Console.WriteLine("Invalid year. Please enter a year between 1990 and 2200.");
            }

            //create instance of EmployeeDemo class
            EmployeeDemo demo = new EmployeeDemo();

            //call the ReadData() function from the EmployeeDemo class
            demo.ReadData();

            while (true)
            {
                //menue to display
                Console.WriteLine("1. Display Workers and Managers within a specified range of Current Salary");
                Console.WriteLine("2. Display All Worker Data Ordered by Salary");
                Console.WriteLine("3. Display All Manager Data Ordered by Salary");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                var choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1) //displays only employees within the curSalary range entered by the user
                {
                    var maxCurSalary = Math.Max(demo.Work.Max(w => w.CurSalary), demo.Manage.Max(m => m.CurSalary));
                    var minCurSalary = Math.Min(demo.Work.Min(w => w.CurSalary), demo.Manage.Min(m => m.CurSalary));

                    //inform user of the current lowest and highest salary in the arrays to assist in deciding on range
                    Console.WriteLine($"The lowest Current Salary is ${minCurSalary}.");
                    Console.WriteLine($"The highest Current Salary is ${maxCurSalary}.");
                    double lowCurSalary, highCurSalary;

                    while (true)
                    {
                        //collect the low value for the range to display
                        Console.Write("Enter the low value of the Current Salary range you desire to see: $");
                        lowCurSalary = Convert.ToDouble(Console.ReadLine());

                        if (lowCurSalary >= minCurSalary && lowCurSalary <= maxCurSalary)
                            break;
                        else
                            Console.WriteLine($"Invalid value. Please enter a value between {minCurSalary} and {maxCurSalary}.");
                    }

                    while (true)
                    {
                        //collect the high value of the range to display
                        Console.Write("Enter the high value of the Current Salary range you desire to see: $");
                        highCurSalary = Convert.ToDouble(Console.ReadLine());

                        if (highCurSalary >= lowCurSalary && highCurSalary <= maxCurSalary)
                            break;
                        else
                            Console.WriteLine($"Invalid value. Please enter a value between {lowCurSalary} and {maxCurSalary}.");
                    }

                    var workersInRange = demo.Work.Where(w => w.CurSalary >= lowCurSalary && w.CurSalary <= highCurSalary).ToList();
                    var managersInRange = demo.Manage.Where(m => m.CurSalary >= lowCurSalary && m.CurSalary <= highCurSalary).ToList();

                    demo.ObjSort(workersInRange);
                    demo.ObjSort(managersInRange);

                    
                    Console.Clear();
                    Console.WriteLine(" ");
                    Console.WriteLine($"             -------------- Employee Salaries Between ${lowCurSalary} and ${highCurSalary} ----------------");
                    Console.WriteLine(" ");
                    foreach (var manager in managersInRange)
                        Console.WriteLine($"ID: {manager.WorkID}, Name: {manager.FirstName} {manager.LastName}, Years Worked: {manager.YearWorked}, Initial Salary: ${manager.InitSalary}, Current Salary: ${manager.CurSalary}");

                    
                    foreach (var worker in workersInRange)
                        Console.WriteLine($"ID: {worker.WorkID}, Name: {worker.FirstName} {worker.LastName}, Years Worked: {worker.YearWorked}, Initial Salary: ${worker.InitSalary}, Current Salary: ${worker.CurSalary}");
                    Console.WriteLine(" ");
                    Console.WriteLine($"              --------------------Sorted by Current Salary Decending----------------------");
                    Console.WriteLine(" ");

                }
                else if (choice == 2) //displays the data for all workers
                {
                    demo.ObjSort(demo.Work);

                    Console.Clear();
                    Console.WriteLine(" ");
                    Console.WriteLine($"               -------------- All Worker Data Ordered by Salary Decending  ----------------");
                    Console.WriteLine(" ");
                    foreach (var worker in demo.Work)
                        Console.WriteLine($"ID: {worker.WorkID}, Name: {worker.FirstName} {worker.LastName}, Years Worked: {worker.YearWorked}, Initial Salary: ${worker.InitSalary}, Current Salary: ${worker.CurSalary}");
                        Console.WriteLine(" ");
                    Console.WriteLine($"               ----------------------------------------------------------------------------");
                    Console.WriteLine(" ");
                }
                else if (choice == 3) //displays the data for all managers
                {
                    demo.ObjSort(demo.Manage);

                    Console.Clear();
                    Console.WriteLine(" ");
                    Console.WriteLine($"                   -------------- All Manager Data Ordered by Salary Decending ----------------");
                    Console.WriteLine(" ");
                    foreach (var manager in demo.Manage)
                        Console.WriteLine($"ID: {manager.WorkID}, Name: {manager.FirstName} {manager.LastName}, Years Worked: {manager.YearWorked}, Years Manager: {manager.YearWorked - manager.YearsWorker}, Initial Salary: ${manager.InitSalary}, Current Salary: ${manager.CurSalary}");
                    Console.WriteLine(" ");
                    Console.WriteLine($"                   ----------------------------------------------------------------------------");
                    Console.WriteLine(" ");
                }
                else if (choice == 4) //exits the program
                    break;
            }
        }
    }
}

