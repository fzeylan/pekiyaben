import apiClient from './api'

const CoachesService = {
    // Get all coaches for a specific interview type
    async getCoaches(interviewType = '') {
        try {
            const response = await apiClient.get('/Account/_Coaches', {
                params: { interviewType }
            })
            return response.data
        } catch (error) {
            console.error('Error fetching coaches:', error)
            throw error
        }
    },

    // Get coach details
    async getCoachDetail(id, title, coachId, coachName) {
        try {
            const response = await apiClient.get(`/Home/Coach/${id}/${title}/${coachId}/${coachName}`)
            return response.data
        } catch (error) {
            console.error('Error fetching coach detail:', error)
            throw error
        }
    },

    // Get coach comments
    async getCoachComments(id, title, coachId, coachName) {
        try {
            const response = await apiClient.get(`/Home/Comment/${id}/${title}/${coachId}/${coachName}`)
            return response.data
        } catch (error) {
            console.error('Error fetching coach comments:', error)
            throw error
        }
    },

    // Get available calendar slots for a coach
    async getAvailableSlots(coachId, product, dateStart) {
        try {
            const response = await apiClient.post('/Account/GetCalendar', {
                id: coachId,
                product,
                dateStart
            })
            return response.data
        } catch (error) {
            console.error('Error fetching available slots:', error)
            throw error
        }
    },

    // Book an interview slot
    async bookInterview(bookingData) {
        try {
            const response = await apiClient.post('/Account/BookInterview', bookingData)
            return response.data
        } catch (error) {
            console.error('Error booking interview:', error)
            throw error
        }
    },
}

export default CoachesService