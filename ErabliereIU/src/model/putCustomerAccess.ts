export class PutCustomerAccess {
    idErabliere: any;
    customerErablieres: CustomerErabliere[] = [new CustomerErabliere()];
}

export class CustomerErabliere {
    action?: number;
    idCustomer: any;
    access?: number;
}