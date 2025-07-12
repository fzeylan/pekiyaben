import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAppStore = defineStore('app', () => {
  // State
  const isLoading = ref(false)
  const loadingCount = ref(0)
  const sidebarOpen = ref(false)
  const theme = ref('light')
  const breadcrumbs = ref([])
  
  // Getters
  const isDarkTheme = computed(() => theme.value === 'dark')
  
  // Actions
  const setLoading = (loading) => {
    if (loading) {
      loadingCount.value++
    } else {
      loadingCount.value = Math.max(0, loadingCount.value - 1)
    }
    isLoading.value = loadingCount.value > 0
  }
  
  const toggleSidebar = () => {
    sidebarOpen.value = !sidebarOpen.value
  }
  
  const closeSidebar = () => {
    sidebarOpen.value = false
  }
  
  const toggleTheme = () => {
    theme.value = theme.value === 'light' ? 'dark' : 'light'
    localStorage.setItem('theme', theme.value)
    updateThemeClass()
  }
  
  const initializeTheme = () => {
    const savedTheme = localStorage.getItem('theme')
    if (savedTheme) {
      theme.value = savedTheme
    } else {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches
      theme.value = prefersDark ? 'dark' : 'light'
    }
    updateThemeClass()
  }
  
  const updateThemeClass = () => {
    if (theme.value === 'dark') {
      document.documentElement.classList.add('dark')
    } else {
      document.documentElement.classList.remove('dark')
    }
  }
  
  const setBreadcrumbs = (crumbs) => {
    breadcrumbs.value = crumbs
  }
  
  const addBreadcrumb = (crumb) => {
    breadcrumbs.value.push(crumb)
  }
  
  const clearBreadcrumbs = () => {
    breadcrumbs.value = []
  }
  
  return {
    // State
    isLoading: readonly(isLoading),
    sidebarOpen: readonly(sidebarOpen),
    theme: readonly(theme),
    breadcrumbs: readonly(breadcrumbs),
    
    // Getters
    isDarkTheme,
    
    // Actions
    setLoading,
    toggleSidebar,
    closeSidebar,
    toggleTheme,
    initializeTheme,
    setBreadcrumbs,
    addBreadcrumb,
    clearBreadcrumbs
  }
})