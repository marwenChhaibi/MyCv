<script setup lang="ts">
import { ref, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'
import type { ProjectDto, FeatureDto } from '@/api'

const { t, locale } = useI18n()
const store = usePortfolioStore()
const filter = ref<'All' | 'Personal' | 'Professional'>('Professional')
const expanded = ref<string | null>(null)

const visible = computed(() =>
  store.projects
    .filter(p => p.isVisible)
    .filter(p => filter.value === 'All' || p.type === filter.value)
)

function getTitle(p: ProjectDto) { return locale.value === 'fr' ? p.titleFr ?? p.title : p.title }
function getDesc(p: ProjectDto) { return locale.value === 'fr' ? p.descriptionFr ?? p.description : p.description }
function getFeatureLabel(f: FeatureDto) { return locale.value === 'fr' ? f.labelFr ?? f.label : f.label }

function toggle(id: string) { expanded.value = expanded.value === id ? null : id }

const tabs = [
  { value: 'All', key: 'all' },
  { value: 'Personal', key: 'personal' },
  { value: 'Professional', key: 'professional' },
]
</script>

<template>
  <section id="projects" class="py-24 px-6">
    <div class="max-w-6xl mx-auto">
      <h2 class="section-title mb-2">{{ t('projects.title') }}</h2>
      <span class="accent-line mb-8"></span>

      <div class="flex gap-2 mb-10">
        <button
          v-for="tab in tabs"
          :key="tab.value"
          @click="filter = tab.value as any"
          class="px-4 py-2 text-sm rounded-lg border transition-all duration-200"
          :class="filter === tab.value
            ? 'border-blue-500 bg-blue-500/20 text-blue-400'
            : 'border-slate-700 text-slate-400 hover:border-slate-500 hover:text-slate-300'"
        >
          {{ t(`projects.${tab.key}`) }}
        </button>
      </div>

      <div class="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="p in visible"
          :key="p.id"
          class="glass-card p-6 flex flex-col transition-all duration-300 hover:border-blue-500/40 cursor-pointer group"
          @click="toggle(p.id)"
        >
          <div class="flex items-start justify-between mb-3">
            <h3 class="font-display font-semibold text-white group-hover:text-blue-400 transition-colors pr-2">
              {{ getTitle(p) }}
            </h3>
            <span
              class="flex-shrink-0 text-xs px-2 py-0.5 rounded-full border"
              :class="p.type === 'Personal'
                ? 'border-blue-500/30 bg-blue-500/10 text-blue-400'
                : 'border-purple-500/30 bg-purple-500/10 text-purple-400'"
            >
              {{ t(`projects.${p.type.toLowerCase()}`) }}
            </span>
          </div>

          <p class="text-sm text-slate-400 flex-1 mb-4 leading-relaxed">{{ getDesc(p) }}</p>

          <div class="flex flex-wrap gap-1.5 mb-4">
            <span v-for="tech in p.techStack.slice(0, 5)" :key="tech" class="tech-badge text-xs">{{ tech }}</span>
            <span v-if="p.techStack.length > 5" class="text-xs text-slate-600 self-center">+{{ p.techStack.length - 5 }}</span>
          </div>

          <div class="flex gap-3 mt-auto pt-4 border-t border-slate-700/50">
            <a v-if="p.liveUrl" :href="p.liveUrl" target="_blank" @click.stop
              class="text-xs text-blue-400 hover:underline flex items-center gap-1">
              ↗ {{ t('projects.live') }}
            </a>
            <a v-if="p.githubUrl" :href="p.githubUrl" target="_blank" @click.stop
              class="text-xs text-slate-400 hover:text-white flex items-center gap-1">
              ⎔ {{ t('projects.github') }}
            </a>
            <a v-if="p.azureDevOpsUrl" :href="p.azureDevOpsUrl" target="_blank" @click.stop
              class="text-xs text-slate-400 hover:text-white flex items-center gap-1">
              △ {{ t('projects.devops') }}
            </a>
            <span class="ml-auto text-xs text-slate-600 self-center transition-transform duration-200" :class="expanded === p.id ? 'rotate-180' : ''">
              ∨
            </span>
          </div>

          <transition name="expand">
            <div v-if="expanded === p.id" class="mt-4 pt-4 border-t border-slate-700/50">
              <div v-if="p.features.length">
                <h4 class="text-xs uppercase tracking-widest text-slate-500 mb-2">{{ t('projects.features') }}</h4>
                <ul class="space-y-1 mb-4">
                  <li v-for="f in p.features" :key="f.id" class="text-sm text-slate-300 flex items-start gap-2">
                    <span class="mt-1 flex-shrink-0" style="color: var(--color-accent)">›</span>
                    {{ getFeatureLabel(f) }}
                  </li>
                </ul>
              </div>
              <div v-if="p.screenshots.length" class="mt-2">
                <h4 class="text-xs uppercase tracking-widest text-slate-500 mb-2">{{ t('projects.screenshots') }}</h4>
                <div class="flex gap-2 overflow-x-auto pb-2">
                  <img
                    v-for="s in p.screenshots"
                    :key="s.id"
                    :src="s.url"
                    :alt="s.caption"
                    class="h-24 rounded-lg object-cover border border-slate-700 flex-shrink-0"
                  />
                </div>
              </div>
            </div>
          </transition>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.expand-enter-active,
.expand-leave-active {
  transition: all 0.3s ease;
  overflow: hidden;
}
.expand-enter-from,
.expand-leave-to {
  max-height: 0;
  opacity: 0;
}
.expand-enter-to,
.expand-leave-from {
  max-height: 600px;
  opacity: 1;
}
</style>
