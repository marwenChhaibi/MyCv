<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { usePortfolioStore } from '@/stores/portfolio'
import type { SkillDto } from '@/api'

const { t, locale } = useI18n()
const store = usePortfolioStore()

const levelOrder = ['Expert', 'Advanced', 'Intermediate', 'Familiar']

const levelStyle: Record<string, string> = {
  Expert: 'border-blue-500/50 bg-blue-500/10 text-blue-300',
  Advanced: 'border-indigo-500/50 bg-indigo-500/10 text-indigo-300',
  Intermediate: 'border-slate-500/50 bg-slate-500/10 text-slate-300',
  Familiar: 'border-slate-700/50 bg-slate-700/10 text-slate-500',
}

const grouped = computed(() => {
  const map = new Map<string, { cat: string; skills: SkillDto[] }>()
  for (const s of store.skills) {
    if (!map.has(s.category)) map.set(s.category, { cat: '', skills: [] })
    const g = map.get(s.category)!
    g.cat = locale.value === 'fr' ? s.categoryFr : s.category
    g.skills.push(s)
  }
  return Array.from(map.values())
})
</script>

<template>
  <section id="skills" class="py-24 px-6 bg-slate-900/30">
    <div class="max-w-6xl mx-auto">
      <h2 class="section-title mb-2">{{ t('skills.title') }}</h2>
      <span class="accent-line mb-12"></span>

      <div class="grid md:grid-cols-2 gap-6">
        <div v-for="group in grouped" :key="group.cat" class="glass-card p-6">
          <h3 class="font-display font-semibold text-white mb-4">{{ group.cat }}</h3>
          <div class="flex flex-wrap gap-2">
            <div
              v-for="skill in [...group.skills].sort((a, b) => levelOrder.indexOf(a.level) - levelOrder.indexOf(b.level))"
              :key="skill.id"
              class="flex items-center gap-1.5 px-3 py-1.5 rounded-lg border text-xs"
              :class="levelStyle[skill.level]"
            >
              <span class="font-mono">{{ skill.name }}</span>
              <span class="opacity-50">· {{ t(`skills.${skill.level.toLowerCase()}`) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
