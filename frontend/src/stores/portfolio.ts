import { defineStore } from 'pinia'
import { portfolioApi, type ProfileDto, type ExperienceDto, type SkillDto, type ProjectDto } from '@/api'

export const usePortfolioStore = defineStore('portfolio', {
  state: () => ({
    profile: null as ProfileDto | null,
    experiences: [] as ExperienceDto[],
    skills: [] as SkillDto[],
    projects: [] as ProjectDto[],
    loading: false,
  }),
  actions: {
    async load() {
      this.loading = true
      try {
        const [p, e, s, pr] = await Promise.all([
          portfolioApi.getProfile(),
          portfolioApi.getExperiences(),
          portfolioApi.getSkills(),
          portfolioApi.getProjects(),
        ])
        this.profile = p.data
        this.experiences = e.data
        this.skills = s.data
        this.projects = pr.data
      } finally {
        this.loading = false
      }
    },
  },
})
