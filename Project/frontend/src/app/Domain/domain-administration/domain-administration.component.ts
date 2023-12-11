import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-domain-administration',
  templateUrl: './domain-administration.component.html',
  styleUrls: ['./domain-administration.component.css'],
})
export class DomainAdministrationComponent {
  domains: any[];
  name: string = '';
  logo: string = '';
  edition: string = '';
  isSsoEnabled: boolean = false;
  comment: string = '';
  parentCompany: string = '';
  selectedDomain: any;

  isNewDomainMode: boolean = false;

  constructor(private renderer: Renderer2, private http: HttpClient, private router: Router) {
    this.http.get('http://localhost:5050/api/domain').subscribe(
      (data: any) => {
        this.domains = data;
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  onSelect(selectedDomain: any) {
    this.selectedDomain = selectedDomain;
  }

  startNewDomainMode() {
    console.log("je rentre dans startnewdomain");
    this.isNewDomainMode = true;

    this.name = '';
    this.logo = '';
    this.edition = '';
    this.isSsoEnabled = false;
    this.comment = '';
    this.parentCompany = '';
  }

  endNewDomainMode() {
    this.isNewDomainMode = false;
  }

  addDomain() {
    if(this.isNewDomainMode) {
      this.http
        .post('http://localhost:5050/api/domain', {
          name: this.name,
          logo: this.logo,
          edition: this.edition,
          isSsoEnabled: this.isSsoEnabled,
          comment: this.comment,
          parentCompany: this.parentCompany
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
}
