import api from './api'

const ContentService = {
    // Blog Posts
    async getBlogPosts(params = {}) {
        const response = await api.get('/Home/Blog', { params })
        return response.data
    },

    async getBlogPost(id) {
        const response = await api.get(`/Home/BlogDetail/${id}`)
        return response.data
    },

    async getRelatedPosts(postId, categoryId, limit = 5) {
        const response = await api.get('/Home/Blog', {
            params: {
                categoryId,
                excludeId: postId,
                limit
            }
        })
        return response.data
    },

    async getPopularPosts(limit = 10) {
        const response = await api.get('/Home/Blog', {
            params: {
                sortBy: 'popular',
                limit
            }
        })
        return response.data
    },

    async searchBlogPosts(query, page = 1, limit = 10) {
        const response = await api.get('/Home/Blog', {
            params: {
                search: query,
                page,
                limit
            }
        })
        return response.data
    },

    // Blog Categories
    async getBlogCategories() {
        const response = await api.get('/Home/BlogCategories')
        return response.data
    },

    async getBlogCategory(id) {
        const response = await api.get(`/Home/BlogCategory/${id}`)
        return response.data
    },

    // Coaches
    async getCoaches() {
        const response = await api.get('/webapi/coaches')
        return response.data
    },

    async getCoach(id) {
        const response = await api.get(`/Home/Coach/${id}`)
        return response.data
    },

    async getCoachBySlug(slug) {
        const response = await api.get(`/Home/CoachBySlug/${slug}`)
        return response.data
    },

    async getCoachCategories() {
        const response = await api.get('/Home/CoachCategories')
        return response.data
    },

    async getCoachReviews(coachId, page = 1, limit = 10) {
        const response = await api.get(`/Home/Coach/${coachId}/Reviews`, {
            params: { page, limit }
        })
        return response.data
    },

    async getSimilarCoaches(coachId, categoryId, limit = 5) {
        const response = await api.get('/Home/Coaches', {
            params: {
                categoryId,
                excludeId: coachId,
                limit
            }
        })
        return response.data
    },

    async getCoachAvailability(coachId, date) {
        const response = await api.get(`/Home/Coach/${coachId}/Availability`, {
            params: { date }
        })
        return response.data
    },

    // Services
    async getServices() {
        const response = await api.get('/Home/Products')
        return response.data
    },

    async getService(id) {
        const response = await api.get(`/Home/Product/${id}`)
        return response.data
    },

    async getServicesByCategory(categoryId) {
        const response = await api.get('/Home/Products', {
            params: { categoryId }
        })
        return response.data
    },

    // Interviews/Sessions
    async getInterviews(params = {}) {
        const response = await api.get('/Home/Interviews', { params })
        return response.data
    },

    async getInterview(id) {
        const response = await api.get(`/Home/Interview/${id}`)
        return response.data
    },

    // Static Pages
    async getAboutPage() {
        const response = await api.get('/Home/About')
        return response.data
    },

    async getContactPage() {
        const response = await api.get('/Home/Contact')
        return response.data
    },

    async getFAQs() {
        const response = await api.get('/Home/FAQ')
        return response.data
    },

    // Newsletter
    async subscribeNewsletter(email) {
        const response = await api.post('/Home/Newsletter', { email })
        return response.data
    },

    // Contact Form
    async submitContactForm(data) {
        const response = await api.post('/Home/Contact', data)
        return response.data
    },

    // Search
    async globalSearch(query) {
        const response = await api.get('/Home/Search', {
            params: { q: query }
        })
        return response.data
    },

    // Home Page Data
    async getHomePageData() {
        const response = await api.get('/Home/Index')
        return response.data
    },

    // Testimonials
    async getTestimonials() {
        const response = await api.get('/Home/Testimonials')
        return response.data
    },

    // Blog Statistics
    async getBlogStats() {
        const response = await api.get('/Home/BlogStats')
        return response.data
    },

    // Get FAQ items
    async getFAQs() {
        try {
            const response = await api.get('/Home/FAQ')
            return response.data
        } catch (error) {
            console.error('Error fetching FAQs:', error)
            throw error
        }
    },

    // Get products/services
    async getProducts() {
        try {
            const response = await api.get('/Home/GetProducts')
            return response.data
        } catch (error) {
            console.error('Error fetching products:', error)
            throw error
        }
    },

    // Get product detail
    async getProductDetail(id, title = '') {
        try {
            let url = '/Account/NewInterview'
            if (id && title) {
                url += `/${id}/${title}`
            }
            const response = await api.get(url)
            return response.data
        } catch (error) {
            console.error('Error fetching product detail:', error)
            throw error
        }
    },

    // Submit contact form
    async submitContactForm(formData) {
        try {
            const response = await api.post('/Home/Index', formData)
            return response.data
        } catch (error) {
            console.error('Error submitting contact form:', error)
            throw error
        }
    }
}

export default ContentService