// src/services/api.js
import axios from 'axios'
import { useAuthStore } from '@/stores/auth'
import { useAppStore } from '@/stores/app'
import router from '@/router'

// Create axios instance
const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || '/api',
    timeout: 30000,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
})

// Request interceptor
api.interceptors.request.use(
    (config) => {
        const authStore = useAuthStore()
        const appStore = useAppStore()

        // Add auth token if available
        if (authStore.token) {
            config.headers.Authorization = `Bearer ${authStore.token}`
        }

        // Add CSRF token if available
        const csrfToken = document.querySelector('meta[name="csrf-token"]')?.content
        if (csrfToken) {
            config.headers['X-CSRF-TOKEN'] = csrfToken
        }

        // Show loading for non-GET requests
        if (config.method !== 'get') {
            appStore.setLoading(true)
        }

        return config
    },
    (error) => {
        const appStore = useAppStore()
        appStore.setLoading(false)
        return Promise.reject(error)
    }
)

// Response interceptor
api.interceptors.response.use(
    (response) => {
        const appStore = useAppStore()
        appStore.setLoading(false)

        return response
    },
    (error) => {
        const authStore = useAuthStore()
        const appStore = useAppStore()

        appStore.setLoading(false)

        // Handle different error status codes
        if (error.response) {
            const { status, data } = error.response

            switch (status) {
                case 401:
                    // Unauthorized - clear auth and redirect to login
                    authStore.logout()
                    if (router.currentRoute.value.meta?.requiresAuth) {
                        router.push({
                            name: 'Login',
                            query: { returnUrl: router.currentRoute.value.fullPath }
                        })
                    }
                    break

                case 403:
                    // Forbidden - show error message
                    console.error('Access denied:', data?.message || 'You do not have permission to access this resource')
                    break

                case 404:
                    // Not found - redirect to 404 page for navigation requests
                    if (error.config.method === 'get' && !error.config.url.includes('/api/')) {
                        router.push({ name: 'NotFound' })
                    }
                    break

                case 422:
                    // Validation errors - these are handled by the calling component
                    break

                case 429:
                    // Too many requests
                    console.error('Rate limit exceeded. Please try again later.')
                    break

                case 500:
                    // Server error
                    console.error('Server error occurred. Please try again later.')
                    break

                default:
                    console.error('API Error:', data?.message || error.message)
            }
        } else if (error.request) {
            // Network error
            console.error('Network error. Please check your internet connection.')
        } else {
            // Other error
            console.error('Request failed:', error.message)
        }

        return Promise.reject(error)
    }
)

// Helper function to handle errors
const handleError = (error) => {
    if (error.response?.data) {
        return {
            message: error.response.data.message || 'An error occurred',
            errors: error.response.data.errors || {},
            status: error.response.status
        }
    }

    return {
        message: error.message || 'Network error occurred',
        errors: {},
        status: 0
    }
}

// Helper methods for auth token management
api.setAuthToken = (token) => {
    if (token) {
        api.defaults.headers.Authorization = `Bearer ${token}`
    } else {
        delete api.defaults.headers.Authorization
    }
}

api.clearAuthToken = () => {
    delete api.defaults.headers.Authorization
}

// Enhanced API methods that wrap the original axios methods
const apiService = {
    // GET request with error handling
    async get(url, config = {}) {
        try {
            const response = await api.get(url, config)
            return { data: response.data, status: response.status }
        } catch (error) {
            throw handleError(error)
        }
    },

    // POST request with error handling
    async post(url, data = {}, config = {}) {
        try {
            const response = await api.post(url, data, config)
            return { data: response.data, status: response.status }
        } catch (error) {
            throw handleError(error)
        }
    },

    // PUT request with error handling
    async put(url, data = {}, config = {}) {
        try {
            const response = await api.put(url, data, config)
            return { data: response.data, status: response.status }
        } catch (error) {
            throw handleError(error)
        }
    },

    // PATCH request with error handling
    async patch(url, data = {}, config = {}) {
        try {
            const response = await api.patch(url, data, config)
            return { data: response.data, status: response.status }
        } catch (error) {
            throw handleError(error)
        }
    },

    // DELETE request with error handling
    async delete(url, config = {}) {
        try {
            const response = await api.delete(url, config)
            return { data: response.data, status: response.status }
        } catch (error) {
            throw handleError(error)
        }
    },

    // Utility methods
    setAuthToken: api.setAuthToken,
    clearAuthToken: api.clearAuthToken,

    // Access to the raw axios instance if needed
    raw: api
}

export default apiService