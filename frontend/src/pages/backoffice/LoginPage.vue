<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'

const { t } = useI18n()
const auth = useAuthStore()
</script>

<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="glass-card p-10 w-full max-w-sm text-center">
      <div class="text-5xl mb-6 font-mono" style="color: var(--color-accent)">⌘</div>
      <h1 class="font-display font-bold text-2xl text-white mb-2">{{ t('backoffice.login') }}</h1>
      <p class="text-slate-400 text-sm mb-8">{{ t('backoffice.login_desc') }}</p>
      <button @click="auth.login()" class="btn-primary w-full justify-center">
        {{ t('backoffice.login') }}
      </button>
    </div>
  </div>
</template>
