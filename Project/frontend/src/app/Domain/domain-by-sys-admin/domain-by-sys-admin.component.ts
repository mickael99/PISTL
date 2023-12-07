import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Renderer2 } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomainBySysAdminComponentDialog } from './domain-by-sys-admin-dialog.component';

@Component({
  selector: 'app-domain-by-sys-admin',
  templateUrl: './domain-by-sys-admin.component.html',
  styleUrls: ['./domain-by-sys-admin.component.css'],
})
export class DomainBySysAdminComponent implements OnInit {
  domains:  { [domainId: number]: any } = {};
  users: { [logindId: number]: any } = {};
  logins: { [logindId: number]: any } = {};
  envs: { [environmentId: number]: any } = {};
  selectedUser: any;
  domainName: string = '';

  constructor(private renderer: Renderer2, private http: HttpClient, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5050/api/domainbysysadmin').subscribe(
      (data: any) => {
        for (const domain of data.domains) {
          this.domains[domain.domainId] = domain;
        }
        for (const user of data.users) {
          this.users[user.loginId] = user;
        }
        for (const login of data.logins) {
          this.logins[login.loginId] = login;
        }
        for (const env of data.envs) {
          this.envs[env.domainEnvironmentId] = env;
        }
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  onSelect(user: any): void {
    this.selectedUser = user;
  }

  onAddedToEnv(user: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    if(user.environment === env){
      user.sysAdmin = !user.sysAdmin;
      this.users[user.loginId] = user;
    }else{
      console.log('User not in env, new user');
      user = {
        loginId: user.loginId,
        domainId: user.domainId,
        userId: user.userId,
        environment: env,
        sysAdmin: true,
        sysAdminStartDate: null,
        sysAdminEndDate: null,
        comment: '',
        modifiedBy: 'admin', // TODO: Replace with the actual login
      };
    }

    dialogConfig.data = user;
    console.log('Dialog data:', dialogConfig.data);

    // Open the dialog
    const dialogRef = this.dialog.open(DomainBySysAdminComponentDialog, dialogConfig);

    // Subscribe to the dialog's afterClosed event to handle the result
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Dialog result:', result);
        this.http.post('http://localhost:5050/api/domainbysysadmin', {
          loginId: result.user.loginId,
          domainId: result.user.domainId,
          userId: result.user.userId,
          environment: result.user.environment,
          sysAdmin: result.user.sysAdmin,
          sysAdminStartDate: result.from,
          sysAdminEndDate: result.to,
          comment: result.comment,
          modifiedBy: 'admin', // TODO: Replace with the actual login
        }).subscribe(
          (data: any) => {
            console.log('Dialog result:', data);
          },
          (error) => {  
            alert('Connection error: ' + error.message);
          }
        );
      }
    });
  }
}
