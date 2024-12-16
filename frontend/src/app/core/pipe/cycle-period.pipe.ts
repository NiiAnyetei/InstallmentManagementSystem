import { Pipe, PipeTransform } from '@angular/core';
import { components } from '../models/models';

type CyclePeriod = components["schemas"]["CyclePeriod"]

@Pipe({
  name: 'cyclePeriod',
  standalone: true
})
export class CyclePeriodPipe implements PipeTransform {

  transform(value: CyclePeriod, args?: number): string {
    switch (value) {
      case 'Daily': return args && args > 1 ? 'Days' : 'Day';
      case 'Weekly': return args && args > 1 ? 'Weeks' : 'Week';
      case 'Monthly': return args && args > 1 ? 'Months' : 'Month';
    
      default: return 'N/A'
    }
  }

}
