using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * @Author: Austin Campbell
 * For main menu options and logo display so the program.cs is not crowded.
*/


namespace CLI_CMS.src
{
    internal class Menu
    {
        private string _title = """
            _____________        _____________________     
            ___  __ \__(_)__________  __/__(_)_____  /____ 
            __  /_/ /_  /___  __ \_  /  __  /_  __  /_  _ \
            _  _, _/_  / __  /_/ /  /   _  / / /_/ / /  __/
            /_/ |_| /_/  _  .___//_/    /_/  \__,_/  \___/ 
                         /_/                               
            """;
        private string _options = """
            1. Login
            2. Register
            3. Exit
            """;

        private string _menu_options = """
            1. Create Client
            2. View Clients
            3. View A Client
            4. Update Client
            5. Delete Client
            6. Report
            7. Logout
            """;

        /// <summary>
        /// Displays the title of application (Maker)
        /// </summary>
        public void DisplayTitle()
        {
            WriteColor(ConsoleColor.Cyan, _title);
        }


        /// <summary>
        /// Shows the login menu only.
        /// </summary>
        public void ShowMenu()
        {
            WriteColor(ConsoleColor.Cyan, _options);
        }


        /// <summary>
        /// Shows the logged in menu.
        /// </summary>
        public void ShowLoggedInMenu()
        {
            WriteColor(ConsoleColor.Cyan, _menu_options);
        }


        /// <summary>
        /// Writes a message to the console with a specified color.
        /// Replaces the need for Console.WriteLine.
        /// </summary>
        /// <param name="color">ConsoleColor color</param>
        /// <param name="message">Desired message.</param>
        public void WriteColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
    }
}
