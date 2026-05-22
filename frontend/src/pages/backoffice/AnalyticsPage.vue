<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import {
  portfolioApi,
  type VisitStatsDto,
  type CvDownloadStatsDto,
} from '@/api'

const stats   = ref<VisitStatsDto | null>(null)
const cvStats = ref<CvDownloadStatsDto | null>(null)
const loading = ref(true)
const error   = ref(false)

onMounted(async () => {
  try {
    const [visitsRes, cvRes] = await Promise.all([
      portfolioApi.getVisitStats(),
      portfolioApi.getCvDownloadStats(),
    ])
    stats.value   = visitsRes.data
    cvStats.value = cvRes.data
  } catch {
    error.value = true
  } finally {
    loading.value = false
  }
})

// ── bar chart helpers ─────────────────────────────────────────────────────────
const maxVisits    = computed(() => Math.max(1, ...(stats.value?.last30Days.map(d => d.count) ?? [])))
const maxDownloads = computed(() => Math.max(1, ...(cvStats.value?.last30Days.map(d => d.count) ?? [])))
const maxHour      = computed(() => Math.max(1, ...(stats.value?.visitsByHour.map(h => h.count) ?? [])))
const maxDow       = computed(() => Math.max(1, ...(stats.value?.visitsByDayOfWeek.map(d => d.count) ?? [])))

function barH(count: number, max: number, maxPx = 80) {
  return Math.max(2, Math.round((count / max) * maxPx))
}

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString('fr-FR', { day: '2-digit', month: 'short' })
}

function shortReferrer(url: string) {
  try { return new URL(url).hostname.replace('www.', '') } catch { return url }
}

// ── device icon ───────────────────────────────────────────────────────────────
const deviceIcon: Record<string, string> = { mobile: '📱', tablet: '📋', desktop: '🖥️' }
const browserIcon: Record<string, string> = {
  Chrome: '🟡', Firefox: '🟠', Safari: '🔵', Edge: '🟢', Opera: '🔴', Other: '⚪',
}
</script>

<template>
  <div>
    <h1 class="text-2xl font-display font-semibold text-white mb-8">Analytics</h1>

    <div v-if="loading" class="text-slate-500 text-sm">Loading…</div>
    <div v-else-if="error" class="text-red-400 text-sm">Failed to load stats.</div>

    <template v-else-if="stats">

      <!-- ── KPI cards ──────────────────────────────────────────────────────── -->
      <div class="grid grid-cols-2 sm:grid-cols-4 gap-4 mb-8">
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold text-white font-mono">{{ stats.total }}</div>
          <div class="text-xs text-slate-400 mt-1 uppercase tracking-wider">Total visits</div>
        </div>
        <div class="glass-card p-5 text-center">
          <div class="text-3xl font-bold font-mono" style="color:var(--color-accent)">{{ stats.thisWeek }}</div>
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

      <!-- ── Visits 30-day bar chart ────────────────────────────────────────── -->
      <div class="glass-card p-6 mb-6">
        <h2 class="section-title">Page visits — last 30 days</h2>
        <div class="flex items-end gap-[2px]" style="height:140px">
          <div
            v-for="day in stats.last30Days" :key="day.date"
            class="flex-1 flex flex-col items-center justify-end group relative"
          >
            <div class="tooltip">
              {{ day.count }} visit{{ day.count !== 1 ? 's' : '' }}
              <span class="text-slate-400 ml-1">{{ formatDate(day.date) }}</span>
              <div class="tooltip-arrow" />
            </div>
            <div class="w-full rounded-t transition-all duration-200"
              :style="{ height: barH(day.count, maxVisits, 120)+'px',
                        background: day.count>0 ? 'var(--color-accent)' : 'rgb(51 65 85)',
                        opacity: day.count>0 ? 0.85 : 0.25 }" />
          </div>
        </div>
        <div class="flex gap-[2px] mt-1">
          <div v-for="(day,i) in stats.last30Days" :key="day.date"
            class="flex-1 text-center text-slate-600 overflow-hidden" style="font-size:9px">
            {{ i%5===0 ? formatDate(day.date) : '' }}
          </div>
        </div>
      </div>

      <!-- ── Visits by hour + day-of-week ──────────────────────────────────── -->
      <div class="grid grid-cols-2 gap-6 mb-6">
        <!-- Hour of day -->
        <div class="glass-card p-6">
          <h2 class="section-title">Visits by hour of day (UTC)</h2>
          <div class="flex items-end gap-[2px]" style="height:80px">
            <div
              v-for="h in stats.visitsByHour" :key="h.hour"
              class="flex-1 flex flex-col items-center justify-end group relative"
            >
              <div class="tooltip text-xs">{{ h.count }}<br/><span class="text-slate-400">{{h.hour}}h</span><div class="tooltip-arrow"/></div>
              <div class="w-full rounded-t"
                :style="{ height: barH(h.count, maxHour)+'px',
                          background: h.count>0 ? '#38bdf8' : 'rgb(51 65 85)',
                          opacity: h.count>0 ? 0.85 : 0.2 }" />
            </div>
          </div>
          <div class="flex gap-[2px] mt-1">
            <div v-for="h in stats.visitsByHour" :key="h.hour"
              class="flex-1 text-center text-slate-600 overflow-hidden" style="font-size:8px">
              {{ h.hour%6===0 ? h.hour+'h' : '' }}
            </div>
          </div>
        </div>

        <!-- Day of week -->
        <div class="glass-card p-6">
          <h2 class="section-title">Visits by day of week</h2>
          <div class="flex items-end gap-2" style="height:80px">
            <div
              v-for="d in stats.visitsByDayOfWeek" :key="d.day"
              class="flex-1 flex flex-col items-center justify-end group relative"
            >
              <div class="tooltip text-xs">{{ d.count }}<div class="tooltip-arrow"/></div>
              <div class="w-full rounded-t"
                :style="{ height: barH(d.count, maxDow)+'px',
                          background: d.count>0 ? '#a78bfa' : 'rgb(51 65 85)',
                          opacity: d.count>0 ? 0.85 : 0.2 }" />
            </div>
          </div>
          <div class="flex gap-2 mt-1">
            <div v-for="d in stats.visitsByDayOfWeek" :key="d.day"
              class="flex-1 text-center text-slate-500 text-xs">{{ d.day }}</div>
          </div>
        </div>
      </div>

      <!-- ── Countries + Traffic sources ───────────────────────────────────── -->
      <div class="grid grid-cols-2 gap-6 mb-6">
        <div class="glass-card p-6">
          <h2 class="section-title">Visitors by country</h2>
          <div v-if="!stats.topCountries.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in stats.topCountries" :key="row.country" class="stat-row">
              <span class="stat-label">{{ row.country }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar" style="background:var(--color-accent)"
                  :style="{ width: (row.count/stats.topCountries[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>

        <div class="glass-card p-6">
          <h2 class="section-title">Traffic sources (referrer)</h2>
          <div v-if="!stats.topReferrers.length" class="empty-state">No referrer data yet — add UTM params to your shared links</div>
          <ul v-else class="space-y-2">
            <li v-for="row in stats.topReferrers" :key="row.referrer" class="stat-row">
              <span class="stat-label" :title="row.referrer">{{ shortReferrer(row.referrer) }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-green-500"
                  :style="{ width: (row.count/stats.topReferrers[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>
      </div>

      <!-- ── Languages ────────────────────────────────────────────────────── -->
      <div class="grid grid-cols-2 gap-6 mb-6">
        <div class="glass-card p-6">
          <h2 class="section-title">Visitor languages</h2>
          <div v-if="!stats.topLanguages.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in stats.topLanguages" :key="row.language" class="stat-row">
              <span class="stat-label uppercase">{{ row.language }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-pink-500"
                  :style="{ width: (row.count/stats.topLanguages[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>
      </div>

      <!-- ── Device + Browser breakdown ────────────────────────────────────── -->
      <div class="grid grid-cols-2 gap-6 mb-8">
        <div class="glass-card p-6">
          <h2 class="section-title">Device type</h2>
          <div v-if="!stats.deviceBreakdown.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in stats.deviceBreakdown" :key="row.device" class="stat-row">
              <span class="stat-label">{{ deviceIcon[row.device] ?? '?' }} {{ row.device }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-sky-500"
                  :style="{ width: (row.count/stats.deviceBreakdown[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>

        <div class="glass-card p-6">
          <h2 class="section-title">Browser</h2>
          <div v-if="!stats.browserBreakdown.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in stats.browserBreakdown" :key="row.browser" class="stat-row">
              <span class="stat-label">{{ browserIcon[row.browser] ?? '⚪' }} {{ row.browser }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-orange-500"
                  :style="{ width: (row.count/stats.browserBreakdown[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>
      </div>

      <!-- ── CV Downloads section ───────────────────────────────────────────── -->
      <h2 class="text-lg font-semibold text-slate-300 mb-4 mt-2 uppercase tracking-wider text-xs">CV Downloads</h2>

      <div v-if="cvStats" class="glass-card p-6 mb-6">
        <h2 class="section-title">Downloads — last 30 days</h2>
        <div class="flex items-end gap-[2px]" style="height:100px">
          <div
            v-for="day in cvStats.last30Days" :key="day.date"
            class="flex-1 flex flex-col items-center justify-end group relative"
          >
            <div class="tooltip">
              {{ day.count }} dl<span class="text-slate-400 ml-1">{{ formatDate(day.date) }}</span>
              <div class="tooltip-arrow"/>
            </div>
            <div class="w-full rounded-t transition-all duration-200"
              :style="{ height: barH(day.count, maxDownloads, 90)+'px',
                        background: day.count>0 ? 'rgb(192 132 252)' : 'rgb(51 65 85)',
                        opacity: day.count>0 ? 0.85 : 0.25 }" />
          </div>
        </div>
        <div class="flex gap-[2px] mt-1">
          <div v-for="(day,i) in cvStats.last30Days" :key="day.date"
            class="flex-1 text-center text-slate-600 overflow-hidden" style="font-size:9px">
            {{ i%5===0 ? formatDate(day.date) : '' }}
          </div>
        </div>
      </div>

      <div v-if="cvStats" class="grid grid-cols-2 gap-6 mb-6">
        <!-- CV by country -->
        <div class="glass-card p-6">
          <h2 class="section-title">Downloads by country</h2>
          <div v-if="!cvStats.topCountries.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in cvStats.topCountries" :key="row.country" class="stat-row">
              <span class="stat-label">{{ row.country }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-purple-500"
                  :style="{ width: (row.count/cvStats.topCountries[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>
      </div>

      <div v-if="cvStats" class="grid grid-cols-2 gap-6 mb-6">
        <!-- CV device -->
        <div class="glass-card p-6">
          <h2 class="section-title">Downloads by device</h2>
          <div v-if="!cvStats.deviceBreakdown.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in cvStats.deviceBreakdown" :key="row.device" class="stat-row">
              <span class="stat-label">{{ deviceIcon[row.device] ?? '?' }} {{ row.device }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-sky-500"
                  :style="{ width: (row.count/cvStats.deviceBreakdown[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>

        <!-- CV browser -->
        <div class="glass-card p-6">
          <h2 class="section-title">Downloads by browser</h2>
          <div v-if="!cvStats.browserBreakdown.length" class="empty-state">No data yet</div>
          <ul v-else class="space-y-2">
            <li v-for="row in cvStats.browserBreakdown" :key="row.browser" class="stat-row">
              <span class="stat-label">{{ browserIcon[row.browser] ?? '⚪' }} {{ row.browser }}</span>
              <div class="stat-bar-track">
                <div class="stat-bar bg-orange-500"
                  :style="{ width: (row.count/cvStats.browserBreakdown[0].count*100)+'%' }" />
              </div>
              <span class="stat-count">{{ row.count }}</span>
            </li>
          </ul>
        </div>
      </div>

    </template>
  </div>
</template>

<style scoped>
@reference "../../style.css";

.section-title {
  @apply text-xs font-medium text-slate-300 mb-4 uppercase tracking-wider;
}
.empty-state {
  @apply text-slate-500 text-sm;
}
.stat-row {
  @apply flex items-center gap-3 text-sm;
}
.stat-label {
  @apply text-white font-medium w-28 truncate capitalize;
}
.stat-bar-track {
  @apply flex-1 bg-slate-700 rounded-full h-2 overflow-hidden;
}
.stat-bar {
  @apply h-full rounded-full transition-all duration-300;
}
.stat-count {
  @apply text-slate-400 w-8 text-right font-mono text-xs;
}
/* tooltip shown on bar hover */
.group:hover .tooltip { display: flex; }
.tooltip {
  @apply hidden absolute bottom-full mb-1 flex-col items-center pointer-events-none z-10;
  @apply bg-slate-700 text-white text-xs rounded px-2 py-1 whitespace-nowrap;
}
.tooltip-arrow {
  @apply w-0 h-0;
  border-left: 4px solid transparent;
  border-right: 4px solid transparent;
  border-top: 4px solid rgb(51 65 85);
}
</style>

