import { defineStore } from 'pinia'
import { UserManager, type User } from 'oidc-client-ts'

export const manager = new UserManager({
  authority: import.meta.env.VITE_IDENTITY_URL ?? 'http://localhost:8085',
  client_id: 'mycv',
  redirect_uri: `${window.location.origin}/backoffice/callback`,
  post_logout_redirect_uri: `${window.location.origin}/`,
  scope: 'openid profile email',
  response_type: 'code',
})

export const useAuthStore = defineStore('auth', {
  state: () => ({ user: null as User | null, initialized: false }),
  getters: {
    isAuthenticated: (s) => !!s.user && !s.user.expired,
    token: (s) => s.user?.access_token ?? null,
  },
  actions: {
    async init() {
      if (this.initialized) return
      this.user = await manager.getUser()
      this.initialized = true
    },
    login() {
      return manager.signinRedirect()
    },
    async handleCallback() {
      this.user = await manager.signinRedirectCallback()
    },
    async logout() {
      await manager.signoutRedirect()
    },
  },
})
