<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { portfolioApi, type ExperienceDto } from '@/api'

const { t, locale } = useI18n()
const experiences = ref<ExperienceDto[]>([])
const loading = ref(false)

async function load() {
  loading.value = true
  experiences.value = (await portfolioApi.getExperiences()).data
  loading.value = false
}

onMounted(load)

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
  <div>
    <h2 class="font-display font-bold text-2xl text-white mb-6">{{ t('backoffice.experiences') }}</h2>

    <div v-if="loading" class="text-slate-500 font-mono text-sm">loading...</div>

    <div v-else class="space-y-4">
      <div v-for="e in experiences" :key="e.id" class="glass-card p-5">
        <div class="flex items-start justify-between gap-4">
          <div class="flex-1">
            <div class="flex items-center gap-2 mb-1">
              <h3 class="font-semibold text-white">{{ e.role }}</h3>
              <span v-if="e.isCurrentPosition" class="text-xs px-2 py-0.5 rounded-full bg-green-500/10 border border-green-500/30 text-green-400">
                {{ t('experience.current') }}
              </span>
            </div>
            <p class="text-sm font-medium mb-1" style="color: var(--color-accent)">{{ e.company }}</p>
            <p class="text-xs text-slate-500 font-mono mb-3">
              {{ formatDate(e.startDate) }} → {{ formatDate(e.endDate) }}
              <span v-if="e.location" class="ml-2">· {{ e.location }}</span>
            </p>
            <p class="text-sm text-slate-400 mb-3 leading-relaxed">{{ e.description }}</p>
            <ul class="space-y-1 mb-3">
              <li v-for="(h, i) in e.highlights" :key="i" class="text-xs text-slate-400 flex items-start gap-2">
                <span class="mt-0.5 flex-shrink-0" style="color: var(--color-accent)">›</span>
                {{ h }}
              </li>
            </ul>
            <div class="flex gap-1.5 flex-wrap">
              <span v-for="tech in e.techStack" :key="tech" class="tech-badge">{{ tech }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>

    <p class="text-xs text-slate-600 mt-6 font-mono">
      Experiences are managed via the API seeder. Use PUT /api/experiences/{id} for edits.
    </p>
  </div>
</template>
