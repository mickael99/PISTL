import { OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServerDetailComponent } from './server-detail.component'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  Component,
  ElementRef,
  HostListener,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ChangeDetectorRef } from '@angular/core';


@Component({
  selector: 'app-server',
  templateUrl: './server.component.html',
  styleUrls: ['./server.component.css']
})
export class ServerComponent {

  // Servers form the DB
  server: any;

  // Table used for data display
  dataSource: any;

  // Form data used to create a new server
  formDataCreate = {
    ServerId: 0,
    Name: '',
    Address: '',
    Context: '',
    Type: '',
    CreatedBy: '',
  };

  // Bool used for the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
  isClicked: boolean = false;

  // Columns names in the table
  displayedColumns: string[] = ['Server ID','Name', 'Address', 'Context', 'Type'];

  // Bool that allows or not to display the error popup
  showPopup: boolean = false;

  // User selected in the table
  serverSelected = {
    ServerId: 0,
    Name: '',
    Address: '',
    Context: '',
    Type: '',
    CreatedBy: '',
    ModifiedBy: '',
    CreatedDate: '',
    ModifiedDate: '',
  };

  // Copy of the user selected in the table (for the 'Save' button)
  serverSelectedCopy = {
    ServerId: 0,
    Name: '',
    Address: '',
    Context: '',
    Type: '',
  };

  // Hover the two columns in the table
  isHovered: boolean = false;

  // User hovered in the table
  userHovered: string = '';

  // Bool used to activate the 'Edit' button
  editEnabled: boolean = false;

  // Bool that allows or not the display of the create a new server form
  showFormCreate: boolean = false;

  // Bool used to display the delete confirmation popup
  showDeleteConfirmation: boolean = false;

  // Confirmation message to display
  confirmationMessage: string = '';

  // Bool used to display the information popup
  showInformPopup: boolean = false;

  // Bool that allows or not to display the error popup
  showPopupError: boolean = false;

  // Error message to display in the popup
  popupMessage: string = '';

  @ViewChild(ServerDetailComponent) detailModal: ServerDetailComponent;

  /***************************************************************************************/
  /**
   * Creates an instance of ServerComponent.
   * @param renderer - The renderer used to create the users page.
   * @param http - The http client used to connect to the database.
   */
  constructor(
    private renderer: Renderer2,
    private http: HttpClient,
    private dialog: MatDialog,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.http.get('http://localhost:5050/api/server', options).subscribe(
      (data: any) => {
        this.server = data;
        this.dataSource = new MatTableDataSource(this.server);
      },
      (error) => {
        this.showErrorPopup(error.error);
      }
    );

    console.log('server: ', this.server);
    console.log('dataSource: ', this.dataSource);
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the error popup.
   * @param message - Error message.
   */
  showErrorPopup(message: string) {
    this.showPopup = true;
    this.popupMessage = message;
  }

  /***************************************************************************************/
  /**
   * Function used to close the error popup.
   */
  closeErrorPopup() {
    this.showPopup = false;
  }

  /***************************************************************************************/
  /**
   * Function used to display the user's information in the form.
   */
  showFormCreateServer() {
    this.showFormCreate = !this.showFormCreate;

    // Reinitialize the form if reclicked
    this.formDataCreate = {
      ServerId: 0,
      Name: '',
      Address: '',
      Context: '',
      Type: '',
      CreatedBy: '',
    };

    this.reinitaliseServerSelectedForm();
  }

  /***************************************************************************************/
  /**
   * Function used to POST the user's information from the form.
   */
  newServerFormCreateServer() {
    console.log('creating new server');
    if (
      this.formDataCreate.Name == '' ||
      this.formDataCreate.Address == '' ||
      this.formDataCreate.Context == '' ||
      this.formDataCreate.Type == ''
    ) {
      this.showErrorPopup('Please fill all the fields.');
      return;
    }

    this.formDataCreate.CreatedBy = localStorage.getItem('email');

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    
    this.formDataCreate.ServerId = parseInt(this.formDataCreate.ServerId.toString());
    console.log(typeof this.formDataCreate.ServerId);

    console.log('requestBody: ', this.formDataCreate);

    this.http
      .post(
        'http://localhost:5050/api/server',
        {        
          ServerId: this.formDataCreate.ServerId,
          Name: this.formDataCreate.Name,
          Address: this.formDataCreate.Address,
          Context: this.formDataCreate.Context,
          Type: this.formDataCreate.Type,
          CreatedBy: this.formDataCreate.CreatedBy,
        })
      .subscribe({
        next: (data: any) => {
          this.server = data.server;
          this.dataSource = new MatTableDataSource(this.server);
        },
        error: (error: any) => {
          console.error(error.error.message);
          this.showErrorPopup(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  getServer(server: any) {
    this.serverSelected.ServerId = server.serverId;
    this.serverSelected.Name = server.name;
    this.serverSelected.Address = server.address;
    this.serverSelected.Context = server.context;
    this.serverSelected.Type = server.type;
    this.serverSelected.CreatedBy = server.createdBy;
    this.serverSelected.ModifiedBy = server.modifiedBy;
    this.serverSelected.CreatedDate = server.createdDate;
    this.serverSelected.ModifiedDate = server.modifiedDate;

    this.reinitaliseServerCreatedForm();

    // Activate the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
    this.isClicked = true;

    console.table(this.serverSelected);
  }

    /***************************************************************************************/
  /**
   * Function used to activate the hover.
   */
  onMouseEnter(userHovered: string) {
    this.isHovered = true;
    this.userHovered = userHovered;
  }

  /***************************************************************************************/
  /**
   * Function used to deactivate the hover.
   */
  onMouseLeave() {
    this.isHovered = false;
    this.userHovered = '';
  }

  /***************************************************************************************/
  /**
   * Function used to reinitalise the selected Server.
   */
  reinitaliseServerSelectedForm() {
    // Disable access user form
    this.serverSelected = {
      ServerId: 0,
      Name: '',
      Address: '',
      Context: '',
      Type: '',
      CreatedBy: '',
      ModifiedBy: '',
      CreatedDate: '',
      ModifiedDate: '', 
    };

    // Disable the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
    this.isClicked = false;

    // Disable the 'Edit' button
    this.editEnabled = false;
  }

    /***************************************************************************************/
  /**
   * Function used to reinitalise the created Sys Admin form.
   */
  reinitaliseServerCreatedForm() {
    this.showFormCreate = false;

    this.formDataCreate = {
      ServerId: 0,
      Name: '',
      Address: '',
      Context: '',
      Type: '',
      CreatedBy: '',
    };
  };

    /***************************************************************************************/
  /**
   * Function used to enable the a Server edition.
   */
  editEnable() {
    this.editEnabled = true;

    // Copy the user selected to compare the changes
    this.serverSelectedCopy.ServerId = this.serverSelected.ServerId;
    this.serverSelectedCopy.Name = this.serverSelected.Name;
    this.serverSelectedCopy.Address = this.serverSelected.Address;
    this.serverSelectedCopy.Context = this.serverSelected.Context;
    this.serverSelectedCopy.Type = this.serverSelected.Type;
    
  }

  /***************************************************************************************/
  /**
   * Function used to edit the Sys Admin's information.
   */
  editServer() {
    if (

      this.serverSelected.Name == this.serverSelectedCopy.Name &&
      this.serverSelected.ServerId == this.serverSelectedCopy.ServerId &&
      this.serverSelected.Address == this.serverSelectedCopy.Address &&
      this.serverSelected.Context == this.serverSelectedCopy.Context &&
      this.serverSelected.Type == this.serverSelectedCopy.Type
    ) {
      this.showErrorPopup('Please do some changes before Save.');
      return;
    }

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    const requestBody = {
      ServerId: this.serverSelected.ServerId,
      Name: this.serverSelected.Name,
      Address: this.serverSelected.Address,
      Context: this.serverSelected.Context,
      Type: this.serverSelected.Type,
    };

    console.log('requestBody: ', requestBody);

    this.http
      .put('http://localhost:5050/api/server/' + requestBody.ServerId, requestBody)
      .subscribe({
        next: (data: any) => {
          this.server = data.serverToUpdate;
          this.dataSource = new MatTableDataSource(this.server);
          this.editEnabled = false;
        },
        error: (error: any) => {
          console.error(error.error.message);
          this.showErrorPopup(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  openDeleteConfirmation() {
    this.showDeleteConfirmation = true;
    console.log('showDeleteConfirmation: ', this.showDeleteConfirmation);
    this.confirmationMessage =
      'Are you sure you want to delete this server: ' +
      this.serverSelected.Name +
      '?';
    console.log('confirmationMessage: ', this.confirmationMessage);
    this.changeDetectorRef.detectChanges();

  }

  
  


  // addServer() {
  //   this.http
  //     .post('http://localhost:5050/api/server', {
  //       Name: this.Name,
  //       createdBy: this.createdBy,
  //       Address: this.Address,
  //       ServerId: this.id,
  //    })
  //     .subscribe({
  //       next: (data: any) => {
  //         this.server = data;
  //       },
  //       error: (error: any) => {
  //         alert('Connection error: ' + error.message);
  //       },
  //     });
  // }

  // openModal(serverId: number) {
  //   console.log(serverId);
  //   this.detailModal.serverId = serverId;
  //   this.detailModal.openModal();
  // }

}
