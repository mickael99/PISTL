import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('LoginService', () => {
  let service: AppComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppComponent);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should log in with valid credentials', () => {
    const isLoggedIn = service.login('test', 'password');
    expect(isLoggedIn).toBeTrue();
  });

  it('should not log in with invalid credentials', () => {
    const isLoggedIn = service.login('test', 'oui');
    expect(isLoggedIn).toBeFalse();
  });
});
