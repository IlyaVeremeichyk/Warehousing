import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'order-products',
    templateUrl: 'order-products.component.html'
})

export class OrderProductsComponent implements OnInit {
    public categories: string[];
    public selectedCategory: string;
    public selectedProduct: string;
    public selectedCount: number;

    public products: any;

    public addedProducts: any[] = [];

    constructor() {
        this.categories = ["Велосипеды", "Мячи", "Мебель"];
        this.products = {
            "Велосипеды": ["Aist", "Stels", "Hornet", "Marin"],
            "Мячи": ["Баскетбольный", "Футбольный", "Для плавания", "Для гольфа"],
            "Мебель": ["Стул", "Стол", "Диван", "Кровать", "Шкаф"]
        }
        this.selectedCategory = "Велосипеды";
        this.selectedProduct = "Aist";
        this.selectedCount = 0;
    }

    ngOnInit() { }

    addProduct() {    
        this.addedProducts.push({
            category : this.selectedCategory,
            product : this.selectedProduct,
            count : +this.selectedCount
        });

        this.selectedCategory = "Велосипеды";
        this.selectedProduct = "Aist";
        this.selectedCount = 0;
    }

    clean(){
        this.addedProducts = [];
    }
}