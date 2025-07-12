<template>
    <div class="interview-calendar-page">
        <!-- Breadcrumb -->
        <section class="breadcrumb-section bg-primary text-white py-4">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <h2 class="mb-3">Randevu Takvimi</h2>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb bg-transparent mb-0">
                                <li class="breadcrumb-item">
                                    <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                                </li>
                                <li class="breadcrumb-item">
                                    <router-link to="/kocluk-ekibimiz" class="text-white-50">Koçlar</router-link>
                                </li>
                                <li class="breadcrumb-item active text-white">{{ coach?.fullName }} - Randevu</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </section>

        <!-- Loading State -->
        <div v-if="loading" class="container py-5">
            <div class="text-center">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Yükleniyor...</span>
                </div>
                <p class="mt-3">Takvim yükleniyor...</p>
            </div>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="container py-5">
            <div class="alert alert-danger" role="alert">
                <h4 class="alert-heading">Hata!</h4>
                <p>{{ error }}</p>
                <hr>
                <router-link to="/kocluk-ekibimiz" class="btn btn-primary">
                    Koçlara Geri Dön
                </router-link>
            </div>
        </div>

        <!-- Calendar Content -->
        <section v-else-if="coach" class="calendar-content py-5">
            <div class="container">
                <div class="row">
                    <!-- Coach Info -->
                    <div class="col-lg-4 mb-4">
                        <div class="coach-info-card">
                            <div class="coach-header">
                                <img :src="coach.profilePhoto || '/default-coach.jpg'"
                                     :alt="coach.fullName"
                                     class="coach-avatar"
                                     @error="handleImageError" />
                                <div class="coach-details">
                                    <h4>{{ coach.fullName }}</h4>
                                    <p class="coach-title">{{ coach.title }}</p>
                                    <div class="coach-rating">
                                        <div class="stars">
                                            <i v-for="star in 5"
                                               :key="star"
                                               class="fas fa-star"
                                               :class="{ 'text-warning': star <= (coach.rating || 0) }"></i>
                                        </div>
                                        <span class="rating-text ms-2">{{ coach.rating || 0 }}/5</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Session Types -->
                            <div class="session-types mt-4">
                                <h6>Seans Türleri</h6>
                                <div v-for="sessionType in sessionTypes"
                                     :key="sessionType.id"
                                     class="session-type"
                                     :class="{ active: selectedSessionType?.id === sessionType.id }"
                                     @click="selectSessionType(sessionType)">
                                    <div class="session-info">
                                        <h6>{{ sessionType.name }}</h6>
                                        <p class="session-duration">{{ sessionType.duration }} dakika</p>
                                        <p class="session-price">{{ formatPrice(sessionType.price) }}</p>
                                    </div>
                                </div>
                            </div>

                            <!-- Selected Session Info -->
                            <div v-if="selectedSessionType" class="selected-session mt-4">
                                <h6>Seçilen Seans</h6>
                                <div class="session-summary">
                                    <p><strong>{{ selectedSessionType.name }}</strong></p>
                                    <p>Süre: {{ selectedSessionType.duration }} dakika</p>
                                    <p>Ücret: {{ formatPrice(selectedSessionType.price) }}</p>
                                </div>
                            </div>

                            <!-- Selected Date/Time -->
                            <div v-if="selectedDateTime" class="selected-datetime mt-4">
                                <h6>Seçilen Tarih & Saat</h6>
                                <div class="datetime-summary">
                                    <p><i class="fas fa-calendar me-2"></i>{{ formatDate(selectedDateTime.date) }}</p>
                                    <p><i class="fas fa-clock me-2"></i>{{ selectedDateTime.time }}</p>
                                </div>
                            </div>

                            <!-- Book Button -->
                            <div v-if="selectedSessionType && selectedDateTime" class="booking-actions mt-4">
                                <button @click="proceedToBooking"
                                        class="btn btn-primary btn-lg w-100"
                                        :disabled="isBooking">
                                    <span v-if="isBooking" class="spinner-border spinner-border-sm me-2"></span>
                                    Randevuyu Onayla
                                </button>
                                <p class="booking-note mt-2">
                                    <small class="text-muted">
                                        Randevunuzu onayladıktan sonra ödeme sayfasına yönlendirileceksiniz.
                                    </small>
                                </p>
                            </div>
                        </div>
                    </div>

                    <!-- Calendar -->
                    <div class="col-lg-8">
                        <div class="calendar-card">
                            <div class="calendar-header">
                                <h5>Müsait Günler ve Saatler</h5>
                                <p class="text-muted">
                                    Randevu almak istediğiniz tarihi ve saati seçiniz
                                </p>

                                <!-- Month Navigation -->
                                <div class="month-navigation">
                                    <button @click="previousMonth"
                                            class="btn btn-outline-primary"
                                            :disabled="!canGoPrevious">
                                        <i class="fas fa-chevron-left"></i>
                                    </button>
                                    <h5 class="current-month">
                                        {{ formatMonth(currentMonth) }}
                                    </h5>
                                    <button @click="nextMonth"
                                            class="btn btn-outline-primary">
                                        <i class="fas fa-chevron-right"></i>
                                    </button>
                                </div>
                            </div>

                            <!-- Calendar Grid -->
                            <div class="calendar-grid">
                                <!-- Days of Week Header -->
                                <div class="calendar-header-row">
                                    <div v-for="day in daysOfWeek" :key="day" class="day-header">
                                        {{ day }}
                                    </div>
                                </div>

                                <!-- Calendar Days -->
                                <div v-for="day in calendarDays"
                                     :key="`${day.date}-${day.month}`"
                                     class="calendar-day"
                                     :class="{
                                     'other-month' : day.isOtherMonth,
                                     'today' : day.isToday,
                                     'has-slots' : day.availableSlots && day.availableSlots.length>
                                    0,
                                    'selected': day.isSelected,
                                    'past-date': day.isPast
                                    }"
                                    @click="selectDate(day)"
                                    >
                                    <span class="day-number">{{ day.dayNumber }}</span>
                                    <div v-if="day.availableSlots && day.availableSlots.length > 0" class="slot-indicator">
                                        {{ day.availableSlots.length }} slot
                                    </div>
                                </div>
                            </div>

                            <!-- Time Slots -->
                            <div v-if="selectedDate && availableTimeSlots.length" class="time-slots mt-4">
                                <h6>{{ formatDate(selectedDate.date) }} - Müsait Saatler</h6>
                                <div class="time-slots-grid">
                                    <button v-for="slot in availableTimeSlots"
                                            :key="slot.time"
                                            class="time-slot-btn"
                                            :class="{
                      active: selectedDateTime?.time === slot.time,
                      disabled: slot.isBooked
                    }"
                                            :disabled="slot.isBooked"
                                            @click="selectTimeSlot(slot)">
                                        {{ slot.time }}
                                        <span v-if="slot.isBooked" class="booked-indicator">
                                            <i class="fas fa-times"></i>
                                        </span>
                                    </button>
                                </div>
                            </div>

                            <!-- No Slots Available -->
                            <div v-else-if="selectedDate && availableTimeSlots.length === 0" class="no-slots mt-4">
                                <div class="text-center py-4">
                                    <i class="fas fa-calendar-times fa-3x text-muted mb-3"></i>
                                    <h6>Bu Tarihte Müsait Saat Yok</h6>
                                    <p class="text-muted">Lütfen başka bir tarih seçiniz.</p>
                                </div>
                            </div>

                            <!-- Instructions -->
                            <div v-if="!selectedDate" class="calendar-instructions mt-4">
                                <div class="alert alert-info">
                                    <h6><i class="fas fa-info-circle me-2"></i>Nasıl Randevu Alırım?</h6>
                                    <ol class="mb-0">
                                        <li>Önce bir seans türü seçin</li>
                                        <li>Takvimden müsait bir gün seçin (yeşil işaretli günler)</li>
                                        <li>O günün müsait saatlerinden birini seçin</li>
                                        <li>"Randevuyu Onayla" butonuna tıklayın</li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    import { ref, computed, onMounted, watch } from 'vue'
    import { useRoute, useRouter } from 'vue-router'
    import { useAuthStore } from '@/stores/auth'
    import { useToast } from 'vue-toastification'
    import contentService from '@/services/content'
    import appointmentService from '@/services/appointment'
    import dayjs from 'dayjs'

    export default {
        name: 'InterviewCalendarPage',
        setup() {
            const route = useRoute()
            const router = useRouter()
            const authStore = useAuthStore()
            const toast = useToast()

            // Reactive data
            const coach = ref(null)
            const sessionTypes = ref([])
            const selectedSessionType = ref(null)
            const currentMonth = ref(dayjs())
            const selectedDate = ref(null)
            const selectedDateTime = ref(null)
            const availableTimeSlots = ref([])
            const calendarData = ref({})
            const loading = ref(true)
            const error = ref(null)
            const isBooking = ref(false)

            // Days of week
            const daysOfWeek = ['Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt', 'Paz']

            // Computed properties
            const isAuthenticated = computed(() => authStore.isAuthenticated)

            const canGoPrevious = computed(() => {
                return currentMonth.value.isAfter(dayjs(), 'month')
            })

            const calendarDays = computed(() => {
                const startOfMonth = currentMonth.value.startOf('month')
                const endOfMonth = currentMonth.value.endOf('month')
                const startOfCalendar = startOfMonth.startOf('week').add(1, 'day') // Start from Monday
                const endOfCalendar = endOfMonth.endOf('week').add(1, 'day')

                const days = []
                let currentDay = startOfCalendar

                while (currentDay.isBefore(endOfCalendar) || currentDay.isSame(endOfCalendar)) {
                    const dayKey = currentDay.format('YYYY-MM-DD')
                    const monthData = calendarData.value[currentMonth.value.format('YYYY-MM')] || {}
                    const dayData = monthData[dayKey] || {}

                    days.push({
                        date: currentDay.toDate(),
                        dayNumber: currentDay.date(),
                        month: currentDay.month(),
                        isOtherMonth: !currentDay.isSame(currentMonth.value, 'month'),
                        isToday: currentDay.isSame(dayjs(), 'day'),
                        isSelected: selectedDate.value && currentDay.isSame(selectedDate.value.date, 'day'),
                        isPast: currentDay.isBefore(dayjs(), 'day'),
                        availableSlots: dayData.slots || []
                    })

                    currentDay = currentDay.add(1, 'day')
                }

                return days
            })

            // Methods
            const loadCoach = async () => {
                try {
                    const coachId = route.params.coachId
                    const response = await contentService.getCoach(coachId)
                    coach.value = response.data
                } catch (err) {
                    error.value = 'Koç bilgileri yüklenirken hata oluştu.'
                    console.error('Coach load error:', err)
                }
            }

            const loadSessionTypes = async () => {
                try {
                    const response = await contentService.getSessionTypes()
                    sessionTypes.value = response.data || [
                        { id: 1, name: 'Standart Seans', duration: 60, price: 500 },
                        { id: 2, name: 'Yoğun Seans', duration: 90, price: 700 }
                    ]

                    // Auto-select first session type
                    if (sessionTypes.value.length > 0) {
                        selectedSessionType.value = sessionTypes.value[0]
                    }
                } catch (err) {
                    console.error('Session types load error:', err)
                    // Use default session types
                    sessionTypes.value = [
                        { id: 1, name: 'Standart Seans', duration: 60, price: 500 },
                        { id: 2, name: 'Yoğun Seans', duration: 90, price: 700 }
                    ]
                    selectedSessionType.value = sessionTypes.value[0]
                }
            }

            const loadCalendarData = async () => {
                if (!coach.value) return

                try {
                    const monthKey = currentMonth.value.format('YYYY-MM')

                    if (!calendarData.value[monthKey]) {
                        const response = await appointmentService.getCoachCalendar(
                            coach.value.id,
                            currentMonth.value.month() + 1,
                            currentMonth.value.year()
                        )

                        calendarData.value[monthKey] = response.data || {}
                    }
                } catch (err) {
                    console.error('Calendar data load error:', err)
                }
            }

            const loadTimeSlots = async (date) => {
                if (!coach.value || !date) return

                try {
                    const dateStr = dayjs(date).format('YYYY-MM-DD')
                    const response = await appointmentService.getAvailableSlots(coach.value.id, dateStr)
                    availableTimeSlots.value = response.data || []
                } catch (err) {
                    console.error('Time slots load error:', err)
                    availableTimeSlots.value = []
                }
            }

            const selectSessionType = (sessionType) => {
                selectedSessionType.value = sessionType
                // Clear date/time selection when session type changes
                selectedDate.value = null
                selectedDateTime.value = null
                availableTimeSlots.value = []
            }

            const selectDate = async (day) => {
                if (day.isPast || day.isOtherMonth || !day.availableSlots?.length) return

                selectedDate.value = day
                selectedDateTime.value = null

                await loadTimeSlots(day.date)
            }

            const selectTimeSlot = (slot) => {
                if (slot.isBooked) return

                selectedDateTime.value = {
                    date: selectedDate.value.date,
                    time: slot.time,
                    slot: slot
                }
            }

            const previousMonth = async () => {
                if (!canGoPrevious.value) return

                currentMonth.value = currentMonth.value.subtract(1, 'month')
                await loadCalendarData()
            }

            const nextMonth = async () => {
                currentMonth.value = currentMonth.value.add(1, 'month')
                await loadCalendarData()
            }

            const proceedToBooking = async () => {
                if (!isAuthenticated.value) {
                    toast.warning('Randevu almak için giriş yapmanız gerekiyor.')
                    router.push({
                        name: 'Login',
                        query: { returnUrl: route.fullPath }
                    })
                    return
                }

                if (!selectedSessionType.value || !selectedDateTime.value) {
                    toast.warning('Lütfen seans türü ve tarih/saat seçiniz.')
                    return
                }

                try {
                    isBooking.value = true

                    const appointmentData = {
                        coachId: coach.value.id,
                        sessionTypeId: selectedSessionType.value.id,
                        date: dayjs(selectedDateTime.value.date).format('YYYY-MM-DD'),
                        time: selectedDateTime.value.time,
                        duration: selectedSessionType.value.duration,
                        price: selectedSessionType.value.price
                    }

                    const response = await appointmentService.createAppointment(appointmentData)

                    if (response.data?.success) {
                        toast.success('Randevu başarıyla oluşturuldu!')

                        // Redirect to payment page
                        router.push({
                            name: 'Payment',
                            params: { appointmentId: response.data.appointmentId }
                        })
                    } else {
                        throw new Error(response.data?.message || 'Randevu oluşturulamadı.')
                    }

                } catch (err) {
                    toast.error(err.message || 'Randevu oluşturulurken hata oluştu.')
                    console.error('Booking error:', err)
                } finally {
                    isBooking.value = false
                }
            }

            const formatDate = (date) => {
                return dayjs(date).format('DD MMMM YYYY dddd')
            }

            const formatMonth = (date) => {
                return date.format('MMMM YYYY')
            }

            const formatPrice = (price) => {
                return `${price.toLocaleString('tr-TR')} ₺`
            }

            const handleImageError = (event) => {
                event.target.src = '/default-coach.jpg'
            }

            // Lifecycle
            onMounted(async () => {
                try {
                    loading.value = true

                    await loadCoach()
                    await loadSessionTypes()
                    await loadCalendarData()

                } catch (err) {
                    error.value = 'Sayfa yüklenirken hata oluştu.'
                    console.error('Page load error:', err)
                } finally {
                    loading.value = false
                }
            })

            // Watch for month changes
            watch(currentMonth, async () => {
                await loadCalendarData()
            })

            return {
                // Reactive data
                coach,
                sessionTypes,
                selectedSessionType,
                currentMonth,
                selectedDate,
                selectedDateTime,
                availableTimeSlots,
                loading,
                error,
                isBooking,
                daysOfWeek,

                // Computed
                isAuthenticated,
                canGoPrevious,
                calendarDays,

                // Methods
                selectSessionType,
                selectDate,
                selectTimeSlot,
                previousMonth,
                nextMonth,
                proceedToBooking,
                formatDate,
                formatMonth,
                formatPrice,
                handleImageError
            }
        }
    }
</script>

<style scoped>
    .coach-info-card,
    .calendar-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        height: fit-content;
    }

    .coach-header {
        display: flex;
        align-items: center;
        gap: 1rem;
        margin-bottom: 1.5rem;
    }

    .coach-avatar {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        object-fit: cover;
    }

    .coach-details h4 {
        margin-bottom: 0.5rem;
        color: #333;
    }

    .coach-title {
        color: #007bff;
        font-weight: 500;
        margin-bottom: 0.5rem;
    }

    .stars i {
        color: #ddd;
        font-size: 1rem;
    }

        .stars i.text-warning {
            color: #ffc107 !important;
        }

    .session-types h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .session-type {
        border: 2px solid #eee;
        border-radius: 10px;
        padding: 1rem;
        margin-bottom: 1rem;
        cursor: pointer;
        transition: all 0.3s ease;
    }

        .session-type:hover {
            border-color: #007bff;
        }

        .session-type.active {
            border-color: #007bff;
            background: rgba(0, 123, 255, 0.1);
        }

    .session-info h6 {
        margin-bottom: 0.5rem;
        color: #333;
    }

    .session-duration,
    .session-price {
        margin: 0;
        font-size: 0.9rem;
    }

    .session-duration {
        color: #666;
    }

    .session-price {
        color: #007bff;
        font-weight: 600;
    }

    .selected-session,
    .selected-datetime {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 1rem;
    }

        .selected-session h6,
        .selected-datetime h6 {
            color: #333;
            font-weight: 600;
            margin-bottom: 0.75rem;
        }

    .session-summary p,
    .datetime-summary p {
        margin-bottom: 0.25rem;
        font-size: 0.9rem;
    }

    .booking-note {
        text-align: center;
    }

    .calendar-header {
        margin-bottom: 2rem;
    }

        .calendar-header h5 {
            color: #333;
            font-weight: 600;
            margin-bottom: 0.5rem;
        }

    .month-navigation {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-top: 1rem;
        padding: 1rem;
        background: #f8f9fa;
        border-radius: 10px;
    }

    .current-month {
        margin: 0;
        color: #333;
        font-weight: 600;
    }

    .calendar-grid {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 1px;
        background: #dee2e6;
        border-radius: 10px;
        overflow: hidden;
    }

    .calendar-header-row {
        display: contents;
    }

    .day-header {
        background: #007bff;
        color: white;
        padding: 1rem;
        text-align: center;
        font-weight: 600;
        font-size: 0.9rem;
    }

    .calendar-day {
        background: white;
        padding: 1rem;
        min-height: 80px;
        cursor: pointer;
        position: relative;
        transition: all 0.3s ease;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }

        .calendar-day:hover:not(.past-date):not(.other-month) {
            background: #f8f9fa;
        }

        .calendar-day.other-month {
            background: #f8f9fa;
            color: #ccc;
            cursor: not-allowed;
        }

        .calendar-day.past-date {
            background: #f8f9fa;
            color: #ccc;
            cursor: not-allowed;
        }

        .calendar-day.today {
            background: #e3f2fd;
        }

        .calendar-day.has-slots {
            background: #e8f5e8;
            border: 2px solid #28a745;
        }

        .calendar-day.selected {
            background: #007bff;
            color: white;
        }

    .day-number {
        font-weight: 600;
        font-size: 1.1rem;
    }

    .slot-indicator {
        position: absolute;
        bottom: 5px;
        font-size: 0.7rem;
        color: #28a745;
        font-weight: 500;
    }

    .calendar-day.selected .slot-indicator {
        color: white;
    }

    .time-slots h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .time-slots-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
        gap: 0.75rem;
    }

    .time-slot-btn {
        background: white;
        border: 2px solid #eee;
        border-radius: 8px;
        padding: 0.75rem;
        text-align: center;
        cursor: pointer;
        transition: all 0.3s ease;
        position: relative;
        font-weight: 500;
    }

        .time-slot-btn:hover:not(.disabled) {
            border-color: #007bff;
            background: #f8f9fa;
        }

        .time-slot-btn.active {
            background: #007bff;
            border-color: #007bff;
            color: white;
        }

        .time-slot-btn.disabled {
            background: #f8f9fa;
            border-color: #ddd;
            color: #999;
            cursor: not-allowed;
        }

    .booked-indicator {
        position: absolute;
        top: 5px;
        right: 5px;
        color: #dc3545;
        font-size: 0.8rem;
    }

    .no-slots {
        text-align: center;
        padding: 2rem;
        background: #f8f9fa;
        border-radius: 10px;
    }

    .calendar-instructions .alert {
        border-radius: 10px;
    }

    .calendar-instructions ol {
        text-align: left;
        padding-left: 1.5rem;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .coach-header {
            flex-direction: column;
            text-align: center;
        }

        .coach-info-card,
        .calendar-card {
            padding: 1.5rem;
        }

        .calendar-day {
            min-height: 60px;
            padding: 0.5rem;
        }

        .day-number {
            font-size: 1rem;
        }

        .time-slots-grid {
            grid-template-columns: repeat(auto-fill, minmax(80px, 1fr));
        }

        .time-slot-btn {
            padding: 0.5rem;
            font-size: 0.9rem;
        }

        .month-navigation {
            padding: 0.75rem;
        }

        .current-month {
            font-size: 1.1rem;
        }
    }

    @media (max-width: 576px) {
        .calendar-grid {
            gap: 0;
        }

        .calendar-day {
            min-height: 50px;
            padding: 0.25rem;
        }

        .day-header {
            padding: 0.5rem;
            font-size: 0.8rem;
        }

        .day-number {
            font-size: 0.9rem;
        }

        .slot-indicator {
            font-size: 0.6rem;
            bottom: 2px;
        }
    }

    /* Loading animations */
    @keyframes fadeInUp {
        from {
            opacity: 0;
            transform: translateY(30px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .coach-info-card,
    .calendar-card {
        animation: fadeInUp 0.6s ease-out;
    }

    /* Accessibility */
    .calendar-day:focus,
    .time-slot-btn:focus,
    .session-type:focus {
        outline: 2px solid #007bff;
        outline-offset: 2px;
    }

    /* Dark theme support */
    @media (prefers-color-scheme: dark) {
        .coach-info-card,
        .calendar-card {
            background: #2d3748;
            color: #e2e8f0;
        }

        .calendar-day {
            background: #4a5568;
            color: #e2e8f0;
        }

            .calendar-day.other-month,
            .calendar-day.past-date {
                background: #2d3748;
                color: #718096;
            }
    }
</style>