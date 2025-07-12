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
                  <router-link to="/online-kocluk-ve-atolye" class="text-white-50">Online Koçluk</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Randevu Takvimi</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Calendar Content -->
    <section class="calendar-content py-5">
      <div class="container">
        <div class="row">
          <!-- Service & Coach Info -->
          <div class="col-lg-4 mb-4">
            <div class="booking-info-card">
              <h4>Randevu Bilgileri</h4>
              
              <!-- Service Info -->
              <div class="service-info mb-4">
                <h6>Hizmet</h6>
                <div class="service-card">
                  <h5>{{ selectedService?.title }}</h5>
                  <p class="text-muted">{{ selectedService?.description }}</p>
                  <div class="price">
                    <span class="amount">{{ formatPrice(selectedService?.price) }}</span>
                  </div>
                </div>
              </div>
              
              <!-- Coach Info -->
              <div class="coach-info mb-4">
                <h6>Koç</h6>
                <div class="coach-card">
                  <div class="coach-avatar">
                    <img 
                      :src="selectedCoach?.profilePhoto || '/default-coach.jpg'" 
                      :alt="selectedCoach?.fullName"
                      @error="handleImageError"
                    />
                  </div>
                  <div class="coach-details">
                    <h5>{{ selectedCoach?.fullName }}</h5>
                    <p class="coach-title">{{ selectedCoach?.title }}</p>
                    <div class="coach-rating">
                      <div class="stars">
                        <i v-for="star in 5" :key="star" 
                           :class="star <= (selectedCoach?.rating || 0) ? 'fas fa-star' : 'far fa-star'"></i>
                      </div>
                      <span>({{ selectedCoach?.reviewCount || 0 }} değerlendirme)</span>
                    </div>
                  </div>
                </div>
              </div>
              
              <!-- Selected Date/Time -->
              <div v-if="selectedSlot" class="selected-slot mb-4">
                <h6>Seçilen Randevu</h6>
                <div class="slot-card selected">
                  <div class="slot-date">
                    <i class="fas fa-calendar me-2"></i>
                    {{ formatDate(selectedSlot.date) }}
                  </div>
                  <div class="slot-time">
                    <i class="fas fa-clock me-2"></i>
                    {{ selectedSlot.time }}
                  </div>
                </div>
              </div>
              
              <!-- Book Button -->
              <button 
                v-if="selectedSlot"
                @click="confirmBooking"
                class="btn btn-success w-100"
                :disabled="isBooking"
              >
                <span v-if="isBooking" class="spinner-border spinner-border-sm me-2"></span>
                Randevuyu Onayla
              </button>
            </div>
          </div>

          <!-- Calendar -->
          <div class="col-lg-8">
            <div class="calendar-container">
              <div class="calendar-header">
                <h4>Müsait Randevular</h4>
                <p class="text-muted">Lütfen müsait bir tarih ve saat seçiniz</p>
              </div>
              
              <!-- Month Navigation -->
              <div class="month-navigation">
                <button 
                  @click="previousMonth"
                  class="btn btn-outline-primary"
                  :disabled="!canGoPrevious"
                >
                  <i class="fas fa-chevron-left"></i>
                </button>
                <h5 class="current-month">
                  {{ formatMonth(currentMonth) }}
                </h5>
                <button 
                  @click="nextMonth"
                  class="btn btn-outline-primary"
                >
                  <i class="fas fa-chevron-right"></i>
                </button>
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
                <div 
                  v-for="day in calendarDays" 
                  :key="`${day.date}-${day.month}`"
                  class="calendar-day"
                  :class="{
                    'other-month': day.isOtherMonth,
                    'today': day.isToday,
                    'has-slots': day.availableSlots && day.availableSlots.length > 0,
                    'selected': day.isSelected
                  }"
                  @click="selectDay(day)"
                >
                  <span class="day-number">{{ day.date }}</span>
                  <div v-if="day.availableSlots && day.availableSlots.length > 0" class="slot-count">
                    {{ day.availableSlots.length }} müsait
                  </div>
                </div>
              </div>
              
              <!-- Available Time Slots -->
              <div v-if="selectedDay" class="time-slots-section">
                <h6>{{ formatDate(selectedDay.fullDate) }} - Müsait Saatler</h6>
                <div class="time-slots-grid">
                  <button
                    v-for="slot in selectedDay.availableSlots"
                    :key="slot.id"
                    class="time-slot"
                    :class="{ 
                      'selected': selectedSlot && selectedSlot.id === slot.id,
                      'full': slot.isFull 
                    }"
                    :disabled="slot.isFull"
                    @click="selectTimeSlot(slot, selectedDay.fullDate)"
                  >
                    <div class="slot-time">{{ slot.time }}</div>
                    <div class="slot-info">
                      <span v-if="slot.isFull" class="text-danger">Dolu</span>
                      <span v-else class="text-success">Müsait</span>
                    </div>
                  </button>
                </div>
              </div>
              
              <!-- Loading State -->
              <div v-if="loadingSlots" class="loading-state">
                <div class="spinner-border text-primary"></div>
                <p>Müsait randevular yükleniyor...</p>
              </div>
              
              <!-- No Slots Available -->
              <div v-if="!loadingSlots && selectedDay && (!selectedDay.availableSlots || selectedDay.availableSlots.length === 0)" class="no-slots">
                <i class="fas fa-calendar-times"></i>
                <p>Bu tarih için müsait randevu bulunmamaktadır.</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Booking Confirmation Modal -->
    <div v-if="showConfirmModal" class="modal-overlay" @click="closeConfirmModal">
      <div class="modal-content" @click.stop>
        <h5>Randevu Onayı</h5>
        <div class="booking-summary">
          <div class="summary-item">
            <strong>Hizmet:</strong> {{ selectedService?.title }}
          </div>
          <div class="summary-item">
            <strong>Koç:</strong> {{ selectedCoach?.fullName }}
          </div>
          <div class="summary-item">
            <strong>Tarih:</strong> {{ formatDate(selectedSlot?.date) }}
          </div>
          <div class="summary-item">
            <strong>Saat:</strong> {{ selectedSlot?.time }}
          </div>
          <div class="summary-item">
            <strong>Ücret:</strong> {{ formatPrice(selectedService?.price) }}
          </div>
        </div>
        <p class="text-muted mt-3">
          Randevunuzu onaylamak için aşağıdaki butona tıklayın. Ödeme sayfasına yönlendirileceksiniz.
        </p>
        <div class="modal-actions">
          <button class="btn btn-secondary" @click="closeConfirmModal">İptal</button>
          <button class="btn btn-success" @click="proceedToPayment" :disabled="isBooking">
            <span v-if="isBooking" class="spinner-border spinner-border-sm me-2"></span>
            Onayla ve Ödeme Yap
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import coachesService from '@/services/coaches'
import contentService from '@/services/content'

export default {
  name: 'InterviewCalendarPage',
  setup() {
    const route = useRoute()
    const router = useRouter()
    const toast = useToast()
    
    const selectedService = ref(null)
    const selectedCoach = ref(null)
    const currentMonth = ref(new Date())
    const selectedDay = ref(null)
    const selectedSlot = ref(null)
    const availableSlots = ref([])
    const loadingSlots = ref(false)
    const isBooking = ref(false)
    const showConfirmModal = ref(false)
    
    const daysOfWeek = ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt']
    
    const canGoPrevious = computed(() => {
      const today = new Date()
      const firstDayOfMonth = new Date(currentMonth.value.getFullYear(), currentMonth.value.getMonth(), 1)
      return firstDayOfMonth > today
    })
    
    const calendarDays = computed(() => {
      const year = currentMonth.value.getFullYear()
      const month = currentMonth.value.getMonth()
      const firstDay = new Date(year, month, 1)
      const lastDay = new Date(year, month + 1, 0)
      const startDate = new Date(firstDay)
      const endDate = new Date(lastDay)
      
      // Adjust to start from Sunday
      startDate.setDate(startDate.getDate() - startDate.getDay())
      endDate.setDate(endDate.getDate() + (6 - endDate.getDay()))
      
      const days = []
      const current = new Date(startDate)
      
      while (current <= endDate) {
        const daySlots = availableSlots.value.filter(slot => {
          const slotDate = new Date(slot.date)
          return slotDate.toDateString() === current.toDateString()
        })
        
        days.push({
          date: current.getDate(),
          month: current.getMonth(),
          year: current.getFullYear(),
          fullDate: new Date(current),
          isOtherMonth: current.getMonth() !== month,
          isToday: current.toDateString() === new Date().toDateString(),
          isSelected: selectedDay.value && current.toDateString() === selectedDay.value.fullDate.toDateString(),
          availableSlots: daySlots
        })
        
        current.setDate(current.getDate() + 1)
      }
      
      return days
    })
    
    const loadServiceAndCoach = async () => {
      try {
        // Load service details
        const serviceResponse = await contentService.getProductDetail(route.params.id, route.params.title)
        selectedService.value = serviceResponse.selectedProduct
        
        // Load coach details
        const coachResponse = await coachesService.getCoachDetail(
          route.params.id,
          route.params.title,
          route.params.coachId,
          route.params.coachName
        )
        selectedCoach.value = coachResponse.coach
      } catch (error) {
        console.error('Error loading service and coach:', error)
        toast.error('Hizmet veya koç bilgileri yüklenirken hata oluştu.')
      }
    }
    
    const loadAvailableSlots = async () => {
      if (!selectedCoach.value || !selectedService.value) return
      
      loadingSlots.value = true
      
      try {
        const startDate = new Date(currentMonth.value.getFullYear(), currentMonth.value.getMonth(), 1)
        const response = await coachesService.getAvailableSlots(
          selectedCoach.value.id,
          selectedService.value.title,
          startDate
        )
        
        // Transform response to expected format
        availableSlots.value = response.map(slot => ({
          id: slot.id,
          date: slot.availableDate,
          time: slot.time || '09:00',
          isFull: slot.coachIsFull || false
        }))
      } catch (error) {
        console.error('Error loading available slots:', error)
        toast.error('Müsait randevular yüklenirken hata oluştu.')
        availableSlots.value = []
      } finally {
        loadingSlots.value = false
      }
    }
    
    const formatPrice = (price) => {
      if (!price) return '0 ₺'
      return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
      }).format(price)
    }
    
    const formatDate = (date) => {
      if (!date) return ''
      return new Intl.DateTimeFormat('tr-TR', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      }).format(new Date(date))
    }
    
    const formatMonth = (date) => {
      return new Intl.DateTimeFormat('tr-TR', {
        year: 'numeric',
        month: 'long'
      }).format(date)
    }
    
    const previousMonth = () => {
      currentMonth.value = new Date(currentMonth.value.getFullYear(), currentMonth.value.getMonth() - 1)
    }
    
    const nextMonth = () => {
      currentMonth.value = new Date(currentMonth.value.getFullYear(), currentMonth.value.getMonth() + 1)
    }
    
    const selectDay = (day) => {
      if (day.isOtherMonth || !day.availableSlots || day.availableSlots.length === 0) return
      
      selectedDay.value = day
      selectedSlot.value = null
    }
    
    const selectTimeSlot = (slot, date) => {
      if (slot.isFull) return
      
      selectedSlot.value = {
        ...slot,
        date: date
      }
    }
    
    const confirmBooking = () => {
      showConfirmModal.value = true
    }
    
    const closeConfirmModal = () => {
      showConfirmModal.value = false
    }
    
    const proceedToPayment = async () => {
      isBooking.value = true
      
      try {
        const bookingData = {
          coachId: selectedCoach.value.id,
          productId: selectedService.value.id,
          coachCalendarId: selectedSlot.value.id,
          date: selectedSlot.value.date,
          time: selectedSlot.value.time
        }
        
        const response = await coachesService.bookInterview(bookingData)
        
        if (response.success) {
          toast.success('Randevu başarıyla oluşturuldu!')
          router.push('/hesabim/randevularim')
        } else {
          toast.error(response.message || 'Randevu oluşturulurken hata oluştu.')
        }
      } catch (error) {
        console.error('Error booking interview:', error)
        toast.error('Randevu oluşturulurken hata oluştu.')
      } finally {
        isBooking.value = false
        closeConfirmModal()
      }
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-coach.jpg'
    }