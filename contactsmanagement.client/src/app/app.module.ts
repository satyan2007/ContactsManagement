import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ContactListComponent } from  '../../src/app/components/contact-list/contact-list.component';
import { ContactFormComponent } from '../../src/app/components/contact-form/contact-form.component';
import { ContactService } from '../../src/app/services/contact.service';


@NgModule({
  declarations: [
    AppComponent,
    ContactListComponent,
    ContactFormComponent
    
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

