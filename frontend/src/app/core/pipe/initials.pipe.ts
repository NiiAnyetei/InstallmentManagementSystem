import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'initials',
  standalone: true
})
export class InitialsPipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    if(!value) return 'U';
    
    let initials: string[] = [];
    const names = value.split(' ');
    names.forEach(name => {
      const initial = name.slice(0, 1).toUpperCase();
      initials.push(initial);
    });

    return initials.join('');
  }

}
