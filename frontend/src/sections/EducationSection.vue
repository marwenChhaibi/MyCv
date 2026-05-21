<script setup lang="ts">
import { useI18n } from 'vue-i18n'
const { t } = useI18n()
</script>

<template>
  <section class="py-24 px-6 bg-slate-900/30">
    <div class="max-w-4xl mx-auto">
      <h2 class="section-title mb-2">{{ t('education.title') }}</h2>
      <span class="accent-line mb-12"></span>

      <div class="grid md:grid-cols-2 gap-6">
        <div class="glass-card p-6">
          <div class="text-3xl mb-4">🎓</div>
          <h3 class="font-display font-semibold text-white mb-1">{{ t('education.degree') }}</h3>
          <p class="text-sm text-slate-400 mb-5">{{ t('education.school') }}</p>
          <div class="border-t border-slate-700/50 pt-4">
            <h4 class="text-sm font-semibold text-white mb-1">{{ t('education.pfe') }}</h4>
            <p class="text-sm text-slate-400 leading-relaxed">{{ t('education.pfe_desc') }}</p>
          </div>
        </div>

        <div class="glass-card p-6">
          <div class="text-3xl mb-4">🌐</div>
          <h3 class="font-display font-semibold text-white mb-6">{{ t('education.languages') }}</h3>
          <div class="space-y-4">
            <div
              v-for="lang in ['french', 'arabic', 'english']"
              :key="lang"
              class="flex items-center gap-3"
            >
              <span class="w-2 h-2 rounded-full flex-shrink-0" style="background: var(--color-accent)"></span>
              <span class="text-slate-300">{{ t(`education.${lang}`) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
