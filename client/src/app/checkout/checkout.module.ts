import { NgModule, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutComponent } from './checkout.component';
import { CheckoutRoutingModule } from './checkout-routing.module';



@NgModule({
  declarations: [
    CheckoutComponent
  ],
  imports: [
    CommonModule,
    CheckoutRoutingModule
  ]
})
export class CheckoutModule implements OnInit {
  
  ngOnInit(): void {
    console.log('Checkout module loaded');
  }
  // Inside checkout.module.ts

  
 }
