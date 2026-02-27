using System;
using System.Collections.Generic;
using System.Linq;

namespace Exercise04
{
    class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }  // Personal, Work, etc.
        
        public override string ToString()
        {
            return $"{Name} | {Email} | {Phone} | {Category}";
        }
    }
    
    class ContactManager
    {
        private Dictionary<string, Contact> contacts = new Dictionary<string, Contact>();
        
        public bool AddContact(Contact contact)
        {
            if (contacts.ContainsKey(contact.Email))
                return false;
            
            contacts[contact.Email] = contact;
            return true;
        }
        
        public bool RemoveContact(string email)
        {
            return contacts.Remove(email);
        }
        
        public Contact? FindByEmail(string email)
        {
            contacts.TryGetValue(email, out Contact? contact);
            return contact;
        }
        
        public List<Contact> FindByName(string partialName)
        {
            return contacts.Values
                .Where(c => c.Name.Contains(partialName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        // Find by category
        public List<Contact> FindByCategory(string category)
        {
            return contacts.Values
                .Where(c => c.Category.Contains(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        public List<Contact> GetAllContacts()
        {
            return contacts.Values.OrderBy(c => c.Name).ToList();
        }
        
        // Update contact
        public bool UpdateContact(string email, Contact updatedContact)
        {
            if (!contacts.ContainsKey(email))
                return false;

            contacts[email] = updatedContact;
            return true;
        }
        
        // Export to CSV
        public void ExportToCsv(string filename)
        {
            using (var writer = new System.IO.StreamWriter(filename))
            {
                writer.WriteLine("Name,Email,Phone,Category");
                foreach (var contact in contacts.Values)
                {
                    writer.WriteLine($"{contact.Name},{contact.Email},{contact.Phone},{contact.Category}");
                }
            }
            Console.WriteLine("✓ Contacts exported to CSV.");
        }

        // Import from CSV
        public void ImportFromCsv(string filename)
        {
            try
            {
                using (var reader = new System.IO.StreamReader(filename))
                {
                    reader.ReadLine(); // Skip header
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line?.Split(',');

                        if (values?.Length == 4)
                        {
                            Contact contact = new Contact
                            {
                                Name = values[0],
                                Email = values[1],
                                Phone = values[2],
                                Category = values[3]
                            };

                            AddContact(contact);
                        }
                    }
                }
                Console.WriteLine("✓ Contacts imported from CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing from CSV: {ex.Message}");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            ContactManager manager = new ContactManager();
            bool running = true;
            
            while (running)
            {
                Console.WriteLine("\n=== Contact Manager ===");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. Remove Contact");
                Console.WriteLine("3. Find by Name");
                Console.WriteLine("4. Find by Category");
                Console.WriteLine("5. List All");
                Console.WriteLine("6. Update Contact");
                Console.WriteLine("7. Export to CSV");
                Console.WriteLine("8. Import from CSV");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        AddContactMenu(manager);
                        break;
                    case "2":
                        RemoveContactMenu(manager);
                        break;
                    case "3":
                        FindByNameMenu(manager);
                        break;
                    case "4":
                        FindByCategoryMenu(manager);
                        break;
                    case "5":
                        ListAllContacts(manager);
                        break;
                    case "6":
                        UpdateContactMenu(manager);
                        break;
                    case "7":
                        ExportToCsvMenu(manager);
                        break;
                    case "8":
                        ImportFromCsvMenu(manager);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddContactMenu(ContactManager manager)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Phone: ");
            string phone = Console.ReadLine() ?? "";
            Console.Write("Category: ");
            string category = Console.ReadLine() ?? "";
            
            Contact contact = new Contact
            {
                Name = name,
                Email = email,
                Phone = phone,
                Category = category
            };
            
            if (manager.AddContact(contact))
                Console.WriteLine("✓ Contact added");
            else
                Console.WriteLine("✗ Contact already exists");
        }

        static void RemoveContactMenu(ContactManager manager)
        {
            Console.Write("Enter the email of the contact to remove: ");
            string email = Console.ReadLine() ?? "";
            
            if (manager.RemoveContact(email))
                Console.WriteLine("✓ Contact removed");
            else
                Console.WriteLine("✗ Contact not found");
        }

        static void FindByNameMenu(ContactManager manager)
        {
            Console.Write("Enter partial name to search: ");
            string partialName = Console.ReadLine() ?? "";
            
            var foundContacts = manager.FindByName(partialName);
            foreach (var contact in foundContacts)
            {
                Console.WriteLine(contact);
            }
        }

        static void FindByCategoryMenu(ContactManager manager)
        {
            Console.Write("Enter category to search: ");
            string category = Console.ReadLine() ?? "";
            
            var foundContacts = manager.FindByCategory(category);
            foreach (var contact in foundContacts)
            {
                Console.WriteLine(contact);
            }
        }

        static void ListAllContacts(ContactManager manager)
        {
            var allContacts = manager.GetAllContacts();
            foreach (var contact in allContacts)
            {
                Console.WriteLine(contact);
            }
        }

        static void UpdateContactMenu(ContactManager manager)
        {
            Console.Write("Enter the email of the contact to update: ");
            string email = Console.ReadLine() ?? "";
            
            var existingContact = manager.FindByEmail(email);
            
            if (existingContact != null)
            {
                Console.Write("New Name (leave blank to keep the current name): ");
                string name = Console.ReadLine() ?? existingContact.Name;
                Console.Write("New Phone (leave blank to keep the current phone): ");
                string phone = Console.ReadLine() ?? existingContact.Phone;
                Console.Write("New Category (leave blank to keep the current category): ");
                string category = Console.ReadLine() ?? existingContact.Category;
                
                Contact updatedContact = new Contact
                {
                    Name = name,
                    Email = existingContact.Email,
                    Phone = phone,
                    Category = category
                };

                if (manager.UpdateContact(email, updatedContact))
                    Console.WriteLine("✓ Contact updated");
                else
                    Console.WriteLine("✗ Contact update failed");
            }
            else
            {
                Console.WriteLine("✗ Contact not found");
            }
        }

        static void ExportToCsvMenu(ContactManager manager)
        {
            Console.Write("Enter the filename to export to: ");
            string filename = Console.ReadLine() ?? "contacts.csv";
            manager.ExportToCsv(filename);
        }

        static void ImportFromCsvMenu(ContactManager manager)
        {
            Console.Write("Enter the filename to import from: ");
            string filename = Console.ReadLine() ?? "contacts.csv";
            manager.ImportFromCsv(filename);
        }
    }
}
