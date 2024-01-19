import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DomainAdministrationComponent } from './Domain/domain-administration/domain-administration.component';
import { DomainBySysAdminComponent } from './Domain/domain-by-sys-admin/domain-by-sys-admin.component';
import { SysAdminByDomainComponent } from './Domain/sys-admin-by-domain/sys-admin-by-domain.component';
import { MatDialogModule } from '@angular/material/dialog';
import { ServerParameterComponent } from './server-parameter/server-parameter.component';

const routes: Routes = [
  { path: 'domain-administration', component: DomainAdministrationComponent },
  { path: 'sys-admin-by-domain', component: SysAdminByDomainComponent },
  { path: 'domain-by-sys-admin', component: DomainBySysAdminComponent },
  { path: 'server-parameter', component: ServerParameterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), MatDialogModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
