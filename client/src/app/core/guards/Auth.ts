// import { inject } from "@angular/core"
// import { Router } from "@angular/router";
// import { map } from "rxjs";
// import { AccountService } from "src/app/account/account.service"



// export const authGuard = () => {
//     console.log('AuthGuard canActivate called');

//     const accountService = inject(AccountService);
//     const router = inject(Router);

//     return accountService.currentUser$.pipe(
//         map(auth => {
//             if (auth) {return true;
//             console.log(auth);}
            
//             else {
//                 router.navigate(['/account/login'],{queryParams:{returnUrl:router.url}})
//                 return false;
          
//             }
//         })
//     );
// };


// export const authGuard=()=>{
//     const accountService=inject(AccountService);
//     const router=inject(Router);

//     return accountService.currentUser$.pipe(
//         map(auth=>{
//             if(auth) return true;
//             else
//             {
//                 router.navigateByUrl('/account/login',{queryParams:{returnUrl:router.url}});
//                 return false;
//             }
//         })
//     )

// }
// import { NavigationExtras } from '@angular/router';

// export const authGuard = () => {
//     const accountService = inject(AccountService);
//     const router = inject(Router);

//     return accountService.currentUser$.pipe(
//         map(auth => {
//             if (auth) return true;
//             else {
//                 const navigationExtras: NavigationExtras = {
//                     queryParams: { returnUrl: router.url }
//                 };
//                 router.navigateByUrl('/account/login', navigationExtras);
//                 return false;
//             }
//         })
//     );
// };
import { inject } from "@angular/core";
import { map, take } from "rxjs/operators";
import { AccountService } from "src/app/account/account.service";
import { Router } from "@angular/router";
import { state } from "@angular/animations";

export const authGuard = () => {
  console.log('AuthGuard canActivate called');

  const accountService = inject(AccountService);
  const router = inject(Router);
  console.log(router.url);
  
  return accountService.currentUser$.pipe(

    take(1), // Take only the first emitted value (complete the observable after one emission)
    map(auth => {
      if (auth) {
        console.log('User is authenticated:', auth);
        console.log('inside if'+router.url);

        return true;
      } else {
        console.log('User is not authenticated. Redirecting to login.');
        router.navigate(['/account/login'], { queryParams: { returnUrl:'/checkout' } });
        console.log('inside else'+router.url);

        return false;
      }
    })
  );
};
