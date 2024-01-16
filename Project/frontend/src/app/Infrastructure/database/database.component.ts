import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DatabaseDetailComponent } from './database-detail.component'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  ElementRef,
  HostListener,
  Renderer2,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.css']
})
export class DatabaseComponent implements OnInit {

  // Servers form the DB
  server: any;

  // Table used for data display
  dataSource: any;

  // Form data used to create a new server
  formDataCreate = {
    DatabaseIdId: 0,
    Name: '',
    UserName : '',
    Password: '',
    ServerId: 0,
    CreatedBy: '',
  };

  // Bool used for the 'Edit', 'Delete', 'Reset Password' and 'Unlock' buttons
  isClicked: boolean = false;

  // Columns names in the table
  displayedColumns: string[] = ['Server ID','Name', 'Address', 'Context', 'Type'];

  // Bool that allows or not to display the error popup
  showPopup: boolean = false;

  @ViewChild(DatabaseDetailComponent) detailModal: DatabaseDetailComponent;
  database: any;
  DatabaseId: number = 0;
  Name: string = '';
  UserName: string = '';
  Password: string = '';
  createdBy: string = '';
  edition: string = '';
  ServerSelected: any;

  // Combine the HttpClient and Router in a single constructor
  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.loadDatabases();
    this.loadServers();
  }

  loadDatabases(): void {
    this.http.get('http://localhost:5050/api/database').subscribe(
      (data: any) => {
        this.database = data;
        console.log(this.database);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  loadServers(): void {
    this.http.get('http://localhost:5050/api/server').subscribe(
      (data: any) => {
        this.server = data;
        console.log(this.server);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  addDatabase() {
    console.log('Add database');
    console.log(this.ServerSelected);
    this.http
      .post('http://localhost:5050/api/database', {
        DatabaseId: this.DatabaseId,
        Name: this.Name,
        createdBy: this.createdBy,
        ServerId: this.ServerSelected.serverId,
        Server: this.ServerSelected,
        UserName: this.UserName,
        Password: this.Password,
      })
      .subscribe({
        next: (data: any) => {
          this.database = data;
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }

  openModal(databaseId: number) {
    this.detailModal.databaseId = databaseId;
    this.detailModal.openModal();
  }

}
