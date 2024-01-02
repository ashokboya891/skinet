import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  providers:[{provide:CdkStepper,useExisting:StepperComponent}]
})
export class StepperComponent extends CdkStepper implements OnInit {

  @Input() linearModeSelected=true;
  
  ngOnInit(): void {
    this.linearModeSelected=this.linearModeSelected;
  }

  onClick(index:number)
  {
    this.selectedIndex=index;     //selected index coming from cdkstepper moddule that we installed
    
  }

}
