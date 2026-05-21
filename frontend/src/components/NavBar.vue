<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { ref, onMounted, onUnmounted } from 'vue'

const { t, locale } = useI18n()
const scrolled = ref(false)

function onScroll() { scrolled.value = window.scrollY > 20 }
onMounted(() => window.addEventListener('scroll', onScroll))
onUnmounted(() => window.removeEventListener('scroll', onScroll))

function toggleLang() { locale.value = locale.value === 'fr' ? 'en' : 'fr' }

const sections = ['about', 'experience', 'projects', 'skills', 'contact']
</script>

<template>
  <header
    class="fixed top-0 left-0 right-0 z-50 transition-all duration-300"
    :class="scrolled ? 'glass-card rounded-none border-t-0 border-x-0' : 'bg-transparent'"
  >
    <nav class="max-w-6xl mx-auto px-6 py-4 flex items-center justify-between">
      <a href="/" class="font-bold font-display text-white tracking-tight leading-tight flex flex-col items-start">
        <span class="text-base"><span style="color: var(--color-accent)">M</span>arwen</span>
        <span class="text-sm text-slate-300 tracking-widest" style="margin-left: 0.95rem"><span style="color: var(--color-accent)">C</span>HHAIBI</span>
      </a>

      <div class="hidden md:flex items-center gap-6">
        <a
          v-for="s in sections"
          :key="s"
          :href="`#${s}`"
          class="text-sm text-slate-400 hover:text-white transition-colors duration-200"
        >
          {{ t(`nav.${s}`) }}
        </a>
        <a
          href="/backoffice"
          class="text-sm text-slate-600 hover:text-slate-400 transition-colors"
        >
          {{ t('nav.backoffice') }}
        </a>
      </div>

      <button
        @click="toggleLang"
        class="text-xs font-mono px-3 py-1.5 rounded-lg border border-slate-700 text-slate-400 hover:border-slate-500 hover:text-white transition-all"
      >
        {{ locale === 'fr' ? 'EN' : 'FR' }}
      </button>
    </nav>
  </header>
</template>
