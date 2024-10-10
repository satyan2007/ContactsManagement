using ContactsManagement.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly DataContactService _contactService;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(DataContactService contactService, ILogger<ContactsController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    // GET: api/contacts
    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetContacts()
    {
        var contacts = _contactService.GetAllContacts();
        return Ok(contacts);
    }

    // GET: api/contacts/5
    [HttpGet("{id}")]
    public ActionResult<Contact> GetContact(int id)
    {
        var contact = _contactService.GetContactById(id);
        if (contact == null)
        {
            return NotFound(new { Message = "Contact not found." });
        }
        return Ok(contact);
    }

    // POST: api/contacts
    [HttpPost]
    public ActionResult CreateContact([FromBody] Contact contact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, errorMessage) = _contactService.AddContact(contact);
        if (!success)
        {
            return BadRequest(new { Message = errorMessage });
        }

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    // PUT: api/contacts/5
    [HttpPut("{id}")]
    public IActionResult UpdateContact(int id, [FromBody] Contact contact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (success, errorMessage) = _contactService.UpdateContact(id, contact);
        if (!success)
        {
            return NotFound(new { Message = errorMessage });
        }

        return NoContent();
    }

    // DELETE: api/contacts/5
    [HttpDelete("{id}")]
    public IActionResult DeleteContact(int id)
    {
        var success = _contactService.DeleteContact(id);
        if (!success)
        {
            return NotFound(new { Message = "Contact not found." });
        }

        return NoContent();
    }
}












//using ContactsManagement.Server.Data;
//using ContactsManagement.Server.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//namespace ContactsManagement.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ContactsController : ControllerBase
//    {
//        private readonly DataService _contactService;

//        public ContactsController(DataService contactService)
//        {
//            _contactService = contactService;
//        }

//        [HttpGet]
//        public ActionResult<List<Contact>> GetContacts()
//        {
//            return Ok(_contactService.GetContacts());
//        }

//        [HttpGet("{id}")]
//        public ActionResult<Contact> GetContact(int id)
//        {
//            var contact = _contactService.GetContactById(id);
//            if (contact == null)
//                return NotFound();
//            return Ok(contact);
//        }

//        [HttpPost]
//        public ActionResult<Contact> AddContact([FromBody] Contact contact)
//        {
//            if (contact == null)
//            {
//                return BadRequest("Invalid contact data");
//            }

//            var newContact = _contactService.AddContact(contact);

//            return CreatedAtAction(nameof(GetContact), new { id = newContact.Id }, newContact);

//        }

//        [HttpPut("{id}")]
//        public ActionResult<Contact> UpdateContact(int id, [FromBody] Contact contact)
//        {
//            if (id != contact.Id)
//                return BadRequest();

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState); 
//            }

//            var contacts = _contactService.GetContacts();
//            var existingContact = contacts.FirstOrDefault(c => c.Id == id);

//            if (existingContact == null) return NotFound();

//            existingContact.FirstName = contact.FirstName;
//            existingContact.LastName = contact.LastName;
//            existingContact.Email = contact.Email;

//            _contactService.SaveContacts(contacts);
//            return Ok(existingContact);

//        }

//        [HttpDelete("{id}")]
//        public ActionResult DeleteContact(int id)
//        {         

//            var contacts = _contactService.GetContacts();
//            var contact = contacts.FirstOrDefault(c => c.Id == id);

//            if (contact == null) return NotFound();

//            contacts.Remove(contact);
//            _contactService.SaveContacts(contacts);

//            return NoContent();
//        }
//    }
//}