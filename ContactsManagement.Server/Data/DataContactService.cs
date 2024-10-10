//using ContactsManagement.Server.Models;
//using System.Text.Json;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;

//namespace ContactsManagement.Server.Data
//{
//    public class DataService
//    {
//        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "contacts.json");

//        public List<Contact> GetContacts()
//        {
//            if (!File.Exists(_filePath))
//            {
//                return new List<Contact>();
//            }

//            var jsonData = File.ReadAllText(_filePath);
//            return JsonSerializer.Deserialize<List<Contact>>(jsonData) ?? new List<Contact>();
//        }

//        public void SaveContacts(List<Contact> contacts)
//        {
//            var jsonData = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
//            File.WriteAllText(_filePath, jsonData);
//        }
//        public Contact AddContact(Contact newContact)
//        {
//            var contacts = GetContacts();
//            newContact.Id = contacts.Max(c => c.Id) + 1;  // Generate unique ID
//            contacts.Add(newContact);
//            SaveContacts(contacts);
//            return newContact;
//        }
//        //public Contact GetContactById(int id)
//        //{
//        //    var contacts = GetContacts();
//        //    return contacts.FirstOrDefault(c => c.Id == id).;
//        //}

//        public Contact GetContactById(int id)
//        {
//            // Assuming GetContacts() returns an IEnumerable<Contact>
//            var contacts = GetContacts();

//            // Check if the contact exists with the provided ID
//            var contact = contacts.FirstOrDefault(c => c.Id == id);

//            if (contact == null)            {

//                throw new Exception($"Contact with ID {id} not found.");
//            }

//            return contact;
//        }

//    }
//}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using ContactsManagement.Server.Models;

public class DataContactService
{
    private readonly string _filePath;
    private readonly ILogger<DataContactService> _logger;

    public DataContactService(IHostEnvironment environment, ILogger<DataContactService> logger)
    {
        
        var dataFolder = Path.Combine(environment.ContentRootPath, "Data");
        _filePath = Path.Combine(dataFolder, "contacts.json");
       
        _logger = logger;
    }

    // Get all contacts
    public List<Contact> GetAllContacts()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Contact>>(jsonData) ?? new List<Contact>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading contacts from JSON file.");
        }
        return new List<Contact>();
    }

    // Get contact by ID
    public Contact GetContactById(int id)
    {
        var contacts = GetAllContacts();
        return contacts.FirstOrDefault(c => c.Id == id);
    }

    // Add new contact
    public (bool Success, string ErrorMessage) AddContact(Contact contact)
    {
        // Validate the contact
        var validationResults = ValidateContact(contact);
        if (!validationResults.IsValid)
        {
            return (false, string.Join(", ", validationResults.Errors.Select(e => e.ErrorMessage)));
        }

        var contacts = GetAllContacts();
        contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
        contacts.Add(contact);
        SaveToFile(contacts);
        return (true, string.Empty);
    }

    // Update existing contact
    public (bool Success, string ErrorMessage) UpdateContact(int id, Contact updatedContact)
    {
        var validationResults = ValidateContact(updatedContact);
        if (!validationResults.IsValid)
        {
            return (false, string.Join(", ", validationResults.Errors.Select(e => e.ErrorMessage)));
        }

        var contacts = GetAllContacts();
        var contact = contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null)
        {
            return (false, "Contact not found.");
        }

        contact.FirstName = updatedContact.FirstName;
        contact.LastName = updatedContact.LastName;
        contact.Email = updatedContact.Email;
        SaveToFile(contacts);
        return (true, string.Empty);
    }

    // Delete contact
    public bool DeleteContact(int id)
    {
        var contacts = GetAllContacts();
        var contact = contacts.FirstOrDefault(c => c.Id == id);
        if (contact == null) return false;

        contacts.Remove(contact);
        SaveToFile(contacts);
        return true;
    }

    // Helper method to validate a contact using data annotations
    private ValidationResultCollection ValidateContact(Contact contact)
    {
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(contact, new ValidationContext(contact), validationResults, true);
        return new ValidationResultCollection { IsValid = isValid, Errors = validationResults };
    }

    // Helper method to save the list to the JSON file
    private void SaveToFile(List<Contact> contacts)
    {
        var jsonData = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }
}

// Custom ValidationResultCollection class
public class ValidationResultCollection
{
    public bool IsValid { get; set; }
    public IEnumerable<ValidationResult> Errors { get; set; }
}




