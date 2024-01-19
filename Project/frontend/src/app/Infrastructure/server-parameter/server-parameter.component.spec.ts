import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerParameterComponent } from './server-parameter.component';

describe('ServerParameterComponent', () => {
  let component: ServerParameterComponent;
  let fixture: ComponentFixture<ServerParameterComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ServerParameterComponent]
    });
    fixture = TestBed.createComponent(ServerParameterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
