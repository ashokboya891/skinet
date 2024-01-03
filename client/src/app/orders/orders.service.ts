import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Order } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
baseUrl=environment.apiUrl;
orders=[]=[];
  constructor(private http:HttpClient) { }

  getOrders()
  {
    return this.http.get<Order[]>(this.baseUrl+'orders');

  }
  getOrdersDetails(id:number)
  {
    return this.http.get<Order>(this.baseUrl+'orders/'+id);

  }
}
