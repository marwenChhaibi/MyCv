<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'

const { t } = useI18n()
const store = usePortfolioStore()
const p = computed(() => store.profile)
</script>

<template>
  <section id="contact" class="py-24 px-6">
    <div class="max-w-3xl mx-auto">
      <h2 class="section-title mb-2">{{ t('contact.title') }}</h2>
      <span class="accent-line mb-4"></span>
      <p class="text-slate-400 mb-12 text-lg leading-relaxed">{{ t('contact.subtitle') }}</p>

      <div v-if="p" class="grid md:grid-cols-2 gap-6 mb-12">
        <div class="glass-card p-6 flex flex-col gap-5">
          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-lg flex items-center justify-center text-lg flex-shrink-0" style="background: rgba(59,130,246,0.1); color: var(--color-accent)">
              📍
            </div>
            <div>
              <div class="text-xs text-slate-500 uppercase tracking-widest mb-0.5">Location</div>
              <div class="text-white font-medium">{{ p.location }}</div>
            </div>
          </div>

          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-lg flex items-center justify-center text-lg flex-shrink-0" style="background: rgba(59,130,246,0.1); color: var(--color-accent)">
              📞
            </div>
            <div>
              <div class="text-xs text-slate-500 uppercase tracking-widest mb-0.5">{{ t('contact.phone') }}</div>
              <a :href="`tel:${p.phone}`" class="text-white font-medium hover:text-blue-400 transition-colors font-mono">
                {{ p.phone }}
              </a>
            </div>
          </div>

          <div class="flex items-center gap-4">
            <div class="w-10 h-10 rounded-lg flex items-center justify-center text-lg flex-shrink-0" style="background: rgba(59,130,246,0.1); color: var(--color-accent)">
              ✉️
            </div>
            <div>
              <div class="text-xs text-slate-500 uppercase tracking-widest mb-0.5">{{ t('contact.email') }}</div>
              <a :href="`mailto:${p.email}`" class="text-white font-medium hover:text-blue-400 transition-colors font-mono text-sm">
                {{ p.email }}
              </a>
            </div>
          </div>
        </div>

        <div class="glass-card p-6 flex flex-col gap-4">
          <a
            v-if="p.linkedInUrl"
            :href="p.linkedInUrl"
            target="_blank"
            class="flex items-center gap-4 p-3 rounded-xl hover:bg-white/5 transition-all group"
          >
            <div class="w-10 h-10 rounded-lg bg-blue-600/20 border border-blue-600/30 flex items-center justify-center font-bold text-blue-400 text-sm flex-shrink-0">
              in
            </div>
            <div class="flex-1 min-w-0">
              <div class="text-white text-sm font-medium group-hover:text-blue-400 transition-colors">{{ t('contact.linkedin') }}</div>
              <div class="text-slate-500 text-xs truncate">linkedin.com/in/medmarwenchhibi</div>
            </div>
            <span class="text-slate-600 group-hover:text-blue-400 transition-colors">↗</span>
          </a>

          <a
            v-if="p.gitHubUrl"
            :href="p.gitHubUrl"
            target="_blank"
            class="flex items-center gap-4 p-3 rounded-xl hover:bg-white/5 transition-all group"
          >
            <div class="w-10 h-10 rounded-lg bg-slate-700/50 border border-slate-600/30 flex items-center justify-center text-slate-300 flex-shrink-0">
              <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24"><path d="M12 2C6.477 2 2 6.484 2 12.017c0 4.425 2.865 8.18 6.839 9.504.5.092.682-.217.682-.483 0-.237-.008-.868-.013-1.703-2.782.605-3.369-1.343-3.369-1.343-.454-1.158-1.11-1.466-1.11-1.466-.908-.62.069-.608.069-.608 1.003.07 1.531 1.032 1.531 1.032.892 1.53 2.341 1.088 2.91.832.092-.647.35-1.088.636-1.338-2.22-.253-4.555-1.113-4.555-4.951 0-1.093.39-1.988 1.029-2.688-.103-.253-.446-1.272.098-2.65 0 0 .84-.27 2.75 1.026A9.564 9.564 0 0112 6.844c.85.004 1.705.115 2.504.337 1.909-1.296 2.747-1.027 2.747-1.027.546 1.379.202 2.398.1 2.651.64.7 1.028 1.595 1.028 2.688 0 3.848-2.339 4.695-4.566 4.943.359.309.678.92.678 1.855 0 1.338-.012 2.419-.012 2.747 0 .268.18.58.688.482A10.019 10.019 0 0022 12.017C22 6.484 17.522 2 12 2z"/></svg>
            </div>
            <div class="flex-1 min-w-0">
              <div class="text-white text-sm font-medium group-hover:text-white transition-colors">{{ t('contact.github') }}</div>
              <div class="text-slate-500 text-xs truncate">github.com/marwenChhaibi</div>
            </div>
            <span class="text-slate-600 group-hover:text-white transition-colors">↗</span>
          </a>

          <a
            v-if="p.azureDevOpsUrl"
            :href="p.azureDevOpsUrl"
            target="_blank"
            class="flex items-center gap-4 p-3 rounded-xl hover:bg-white/5 transition-all group"
          >
            <div class="w-10 h-10 rounded-lg bg-blue-500/10 border border-blue-500/20 flex items-center justify-center text-blue-400 flex-shrink-0">
              <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 24 24"><path d="M0 6.265l2.378-.924L9.542 2.25l.005-.002 3.09 8.337-2.375.943L6.24 4.18 2.374 8.36l-.003-.002L0 6.265zm24 5.268L21.623 9.46l-7.164-3.092-.002-.001L11.37 14.7l2.374.943 3.968-6.295 3.872 4.162.002-.002 2.414-1.975zm-24 .87l2.408 1.92 7.129 3.064.002.001 3.09-8.337-2.378-.924L6.24 14.5l-3.865-4.11-2.376 1.884v-.871zm24-2.22L21.625 8.11l-3.09 8.337 2.378.924L24 10.183z"/></svg>
            </div>
            <div class="flex-1 min-w-0">
              <div class="text-white text-sm font-medium group-hover:text-blue-400 transition-colors">{{ t('contact.devops') }}</div>
              <div class="text-slate-500 text-xs truncate">dev.azure.com/Urfium</div>
            </div>
            <span class="text-slate-600 group-hover:text-blue-400 transition-colors">↗</span>
          </a>
        </div>
      </div>

      <div class="text-center">
        <a v-if="p?.email" :href="`mailto:${p.email}`" class="btn-primary inline-flex">
          {{ t('contact.cta') }}
        </a>
      </div>

      <div class="mt-16 text-center text-xs text-slate-700 font-mono">
        © {{ new Date().getFullYear() }} Marwen Chhaibi · Paris
      </div>
    </div>
  </section>
</template>
