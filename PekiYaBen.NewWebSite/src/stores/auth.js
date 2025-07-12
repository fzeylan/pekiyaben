import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import apiClient from '@/services/api'

export const useAuthStore = defineStore('auth', () => {
  const router = useRouter()
  const toast = useToast()
  
  // State
  const user = ref(null)
  const token = ref(null)
  const isLoading = ref(false)
  
  // Getters
  const isAuthenticated = computed(() => !!token.value && !!user.value)
  const userFullName = computed(() => user.value?.fullName || '')
  const userEmail = computed(() => user.value?.email || '')
  
  // Actions
  const initializeAuth = () => {
    const savedToken = localStorage.getItem('auth_token')
    const savedUser = localStorage.getItem('auth_user')
    
    if (savedToken && savedUser) {
      try {
        token.value = savedToken
        user.value = JSON.parse(savedUser)
        apiClient.setAuthToken(savedToken)
      } catch (error) {
        console.error('Error parsing saved user data:', error)
        logout()
      }
    }
  }
  
  const login = async (credentials) => {
    isLoading.value = true
    try {
      const response = await apiClient.post('/Account/LoginMod', {
        email: credentials.email,
        pass: credentials.password
      })
      
      if (response.data.Succeed) {
        // Since your backend doesn't return JWT tokens, we'll create a simple auth token
        const authToken = btoa(`${credentials.email}:${Date.now()}`)
        
        // Get user details after successful login
        const userResponse = await apiClient.get('/Account/GetCurrentUser')
        
        token.value = authToken
        user.value = userResponse.data
        
        // Store in localStorage
        localStorage.setItem('auth_token', authToken)
        localStorage.setItem('auth_user', JSON.stringify(userResponse.data))
        
        apiClient.setAuthToken(authToken)
        
        toast.success('Başarıyla giriş yaptınız!')
        
        return { success: true }
      } else {
        toast.error(response.data.Message || 'Giriş başarısız!')
        return { success: false, message: response.data.Message }
      }
    } catch (error) {
      const message = error.response?.data?.Message || 'Giriş sırasında bir hata oluştu'
      toast.error(message)
      return { success: false, message }
    } finally {
      isLoading.value = false
    }
  }
  
  const register = async (userData) => {
    isLoading.value = true
    try {
      const response = await apiClient.post('/Account/SignMod', {
        fullName: userData.fullName,
        email: userData.email,
        pass: userData.password,
        newPass: userData.confirmPassword,
        agreement: userData.agreement
      })
      
      if (response.data.Succeed) {
        toast.success('Kayıt işlemi başarılı! Giriş yapabilirsiniz.')
        return { success: true }
      } else {
        toast.error(response.data.Message || 'Kayıt başarısız!')
        return { success: false, message: response.data.Message }
      }
    } catch (error) {
      const message = error.response?.data?.Message || 'Kayıt sırasında bir hata oluştu'
      toast.error(message)
      return { success: false, message }
    } finally {
      isLoading.value = false
    }
  }
  
  const forgotPassword = async (email) => {
    isLoading.value = true
    try {
      const response = await apiClient.post('/Account/RemindPassword', { email })
      
      if (response.data.Succeed) {
        toast.success('Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.')
        return { success: true }
      } else {
        toast.error(response.data.Message || 'İşlem başarısız!')
        return { success: false, message: response.data.Message }
      }
    } catch (error) {
      const message = error.response?.data?.Message || 'İşlem sırasında bir hata oluştu'
      toast.error(message)
      return { success: false, message }
    } finally {
      isLoading.value = false
    }
  }
  
  const updateProfile = async (profileData) => {
    isLoading.value = true
    try {
      const response = await apiClient.post('/Account/EditProfile', profileData)
      
      if (response.data.Succeed) {
        // Update user data in store
        user.value = { ...user.value, ...profileData }
        localStorage.setItem('auth_user', JSON.stringify(user.value))
        
        toast.success('Profil başarıyla güncellendi!')
        return { success: true }
      } else {
        toast.error(response.data.Message || 'Güncelleme başarısız!')
        return { success: false, message: response.data.Message }
      }
    } catch (error) {
      const message = error.response?.data?.Message || 'Güncelleme sırasında bir hata oluştu'
      toast.error(message)
      return { success: false, message }
    } finally {
      isLoading.value = false
    }
  }
  
  const logout = () => {
    user.value = null
    token.value = null
    
    localStorage.removeItem('auth_token')
    localStorage.removeItem('auth_user')
    
    apiClient.setAuthToken(null)
    
    toast.info('Başarıyla çıkış yaptınız.')
    
    if (router.currentRoute.value.meta.requiresAuth) {
      router.push({ name: 'Home' })
    }
  }
  
  const getCurrentUser = async () => {
    if (!isAuthenticated.value) return null
    
    try {
      const response = await apiClient.get('/Account/GetCurrentUser')
      user.value = response.data
      localStorage.setItem('auth_user', JSON.stringify(response.data))
      return response.data
    } catch (error) {
      console.error('Error fetching current user:', error)
      if (error.response?.status === 401) {
        logout()
      }
      return null
    }
  }
  
  return {
    // State
    user: readonly(user),
    token: readonly(token),
    isLoading: readonly(isLoading),
    
    // Getters
    isAuthenticated,
    userFullName,
    userEmail,
    
    // Actions
    initializeAuth,
    login,
    register,
    forgotPassword,
    updateProfile,
    logout,
    getCurrentUser
  }
})