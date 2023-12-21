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

  displayLogo(EncodinglogoPath: string) : void {
    const imagePreview = document.getElementById('printLogo');
    imagePreview.innerHTML = `<img src="data:image/png;base64,${EncodinglogoPath}" alt="Logo" style="max-width: 10%; max-height: 10%;">`;
  }

  onSelect(selectedDomain: any) : void {
    console.log("chemin du fichier => ", selectedDomain.logo)
    this.selectedDomain = selectedDomain;
    if(selectedDomain && selectedDomain.logo)
      this.displayLogo(selectedDomain.logo);
  }

  startNewDomainMode() : void {
    console.log("je rentre dans startnewdomain");
    this.isNewDomainMode = true;

    this.name = '';
    this.logo = '';
    this.edition = '';
    this.isSsoEnabled = false;
    this.comment = '';
    this.parentCompany = '';
  }

  endNewDomainMode() : void {
    this.isNewDomainMode = false;
  }

  addDomain() : void {
    if(!this.isNewDomainMode)
      throw new Error("problem while domain adding");

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

      this.endNewDomainMode();
  } 

  confirmSave() : void {
    if(!this.isNewDomainMode)
      throw new Error("problem while domain adding");

    const isConfirmed = window.confirm("Are you sure to add the domain ?");
    if(isConfirmed)
      this.addDomain();
  }

  onLogoChange(event : any) : void {
    if(this.isFileFormatConformed(this.logo)) {
      const input = event.target;
      const reader = new FileReader();
      reader.onload = (e) => {
        const imagePreview = document.getElementById('imagePreview');
        imagePreview.innerHTML = `<img src="${e.target.result}" alt="Preview" style="max-width: 10%; max-height: 10%;">`;
      };
      reader.readAsDataURL(input.files[0]);
    }
    else {
      alert("The format file is not correct, only jpeg jpg and png extention are allowed");
      this.logo = '';
    }
  }

  isFileFormatConformed(file : string) : boolean {
    const regex = /\.(jpeg|jpg|png)$/i;
    return regex.test(file);
  }
}