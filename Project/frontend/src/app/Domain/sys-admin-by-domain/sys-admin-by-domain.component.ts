import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { DomainModel } from 'src/models/domain.model';
import { UserModel } from 'src/models/user.model';
import { LoginModel } from 'src/models/login.model';

@Component({
  selector: 'app-sys-admin-by-domain',
  templateUrl: './sys-admin-by-domain.component.html',
  styleUrls: ['./sys-admin-by-domain.component.css']
})
export class SysAdminByDomainComponent implements OnInit {
  domains: DomainModel[] = [];
  users: UserModel[] = [];
  logins: LoginModel[] = [];
  selectedDomain: any;
  domainName: string = '';

  constructor(private renderer: Renderer2, private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5050/api/sysadminbydomain').subscribe(
      (data: any) => {
        for (const domain of data.domains) {
          this.domains.push(domain);
        }
        for (const user of data.users) {
          this.users.push(user);
        }
        for (const login of data.logins) {
          this.logins.push(login);
        }
        console.log(this.domains[4].domainId);
        console.log(this.users[0].loginId);
        console.log(this.logins[0].email);
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  onSelect(domain: DomainModel): void {
    this.selectedDomain = domain;
    console.log(this.selectedDomain);
  }

  onCheckboxChange(user: UserModel): void {
    // Assuming user.sysAdmin is an object with properties like 'dev', 'preprod', etc.
    // Toggle the corresponding property value when the checkbox changes
    user.sysAdmin = !user.sysAdmin;
  
    // Here, you can handle additional logic based on the checkbox change if needed
    // For example, you might want to send an HTTP request to update the backend.
  }

}
