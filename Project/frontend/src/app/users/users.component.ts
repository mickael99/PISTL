import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent {
  users: any;
  displayedColumns: string[] = ['email', 'DAT'];
  dataSource: any;
  isClicked: boolean = false;
  showFormCreate: boolean = false;
  formDataCreate = {
    name: '',
    email: '',
    phone: '',
    modifiedBy: '',
    DATEnabled: '',
    locked: '',
  };

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
        console.log('GET error: ' + error.message);
      }
    );
  }

  /***************************************************************************************/
  // createUser() {
  //   let JWTToken = localStorage.getItem('token');

  //   const   options = {
  //     headers: new HttpHeaders({
  //       Authorization: 'Bearer ' + JWTToken,
  //       'Content-Type': 'application/json',
  //     }),
  //   };
  // }

  /***************************************************************************************/
  showFormCreateUser() {
    this.showFormCreate = !this.showFormCreate;
    this.formDataCreate = {
      name: '',
      email: '',
      phone: '',
      modifiedBy: '',
      DATEnabled: '',
      locked: '',
    };
  }

  /***************************************************************************************/
  afffFormCreateUser() {
    this.formDataCreate.modifiedBy = localStorage.getItem('email');
    console.table(this.formDataCreate);

    let JWTToken = localStorage.getItem('token');

    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    const requestBody = { formDataCreate: this.formDataCreate };

    this.http
      .post('http://localhost:5050/api/users/create', requestBody, options)
      .subscribe(
        (data: any) => {
          console.log('Response from server:', data);
        },
        (error) => {
          console.error('Error from server:', error);
        }
      );
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
