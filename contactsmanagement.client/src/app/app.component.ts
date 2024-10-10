import { Component, OnInit } from '@angular/core';
import { Contact } from './models/contact.model';
import { ContactService } from './services/contact.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  contacts: Contact[] = [];
  contactToEdit: Contact | null = null;
  showForm: boolean = false;

  constructor(private contactService: ContactService) { }

  ngOnInit() {
    this.loadContacts();
  }
 

  // This function is triggered when we click on "Add New Contact"
  onAddNewContact(): void {
    this.contactToEdit = null; // Reset the contactToEdit to null
    this.showForm = true;      // Show the form
  }
  // Load all contacts from the backend
  loadContacts(): void {
    this.contactService.getContacts().subscribe(
      (contacts: Contact[]) => {
        this.contacts = contacts;
      },
      (error) => {
        console.error('Error fetching contacts', error);
      }
    );
  }

  // Handle the event of adding a new contact or updating an existing contact
  onContactAdded(contact: Contact): void {
    if (this.contactToEdit) {
      // Update contact
      this.contactService.updateContact(this.contactToEdit.id, contact).subscribe(
        () => {
          this.loadContacts();
          this.resetForm();
        },
        (error) => {
          console.error('Error updating contact', error);
        }
      );
    } else {
      //// Add new contact
       // this.contactService.addContact(contact).subscribe(
      //  () => {
      //    this.loadContacts();
      //    this.resetForm();
      //  },
      //  (error) => {
      //    console.error('Error adding contact', error);
      //  }
      //);
    }
    this.loadContacts();
    this.resetForm();
  }

  // Handle the event of editing a contact
  onEditContact(contact: Contact): void {
    this.contactService.getContact(contact.id).subscribe(
      (contact: Contact) => {
        this.contactToEdit = contact;
        this.showForm = true;
      },
      (error) => {
        console.error('Error fetching contact for edit', error);
      }
    );
  }

  // Handle the event of deleting a contact
  onDeleteContact(id: number): void {
    this.contactService.deleteContact(id).subscribe(
      () => {
        this.loadContacts();
      },
      (error) => {
        console.error('Error deleting contact', error);
      }
    );
  }

  // Reset the form after submit or cancel
  resetForm(): void {
    this.contactToEdit = null;
    this.showForm = false;
  }

  // Cancel editing mode
  onCancelEdit(): void {
    this.resetForm();
    this.showForm = false;
  }

  isComponentVisible: boolean = false;

  openComponent() {
    this.isComponentVisible = true;
  }

  closeComponent() {
    this.isComponentVisible = false;
  }
}
