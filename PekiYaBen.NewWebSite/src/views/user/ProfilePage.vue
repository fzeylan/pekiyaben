<template>
  <div class="profile-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Profilim</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Profilim</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Profile Content -->
    <section class="profile-content py-5">
      <div class="container">
        <div class="row">
          <!-- Sidebar -->
          <div class="col-lg-3 mb-4">
            <div class="profile-sidebar">
              <div class="user-info-card">
                <div class="user-avatar">
                  <img 
                    :src="userAvatar" 
                    :alt="userFullName"
                    @error="handleAvatarError"
                  />
                  <button class="avatar-upload-btn" @click="triggerFileUpload">
                    <i class="fas fa-camera"></i>
                  </button>
                  <input 
                    ref="fileInput"
                    type="file" 
                    accept="image/*" 
                    @change="handleAvatarUpload"
                    style="display: none;"
                  />
                </div>
                <h5 class="user-name">{{ userFullName }}</h5>
                <p class="user-email">{{ userEmail }}</p>
              </div>
              
              <nav class="profile-nav">
                <ul class="nav flex-column">
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link" 
                      :class="{ active: activeTab === 'profile' }"
                      @click.prevent="activeTab = 'profile'"
                    >
                      <i class="fas fa-user me-2"></i>
                      Kişisel Bilgiler
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link" 
                      :class="{ active: activeTab === 'appointments' }"
                      @click.prevent="activeTab = 'appointments'"
                    >
                      <i class="fas fa-calendar-alt me-2"></i>
                      Randevularım
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link" 
                      :class="{ active: activeTab === 'orders' }"
                      @click.prevent="activeTab = 'orders'"
                    >
                      <i class="fas fa-shopping-bag me-2"></i>
                      Siparişlerim
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link" 
                      :class="{ active: activeTab === 'password' }"
                      @click.prevent="activeTab = 'password'"
                    >
                      <i class="fas fa-lock me-2"></i>
                      Şifre Değiştir
                    </a>
                  </li>
                </ul>
              </nav>
            </div>
          </div>

          <!-- Main Content -->
          <div class="col-lg-9">
            <!-- Profile Tab -->
            <div v-if="activeTab === 'profile'" class="tab-content">
              <div class="card">
                <div class="card-header">
                  <h5 class="mb-0">Kişisel Bilgiler</h5>
                </div>
                <div class="card-body">
                  <form @submit.prevent="updateProfile">
                    <div class="row">
                      <div class="col-md-6 mb-3">
                        <label for="firstName" class="form-label">Ad *</label>
                        <input
                          id="firstName"
                          v-model="profileForm.firstName"
                          type="text"
                          class="form-control"
                          :class="{ 'is-invalid': profileErrors.firstName }"
                          required
                        />
                        <div v-if="profileErrors.firstName" class="invalid-feedback">
                          {{ profileErrors.firstName }}
                        </div>
                      </div>
                      <div class="col-md-6 mb-3">
                        <label for="lastName" class="form-label">Soyad *</label>
                        <input
                          id="lastName"
                          v-model="profileForm.lastName"
                          type="text"
                          class="form-control"
                          :class="{ 'is-invalid': profileErrors.lastName }"
                          required
                        />
                        <div v-if="profileErrors.lastName" class="invalid-feedback">
                          {{ profileErrors.lastName }}
                        </div>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-md-6 mb-3">
                        <label for="email" class="form-label">E-posta *</label>
                        <input
                          id="email"
                          v-model="profileForm.email"
                          type="email"
                          class="form-control"
                          :class="{ 'is-invalid': profileErrors.email }"
                          required
                        />
                        <div v-if="profileErrors.email" class="invalid-feedback">
                          {{ profileErrors.email }}
                        </div>
                      </div>
                      <div class="col-md-6 mb-3">
                        <label for="phone" class="form-label">Telefon</label>
                        <input
                          id="phone"
                          v-model="profileForm.phone"
                          type="tel"
                          class="form-control"
                          :class="{ 'is-invalid': profileErrors.phone }"
                        />
                        <div v-if="profileErrors.phone" class="invalid-feedback">
                          {{ profileErrors.phone }}
                        </div>
                      </div>
                    </div>
                    <div class="mb-3">
                      <label for="birthDate" class="form-label">Doğum Tarihi</label>
                      <input
                        id="birthDate"
                        v-model="profileForm.birthDate"
                        type="date"
                        class="form-control"
                        :class="{ 'is-invalid': profileErrors.birthDate }"
                      />
                      <div v-if="profileErrors.birthDate" class="invalid-feedback">
                        {{ profileErrors.birthDate }}
                      </div>
                    </div>
                    <div class="mb-3">
                      <label for="gender" class="form-label">Cinsiyet</label>
                      <select
                        id="gender"
                        v-model="profileForm.gender"
                        class="form-select"
                        :class="{ 'is-invalid': profileErrors.gender }"
                      >
                        <option value="">Seçiniz</option>
                        <option value="male">Erkek</option>
                        <option value="female">Kadın</option>
                        <option value="other">Diğer</option>
                      </select>
                      <div v-if="profileErrors.gender" class="invalid-feedback">
                        {{ profileErrors.gender }}
                      </div>
                    </div>
                    <div class="mb-3">
                      <label for="bio" class="form-label">Hakkımda</label>
                      <textarea
                        id="bio"
                        v-model="profileForm.bio"
                        class="form-control"
                        rows="4"
                        :class="{ 'is-invalid': profileErrors.bio }"
                        placeholder="Kendiniz hakkında kısa bir açıklama yazın..."
                      ></textarea>
                      <div v-if="profileErrors.bio" class="invalid-feedback">
                        {{ profileErrors.bio }}
                      </div>
                    </div>
                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                      <button
                        type="submit"
                        class="btn btn-primary"
                        :disabled="isUpdatingProfile"
                      >
                        <span v-if="isUpdatingProfile" class="spinner-border spinner-border-sm me-2"></span>
                        Güncelle
                      </button>
                    </div>
                  </form>
                </div>
              </div>
            </div>

            <!-- Appointments Tab -->
            <div v-else-if="activeTab === 'appointments'" class="tab-content">
              <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <h5 class="mb-0">Randevularım</h5>
                  <router-link to="/randevu-al" class="btn btn-primary btn-sm">
                    Yeni Randevu
                  </router-link>
                </div>
                <div class="card-body">
                  <div v-if="loadingAppointments" class="text-center py-4">
                    <div class="spinner-border text-primary" role="status">
                      <span class="visually-hidden">Yükleniyor...</span>
                    </div>
                  </div>
                  <div v-else-if="appointments.length === 0" class="text-center py-4">
                    <i class="fas fa-calendar-times fa-3x text-muted mb-3"></i>
                    <p class="text-muted">Henüz randevunuz bulunmamaktadır.</p>
                    <router-link to="/randevu-al" class="btn btn-primary">
                      İlk Randevunuzu Alın
                    </router-link>
                  </div>
                  <div v-else class="appointments-list">
                    <div
                      v-for="appointment in appointments"
                      :key="appointment.id"
                      class="appointment-card mb-3"
                    >
                      <div class="row align-items-center">
                        <div class="col-md-3">
                          <div class="appointment-date">
                            <i class="fas fa-calendar-alt text-primary me-2"></i>
                            {{ formatDate(appointment.date) }}
                          </div>
                          <div class="appointment-time">
                            <i class="fas fa-clock text-muted me-2"></i>
                            {{ appointment.time }}
                          </div>
                        </div>
                        <div class="col-md-4">
                          <div class="coach-info">
                            <h6 class="mb-1">{{ appointment.coachName }}</h6>
                            <p class="text-muted mb-0">{{ appointment.serviceName }}</p>
                          </div>
                        </div>
                        <div class="col-md-3">
                          <span
                            class="badge"
                            :class="getStatusBadgeClass(appointment.status)"
                          >
                            {{ getStatusText(appointment.status) }}
                          </span>
                        </div>
                        <div class="col-md-2 text-end">
                          <div class="dropdown">
                            <button
                              class="btn btn-outline-secondary btn-sm dropdown-toggle"
                              type="button"
                              :id="`appointment-${appointment.id}`"
                              data-bs-toggle="dropdown"
                            >
                              İşlemler
                            </button>
                            <ul class="dropdown-menu">
                              <li>
                                <a
                                  class="dropdown-item"
                                  href="#"
                                  @click.prevent="viewAppointment(appointment)"
                                >
                                  Detayları Görüntüle
                                </a>
                              </li>
                              <li v-if="canCancelAppointment(appointment)">
                                <a
                                  class="dropdown-item text-danger"
                                  href="#"
                                  @click.prevent="cancelAppointment(appointment)"
                                >
                                  İptal Et
                                </a>
                              </li>
                            </ul>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Orders Tab -->
            <div v-else-if="activeTab === 'orders'" class="tab-content">
              <div class="card">
                <div class="card-header">
                  <h5 class="mb-0">Siparişlerim</h5>
                </div>
                <div class="card-body">
                  <div v-if="loadingOrders" class="text-center py-4">
                    <div class="spinner-border text-primary" role="status">
                      <span class="visually-hidden">Yükleniyor...</span>
                    </div>
                  </div>
                  <div v-else-if="orders.length === 0" class="text-center py-4">
                    <i class="fas fa-shopping-bag fa-3x text-muted mb-3"></i>
                    <p class="text-muted">Henüz siparişiniz bulunmamaktadır.</p>
                  </div>
                  <div v-else class="orders-list">
                    <div
                      v-for="order in orders"
                      :key="order.id"
                      class="order-card mb-3"
                    >
                      <div class="row align-items-center">
                        <div class="col-md-2">
                          <strong>#{{ order.orderNumber }}</strong>
                        </div>
                        <div class="col-md-3">
                          {{ formatDate(order.orderDate) }}
                        </div>
                        <div class="col-md-3">
                          {{ order.serviceName }}
                        </div>
                        <div class="col-md-2">
                          <span class="fw-bold">{{ order.amount }} ₺</span>
                        </div>
                        <div class="col-md-2">
                          <span
                            class="badge"
                            :class="getOrderStatusBadgeClass(order.status)"
                          >
                            {{ getOrderStatusText(order.status) }}
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Password Tab -->
            <div v-else-if="activeTab === 'password'" class="tab-content">
              <div class="card">
                <div class="card-header">
                  <h5 class="mb-0">Şifre Değiştir</h5>
                </div>
                <div class="card-body">
                  <form @submit.prevent="updatePassword">
                    <div class="mb-3">
                      <label for="currentPassword" class="form-label">Mevcut Şifre *</label>
                      <input
                        id="currentPassword"
                        v-model="passwordForm.currentPassword"
                        type="password"
                        class="form-control"
                        :class="{ 'is-invalid': passwordErrors.currentPassword }"
                        required
                      />
                      <div v-if="passwordErrors.currentPassword" class="invalid-feedback">
                        {{ passwordErrors.currentPassword }}
                      </div>
                    </div>
                    <div class="mb-3">
                      <label for="newPassword" class="form-label">Yeni Şifre *</label>
                      <input
                        id="newPassword"
                        v-model="passwordForm.newPassword"
                        type="password"
                        class="form-control"
                        :class="{ 'is-invalid': passwordErrors.newPassword }"
                        required
                        minlength="6"
                      />
                      <div v-if="passwordErrors.newPassword" class="invalid-feedback">
                        {{ passwordErrors.newPassword }}
                      </div>
                      <div class="form-text">En az 6 karakter olmalıdır.</div>
                    </div>
                    <div class="mb-3">
                      <label for="confirmPassword" class="form-label">Yeni Şifre Tekrar *</label>
                      <input
                        id="confirmPassword"
                        v-model="passwordForm.confirmPassword"
                        type="password"
                        class="form-control"
                        :class="{ 'is-invalid': passwordErrors.confirmPassword }"
                        required
                      />
                      <div v-if="passwordErrors.confirmPassword" class="invalid-feedback">
                        {{ passwordErrors.confirmPassword }}
                      </div>
                    </div>
                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                      <button
                        type="submit"
                        class="btn btn-primary"
                        :disabled="isUpdatingPassword"
                      >
                        <span v-if="isUpdatingPassword" class="spinner-border spinner-border-sm me-2"></span>
                        Şifreyi Güncelle
                      </button>
                    </div>
                  </form>
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
import { ref, reactive, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useToast } from 'vue-toastification'
import userService from '@/services/user'
import appointmentService from '@/services/appointment'
import dayjs from 'dayjs'

export default {
  name: 'ProfilePage',
  setup() {
    const authStore = useAuthStore()
    const toast = useToast()
    
    // Reactive data
    const activeTab = ref('profile')
    const fileInput = ref(null)
    const isUpdatingProfile = ref(false)
    const isUpdatingPassword = ref(false)
    const loadingAppointments = ref(false)
    const loadingOrders = ref(false)
    const appointments = ref([])
    const orders = ref([])
    
    // Forms
    const profileForm = reactive({
      firstName: '',
      lastName: '',
      email: '',
      phone: '',
      birthDate: '',
      gender: '',
      bio: ''
    })
    
    const passwordForm = reactive({
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    })
    
    // Form errors
    const profileErrors = ref({})
    const passwordErrors = ref({})
    
    // Computed properties
    const userFullName = computed(() => `${profileForm.firstName} ${profileForm.lastName}`)
    const userEmail = computed(() => profileForm.email)
    const userAvatar = computed(() => authStore.user?.avatar || '/default-avatar.png')
    
    // Methods
    const loadUserProfile = async () => {
      try {
        const response = await userService.getProfile()
        Object.assign(profileForm, response.data)
      } catch (error) {
        toast.error('Profil bilgileri yüklenirken hata oluştu.')
        console.error('Profile load error:', error)
      }
    }
    
    const updateProfile = async () => {
      try {
        profileErrors.value = {}
        isUpdatingProfile.value = true
        
        await userService.updateProfile(profileForm)
        toast.success('Profil bilgileriniz başarıyla güncellendi!')
        
        // Update auth store
        authStore.updateUser(profileForm)
      } catch (error) {
        if (error.response?.data?.errors) {
          profileErrors.value = error.response.data.errors
        } else {
          toast.error('Profil güncellenirken hata oluştu.')
        }
        console.error('Profile update error:', error)
      } finally {
        isUpdatingProfile.value = false
      }
    }
    
    const updatePassword = async () => {
      try {
        passwordErrors.value = {}
        
        // Validate passwords match
        if (passwordForm.newPassword !== passwordForm.confirmPassword) {
          passwordErrors.value.confirmPassword = 'Şifreler eşleşmiyor.'
          return
        }
        
        isUpdatingPassword.value = true
        
        await userService.updatePassword({
          currentPassword: passwordForm.currentPassword,
          newPassword: passwordForm.newPassword
        })
        
        toast.success('Şifreniz başarıyla güncellendi!')
        
        // Reset form
        Object.assign(passwordForm, {
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        })
      } catch (error) {
        if (error.response?.data?.errors) {
          passwordErrors.value = error.response.data.errors
        } else {
          toast.error('Şifre güncellenirken hata oluştu.')
        }
        console.error('Password update error:', error)
      } finally {
        isUpdatingPassword.value = false
      }
    }
    
    const loadAppointments = async () => {
      try {
        loadingAppointments.value = true
        const response = await appointmentService.getUserAppointments()
        appointments.value = response.data
      } catch (error) {
        toast.error('Randevular yüklenirken hata oluştu.')
        console.error('Appointments load error:', error)
      } finally {
        loadingAppointments.value = false
      }
    }
    
    const loadOrders = async () => {
      try {
        loadingOrders.value = true
        const response = await userService.getOrders()
        orders.value = response.data
      } catch (error) {
        toast.error('Siparişler yüklenirken hata oluştu.')
        console.error('Orders load error:', error)
      } finally {
        loadingOrders.value = false
      }
    }
    
    const triggerFileUpload = () => {
      fileInput.value?.click()
    }
    
    const handleAvatarUpload = async (event) => {
      const file = event.target.files[0]
      if (!file) return
      
      // Validate file type
      if (!file.type.startsWith('image/')) {
        toast.error('Lütfen geçerli bir resim dosyası seçin.')
        return
      }
      
      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        toast.error('Resim boyutu 5MB\'dan küçük olmalıdır.')
        return
      }
      
      try {
        const formData = new FormData()
        formData.append('avatar', file)
        
        await userService.updateAvatar(formData)
        toast.success('Profil fotoğrafınız güncellendi!')
        
        // Reload user data
        await authStore.fetchUser()
      } catch (error) {
        toast.error('Profil fotoğrafı güncellenirken hata oluştu.')
        console.error('Avatar update error:', error)
      }
    }
    
    const handleAvatarError = (event) => {
      event.target.src = '/default-avatar.png'
    }
    
    const formatDate = (date) => {
      return dayjs(date).format('DD.MM.YYYY')
    }
    
    const getStatusBadgeClass = (status) => {
      const classes = {
        pending: 'bg-warning',
        confirmed: 'bg-success',
        cancelled: 'bg-danger',
        completed: 'bg-info'
      }
      return classes[status] || 'bg-secondary'
    }
    
    const getStatusText = (status) => {
      const texts = {
        pending: 'Beklemede',
        confirmed: 'Onaylandı',
        cancelled: 'İptal Edildi',
        completed: 'Tamamlandı'
      }
      return texts[status] || 'Bilinmiyor'
    }
    
    const getOrderStatusBadgeClass = (status) => {
      const classes = {
        pending: 'bg-warning',
        paid: 'bg-success',
        cancelled: 'bg-danger',
        refunded: 'bg-info'
      }
      return classes[status] || 'bg-secondary'
    }
    
    const getOrderStatusText = (status) => {
      const texts = {
        pending: 'Beklemede',
        paid: 'Ödendi',
        cancelled: 'İptal Edildi',
        refunded: 'İade Edildi'
      }
      return texts[status] || 'Bilinmiyor'
    }
    
    const canCancelAppointment = (appointment) => {
      const appointmentDate = dayjs(appointment.date)
      const now = dayjs()
      return appointmentDate.isAfter(now.add(24, 'hour')) && 
             (appointment.status === 'pending' || appointment.status === 'confirmed')
    }
    
    const viewAppointment = (appointment) => {
      // Navigate to appointment detail or show modal
      console.log('View appointment:', appointment)
    }
    
    const cancelAppointment = async (appointment) => {
      if (!confirm('Bu randevuyu iptal etmek istediğinizden emin misiniz?')) return
      
      try {
        await appointmentService.cancelAppointment(appointment.id)
        toast.success('Randevu başarıyla iptal edildi.')
        await loadAppointments()
      } catch (error) {
        toast.error('Randevu iptal edilirken hata oluştu.')
        console.error('Cancel appointment error:', error)
      }
    }
    
    // Lifecycle
    onMounted(async () => {
      await loadUserProfile()
      
      // Load appointments when tab is active
      if (activeTab.value === 'appointments') {
        await loadAppointments()
      }
      
      // Load orders when tab is active
      if (activeTab.value === 'orders') {
        await loadOrders()
      }
    })
    
    // Watch for tab changes to load data
    const originalActiveTab = activeTab.value
    const tabChanged = () => {
      if (activeTab.value === 'appointments' && !appointments.value.length) {
        loadAppointments()
      } else if (activeTab.value === 'orders' && !orders.value.length) {
        loadOrders()
      }
    }
    
    return {
      // Reactive data
      activeTab,
      fileInput,
      isUpdatingProfile,
      isUpdatingPassword,
      loadingAppointments,
      loadingOrders,
      appointments,
      orders,
      
      // Forms
      profileForm,
      passwordForm,
      
      // Errors
      profileErrors,
      passwordErrors,
      
      // Computed
      userFullName,
      userEmail,
      userAvatar,
      
      // Methods
      updateProfile,
      updatePassword,
      triggerFileUpload,
      handleAvatarUpload,
      handleAvatarError,
      formatDate,
      getStatusBadgeClass,
      getStatusText,
      getOrderStatusBadgeClass,
      getOrderStatusText,
      canCancelAppointment,
      viewAppointment,
      cancelAppointment,
      tabChanged
    }
  }
}
</script>

<style scoped>
.profile-sidebar {
  background: white;
  border-radius: 10px;
  padding: 0;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.user-info-card {
  text-align: center;
  padding: 2rem 1.5rem;
  border-bottom: 1px solid #eee;
}

.user-avatar {
  position: relative;
  display: inline-block;
  margin-bottom: 1rem;
}

.user-avatar img {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  object-fit: cover;
  border: 3px solid #007bff;
}

.avatar-upload-btn {
  position: absolute;
  bottom: 0;
  right: 0;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 50%;
  width: 35px;
  height: 35px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 123, 255, 0.3);
}

.avatar-upload-btn:hover {
  background: #0056b3;
  transform: scale(1.1);
}

.user-name {
  color: #333;
  margin-bottom: 0.5rem;
}

.user-email {
  color: #666;
  font-size: 0.9rem;
  margin-bottom: 0;
}

.profile-nav {
  padding: 1rem 0;
}

.profile-nav .nav-link {
  color: #666;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 0;
  text-decoration: none;
  transition: all 0.3s ease;
}

.profile-nav .nav-link:hover,
.profile-nav .nav-link.active {
  background: #007bff;
  color: white;
}

.tab-content .card {
  border: none;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  border-radius: 10px;
}

.card-header {
  background: #f8f9fa;
  border-bottom: 1px solid #eee;
  border-radius: 10px 10px 0 0 !important;
}

.appointment-card,
.order-card {
  background: #f8f9fa;
  border: 1px solid #eee;
  border-radius: 8px;
  padding: 1rem;
  transition: all 0.3s ease;
}

.appointment-card:hover,
.order-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  transform: translateY(-2px);
}

.appointment-date,
.appointment-time {
  font-size: 0.9rem;
  margin-bottom: 0.25rem;
}

.coach-info h6 {
  color: #333;
  font-weight: 600;
}

.form-label {
  font-weight: 500;
  color: #333;
  margin-bottom: 0.5rem;
}

.form-control:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.btn-primary {
  background: #007bff;
  border-color: #007bff;
  border-radius: 25px;
  padding: 0.5rem 1.5rem;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  background: #0056b3;
  border-color: #0056b3;
  transform: translateY(-1px);
  box-shadow: 0 4px 8px rgba(0, 123, 255, 0.3);
}

.badge {
  font-size: 0.75rem;
  padding: 0.5rem 0.75rem;
  border-radius: 15px;
}

/* Responsive */
@media (max-width: 768px) {
  .user-avatar img {
    width: 80px;
    height: 80px;
  }
  
  .avatar-upload-btn {
    width: 30px;
    height: 30px;
  }
  
  .profile-nav .nav-link {
    padding: 0.5rem 1rem;
    font-size: 0.9rem;
  }
  
  .appointment-card,
  .order-card {
    text-align: center;
  }
  
  .appointment-card .row > div,
  .order-card .row > div {
    margin-bottom: 0.5rem;
  }
}

/* Dark theme support */
@media (prefers-color-scheme: dark) {
  .profile-sidebar,
  .tab-content .card {
    background: #2d3748;
    color: white;
  }
  
  .user-info-card {
    border-bottom-color: #4a5568;
  }
  
  .card-header {
    background: #4a5568;
    border-bottom-color: #2d3748;
  }
  
  .appointment-card,
  .order-card {
    background: #4a5568;
    border-color: #2d3748;
  }
}