// src/services/comment.js
import api from './api'

const CommentService = {
    // Get Comments
    async getPostComments(postId, page = 1, limit = 50) {
        const response = await api.get(`/Home/Comment/${postId}`, {
            params: { page, limit }
        })
        return response.data
    },

    async getComment(commentId) {
        const response = await api.get(`/Home/Comment/Detail/${commentId}`)
        return response.data
    },

    // Create Comment
    async createComment(commentData) {
        const response = await api.post('/Home/Comment', commentData)
        return response.data
    },

    // Update Comment
    async updateComment(commentId, commentData) {
        const response = await api.put(`/Home/Comment/${commentId}`, commentData)
        return response.data
    },

    // Delete Comment
    async deleteComment(commentId) {
        const response = await api.delete(`/Home/Comment/${commentId}`)
        return response.data
    },

    // Reply to Comment
    async replyToComment(parentCommentId, replyData) {
        const response = await api.post(`/Home/Comment/${parentCommentId}/reply`, replyData)
        return response.data
    },

    // Get Comment Replies
    async getCommentReplies(commentId, page = 1, limit = 20) {
        const response = await api.get(`/Home/Comment/${commentId}/replies`, {
            params: { page, limit }
        })
        return response.data
    },

    // Like/Unlike Comment
    async likeComment(commentId) {
        const response = await api.post(`/Home/Comment/${commentId}/like`)
        return response.data
    },

    async unlikeComment(commentId) {
        const response = await api.delete(`/Home/Comment/${commentId}/like`)
        return response.data
    },

    // Report Comment
    async reportComment(commentId, reason) {
        const response = await api.post(`/Home/Comment/${commentId}/report`, { reason })
        return response.data
    },

    // Comment Moderation
    async approveComment(commentId) {
        const response = await api.put(`/Home/Comment/${commentId}/approve`)
        return response.data
    },

    async rejectComment(commentId, reason = '') {
        const response = await api.put(`/Home/Comment/${commentId}/reject`, { reason })
        return response.data
    },

    // User Comments
    async getUserComments(userId, page = 1, limit = 20) {
        const response = await api.get(`/Account/UserComments/${userId}`, {
            params: { page, limit }
        })
        return response.data
    },

    async getMyComments(page = 1, limit = 20) {
        const response = await api.get('/Account/MyComments', {
            params: { page, limit }
        })
        return response.data
    },

    // Comment Statistics
    async getCommentStats(postId) {
        const response = await api.get(`/Home/Comment/${postId}/stats`)
        return response.data
    },

    // Bulk Operations
    async bulkDeleteComments(commentIds) {
        const response = await api.post('/Home/Comment/bulk-delete', { commentIds })
        return response.data
    },

    async bulkApproveComments(commentIds) {
        const response = await api.post('/Home/Comment/bulk-approve', { commentIds })
        return response.data
    },

    // Comment Search
    async searchComments(query, filters = {}) {
        const response = await api.get('/Home/Comment/search', {
            params: { query, ...filters }
        })
        return response.data
    },

    // Comment Notifications
    async getCommentNotifications(page = 1, limit = 20) {
        const response = await api.get('/Account/CommentNotifications', {
            params: { page, limit }
        })
        return response.data
    },

    async markCommentNotificationAsRead(notificationId) {
        const response = await api.put(`/Account/CommentNotifications/${notificationId}/read`)
        return response.data
    },

    // Comment Settings
    async getCommentSettings() {
        const response = await api.get('/Account/CommentSettings')
        return response.data
    },

    async updateCommentSettings(settings) {
        const response = await api.put('/Account/CommentSettings', settings)
        return response.data
    }
}

export default CommentService