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
          icon: 'assets/icons/heroicons/outline/users.svg',
          label: 'Customers',
          route: '/customers',
        },
        {
          icon: 'assets/icons/heroicons/outline/percent-badge.svg',
          label: 'Installments',
          route: '/installments',
        },
        {
          icon: 'assets/icons/heroicons/outline/credit-card.svg',
          label: 'Bills',
          route: '/bills',
        },
        {
          icon: 'assets/icons/heroicons/outline/currency-dollar.svg',
          label: 'Payments',
          route: '/payments',
        },
      ],
    },
  ];
}
