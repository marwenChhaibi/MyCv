import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(),
  scrollBehavior: (_to, _from, saved) => saved ?? { top: 0 },
  routes: [
    { path: '/', component: () => import('@/pages/HomePage.vue') },
    { path: '/backoffice/login', component: () => import('@/pages/backoffice/LoginPage.vue') },
    { path: '/backoffice/callback', component: () => import('@/pages/backoffice/CallbackPage.vue') },
    {
      path: '/backoffice',
      component: () => import('@/pages/backoffice/DashboardPage.vue'),
      meta: { requiresAuth: true },
      children: [
        { path: '', redirect: '/backoffice/projects' },
        { path: 'projects', component: () => import('@/pages/backoffice/ProjectsPage.vue') },
        { path: 'experiences', component: () => import('@/pages/backoffice/ExperiencesPage.vue') },
      ],
    },
  ],
})

router.beforeEach(async (to) => {
  if (to.meta.requiresAuth) {
    const auth = useAuthStore()
    await auth.init()
    if (!auth.isAuthenticated) return '/backoffice/login'
  }
})

export default router
