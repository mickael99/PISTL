import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { DomainAdministrationComponent } from './Domain/domain-administration/domain-administration.component';
import { LoginComponent } from './Login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { SysAdminByDomainComponent } from './Domain/sys-admin-by-domain/sys-admin-by-domain.component';
import { DomainBySysAdminComponent } from './Domain/domain-by-sys-admin/domain-by-sys-admin.component';
import { DatabaseComponent } from './Infrastructure/database/database.component';
import { ServerComponent } from './Infrastructure/server/server.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { ErrorPopupComponent } from './utils/error-popup/error-popup.component';
import { ConfirmPopupComponent } from './utils/confirm-popup/confirm-popup.component';
import { InformPopupComponent } from './utils/inform-popup/inform-popup.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';



@NgModule({
  declarations: [
    AppComponent,
    DomainAdministrationComponent,
    LoginComponent,
    SysAdminByDomainComponent,
    DomainBySysAdminComponent,
    DatabaseComponent,
    ServerComponent,
    ErrorPopupComponent,
    ConfirmPopupComponent,
    InformPopupComponent,
  ],
  imports: [BrowserModule, HttpClientModule, FormsModule, AppRoutingModule, CommonModule,
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatButtonModule,
    MatDatepickerModule,
    MatIconModule,
    MatDividerModule,
    MatCheckboxModule,],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
