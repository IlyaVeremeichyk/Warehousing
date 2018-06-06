import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { OrderRequestComponent } from './order-request/order-request.component';
import { GeneralOrderDataComponent } from './order-request/general-order-data/general-order-data.component';
import { OrderProductsComponent } from './order-request/order-products/order-products.component';
import { OrderTransportComponent } from './order-request/order-transport/order-transport.component';
import { NgDatepickerModule } from 'ng2-datepicker';
import { RouterModule, Routes } from '@angular/router';
import { VisualizationComponent } from './visualization/visualization.component';

const appRoutes: Routes = [
  { path: '', component: OrderRequestComponent },
  { path: 'asd', component: VisualizationComponent }
];


@NgModule({
  declarations: [
    AppComponent,
    OrderRequestComponent,
    GeneralOrderDataComponent,
    OrderProductsComponent,
    OrderTransportComponent,
    VisualizationComponent
  ],
  imports: [
    BrowserModule, FormsModule, NgDatepickerModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true }
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
