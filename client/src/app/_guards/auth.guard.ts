import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
constructor(private accountService: AccountService, private toastr: ToastrService) {}

  canActivate(): Observable<boolean>{
    return <Observable<boolean>>(this.accountService.currentUser$.pipe(
      map(user => {
        if (user) return true;
        this.toastr.error("You shall not pass!")
        return false;
      })
    ))
  }//Own modified code for property to bypass error. Casting to observable<boolean> and "return false" added
  
}
