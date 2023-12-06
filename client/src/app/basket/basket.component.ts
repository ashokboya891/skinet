import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/Basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent {

  constructor(public basketService:BasketService) {
    
  }
  incrementQuantity(item:BasketItem)
  {
    this.basketService.addItemToBasket(item);

  }
  RemoveItem(id:number,quantity:number)
  {
    this.basketService.RemoveItemFromBasket(id,quantity);
    
  }
}
