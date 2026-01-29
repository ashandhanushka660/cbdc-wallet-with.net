const routes = [
  {
    path: '/',
    component: () => import('src/layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/IndexPage.vue') },
      { path: 'team', component: () => import('src/pages/TeamPage.vue') },
    ],
  },

  {
    path: '/auth',
    component: () => import('src/layouts/MainLayout.vue'),
    children: [
      { path: 'login', component: () => import('src/pages/auth/LoginPage.vue') },
      { path: 'register', component: () => import('src/pages/auth/RegisterPage.vue') },
    ],
  },

  {
    path: '/dashboard',
    component: () => import('src/layouts/DashboardLayout.vue'),
    children: [
      { path: '', component: () => import('src/pages/dashboard/DashboardPage.vue') },
      { path: 'transactions', component: () => import('src/pages/dashboard/TransactionsPage.vue') },
      { path: 'cards', component: () => import('src/pages/dashboard/CardsPage.vue') },
      { path: 'loans', component: () => import('src/pages/dashboard/LoansPage.vue') },
      { path: 'services', component: () => import('src/pages/dashboard/ServicesPage.vue') },
      { path: 'settings', component: () => import('src/pages/dashboard/SettingsPage.vue') },
      { path: 'admin', component: () => import('src/pages/admin/AdminPanel.vue') },
    ],
  },

  {
    path: '/:catchAll(.*)*',
    component: () => import('src/pages/ErrorNotFound.vue'),
  },
]

export default routes
