import { Pipe, PipeTransform } from '@angular/core';
import { PaymentChannel } from '../models/custom.model';

@Pipe({
  name: 'paymentChannel',
  standalone: true
})
export class PaymentChannelPipe implements PipeTransform {

  transform(value: string): string {
    switch (value) {
      case 'mtn': return 'MTN';
      case 'atl': return 'AirtelTigo';
      case 'vod': return 'Vodafone/Telecel';
    
      default: return 'N/A'
    }
  }

}
