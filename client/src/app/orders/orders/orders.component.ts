import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  order:Order[]=[];
  constructor(private ordersService:OrdersService) {
    
  }
  ngOnInit(): void {
    this.getOrders();
  }
  getOrders()
  {
    this.ordersService.getOrders().subscribe({
      next:ord=>{
        this.order=ord;
      }
    })
  }


}
