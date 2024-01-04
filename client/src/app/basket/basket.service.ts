import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { BehaviorSubject, isEmpty, map } from 'rxjs';
import { Basket, BasketItem, BasketTotals } from '../shared/models/Basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';
import { deliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl=environment.apiUrl;
  private basketSource=new BehaviorSubject<Basket|null>(null);
  basketSource$=this.basketSource.asObservable();
  private  basketTotalSource=new BehaviorSubject<BasketTotals| null>(null);
  basketTotalSourc$=this.basketTotalSource.asObservable();
  
  
  constructor(private http:HttpClient) { }
  
  createPaymentIntent()
  {
    return  this.http.post<Basket>(this.baseUrl+'payments/'+this.getCurrentBasketValue()?.id,{})
    .pipe(
      map(basket=>{
        this.basketSource.next(basket);
        console.log(basket.paymentIntentId);
      })
    )
  }

  setShippingPrice(deliveryMethod:deliveryMethod)
  {
    const basket=this.getCurrentBasketValue();
    // this.shipping=deliveryMethod.price;
    if(basket) 
    {
    basket.shippingPrice=deliveryMethod.price
      basket.deliveryMethodId=deliveryMethod.id;
      this.setBasket(basket);
    }
    // this.calculateTotals(); cmtnd in sec-21 after stripe and setshipping updated
  }

  getBasket(id:string)
  {
    return this.http.get<Basket>(this.baseUrl+'basket?id='+id).subscribe({
      next:basket=>{
        this.basketSource.next(basket)
        // console.log(this.basketSource+'printing after getting it from observables');
        this.calculateTotals() 
      }
    })

  }
  setBasket(basket:Basket)
  {
    return this.http.post<Basket>(this.baseUrl+'basket',basket).subscribe({
      next:basket=>{
        this.basketSource.next(basket)
        this.calculateTotals();
      }
    })
  }
  getCurrentBasketValue()
  {
    return this.basketSource.value
  }
  addItemToBasket(item:Product| BasketItem,quantity=1)
  {
   
    if(this.isProduct(item)) item=this.mapProductItemToBasket(item);
    const basket=this.getCurrentBasketValue()??this.createBasket()
    basket.items=this.addOrUpdateItem(basket.items,item,quantity);
    this.setBasket(basket);

  }
  RemoveItemFromBasket(id:number,quantity=1)
  {
    const basket=this.getCurrentBasketValue();
    if(!basket) return;
    const item=basket.items.find(x=>x.id===id);
    if(item)
    {
      item.quantity-=quantity;
      if(item.quantity===0)
      {
       basket.items= basket.items.filter(x=>x.id!=id);
      }
      if(basket.items.length>0)this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }
  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe({
      next:()=>{
        this.deleteLocalBasket();
      }
      //below part cmnt in sec 19 after order placed basket as to remove from both localstorage & redis there also we invoking this and here also to bin symbol in checkout
      // next:()=>{
      //   this.basketSource.next(null);
      //   this.basketTotalSource.next(null);
      //   localStorage.removeItem('basket_id');
      // }
    })
  }
  deleteLocalBasket()
  {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');

  }

  
   private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    // if (!items) {
    //   console.error("Items array is not defined.");
    //   return items;
    // }
  const item=items.find(x=>x.id===itemToAdd.id);
    if(item) item.quantity +=quantity;
    else
    {
      itemToAdd.quantity=quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket  {
    const basket=new Basket();
    localStorage.setItem('basket_id',basket.id);
    return basket;

  }
  private mapProductItemToBasket(Item:Product):BasketItem
  {
    return{
      id:Item.id,
      productName:Item.name,
      price:Item.price,
      quantity:0,
      pictureUrl:Item.pictureUrl,
      brand:Item.productBrand,
      type:Item.productType
    }

  }
  private calculateTotals()
  {
    const basket=this.getCurrentBasketValue();
    if(!basket) return;
    // const shipping=0;
    const subtotal=basket.items.reduce((a,b)=>(b.price*b.quantity)+a,0);
    const total=subtotal+basket.shippingPrice;
    this.basketTotalSource.next({shipping:basket.shippingPrice,total,subtotal});
  }

  private isProduct(item:Product| BasketItem):item is Product
  {
    return (item as Product).productBrand!==undefined;
  }
}


  // addItemToBasket(item: Product, quantity = 1) {
  //   const itemToAdd = this.mapProductItemToBasket(item);
  //   const basket = this.getCurrentBasketValue() ?? this.createBasket();
  
  //   // Ensure that basket.items is initialized as an array
  //   if (!basket.items) {
  //     basket.items = [];
  //   }
  
  //   // Use a temporary variable for better type inference
  //   const updatedItems = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
  
  //   if (updatedItems) {
  //     basket.items = updatedItems;
  //     this.setBasket(basket);
  //   } else {
  //     console.error("Error updating basket items.");
  //   }
  // }
  
  // private addOrUpdateItem(items: BasketItem[] | undefined, itemToAdd: BasketItem, quantity: number): BasketItem[] | undefined {
  //   if (!items) {
  //     console.error("Items array is not defined.");
  //     return items;
  //   }
  
  //   const item = items.find(x => x.id === itemToAdd.id);
  //   if (item) {
  //     item.quantity += quantity;
  //   } else {
  //     itemToAdd.quantity = quantity;
  //     items.push(itemToAdd);
  //   }
  //   return items;
  // }
