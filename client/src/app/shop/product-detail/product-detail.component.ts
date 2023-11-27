import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit{
  product?:Product;
  constructor(private shopService:ShopService,private activatedRoute:ActivatedRoute)
  {


  }
  ngOnInit(): void {
this.loadProduct();
  }
  loadProduct()
  {
    const id=this.activatedRoute.snapshot.paramMap.get('id');
     if(id) this.shopService.getProduct(+id).subscribe({
      next:product=>this.product=product,
      error:error=>console.log(error)
      
      


     })
  }

}
