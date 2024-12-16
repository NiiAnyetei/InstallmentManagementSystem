import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'humanizeBool',
  standalone: true
})
export class YesNoPipe implements PipeTransform {

  transform(value: any): any {
    return value ? 'Yes' : 'No';
  }

}
