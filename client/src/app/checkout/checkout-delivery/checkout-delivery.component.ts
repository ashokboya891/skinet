import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { deliveryMethod } from 'src/app/shared/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  deliveryMethods:deliveryMethod[]=[];
  constructor(private checkoutservice:CheckoutService,private basketService:BasketService) {

  
  }

  ngOnInit(): void {
    this.checkoutservice.getDeliveryMethods().subscribe({
      next:dm=>this.deliveryMethods=dm
    })
  }
   setShppingPrice(deliveryMethod:deliveryMethod)
   {
    this.basketService.setShippingPrice(deliveryMethod);

   }
}
