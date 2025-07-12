// src/main.js
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import router from './router'
import App from './App.vue'

// Import Bootstrap CSS and JS
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'

// Import Font Awesome
import '@fortawesome/fontawesome-free/css/all.min.css'

// Import Toast notifications
import Toast, { POSITION } from 'vue-toastification'
import 'vue-toastification/dist/index.css'

// Import dayjs
import dayjs from 'dayjs'
import 'dayjs/locale/tr'
import relativeTime from 'dayjs/plugin/relativeTime'
import customParseFormat from 'dayjs/plugin/customParseFormat'

// Configure dayjs
dayjs.locale('tr')
dayjs.extend(relativeTime)
dayjs.extend(customParseFormat)

// Create Vue app
const app = createApp(App)

// Create Pinia store
const pinia = createPinia()

// Configure Toast
const toastOptions = {
    position: POSITION.TOP_RIGHT,
    timeout: 5000,
    closeOnClick: true,
    pauseOnFocusLoss: true,
    pauseOnHover: true,
    draggable: true,
    draggablePercent: 0.6,
    showCloseButtonOnHover: false,
    hideProgressBar: false,
    closeButton: "button",
    icon: true,
    rtl: false,
    maxToasts: 5,
    newestOnTop: true
}

// Use plugins
app.use(pinia)
app.use(router)
app.use(Toast, toastOptions)

// Global error handler
app.config.errorHandler = (err, instance, info) => {
    console.error('Global error:', err)
    console.error('Vue instance:', instance)
    console.error('Error info:', info)

    // You could send this to an error reporting service
    // errorReportingService.captureException(err, { extra: { info } })
}

// Global warning handler
app.config.warnHandler = (msg, instance, trace) => {
    console.warn('Vue warning:', msg)
    console.warn('Component trace:', trace)
}

// Global properties
app.config.globalProperties.$dayjs = dayjs

// Initialize and mount app
const initApp = async () => {
    try {
        // Initialize stores
        const { useAuthStore } = await import('@/stores/auth')
        const { useAppStore } = await import('@/stores/app')

        const authStore = useAuthStore()
        const appStore = useAppStore()

        // Initialize app store
        appStore.initializeApp()

        // Initialize auth if token exists
        if (authStore.token) {
            try {
                await authStore.initializeAuth()
            } catch (error) {
                console.warn('Auth initialization failed:', error)
                // Clear invalid token
                await authStore.logout()
            }
        }

        // Mount the app
        app.mount('#app')

        console.log('PekiYaBen Vue.js App initialized successfully')

    } catch (error) {
        console.error('Failed to initialize app:', error)

        // Mount app anyway with error state
        app.mount('#app')
    }
}

// Start the application
initApp()

export default app