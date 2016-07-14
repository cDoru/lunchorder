import { UserOrderHistoryEntry } from './userOrderHistoryEntry'
import { UserOrderHistoryRule } from './userOrderHistoryRule'

export class MenuVendorAddress implements app.domain.dto.IMenuVendorAddress {
    id: string;
    finalPrice: number;
    entry: UserOrderHistoryEntry;
    orderTime: Date;
    rules: UserOrderHistoryRule[];
    street: string;
    streetNumber: string;
    city: string;
    phone: string;
    email: string;
}