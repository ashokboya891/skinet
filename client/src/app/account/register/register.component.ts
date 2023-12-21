import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';
import { debounce, debounceTime, finalize, map, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errors:string[]|null=null;

  complesPassword="(?=^.{6,15}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$";

  constructor(private fb:FormBuilder,private accountService:AccountService,private router:Router)
  {
    
  }
  registerForm=this.fb.group({
    displayName:['',Validators.required],
    email:['',[Validators.required,Validators.email],[this.validateEmailNotTaken()]],
    password:['',[Validators.required,Validators.pattern(this.complesPassword)]],

  })
  onSubmit()
  {
    this.accountService.register(this.registerForm.value).subscribe({
      next:()=>this.router.navigateByUrl('/shop'),
      error:error=>this.errors=error.errors
    })
  }
  validateEmailNotTaken():AsyncValidatorFn
  {
    return (control:AbstractControl)=>{
      return control.valueChanges.pipe(
        debounceTime(1000),
        take(1),
        switchMap(()=>{
          return this.accountService.checkEmailExists(control.value).pipe(
            map(result=>result ?{emailExists:true}:null),
            finalize(()=>control.markAsTouched())
          );
          
        })
      )
    }
  }
}
