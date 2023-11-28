import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-domain-administration',
  templateUrl: './domain-administration.component.html',
  styleUrls: ['./domain-administration.component.css'],
})
export class DomainAdministrationComponent {
  domains: any;
  domainName: string = '';
  createdBy: string = '';
  edition: string = '';

  constructor(private renderer: Renderer2, private http: HttpClient) {
    this.http.get('http://localhost:5050/api/domain').subscribe(
      (data: any) => {
        this.domains = data;
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  addDomain() {
    this.http
      .post('http://localhost:5050/api/domain', {
        domainName: this.domainName,
        createdBy: this.createdBy,
        edition: this.edition,
      })
      .subscribe({
        next: (data: any) => {
          this.domains = data;
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }
}