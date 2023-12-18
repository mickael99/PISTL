import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DomainBySysAdminComponentDialog } from './domain-by-sys-admin-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-domain-by-sys-admin',
  templateUrl: './domain-by-sys-admin.component.html',
  styleUrls: ['./domain-by-sys-admin.component.css'],
})
export class DomainBySysAdminComponent implements OnInit {
  domains:  any[] = [];
  users:  any[] = [];
  logins: any[] = [];
  dataDomainTable: any[] = [];
  selectedLogin: any;
  domainName: string = '';

  dataSource: any;
  displayedColumns: string[] = ['domain', 'dev', 'preprod', 'prod', 'test', 'prodCopy', 'staging'];
  @ViewChild(MatPaginator) paginator !: MatPaginator;
  @ViewChild(MatSort) sort !: MatSort;

  constructor(private http: HttpClient, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5050/api/sysadminbydomain').subscribe(
      (data: any) => {
        for (const domain of data.domains) {
          this.domains.push(domain);
        }
        for (const login of data.logins) {
          this.logins.push(login);
        }
        this.selectedLogin = this.logins[4];

        for (const user of data.users) {
          this.users.push(user);
        }

        this.onSelect({value: this.selectedLogin});
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  onSelect(event: any): void {
    this.selectedLogin = event.value;
    console.log('onSelect', this.selectedLogin);

    var domain_object;
    this.dataDomainTable = [];
    for (const domain of this.domains) {
      domain_object = [];
      var tab_users = [];
      // max 6 users per login for a domain
      for (const user of this.users) {
        if(user.domainId === domain.domainId){
          if(user.loginId === this.selectedLogin.loginId){   
            tab_users.push(user);
          }
        }
      }
      
      if(tab_users.length > 0){
        domain_object.push(domain.name + "["+domain.domainId+"]");
        for(var i = 1; i <= 6; i++){
          var found = false;
          for(const user of tab_users){
            if(user.environment === i){
              domain_object.push([user.sysAdmin, user]);
              found = true;
              break;
            }
          }
          if(!found){
            domain_object.push([false, null]);
          }
        }
        this.dataDomainTable.push(domain_object);
      }
    }
    console.log('dataDomainTable', this.dataDomainTable);
    this.dataSource = new MatTableDataSource(this.dataDomainTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  onAddedToEnv(user: any, env: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;

    console.log("user: ", user);
    if(user){
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
          this.http.put('http://localhost:5050/api/sysadminbydomain', {
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

  /**
   * Checks if at least one domain is checked for a given environment.
   * 
   * @param env - The environment to check.
   * @returns True if at least one domain is checked for the given environment, false otherwise.
   */
  oneChecked(env: any): boolean {
    for (const d of Object.keys(this.dataDomainTable)) {
      if(this.dataDomainTable[d].length > 0){
        for(const user of this.dataDomainTable[d].slice(1)){
          if(user[0] === true && user[1].environment === env){
            return true;
          }
        }
      }
    }
    return false;
  }

  /**
   * Unchecks all login users for a given environment.
   * @param env - The environment to uncheck for.
   */
  unCheckAll(env: any): void {
    console.log('unCheckAll');
    for (const d of Object.keys(this.dataDomainTable)) {
      for(const domain of this.dataDomainTable[d]){
        domain[env][0] = false;
        this.onAddedToEnv(domain[env][1], env);
      }
    }
  }
}
