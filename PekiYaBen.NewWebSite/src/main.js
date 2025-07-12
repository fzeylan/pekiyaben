import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Toast from 'vue-toastification'
import router from './router'
import App from './App.vue'
import { useAuthStore } from './stores/auth'

// Import CSS
import 'bootstrap/dist/css/bootstrap.min.css'
import 'vue-toastification/dist/index.css'
import './assets/css/main.css'

// Create Vue app
const app = createApp(App)

// Create Pinia store
const pinia = createPinia()
app.use(pinia)

// Initialize auth store and check for existing token
const authStore = useAuthStore()
authStore.initializeAuth()

// Setup router
app.use(router)

// Setup toast notifications
app.use(Toast, {
  position: 'top-right',
  timeout: 5000,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  showCloseButtonOnHover: true,
  hideProgressBar: false,
  icon: true,
  rtl: false,
  transition: 'Vue-Toastification__bounce',
  maxToasts: 5,
  newestOnTop: true
})

// Global error handler
app.config.errorHandler = (error, instance, info) => {
  console.error('Global error:', error)
  console.error('Error info:', info)
}

// Mount app
app.mount('#app')

// Global event listeners
window.addEventListener('unhandledrejection', event => {
  console.error('Unhandled promise rejection:', event.reason)
})

window.addEventListener('error', event => {
  console.error('Global error:', event.error)
})