import { inject } from "@angular/core"
import { Router } from "@angular/router";
import { map } from "rxjs";
import { AccountService } from "src/app/account/account.service"


// import { NavigationExtras } from '@angular/router';

export const authGuard = () => {
    const accountService = inject(AccountService);
    const router = inject(Router);

    return accountService.currentUser$.pipe(
        map(auth => {
            if (auth) return true;
            else {
                router.navigate(['/account/login'],{queryParams:{returnUrl:router.url}})
                return false;
                // const navigationExtras: NavigationExtras = {
                //     queryParams: { returnUrl: router.url }
                // };
                // router.navigateByUrl('/account/login', navigationExtras);
                // return false;
            }
        })
    );
};


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
