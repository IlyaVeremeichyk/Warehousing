import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'general-order-data',
    templateUrl: './general-order-data.component.html'
})

export class GeneralOrderDataComponent implements OnInit {
    public customers: string[];
    public selectedCustomer: string;
    public orderDate: string = new Date().toString();

    constructor() {
        this.customers = ["ОАО БЗА", "ЗАО БелИнвест", "ООО ПромСтрой"];
        this.selectedCustomer = "ОАО БЗА";
    }

    ngOnInit() { }
}