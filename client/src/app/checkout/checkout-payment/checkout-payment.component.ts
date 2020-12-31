import { templateJitUrl } from '@angular/compiler';
import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { IOrder } from 'src/app/shared/models/order';
import { CheckoutService } from '../checkout.service';

declare var Stripe: any;

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss'],
})
export class CheckoutPaymentComponent
  implements OnInit, AfterViewInit, OnDestroy {
  @Input() checkoutForm: FormGroup = new FormGroup({});
  @ViewChild('cardNumber', { static: true })
  cardNumerElement: ElementRef = null as any;
  @ViewChild('cardExpiry', { static: true })
  cardExpiryElement: ElementRef = null as any;
  @ViewChild('cardCvc', { static: true })
  cardCvcElement: ElementRef = null as any;
  stripe: any;
  cardNumber: any;
  cardExpiry: any;
  cardCvc: any;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngAfterViewInit(): void {
    this.stripe = Stripe(
      'pk_test_51I428MFx8bcBmC8ZlbIZLGAoaUbFF77y0i9mJnMM2ArDywm5FsfC80e5mvG34HXasa5xRwuLFTddcDc2vkKoZGJG00zDvlAyq0'
    );
    const elements = this.stripe.elements();

    // stripe elements
    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumerElement.nativeElement);
    this.cardNumber.addEventListener('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.addEventListener('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.addEventListener('change', this.cardHandler);
  }

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  ngOnInit(): void {}

  onChange({ error }: { error: any }) {
    if (error) {
      // error comes from stripe
      this.cardErrors = error.message;
    } else {
      this.cardErrors = null;
    }
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    const order = this.getOrderToCreate(basket);
    this.checkoutService.createOrder(order).subscribe(
      (order: IOrder) => {
        this.toastr.success('Order created successfully');
        this.stripe
          .confirmCardPayment(basket.clientSecret, {
            payment_method: {
              card: this.cardNumber,
              billing_details: {
                name: this.checkoutForm.get('paymentForm')?.get('nameOnCard')
                  ?.value,
              },
            },
          })
          .then((result: any) => {
            
            console.log(result);
            if (result.paymentIntent) {
              this.basketService.deleteLocalBasket(basket.id);
              const navigationExtras: NavigationExtras = { state: order };
              this.router.navigate(['checkout/success'], navigationExtras);
            } else {
              this.toastr.error(result.error.message);
            }
          });
      },
      (error) => {
        this.toastr.error(error.message);
        console.log(error);
      }
    );
  }

  private getOrderToCreate(basket: IBasket) {
    console.log(basket);
    return {
      basketId: basket.id,
      deliveryMethodId: +this.checkoutForm
        .get('deliveryForm')
        ?.get('deliveryMethod')?.value,
      shipToAddress: this.checkoutForm.get('address')?.value,
    };
  }
}
