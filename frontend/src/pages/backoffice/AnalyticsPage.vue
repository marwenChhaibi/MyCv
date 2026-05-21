<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { portfolioApi, type VisitStatsDto, type CvDownloadStatsDto } from '@/api'

const stats = ref<VisitStatsDto | null>(null)
const cvStats = ref<CvDownloadStatsDto | null>(null)
const loading = ref(true)
const error = ref(false)

onMounted(async () => {
  try {
    const [visitsRes, cvRes] = await Promise.all([
      portfolioApi.getVisitStats(),
      portfolioApi.getCvDownloadStats(),
    ])
    stats.value = visitsRes.data
    cvStats.value = cvRes.data
  } catch {
    error.value = true
  } finally {
    loading.value = false
  }
})

const maxVisits = computed(() =>
  Math.max(1, ...(stats.value?.last30Days.map(d => d.count) ?? []))
)

const maxDownloads = computed(() =>
  Math.max(1, ...(cvStats.value?.last30Days.map(d => d.count) ?? []))
)

function visitBarHeight(count: number) {
  return Math.max(2, Math.round((count / maxVisits.value) * 120))
}

function downloadBarHeight(count: number) {
  return Math.max(2, Math.round((count / maxDownloads.value) * 120))
}

function formatDate(iso: string) {
  const d = new Date(iso)
  return d.toLocaleDateString('fr-FR', { day: '2-digit', month: 'short' })
}
</script>

<template>
  <div>
    <h1 class="text-2xl font-display font-semibold text-white mb-8">Analytics</h1>

    <div v-if="loading" class="text-slate-500 text-sm">Loading…</div>
    <div v-else-if="error" class="text-red-400 text-sm">Failed to load stats.</div>

    <template v-else-if="stats">
      <!-- Stat cards -->
      <div class="grid grid-cols-4 gap-4 mb-10">
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold text-white font-mono">{{ stats.total }}</div>
          <div class="text-xs text-slate-400 mt-1 uppercase tracking-wider">Total visits</div>
        </div>
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold font-mono" style="color: var(--color-accent)">{{ stats.thisWeek }}</div>
          <div class="text-xs text-slate-400 mt-1 uppercase tracking-wider">This week</div>
        </div>
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold text-green-400 font-mono">{{ stats.today }}</div>
          <div class="text-xs text-slate-400 mt-1 uppercase tracking-wider">Today</div>
        </div>
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold text-purple-400 font-mono">{{ cvStats?.total ?? 0 }}</div>
          <div class="text-xs text-slate-400 mt-1 uppercase tracking-wider">CV downloads</div>
        </div>
      </div>

      <!-- Visits bar chart -->
      <div class="glass-card p-6 mb-6">
        <h2 class="text-sm font-medium text-slate-300 mb-6 uppercase tracking-wider">Page visits — last 30 days</h2>
        <div class="flex items-end gap-1" style="height: 140px">
          <div
            v-for="day in stats.last30Days"
            :key="day.date"
            class="flex-1 flex flex-col items-center justify-end group relative"
          >
            <div class="absolute bottom-full mb-1 hidden group-hover:flex flex-col items-center pointer-events-none z-10">
              <div class="bg-slate-700 text-white text-xs rounded px-2 py-1 whitespace-nowrap">
                {{ day.count }} visit{{ day.count !== 1 ? 's' : '' }}
                <span class="text-slate-400 ml-1">{{ formatDate(day.date) }}</span>
              </div>
              <div class="w-0 h-0 border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-slate-700" />
            </div>
            <div
              class="w-full rounded-t transition-all duration-200 group-hover:opacity-90"
              :style="{
                height: visitBarHeight(day.count) + 'px',
                background: day.count > 0 ? 'var(--color-accent)' : 'rgb(51 65 85)',
                opacity: day.count > 0 ? 0.8 : 0.3,
              }"
            />
          </div>
        </div>
        <div class="flex gap-1 mt-2">
          <div
            v-for="(day, i) in stats.last30Days"
            :key="day.date"
            class="flex-1 text-center text-slate-600 overflow-hidden"
            style="font-size: 9px"
          >
            {{ i % 5 === 0 ? formatDate(day.date) : '' }}
          </div>
        </div>
      </div>

      <!-- CV downloads bar chart -->
      <div v-if="cvStats" class="glass-card p-6">
        <h2 class="text-sm font-medium text-slate-300 mb-6 uppercase tracking-wider">CV downloads — last 30 days</h2>
        <div class="flex items-end gap-1" style="height: 140px">
          <div
            v-for="day in cvStats.last30Days"
            :key="day.date"
            class="flex-1 flex flex-col items-center justify-end group relative"
          >
            <div class="absolute bottom-full mb-1 hidden group-hover:flex flex-col items-center pointer-events-none z-10">
              <div class="bg-slate-700 text-white text-xs rounded px-2 py-1 whitespace-nowrap">
                {{ day.count }} download{{ day.count !== 1 ? 's' : '' }}
                <span class="text-slate-400 ml-1">{{ formatDate(day.date) }}</span>
              </div>
              <div class="w-0 h-0 border-l-4 border-r-4 border-t-4 border-l-transparent border-r-transparent border-t-slate-700" />
            </div>
            <div
              class="w-full rounded-t transition-all duration-200 group-hover:opacity-90"
              :style="{
                height: downloadBarHeight(day.count) + 'px',
                background: day.count > 0 ? 'rgb(192 132 252)' : 'rgb(51 65 85)',
                opacity: day.count > 0 ? 0.8 : 0.3,
              }"
            />
          </div>
        </div>
        <div class="flex gap-1 mt-2">
          <div
            v-for="(day, i) in cvStats.last30Days"
            :key="day.date"
            class="flex-1 text-center text-slate-600 overflow-hidden"
            style="font-size: 9px"
          >
            {{ i % 5 === 0 ? formatDate(day.date) : '' }}
          </div>
        </div>
      </div>
    </template>
  </div>
</template>
