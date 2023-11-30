import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit{
  product?:Product;
  constructor(private shopService:ShopService,private activatedRoute:ActivatedRoute,
    private bcService:BreadcrumbService)
  {

      this.bcService.set('@productDetail','  ')
  }
  ngOnInit(): void {
   this.loadProduct();
  }
  loadProduct()
  {
    const id=this.activatedRoute.snapshot.paramMap.get('id');
     if(id) this.shopService.getProduct(+id).subscribe({
      next:product=>{this.product=product;
        this.bcService.set('@productDetail',product.name)
      },
      error:error=>console.log(error)
      
      


     })
  }

}
