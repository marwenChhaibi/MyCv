<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'

const { t, locale } = useI18n()
const store = usePortfolioStore()
const profile = computed(() => store.profile)
const bio = computed(() =>
  locale.value === 'fr' ? profile.value?.bioFr : profile.value?.bio
)

const stats = computed(() => [
  { value: profile.value?.yearsOfExperience ?? 7, label: t('about.stats_years') },
  { value: 4, label: t('about.stats_companies') },
  { value: 7, label: t('about.stats_projects') },
  { value: 20, label: t('about.stats_stacks') },
])
</script>

<template>
  <section id="about" class="py-24 px-6">
    <div class="max-w-6xl mx-auto">
      <h2 class="section-title mb-2">{{ t('about.title') }}</h2>
      <span class="accent-line mb-12"></span>

      <div class="grid md:grid-cols-2 gap-12 items-center">
        <div>
          <p class="text-slate-300 text-lg leading-relaxed">{{ bio }}</p>
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div v-for="s in stats" :key="s.label" class="glass-card p-6 text-center">
            <div class="font-display font-bold text-4xl mb-2" style="color: var(--color-accent)">
              {{ s.value }}+
            </div>
            <div class="text-sm text-slate-400">{{ s.label }}</div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
