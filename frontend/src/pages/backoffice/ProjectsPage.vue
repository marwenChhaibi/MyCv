<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { portfolioApi, type ProjectDto } from '@/api'

const { t } = useI18n()
const projects = ref<ProjectDto[]>([])
const loading = ref(false)
const saving = ref(false)
const editTarget = ref<Partial<ProjectDto> | null>(null)

async function load() {
  loading.value = true
  projects.value = (await portfolioApi.getProjects()).data
  loading.value = false
}

onMounted(load)

function startCreate() {
  editTarget.value = {
    title: '', titleFr: '', description: '', descriptionFr: '',
    type: 'Personal', techStack: [], isVisible: true, sortOrder: 0,
  }
}

function startEdit(p: ProjectDto) {
  editTarget.value = { ...p, techStack: [...p.techStack] }
}

async function save() {
  if (!editTarget.value) return
  saving.value = true
  try {
    if (editTarget.value.id) {
      await portfolioApi.updateProject(editTarget.value.id, editTarget.value)
    } else {
      await portfolioApi.createProject(editTarget.value)
    }
    editTarget.value = null
    await load()
  } finally {
    saving.value = false
  }
}

async function remove(id: string) {
  if (!confirm(t('backoffice.confirm_delete'))) return
  await portfolioApi.deleteProject(id)
  await load()
}

async function onScreenshotFile(projectId: string, e: Event) {
  const file = (e.target as HTMLInputElement).files?.[0]
  if (!file) return
  await portfolioApi.uploadScreenshot(projectId, file)
  await load()
}

async function removeScreenshot(id: string) {
  await portfolioApi.deleteScreenshot(id)
  await load()
}

function getTechStackStr() { return editTarget.value?.techStack?.join(', ') ?? '' }
function setTechStack(e: Event) {
  if (editTarget.value) {
    editTarget.value.techStack = (e.target as HTMLInputElement).value
      .split(',').map(s => s.trim()).filter(Boolean)
  }
}
</script>

<template>
  <div>
    <div class="flex items-center justify-between mb-6">
      <h2 class="font-display font-bold text-2xl text-white">{{ t('backoffice.projects') }}</h2>
      <button @click="startCreate" class="btn-primary text-sm">
        + {{ t('backoffice.add') }}
      </button>
    </div>

    <div v-if="loading" class="text-slate-500 font-mono text-sm">loading...</div>

    <div v-else class="space-y-4">
      <div v-for="p in projects" :key="p.id" class="glass-card p-5">
        <div class="flex items-start gap-4">
          <div class="flex-1 min-w-0">
            <div class="flex items-center gap-2 mb-1">
              <h3 class="font-semibold text-white truncate">{{ p.title }}</h3>
              <span class="text-slate-600 text-sm">/ {{ p.titleFr }}</span>
              <span
                class="flex-shrink-0 text-xs px-2 py-0.5 rounded-full border"
                :class="p.type === 'Personal'
                  ? 'border-blue-500/30 bg-blue-500/10 text-blue-400'
                  : 'border-purple-500/30 bg-purple-500/10 text-purple-400'"
              >{{ p.type }}</span>
              <span v-if="!p.isVisible" class="text-xs text-slate-600 border border-slate-700 px-2 py-0.5 rounded-full">hidden</span>
            </div>
            <p class="text-sm text-slate-400 mb-3 leading-relaxed">{{ p.description }}</p>
            <div class="flex gap-1.5 flex-wrap mb-3">
              <span v-for="tech in p.techStack" :key="tech" class="tech-badge">{{ tech }}</span>
            </div>
            <div v-if="p.screenshots.length" class="flex gap-2 flex-wrap">
              <div v-for="s in p.screenshots" :key="s.id" class="relative group/img">
                <img :src="s.url" class="h-16 rounded object-cover border border-slate-700" />
                <button
                  @click="removeScreenshot(s.id)"
                  class="absolute top-1 right-1 w-5 h-5 rounded-full bg-red-500/80 text-white text-xs hidden group-hover/img:flex items-center justify-center"
                >×</button>
              </div>
            </div>
          </div>
          <div class="flex flex-col gap-2 flex-shrink-0">
            <button @click="startEdit(p)" class="btn-outline text-xs py-1 px-3">
              {{ t('backoffice.edit') }}
            </button>
            <button
              @click="remove(p.id)"
              class="text-xs px-3 py-1 rounded-lg border border-red-500/40 text-red-400 hover:bg-red-500/10 transition-all"
            >
              {{ t('backoffice.delete') }}
            </button>
            <label class="text-xs px-3 py-1.5 rounded-lg border border-slate-600 text-slate-400 hover:text-white cursor-pointer text-center transition-colors">
              {{ t('backoffice.upload_screenshot') }}
              <input type="file" accept="image/*" class="hidden" @change="e => onScreenshotFile(p.id, e)" />
            </label>
          </div>
        </div>
      </div>
    </div>

    <div v-if="editTarget" class="fixed inset-0 bg-black/70 backdrop-blur-sm flex items-center justify-center z-50 p-4">
      <div class="glass-card p-6 w-full max-w-lg max-h-[90vh] overflow-y-auto">
        <h3 class="font-display font-bold text-white mb-5">
          {{ editTarget.id ? t('backoffice.edit') : t('backoffice.add') }}
        </h3>

        <div class="space-y-3">
          <div class="grid grid-cols-2 gap-3">
            <input v-model="editTarget.title" placeholder="Title (EN)" class="bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm w-full focus:border-blue-500 outline-none" />
            <input v-model="editTarget.titleFr" placeholder="Titre (FR)" class="bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm w-full focus:border-blue-500 outline-none" />
          </div>
          <textarea v-model="editTarget.description" placeholder="Description (EN)" rows="3" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none resize-none" />
          <textarea v-model="editTarget.descriptionFr" placeholder="Description (FR)" rows="3" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none resize-none" />
          <select v-model="editTarget.type" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none">
            <option>Personal</option>
            <option>Professional</option>
          </select>
          <input
            :value="getTechStackStr()"
            @input="setTechStack"
            placeholder="Tech stack (comma separated)"
            class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm font-mono focus:border-blue-500 outline-none"
          />
          <input v-model="editTarget.liveUrl" placeholder="Live URL" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none" />
          <input v-model="editTarget.githubUrl" placeholder="GitHub URL" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none" />
          <input v-model="editTarget.azureDevOpsUrl" placeholder="Azure DevOps URL" class="w-full bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none" />
          <div class="flex items-center gap-2 pt-1">
            <input type="number" v-model.number="editTarget.sortOrder" class="w-24 bg-slate-800 border border-slate-700 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none" placeholder="Order" />
            <label class="flex items-center gap-2 text-sm text-slate-300 cursor-pointer">
              <input type="checkbox" v-model="editTarget.isVisible" class="accent-blue-500" />
              Visible
            </label>
          </div>
        </div>

        <div class="flex gap-3 mt-6">
          <button @click="save" :disabled="saving" class="btn-primary flex-1 justify-center">
            {{ saving ? '...' : t('backoffice.save') }}
          </button>
          <button @click="editTarget = null" class="btn-outline flex-1 justify-center">
            {{ t('backoffice.cancel') }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
