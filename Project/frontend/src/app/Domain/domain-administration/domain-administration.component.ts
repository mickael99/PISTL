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
  isEditDomainMode: boolean = false;

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

  displayLogo(encodingLogoPath: string): void {
    const imagePreview = document.getElementById('printLogo') as HTMLDivElement;
    imagePreview.innerHTML = `<img src="data:image/png;base64,${encodingLogoPath}" alt="Logo" style="max-width: 100%; max-height: 100%;">`;
  }

  onSelect(selectedDomain: any) : void {
    console.log("chemin du fichier => ", selectedDomain.logo)
    this.selectedDomain = selectedDomain;
    if(selectedDomain && selectedDomain.logo)
      this.displayLogo(selectedDomain.logo);
  }

  startNewDomainMode() : void {
    this.isNewDomainMode = true;

    this.name = '';
    this.logo = '';
    this.edition = '';
    this.isSsoEnabled = false;
    this.comment = '';
    this.parentCompany = '';
  }

  endNewDomainMode() : void {
    if(this.isNewDomainMode)
      this.isNewDomainMode = false;
  }

  endEditDomainMode() : void {
    if(this.isEditDomainMode)
      this.isEditDomainMode = false;
  }

  addDomain() : void {
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
    if(!this.isNewDomainMode && !this.isEditDomainMode)
      throw new Error("problem while domain adding");

    //ajouter un domaine
    if(!this.isEditDomainMode) {
      const isConfirmed = window.confirm("Are you sure to add the domain ?");
      if(isConfirmed)
        this.addDomain();
    }
    //modifier un domaine
    else {
      const isConfirmed = window.confirm("Are you sure to edit the domain ?");
      if(isConfirmed)
        this.updateDomain();
    }
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

  editDomain() : void {
    if(this.selectedDomain) {
      this.isNewDomainMode = true;
      this.isEditDomainMode = true;

      this.name = this.selectedDomain.name;
      this.logo = this.selectedDomain.logo;
      this.edition = this.selectedDomain.edition;
      this.isSsoEnabled = this.selectedDomain.isSsoEnabled;
      this.comment = this.selectedDomain.comment;
      this.parentCompany = this.selectedDomain.parentCompany;
    }
  }

  updateDomain() : void {
    if(this.selectedDomain && this.isEditDomainMode) {
      const updatedDomain = {
        domainId: this.selectedDomain.domainId,
        name: this.name,
        logo: this.logo,
        edition: this.edition,
        isSsoEnabled: this.isSsoEnabled,
        comment: this.comment,
        parentCompany: this.parentCompany, 
      };

      this.http.put(`http://localhost:5050/api/domain/${this.selectedDomain.domainId}`, {
        name: this.name,
        logo: this.logo,
        edition: this.edition,
        isSsoEnabled: this.isSsoEnabled,
        comment: this.comment,
        parentCompany: this.parentCompany
        })
        .subscribe((data: any) => {
          this.domains = data;
        }, (error) => {
          alert("Connection error: " + error.message);
        });
        this.endEditDomainMode();
        this.endNewDomainMode();
    }
  }

  copyDomain(): void {
    if (this.selectedDomain) {
      const isConfirmed = window.confirm("Are you sure to copy the domain?");
      if (isConfirmed) {
        this.name = this.selectedDomain.name;
        this.logo = this.selectedDomain.logo;
        this.edition = this.selectedDomain.edition;
        this.isSsoEnabled = this.selectedDomain.isSsoEnabled;
        this.comment = this.selectedDomain.comment;
        this.parentCompany = this.selectedDomain.parentCompany;
  
        // Ajoutez le domaine copié en appelant la fonction addDomain
        this.addDomain();
      }
    }
  }
}
