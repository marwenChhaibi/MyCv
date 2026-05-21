<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const cards = [
  { key: 'clean', icon: '⬡' },
  { key: 'cqrs', icon: '⇄' },
  { key: 'ddd', icon: '◈' },
  { key: 'devops', icon: '∞' },
  { key: 'solid', icon: '◆' },
  { key: 'testing', icon: '✓' },
]
</script>

<template>
  <section class="py-24 px-6">
    <div class="max-w-6xl mx-auto">
      <h2 class="section-title mb-2">{{ t('architecture.title') }}</h2>
      <span class="accent-line mb-4"></span>
      <p class="text-slate-400 mb-12 text-lg">{{ t('architecture.subtitle') }}</p>

      <div class="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="c in cards"
          :key="c.key"
          class="glass-card p-6 hover:border-blue-500/40 transition-all duration-300 group"
        >
          <div class="text-3xl mb-4 font-mono" style="color: var(--color-accent)">{{ c.icon }}</div>
          <h3 class="font-display font-semibold text-white mb-2 group-hover:text-blue-400 transition-colors">
            {{ t(`architecture.${c.key}`) }}
          </h3>
          <p class="text-sm text-slate-400 leading-relaxed">{{ t(`architecture.${c.key}_desc`) }}</p>
        </div>
      </div>
    </div>
  </section>
</template>
