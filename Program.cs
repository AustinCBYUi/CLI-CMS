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


displayMenu();

void displayMenu()
{
    var CYAN = ConsoleColor.Cyan;
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
                        menu.WriteColor(CYAN, "Login Successful\n");
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
        menu.WriteColor(ConsoleColor.Green, "Enter your Choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            //Create Client
            case "1":
                break;
            //View Client
            case "2":
                break;
            //Update Client
            case "3":
                break;
            //Delete Client
            case "4":
                break;
            //Generate Report
            case "5":
                break;
            //Logout
            case "6":
                loggedOut = true;
                displayMenu();
                break;
        }
    }
}


