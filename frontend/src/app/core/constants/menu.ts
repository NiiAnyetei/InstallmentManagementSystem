import { MenuItem } from '../models/menu.model';

export class Menu {
  public static pages: MenuItem[] = [
    {
      group: 'ADMIN',
      separator: false,
      items: [
        {
          icon: 'assets/icons/heroicons/outline/chart-pie.svg',
          label: 'Dashboard',
          route: '/dashboard',
        },
        {
          icon: 'assets/icons/heroicons/outline/lock-closed.svg',
          label: 'Customers',
          route: '/customers',
        },
        {
          icon: 'assets/icons/heroicons/outline/exclamation-triangle.svg',
          label: 'Installments',
          route: '/installments',
        },
        {
          icon: 'assets/icons/heroicons/outline/cube.svg',
          label: 'Payments',
          route: '/payments',
        },
        {
          icon: 'assets/icons/heroicons/outline/cube.svg',
          label: 'Bills',
          route: '/bills',
        },
      ],
    },
  ];
}
