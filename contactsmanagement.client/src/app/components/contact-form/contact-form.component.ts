import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
})
export class ContactFormComponent implements OnInit {
  @Input() isEditing: boolean = false; 
  @Input() contactToEdit: Contact | null = null;
  @Output() contactAdded = new EventEmitter<Contact>();
  @Output() cancel = new EventEmitter<void>();
  errorMessage: string = '';  // Store the error message
  contactForm: FormGroup;

  constructor(private fb: FormBuilder, private contactService: ContactService) {
    this.contactForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
    });
  }
  ngOnInit(): void {
    if (this.contactToEdit) {
      this.isEditing = true;
      this.contactForm.patchValue({
        id: this.contactToEdit.id,
        firstName: this.contactToEdit.firstName,
        lastName: this.contactToEdit.lastName,
        email: this.contactToEdit.email,
      });
    }
  }

  onSubmit() {
    if (this.contactForm.invalid) {
      return;
    }

    const contact: Contact = this.contactForm.value;
    if (this.isEditing && this.contactToEdit) {
      // Use the `id` from `contactToEdit` and update the contact.
      const id = this.contactToEdit.id;
      contact.id = id;
      this.contactService.updateContact(id, contact).subscribe(() => {
        this.contactAdded.emit(contact);
      });
    } else {
      // For adding a new contact
      this.contactService.addContact(contact).subscribe((newContact) => {
        this.contactAdded.emit(newContact);

      }, (error) => {
        this.errorMessage = error.message;  // Handle and display error
      });
    }

    this.contactForm.reset();
  }

  onCancel() {
    this.contactForm.reset();
    this.cancel.emit();   
        
  }
}
