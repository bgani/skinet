import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, isObservable, Observable, pipe, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // The job of BehaviorSubject is to immedietly emit initial value, and it is not suitable to use with auth guard in our case
  // That's why we gonna use ReplaySubject, we specify the number of values we want the ReplaySubject to hold e.g 1
  // And it is gonna cache the value so we can use it
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  loadCurrentUser(token: string | null) {
    if(token === null) {
      this.currentUserSource.next(null as any);
      return new Observable<void>();
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http
      .get<IUser>(this.baseUrl + 'account', { headers })
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        })
      );
  }

  login(values: any) {
    return this.http.post<IUser>(this.baseUrl + 'account/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http
      .post<IUser>(this.baseUrl + 'account/register', values)
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null as any);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }
}
