import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { OrderRequestComponent } from './order-request/order-request.component';
import { GeneralOrderDataComponent } from './order-request/general-order-data/general-order-data.component';
import { OrderProductsComponent } from './order-request/order-products/order-products.component';
import { OrderTransportComponent } from './order-request/order-transport/order-transport.component';

@NgModule({
  declarations: [
    AppComponent,
    OrderRequestComponent,
    GeneralOrderDataComponent,
    OrderProductsComponent,
    OrderTransportComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
