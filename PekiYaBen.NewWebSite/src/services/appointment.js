// src/services/appointment.js
import api from './api'

const AppointmentService = {
    // User Appointments
    async getUserAppointments(status = null, page = 1, limit = 10) {
        const params = { page, limit }
        if (status) params.status = status

        const response = await api.get('/Account/MyInterviews', { params })
        return response.data
    },

    async getAppointment(appointmentId) {
        const response = await api.get(`/Account/Interview/${appointmentId}`)
        return response.data
    },

    // Create Appointment
    async createAppointment(appointmentData) {
        const response = await api.post('/Account/NewInterview', appointmentData)
        return response.data
    },

    // Update Appointment
    async updateAppointment(appointmentId, appointmentData) {
        const response = await api.put(`/Account/EditMeeting/${appointmentId}`, appointmentData)
        return response.data
    },

    // Cancel Appointment
    async cancelAppointment(appointmentId, reason = '') {
        const response = await api.post(`/Account/CancelMeeting/${appointmentId}`, { reason })
        return response.data
    },

    // Reschedule Appointment
    async rescheduleAppointment(appointmentId, newDateTime) {
        const response = await api.put(`/Account/RescheduleMeeting/${appointmentId}`, {
            newDateTime
        })
        return response.data
    },

    // Coach Calendar
    async getCoachCalendar(coachId, month, year) {
        const response = await api.get(`/Account/InterviewCalendar/${coachId}`, {
            params: { month, year }
        })
        return response.data
    },

    async getAvailableSlots(coachId, date) {
        const response = await api.get(`/Account/AvailableSlots/${coachId}`, {
            params: { date }
        })
        return response.data
    },

    // Coach Availability
    async getCoachAvailability(coachId, startDate, endDate) {
        const response = await api.get(`/Account/CoachAvailability/${coachId}`, {
            params: { startDate, endDate }
        })
        return response.data
    },

    // Appointment Types/Services
    async getAppointmentTypes() {
        const response = await api.get('/Account/AppointmentTypes')
        return response.data
    },

    async getAppointmentType(typeId) {
        const response = await api.get(`/Account/AppointmentType/${typeId}`)
        return response.data
    },

    // Payment Integration
    async createPayment(appointmentId, paymentData) {
        const response = await api.post(`/Account/Payment/${appointmentId}`, paymentData)
        return response.data
    },

    async verifyPayment(appointmentId, paymentId) {
        const response = await api.post(`/Account/VerifyPayment/${appointmentId}`, {
            paymentId
        })
        return response.data
    },

    async refundPayment(appointmentId, reason = '') {
        const response = await api.post(`/Account/RefundPayment/${appointmentId}`, { reason })
        return response.data
    },

    // Appointment Reminders
    async getAppointmentReminders(appointmentId) {
        const response = await api.get(`/Account/Reminders/${appointmentId}`)
        return response.data
    },

    async updateReminderSettings(appointmentId, settings) {
        const response = await api.put(`/Account/Reminders/${appointmentId}`, settings)
        return response.data
    },

    // Appointment Notes
    async getAppointmentNotes(appointmentId) {
        const response = await api.get(`/Account/AppointmentNotes/${appointmentId}`)
        return response.data
    },

    async addAppointmentNote(appointmentId, note) {
        const response = await api.post(`/Account/AppointmentNotes/${appointmentId}`, { note })
        return response.data
    },

    async updateAppointmentNote(appointmentId, noteId, note) {
        const response = await api.put(`/Account/AppointmentNotes/${appointmentId}/${noteId}`, { note })
        return response.data
    },

    async deleteAppointmentNote(appointmentId, noteId) {
        const response = await api.delete(`/Account/AppointmentNotes/${appointmentId}/${noteId}`)
        return response.data
    },

    // Appointment History
    async getAppointmentHistory(page = 1, limit = 10) {
        const response = await api.get('/Account/AppointmentHistory', {
            params: { page, limit }
        })
        return response.data
    },

    // Feedback and Reviews
    async submitAppointmentFeedback(appointmentId, feedback) {
        const response = await api.post(`/Account/AppointmentFeedback/${appointmentId}`, feedback)
        return response.data
    },

    async getAppointmentFeedback(appointmentId) {
        const response = await api.get(`/Account/AppointmentFeedback/${appointmentId}`)
        return response.data
    },

    // Appointment Statistics
    async getAppointmentStats() {
        const response = await api.get('/Account/AppointmentStats')
        return response.data
    },

    // Bulk Operations
    async bulkCancelAppointments(appointmentIds, reason = '') {
        const response = await api.post('/Account/BulkCancelAppointments', {
            appointmentIds,
            reason
        })
        return response.data
    },

    async bulkRescheduleAppointments(appointments) {
        const response = await api.post('/Account/BulkRescheduleAppointments', {
            appointments
        })
        return response.data
    },

    // Appointment Search
    async searchAppointments(query, filters = {}) {
        const response = await api.get('/Account/SearchAppointments', {
            params: { query, ...filters }
        })
        return response.data
    },

    // Recurring Appointments
    async createRecurringAppointment(appointmentData) {
        const response = await api.post('/Account/RecurringAppointment', appointmentData)
        return response.data
    },

    async updateRecurringAppointment(seriesId, appointmentData) {
        const response = await api.put(`/Account/RecurringAppointment/${seriesId}`, appointmentData)
        return response.data
    },

    async cancelRecurringAppointment(seriesId, reason = '') {
        const response = await api.post(`/Account/CancelRecurringAppointment/${seriesId}`, { reason })
        return response.data
    },

    // Waiting List
    async joinWaitingList(coachId, preferredDate, timeSlots) {
        const response = await api.post('/Account/JoinWaitingList', {
            coachId,
            preferredDate,
            timeSlots
        })
        return response.data
    },

    async leaveWaitingList(waitingListId) {
        const response = await api.delete(`/Account/WaitingList/${waitingListId}`)
        return response.data
    },

    async getWaitingListStatus() {
        const response = await api.get('/Account/WaitingListStatus')
        return response.data
    }
}

export default AppointmentService