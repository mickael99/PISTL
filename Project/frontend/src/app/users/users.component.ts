import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { PopUpComponent } from './pop-up/pop-up.component';

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
    email: '',
    password: '',
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
    this.dialog.open(PopUpComponent, {
      data: {
        name: 'Daniel',
      },
    });
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
