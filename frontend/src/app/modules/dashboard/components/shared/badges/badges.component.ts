import { Component, Input } from '@angular/core';

type BadgeType = 'success' | 'warning' | 'danger' | 'error';

@Component({
  selector: 'app-badges',
  standalone: true,
  imports: [],
  templateUrl: './badges.component.html',
  styleUrl: './badges.component.scss'
})
export class BadgesComponent {
  @Input({ required: true }) status!: string;
  @Input({ required: false }) badgeType!: BadgeType;
}
