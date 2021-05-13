import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(  //casting to User
      map((response: User) => {
        const user = response as User;//as User
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles)? user.roles = roles : user.roles.push(roles);//checks if roles are array, if not create an array with 1 element
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);//Does not accept: .next(null) if strict: true. Does then create problem on logout. Empty user is still a user that's logged in.
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
