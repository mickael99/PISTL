import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { DomainAdministrationComponent } from './Domain/domain-administration/domain-administration.component';
import { LoginComponent } from './Login/login.component';
import { AppRoutingModule } from './app-routing.module';

import { SysAdminByDomainComponent } from './Domain/sys-admin-by-domain/sys-admin-by-domain.component';
import { SysAdminByDomainDialog } from './Domain/sys-admin-by-domain/sys-admin-by-domain-dialog.component';
import { DomainBySysAdminComponent } from './Domain/domain-by-sys-admin/domain-by-sys-admin.component';
import { DomainBySysAdminComponentDialog } from './Domain/domain-by-sys-admin/domain-by-sys-admin-dialog.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule} from '@angular/material/select';
import { MatTableModule } from '@angular/material/table'; 
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatIconModule} from '@angular/material/icon';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    DomainAdministrationComponent,
    LoginComponent,
    SysAdminByDomainComponent,
    SysAdminByDomainDialog,
    DomainBySysAdminComponent,
    DomainBySysAdminComponentDialog
  ],
  imports: [
    BrowserModule,
    HttpClientModule, 
    FormsModule, 
    AppRoutingModule, 
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatButtonModule,
    MatDatepickerModule,
    MatIconModule,
    DatePipe],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
