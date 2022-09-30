using AddressBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Services
{
    internal interface IMenuManager
    {
        // Interface som visar vilka metoder min MenuManager har
        public List<Contact> Init();
        public void MainMenu();
        public void CreateNewContact();
        public void ViewAllContacts();
        public void ViewDetailedContact();
        public void EditContact(ref List<Contact> _contacts, Guid id);
        public void DeleteContact(Guid id);
    }
    internal class MenuManager : IMenuManager

    {
        // Skapar lista av typen kontakter
        private List<Contact> _contacts = new();

        // Initierar min FileManager i MenuMager så jag får tillgång till dens metoder och i kontrsuktorn flyttar att sökvägen ska sättas i MenuManager.
        private IFileManager _fileManager;
        public MenuManager(string filePath)
        {           
            _fileManager = new FileManager(filePath);
        }

        //Metod för att hämta en lista om det finns en vid start av programmet.
        public List<Contact> Init()
        {
            _contacts = _fileManager.Read();
            return _contacts.ToList();
        }

        // Metod som skapar min huvudmeny för adressboken som och ger användaren olika val samt en default text vid fel inmatat val av användaren.
        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("---- ADDRESSBOOK -----");
            Console.WriteLine("1. Create new contact");
            Console.WriteLine("2. View all contacts");
            Console.WriteLine("3. View detailed contact");
            Console.WriteLine("Q. Quit application");
            Console.WriteLine();
            Console.Write("Choose one option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CreateNewContact();
                    break;

                case "2":
                    ViewAllContacts();
                    break;

                case "3":
                    ViewDetailedContact();
                    break;

                case "Q":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid option, try again!");
                    break;
            }           
        }

        // Metod för att skapa en ny kontakt och spara ned i listan.
        public void CreateNewContact()
        {
            var contact = new Contact();

            Console.Clear();
            Console.WriteLine("****** CREATE NEW CONTACT ******");
            Console.Write("First Name: ");
            contact.FirstName = Console.ReadLine() ?? "";
            Console.Write("Last Name: ");
            contact.LastName = Console.ReadLine() ?? "";
            Console.Write("Street Name: ");
            contact.StreetName = Console.ReadLine() ?? "";
            Console.Write("Postal Code: ");
            contact.PostalCode = Console.ReadLine() ?? "";
            Console.Write("City: ");
            contact.City = Console.ReadLine() ?? "";
            Console.Write("Phone Number: ");
            contact.PhoneNumber = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            contact.Email = Console.ReadLine() ?? "";

            //Tvingar användaren att mata in både Förnamn och Telefonnummer för att kunna spara kontakten.
            if (contact.FirstName == "" || contact.PhoneNumber == "")
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid values added!");
                Console.WriteLine("Firstname and Phonenumber must be entered, try again!");
            }
            else
            {
                _contacts.Add(contact);
                _fileManager.Save(_contacts);
                Console.WriteLine();
                Console.WriteLine("Contact added!");
            }
        }

        //Metod för visa samtliga kontakter som finns i listan, men endast ID, Förnamn och Efternamn. Alltså in all info av en kontakt.
        public void ViewAllContacts()
        {
            _fileManager.Read();
            Console.Clear();
            Console.WriteLine("****** ALL CONTACTS ******");
            foreach (var contact in _contacts)            
                Console.WriteLine($"{contact.Id} - {contact.FirstName} {contact.LastName}");

            Console.ReadKey();
            MainMenu();
        }

        //Metod för att visa en detlajerad kontakt.
        public void ViewDetailedContact()
        {
            //Läser in listan och visar samtliga kontakter likt metoden "ViewAllContacts
            _fileManager.Read();
            Console.Clear();
            Console.WriteLine("****** ALL CONTACTS ******");
            foreach (var contact in _contacts)
                Console.WriteLine($"{contact.Id} - {contact.FirstName} {contact.LastName}");

            try
            {
                //Ber användaren mata in ID på kontakten som han vill se.
                Console.WriteLine();
                Console.Write("Please enter the ID of the contact you wish to see: ");
                var id = Console.ReadLine();
                if (!string.IsNullOrEmpty(id))
                {
                    var contact = _contacts.FirstOrDefault(x => x.Id == new Guid(id));

                    Console.Clear();
                    Console.WriteLine("****** DETAILED CONTACT ******");
                    Console.WriteLine($"ID: {contact?.Id}");
                    Console.WriteLine($"Name: {contact?.FirstName} {contact?.LastName}");
                    Console.WriteLine($"Address: {contact?.StreetName}, {contact?.PostalCode} {contact?.City}");
                    Console.WriteLine($"Phonenumber: {contact?.PhoneNumber}");
                    Console.WriteLine($"Email: {contact?.Email.ToLower()}");

                    // När den detaljerade kontakten visas, frågar jag användaren nedan alternativ
                    Console.WriteLine();
                    Console.WriteLine("1. Edit contact");
                    Console.WriteLine("2. Delete contact");
                    Console.WriteLine("3. Back to main menu");
                    Console.Write("Choose one option: ");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            //Använder metoden EditContact
                            EditContact(ref _contacts, contact.Id);
                            break;

                        case "2":
                            //Använder metoden DeleteContact
                            DeleteContact(contact.Id);
                            break;

                        case "3":
                            //Skickar tillbaka användaren till huvudmenyn
                            MainMenu();
                            break;

                        default:
                            //Vid ogiltigt inmatat alternativ skickas användaren tillbaka till huvudmenyn
                            Console.WriteLine("Invalid Option");
                            Console.ReadKey();
                            MainMenu();
                            break;
                    }
                }
                else
                {
                    //När användaren inte anger något ID visas detta felmeddelande och får försöka igen.
                    Console.WriteLine("Invalid ID entered, try again!");
                    Console.ReadKey();
                    ViewDetailedContact();
                }
            }
            catch
            {
                //När användaren anger ogiltigt ID visas detta felmeddelande och får försöka igen.
                Console.WriteLine("Invalid ID entered, try again!");
                Console.ReadKey();
                ViewDetailedContact();
            }
        }

        //Metod för att redigera en befintlig kontakt i listan.
        public void EditContact(ref List<Contact> _contacts, Guid id)
        {
            //Letar fram indexet av kontakten som man angett ID för i metoden ViewDetailedContact.
            string index = _contacts.FindIndex(x => x.Id == id).ToString();
            int _index = int.Parse(index);

            //Visar kontaktens detaljer och frågar sedan användaren vad han vill upptadera i kontakten.
            Console.Clear();
            Console.WriteLine("****** EDIT CONTACT ******");
            Console.WriteLine($"ID: {_contacts[_index].Id}");
            Console.WriteLine($"Name: {_contacts[_index].FirstName} {_contacts[_index].LastName}");
            Console.WriteLine($"Address: {_contacts[_index].StreetName}, {_contacts[_index].PostalCode} {_contacts[_index].City}");
            Console.WriteLine($"Phonenumber: {_contacts[_index].PhoneNumber}");
            Console.WriteLine($"Email: {_contacts[_index].Email.ToLower()}");
            Console.WriteLine();

            Console.WriteLine("1. First Name");
            Console.WriteLine("2. Last Name");
            Console.WriteLine("3. Street Name");
            Console.WriteLine("4. Postal Code");
            Console.WriteLine("5. City");
            Console.WriteLine("6. Phone Number");
            Console.WriteLine("7. Email");
            Console.WriteLine("8. Back to Main Menu");
            Console.WriteLine();
            Console.Write("Choose what you like to Edit: ");

            // Uppdaterar med nytt inmatat värde av valet användaren gjorde i steget ovan och sparar refererad lista.
            // Förnamn och och telefonnummer måste ha ett värde annars går programmet tillbaka till huvudmenyn.
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.WriteLine("First Name must have a value!");
                    Console.Write("Enter new First Name: ");
                    _contacts[_index].FirstName = Console.ReadLine() ?? "";
                    

                    if (_contacts[_index].FirstName != "")
                    {
                        _fileManager.Save(_contacts);
                        Console.WriteLine();
                        Console.WriteLine("Firstname has been updated");
                        Console.ReadKey();
                        EditContact(ref _contacts, id);
                    }
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.Write("Enter new Last Name: ");
                    _contacts[_index].LastName = Console.ReadLine() ?? "";
                    _fileManager.Save(_contacts);

                    Console.WriteLine();
                    Console.WriteLine("Lastname has been updated!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.Write("Enter new Street Name: ");
                    _contacts[_index].StreetName = Console.ReadLine() ?? "";
                    _fileManager.Save(_contacts);

                    Console.WriteLine();
                    Console.WriteLine("Streetname has been updated!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.Write("Enter new Postal Code: ");
                    _contacts[_index].PostalCode = Console.ReadLine() ?? "";
                    _fileManager.Save(_contacts);

                    Console.WriteLine();
                    Console.WriteLine("Postal code has been updated!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;

                case "5":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.Write("Enter new City: ");
                    _contacts[_index].City = Console.ReadLine() ?? "";
                    _fileManager.Save(_contacts);

                    Console.WriteLine();
                    Console.WriteLine("City has been updated!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;

                case "6":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.WriteLine("Phone Number must have a value!");
                    Console.Write("Enter new Phone Number: ");
                    _contacts[_index].PhoneNumber = Console.ReadLine() ?? "";

                    if (_contacts[_index].PhoneNumber != "")
                    {
                        _fileManager.Save(_contacts);
                        Console.WriteLine();
                        Console.WriteLine("Phonenumber has been updated");
                        Console.ReadKey();
                        EditContact(ref _contacts, id);
                    }
                    break;

                case "7":
                    Console.Clear();
                    Console.WriteLine("****** EDIT CONTACT ******");
                    Console.WriteLine();
                    Console.Write("Enter new Email: ");
                    _contacts[_index].Email = Console.ReadLine() ?? "";
                    _fileManager.Save(_contacts);

                    Console.WriteLine();
                    Console.WriteLine("Email has been updated!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;

                case "8":
                    MainMenu();
                    break;

                default:                   
                    Console.WriteLine();
                    Console.WriteLine("Invalid option, try again!");
                    Console.ReadKey();
                    EditContact(ref _contacts, id);
                    break;
            }
        }

        //Metod för att ta bort en kontakt i listan.
        public void DeleteContact(Guid id)
        {
            //Sorterar bort kontakten från listan som har det ID som matatdes in i metoden ViewDetailedContact och sparar listan.
            _contacts =_contacts.Where(x => x.Id != id).ToList();
            _fileManager.Save(_contacts);

            Console.WriteLine();
            Console.WriteLine("Contact deleted!");           
        }
    }
}
