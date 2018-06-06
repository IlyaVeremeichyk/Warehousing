import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
    selector: 'order-request',
    templateUrl: 'order-request.component.html'
})

export class OrderRequestComponent implements OnInit {

    @ViewChild("products") productsComponent;
    @ViewChild("transports") transportsComponent;

    constructor() { }

    ngOnInit() { }

    submit(){
        this.productsComponent.clean();
        this.transportsComponent.clean();
    }
}