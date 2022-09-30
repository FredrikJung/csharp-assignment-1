using AddressBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Services
{
    internal interface IFileManager
    {
        // Interface som visar vilka metoder min FileManager har.
        public void Save(List<Contact> list);
        public List<Contact> Read();
    }
    internal class FileManager : IFileManager
    {

        // Skapar ett fält för sökvägen
        private string _filePath;

        //Konstruktor för FileManager där sökvägen ska sättas (flyttas sedan till MenuManager)
        public FileManager(string filePath)
        {
            _filePath = filePath;
        }

        //Metod för att spara lista till sökvägen i Json format samt formatera listan så den blir mer lättläslig.
        public void Save(List<Contact> list)
        {
            using var sw = new StreamWriter(_filePath);
            sw.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        // Metod för att hämta lista av kontakter från sökvägen i json format samt konvertera till C# lista.
        public List<Contact> Read()
        {
            var list = new List<Contact>();

            try
            {
                using var sr = new StreamReader(_filePath);
                list = JsonConvert.DeserializeObject<List<Contact>>(sr.ReadToEnd());
            }
            catch { }

            return list;
        }
    }
}
