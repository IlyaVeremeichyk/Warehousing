import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'order-transport',
    templateUrl: 'order-transport.component.html'
})

export class OrderTransportComponent implements OnInit {
    public categories: string[];
    public selectedCategory: string;
    public selectedTransport: string;
    public selectedCount: number;
    public selectedDriver : string;
    public transports: any;

    public addedTransports: any[] = [];

    constructor() {
        this.categories = ["Категория B", "Категория C", "Категория E"];
        this.transports = {
            "Категория B": ["BMW", "Opel", "Honda", "Reno"],
            "Категория C": ["Mann", "Маз", "Камаз", "Reno"],
            "Категория E": ["Mann", "Маз", "Камаз", "Reno"]
        }
        this.selectedCategory = "Категория B";
        this.selectedTransport = "BMW";
        this.selectedCount = 0;
        this.selectedDriver = "";
     }

    ngOnInit() { }

    addTransport(){
        this.addedTransports.push({
            category : this.selectedCategory,
            transport : this.selectedTransport,
            count : +this.selectedCount,
            driver : this.selectedDriver
        });
        this.selectedCategory = "Категория B";
        this.selectedTransport = "BMW";
        this.selectedCount = 0;
        this.selectedDriver = "";
    }
    
    clean(){
        this.addedTransports = [];
    }
}