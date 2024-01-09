import { HttpClient } from '@angular/common/http';
import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServerDetailComponent } from './server-detail.component'

@Component({
  selector: 'app-server',
  templateUrl: './server.component.html',
  styleUrls: ['./server.component.css']
})
export class ServerComponent implements OnInit, AfterViewInit {

  server: any;
  id: number;
  Name: string = '';
  createdBy: string = '';
  edition: string = '';
  Address: string = '';
  ServerId: number;
  @ViewChild(ServerDetailComponent) detailModal: ServerDetailComponent;

  // Combine the HttpClient and Router in a single constructor
  constructor(private http: HttpClient, private router: Router) {

  }

  ngOnInit(): void {
    this.loadServers();
  }

  ngAfterViewInit() {
    console.log(this.detailModal);
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

  addServer() {
    this.http
      .post('http://localhost:5050/api/server', {
        Name: this.Name,
        createdBy: this.createdBy,
        Address: this.Address,
        ServerId: this.id,
     })
      .subscribe({
        next: (data: any) => {
          this.server = data;
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }

  openModal(serverId: number) {
    console.log(serverId);
    this.detailModal.serverId = serverId;
    this.detailModal.openModal();
  }

}
