import { Component, OnInit,Input,Output,EventEmitter } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
})
export class ContactListComponent {
  @Input() contacts: Contact[] = [];
  @Output() editContact = new EventEmitter<Contact>();
  @Output() deleteContact = new EventEmitter<number>();
 

  onEdit(contact: Contact): void {
    this.editContact.emit(contact);
  }

  onDelete(id: number): void {
    this.deleteContact.emit(id);
  }
}
