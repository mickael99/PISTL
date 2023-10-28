import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SessionListComponent } from './features/session/session-list/session-list.component';
import { AddSessionComponent } from './features/session/add-session/add-session.component';

const routes: Routes = [
  {
    path: 'admin/sessions',
    component: SessionListComponent
  },
  {
    path: 'admin/sessions/add',
    component: AddSessionComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
