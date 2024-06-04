import {CustomerAccess} from "./customerAccess";

export class Customer {
    id?: any
    name?: string
    uniqueName?: string
    email?: string
    customerErablieres?: Array<CustomerAccess>
}
