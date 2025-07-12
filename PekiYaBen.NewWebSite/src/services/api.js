import axios from 'axios'
import { useAppStore } from '@/stores/app'

class ApiClient {
  constructor() {
    this.client = axios.create({
      baseURL: '/api', // This will be proxied to your .NET backend
      timeout: 30000,
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    })
    
    this.setupInterceptors()
  }
  
  setupInterceptors() {
    // Request interceptor
    this.client.interceptors.request.use(
      (config) => {
        const appStore = useAppStore()
        appStore.setLoading(true)
        
        // Add auth token if available
        const token = localStorage.getItem('auth_token')
        if (token) {
          config.headers.Authorization = `Bearer ${token}`
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
    this.client.interceptors.response.use(
      (response) => {
        const appStore = useAppStore()
        appStore.setLoading(false)
        return response
      },
      (error) => {
        const appStore = useAppStore()
        appStore.setLoading(false)
        
        // Handle common errors
        if (error.response?.status === 401) {
          // Unauthorized - redirect to login
          localStorage.removeItem('auth_token')
          localStorage.removeItem('auth_user')
          window.location.href = '/hesabim/giris'
        } else if (error.response?.status === 404) {
          // Not found - redirect to error page
          window.location.href = '/hata'
        }
        
        return Promise.reject(error)
      }
    )
  }
  
  setAuthToken(token) {
    if (token) {
      this.client.defaults.headers.Authorization = `Bearer ${token}`
    } else {
      delete this.client.defaults.headers.Authorization
    }
  }
  
  // HTTP Methods
  get(url, config = {}) {
    return this.client.get(url, config)
  }
  
  post(url, data = {}, config = {}) {
    return this.client.post(url, data, config)
  }
  
  put(url, data = {}, config = {}) {
    return this.client.put(url, data, config)
  }
  
  patch(url, data = {}, config = {}) {
    return this.client.patch(url, data, config)
  }
  
  delete(url, config = {}) {
    return this.client.delete(url, config)
  }
  
  // File upload
  uploadFile(url, file, onProgress = null) {
    const formData = new FormData()
    formData.append('file', file)
    
    const config = {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    }
    
    if (onProgress) {
      config.onUploadProgress = onProgress
    }
    
    return this.client.post(url, formData, config)
  }
}

export default new ApiClient()