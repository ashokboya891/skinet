import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/Basket';
import { Address } from 'src/app/shared/models/user';
import { LinkedList } from 'ngx-bootstrap/utils';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent {
  @Input() checkoutForm?: FormGroup;
  constructor(private basketService:BasketService,
  private checkoutService:CheckoutService,private tostr:ToastrService,private router:Router)
  {
    
  }
  // basket:LinkedList;
  submitOrder()
  {
    const basket=this.basketService.getCurrentBasketValue();
    console.log('basket values'+ basket?.id+'items are as folloes'+basket?.items.toString());
    // this.basket=basket?.items;
    if(!basket) return ;
    const orderToCreate=this.getOrderToCreate(basket);
    // console.log(orderToCreate?.basketID+"inside payments");
    
    if(!orderToCreate) return;
    this.checkoutService.createOrder(orderToCreate).subscribe({
      next:order=>{
        this.tostr.success('Order Created Successfully');
        this.basketService.deleteLocalBasket();
        
        console.log(order);
        const navi:NavigationExtras={state:order};
        this.router.navigate(['checkout/success'],navi);
      }
    })
  }
  private getOrderToCreate(basket:Basket)
  {
    const deliveryMethodId=this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress=this.checkoutForm?.get('addressForm')?.value as Address;
    if(!deliveryMethodId || !shipToAddress) return;
    return {
      basketID: basket.id,
      deliveryMethodId: deliveryMethodId,
      ShipToAddress: shipToAddress
    }
  }


}
