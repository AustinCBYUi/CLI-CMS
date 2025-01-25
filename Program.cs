/*
 * @Author: Austin Campbell
 * 
 */

using CLI_CMS.src;
using Microsoft.Data.Sqlite;

//Class initialization and DB
Menu menu = new Menu();
var dbHelper = new DatabaseHelper("cms.db");

menu.DisplayTitle();

//Constants
var GREEN = ConsoleColor.Green;
var RED = ConsoleColor.Red;
var CYAN = ConsoleColor.Cyan;

displayMenu();

void displayMenu()
{
    bool loggedIn = false;

    while (!loggedIn)
    {
        menu.ShowMenu();
        menu.WriteColor(CYAN, "Enter your Choice: ");
        string choice = Console.ReadLine();

        switch(choice)
        {
            //Login
            case "1":
                {
                    menu.WriteColor(CYAN, "Username: ");
                    Console.Write(">> ");
                    string username = Console.ReadLine();
                    menu.WriteColor(CYAN, "Password: ");
                    Console.Write(">> ");
                    string password = Console.ReadLine();
                    bool isValid = dbHelper.FindUser(username, password);

                    if (isValid)
                    {
                        Console.Clear();
                        menu.WriteColor(GREEN, "Login Successful\n");
                        //display logged in menu
                        loggedIn = true;
                        displayLoggedInMessage();
                    }
                    else
                    {
                        menu.WriteColor(CYAN, "Login Failed\n");
                        displayMenu();
                    }
                    break;
                }
            //Register
            case "2":
                {
                    menu.WriteColor(CYAN, "Username: ");
                    Console.Write(">> ");
                    string username = Console.ReadLine();
                    menu.WriteColor(CYAN, "Password: ");
                    Console.Write(">> ");
                    string password = Console.ReadLine();
                    string hashedPassword = Hasher.Hash(password);
                    dbHelper.AddUser(username, hashedPassword);
                    Console.Clear();
                    menu.WriteColor(GREEN, "User Registered\n");
                    break;
                }
            //Exit
            case "3":
                Environment.Exit(0);
                break;
            default:
                menu.WriteColor(CYAN, "Invalid Choice\n");
                break;
        }
    }
}


void displayLoggedInMessage()
{
    bool loggedOut = false;

    while (!loggedOut)
    {
        menu.ShowLoggedInMenu();
        menu.WriteColor(GREEN, "\nEnter your Choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            //Create Client
            case "1":
                {
                    menu.WriteColor(CYAN, "First Name: ");
                    Console.Write(">> ");
                    string first_name = Console.ReadLine();
                    menu.WriteColor(CYAN, "Last Name: ");
                    Console.Write(">> ");
                    string last_name = Console.ReadLine();
                    menu.WriteColor(CYAN, "Email: ");
                    Console.Write(">> ");
                    string email = Console.ReadLine();
                    menu.WriteColor(CYAN, "Phone: ");
                    Console.Write(">> ");
                    string phone = Console.ReadLine();
                    menu.WriteColor(CYAN, "Address: ");
                    Console.Write(">> ");
                    string address = Console.ReadLine();
                    dbHelper.AddClient(first_name, last_name, email, phone, address);
                    menu.WriteColor(GREEN, "Client Added\n");
                    break;
                }
            //View Client
            case "2":
                dbHelper.ViewClients();
                break;
            //View a client
            case "3":
                {
                    menu.WriteColor(CYAN, "First or Last Name search? (1 or 2): ");
                    Console.Write(">> ");
                    string searchChoice = Console.ReadLine();
                    if (searchChoice == "1")
                    {
                        menu.WriteColor(CYAN, "First Name: ");
                        Console.Write(">> ");
                        string first_name = Console.ReadLine();
                        dbHelper.ViewAClient(first_name, null);
                    }
                    else if (searchChoice == "2")
                    {
                        menu.WriteColor(CYAN, "Last Name: ");
                        Console.Write(">> ");
                        string last_name = Console.ReadLine();
                        dbHelper.ViewAClient(null, last_name);
                    }
                    else
                    {
                        menu.WriteColor(CYAN, "Invalid Choice\n");
                    }
                    break;
                }
            //Update Client
            case "4":
                {
                    menu.WriteColor(CYAN, "First or Last Name serach? (1 or 2):");
                    Console.Write(">> ");
                    string searchChoice = Console.ReadLine();
                    if (searchChoice == "1")
                    {
                        menu.WriteColor(CYAN, "First Name: ");
                        Console.Write(">> ");
                        string search_first_name = Console.ReadLine();
                        //Displays that user if it exists
                        dbHelper.ViewAClient(search_first_name, null);
                        int id = dbHelper.GetClientId(search_first_name, null);

                        Console.WriteLine("\n");

                        menu.WriteColor(CYAN, "First Name: ");
                        Console.Write(">> ");
                        //First name is already defined
                        string first_name = Console.ReadLine();

                        menu.WriteColor(CYAN, "Last Name: ");
                        Console.Write(">> ");
                        string last_name = Console.ReadLine();

                        menu.WriteColor(CYAN, "Email: ");
                        Console.Write(">> ");
                        string email = Console.ReadLine();

                        menu.WriteColor(CYAN, "Phone: ");
                        Console.Write(">> ");
                        string phone = Console.ReadLine();

                        menu.WriteColor(CYAN, "Address: ");
                        Console.Write(">> ");
                        string address = Console.ReadLine();

                        dbHelper.UpdateClient(id, first_name, last_name, email, phone, address);
                        menu.WriteColor(GREEN, "Client Updated\n");
                    }
                    else if (searchChoice == "2")
                    {
                        menu.WriteColor(CYAN, "Last Name: ");
                        Console.Write(">> ");
                        string search_last_name = Console.ReadLine();

                        dbHelper.ViewAClient(null, search_last_name);
                        int id = dbHelper.GetClientId(null, search_last_name);

                        Console.WriteLine("\n");

                        menu.WriteColor(CYAN, "First Name: ");
                        Console.Write(">> ");
                        string first_name = Console.ReadLine();

                        menu.WriteColor(CYAN, "Last Name: ");
                        Console.Write(">> ");
                        //Last name is already defined
                        string last_name = Console.ReadLine();

                        menu.WriteColor(CYAN, "Email: ");
                        Console.Write(">> ");
                        string email = Console.ReadLine();

                        menu.WriteColor(CYAN, "Phone: ");
                        Console.Write(">> ");
                        string phone = Console.ReadLine();

                        menu.WriteColor(CYAN, "Address: ");
                        Console.Write(">> ");
                        string address = Console.ReadLine();

                        dbHelper.UpdateClient(id, first_name, last_name, email, phone, address);
                        menu.WriteColor(GREEN, "Client Updated\n");
                    }
                    else
                    {
                        menu.WriteColor(CYAN, "Invalid Choice\n");
                    }
                    break;
                }
            //Delete Client
            case "5":
                {
                    menu.WriteColor(CYAN, "First or Last Name search? (1 or 2): ");
                    Console.Write(">> ");
                    string searchChoice = Console.ReadLine();
                    if (searchChoice == "1")
                    {
                        menu.WriteColor(CYAN, "First Name: ");
                        Console.Write(">> ");
                        string first_name = Console.ReadLine();
                        dbHelper.DeleteClient(first_name, null);
                        menu.WriteColor(GREEN, first_name + " has been deleted\n");
                    }
                    else if (searchChoice == "2")
                    {
                        menu.WriteColor(CYAN, "Last Name: ");
                        Console.Write(">> ");
                        string last_name = Console.ReadLine();
                        dbHelper.DeleteClient(null, last_name);
                        menu.WriteColor(GREEN, last_name + " has been deleted\n");
                    }
                    else
                    {
                        menu.WriteColor(CYAN, "Invalid Choice\n");
                    }
                    break;
                }
            //Generate Report
            case "6":
                dbHelper.GenerateReport();
                break;
            //Logout
            case "7":
                Console.Clear();
                menu.WriteColor(ConsoleColor.Red, "\nLogging out...\n\n");
                loggedOut = true;
                displayMenu();
                break;
        }
    }
}


