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
                                    <img :src="userAvatar"
                                         :alt="userFullName"
                                         @error="handleAvatarError" />
                                    <button class="avatar-upload-btn" @click="triggerFileUpload">
                                        <i class="fas fa-camera"></i>
                                    </button>
                                    <input ref="fileInput"
                                           type="file"
                                           accept="image/*"
                                           @change="handleAvatarUpload"
                                           style="display: none;" />
                                </div>
                                <h5 class="user-name">{{ userFullName }}</h5>
                                <p class="user-email">{{ userEmail }}</p>
                            </div>

                            <nav class="profile-nav">
                                <ul class="nav flex-column">
                                    <li class="nav-item">
                                        <a href="#"
                                           class="nav-link"
                                           :class="{ active: activeTab === 'profile' }"
                                           @click.prevent="activeTab = 'profile'">
                                            <i class="fas fa-user me-2"></i>
                                            Kişisel Bilgiler
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="#"
                                           class="nav-link"
                                           :class="{ active: activeTab === 'appointments' }"
                                           @click.prevent="activeTab = 'appointments'">
                                            <i class="fas fa-calendar-alt me-2"></i>
                                            Randevularım
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="#"
                                           class="nav-link"
                                           :class="{ active: activeTab === 'orders' }"
                                           @click.prevent="activeTab = 'orders'">
                                            <i class="fas fa-shopping-bag me-2"></i>
                                            Siparişlerim
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a href="#"
                                           class="nav-link"
                                           :class="{ active: activeTab === 'password' }"
                                           @click.prevent="activeTab = 'password'">
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
                                                <input id="firstName"
                                                       v-model="profileForm.firstName"
                                                       type="text"
                                                       class="form-control"
                                                       :class="{ 'is-invalid': profileErrors.firstName }"
                                                       required />
                                                <div v-if="profileErrors.firstName" class="invalid-feedback">
                                                    {{ profileErrors.firstName }}
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="lastName" class="form-label">Soyad *</label>
                                                <input id="lastName"
                                                       v-model="profileForm.lastName"
                                                       type="text"
                                                       class="form-control"
                                                       :class="{ 'is-invalid': profileErrors.lastName }"
                                                       required />
                                                <div v-if="profileErrors.lastName" class="invalid-feedback">
                                                    {{ profileErrors.lastName }}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label for="email" class="form-label">E-posta *</label>
                                                <input id="email"
                                                       v-model="profileForm.email"
                                                       type="email"
                                                       class="form-control"
                                                       :class="{ 'is-invalid': profileErrors.email }"
                                                       required />
                                                <div v-if="profileErrors.email" class="invalid-feedback">
                                                    {{ profileErrors.email }}
                                                </div>
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label for="phone" class="form-label">Telefon</label>
                                                <input id="phone"
                                                       v-model="profileForm.phone"
                                                       type="tel"
                                                       class="form-control"
                                                       :class="{ 'is-invalid': profileErrors.phone }" />
                                                <div v-if="profileErrors.phone" class="invalid-feedback">
                                                    {{ profileErrors.phone }}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="mb-3">
                                            <label for="birthDate" class="form-label">Doğum Tarihi</label>
                                            <input id="birthDate"
                                                   v-model="profileForm.birthDate"
                                                   type="date"
                                                   class="form-control"
                                                   :class="{ 'is-invalid': profileErrors.birthDate }" />
                                            <div v-if="profileErrors.birthDate" class="invalid-feedback">
                                                {{ profileErrors.birthDate }}
                                            </div>
                                        </div>
                                        <div class="mb-3">
                                            <label for="gender" class="form-label">Cinsiyet</label>
                                            <select id="gender"
                                                    v-model="profileForm.gender"
                                                    class="form-select"
                                                    :class="{ 'is-invalid': profileErrors.gender }">
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
                                            <textarea id="bio"
                                                      v-model="profileForm.bio"
                                                      class="form-control"
                                                      rows="4"
                                                      :class="{ 'is-invalid': profileErrors.bio }"
                                                      placeholder="Kendiniz hakkında kısa bir açıklama yazın..."></textarea>
                                            <div v-if="profileErrors.bio" class="invalid-feedback">
                                                {{ profileErrors.bio }}
                                            </div>
                                        </div>
                                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                            <button type="submit"
                                                    class="btn btn-primary"
                                                    :disabled="isUpdatingProfile">
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
                                        <div v-for="appointment in appointments"
                                             :key="appointment.id"
                                             class="appointment-card mb-3">
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
                                                    <span class="badge"
                                                          :class="getStatusBadgeClass(appointment.status)">
                                                        {{ getStatusText(appointment.status) }}
                                                    </span>
                                                </div>
                                                <div class="col-md-2 text-end">
                                                    <div class="dropdown">
                                                        <button class="btn btn-outline-secondary btn-sm dropdown-toggle"
                                                                type="button"
                                                                :id="`appointment-${appointment.id}`"
                                                                data-bs-toggle="dropdown">
                                                            İşlemler
                                                        </button>
                                                        <ul class="dropdown-menu">
                                                            <li>
                                                                <a class="dropdown-item"
                                                                   href="#"
                                                                   @click.prevent="viewAppointment(appointment)">
                                                                    Detayları Görüntüle
                                                                </a>
                                                            </li>
                                                            <li v-if="canCancelAppointment(appointment)">
                                                                <a class="dropdown-item text-danger"
                                                                   href="#"
                                                                   @click.prevent="cancelAppointment(appointment)">
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
                                        <div v-for="order in orders"
                                             :key="order.id"
                                             class="order-card mb-3">
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
                                                    <span class="badge"
                                                          :class="getOrderStatusBadgeClass(order.status)">
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
                                            <input id="currentPassword"
                                                   v-model="passwordForm.currentPassword"
                                                   type="password"
                                                   class="form-control"
                                                   :class="{ 'is-invalid': passwordErrors.currentPassword }"
                                                   required />
                                            <div v-if="passwordErrors.currentPassword" class="invalid-feedback">
                                                {{ passwordErrors.currentPassword }}
                                            </div>
                                        </div>
                                        <div class="mb-3">
                                            <label for="newPassword" class="form-label">Yeni Şifre *</label>
                                            <input id="newPassword"
                                                   v-model="passwordForm.newPassword"
                                                   type="password"
                                                   class="form-control"
                                                   :class="{ 'is-invalid': passwordErrors.newPassword }"
                                                   required
                                                   minlength="6" />
                                            <div v-if="passwordErrors.newPassword" class="invalid-feedback">
                                                {{ passwordErrors.newPassword }}
                                            </div>
                                            <div class="form-text">En az 6 karakter olmalıdır.</div>
                                        </div>
                                        <div class="mb-3">
                                            <label for="confirmPassword" class="form-label">Yeni Şifre Tekrar *</label>
                                            <input id="confirmPassword"
                                                   v-model="passwordForm.confirmPassword"
                                                   type="password"
                                                   class="form-control"
                                                   :class="{ 'is-invalid': passwordErrors.confirmPassword }"
                                                   required />
                                            <div v-if="passwordErrors.confirmPassword" class="invalid-feedback">
                                                {{ passwordErrors.confirmPassword }}
                                            </div>
                                        </div>
                                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                                            <button type="submit"
                                                    class="btn btn-primary"
                                                    :disabled="isUpdatingPassword">
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
    /* Main Layout */
    .profile-page {
        background: #f8f9fa;
        min-height: 100vh;
    }

    .breadcrumb-section {
        background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
    }

    .breadcrumb {
        background: transparent !important;
    }

    .breadcrumb-item a {
        color: rgba(255, 255, 255, 0.7);
        text-decoration: none;
        transition: color 0.3s ease;
    }

        .breadcrumb-item a:hover {
            color: rgba(255, 255, 255, 0.9);
        }

    /* Profile Content */
    .profile-content {
        padding: 3rem 0;
    }

    /* Sidebar */
    .profile-sidebar {
        background: white;
        border-radius: 15px;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        overflow: hidden;
        position: sticky;
        top: 2rem;
    }

    .user-info-card {
        text-align: center;
        padding: 2rem 1.5rem;
        border-bottom: 1px solid #eee;
    }

    .user-avatar {
        position: relative;
        display: inline-block;
        margin-bottom: 1.5rem;
    }

        .user-avatar img {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 4px solid #007bff;
            box-shadow: 0 4px 15px rgba(0, 123, 255, 0.3);
        }

    .avatar-upload-btn {
        position: absolute;
        bottom: 0;
        right: 0;
        width: 35px;
        height: 35px;
        background: #007bff;
        border: 3px solid white;
        border-radius: 50%;
        color: white;
        font-size: 0.8rem;
        cursor: pointer;
        transition: all 0.3s ease;
        display: flex;
        align-items: center;
        justify-content: center;
    }

        .avatar-upload-btn:hover {
            background: #0056b3;
            transform: scale(1.1);
        }

    .user-name {
        color: #333;
        font-weight: 600;
        margin-bottom: 0.5rem;
        font-size: 1.3rem;
    }

    .user-email {
        color: #666;
        font-size: 0.9rem;
        margin: 0;
    }

    /* Profile Navigation */
    .profile-nav {
        padding: 1rem 0;
    }

        .profile-nav .nav {
            list-style: none;
            margin: 0;
            padding: 0;
        }

        .profile-nav .nav-item {
            border-bottom: 1px solid #f1f1f1;
        }

            .profile-nav .nav-item:last-child {
                border-bottom: none;
            }

        .profile-nav .nav-link {
            display: flex;
            align-items: center;
            padding: 1rem 1.5rem;
            color: #666;
            text-decoration: none;
            font-weight: 500;
            transition: all 0.3s ease;
            border-left: 3px solid transparent;
        }

            .profile-nav .nav-link:hover {
                background: #f8f9fa;
                color: #007bff;
                border-left-color: #007bff;
            }

            .profile-nav .nav-link.active {
                background: rgba(0, 123, 255, 0.1);
                color: #007bff;
                border-left-color: #007bff;
            }

            .profile-nav .nav-link i {
                margin-right: 0.75rem;
                width: 20px;
                text-align: center;
            }

    /* Main Content */
    .tab-content {
        animation: fadeIn 0.4s ease;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(20px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .tab-content .card {
        background: white;
        border-radius: 15px;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        border: none;
        margin-bottom: 2rem;
    }

    .card-header {
        background: #f8f9fa;
        border-bottom: 1px solid #eee;
        border-radius: 15px 15px 0 0 !important;
        padding: 1.5rem;
    }

        .card-header h5 {
            color: #333;
            font-weight: 600;
            margin: 0;
        }

    .card-body {
        padding: 2rem;
    }

    /* Form Styles */
    .form-label {
        font-weight: 500;
        color: #333;
        margin-bottom: 0.5rem;
    }

    .form-control {
        border: 2px solid #eee;
        border-radius: 10px;
        padding: 0.75rem 1rem;
        font-size: 1rem;
        transition: all 0.3s ease;
    }

        .form-control:focus {
            border-color: #007bff;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
            outline: none;
        }

        .form-control.is-invalid {
            border-color: #dc3545;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

    .invalid-feedback {
        display: block;
        color: #dc3545;
        font-size: 0.875rem;
        margin-top: 0.25rem;
    }

    /* Button Styles */
    .btn-primary {
        background: #007bff;
        border-color: #007bff;
        border-radius: 25px;
        padding: 0.75rem 2rem;
        font-weight: 500;
        transition: all 0.3s ease;
    }

        .btn-primary:hover {
            background: #0056b3;
            border-color: #0056b3;
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(0, 123, 255, 0.3);
        }

    .btn-outline-primary {
        border: 2px solid #007bff;
        color: #007bff;
        border-radius: 25px;
        padding: 0.75rem 2rem;
        font-weight: 500;
        transition: all 0.3s ease;
    }

        .btn-outline-primary:hover {
            background: #007bff;
            border-color: #007bff;
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(0, 123, 255, 0.3);
        }

    .btn-danger {
        background: #dc3545;
        border-color: #dc3545;
        border-radius: 25px;
        padding: 0.75rem 2rem;
        font-weight: 500;
        transition: all 0.3s ease;
    }

        .btn-danger:hover {
            background: #c82333;
            border-color: #bd2130;
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(220, 53, 69, 0.3);
        }

    /* Appointments Section */
    .appointment-card {
        background: white;
        border: 1px solid #eee;
        border-radius: 15px;
        padding: 1.5rem;
        margin-bottom: 1.5rem;
        transition: all 0.3s ease;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
    }

        .appointment-card:hover {
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
            transform: translateY(-2px);
        }

    .appointment-date {
        font-size: 0.9rem;
        color: #666;
        margin-bottom: 0.25rem;
    }

    .appointment-time {
        font-size: 0.9rem;
        color: #007bff;
        font-weight: 500;
        margin-bottom: 0.25rem;
    }

    .coach-info h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 0.5rem;
    }

    .coach-info p {
        color: #666;
        font-size: 0.9rem;
        margin: 0;
    }

    /* Orders Section */
    .order-card {
        background: white;
        border: 1px solid #eee;
        border-radius: 15px;
        padding: 1.5rem;
        margin-bottom: 1.5rem;
        transition: all 0.3s ease;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
    }

        .order-card:hover {
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
            transform: translateY(-2px);
        }

    .order-info h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 0.5rem;
    }

    .order-info p {
        color: #666;
        font-size: 0.9rem;
        margin: 0;
    }

    /* Badge Styles */
    .badge {
        font-size: 0.75rem;
        padding: 0.5rem 0.75rem;
        border-radius: 15px;
        font-weight: 500;
    }

        .badge.bg-success {
            background: #28a745 !important;
        }

        .badge.bg-warning {
            background: #ffc107 !important;
            color: #212529 !important;
        }

        .badge.bg-danger {
            background: #dc3545 !important;
        }

        .badge.bg-info {
            background: #17a2b8 !important;
        }

    /* Empty State */
    .text-center.text-muted {
        padding: 3rem;
        color: #666 !important;
    }

        .text-center.text-muted i {
            font-size: 3rem;
            margin-bottom: 1rem;
            opacity: 0.5;
        }

    /* Loading State */
    .loading-skeleton {
        background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
        background-size: 200% 100%;
        animation: loading 1.5s infinite;
        border-radius: 10px;
        height: 20px;
        margin-bottom: 1rem;
    }

    @keyframes loading {
        0% {
            background-position: 200% 0;
        }

        100% {
            background-position: -200% 0;
        }
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .profile-content {
            padding: 2rem 0;
        }

        .user-avatar img {
            width: 80px;
            height: 80px;
        }

        .avatar-upload-btn {
            width: 30px;
            height: 30px;
            font-size: 0.7rem;
        }

        .profile-nav .nav-link {
            padding: 0.75rem 1rem;
            font-size: 0.9rem;
        }

        .card-body {
            padding: 1.5rem;
        }

        .appointment-card,
        .order-card {
            text-align: center;
            padding: 1rem;
        }

            .appointment-card .row > div,
            .order-card .row > div {
                margin-bottom: 0.5rem;
            }

        .btn {
            width: 100%;
            margin-bottom: 0.5rem;
        }
    }

    @media (max-width: 576px) {
        .profile-sidebar {
            position: static;
            margin-bottom: 2rem;
        }

        .user-info-card {
            padding: 1.5rem 1rem;
        }

        .card-header {
            padding: 1rem;
        }

        .card-body {
            padding: 1rem;
        }
    }

    /* Print Styles */
    @media print {
        .profile-nav,
        .avatar-upload-btn,
        .btn {
            display: none !important;
        }

        .profile-sidebar,
        .tab-content .card {
            box-shadow: none;
            border: 1px solid #ddd;
        }
    }

    /* Dark Theme Support */
    @media (prefers-color-scheme: dark) {
        .profile-page {
            background: #1a202c;
        }

        .profile-sidebar,
        .tab-content .card,
        .appointment-card,
        .order-card {
            background: #2d3748;
            color: #e2e8f0;
            border-color: #4a5568;
        }

        .user-info-card {
            border-bottom-color: #4a5568;
        }

        .card-header {
            background: #4a5568;
            border-bottom-color: #2d3748;
        }

            .user-name,
            .card-header h5 {
                color: #e2e8f0;
            }

        .form-control {
            background: #4a5568;
            border-color: #4a5568;
            color: #e2e8f0;
        }

            .form-control:focus {
                background: #4a5568;
                border-color: #63b3ed;
                color: #e2e8f0;
            }

        .profile-nav .nav-link {
            color: #a0aec0;
        }

            .profile-nav .nav-link:hover,
            .profile-nav .nav-link.active {
                color: #63b3ed;
                background: rgba(99, 179, 237, 0.1);
            }
    }

    /* High Contrast Mode */
    @media (prefers-contrast: high) {
        .form-control:focus {
            outline: 3px solid #007bff;
            outline-offset: 2px;
        }

        .btn:focus {
            outline: 3px solid currentColor;
            outline-offset: 2px;
        }

        .appointment-card:hover,
        .order-card:hover {
            outline: 2px solid #007bff;
        }
    }

    /* Reduced Motion */
    @media (prefers-reduced-motion: reduce) {
        .tab-content,
        .appointment-card,
        .order-card,
        .btn,
        .form-control,
        .avatar-upload-btn {
            transition: none;
            animation: none;
        }

            .appointment-card:hover,
            .order-card:hover,
            .btn:hover {
                transform: none;
            }
    }

    /* Focus Styles for Accessibility */
    .form-control:focus,
    .btn:focus,
    .nav-link:focus,
    .avatar-upload-btn:focus {
        outline: 2px solid #007bff;
        outline-offset: 2px;
    }
</style>