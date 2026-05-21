<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const router = useRouter()

onMounted(async () => {
  try {
    await auth.handleCallback()
    await router.push('/backoffice/projects')
  } catch {
    await router.push('/backoffice/login')
  }
})
</script>

<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="text-slate-400 font-mono animate-pulse">authenticating...</div>
  </div>
</template>
