import { Component } from '@angular/core';
import { AddSessionRequest } from '../models/add-session-request-model';
import { SessionService } from '../services/session.service';

@Component({
  selector: 'app-add-session',
  templateUrl: './add-session.component.html',
  styleUrls: ['./add-session.component.css']
})
export class AddSessionComponent {

  model: AddSessionRequest;

  constructor(private sessionService: SessionService) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(){
    this.sessionService.addSession(this.model)
    .subscribe({
      next: (response) => {
        console.log("This was successful!")
      },
      error: (response) => {
        console.log("This failed")
      }
    })
  }
}
