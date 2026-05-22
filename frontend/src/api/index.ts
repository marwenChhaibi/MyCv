import axios from 'axios'
import { manager } from '@/stores/auth'

const api = axios.create({ baseURL: import.meta.env.VITE_API_URL ?? '/api' })

api.interceptors.request.use(async config => {
  const storageKey = `oidc.user:${manager.settings.authority}:${manager.settings.client_id}`
  const raw = sessionStorage.getItem(storageKey)
  const user = await manager.getUser()

  console.group(`[api] ${config.method?.toUpperCase()} ${config.url}`)
  console.log('sessionStorage key:', storageKey)
  console.log('sessionStorage raw:', raw ? JSON.parse(raw) : 'MISSING — no user in sessionStorage')
  console.log('manager.getUser():', user ? { expired: user.expired, scopes: user.scopes, hasToken: !!user.access_token } : null)
  if (user?.access_token) {
    config.headers.Authorization = `Bearer ${user.access_token}`
    console.log('Authorization header set ✓')
  } else {
    console.warn('NO TOKEN — request will hit 401')
  }
  console.groupEnd()

  return config
})

export default api

/** Returns a stable UUID for this browser, persisted in localStorage. */
export function getOrCreateFingerprint(): string {
  const key = 'mcv_fp'
  let id = localStorage.getItem(key)
  if (!id) {
    id = crypto.randomUUID()
    localStorage.setItem(key, id)
  }
  return id
}

/**
 * Builds the tracking payload (fingerprint, referrer, device, language).
 */
export function getTrackingPayload() {
  const w = window.screen.width
  const deviceType = w < 768 ? 'mobile' : w < 1024 ? 'tablet' : 'desktop'

  return {
    fingerprintId: getOrCreateFingerprint(),
    referrer:      document.referrer || null,
    deviceType,
    language:      navigator.language?.split('-')[0] || null,
  }
}

export interface ProjectDto {
  id: string
  title: string
  titleFr: string
  description: string
  descriptionFr: string
  type: 'Personal' | 'Professional'
  techStack: string[]
  liveUrl?: string
  githubUrl?: string
  azureDevOpsUrl?: string
  sortOrder: number
  isVisible: boolean
  screenshots: ScreenshotDto[]
  features: FeatureDto[]
}

export interface ScreenshotDto {
  id: string
  url: string
  caption?: string
  captionFr?: string
  sortOrder: number
}

export interface FeatureDto {
  id: string
  label: string
  labelFr: string
  sortOrder: number
}

export interface ExperienceDto {
  id: string
  company: string
  role: string
  roleFr: string
  companyUrl?: string
  location?: string
  startDate: string
  endDate?: string
  isCurrentPosition: boolean
  description: string
  descriptionFr: string
  techStack: string[]
  highlights: string[]
  highlightsFr: string[]
  sortOrder: number
}

export interface SkillDto {
  id: string
  category: string
  categoryFr: string
  name: string
  level: 'Familiar' | 'Intermediate' | 'Advanced' | 'Expert'
  sortOrder: number
}

export interface ProfileDto {
  id: string
  fullName: string
  title: string
  titleFr: string
  bio: string
  bioFr: string
  email: string
  phone: string
  location: string
  linkedInUrl: string
  gitHubUrl?: string
  azureDevOpsUrl?: string
  avatarUrl?: string
  yearsOfExperience: number
  openToWork: boolean
}

export const portfolioApi = {
  getProfile: () => api.get<ProfileDto>('/profile'),
  getExperiences: () => api.get<ExperienceDto[]>('/experiences'),
  getSkills: () => api.get<SkillDto[]>('/skills'),
  getProjects: (type?: string) => api.get<ProjectDto[]>('/projects', { params: type ? { type } : {} }),
  createProject: (data: Partial<ProjectDto>) => api.post('/projects', data),
  updateProject: (id: string, data: Partial<ProjectDto>) => api.put(`/projects/${id}`, { id, ...data }),
  deleteProject: (id: string) => api.delete(`/projects/${id}`),
  uploadScreenshot: (projectId: string, file: File, caption?: string, captionFr?: string, sortOrder = 0) => {
    const form = new FormData()
    form.append('file', file)
    if (caption) form.append('caption', caption)
    if (captionFr) form.append('captionFr', captionFr)
    form.append('sortOrder', String(sortOrder))
    return api.post(`/projects/${projectId}/screenshots`, form, { headers: { 'Content-Type': 'multipart/form-data' } })
  },
  deleteScreenshot: (screenshotId: string) => api.delete(`/projects/screenshots/${screenshotId}`),
  recordVisit: () => api.post('/visits', getTrackingPayload()).catch(() => {}),
  getVisitStats: () => api.get<VisitStatsDto>('/visits/stats'),
  recordCvDownload: () => api.post('/cv/downloads', getTrackingPayload()).catch(() => {}),
  getCvDownloadStats: () => api.get<CvDownloadStatsDto>('/cv/downloads/stats'),
}

export interface VisitDayDto { date: string; count: number }
export interface CountryCountDto { country: string; count: number }
export interface ReferrerCountDto { referrer: string; count: number }
export interface DeviceCountDto { device: string; count: number }
export interface BrowserCountDto { browser: string; count: number }
export interface LanguageCountDto { language: string; count: number }
export interface HourCountDto { hour: number; count: number }
export interface DayOfWeekCountDto { day: string; count: number }
export interface VisitStatsDto {
  total: number
  today: number
  thisWeek: number
  last30Days: VisitDayDto[]
  topCountries: CountryCountDto[]
  topReferrers: ReferrerCountDto[]
  deviceBreakdown: DeviceCountDto[]
  browserBreakdown: BrowserCountDto[]
  topLanguages: LanguageCountDto[]
  visitsByHour: HourCountDto[]
  visitsByDayOfWeek: DayOfWeekCountDto[]
}

export interface CvDownloadDayDto { date: string; count: number }
export interface CvDownloadCountryDto { country: string; count: number }
export interface CvDownloadDeviceDto { device: string; count: number }
export interface CvDownloadBrowserDto { browser: string; count: number }
export interface CvDownloadStatsDto {
  total: number
  last30Days: CvDownloadDayDto[]
  topCountries: CvDownloadCountryDto[]
  deviceBreakdown: CvDownloadDeviceDto[]
  browserBreakdown: CvDownloadBrowserDto[]
}
