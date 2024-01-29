import { HttpClient } from '@angular/common/http';
import { Component, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';

/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
/* represent a domain linked to an environment */

/***************************************************************************************/
export class EnvironmentModel {
  DomainId: number;
  Environment: number;
  BpwebServerId: number;
  BpdatabaseId: number;
  EaidatabaseId: number;
  SsrsserverId: number;
  TableauServerId: number;
  EaiftpserverId: number;
  IsBp5Enabled: boolean;
}

export class ServersAndDatabasesName {
  public nameBpwebServer: string;
  public nameBpDatabase: string;
  public nameEaiDatabase: string;
  public nameSsrsServer: string;
  public nameTableauServer: string;
  public nameEaiftpServer: string;
}

/***************************************************************************************/
@Component({
  selector: 'app-domain-administration',
  templateUrl: './domain-administration.component.html',
  styleUrls: ['./domain-administration.component.css'],
})
export class DomainAdministrationComponent {
  /* data get into the Databases */
  domains: any[];
  domainEnvironments: any[];
  databases: any[];
  servers: any[];

  serversAndDatabasesName: ServersAndDatabasesName;

  /* domain selected from the list */
  selectedDomain: any;
  selectedDomainEnvironments: any[];
  selectedDomainEnvironment: any;

  /*domain fields */
  name: string;
  logo: string;
  edition: string;
  isSsoEnabled: boolean;
  comment: string;
  parentCompany: string;

  /* selected environment while adding or editing */
  idEnvSelected: number;

  /* boolean values to manage the options to create and edit a domain */
  isNewDomainMode: boolean = false;
  isEditDomainMode: boolean = false;

  selectedEnabled: { [key: number]: boolean } = {};

  //bp5 link to each environment
  selectedBp5: { [key: number]: boolean } = {};

  /* selected servers when the DAT user want to edit or create a domain.
   *  Each servers is link to the selected environment by the user
   */
  selectedWebServers: { [key: number]: any } = {};
  selectedEaiFtpServers: { [key: number]: any } = {};
  selectedTableauServers: { [key: number]: any } = {};
  selectedSsrsServers: { [key: number]: any } = {};

  /* selected databases when the DAT user want to edit or create a domain.
   *  Each servers is link to the selected environment by the user
   */
  selectedDatabaseServers: { [key: number]: any } = {};
  selectedElasticPools: { [key: number]: any } = {};

  // Logo used to load to 'this.logo' after the user has selected a img file
  logoToAdd: any;

  /*
   *  Update the selected domain from the list and print the dev information.
   *  If the selected domain is not link to the dev environment, preprod is printed and so on
   *
   * @selectedDomain the selected domain
   */
  onSelect(selectedDomain: any): void {
    this.selectedDomain = selectedDomain;
    if (selectedDomain && selectedDomain.logo)
      this.displayLogo(selectedDomain.logo, 'printLogo');

    this.selectedDomainEnvironments = this.domainEnvironments.filter(
      (env) => env.domainId == this.selectedDomain.domainId
    );

    this.onEnvironmentSelect(this.idEnvSelected);
  }

  onSelectServer(
    serverType: string,
    selectedServer: any,
    environmentId: number
  ): void {
    switch (serverType) {
      case 'webServer':
        this.selectedWebServers[environmentId] = selectedServer;
        break;
      case 'eaiFtpServer':
        this.selectedEaiFtpServers[environmentId] = selectedServer;
        break;
      case 'tableauServer':
        this.selectedTableauServers[environmentId] = selectedServer;
        break;
      case 'ssrsServer':
        this.selectedSsrsServers[environmentId] = selectedServer;
        break;
      default:
        console.error('Server not recognized:', serverType);
        break;
    }
  }

  onSelectDatabase(
    databaseType: string,
    selectedDatabase: any,
    environmentId: number
  ): void {
    switch (databaseType) {
      case 'databaseServer':
        this.selectedDatabaseServers[environmentId] = selectedDatabase;
        break;
      case 'elasticPool':
        this.selectedElasticPools[environmentId] = selectedDatabase;
        break;
      default:
        console.error('Database not recognized:', databaseType);
        break;
    }
  }

  resetBp5ServersAndDatabasesList(): void {
    const interne = (environment: number): void => {
      this.selectedEnabled[environment] = false;
      this.selectedBp5[environment] = false;
      this.selectedDatabaseServers[environment] = undefined;
      this.selectedWebServers[environment] = undefined;
      this.selectedEaiFtpServers[environment] = undefined;
      this.selectedTableauServers[environment] = undefined;
      this.selectedSsrsServers[environment] = undefined;
      this.selectedElasticPools[environment] = undefined;
    };

    interne(2);
    interne(4);
    interne(8);
    interne(16);
    interne(32);
    interne(256);
  }

  /***************************************************************************************/
  /*
   * Get domains, servers and databases from the Databases.
   * Select the first occurence into the domain
   */
  constructor(
    private renderer: Renderer2,
    private http: HttpClient,
    private router: Router
  ) {
    const getDomains = (): void => {
      this.http.get('http://localhost:5050/api/domain').subscribe(
        (data: any) => {
          this.domains = data;
          // Changing the logo format
          this.domains.forEach((domain) => {
            // Because for the 'WWBP5' domain, the logo encoded hasn't the same format
            if (domain.name === 'WWBP5') {
              domain.logo = 'data:image/png;base64,' + domain.logo;
            } else {
              domain.logo = atob(domain.logo);
            }
          });

          if (this.domains.length) this.idEnvSelected = 2;
          this.onSelect(this.domains[0]);
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
      );
    };

    const getDomainEnvironments = (): void => {
      this.http.get('http://localhost:5050/api/domainEnvironment').subscribe(
        (data: any) => {
          this.domainEnvironments = data;
          getDomains();
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
      );
    };

    const getServers = (): void => {
      this.http.get('http://localhost:5050/api/server').subscribe(
        (data: any) => {
          this.servers = data;
          getDomainEnvironments();
        },
        (error) => {
          alert('Connection error: ' + error.message);
        }
      );
    };

    this.serversAndDatabasesName = new ServersAndDatabasesName();

    this.http.get('http://localhost:5050/api/database').subscribe(
      (data: any) => {
        this.databases = data;
        getServers();
      },
      (error) => {
        alert('Connection error: ' + error.message);
      }
    );
  }

  /***************************************************************************************/
  /*
   * Disable the environment radio button if the selected domain is not link
   * to the environment
   *
   * @environmentId the environment that need to check
   *
   * @return true if the domain is link to the environment
   *         false if not
   */
  isEnvironmentEnabled(environmentId: number): boolean {
    return !!this.selectedDomainEnvironments?.some(
      (env) => env.environment === environmentId
    );
  }

  findDatabaseNameFromSelectedEnvironment(): void {
    const databaseServer = this.selectedDomainEnvironment
      ? this.databases.find(
          (db) => db.databaseId === this.selectedDomainEnvironment?.bpdatabaseId
        )
      : undefined;
    this.serversAndDatabasesName.nameBpDatabase = databaseServer
      ? databaseServer.name
      : undefined;

    const elasticPool = this.selectedDomainEnvironment
      ? this.databases.find(
          (db) => db.databaseId === this.selectedDomainEnvironment.eaidatabaseId
        )
      : undefined;
    this.serversAndDatabasesName.nameEaiDatabase = elasticPool
      ? elasticPool.name
      : undefined;
  }

  findServerNameFromSelectedEnvironment(): void {
    if (this.selectedDomain) {
      const webServer = this.selectedDomainEnvironment
        ? this.servers.find(
            (s) => s.serverId === this.selectedDomainEnvironment.bpwebServerId
          )
        : undefined;
      this.serversAndDatabasesName.nameBpwebServer = webServer
        ? webServer.name
        : undefined;

      const eaiServer = this.selectedDomainEnvironment
        ? this.servers.find(
            (s) => s.serverId === this.selectedDomainEnvironment.eaiftpserverId
          )
        : undefined;
      this.serversAndDatabasesName.nameEaiftpServer = eaiServer
        ? eaiServer.name
        : undefined;

      const tableauServer = this.selectedDomainEnvironment
        ? this.servers.find(
            (s) => s.serverId === this.selectedDomainEnvironment.tableauServerId
          )
        : undefined;
      this.serversAndDatabasesName.nameTableauServer = tableauServer
        ? tableauServer.name
        : undefined;

      const ssrsServer = this.selectedDomainEnvironment
        ? this.servers.find(
            (s) => s.serverId === this.selectedDomainEnvironment.ssrsserverId
          )
        : undefined;
      this.serversAndDatabasesName.nameSsrsServer = ssrsServer
        ? ssrsServer.name
        : undefined;
    }
  }

  setEnvironment(environmentId: number): void {
    this.idEnvSelected = environmentId;
  }

  getRadioNameById(environementId: number) {
    switch (environementId) {
      case 2:
        return 'dev';
      case 4:
        return 'preprod';
      case 8:
        return 'prod';
      case 16:
        return 'test';
      case 32:
        return 'prodcopy';
      case 256:
        return 'staging';
      default:
        console.error('error in getaRadioNameById function');
        break;
    }
  }

  onEnvironmentSelect(environmentId: number): void {
    const domainEnvironmentLinked = this.domainEnvironments.filter(
      (env) => env.domainId == this.selectedDomain.domainId
    );
    if (domainEnvironmentLinked.length == 0) environmentId = 2;
    else if (
      domainEnvironmentLinked.every((env) => env.environment != environmentId)
    )
      environmentId = domainEnvironmentLinked[0].environment;

    this.setEnvironment(environmentId);

    if (!this.isNewDomainMode) {
      var element = document.getElementById(
        this.getRadioNameById(environmentId)
      ) as HTMLInputElement;
      element.checked = true;
    }

    if (this.selectedDomain && this.selectedDomainEnvironments) {
      this.selectedDomainEnvironment = this.selectedDomainEnvironments.find(
        (env) => env.environment == environmentId
      );
    }
    this.findDatabaseNameFromSelectedEnvironment();
    this.findServerNameFromSelectedEnvironment();
  }

  /***************************************************************************************/
  /**
   * Display the logo of the domain
   * @param encodingLogoPath the path of the logo encoded in base64
   */
  displayLogo(encodingLogoPath: string, id: string): void {
    const imagePreview = document.getElementById(id) as HTMLDivElement;
    console.log(encodingLogoPath);
    if (encodingLogoPath !== 'undefined') {
      imagePreview.innerHTML = `<img src="${encodingLogoPath}" alt="Logo" style="width: 10rem; height: 10rem;">`;
    } else {
      imagePreview.innerHTML = '';
    }
  }

  /***************************************************************************************/
  startNewDomainMode(): void {
    this.resetBp5ServersAndDatabasesList();
    this.idEnvSelected = 2;
    this.isNewDomainMode = true;

    this.name = '';
    this.logo = '';
    this.edition = '';
    this.isSsoEnabled = false;
    this.comment = '';
    this.parentCompany = '';
  }

  cancel(): void {
    this.endNewDomainMode();
    setTimeout(() => this.onSelect(this.selectedDomain), 5);
  }

  endNewDomainMode(): void {
    this.isEditDomainMode = false;
    this.isNewDomainMode = false;
  }

  getServerOrDatabaseIsSelected(): number[] {
    const checkIfServerOrDatabaseIsSelected = (
      environmentId: number
    ): boolean => {
      if (this.selectedDatabaseServers[environmentId] == 'undefined')
        this.selectedDatabaseServers[environmentId] = undefined;
      if (this.selectedWebServers[environmentId] == 'undefined')
        this.selectedWebServers[environmentId] = undefined;
      if (this.selectedEaiFtpServers[environmentId] == 'undefined')
        this.selectedEaiFtpServers[environmentId] = undefined;
      if (this.selectedTableauServers[environmentId] == 'undefined')
        this.selectedTableauServers[environmentId] = undefined;
      if (this.selectedSsrsServers[environmentId] == 'undefined')
        this.selectedSsrsServers[environmentId] = undefined;
      if (this.selectedElasticPools[environmentId] == 'undefined')
        this.selectedElasticPools[environmentId] = undefined;

      return (
        this.selectedEnabled[environmentId] &&
        this.selectedWebServers[environmentId] != undefined
      );
    };

    let selectedEnvironment: number[] = [];
    if (checkIfServerOrDatabaseIsSelected(2)) selectedEnvironment.push(2);
    if (checkIfServerOrDatabaseIsSelected(4)) selectedEnvironment.push(4);
    if (checkIfServerOrDatabaseIsSelected(8)) selectedEnvironment.push(8);
    if (checkIfServerOrDatabaseIsSelected(16)) selectedEnvironment.push(16);
    if (checkIfServerOrDatabaseIsSelected(32)) selectedEnvironment.push(32);
    if (checkIfServerOrDatabaseIsSelected(256)) selectedEnvironment.push(256);

    return selectedEnvironment;
  }

  addDomainEnvironment(domain, environmentsModel, makeOnSelectAction): void {
    this.http
      .post('http://localhost:5050/api/domainEnvironment', environmentsModel)
      .subscribe({
        next: (data: any) => {
          this.domainEnvironments = data.env;
          if (makeOnSelectAction) this.idEnvSelected = 2;
          this.onSelect(domain);
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
  }

  /***************************************************************************************/
  /**
   * Add a new domain into the database
   */
  addDomain(): void {
    let selectedEnvironments: number[];
    selectedEnvironments = this.getServerOrDatabaseIsSelected();
    let environmentsModel = new Array<EnvironmentModel>();
    for (const e of selectedEnvironments) {
      let environmentToAdd = new EnvironmentModel();
      environmentToAdd.DomainId = this.selectedDomain.domainId;
      environmentToAdd.Environment = e;
      environmentToAdd.BpwebServerId =
        this.selectedWebServers[e]?.serverId ?? undefined;
      environmentToAdd.BpdatabaseId =
        this.selectedDatabaseServers[e]?.databaseId ?? undefined;
      environmentToAdd.EaidatabaseId =
        this.selectedElasticPools[e]?.databaseId ?? undefined;
      environmentToAdd.SsrsserverId =
        this.selectedSsrsServers[e]?.serverId ?? undefined;
      environmentToAdd.TableauServerId =
        this.selectedTableauServers[e]?.serverId ?? undefined;
      environmentToAdd.EaiftpserverId =
        this.selectedEaiFtpServers[e]?.serverId ?? undefined;
      environmentToAdd.IsBp5Enabled = this.selectedBp5[e];

      environmentsModel.push(environmentToAdd);
    }
    this.logo = this.logoToAdd;

    const requestBody = {
      Name: this.name,
      Logo: btoa(this.logo),
      Edition: this.edition,
      IsSsoEnabled: this.isSsoEnabled,
      Comment: this.comment,
      ParentCompany: this.parentCompany,
      Environments: environmentsModel,
    };

    //add a new domain
    this.http.post('http://localhost:5050/api/domain', requestBody).subscribe({
      next: (data: any) => {
        this.domains = data.domains;
        // Changing the logo format
        this.domains.forEach((domain) => {
          // Because for the 'WWBP5' domain, the logo encoded hasn't the same format
          if (domain.name === 'WWBP5') {
            domain.logo = 'data:image/png;base64,' + domain.logo;
          } else {
            domain.logo = atob(domain.logo);
          }
        });

        // Setting the selected domain
        let newDomainId = data.newDomainId;
        environmentsModel.forEach((e) => {
          e.DomainId = newDomainId;
        });
        let domain = this.domains.find((d) => d.domainId === newDomainId);

        this.addDomainEnvironment(domain, environmentsModel, true);
      },
      error: (error: any) => {
        alert('Connection error: ' + error.message);
      },
    });

    this.endNewDomainMode();
  }

  confirmSave(): void {
    if (!this.isNewDomainMode && !this.isEditDomainMode)
      throw new Error('problem while domain adding');
    var error = false;
    console.log('hbbdnbqndn ', typeof this.selectedWebServers[2]);
    if (
      (this.selectedEnabled[2] && this.selectedWebServers[2] === undefined) ||
      (this.selectedEnabled[2] && this.selectedWebServers[2] === 'undefined') ||
      (this.selectedEnabled[4] && this.selectedWebServers[4] === undefined) ||
      (this.selectedEnabled[4] && this.selectedWebServers[4] === 'undefined') ||
      (this.selectedEnabled[8] && this.selectedWebServers[8] === undefined) ||
      (this.selectedEnabled[8] && this.selectedWebServers[8] === 'undefined') ||
      (this.selectedEnabled[16] && this.selectedWebServers[16] === undefined) ||
      (this.selectedEnabled[16] &&
        this.selectedWebServers[16] === 'undefined') ||
      (this.selectedEnabled[32] && this.selectedWebServers[32] === undefined) ||
      (this.selectedEnabled[32] &&
        this.selectedWebServers[32] === 'undefined') ||
      (this.selectedEnabled[256] &&
        this.selectedWebServers[256] === undefined) ||
      (this.selectedEnabled[256] &&
        this.selectedWebServers[256] === 'undefined')
    ) {
      window.alert(
        'You have selected an environment without assigning a web server to it!'
      );
      error = true;
    }

    if (this.name === undefined || this.name === '') {
      window.alert('You must provide a name for the domain!');
      error = true;
    }

    if (!error) {
      //ajouter un domaine
      if (!this.isEditDomainMode) {
        const isConfirmed = window.confirm('Are you sure to add the domain ?');
        if (isConfirmed) this.addDomain();
      }
      //modifier un domaine
      else {
        const isConfirmed = window.confirm('Are you sure to edit the domain ?');
        if (isConfirmed) this.update();
      }
    }
  }

  onLogoChange(event: any): void {
    if (this.isFileFormatConformed(this.logo)) {
      const file = event.target.files[0];
      const reader = new FileReader();

      reader.onload = (e) => {
        const imagePreview = document.getElementById('imagePreview');
        imagePreview.innerHTML = `<img src="${e.target.result}" alt="Preview" style="width: 10rem; height: 10rem;">`;
      };

      reader.addEventListener('load', () => {
        this.logoToAdd = reader.result;
      });

      reader.readAsDataURL(file);
    } else {
      alert(
        'The format file is not correct, only jpeg jpg and png extention are allowed'
      );
      this.logo = '';
    }
  }

  isFileFormatConformed(file: string): boolean {
    const regex = /\.(jpeg|jpg|png)$/i;
    return regex.test(file);
  }

  findDatabaseFromId(id: number): any {
    return this.databases.find((d) => d.databaseId == id);
  }

  findServerFromId(id: number): any {
    return this.servers.find((s) => s.serverId == id);
  }

  editDomain(): void {
    if (this.selectedDomain) {
      this.name = this.selectedDomain.name;
      this.logo = this.selectedDomain.logo;
      this.edition = this.selectedDomain.edition;
      this.isSsoEnabled = this.selectedDomain.isSsoEnabled;
      this.comment = this.selectedDomain.comment;
      this.parentCompany = this.selectedDomain.parentCompany;

      console.log('this.logo: ', this.logo);

      console.log('this.selectedDomain.logo', this.selectedDomain.logo);
      setTimeout(
        () => this.displayLogo(this.selectedDomain.logo, 'imagePreview'),
        10
      );

      this.resetBp5ServersAndDatabasesList();
      console.log(this.selectedDomainEnvironments);
      this.selectedDomainEnvironments.forEach((e) => {
        this.selectedBp5[e.environment] = e.isBp5Enabled;
        this.selectedDatabaseServers[e.environment] = this.findDatabaseFromId(
          e.bpdatabaseId
        );
        this.selectedElasticPools[e.environment] = this.findDatabaseFromId(
          e.eaidatabaseId
        );
        this.selectedWebServers[e.environment] = this.findServerFromId(
          e.bpwebServerId
        );
        this.selectedEaiFtpServers[e.environment] = this.findServerFromId(
          e.eaiftpserverId
        );
        this.selectedTableauServers[e.environment] = this.findServerFromId(
          e.tableauServerId
        );
        this.selectedSsrsServers[e.environment] = this.findServerFromId(
          e.ssrsserverId
        );
        this.selectedEnabled[e.environment] = true;
      });

      this.isNewDomainMode = true;
      this.isEditDomainMode = true;
    }
  }

  update(): void {
    const updateDomain = () => {
      this.logo = this.logoToAdd;

      const domainToUpdate = {
        Name: this.name,
        Logo: btoa(this.logo),
        Edition: this.edition,
        IsSsoEnabled: this.isSsoEnabled,
        Comment: this.comment,
        ParentCompany: this.parentCompany,
      };

      this.http
        .put(
          `http://localhost:5050/api/domain/${this.selectedDomain.domainId}`,
          domainToUpdate
        )
        .subscribe(
          (data: any) => {
            this.domains = data.domains;
            this.domains.forEach((domain) => {
              // Because for the 'WWBP5' domain, the logo encoded hasn't the same format
              if (domain.name === 'WWBP5') {
                domain.logo = 'data:image/png;base64,' + domain.logo;
              } else {
                domain.logo = atob(domain.logo);
              }
            });
            console.table(this.domains);
            this.endNewDomainMode();
            const index = this.domains.findIndex(
              (d) => d.domainId == data.domain.domainId
            );
            setTimeout(() => this.onSelect(this.domains[index]), 5);
          },
          (error) => {
            alert('Connection error: ' + error.message);
          }
        );
    };

    const deleteDomainEnv = (domainEnvironments, index) => {
      if (domainEnvironments.length > index) {
        const id = domainEnvironments[index];
        this.http
          .delete(`http://localhost:5050/api/domainEnvironment/${id}`)
          .subscribe(
            (data: any) => {
              this.domainEnvironments = data;
              deleteDomainEnv(domainEnvironments, index + 1);
            },
            (error) => {
              alert('Connection error: ' + error.message);
            }
          );
      } else updateDomain();
    };

    const checkIfDeleteDomainEnv = (
      domainEnvLinkedBeforeUpdate,
      domainEnvLinkedAfterUpdate
    ) => {
      let arrayDomainEnvToDelete = new Array<number>();
      const idDomainEnvToDelete = domainEnvLinkedBeforeUpdate.filter(
        (e) => !domainEnvLinkedAfterUpdate.includes(e)
      );

      idDomainEnvToDelete.forEach((e) => {
        const domainEnvIdToDelete = this.domainEnvironments.find(
          (env) =>
            env.domainId == this.selectedDomain.domainId && env.environment == e
        ).domainEnvironmentId;

        arrayDomainEnvToDelete.push(domainEnvIdToDelete);
      });

      if (arrayDomainEnvToDelete.length > 0)
        deleteDomainEnv(arrayDomainEnvToDelete, 0);
      else updateDomain();
    };

    const checkIfAddingDomainEnv = (
      domainEnvLinkedBeforeUpdate,
      domainEnvLinkedAfterUpdate
    ) => {
      let arrayDomainEnvToAdd = new Array<EnvironmentModel>();
      const idDomainEnvToAdd = domainEnvLinkedAfterUpdate.filter(
        (e) => !domainEnvLinkedBeforeUpdate.includes(e)
      );

      idDomainEnvToAdd.forEach((e) => {
        const domainEnvToAdd = new EnvironmentModel();
        domainEnvToAdd.DomainId = this.selectedDomain.domainId;
        domainEnvToAdd.Environment = e;
        domainEnvToAdd.BpwebServerId = this.selectedWebServers[e].serverId;
        domainEnvToAdd.BpdatabaseId =
          this.selectedDatabaseServers[e]?.databaseId ?? undefined;
        domainEnvToAdd.EaidatabaseId =
          this.selectedElasticPools[e]?.databaseId ?? undefined;
        domainEnvToAdd.SsrsserverId =
          this.selectedSsrsServers[e]?.serverId ?? undefined;
        domainEnvToAdd.TableauServerId =
          this.selectedTableauServers[e]?.serverId ?? undefined;
        domainEnvToAdd.EaiftpserverId =
          this.selectedEaiFtpServers[e]?.serverId ?? undefined;
        domainEnvToAdd.IsBp5Enabled = this.selectedBp5[e];

        arrayDomainEnvToAdd.push(domainEnvToAdd);
      });

      this.http
        .post(
          'http://localhost:5050/api/domainEnvironment',
          arrayDomainEnvToAdd
        )
        .subscribe({
          next: (data: any) => {
            this.domainEnvironments = data.env;
            checkIfDeleteDomainEnv(
              domainEnvLinkedBeforeUpdate,
              domainEnvLinkedAfterUpdate
            );
          },
          error: (error: any) => {
            alert('Connection error: ' + error.message);
          },
        });
    };

    const updateDomainEnv = (
      arrayDomainEnv,
      index,
      domainEnvLinkedBeforeUpdate,
      domainEnvLinkedAfterUpdate
    ) => {
      if (
        arrayDomainEnv.length > index &&
        this.selectedWebServers[arrayDomainEnv[index].environment] === undefined
      )
        updateDomainEnv(
          arrayDomainEnv,
          index + 1,
          domainEnvLinkedBeforeUpdate,
          domainEnvLinkedAfterUpdate
        );
      else {
        if (arrayDomainEnv.length > index) {
          const e = arrayDomainEnv[index].environment;
          const domainEnvUpdating = new EnvironmentModel();
          domainEnvUpdating.DomainId = this.selectedDomain.domainId;
          domainEnvUpdating.Environment = e;
          domainEnvUpdating.BpwebServerId =
            this.selectedWebServers[e]?.serverId;
          domainEnvUpdating.BpdatabaseId =
            this.selectedDatabaseServers[e]?.databaseId ?? undefined;
          domainEnvUpdating.EaidatabaseId =
            this.selectedElasticPools[e]?.databaseId ?? undefined;
          domainEnvUpdating.SsrsserverId =
            this.selectedSsrsServers[e]?.serverId ?? undefined;
          domainEnvUpdating.TableauServerId =
            this.selectedTableauServers[e]?.serverId ?? undefined;
          domainEnvUpdating.EaiftpserverId =
            this.selectedEaiFtpServers[e]?.serverId ?? undefined;
          domainEnvUpdating.IsBp5Enabled = this.selectedBp5[e];

          this.http
            .put(
              `http://localhost:5050/api/domainEnvironment/${arrayDomainEnv[index].domainEnvironmentId}`,
              domainEnvUpdating
            )
            .subscribe(
              (data: any) => {
                this.domainEnvironments = data.domainEnvironments;
                updateDomainEnv(
                  arrayDomainEnv,
                  index + 1,
                  domainEnvLinkedBeforeUpdate,
                  domainEnvLinkedAfterUpdate
                );
              },
              (error) => {
                alert('Connection error: ' + error.message);
              }
            );
        } else
          checkIfAddingDomainEnv(
            domainEnvLinkedBeforeUpdate,
            domainEnvLinkedAfterUpdate
          );
      }
    };

    const checkIfUpdateDomainEnv = (
      domainEnvLinkedBeforeUpdate,
      domainEnvLinkedAfterUpdate
    ) => {
      const domainEnvToUpdate = this.domainEnvironments.filter(
        (e) => e.domainId == this.selectedDomain.domainId
      );

      if (domainEnvToUpdate.length)
        updateDomainEnv(
          domainEnvToUpdate,
          0,
          domainEnvLinkedBeforeUpdate,
          domainEnvLinkedAfterUpdate
        );
      else
        checkIfAddingDomainEnv(
          domainEnvLinkedBeforeUpdate,
          domainEnvLinkedAfterUpdate
        );
    };

    if (this.selectedDomain && this.isEditDomainMode) {
      const domainEnvLinkedBeforeUpdate = this.domainEnvironments
        .filter((e) => e.domainId == this.selectedDomain.domainId)
        .map((env) => env.environment);

      const domainEnvLinkedAfterUpdate = this.getServerOrDatabaseIsSelected();
      checkIfUpdateDomainEnv(
        domainEnvLinkedBeforeUpdate,
        domainEnvLinkedAfterUpdate
      );
    }
  }

  copyDomain(): void {
    const postDomainEnvironment = (newDomainId) => {
      const domainId = this.selectedDomain.domainId;

      const envToCopied = this.domainEnvironments.filter(
        (e) => e.domainId == domainId
      );

      envToCopied.forEach((e) => (e.domainId = newDomainId));

      this.http
        .post('http://localhost:5050/api/domainEnvironment', envToCopied)
        .subscribe({
          next: (data: any) => {
            this.domainEnvironments = data.env;
            const domain = this.domains.find((e) => e.domainId == newDomainId);
            this.onSelect(domain);
          },
          error: (error: any) => {
            alert('Connection error: ' + error.message);
          },
        });
    };

    const postDomain = (domain) => {
      this.http.post('http://localhost:5050/api/domain', domain).subscribe({
        next: (data: any) => {
          this.domains = data.domains;
          // Changing the logo format
          this.domains.forEach((domain) => {
            // Because for the 'WWBP5' domain, the logo encoded hasn't the same format
            if (domain.name === 'WWBP5') {
              domain.logo = 'data:image/png;base64,' + domain.logo;
            } else {
              domain.logo = atob(domain.logo);
            }
          });
          const newDomainId = data.newDomainId;

          postDomainEnvironment(newDomainId);
        },
        error: (error: any) => {
          alert('Connection error: ' + error.message);
        },
      });
    };

    if (this.selectedDomain) {
      const isConfirmed = window.confirm('Are you sure to copy the domain?');
      if (isConfirmed) {
        const newDomain = {
          Name: 'Copy - ' + this.selectedDomain.name,
          Logo: btoa(this.selectedDomain.logo),
          Edition: this.selectedDomain.edition,
          IsSsoEnabled: this.isSsoEnabled,
          Comment: this.comment,
          ParentCompany: this.parentCompany,
        };

        postDomain(newDomain);
      }
    }
  }

  deleteDomain(): void {
    const deleteDomain = (domainIdToDelete) => {
      this.http
        .delete(`http://localhost:5050/api/domain/${domainIdToDelete}`)
        .subscribe(
          (data: any) => {
            this.domains = data;
            this.domains.forEach((domain) => {
              // Because for the 'WWBP5' domain, the logo encoded hasn't the same format
              if (domain.name === 'WWBP5') {
                domain.logo = 'data:image/png;base64,' + domain.logo;
              } else {
                domain.logo = atob(domain.logo);
              }
            });
            this.onSelect(this.domains[0]);
          },
          (error) => {
            alert('Connection error: ' + error.message);
          }
        );
    };

    const deleteDomainEnvironment = (
      domainEnvironmentsToDelete,
      index,
      domainIdToDelete
    ) => {
      if (domainEnvironmentsToDelete.length <= index)
        deleteDomain(domainIdToDelete);
      console.log(domainEnvironmentsToDelete[index].domainEnvironmentId);
      this.http
        .delete(
          `http://localhost:5050/api/domainEnvironment/${domainEnvironmentsToDelete[index]?.domainEnvironmentId}`
        )
        .subscribe(
          (data: any) => {
            this.domainEnvironments = data;
            deleteDomainEnvironment(
              domainEnvironmentsToDelete,
              index + 1,
              domainIdToDelete
            );
          },
          (error) => {
            alert('Connection error: ' + error.message);
          }
        );
    };

    if (this.selectedDomain) {
      const isConfirmed = window.confirm(
        'Are you sure to delete this domain ?'
      );
      if (isConfirmed) {
        const domainIdToDelete = this.selectedDomain.domainId;

        const domainEnvironmentsToDelete = this.domainEnvironments.filter(
          (e) => e.domainId == domainIdToDelete
        );

        deleteDomainEnvironment(
          domainEnvironmentsToDelete,
          0,
          domainIdToDelete
        );
      }
    }
  }
}
/***************************************************************************************/
/***************************************************************************************/
/***************************************************************************************/
