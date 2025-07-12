// src/services/user.js
import api from './api'

const UserService = {
    // Profile Management
    async getProfile() {
        const response = await api.get('/Account/Profile')
        return response.data
    },

    async updateProfile(profileData) {
        const response = await api.put('/Account/EditProfile', profileData)
        return response.data
    },

    async updateAvatar(formData) {
        const response = await api.post('/Account/UpdateAvatar', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        return response.data
    },

    async updatePassword(passwordData) {
        const response = await api.put('/Account/ChangePassword', passwordData)
        return response.data
    },

    // User Orders
    async getOrders(page = 1, limit = 10) {
        const response = await api.get('/Account/Orders', {
            params: { page, limit }
        })
        return response.data
    },

    async getOrder(orderId) {
        const response = await api.get(`/Account/Order/${orderId}`)
        return response.data
    },

    // User Preferences
    async getPreferences() {
        const response = await api.get('/Account/Preferences')
        return response.data
    },

    async updatePreferences(preferences) {
        const response = await api.put('/Account/Preferences', preferences)
        return response.data
    },

    // Account Settings
    async getAccountSettings() {
        const response = await api.get('/Account/Settings')
        return response.data
    },

    async updateAccountSettings(settings) {
        const response = await api.put('/Account/Settings', settings)
        return response.data
    },

    // Notifications
    async getNotifications(page = 1, limit = 20) {
        const response = await api.get('/Account/Notifications', {
            params: { page, limit }
        })
        return response.data
    },

    async markNotificationAsRead(notificationId) {
        const response = await api.put(`/Account/Notifications/${notificationId}/read`)
        return response.data
    },

    async markAllNotificationsAsRead() {
        const response = await api.put('/Account/Notifications/read-all')
        return response.data
    },

    async deleteNotification(notificationId) {
        const response = await api.delete(`/Account/Notifications/${notificationId}`)
        return response.data
    },

    // User Statistics
    async getUserStats() {
        const response = await api.get('/Account/Statistics')
        return response.data
    },

    // Account Deletion
    async requestAccountDeletion() {
        const response = await api.post('/Account/RequestDeletion')
        return response.data
    },

    async cancelAccountDeletion() {
        const response = await api.post('/Account/CancelDeletion')
        return response.data
    },

    // Data Export
    async requestDataExport() {
        const response = await api.post('/Account/ExportData')
        return response.data
    },

    async getDataExportStatus() {
        const response = await api.get('/Account/ExportStatus')
        return response.data
    },

    // Privacy Settings
    async getPrivacySettings() {
        const response = await api.get('/Account/Privacy')
        return response.data
    },

    async updatePrivacySettings(settings) {
        const response = await api.put('/Account/Privacy', settings)
        return response.data
    },

    // Two-Factor Authentication
    async enableTwoFactor() {
        const response = await api.post('/Account/EnableTwoFactor')
        return response.data
    },

    async disableTwoFactor(code) {
        const response = await api.post('/Account/DisableTwoFactor', { code })
        return response.data
    },

    async verifyTwoFactor(code) {
        const response = await api.post('/Account/VerifyTwoFactor', { code })
        return response.data
    },

    // Session Management
    async getActiveSessions() {
        const response = await api.get('/Account/Sessions')
        return response.data
    },

    async revokeSession(sessionId) {
        const response = await api.delete(`/Account/Sessions/${sessionId}`)
        return response.data
    },

    async revokeAllSessions() {
        const response = await api.delete('/Account/Sessions/all')
        return response.data
    }
}

export default UserService