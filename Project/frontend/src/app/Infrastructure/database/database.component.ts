import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ElementRef, HostListener, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ChangeDetectorRef } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.css'],
})
export class DatabaseComponent {
  // Servers form the DB
  server: any;

  // Table used for data display
  dataSource: any;

  // Form data used to create a new server
  formDataCreate = {
    DatabaseId: 0,
    Name: '',
    UserName: '',
    Password: '',
    Server: null,
    ServerId: 0,
    CreatedBy: '',
    ModifiedBy: '',
    Context: null,
  };

  // User selected in the table
  databaseSelected = {
    ServerId: 0,
    Name: '',
    UserName: '',
    Password: '',
    PasswordModified: '',
    Server: null,
    DatabaseId: 0,
    CreatedBy: '',
    ModifiedBy: '',
    CreatedDate: '',
    ModifiedDate: '',
    Context: null,
  };

  // Copy of the user selected in the table (for the 'Save' button)
  databaseSelectedCopy = {
    DatabaseId: 0,
    Name: '',
    UserName: '',
    Password: '',
    ServerId: 0,
    Server: null,
    Context: null,
  };

  // Bool used for the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
  isClicked: boolean = false;

  databaseClicked: number;

  // Columns names in the table
  displayedColumns: string[] = [
    'databaseId',
    'name',
    'userName',
    'context',
    'serverId',
  ];

  // Bool that allows or not to display the error popup
  showPopup: boolean = false;

  database: any;

  // Hover the two columns in the table
  isHovered: boolean = false;

  // User hovered in the table
  databaseHovered: number;

  // Bool used to activate the 'Edit' button
  editEnabled: boolean = false;

  // Bool that allows or not the display of the create a new server form
  showFormCreate: boolean = false;

  // Bool used to display the delete confirmation popup
  showDeleteConfirmation: boolean = false;

  // Bool used to display the copy confirmation popup
  showCopyConfirmation: boolean = false;

  // Confirmation message to display
  confirmationMessage: string = '';

  // Bool that allows or not to display the error popup
  showPopupError: boolean = false;

  // Error message to display in the popup
  popupMessage: string = '';

  // Paginator used for the table
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  // Sort used for the table
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  // Sort used for the table
  @ViewChild(MatSort, { static: true }) sortName: MatSort;

  // Sort used for the table
  @ViewChild(MatSort, { static: true }) sortServerId: MatSort;

  // Bool used to display the password
  showPassword: boolean = false;

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

    this.http.get('http://localhost:5050/api/database', options).subscribe(
      (data: any) => {
        this.database = data;
        this.dataSource = new MatTableDataSource(this.database);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sortingDataAccessor = (item, property) => {
          switch (property) {
            case 'databaseId':
              return item.databaseId;
            case 'name': // Add case for Name column
              return item.name;
            case 'serverId':
              return item.serverId;
            case 'userName':
              return item.userName;
            case 'context':
              return item.context;
            default:
              return item[property];
          }
        };
        this.dataSource.sort = this.sort;
        // this.dataSource.sortName = this.sortName;
        this.dataSource.sortServerId = this.sortServerId;
      },
      (error) => {
        console.error(error.error);
      }
    );

    this.http.get('http://localhost:5050/api/server', options).subscribe(
      (data: any) => {
        this.server = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sortingDataAccessor = (item, property) => {
          switch (property) {
            case 'databaseId':
              return item.databaseId;
            default:
              return item[property];
          }
        };
        this.dataSource.sort = this.sort;
      },
      (error) => {
        console.error(error.error);
      }
    );
  }

  /***************************************************************************************/
  /**
   * Function used to POST the database's information from the form.
   */
  newDatabaseFormDatabase() {
    this.showPassword = false;

    if (this.formDataCreate.Server) {
      this.formDataCreate.ServerId = this.formDataCreate.Server.serverId;
    }

    // Check if the user has filled all the fields
    // if (
    // this.formDataCreate.Name == '' ||
    // this.formDataCreate.UserName == '' ||
    // this.formDataCreate.Password == '' ||
    // this.formDataCreate.ServerId == 0
    // ) {
    // this.showErrorPopup('Please fill all the fields.');
    // return;
    // }

    // Create the request body
    this.formDataCreate.CreatedBy = localStorage.getItem('email');
    this.formDataCreate.ModifiedBy = localStorage.getItem('email');

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.formDataCreate.ServerId = parseInt(
      this.formDataCreate.ServerId.toString()
    );

    // Call the POST request
    this.http
      .post('http://localhost:5050/api/database', this.formDataCreate)
      .subscribe({
        next: (data: any) => {
          this.showFormCreate = false;
          this.database = data.databases;
          this.dataSource = new MatTableDataSource(this.database);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
              case 'databaseId':
                return item.databaseId;
              case 'name': // Add case for Name column
                return item.name;
              case 'serverId':
                return item.serverId;
              case 'userName':
                return item.userName;
              case 'context':
                return item.context;
              default:
                return item[property];
            }
          };
          this.dataSource.sort = this.sort;
        },
        error: (error: any) => {
          console.error(error.error.message);
          // this.showErrorPopup(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  /**
   * Function used to call the POST request to delete the database.
   */
  onDeleteConfirm() {
    this.showDeleteConfirmation = false;

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.http
      .delete(
        `http://localhost:5050/api/database/${this.databaseSelected.DatabaseId}`
      )
      .subscribe({
        next: (data: any) => {
          this.database = data.databases;
          this.dataSource = new MatTableDataSource(this.database);
          this.reinitaliseDatabaseSelectedForm();
          this.dataSource.paginator = this.paginator;
          this.dataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
              case 'databaseId':
                return item.databaseId;
              case 'name': // Add case for Name column
                return item.name;
              case 'serverId':
                return item.serverId;
              case 'userName':
                return item.userName;
              case 'context':
                return item.context;
              default:
                return item[property];
            }
          };
          this.dataSource.sort = this.sort;
        },
        error: (error: any) => {
          console.error(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  /**
   * Function used to call the POST request to delete the database.
   */
  onCopyConfirm() {
    this.showCopyConfirmation = false;

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Check if the last two characters of this.selected_parameter[0] match the pattern "_"+ (number)
    const regex = /_\d$/;
    if (regex.test(this.databaseSelected.Name)) {
      this.databaseSelected.Name =
        this.databaseSelected.Name.slice(0, -1) +
        (parseInt(this.databaseSelected.Name.slice(-1)) + 1).toString();
    } else {
      this.databaseSelected.Name = this.databaseSelected.Name + '_1';
    }

    // Create the request body
    const requestBody = {
      ServerId: this.databaseSelected.ServerId,
      Server: this.getServer(this.databaseSelected.ServerId),
      Name: this.databaseSelected.Name,
      UserName: this.databaseSelected.UserName,
      Password: this.databaseSelected.Password,
      CreatedBy: localStorage.getItem('email'),
      Context: this.databaseSelected.Context,
    };

    this.http
      .post('http://localhost:5050/api/database', requestBody)
      .subscribe({
        next: (data: any) => {
          this.database = data.databases;
          this.dataSource = new MatTableDataSource(this.database);
          this.reinitaliseDatabaseSelectedForm();
          this.dataSource.paginator = this.paginator;
          this.dataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
              case 'databaseId':
                return item.databaseId;
              case 'name': // Add case for Name column
                return item.name;
              case 'serverId':
                return item.serverId;
              case 'userName':
                return item.userName;
              case 'context':
                return item.context;
              default:
                return item[property];
            }
          };
          this.dataSource.sort = this.sort;
        },
        error: (error: any) => {
          console.error(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  /**
   * Function used to edit the database's information.
   */
  editDatabase() {
    this.showPassword = false;

    // Find the server with the same name as ServerName
    const serverToUpdate = this.server.find(
      (s) => s.serverId === this.databaseSelected.ServerId
    );

    // Store the found server in databaseSelected.Server
    this.databaseSelected.Server = serverToUpdate;

    //check if the user has changed something
    // if (
    //   this.databaseSelected.Name == this.databaseSelectedCopy.Name &&
    //   this.databaseSelected.UserName == this.databaseSelectedCopy.UserName &&
    //   this.databaseSelected.ServerId == this.databaseSelectedCopy.ServerId &&
    //   this.databaseSelected.Context == this.databaseSelectedCopy.Context &&
    //   this.databaseSelected.Password == this.databaseSelectedCopy.Password
    // ) {
    //   this.showErrorPopup('Please do some changes before Save.');
    //   return;
    // }

    var passwordToModified;

    if (this.databaseSelected.PasswordModified == '') {
      passwordToModified = this.databaseSelected.Password;
    } else {
      passwordToModified = this.databaseSelected.PasswordModified;
    }

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Create the request body
    const requestBody = {
      DatabaseId: this.databaseSelected.DatabaseId,
      ServerId: this.databaseSelected.Server.serverId,
      Server: this.databaseSelected.Server,
      Name: this.databaseSelected.Name,
      UserName: this.databaseSelected.UserName,
      Password: passwordToModified,
      ModifiedBy: localStorage.getItem('email'),
      Context: this.databaseSelected.Context,
    };

    // Call the PUT request
    this.http
      .put(
        'http://localhost:5050/api/database/' + requestBody.DatabaseId,
        requestBody
      )
      .subscribe({
        next: (data: any) => {
          this.database = data.databases;
          this.dataSource = new MatTableDataSource(this.database);
          this.editEnabled = false;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sortingDataAccessor = (item, property) => {
            switch (property) {
              case 'databaseId':
                return item.databaseId;
              case 'name': // Add case for Name column
                return item.name;
              case 'serverId':
                return item.serverId;
              case 'userName':
                return item.userName;
              case 'context':
                return item.context;
              default:
                return item[property];
            }
          };
          this.dataSource.sort = this.sort;
        },
        error: (error: any) => {
          console.error(error.error.message);
        },
      });
  }

  /***************************************************************************************/
  /**
   * Function used to display the database's information in the form.
   */
  showFormCreateDatabase() {
    this.showFormCreate = !this.showFormCreate;
    this.showPassword = false;

    // Reinitialize the form if reclicked
    this.formDataCreate = {
      DatabaseId: 0,
      Name: '',
      UserName: '',
      Password: '',
      Server: null,
      ServerId: 0,
      CreatedBy: '',
      ModifiedBy: '',
      Context: null,
    };

    this.reinitaliseDatabaseSelectedForm();
  }

  /***************************************************************************************/
  /**
   * Retrieves the details of a database and assigns them to the 'databaseSelected' object.
   * Activates the 'Edit', 'Delete' buttons.
   * Sets the 'databaseClicked' variable to the ID of the selected database.
   * Sets the 'isHovered' variable to true.
   *
   * @param database - The database object containing the details to be retrieved.
   */
  getDatabase(database: any) {
    this.databaseSelected.DatabaseId = database.databaseId;
    this.databaseSelected.Name = database.name;
    this.databaseSelected.UserName = database.userName;
    this.databaseSelected.Password = database.password;
    this.databaseSelected.PasswordModified = '';
    this.databaseSelected.Server = database.Server;
    this.databaseSelected.ServerId = database.serverId;
    this.databaseSelected.CreatedBy = database.createdBy;
    this.databaseSelected.ModifiedBy = database.modifiedBy;
    this.databaseSelected.CreatedDate = database.createdDate;
    this.databaseSelected.ModifiedDate = database.modifiedDate;
    this.databaseSelected.Context = database.context;

    this.reinitaliseDatabaseCreatedForm();

    // Activate the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
    this.isClicked = true;

    this.databaseClicked = database.databaseId;

    this.isHovered = true;
  }

  /***************************************************************************************/
  /**
   * Function used to reinitalise the selected database.
   */
  reinitaliseDatabaseSelectedForm() {
    this.showPassword = false;

    // Disable access user form
    this.databaseSelected = {
      DatabaseId: 0,
      Name: '',
      UserName: '',
      Password: '',
      PasswordModified: '',
      ServerId: 0,
      Server: null,
      CreatedBy: '',
      ModifiedBy: '',
      CreatedDate: '',
      ModifiedDate: '',
      Context: 0,
    };

    // Disable the 'Edit', 'Delete' buttons
    this.isClicked = false;
    this.databaseClicked = 0;

    this.isHovered = false;

    // Disable the 'Edit' button
    this.editEnabled = false;
  }

  /***************************************************************************************/
  /**
   * Function used to reinitalise the created Sys Admin form.
   */
  reinitaliseDatabaseCreatedForm() {
    this.showFormCreate = false;

    this.formDataCreate = {
      DatabaseId: 0,
      Name: '',
      UserName: '',
      Password: '',
      Server: null,
      ServerId: 0,
      CreatedBy: '',
      ModifiedBy: '',
      Context: null,
    };
  }

  /***************************************************************************************/
  /**
   * Function used to enable the a Server edition.
   */
  editEnable() {
    this.editEnabled = true;

    // Copy the user selected to compare the changes
    this.databaseSelectedCopy.DatabaseId = this.databaseSelected.DatabaseId;
    this.databaseSelectedCopy.Name = this.databaseSelected.Name;
    this.databaseSelectedCopy.UserName = this.databaseSelected.UserName;
    this.databaseSelectedCopy.Password = this.databaseSelected.Password;
    this.databaseSelectedCopy.ServerId = this.databaseSelected.ServerId;
    this.databaseSelectedCopy.Context = this.databaseSelected.Context;
  }

  /***************************************************************************************/
  /**
   * Opens the delete confirmation dialog.
   * Sets the showDeleteConfirmation flag to true, logs the value of showDeleteConfirmation,
   * sets the confirmation message to a formatted string, logs the confirmation message,
   * and triggers change detection.
   */
  openDeleteConfirmation() {
    this.showDeleteConfirmation = true;
    this.confirmationMessage =
      'Are you sure you want to delete this server: ' +
      this.databaseSelected.Name +
      '?';
    this.changeDetectorRef.detectChanges();
  }

  /***************************************************************************************/
  /**
   * Opens the copy confirmation dialog.
   */
  openCopyConfirmation() {
    this.showCopyConfirmation = true;
    this.confirmationMessage =
      'Are you sure you want to copy this server: ' +
      this.databaseSelected.Name +
      '?';
    this.changeDetectorRef.detectChanges();
  }

  /***************************************************************************************/
  /**
   * Function used to close the delete confirmation popup.
   */
  onDeleteClose() {
    this.showDeleteConfirmation = false;
  }

  /***************************************************************************************/
  /**
   * Function used to close the copy confirmation popup.
   */
  onCopyClose() {
    this.showCopyConfirmation = false;
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the error popup.
   * @param message - Error message.
   */
  showErrorPopup(message: string) {
    this.showPopupError = true;
    this.popupMessage = message;
  }

  /***************************************************************************************/
  /**
   * Function used to close the error popup.
   */
  closeErrorPopup() {
    this.showPopupError = false;
  }

  /***************************************************************************************/
  /**
   * Function used to activate the hover.
   */
  onMouseEnter(databaseHovered: number) {
    if (this.isClicked == false) {
      this.isHovered = true;
    }
    this.databaseHovered = databaseHovered;
  }

  /***************************************************************************************/
  /**
   * Function used to deactivate the hover.
   */
  onMouseLeave() {
    if (this.isClicked == false) {
      this.isHovered = false;
    }
    this.databaseHovered = 0;
  }

  /***************************************************************************************/
  /**
   * Function used to get the server by its id.
   */
  getServer(id: number) {
    for (let i = 0; i < this.server.length; i++) {
      if (this.server[i].serverId == id) {
        return this.server[i];
      }
    }
  }

  /***************************************************************************************/
  /**
   * Function used to display the password.
   */
  togglePassword() {
    this.showPassword = !this.showPassword;
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
