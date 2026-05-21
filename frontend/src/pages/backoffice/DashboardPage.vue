<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'

const { t } = useI18n()
const auth = useAuthStore()
</script>

<template>
  <div class="min-h-screen flex">
    <aside class="w-56 bg-slate-900 border-r border-slate-800 flex flex-col p-4 flex-shrink-0">
      <div class="font-display font-bold text-white mb-8 mt-2 px-3">
        <span style="color: var(--color-accent)">M</span>yCv
        <span class="text-slate-600 font-normal text-sm ml-1">admin</span>
      </div>
      <nav class="flex flex-col gap-1 flex-1">
        <router-link
          to="/backoffice/projects"
          class="px-3 py-2 rounded-lg text-sm text-slate-400 hover:text-white hover:bg-slate-800 transition-all"
          active-class="bg-slate-800 text-white"
        >
          {{ t('backoffice.projects') }}
        </router-link>
        <router-link
          to="/backoffice/experiences"
          class="px-3 py-2 rounded-lg text-sm text-slate-400 hover:text-white hover:bg-slate-800 transition-all"
          active-class="bg-slate-800 text-white"
        >
          {{ t('backoffice.experiences') }}
        </router-link>
      </nav>
      <div class="border-t border-slate-800 pt-4">
        <a href="/" class="block px-3 py-2 text-xs text-slate-600 hover:text-slate-400 transition-colors mb-1">
          ← Portfolio
        </a>
        <button
          @click="auth.logout()"
          class="w-full text-left px-3 py-2 text-xs text-slate-600 hover:text-red-400 transition-colors"
        >
          Logout
        </button>
      </div>
    </aside>
    <main class="flex-1 p-8 overflow-auto">
      <router-view />
    </main>
  </div>
</template>
