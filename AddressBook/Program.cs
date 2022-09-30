using AddressBook.Services;

//Initierar min MenuManager och sätter sökvägen för filen.
IMenuManager menu = new MenuManager($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\AddressBook.json");

//Hämtar en lista från sökvägen ifall det finns någon och kör sedan hela programmet tills att man väljer att avsluta det från huvudmenyn eller kryssar konsolen.
do
{
    

    menu.Init();
    menu.MainMenu();
    Console.ReadKey();

} while (true);

