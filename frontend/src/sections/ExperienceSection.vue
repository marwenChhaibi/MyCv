<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'
import type { ExperienceDto } from '@/api'
import { techIcon, companyLogo } from '@/utils/techIcon'

const { t, locale } = useI18n()
const store = usePortfolioStore()

function getRole(e: ExperienceDto) { return locale.value === 'fr' ? e.roleFr : e.role }
function getDesc(e: ExperienceDto) { return locale.value === 'fr' ? e.descriptionFr : e.description }
function getHighlights(e: ExperienceDto) {
  return locale.value === 'fr' ? e.highlightsFr : e.highlights
}

function formatDate(d?: string | null) {
  if (!d) return t('experience.present')
  const [y, m] = d.split('-')
  return new Date(+y, +m - 1).toLocaleDateString(
    locale.value === 'fr' ? 'fr-FR' : 'en-US',
    { month: 'short', year: 'numeric' }
  )
}
</script>

<template>
  <section id="experience" class="py-24 px-6 bg-slate-900/30">
    <div class="max-w-4xl mx-auto">
      <h2 class="section-title mb-2">{{ t('experience.title') }}</h2>
      <span class="accent-line mb-12"></span>

      <div class="relative pl-12">
        <div class="timeline-line" />

        <div
          v-for="(exp, idx) in store.experiences"
          :key="exp.id"
          class="relative mb-10 animate-fade-up"
          :style="`animation-delay: ${idx * 0.08}s`"
        >
          <div class="timeline-dot" style="top: 1.375rem" />

          <div class="glass-card p-6">
            <div class="flex flex-wrap items-start justify-between gap-4 mb-3">
              <div>
                <h3 class="font-display font-semibold text-xl text-white">{{ getRole(exp) }}</h3>
                <div class="flex items-center gap-2 mt-1.5">
                  <!-- company logo -->
                  <div
                    v-if="companyLogo(exp.company)"
                    class="w-7 h-7 rounded-lg flex items-center justify-center bg-white flex-shrink-0 overflow-hidden"
                  >
                    <img
                      :src="companyLogo(exp.company)!"
                      :alt="exp.company"
                      class="w-5 h-5 object-contain"
                      @error="($event.target as HTMLImageElement).closest('div')!.style.display = 'none'"
                    />
                  </div>
                  <a
                    v-if="exp.companyUrl"
                    :href="exp.companyUrl"
                    target="_blank"
                    class="font-medium hover:underline"
                    style="color: var(--color-accent)"
                  >{{ exp.company }}</a>
                  <span v-else class="text-slate-300 font-medium">{{ exp.company }}</span>
                  <span v-if="exp.location" class="text-slate-500 text-sm">· {{ exp.location }}</span>
                </div>
              </div>
              <div class="text-right flex-shrink-0">
                <div class="text-sm font-mono text-slate-400">
                  {{ formatDate(exp.startDate) }} — {{ formatDate(exp.endDate) }}
                </div>
                <div
                  v-if="exp.isCurrentPosition"
                  class="mt-1 inline-block text-xs px-2 py-0.5 rounded-full bg-green-500/10 border border-green-500/30 text-green-400"
                >
                  {{ t('experience.current') }}
                </div>
              </div>
            </div>

            <p class="text-slate-400 text-sm mb-4 leading-relaxed">{{ getDesc(exp) }}</p>

            <ul class="space-y-1 mb-4">
              <li
                v-for="(h, i) in getHighlights(exp)"
                :key="i"
                class="text-sm text-slate-300 flex items-start gap-2"
              >
                <span class="mt-1 flex-shrink-0" style="color: var(--color-accent)">›</span>
                {{ h }}
              </li>
            </ul>

            <div class="flex flex-wrap gap-2">
              <span
                v-for="tech in exp.techStack"
                :key="tech"
                class="tech-badge inline-flex items-center gap-1.5"
              >
                <img
                  v-if="techIcon(tech)"
                  :src="techIcon(tech)!"
                  :alt="tech"
                  class="w-3.5 h-3.5 object-contain flex-shrink-0"
                  @error="($event.target as HTMLImageElement).style.display = 'none'"
                />
                {{ tech }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
