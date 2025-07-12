<template>
  <div class="appointments-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Randevularım</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item">
                  <router-link to="/hesabim/profilim" class="text-white-50">Hesabım</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Randevularım</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Appointments Content -->
    <section class="appointments-content py-5">
      <div class="container">
        <!-- Filter Tabs -->
        <div class="filter-tabs mb-4">
          <ul class="nav nav-pills justify-content-center">
            <li class="nav-item">
              <button 
                class="nav-link"
                :class="{ active: activeFilter === 'all' }"
                @click="setFilter('all')"
              >
                Tümü ({{ appointments.length }})
              </button>
            </li>
            <li class="nav-item">
              <button 
                class="nav-link"
                :class="{ active: activeFilter === 'upcoming' }"
                @click="setFilter('upcoming')"
              >
                Yaklaşan ({{ upcomingCount }})
              </button>
            </li>
            <li class="nav-item">
              <button 
                class="nav-link"
                :class="{ active: activeFilter === 'completed' }"
                @click="setFilter('completed')"
              >
                Tamamlanan ({{ completedCount }})
              </button>
            </li>
            <li class="nav-item">
              <button 
                class="nav-link"
                :class="{ active: activeFilter === 'cancelled' }"
                @click="setFilter('cancelled')"
              >
                İptal Edilen ({{ cancelledCount }})
              </button>
            </li>
          </ul>
        </div>

        <!-- Loading State -->
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
          <p class="mt-3">Randevularınız yükleniyor...</p>
        </div>

        <!-- Appointments List -->
        <div v-else-if="filteredAppointments.length > 0" class="appointments-list">
          <div 
            v-for="appointment in filteredAppointments" 
            :key="appointment.id"
            class="appointment-card"
          >
            <div class="appointment-header">
              <div class="appointment-status">
                <span 
                  class="status-badge"
                  :class="getStatusClass(appointment.status)"
                >
                  {{ getStatusText(appointment.status) }}
                </span>
              </div>
              <div class="appointment-date">
                <i class="fas fa-calendar me-2"></i>
                {{ formatDate(appointment.appointmentDate) }}
              </div>
            </div>

            <div class="appointment-body">
              <div class="row">
                <!-- Service Info -->
                <div class="col-lg-4">
                  <div class="service-info">
                    <h5>{{ appointment.serviceName }}</h5>
                    <p class="service-description">{{ appointment.serviceDescription }}</p>
                    <div class="service-meta">
                      <div class="duration">
                        <i class="fas fa-clock me-1"></i>
                        {{ appointment.duration || '60' }} dakika
                      </div>
                      <div class="price">
                        <i class="fas fa-tag me-1"></i>
                        {{ formatPrice(appointment.price) }}
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Coach Info -->
                <div class="col-lg-4">
                  <div class="coach-info">
                    <div class="coach-avatar">
                      <img 
                        :src="appointment.coachPhoto || '/default-coach.jpg'" 
                        :alt="appointment.coachName"
                        @error="handleImageError"
                      />
                    </div>
                    <div class="coach-details">
                      <h6>{{ appointment.coachName }}</h6>
                      <p class="coach-title">{{ appointment.coachTitle }}</p>
                      <div class="coach-rating">
                        <div class="stars">
                          <i v-for="star in 5" :key="star" 
                             :class="star <= (appointment.coachRating || 0) ? 'fas fa-star' : 'far fa-star'"></i>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Appointment Details -->
                <div class="col-lg-4">
                  <div class="appointment-details">
                    <div class="detail-item">
                      <i class="fas fa-calendar-day me-2"></i>
                      <span>{{ formatFullDate(appointment.appointmentDate) }}</span>
                    </div>
                    <div class="detail-item">
                      <i class="fas fa-clock me-2"></i>
                      <span>{{ appointment.appointmentTime }}</span>
                    </div>
                    <div class="detail-item">
                      <i class="fas fa-video me-2"></i>
                      <span>Online Görüşme</span>
                    </div>
                    <div v-if="appointment.meetingLink" class="detail-item">
                      <i class="fas fa-link me-2"></i>
                      <a :href="appointment.meetingLink" target="_blank" class="meeting-link">
                        Toplantı Bağlantısı
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Actions -->
            <div class="appointment-actions">
              <!-- Upcoming appointment actions -->
              <div v-if="appointment.status === 'upcoming'" class="action-buttons">
                <button 
                  v-if="canJoinMeeting(appointment)"
                  @click="joinMeeting(appointment)"
                  class="btn btn-success"
                >
                  <i class="fas fa-video me-1"></i>
                  Görüşmeye Katıl
                </button>
                <button 
                  v-if="canReschedule(appointment)"
                  @click="rescheduleAppointment(appointment)"
                  class="btn btn-outline-primary"
                >
                  <i class="fas fa-calendar-alt me-1"></i>
                  Yeniden Planla
                </button>
                <button 
                  v-if="canCancel(appointment)"
                  @click="cancelAppointment(appointment)"
                  class="btn btn-outline-danger"
                >
                  <i class="fas fa-times me-1"></i>
                  İptal Et
                </button>
              </div>

              <!-- Completed appointment actions -->
              <div v-if="appointment.status === 'completed'" class="action-buttons">
                <button 
                  v-if="!appointment.isReviewed"
                  @click="writeReview(appointment)"
                  class="btn btn-outline-primary"
                >
                  <i class="fas fa-star me-1"></i>
                  Değerlendir
                </button>
                <button 
                  @click="bookAgain(appointment)"
                  class="btn btn-primary"
                >
                  <i class="fas fa-redo me-1"></i>
                  Tekrar Randevu Al
                </button>
              </div>

              <!-- Cancelled appointment actions -->
              <div v-if="appointment.status === 'cancelled'" class="action-buttons">
                <button 
                  @click="bookAgain(appointment)"
                  class="btn btn-primary"
                >
                  <i class="fas fa-calendar-plus me-1"></i>
                  Yeni Randevu Al
                </button>
              </div>
            </div>
          </div>
        </div>

        <!-- Empty State -->
        <div v-else class="empty-state">
          <div class="empty-icon">
            <i class="fas fa-calendar-times"></i>
          </div>
          <h4>{{ getEmptyStateTitle() }}</h4>
          <p>{{ getEmptyStateMessage() }}</p>
          <router-link to="/online-kocluk-ve-atolye" class="btn btn-primary">
            <i class="fas fa-plus me-1"></i>
            İlk Randevunuzu Alın
          </router-link>
        </div>
      </div>
    </section>

    <!-- Cancel Confirmation Modal -->
    <div v-if="showCancelModal" class="modal-overlay" @click="closeCancelModal">
      <div class="modal-content" @click.stop>
        <h5>Randevu İptali</h5>
        <p>Bu randevuyu iptal etmek istediğinizden emin misiniz?</p>
        <div class="cancel-info">
          <div class="info-item">
            <strong>Hizmet:</strong> {{ selectedAppointment?.serviceName }}
          </div>
          <div class="info-item">
            <strong>Koç:</strong> {{ selectedAppointment?.coachName }}
          </div>
          <div class="info-item">
            <strong>Tarih:</strong> {{ formatFullDate(selectedAppointment?.appointmentDate) }}
          </div>
          <div class="info-item">
            <strong>Saat:</strong> {{ selectedAppointment?.appointmentTime }}
          </div>
        </div>
        <div class="alert alert-warning">
          <small>
            <i class="fas fa-info-circle me-1"></i>
            24 saatten az kala iptal edilen randevularda ücret iadesi yapılamaz.
          </small>
        </div>
        <div class="modal-actions">
          <button class="btn btn-secondary" @click="closeCancelModal">Vazgeç</button>
          <button class="btn btn-danger" @click="confirmCancel" :disabled="isCancelling">
            <span v-if="isCancelling" class="spinner-border spinner-border-sm me-2"></span>
            İptal Et
          </button>
        </div>
      </div>
    </div>

    <!-- Review Modal -->
    <div v-if="showReviewModal" class="modal-overlay" @click="closeReviewModal">
      <div class="modal-content review-modal" @click.stop>
        <h5>Koçunuzu Değerlendirin</h5>
        <div class="coach-info-review">
          <img 
            :src="selectedAppointment?.coachPhoto || '/default-coach.jpg'" 
            :alt="selectedAppointment?.coachName"
            class="coach-avatar-review"
          />
          <div>
            <h6>{{ selectedAppointment?.coachName }}</h6>
            <p>{{ selectedAppointment?.serviceName }}</p>
          </div>
        </div>
        
        <form @submit.prevent="submitReview">
          <div class="rating-section">
            <label class="form-label">Puanınız</label>
            <div class="star-rating">
              <button
                v-for="star in 5"
                :key="star"
                type="button"
                class="star-btn"
                :class="{ active: star <= reviewForm.rating }"
                @click="reviewForm.rating = star"
              >
                <i class="fas fa-star"></i>
              </button>
            </div>
          </div>
          
          <div class="form-group">
            <label class="form-label">Yorumunuz</label>
            <textarea
              v-model="reviewForm.comment"
              class="form-control"
              rows="4"
              placeholder="Deneyiminizi paylaşın..."
              required
            ></textarea>
          </div>
          
          <div class="modal-actions">
            <button type="button" class="btn btn-secondary" @click="closeReviewModal">İptal</button>
            <button type="submit" class="btn btn-primary" :disabled="isSubmittingReview">
              <span v-if="isSubmittingReview" class="spinner-border spinner-border-sm me-2"></span>
              Gönder
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'MyAppointmentsPage',
  setup() {
    const router = useRouter()
    const toast = useToast()
    const authStore = useAuthStore()
    
    const appointments = ref([])
    const loading = ref(true)
    const activeFilter = ref('all')
    const showCancelModal = ref(false)
    const showReviewModal = ref(false)
    const selectedAppointment = ref(null)
    const isCancelling = ref(false)
    const isSubmittingReview = ref(false)
    
    const reviewForm = ref({
      rating: 5,
      comment: ''
    })
    
    // Mock appointments data
    const mockAppointments = [
      {
        id: 1,
        serviceName: 'Kişisel Durum Analizi',
        serviceDescription: 'Mevcut durumunuzu analiz ederek gelişim alanlarınızı belirleyin.',
        coachName: 'Dr. Ayşe Demir',
        coachTitle: 'Kişisel Gelişim Uzmanı',
        coachPhoto: '/coaches/ayse-demir.jpg',
        coachRating: 4.8,
        appointmentDate: new Date(Date.now() + 2 * 24 * 60 * 60 * 1000), // 2 days from now
        appointmentTime: '14:00',
        duration: 60,
        price: 250,
        status: 'upcoming',
        meetingLink: 'https://meet.google.com/abc-def-ghi',
        isReviewed: false
      },
      {
        id: 2,
        serviceName: 'Kişisel Strateji Belirleme',
        serviceDescription: 'Hedeflerinize ulaşmak için kişiselleştirilmiş stratejiler geliştirin.',
        coachName: 'Mehmet Kaya',
        coachTitle: 'Yaşam Koçu',
        coachPhoto: '/coaches/mehmet-kaya.jpg',
        coachRating: 4.6,
        appointmentDate: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000), // 7 days ago
        appointmentTime: '10:00',
        duration: 90,
        price: 350,
        status: 'completed',
        isReviewed: false
      },
      {
        id: 3,
        serviceName: 'Bilinçaltı Uygulamaları',
        serviceDescription: 'Bilinçaltınızın gücünü keşfederek potansiyelinizi ortaya çıkarın.',
        coachName: 'Zehra Öztürk',
        coachTitle: 'Hipnoterapi Uzmanı',
        coachPhoto: '/coaches/zehra-ozturk.jpg',
        coachRating: 4.9,
        appointmentDate: new Date(Date.now() - 14 * 24 * 60 * 60 * 1000), // 14 days ago
        appointmentTime: '16:00',
        duration: 60,
        price: 300,
        status: 'cancelled',
        isReviewed: false
      }
    ]
    
    const filteredAppointments = computed(() => {
      if (activeFilter.value === 'all') return appointments.value
      return appointments.value.filter(app => app.status === activeFilter.value)
    })
    
    const upcomingCount = computed(() => 
      appointments.value.filter(app => app.status === 'upcoming').length
    )
    
    const completedCount = computed(() => 
      appointments.value.filter(app => app.status === 'completed').length
    )
    
    const cancelledCount = computed(() => 
      appointments.value.filter(app => app.status === 'cancelled').length
    )
    
    const loadAppointments = async () => {
      try {
        loading.value = true
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1000))
        appointments.value = mockAppointments
      } catch (error) {
        console.error('Error loading appointments:', error)
        toast.error('Randevular yüklenirken hata oluştu.')
      } finally {
        loading.value = false
      }
    }
    
    const setFilter = (filter) => {
      activeFilter.value = filter
    }
    
    const getStatusClass = (status) => {
      switch (status) {
        case 'upcoming': return 'status-upcoming'
        case 'completed': return 'status-completed'
        case 'cancelled': return 'status-cancelled'
        default: return 'status-pending'
      }
    }
    
    const getStatusText = (status) => {
      switch (status) {
        case 'upcoming': return 'Yaklaşan'
        case 'completed': return 'Tamamlandı'
        case 'cancelled': return 'İptal Edildi'
        default: return 'Beklemede'
      }
    }
    
    const formatDate = (date) => {
      if (!date) return ''
      return new Intl.DateTimeFormat('tr-TR', {
        day: 'numeric',
        month: 'long'
      }).format(new Date(date))
    }
    
    const formatFullDate = (date) => {
      if (!date) return ''
      return new Intl.DateTimeFormat('tr-TR', {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric'
      }).format(new Date(date))
    }
    
    const formatPrice = (price) => {
      if (!price) return '0 ₺'
      return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
      }).format(price)
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-coach.jpg'
    }
    
    const canJoinMeeting = (appointment) => {
      if (appointment.status !== 'upcoming') return false
      const appointmentTime = new Date(`${appointment.appointmentDate.toDateString()} ${appointment.appointmentTime}`)
      const now = new Date()
      const timeDiff = appointmentTime.getTime() - now.getTime()
      return timeDiff <= 15 * 60 * 1000 && timeDiff >= -60 * 60 * 1000 // 15 min before to 1 hour after
    }
    
    const canReschedule = (appointment) => {
      if (appointment.status !== 'upcoming') return false
      const appointmentTime = new Date(`${appointment.appointmentDate.toDateString()} ${appointment.appointmentTime}`)
      const now = new Date()
      return appointmentTime.getTime() - now.getTime() > 24 * 60 * 60 * 1000 // More than 24 hours
    }
    
    const canCancel = (appointment) => {
      if (appointment.status !== 'upcoming') return false
      const appointmentTime = new Date(`${appointment.appointmentDate.toDateString()} ${appointment.appointmentTime}`)
      const now = new Date()
      return appointmentTime.getTime() - now.getTime() > 2 * 60 * 60 * 1000 // More than 2 hours
    }
    
    const joinMeeting = (appointment) => {
      if (appointment.meetingLink) {
        window.open(appointment.meetingLink, '_blank')
      } else {
        toast.info('Toplantı bağlantısı henüz hazır değil.')
      }
    }
    
    const rescheduleAppointment = (appointment) => {
      router.push({
        name: 'InterviewCalendar',
        params: {
          id: appointment.serviceId || 1,
          title: appointment.serviceName.toLowerCase().replace(/\s+/g, '-'),
          coachId: appointment.coachId || 1,
          coachName: appointment.coachName.toLowerCase().replace(/\s+/g, '-')
        },
        query: {
          operation: 'reschedule',
          transactionId: appointment.id
        }
      })
    }
    
    const cancelAppointment = (appointment) => {
      selectedAppointment.value = appointment
      showCancelModal.value = true
    }
    
    const closeCancelModal = () => {
      showCancelModal.value = false
      selectedAppointment.value = null
    }
    
    const confirmCancel = async () => {
      if (!selectedAppointment.value) return
      
      isCancelling.value = true
      
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1500))
        
        // Update appointment status
        const appointment = appointments.value.find(app => app.id === selectedAppointment.value.id)
        if (appointment) {
          appointment.status = 'cancelled'
        }
        
        toast.success('Randevu başarıyla iptal edildi.')
        closeCancelModal()
      } catch (error) {
        toast.error('Randevu iptal edilirken hata oluştu.')
      } finally {
        isCancelling.value = false
      }
    }
    
    const writeReview = (appointment) => {
      selectedAppointment.value = appointment
      reviewForm.value = {
        rating: 5,
        comment: ''
      }
      showReviewModal.value = true
    }
    
    const closeReviewModal = () => {
      showReviewModal.value = false
      selectedAppointment.value = null
      reviewForm.value = {
        rating: 5,
        comment: ''
      }
    }
    
    const submitReview = async () => {
      if (!selectedAppointment.value || !reviewForm.value.comment.trim()) {
        toast.error('Lütfen yorumunuzu yazın.')
        return
      }
      
      isSubmittingReview.value = true
      
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1500))
        
        // Update appointment
        const appointment = appointments.value.find(app => app.id === selectedAppointment.value.id)
        if (appointment) {
          appointment.isReviewed = true
        }
        
        toast.success('Değerlendirmeniz başarıyla gönderildi.')
        closeReviewModal()
      } catch (error) {
        toast.error('Değerlendirme gönderilirken hata oluştu.')
      } finally {
        isSubmittingReview.value = false
      }
    }
    
    const bookAgain = (appointment) => {
      router.push({
        name: 'InterviewCalendar',
        params: {
          id: appointment.serviceId || 1,
          title: appointment.serviceName.toLowerCase().replace(/\s+/g, '-'),
          coachId: appointment.coachId || 1,
          coachName: appointment.coachName.toLowerCase().replace(/\s+/g, '-')
        }
      })
    }
    
    const getEmptyStateTitle = () => {
      switch (activeFilter.value) {
        case 'upcoming': return 'Yaklaşan randevunuz yok'
        case 'completed': return 'Tamamlanan randevunuz yok'
        case 'cancelled': return 'İptal edilen randevunuz yok'
        default: return 'Henüz randevunuz yok'
      }
    }
    
    const getEmptyStateMessage = () => {
      switch (activeFilter.value) {
        case 'upcoming': return 'Yeni bir randevu alarak kişisel gelişim yolculuğunuza devam edin.'
        case 'completed': return 'Daha önce tamamladığınız randevu bulunmuyor.'
        case 'cancelled': return 'İptal ettiğiniz randevu bulunmuyor.'
        default: return 'Uzman koçlarımızla randevu alarak kişisel gelişim yolculuğunuza başlayın.'
      }
    }
    
    onMounted(() => {
      loadAppointments()
    })
    
    return {
      appointments,
      loading,
      activeFilter,
      showCancelModal,
      showReviewModal,
      selectedAppointment,
      isCancelling,
      isSubmittingReview,
      reviewForm,
      filteredAppointments,
      upcomingCount,
      completedCount,
      cancelledCount,
      setFilter,
      getStatusClass,
      getStatusText,
      formatDate,
      formatFullDate,
      formatPrice,
      handleImageError,
      canJoinMeeting,
      canReschedule,
      canCancel,
      joinMeeting,
      rescheduleAppointment,
      cancelAppointment,
      closeCancelModal,
      confirmCancel,
      writeReview,
      closeReviewModal,
      submitReview,
      bookAgain,
      getEmptyStateTitle,
      getEmptyStateMessage
    }
  }
}
</script>

<style scoped>
.appointments-page {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.filter-tabs {
  background: white;
  border-radius: 15px;
  padding: 1rem;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.nav-pills .nav-link {
  border-radius: 25px;
  padding: 0.75rem 1.5rem;
  margin: 0 0.25rem;
  color: #6c757d;
  background: transparent;
  border: 2px solid transparent;
  font-weight: 500;
  transition: all 0.3s ease;
}

.nav-pills .nav-link:hover {
  color: #007bff;
  background: #f8f9fa;
}

.nav-pills .nav-link.active {
  color: white;
  background: #007bff;
  border-color: #007bff;
}

.appointment-card {
  background: white;
  border-radius: 15px;
  margin-bottom: 2rem;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  overflow: hidden;
  transition: all 0.3s ease;
}

.appointment-card:hover {
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
  transform: translateY(-2px);
}

.appointment-header {
  background: #f8f9fa;
  padding: 1rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #e9ecef;
}

.status-badge {
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.status-upcoming {
  background: #e3f2fd;
  color: #1976d2;
}

.status-completed {
  background: #e8f5e8;
  color: #2e7d32;
}

.status-cancelled {
  background: #ffebee;
  color: #c62828;
}

.appointment-date {
  color: #6c757d;
  font-weight: 500;
}

.appointment-body {
  padding: 1.5rem;
}

.service-info h5 {
  color: #2c3e50;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.service-description {
  color: #6c757d;
  margin-bottom: 1rem;
  line-height: 1.5;
}

.service-meta {
  display: flex;
  gap: 1rem;
  font-size: 0.9rem;
  color: #6c757d;
}

.coach-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.coach-avatar img {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
}

.coach-details h6 {
  margin-bottom: 0.25rem;
  font-weight: 600;
  color: #2c3e50;
}

.coach-title {
  color: #007bff;
  font-size: 0.9rem;
  margin-bottom: 0.5rem;
}

.coach-rating .stars {
  color: #ffc107;
  font-size: 0.9rem;
}

.appointment-details {
  background: #f8f9fa;
  border-radius: 10px;
  padding: 1rem;
}

.detail-item {
  display: flex;
  align-items: center;
  margin-bottom: 0.75rem;
  color: #495057;
  font-size: 0.9rem;
}

.detail-item:last-child {
  margin-bottom: 0;
}

.detail-item i {
  color: #007bff;
  width: 20px;
}

.meeting-link {
  color: #007bff;
  text-decoration: none;
  font-weight: 500;
}

.meeting-link:hover {
  text-decoration: underline;
}

.appointment-actions {
  background: #f8f9fa;
  padding: 1rem 1.5rem;
  border-top: 1px solid #e9ecef;
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.action-buttons .btn {
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.action-buttons .btn:hover {
  transform: translateY(-1px);
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
}

.empty-icon {
  font-size: 4rem;
  color: #6c757d;
  margin-bottom: 1.5rem;
}

.empty-state h4 {
  color: #2c3e50;
  margin-bottom: 1rem;
}

.empty-state p {
  color: #6c757d;
  margin-bottom: 2rem;
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}

.modal-content {
  background: white;
  border-radius: 15px;
  padding: 2rem;
  max-width: 500px;
  width: 90%;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

.review-modal {
  max-width: 600px;
}

.modal-content h5 {
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.cancel-info {
  background: #f8f9fa;
  border-radius: 10px;
  padding: 1rem;
  margin: 1rem 0;
}

.info-item {
  display: flex;
  justify-content: space-between;
  margin-bottom: 0.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #e9ecef;
}

.info-item:last-child {
  border-bottom: none;
  margin-bottom: 0;
}

.coach-info-review {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 10px;
}

.coach-avatar-review {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  object-fit: cover;
}

.rating-section {
  margin-bottom: 1.5rem;
}

.star-rating {
  display: flex;
  gap: 0.25rem;
  margin-top: 0.5rem;
}

.star-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: #ddd;
  cursor: pointer;
  transition: color 0.2s ease;
}

.star-btn.active,
.star-btn:hover {
  color: #ffc107;
}

.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 1.5rem;
}

/* Responsive */
@media (max-width: 768px) {
  .appointment-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.5rem;
  }
  
  .service-meta {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .coach-info {
    flex-direction: column;
    text-align: center;
  }
  
  .action-buttons {
    flex-direction: column;
  }
  
  .action-buttons .btn {
    width: 100%;
  }
  
  .modal-content {
    padding: 1.5rem;
  }
  
  .modal-actions {
    flex-direction: column;
  }
  
  .coach-info-review {
    flex-direction: column;
    text-align: center;
  }
}

/* Loading Animation */
.spinner-border {
  width: 3rem;
  height: 3rem;
}

/* Card Animation */
.appointment-card {
  animation: fadeInUp 0.6s ease forwards;
}

.appointment-card:nth-child(1) { animation-delay: 0.1s; }
.appointment-card:nth-child(2) { animation-delay: 0.2s; }
.appointment-card:nth-child(3) { animation-delay: 0.3s; }

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
</style>