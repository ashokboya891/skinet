import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../orders.service';
import { ActivatedRoute } from '@angular/router';
import { Breadcrumb } from 'xng-breadcrumb/lib/types/breadcrumb';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order?:Order;

  constructor(private orderService:OrdersService,
    private route:ActivatedRoute,private breadCrumb:BreadcrumbService) {
    
  }
  ngOnInit(): void {
    const id=this.route.snapshot.paramMap.get('id');
    id && this.orderService.getOrdersDetails(+id).subscribe({
      next:ord=>{
        this.order=ord,
        console.log(ord.status);
        
        this.breadCrumb.set('@OrderDetailed', `Order# ${ord.id} - ${ord.status}`);
      }
    })

  }


}
