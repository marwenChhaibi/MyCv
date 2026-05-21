<script setup lang="ts">
import { onMounted } from 'vue'
import { usePortfolioStore } from '@/stores/portfolio'
import HeroSection from '@/sections/HeroSection.vue'
import AboutSection from '@/sections/AboutSection.vue'
import ExperienceSection from '@/sections/ExperienceSection.vue'
import ProjectsSection from '@/sections/ProjectsSection.vue'
import SkillsSection from '@/sections/SkillsSection.vue'
import ArchitectureSection from '@/sections/ArchitectureSection.vue'
import EducationSection from '@/sections/EducationSection.vue'
import ContactSection from '@/sections/ContactSection.vue'
import TechLogosSection from '@/sections/TechLogosSection.vue'

const store = usePortfolioStore()
onMounted(() => store.load())
</script>

<template>
  <main>
    <HeroSection />
    <TechLogosSection />
    <AboutSection />
    <ExperienceSection />
    <ProjectsSection />
    <SkillsSection />
    <ArchitectureSection />
    <EducationSection />
    <ContactSection />
  </main>
</template>
