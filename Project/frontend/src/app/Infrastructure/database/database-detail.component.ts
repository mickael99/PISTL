import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-database-detail',
  templateUrl: './database-detail.component.html',
  styleUrls: ['./database-detail.component.css']
})

export class DatabaseDetailComponent implements OnInit {
  @Input() databaseId: number;
  databaseDetails: any;
  server: any;
  ServerSelected: any;
  showModal = true;
  editingMode = false;
  constructor(private route: ActivatedRoute, private http: HttpClient) { }

  ngOnInit(): void {
    this.loadServers();
  }

  loadDatabaseDetails() {
    if (!this.databaseId) {
      console.error('No databaseId provided');
      return;
    }
  
    this.http.get(`http://localhost:5050/api/database/${this.databaseId}`).subscribe(
      data => {
        this.databaseDetails = data;
        console.log(this.databaseDetails)
      },
      error => {
        console.error('Error fetching database details:', error);
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

  openModal() {
    console.log("Opening modal with databaseId:", this.databaseId);
    if (!this.databaseId) {
      console.error('No databaseId provided');
      return;
    }
    this.showModal = true;
    console.log(this.showModal);

    const modalElement = document.querySelector('.modal') as HTMLElement;
    if (modalElement) {
      modalElement.style.display = 'block';
    }
  
    this.loadDatabaseDetails();  
  }

  editDatabase() {
    console.log('Edit database:', this.databaseId);
    this.editingMode = true;
  }

  quitEditing() {
    this.editingMode = false;
  }

  editDatabaseDetails() {
    console.log('Edit database details:', this.databaseId);
    console.log(this.databaseDetails);
    console.log(this.ServerSelected);
    this.http
      .put(`http://localhost:5050/api/database/${this.databaseId}`, {
        Name: this.databaseDetails.name,
        Context: this.databaseDetails.context,
        ModifiedBy: this.databaseDetails.modifiedBy,
        Server: this.ServerSelected,
        ServerId: this.ServerSelected.serverId,
        UserName: this.databaseDetails.userName,
        Password: this.databaseDetails.password,
      })
      .subscribe({
        next: (data: any) => {
          this.databaseDetails = data;
          this.editingMode = false;
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }
  
  closeModal() {
    this.showModal = false;
  }
  

  deleteDatabase() {
    console.log('Delete database:', this.databaseId);
    this.http
      .delete(`http://localhost:5050/api/database/${this.databaseId}`)
      .subscribe({
        next: (data: any) => {
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }

  copyDatabase() {
    console.log('Copy database details:', this.databaseId);
  }


}
