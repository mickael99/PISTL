import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-detail',
  templateUrl: './server-detail.component.html',
  styleUrls: ['./server-detail.component.css']
})

export class ServerDetailComponent implements OnInit {
  @Input() serverId: number;
  serverDetails: any;
  showModal = true;
  editingMode = false;
  Name: string = '';
  ModifiedBy: string = '';
  Address: string = '';

  ngOnInit(): void {
      this.Name = this.serverDetails.name;
      this.Address = this.serverDetails.address;
      this.ModifiedBy = this.serverDetails.modifiedBy;
  }

  loadServerDetails() {
    if (!this.serverId) {
      console.error('No serverId provided');
      return;
    }
  
    this.http.get(`http://localhost:5050/api/server/${this.serverId}`).subscribe(
      data => {
        this.serverDetails = data;
        console.log(this.serverDetails)
      },
      error => {
        console.error('Error fetching server details:', error);
      }
    );
  }

  openModal() {
    console.log("Opening modal with serverId:", this.serverId);
    if (this.serverId == null) {
      console.error('No serverId provided');
      return;
    }
    this.showModal = true;
    console.log(this.showModal);
    
    const modalElement = document.querySelector('.modal') as HTMLElement;
    if (modalElement) {
      modalElement.style.display = 'block';
    }
  
    this.loadServerDetails();  
  }
  
  closeModal() {
    this.showModal = false;
    this.editingMode = false;
  }
  
  editServer() {
    console.log('Edit server:', this.serverId);
    console.log(this.Name);
    this.editingMode = true;
  }

  quitEditing() {
    this.editingMode = false;
  }

  editServerDetails() {
    console.log('Edit server details:', this.serverId);
    console.log(this.Address);
    this.http
      .put(`http://localhost:5050/api/server/${this.serverId}`, {
        Name: this.serverDetails.name,
        Address: this.serverDetails.address,
        ModifiedBy: this.serverDetails.modifiedBy,
      })
      .subscribe({
        next: (data: any) => {
          this.serverDetails = data;
          this.editingMode = false;
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }


  // ...

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) {}

  deleteServer() {
    console.log('Delete server:', this.serverId);
    this.http
      .delete(`http://localhost:5050/api/server/${this.serverId}`)
      .subscribe({
        next: (data: any) => {
          console.log("redirecting to /server");
          this.router.navigate(['/server']);
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }

  copyServer() {
    console.log('Copy server details:', this.serverId);
  }
  


}
