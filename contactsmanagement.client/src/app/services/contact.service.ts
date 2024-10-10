import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Contact } from '../models/contact.model';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ContactService {
  //Api Url
  private apiUrl = 'https://localhost:7084/api/contacts'; 
  constructor(private http: HttpClient) { }

  getContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.apiUrl);
  }

  getContact(id: number): Observable<Contact> {
    return this.http.get<Contact>(`${this.apiUrl}/${id}`);
  }

  //addContact(contact: Contact): Observable<Contact> {
  //  return this.http.post<Contact>(this.apiUrl, contact);
  //}

  addContact(contact: Contact): Observable<Contact> {
    contact.id = 0;   
    return this.http.post<Contact>(this.apiUrl, contact).pipe(
      catchError(this.handleError)  // Handle any errors that occur during the HTTP request
    );
  }

  

  // Error handler function
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Client-side error: ${error.error.message}`;
    } else {
      // Backend error
      errorMessage = `Server-side error: ${error.status} - ${error.message}`;
    }

    // You can show an alert, log the error, or handle it in any way you prefer
    console.error(errorMessage);

    // Return an observable with a user-facing error message
    return throwError(() => new Error(errorMessage));
  }

  updateContact(id: number, contact: Contact): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, contact);
  }

  deleteContact(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}



