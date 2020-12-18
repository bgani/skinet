import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket } from '../shared/models/basket';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  // since it is a BegaviorSubject it is always gonna emit an initial value
  // in strict mode we can not pass null to new BehaviorSubject<IBasket>(null), we can pass null as any
  private basketSource = new BehaviorSubject<IBasket>(null as any);
  basket$ = this.basketSource.asObservable();
  constructor(private http: HttpClient) { }
  
  getBasket(id: string){
    return this.http.get<IBasket>(this.baseUrl + 'basket?id='+id)
    .pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
      })
    );
  }

  setBasket(basket: IBasket){
    return this.http.post<IBasket>(this.baseUrl + 'basket', basket)
      .subscribe((response: IBasket) => {
          this.basketSource.next(response);
      }, 
      error => {
        console.log(error);
      });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }
}
