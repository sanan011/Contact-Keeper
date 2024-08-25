using System;
using System.Linq;

namespace ContactManagementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Contacts[] contactList = InitializeContacts();
            RunContactManagementApp(contactList);
        }

        // Initializes the contact list with default values
        static Contacts[] InitializeContacts()
        {
            return new Contacts[]
            {
                new Contacts(1, "Elchin", "Aliyev", "50-123-45-67"),
                new Contacts(2, "Gunel", "Mammadova", "51-234-56-78"),
                new Contacts(3, "Nijat", "Huseynov", "55-345-67-89"),
                new Contacts(4, "Aygun", "Ismayilova", "70-456-78-90"),
                new Contacts(5, "Elnur", "Guliyev", "77-567-89-01"),
                new Contacts(6, "Leyla", "Hajiyeva", "99-678-90-12"),
                new Contacts(7, "Vugar", "Hasanov", "10-789-01-23"),
                new Contacts(8, "Fidan", "Karimova", "40-890-12-34"),
                new Contacts(9, "Rashad", "Mehdiyev", "60-901-23-45"),
                new Contacts(10, "Zahra", "Ahmadova", "55-012-34-56")
            };
        }

        // Main application loop
        static void RunContactManagementApp(Contacts[] contactList)
        {
            int operationNum;

            do
            {
                DisplayMenu();
                operationNum = GetUserInput("Choose an operation (1-6): ");

                switch (operationNum)
                {
                    case 1:
                        AddContact(ref contactList);
                        break;
                    case 2:
                        DeleteContact(ref contactList);
                        break;
                    case 3:
                        EditContact(ref contactList);
                        break;
                    case 4:
                        ShowAllContacts(contactList);
                        break;
                    case 5:
                        SearchInContacts(contactList);
                        break;
                    case 6:
                        Console.WriteLine("Thank you for using the contact management system!");
                        break;
                    default:
                        Console.WriteLine("Invalid operation! Please select a valid option.");
                        break;
                }

            } while (operationNum != 6);

            Environment.Exit(0);
        }

        // Displays the main menu to the user
        static void DisplayMenu()
        {
            Console.WriteLine("\n---------------------- Contact Management --------------------");
            Console.WriteLine("|                       1. Add a contact                     |");
            Console.WriteLine("|                     2. Delete a contact                    |");
            Console.WriteLine("|                      3. Edit a contact                     |");
            Console.WriteLine("|                    4. Show all contacts                    |");
            Console.WriteLine("|                     5. Search contacts                     |");
            Console.WriteLine("|                          6. Quit                           |");
            Console.WriteLine("--------------------------------------------------------------\n");
        }

        // Retrieves and validates user input for operations
        static int GetUserInput(string message)
        {
            int input;
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    return input;
                }
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }

        // Adds a new contact to the list
        static void AddContact(ref Contacts[] contactList)
        {
            int id = contactList.Max(c => c.Id) + 1;
            string name = GetValidatedInput("First Name: ");
            string surname = GetValidatedInput("Surname: ");
            string number = GetValidatedPhoneNumber("Phone Number: ");

            Array.Resize(ref contactList, contactList.Length + 1);
            contactList[^1] = new Contacts(id, name, surname, number);
            Console.WriteLine($"Contact added successfully. The ID of the newly added contact : {id}");
        }

        // Deletes a contact based on ID
        static void DeleteContact(ref Contacts[] contactList)
        {
            int userId = GetUserInput("Enter the ID of the contact to delete: ");
            int index = Array.FindIndex(contactList, c => c.Id == userId);

            if (index >= 0)
            {
                contactList = contactList.Where((c, i) => i != index).ToArray();
                Console.WriteLine("Contact deleted successfully.");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        // Edits an existing contact based on ID
        static void EditContact(ref Contacts[] contactList)
        {
            int userId = GetUserInput("Enter the ID of the contact to edit: ");
            int index = Array.FindIndex(contactList, c => c.Id == userId);

            if (index >= 0)
            {
                string name = GetValidatedInput("New First Name: ");
                string surname = GetValidatedInput("New Surname: ");
                string number = GetValidatedPhoneNumber("New Phone Number: ");

                contactList[index].Name = name;
                contactList[index].Surname = surname;
                contactList[index].PhoneNumber = number;

                Console.WriteLine("Contact updated successfully.");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        // Displays all contacts
        static void ShowAllContacts(Contacts[] contactList)
        {
            if (contactList.Length == 0)
            {
                Console.WriteLine("No contacts available.");
                return;
            }

            Console.WriteLine("\n--- Contact List ---");
            foreach (var contact in contactList)
            {
                Console.WriteLine($"{contact.Id}. {contact.Name} {contact.Surname} - {contact.PhoneNumber}");
            }
        }

        // Searches for contacts based on user criteria
        static void SearchInContacts(Contacts[] contactList)
        {
            Console.WriteLine("Search by: 1. Name, 2. Surname, 3. Phone Number");
            int methodNum = GetUserInput("Choose a search method (1-3): ");

            switch (methodNum)
            {
                case 1:
                    SearchByField(contactList, "name");
                    break;
                case 2:
                    SearchByField(contactList, "surname");
                    break;
                case 3:
                    SearchByField(contactList, "phone number");
                    break;
                default:
                    Console.WriteLine("Invalid search method.");
                    break;
            }
        }

        // Searches for contacts by a specific field
        static void SearchByField(Contacts[] contactList, string field)
        {
            string searchTerm = GetValidatedInput($"Enter the {field}: ").ToLower();
            var results = contactList.Where(c =>
                field == "name" && c.Name.ToLower() == searchTerm ||
                field == "surname" && c.Surname.ToLower() == searchTerm ||
                field == "phone number" && c.PhoneNumber == searchTerm).ToArray();

            if (results.Any())
            {
                foreach (var contact in results)
                {
                    Console.WriteLine($"{contact.Id}. {contact.Name} {contact.Surname} - {contact.PhoneNumber}");
                }
            }
            else
            {
                Console.WriteLine($"No contacts found with the given {field}.");
            }
        }

        // Validates general string inputs
        static string GetValidatedInput(string message)
        {
            string input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(input));

            return input;
        }

        // Validates phone number input based on custom rules
        static string GetValidatedPhoneNumber(string message)
        {
            string number;
            do
            {
                Console.Write(message);
                number = Console.ReadLine();
            } while (!IsValidPhoneNumber(number));

            return number;
        }

        // Validates the phone number format
        static bool IsValidPhoneNumber(string number)
        {
            string[] validPrefixes = { "55", "50", "51", "70", "77", "99", "10", "40", "60" };
            string[] parts = number.Split('-');

            return parts.Length == 4 && validPrefixes.Contains(parts[0]) && parts.All(IsAllDigits);
        }

        // Checks if a string contains only digits
        static bool IsAllDigits(string str)
        {
            return str.All(char.IsDigit);
        }

        // Contact class definition
        public class Contacts
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string PhoneNumber { get; set; }

            public Contacts(int id, string name, string surname, string phoneNumber)
            {
                Id = id;
                Name = name;
                Surname = surname;
                PhoneNumber = phoneNumber;
            }
        }
    }
}
