import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent  implements OnInit{
  order?:Order;
  ngOnInit(): void {


  }
  constructor(private router:Router) {
   const nav= this.router.getCurrentNavigation();
   this.order=nav?.extras?.state as Order;

  }

}
