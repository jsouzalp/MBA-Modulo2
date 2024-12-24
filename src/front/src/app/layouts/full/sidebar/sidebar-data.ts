import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Dashboard',
  },
  {
    displayName: 'Dashboard',
    iconName: 'material-symbols-light:dashboard-outline',
    route: '/dashboard',
  },
  {
    navCap: 'Categorias',
    divider: true
  },
  {
    displayName: 'Manutenção',
    iconName: 'material-symbols-light:engineering-outline',
    route: '/dashboard-',
  },
  {
    navCap: 'Planejamento de gastos',
    divider: true
  },
  {
    displayName: 'Por categoria',
    iconName: 'fluent:money-hand-16-regular',
    route: '/dashboard-',
  },
  {
    displayName: 'Geral',
    iconName: 'solar:money-bag-linear',
    route: '/dashboard-',
  },
  {
    navCap: 'Transações',
    divider: true
  },
  {
    displayName: 'Registro',
    iconName: 'tabler:transaction-dollar',
    route: '/dashboard-',
  },
  {
    displayName: 'Consulta',
    iconName: 'icon-park-outline:transaction-order',
    route: '/dashboard-',
  },
  {
    navCap: 'Relatórios:',
    divider: true
  },
  {
    displayName: 'Gastos por período',
    iconName: 'material-symbols-light:date-range-outline',
    route: '/dashboard-',
  },
  {
    displayName: 'xxxx',
    iconName: 'ix:ingestion-report',
    route: '/dashboard-',
  },
  {
    navCap: 'Ui Components - será excluído',
    divider: true
  },
  {
    displayName: 'Badge',
    iconName: 'solar:archive-minimalistic-line-duotone',
    route: '/ui-components/badge',
  },
  {
    displayName: 'Chips',
    iconName: 'solar:danger-circle-line-duotone',
    route: '/ui-components/chips',
  },
  {
    displayName: 'Lists',
    iconName: 'solar:bookmark-square-minimalistic-line-duotone',
    route: '/ui-components/lists',
  },
  {
    displayName: 'Menu',
    iconName: 'solar:file-text-line-duotone',
    route: '/ui-components/menu',
  },
  {
    displayName: 'Tooltips',
    iconName: 'solar:text-field-focus-line-duotone',
    route: '/ui-components/tooltips',
  },
  {
    displayName: 'Forms',
    iconName: 'solar:file-text-line-duotone',
    route: '/ui-components/forms',
  },
  {
    displayName: 'Tables',
    iconName: 'solar:tablet-line-duotone',
    route: '/ui-components/tables',
  },
  {
    navCap: 'Auth',
    divider: true
  },
  {
    displayName: 'Login',
    iconName: 'solar:login-3-line-duotone',
    route: '/authentication/login',
  },
  {
    displayName: 'Register',
    iconName: 'solar:user-plus-rounded-line-duotone',
    route: '/authentication/register',
  },
  {
    navCap: 'Extra',
    divider: true
  },
  {
    displayName: 'Icons',
    iconName: 'solar:sticker-smile-circle-2-line-duotone',
    route: '/extra/icons',
  },
  {
    displayName: 'Sample Page',
    iconName: 'solar:planet-3-line-duotone',
    route: '/extra/sample-page',
  },
];
