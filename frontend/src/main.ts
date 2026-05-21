import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { createI18n } from 'vue-i18n'
import router from './router'
import App from './App.vue'
import fr from './i18n/fr'
import en from './i18n/en'
import './style.css'

const i18n = createI18n({
  legacy: false,
  locale: 'fr',
  fallbackLocale: 'en',
  messages: { fr, en },
})

createApp(App).use(createPinia()).use(router).use(i18n).mount('#app')
