// src/stores/auth.js
import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/services/api'
import router from '@/router'

export const useAuthStore = defineStore('auth', () => {
    // State
    const user = ref(null)
    const token = ref(localStorage.getItem('auth_token'))
    const isLoading = ref(false)
    const loginAttempts = ref(0)
    const lastLoginAttempt = ref(null)

    // Getters
    const isAuthenticated = computed(() => !!token.value && !!user.value)
    const userRole = computed(() => user.value?.role || 'user')
    const userFullName = computed(() => {
        if (!user.value) return ''
        return `${user.value.firstName || ''} ${user.value.lastName || ''}`.trim()
    })
    const userInitials = computed(() => {
        if (!user.value) return ''
        const firstName = user.value.firstName || ''
        const lastName = user.value.lastName || ''
        return `${firstName.charAt(0)}${lastName.charAt(0)}`.toUpperCase()
    })

    // Actions
    const setToken = (newToken) => {
        token.value = newToken
        if (newToken) {
            localStorage.setItem('auth_token', newToken)
            api.setAuthToken(newToken)
        } else {
            localStorage.removeItem('auth_token')
            api.clearAuthToken()
        }
    }

    const setUser = (userData) => {
        user.value = userData
    }

    const updateUser = (userData) => {
        if (user.value) {
            user.value = { ...user.value, ...userData }
        }
    }

    const login = async (credentials) => {
        try {
            isLoading.value = true

            // Check rate limiting
            const now = Date.now()
            if (lastLoginAttempt.value && (now - lastLoginAttempt.value < 60000) && loginAttempts.value >= 5) {
                throw new Error('Too many login attempts. Please try again in a minute.')
            }

            const response = await api.post('/Account/Login', credentials)

            if (response.data.success) {
                const { token: authToken, user: userData } = response.data

                setToken(authToken)
                setUser(userData)

                // Reset login attempts on successful login
                loginAttempts.value = 0
                lastLoginAttempt.value = null

                return response.data
            } else {
                throw new Error(response.data.message || 'Login failed')
            }
        } catch (error) {
            // Track failed login attempts
            loginAttempts.value++
            lastLoginAttempt.value = Date.now()

            throw error
        } finally {
            isLoading.value = false
        }
    }

    const register = async (userData) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/Register', userData)

            if (response.data.success) {
                // Some implementations auto-login after registration
                if (response.data.token) {
                    const { token: authToken, user: newUser } = response.data
                    setToken(authToken)
                    setUser(newUser)
                }

                return response.data
            } else {
                throw new Error(response.data.message || 'Registration failed')
            }
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const logout = async (redirectToLogin = false) => {
        try {
            // Call logout endpoint if user is authenticated
            if (token.value) {
                await api.post('/Account/Logout').catch(() => {
                    // Ignore errors on logout endpoint
                })
            }
        } finally {
            // Clear local state regardless of API call result
            setToken(null)
            setUser(null)
            loginAttempts.value = 0
            lastLoginAttempt.value = null

            if (redirectToLogin) {
                router.push({ name: 'Login' })
            }
        }
    }

    const fetchUser = async () => {
        if (!token.value) return null

        try {
            isLoading.value = true
            const response = await api.get('/Account/Profile')

            if (response.data) {
                setUser(response.data)
                return response.data
            } else {
                // Token might be invalid
                await logout()
                return null
            }
        } catch (error) {
            // Token is likely invalid
            await logout()
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const forgotPassword = async (email) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/ForgotPassword', { email })
            return response.data
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const resetPassword = async (resetData) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/ResetPassword', resetData)
            return response.data
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const changePassword = async (passwordData) => {
        try {
            isLoading.value = true
            const response = await api.put('/Account/ChangePassword', passwordData)
            return response.data
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const verifyEmail = async (token) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/VerifyEmail', { token })
            return response.data
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const resendEmailVerification = async () => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/ResendEmailVerification')
            return response.data
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const refreshToken = async () => {
        if (!token.value) return false

        try {
            const response = await api.post('/Account/RefreshToken')

            if (response.data.success && response.data.token) {
                setToken(response.data.token)
                if (response.data.user) {
                    setUser(response.data.user)
                }
                return true
            } else {
                await logout()
                return false
            }
        } catch (error) {
            await logout()
            return false
        }
    }

    const checkAuthStatus = async () => {
        if (!token.value) return false

        try {
            await fetchUser()
            return true
        } catch (error) {
            return false
        }
    }

    // Initialize auth on store creation
    const initializeAuth = async () => {
        if (token.value) {
            api.setAuthToken(token.value)
            try {
                await fetchUser()
            } catch (error) {
                // Token is invalid, clear it
                await logout()
            }
        }
    }

    // Social login methods
    const loginWithGoogle = async (googleToken) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/LoginWithGoogle', { token: googleToken })

            if (response.data.success) {
                const { token: authToken, user: userData } = response.data
                setToken(authToken)
                setUser(userData)
                return response.data
            } else {
                throw new Error(response.data.message || 'Google login failed')
            }
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    const loginWithFacebook = async (facebookToken) => {
        try {
            isLoading.value = true
            const response = await api.post('/Account/LoginWithFacebook', { token: facebookToken })

            if (response.data.success) {
                const { token: authToken, user: userData } = response.data
                setToken(authToken)
                setUser(userData)
                return response.data
            } else {
                throw new Error(response.data.message || 'Facebook login failed')
            }
        } catch (error) {
            throw error
        } finally {
            isLoading.value = false
        }
    }

    return {
        // State
        user,
        token,
        isLoading,

        // Getters
        isAuthenticated,
        userRole,
        userFullName,
        userInitials,

        // Actions
        login,
        register,
        logout,
        fetchUser,
        forgotPassword,
        resetPassword,
        changePassword,
        verifyEmail,
        resendEmailVerification,
        refreshToken,
        checkAuthStatus,
        initializeAuth,
        loginWithGoogle,
        loginWithFacebook,
        updateUser,
        setUser,
        setToken
    }
})

export default useAuthStore