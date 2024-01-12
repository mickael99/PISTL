import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent {
  // Users form the DB
  users: any;

  // Table used for data display
  dataSource: any;

  // Columns names in the table
  displayedColumns: string[] = ['email', 'DAT'];

  // Bool used for the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
  isClicked: boolean = false;

  // Bool that allows or not the display of the create a new user form
  showFormCreate: boolean = false;

  // Form data used to create a new user
  formDataCreate = {
    name: '',
    email: '',
    phone: '',
    modifiedBy: '',
    DATEnabled: false,
    locked: false,
  };

  // Bool that allows or not to display the error popup
  showPopup: boolean = false;

  // Error message to display in the popup
  popupMessage: string = '';

  /***************************************************************************************/
  /**
   * Creates an instance of UsersComponent.
   * @param renderer - The renderer used to create the users page.
   * @param http - The http client used to connect to the database.
   */
  constructor(
    private renderer: Renderer2,
    private http: HttpClient,
    private dialog: MatDialog
  ) {
    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    this.http.get('http://localhost:5050/api/users', options).subscribe(
      (data: any) => {
        this.users = data;
        this.dataSource = new MatTableDataSource(this.users);
      },
      (error) => {
        this.showErrorPopup(error.error);
      }
    );
  }

  /***************************************************************************************/
  /**
   * Function used to display the user's information in the form.
   */
  showFormCreateUser() {
    this.showFormCreate = !this.showFormCreate;

    // Reinitialize the form if reclicked
    this.formDataCreate = {
      name: '',
      email: '',
      phone: '',
      modifiedBy: '',
      DATEnabled: false,
      locked: false,
    };
  }

  /***************************************************************************************/
  /**
   * Function used to POST the user's information from the form.
   */
  afffFormCreateUser() {
    if (
      this.formDataCreate.name == '' ||
      this.formDataCreate.email == '' ||
      this.formDataCreate.phone == ''
    ) {
      this.showErrorPopup('Please fill all the fields.');
      return;
    }

    this.formDataCreate.modifiedBy = localStorage.getItem('email');

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    console.log('requestBody: ', this.formDataCreate);

    this.http
      .post('http://localhost:5050/api/users', this.formDataCreate, options)
      .subscribe({
        next: (data: any) => {
          this.users = data.users;
          this.dataSource = new MatTableDataSource(this.users);
        },
        error: (error: any) => {
          console.error(error.error.message);
          this.showErrorPopup(error.error.message);
        },
      });
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
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
