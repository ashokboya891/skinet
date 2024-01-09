import { ChangeDetectorRef, Component, ElementRef, Input, NgZone, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { CheckoutService } from '../checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from 'src/app/shared/models/Basket';
import { Address } from 'src/app/shared/models/user';
import { NavigationExtras, Router } from '@angular/router';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { firstValueFrom } from 'rxjs';
import { OrderToCreate } from 'src/app/shared/models/order';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit{

  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber')cardNumberElement?:ElementRef;
  @ViewChild('cardExpiry')cardExpiryElement?:ElementRef;
  @ViewChild('cardCvc')cardCvcElement?:ElementRef;
  stripe:Stripe|null=null;
  cardNumber?:StripeCardNumberElement;
  cardExpiry?:StripeCardExpiryElement;
  cardCvc?:StripeCardCvcElement;
  cardErrors:string|null=null;
  cardNumberComplete=false;
  cardExpiryComplete=false;
  cardCvcComplete=false;

loading=false;


  constructor(private basketService:BasketService,
  private checkoutService:CheckoutService,private tostr:ToastrService,private router:Router,private ngZone: NgZone,private cdr: ChangeDetectorRef)
  {
    
  }
  ngOnInit(): void {
    loadStripe('pk_test_51OUTGKSFVmSbHbAjJ99lyHIjdg3ttrLIddBMc12OeN51ilD6lfBAZlYAO81tH4CbQhDCNCvydAAull8QggLfq8yP00C7HZ6IKm').then(stripe=>{
      this.stripe=stripe;
      const elements=stripe?.elements();
   
      if(elements)
      {
        this.cardNumber=elements.create('cardNumber');
        this.cardNumber.mount(this.cardNumberElement?.nativeElement);
        this.cardNumber.on('change',event=>{
          // console.log(event.error?.message+'inside loadstripe');
          console.log(event);
          this.cardNumberComplete=event.complete;  
          if(event.error)this.cardErrors=event.error.message;
          else this.cardErrors=null;
        });

        this.cardExpiry=elements.create('cardExpiry');
        this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
        this.cardExpiry.on('change',event=>{
          this.cardExpiryComplete=event.complete;  
          if(event.error)this.cardErrors=event.error.message;
          else this.cardErrors=null;
        });

        this.cardCvc=elements.create('cardCvc');
        this.cardCvc.mount(this.cardCvcElement?.nativeElement);
        this.cardCvc.on('change',event=>{
          this.cardCvcComplete=event.complete;
          if(event.error)this.cardErrors=event.error.message;
          else this.cardErrors=null;
        });
      }
    })
  }
  get paymentFormComplete()
  {
    return this.checkoutForm?.get('paymentForm')?.valid 
    &&this.cardNumberComplete 
    && this.cardExpiryComplete 
    &&this.cardCvcComplete
  }
  // basket:LinkedList;
  async submitOrder()
  {
    this.loading=true;
    const basket=this.basketService.getCurrentBasketValue();
    // console.log('basket values'+ basket?.id+'items are as folloes'+basket?.items.toString());
    // this.basket=basket?.items;
    if(!basket) throw new Error('cannot get basket');
    
    try {
      const createOrder=this.createOrder(basket);
      const paymentResult=await this.confirmPaymentWithStripe(basket);
      if(paymentResult.paymentIntent) {
        // this.basketService.deleteLocalBasket();
        this.basketService.deleteBasket(basket);
        console.log(createOrder);
        const navi:NavigationExtras={state:createOrder};
        this.router.navigate(['checkout/success'],navi);
      } else{
        this.tostr.error(paymentResult.error.message);
      }
      
    } catch (error:any) {
      console.log(error);
      this.tostr.error(error.message);   
    }
    finally
    {
      this.loading=false;
    }
    //belowe all cmnt in 268 code splited into minimized methods
    // if(!basket) return ;
    // console.log(orderToCreate?.basketID+"inside payments");
    
    // if(!orderToCreate) return;
    // this.checkoutService.createOrder(orderToCreate).subscribe({
    //   next:order=>{
    //     // this.tostr.success('Order Created Successfully');
    //     console.log(basket.clientSecret);
        
    //     this.stripe?.confirmCardPayment(basket.clientSecret!,{
    //       payment_method:{
    //         card:this.cardNumber!,
    //         billing_details:{
    //           name:this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
    //         }
    //       }
    //     }).then(result=>{
    //       console.log(result);
    //       if(result.paymentIntent) {
    //         this.basketService.deleteLocalBasket();
    //         console.log(order);
    //         const navi:NavigationExtras={state:order};
    //         this.router.navigate(['checkout/success'],navi);
    //       } else{
    //         this.tostr.error(result.error.message);
    //       }
    //     })
    //   }
    // })
  }

 private async confirmPaymentWithStripe(basket: Basket | null) {
  if(!basket) throw new Error('Basket is null');
  const result=this.stripe?.confirmCardPayment(basket.clientSecret!,{
    payment_method:{
      card:this.cardNumber!,
      billing_details:{
        name:this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
      }
    }
  });
  if(!result) throw new Error('Problem attempting payment with stripe');
  return result;
 }

 private async createOrder(basket:Basket|null) {

  if(!basket) throw new Error('Basket is null');
  const orderToCreate=this.getOrderToCreate(basket);
  return  firstValueFrom( this.checkoutService.createOrder(orderToCreate));

  }
  private getOrderToCreate(basket:Basket):OrderToCreate
  {
    const deliveryMethodId=this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress=this.checkoutForm?.get('addressForm')?.value as Address;
    if(!deliveryMethodId || !shipToAddress) throw new Error('Problem with basket');
    return {
      basketID: basket.id,
      deliveryMethodId: deliveryMethodId,
      ShipToAddress: shipToAddress
    }
  }


}
