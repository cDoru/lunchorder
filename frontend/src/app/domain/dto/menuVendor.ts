import { MenuVendorAddress } from './menuVendorAddress'
import { MenuVendorClosingDateRange } from './menuVendorClosingDateRange';
import * as moment from 'moment';

export class MenuVendor implements app.domain.dto.IMenuVendor, Serializable<MenuVendor> {
    id: string;
    name: string;
    address: MenuVendorAddress;
    website: string;
    submitOrderTime: Date;
    logo: string;
    closingDateRanges: MenuVendorClosingDateRange[];

    isClosingDate(): boolean {
        var today = new Date();
        for (var closingDateRange of this.closingDateRanges) {
            var fromDate = new Date(closingDateRange.from);
            var untilDate = new Date(closingDateRange.until);

            if (today >= fromDate && today <= untilDate){
                return true;
            }
        }
        return false;
    }

    deserialize(input: any): MenuVendor {
        if (!input) { return; }
        this.id = input.id;
        this.name = input.name;
        this.address = new MenuVendorAddress().deserialize(input.address);
        this.website = input.website;

        var momentUtc = moment.utc(input.submitOrderTime).toDate();
        var local = moment(momentUtc).local().toDate();
        this.submitOrderTime = local;
        this.logo = input.logo;

        this.closingDateRanges = new Array<MenuVendorClosingDateRange>();
        if (input.closingDateRanges) {
            for (var closingDateRange of input.closingDateRanges) {
                this.closingDateRanges.push(closingDateRange);
            }
        }

        return this;
    }
}
