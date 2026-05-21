<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'

const { t, locale } = useI18n()
const store = usePortfolioStore()
const profile = computed(() => store.profile)
const displayTitle = computed(() =>
  locale.value === 'fr' ? profile.value?.titleFr : profile.value?.title
)
</script>

<template>
  <section class="relative min-h-screen flex items-center justify-center overflow-hidden">
    <div class="absolute inset-0 bg-[linear-gradient(rgba(59,130,246,0.03)_1px,transparent_1px),linear-gradient(90deg,rgba(59,130,246,0.03)_1px,transparent_1px)] bg-[size:4rem_4rem]" />
    <div class="absolute top-1/4 left-1/4 w-96 h-96 bg-blue-500/5 rounded-full blur-3xl pointer-events-none" />
    <div class="absolute bottom-1/4 right-1/4 w-64 h-64 bg-indigo-500/5 rounded-full blur-3xl pointer-events-none" />

    <div class="relative z-10 max-w-4xl mx-auto px-6 text-center animate-fade-up">
      <div
        v-if="profile?.openToWork"
        class="inline-flex items-center gap-2 px-4 py-2 rounded-full border border-green-500/30 bg-green-500/10 text-green-400 text-sm mb-8 animate-glow"
      >
        <span class="w-2 h-2 rounded-full bg-green-400"></span>
        {{ t('hero.open_to_work') }}
      </div>

      <p class="text-slate-400 text-lg mb-2 font-mono">{{ t('hero.greeting') }}</p>
      <h1 class="font-display font-bold text-5xl md:text-7xl text-white mb-6 leading-tight">
        {{ profile?.fullName ?? 'Marwen Chhaibi' }}
      </h1>
      <p class="text-xl md:text-2xl text-slate-400 mb-14 max-w-2xl mx-auto leading-relaxed">
        {{ displayTitle ?? '...' }}
      </p>

      <div class="flex flex-wrap items-center justify-center gap-4">
        <a href="#contact" class="btn-primary">{{ t('hero.cta_contact') }}</a>
        <a href="#experience" class="btn-outline">{{ t('hero.cta_projects') }}</a>
      </div>
    </div>

    <div class="absolute bottom-8 left-1/2 -translate-x-1/2 animate-bounce opacity-40">
      <svg class="w-5 h-5 text-slate-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
      </svg>
    </div>
  </section>
</template>
