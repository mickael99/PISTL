import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Renderer2, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ngxCsv } from 'ngx-csv/ngx-csv';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
})
export class ReportsComponent {
  // Report to show/
  reportsList = [
    'User Activity',
    'SysAdmin By Domain History',
    'Domain Administration History',
  ];

  // Environment hash table
  refEnvironment = {
    0: 'Unknown',
    1: 'Local',
    2: 'Dev',
    4: 'Preprod',
    8: 'Prod',
    16: 'Test',
    32: 'ProdCopy',
    256: 'Staging',
    512: 'SimuCopy',
  };

  // LogAction hash table
  refActions = {
    I: 'Insert',
    U: 'Update',
    D: 'Delete',
    V: 'View ?',
  };

  // Selected report
  selected_report: string = '';

  // Selected domain
  selected_domain: string = '';

  // Bool to disable/enable the export button
  isToExport: boolean = true;

  // Logins/Users form the DB
  logins: any;

  // Domains from the DB
  domains: any;

  // Selected all users from the User Activity report
  checkAllUsers: boolean = false;

  // Selected all domains from the SysAdmin By Domain History report
  checkAllDomains: boolean = false;

  // Bool that allows or not to display the error popup
  showPopupError: boolean = false;

  // Error message to display in the popup
  popupMessage: string = '';

  // Paginator used for the table
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  // Sort used for the table
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  // Columns names in the table
  displayedLoginColumns: string[] = ['email'];

  // Columns names in the table
  displayedDomainColumns: string[] = ['name'];

  // Table used for login data display
  dataSourceLogin: any;

  // Table used for domain data display
  dataSourceDomain: any;

  // Start date for the reports
  selectedStartDate: Date;

  // End date for the reports
  selectedEndDate: Date;

  /***************************************************************************************/
  /**
   * Creates an instance of ReportsComponent.
   * @param renderer - The renderer used to create the reports page.
   * @param http - The http client used to connect to the database.
   */
  constructor(
    private renderer: Renderer2,
    private http: HttpClient,
    private dialog: MatDialog
  ) {
    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Get the all the users from the DB
    this.http.get('http://localhost:5050/api/users', options).subscribe(
      (data: any) => {
        this.logins = data.users.map((user: any) => {
          user.checked = false; // Add a checked property to the user object
          return user;
        });
        this.dataSourceLogin = new MatTableDataSource(this.logins);
        this.dataSourceLogin.paginator = this.paginator;
        // this.dataSource.sort = this.sort;
      },
      (error) => {
        this.showErrorPopup(error.error);
      }
    );

    // Get all the domains from the DB
    this.http.get('http://localhost:5050/api/domain').subscribe(
      (data: any) => {
        this.domains = data.map((domain: any) => {
          domain.checked = false; // Add a checked property to the domain object
          return domain;
        });
        this.dataSourceDomain = new MatTableDataSource(this.domains);
        this.dataSourceDomain.paginator = this.paginator;
        // this.dataSource.sort = this.sort;
      },
      (error) => {
        this.showErrorPopup(error.message);
      }
    );
  }

  /***************************************************************************************/
  /**
   * Sets the selected report to the value selected by the user.
   * @param event - The event that triggered the report selection.
   */
  report_selection(event: any) {
    this.reinitialize_reports();
    this.selected_report = event.value;
  }

  /***************************************************************************************/
  /**
   * Function used to close the error popup.
   */
  closeErrorPopup() {
    this.showPopupError = false;
  }

  /***************************************************************************************/
  /**
   * Function used to display activate the error popup.
   * @param message - Error message.
   */
  showErrorPopup(message: string) {
    this.showPopupError = true;
    this.popupMessage = message;
  }

  /***************************************************************************************/
  user_checked(event: any, user: any): void {
    if (event.checked) {
      user.checked = true;
    } else {
      user.checked = false;
    }
  }

  /***************************************************************************************/
  /**
   * Function used to select all the users from the User Activity report if checkbox checked.
   * @param event - The event that triggered the checkbox selection.
   */
  user_check_all(event: any): void {
    if (event.checked) {
      this.logins.forEach((user: any) => {
        user.checked = true;
      });
    } else {
      this.logins.forEach((user: any) => {
        user.checked = false;
      });
    }
  }

  /***************************************************************************************/
  /**
   * Sets the selected domain to the value selected by the user.
   * @param event - The event that triggered the domain selection.
   */
  domain_selection(event: any) {
    this.selected_domain = event.value;
  }

  /***************************************************************************************/
  /**
   * Function used to reinitialize the variables.
   */
  reinitialize_reports() {
    this.selected_report = '';
    this.selected_domain = '';
    this.checkAllUsers = false;
    this.checkAllDomains = false;
    this.logins &&
      this.logins.forEach((user: any) => {
        user.checked = false;
      });
    this.domains &&
      this.domains.forEach((domain: any) => {
        domain.checked = false;
      });
    this.selectedStartDate = undefined;
    this.selectedEndDate = undefined;
  }

  /***************************************************************************************/
  /**
   * Used to export only information between the selected dates.
   */
  date_verification(date: Date): boolean {
    if (
      this.selectedStartDate === undefined ||
      this.selectedEndDate === undefined
    ) {
      return true;
    } else if (date >= this.selectedStartDate && date <= this.selectedEndDate) {
      return true;
    }
    return false;
  }

  /***************************************************************************************/
  /**
   * Function used to export the User Activity Report.
   */
  export_user_activity_report(): void {
    let dataCsv: any = [];

    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Get data from Login_Log table
    this.http
      .get('http://localhost:5050/api/reports/userActivity', options)
      .subscribe(
        (data: any) => {
          let usersChecked = this.logins.filter(
            (user: any) => user.checked === true
          );
          dataCsv = data.loginLog
            .map((user: any) => {
              let usersFilter = usersChecked.filter(
                (userCh: any) => userCh.loginId === user.loginId // or userCh.email === user.email
              );
              if (
                usersFilter.length > 0 &&
                this.date_verification(new Date(user.logDate))
              ) {
                return {
                  login_date:
                    user.logDate !== null
                      ? new Date(user.logDate).toDateString() +
                        ' ' +
                        new Date(user.logDate).toLocaleTimeString()
                      : 'N/A',
                  role:
                    user.datenabled === true ? 'DAT Enabled' : 'DAT Disabled',
                  profile: 'N/A',
                  login: user.email,
                  platform: 'N/A',
                  LoginId: user.loginId,
                  LogAction: this.refActions[user.logAction],
                };
              } else {
                return null;
              }
            })
            .filter((item: any) => item !== null);

          var optionsCsv = {
            filedSeparator: ',',
            quoteStrings: '"',
            decimalseparator: '.',
            showLabels: true,
            useBom: true,
            headers: [
              'Login Date',
              'Role',
              'Profile',
              'Login',
              'Platform',
              'LoginId',
              'LogAction',
            ],
          };
          new ngxCsv(dataCsv, 'User_Activity_Report', optionsCsv);
        },
        (error) => {
          this.showErrorPopup(error.message);
        }
      );
  }

  /***************************************************************************************/
  /**
   * Used to check the domain selected to be check.
   * @param event - The event that triggered the checkbox selection.
   * @param domain - The domain to be checked.
   */
  domain_checked(event: any, domain: any): void {
    if (event.checked) {
      domain.checked = true;
    } else {
      domain.checked = false;
    }
  }

  /***************************************************************************************/
  /**
   * Function used to select all the domains from the SysAdmin By Domain History report if checkbox checked.
   * @param event - The event that triggered the checkbox selection.
   */
  domain_check_all(event: any): void {
    if (event.checked) {
      this.domains.forEach((domain: any) => {
        domain.checked = true;
      });
    } else {
      this.domains.forEach((domain: any) => {
        domain.checked = false;
      });
    }
  }

  /***************************************************************************************/
  /**
   * Function used to export the SysAdmin by Domain History.
   */
  export_sysAdmin_by_domain_history_report(): void {
    let dataCsv: any = [];

    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Get data from LoginDomainUserLog table
    this.http
      .get('http://localhost:5050/api/reports/sysAdmin', options)
      .subscribe(
        (data: any) => {
          let usersChecked = this.logins.filter(
            (user: any) => user.checked === true
          );
          let domainsChecked = this.domains.filter(
            (domain: any) => domain.checked === true
          );
          dataCsv = data.loginDomainUserLog
            .map((log: any) => {
              let usersFiltered = usersChecked.filter(
                (user: any) => user.loginId === log.loginId
              );
              let domainsFiltered = domainsChecked.filter(
                (domain: any) => domain.domainId === log.domainId
              );

              if (
                usersFiltered.length > 0 &&
                domainsFiltered.length > 0 &&
                (this.date_verification(new Date(log.logDate)) ||
                  this.date_verification(new Date(log.createdDate)) ||
                  this.date_verification(new Date(log.modifiedDate)))
              ) {
                return {
                  LoginDomainUser_LogID: log.loginDomainUserLogId,
                  LogAction: this.refActions[log.logAction],
                  LogDate:
                    log.logDate == null
                      ? 'N/A'
                      : new Date(log.logDate).toDateString() +
                        ' ' +
                        new Date(log.logDate).toLocaleTimeString(),
                  LoginID: log.loginId,
                  DomainID: log.domainId,
                  UserID: log.userId,
                  Environment: this.refEnvironment[log.environment],
                  UserName: log.userName,
                  UserActive: log.userActive,
                  LoginEnabled: log.loginEnabled,
                  LoginTypeId: log.loginTypeId,
                  AnalyticsEnabled: log.analyticsEnabled,
                  IsLight: log.isLight,
                  SysAdmin: log.sysAdmin,
                  CreatedDate:
                    log.createdDate == null
                      ? 'N/A'
                      : new Date(log.createdDate).toDateString() +
                        ' ' +
                        new Date(log.createdDate).toLocaleTimeString(),
                  sysAdminStartDate:
                    log.sysAdminStartDate == null
                      ? 'N/A'
                      : new Date(log.sysAdminStartDate).toDateString() +
                        ' ' +
                        new Date(log.sysAdminStartDate).toLocaleTimeString(),
                  sysAdminEndDate:
                    log.sysAdminEndDate == null
                      ? 'N/A'
                      : new Date(log.sysAdminEndDate).toDateString() +
                        ' ' +
                        new Date(log.sysAdminEndDate).toLocaleTimeString(),
                  Comment: log.comment,
                  CreatedBy: log.createdBy,
                  ModifiedDate:
                    log.modifiedDate == null
                      ? 'N/A'
                      : new Date(log.modifiedDate).toDateString() +
                        ' ' +
                        new Date(log.modifiedDate).toLocaleTimeString(),
                  ModifiedBy: log.modifiedBy,
                };
              }
            })
            .filter((item: any) => item !== undefined);

          var optionsCsv = {
            filedSeparator: ',',
            quoteStrings: '"',
            decimalseparator: '.',
            showLabels: true,
            useBom: true,
            headers: [
              'LoginDomainUser_LogID',
              'LogAction',
              'LogDate',
              'LoginID',
              'DomainID',
              'UserID',
              'Environment',
              'UserName',
              'UserActive',
              'LoginEnabled',
              'LoginTypeId',
              'AnalyticsEnabled',
              'IsLight',
              'SysAdmin',
              'CreatedDate',
              'SysAdminStartDate',
              'SysAdminEndDate',
              'Comment',
              'CreatedBy',
              'ModifiedDate',
              'ModifiedBy',
            ],
          };
          new ngxCsv(dataCsv, 'SysAdmin_by_Domain_History_Report', optionsCsv);
        },
        (error) => {
          this.showErrorPopup(error.message);
        }
      );
  }

  /***************************************************************************************/
  /**
   * Function used to export the Domain Administration History.
   */
  export_domain_administration_history_report(): void {
    let dataCsv: any = [];

    let JWTToken = localStorage.getItem('token');
    const options = {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + JWTToken,
        'Content-Type': 'application/json',
      }),
    };

    // Get data from DomainLog table
    this.http
      .get('http://localhost:5050/api/reports/domainLog', options)
      .subscribe(
        (data: any) => {
          dataCsv = data.domainLog
            .map((log: any) => {
              if (
                this.date_verification(new Date(log.modifiedDate)) ||
                this.date_verification(new Date(log.createdDate))
              ) {
                return {
                  DomainID: log.domainLogId,
                  Name: log.name,
                  Edition: log.edition,
                  IsSsoEnabled: log.isSsoEnabled,
                  Coment: log.comment,
                  CreatedDate:
                    log.createdDate == null
                      ? 'N/A'
                      : new Date(log.createdDate).toDateString() +
                        ' ' +
                        new Date(log.createdDate).toLocaleTimeString(),
                  CreatedBy: log.createdBy,
                  ModifiedDate:
                    log.modifiedDate == null
                      ? 'N/A'
                      : new Date(log.modifiedDate).toDateString() +
                        ' ' +
                        new Date(log.modifiedDate).toLocaleTimeString(),
                  ModifiedBy: log.modifiedBy,
                };
              }
            })
            .filter((item: any) => item !== undefined);

          var optionsCsv = {
            filedSeparator: ',',
            quoteStrings: '"',
            decimalseparator: '.',
            showLabels: true,
            useBom: true,
            headers: [
              'DomainLogID',
              'Name',
              'Edition',
              'IsSsoEnabled',
              'Coment',
              'CreatedDate',
              'CreatedBy',
              'ModifiedDate',
              'ModifiedBy',
            ],
          };
          new ngxCsv(
            dataCsv,
            'Domain_Administration_History_Report',
            optionsCsv
          );
        },
        (error) => {
          this.showErrorPopup(error.message);
        }
      );
  }

  /***************************************************************************************/
  /**
   * Function used to export the selected report.
   */
  export_reports(): void {
    if (this.selected_report === 'User Activity') {
      this.export_user_activity_report();
    } else if (this.selected_report === 'SysAdmin By Domain History') {
      this.export_sysAdmin_by_domain_history_report();
    } else if (this.selected_report === 'Domain Administration History') {
      this.export_domain_administration_history_report();
    } else {
      this.showErrorPopup('Please select a report to export.');
    }
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
