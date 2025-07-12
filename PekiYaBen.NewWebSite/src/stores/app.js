// src/stores/app.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAppStore = defineStore('app', () => {
    // State
    const isLoading = ref(false)
    const loadingMessage = ref('')
    const notifications = ref([])
    const sidebarOpen = ref(false)
    const theme = ref(localStorage.getItem('theme') || 'light')
    const language = ref(localStorage.getItem('language') || 'tr')
    const pageTitle = ref('PekiYaBen')
    const breadcrumbs = ref([])
    const errors = ref([])
    const searchQuery = ref('')
    const searchResults = ref([])
    const isSearching = ref(false)

    // UI State
    const modalStack = ref([])
    const activeModal = ref(null)
    const alerts = ref([])
    const toasts = ref([])

    // App Configuration
    const appConfig = ref({
        siteName: 'PekiYaBen',
        version: '1.0.0',
        environment: import.meta.env.MODE || 'development',
        apiUrl: import.meta.env.VITE_API_BASE_URL || '/api',
        features: {
            darkMode: true,
            notifications: true,
            search: true,
            multiLanguage: false
        }
    })

    // Network State
    const isOnline = ref(navigator.onLine)
    const networkError = ref(false)

    // Getters
    const isDarkMode = computed(() => theme.value === 'dark')
    const hasNotifications = computed(() => notifications.value.length > 0)
    const unreadNotificationCount = computed(() =>
        notifications.value.filter(n => !n.read).length
    )
    const hasErrors = computed(() => errors.value.length > 0)
    const currentLanguageLabel = computed(() => {
        const languages = {
            'tr': 'Türkçe',
            'en': 'English'
        }
        return languages[language.value] || 'Türkçe'
    })

    // Actions
    const setLoading = (loading, message = '') => {
        isLoading.value = loading
        loadingMessage.value = message
    }

    const setPageTitle = (title) => {
        pageTitle.value = title
        document.title = title ? `${title} - ${appConfig.value.siteName}` : appConfig.value.siteName
    }

    const setBreadcrumbs = (items) => {
        breadcrumbs.value = items
    }

    const addBreadcrumb = (item) => {
        breadcrumbs.value.push(item)
    }

    const clearBreadcrumbs = () => {
        breadcrumbs.value = []
    }

    // Notifications
    const addNotification = (notification) => {
        const newNotification = {
            id: Date.now() + Math.random(),
            timestamp: new Date(),
            read: false,
            ...notification
        }
        notifications.value.unshift(newNotification)

        // Auto-remove after 10 seconds for info notifications
        if (notification.type === 'info') {
            setTimeout(() => {
                removeNotification(newNotification.id)
            }, 10000)
        }

        return newNotification.id
    }

    const removeNotification = (id) => {
        const index = notifications.value.findIndex(n => n.id === id)
        if (index > -1) {
            notifications.value.splice(index, 1)
        }
    }

    const markNotificationAsRead = (id) => {
        const notification = notifications.value.find(n => n.id === id)
        if (notification) {
            notification.read = true
        }
    }

    const markAllNotificationsAsRead = () => {
        notifications.value.forEach(n => n.read = true)
    }

    const clearNotifications = () => {
        notifications.value = []
    }

    // Error Management
    const addError = (error) => {
        const newError = {
            id: Date.now() + Math.random(),
            timestamp: new Date(),
            message: error.message || error,
            stack: error.stack,
            ...error
        }
        errors.value.push(newError)
        return newError.id
    }

    const removeError = (id) => {
        const index = errors.value.findIndex(e => e.id === id)
        if (index > -1) {
            errors.value.splice(index, 1)
        }
    }

    const clearErrors = () => {
        errors.value = []
    }

    // Theme Management
    const setTheme = (newTheme) => {
        theme.value = newTheme
        localStorage.setItem('theme', newTheme)

        // Apply theme to document
        document.documentElement.setAttribute('data-theme', newTheme)

        if (newTheme === 'dark') {
            document.documentElement.classList.add('dark')
        } else {
            document.documentElement.classList.remove('dark')
        }
    }

    const toggleTheme = () => {
        setTheme(theme.value === 'light' ? 'dark' : 'light')
    }

    // Language Management
    const setLanguage = (newLanguage) => {
        language.value = newLanguage
        localStorage.setItem('language', newLanguage)

        // Apply language to document
        document.documentElement.setAttribute('lang', newLanguage)
    }

    // Sidebar Management
    const toggleSidebar = () => {
        sidebarOpen.value = !sidebarOpen.value
    }

    const openSidebar = () => {
        sidebarOpen.value = true
    }

    const closeSidebar = () => {
        sidebarOpen.value = false
    }

    // Modal Management
    const openModal = (modalComponent, props = {}) => {
        const modal = {
            id: Date.now() + Math.random(),
            component: modalComponent,
            props,
            timestamp: new Date()
        }

        modalStack.value.push(modal)
        activeModal.value = modal

        return modal.id
    }

    const closeModal = (modalId = null) => {
        if (modalId) {
            const index = modalStack.value.findIndex(m => m.id === modalId)
            if (index > -1) {
                modalStack.value.splice(index, 1)
            }
        } else {
            modalStack.value.pop()
        }

        activeModal.value = modalStack.value[modalStack.value.length - 1] || null
    }

    const closeAllModals = () => {
        modalStack.value = []
        activeModal.value = null
    }

    // Alert Management
    const showAlert = (alert) => {
        const newAlert = {
            id: Date.now() + Math.random(),
            timestamp: new Date(),
            dismissible: true,
            autoHide: true,
            duration: 5000,
            ...alert
        }

        alerts.value.push(newAlert)

        if (newAlert.autoHide) {
            setTimeout(() => {
                hideAlert(newAlert.id)
            }, newAlert.duration)
        }

        return newAlert.id
    }

    const hideAlert = (id) => {
        const index = alerts.value.findIndex(a => a.id === id)
        if (index > -1) {
            alerts.value.splice(index, 1)
        }
    }

    const clearAlerts = () => {
        alerts.value = []
    }

    // Search Management
    const setSearchQuery = (query) => {
        searchQuery.value = query
    }

    const setSearchResults = (results) => {
        searchResults.value = results
    }

    const setSearching = (searching) => {
        isSearching.value = searching
    }

    const clearSearch = () => {
        searchQuery.value = ''
        searchResults.value = []
        isSearching.value = false
    }

    // Network Status
    const setOnlineStatus = (online) => {
        isOnline.value = online

        if (online) {
            networkError.value = false
            addNotification({
                type: 'success',
                title: 'Bağlantı Kuruldu',
                message: 'İnternet bağlantısı yeniden kuruldu.'
            })
        } else {
            networkError.value = true
            addNotification({
                type: 'warning',
                title: 'Bağlantı Kesildi',
                message: 'İnternet bağlantısı kesildi. Lütfen bağlantınızı kontrol edin.'
            })
        }
    }

    // Initialize app
    const initializeApp = () => {
        // Set initial theme
        setTheme(theme.value)

        // Set initial language
        setLanguage(language.value)

        // Listen for network status changes
        window.addEventListener('online', () => setOnlineStatus(true))
        window.addEventListener('offline', () => setOnlineStatus(false))

        // Listen for storage changes (for multi-tab sync)
        window.addEventListener('storage', (e) => {
            if (e.key === 'theme') {
                setTheme(e.newValue || 'light')
            } else if (e.key === 'language') {
                setLanguage(e.newValue || 'tr')
            }
        })
    }

    return {
        // State
        isLoading,
        loadingMessage,
        notifications,
        sidebarOpen,
        theme,
        language,
        pageTitle,
        breadcrumbs,
        errors,
        searchQuery,
        searchResults,
        isSearching,
        modalStack,
        activeModal,
        alerts,
        appConfig,
        isOnline,
        networkError,

        // Getters
        isDarkMode,
        hasNotifications,
        unreadNotificationCount,
        hasErrors,
        currentLanguageLabel,

        // Actions
        setLoading,
        setPageTitle,
        setBreadcrumbs,
        addBreadcrumb,
        clearBreadcrumbs,
        addNotification,
        removeNotification,
        markNotificationAsRead,
        markAllNotificationsAsRead,
        clearNotifications,
        addError,
        removeError,
        clearErrors,
        setTheme,
        toggleTheme,
        setLanguage,
        toggleSidebar,
        openSidebar,
        closeSidebar,
        openModal,
        closeModal,
        closeAllModals,
        showAlert,
        hideAlert,
        clearAlerts,
        setSearchQuery,
        setSearchResults,
        setSearching,
        clearSearch,
        setOnlineStatus,
        initializeApp
    }
})

export default useAppStore