<template>
    <div class="coach-detail-page">
        <!-- Breadcrumb -->
        <section class="breadcrumb-section bg-primary text-white py-4">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb bg-transparent mb-0">
                                <li class="breadcrumb-item">
                                    <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                                </li>
                                <li class="breadcrumb-item">
                                    <router-link to="/kocluk-ekibimiz" class="text-white-50">Koçluk Ekibimiz</router-link>
                                </li>
                                <li class="breadcrumb-item active text-white">{{ coach?.fullName }}</li>
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
                <p class="mt-3">Koç bilgileri yükleniyor...</p>
            </div>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="container py-5">
            <div class="alert alert-danger" role="alert">
                <h4 class="alert-heading">Hata!</h4>
                <p>{{ error }}</p>
                <hr>
                <router-link to="/kocluk-ekibimiz" class="btn btn-primary">
                    Koçluk Ekibine Dön
                </router-link>
            </div>
        </div>

        <!-- Coach Profile Content -->
        <section v-else-if="coach" class="coach-profile py-5">
            <div class="container">
                <div class="row">
                    <!-- Coach Info Card -->
                    <div class="col-lg-8">
                        <div class="coach-profile-card">
                            <!-- Coach Header -->
                            <div class="coach-header">
                                <div class="row align-items-center">
                                    <div class="col-md-4 text-center">
                                        <div class="coach-avatar">
                                            <img :src="coach.profilePhoto || '/default-coach.jpg'"
                                                 :alt="coach.fullName"
                                                 class="img-fluid rounded-circle"
                                                 @error="handleImageError" />
                                            <div v-if="coach.isOnline" class="online-indicator" title="Çevrimiçi">
                                                <i class="fas fa-circle"></i>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="coach-basic-info">
                                            <h1 class="coach-name">{{ coach.fullName }}</h1>
                                            <h3 class="coach-title text-primary">{{ coach.title }}</h3>
                                            <div class="coach-rating mb-3">
                                                <div class="stars">
                                                    <i v-for="star in 5"
                                                       :key="star"
                                                       class="fas fa-star"
                                                       :class="{ 'text-warning': star <= coach.rating }"></i>
                                                </div>
                                                <span class="rating-text ms-2">
                                                    {{ coach.rating || 0 }}/5 ({{ coach.reviewCount || 0 }} değerlendirme)
                                                </span>
                                            </div>
                                            <div class="coach-stats">
                                                <div class="row text-center">
                                                    <div class="col-4">
                                                        <div class="stat-item">
                                                            <h4 class="stat-number">{{ coach.experienceYears || 0 }}</h4>
                                                            <p class="stat-label">Yıl Deneyim</p>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="stat-item">
                                                            <h4 class="stat-number">{{ coach.sessionCount || 0 }}</h4>
                                                            <p class="stat-label">Tamamlanan Seans</p>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">
                                                        <div class="stat-item">
                                                            <h4 class="stat-number">{{ coach.clientCount || 0 }}</h4>
                                                            <p class="stat-label">Mutlu Müşteri</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Coach Tabs -->
                            <div class="coach-tabs mt-4">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link"
                                                :class="{ active: activeTab === 'about' }"
                                                @click="activeTab = 'about'">
                                            Hakkında
                                        </button>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link"
                                                :class="{ active: activeTab === 'specialties' }"
                                                @click="activeTab = 'specialties'">
                                            Uzmanlık Alanları
                                        </button>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link"
                                                :class="{ active: activeTab === 'education' }"
                                                @click="activeTab = 'education'">
                                            Eğitim & Sertifikalar
                                        </button>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <button class="nav-link"
                                                :class="{ active: activeTab === 'reviews' }"
                                                @click="activeTab = 'reviews'">
                                            Değerlendirmeler
                                        </button>
                                    </li>
                                    <li v-if="coach.blogs && coach.blogs.length" class="nav-item" role="presentation">
                                        <button class="nav-link"
                                                :class="{ active: activeTab === 'blogs' }"
                                                @click="activeTab = 'blogs'">
                                            Yazıları
                                        </button>
                                    </li>
                                </ul>

                                <div class="tab-content mt-4">
                                    <!-- About Tab -->
                                    <div v-show="activeTab === 'about'" class="tab-pane">
                                        <div class="about-content">
                                            <h4>Koç Hakkında</h4>
                                            <div class="description" v-html="coach.description || 'Açıklama henüz eklenmemiş.'"></div>

                                            <div v-if="coach.approach" class="mt-4">
                                                <h5>Koçluk Yaklaşımı</h5>
                                                <p>{{ coach.approach }}</p>
                                            </div>

                                            <div v-if="coach.languages && coach.languages.length" class="mt-4">
                                                <h5>Konuşulan Diller</h5>
                                                <div class="languages">
                                                    <span v-for="language in coach.languages"
                                                          :key="language"
                                                          class="badge bg-light text-dark me-2">
                                                        {{ language }}
                                                    </span>
                                                </div>
                                            </div>

                                            <div v-if="coach.workingHours" class="mt-4">
                                                <h5>Çalışma Saatleri</h5>
                                                <p>{{ coach.workingHours }}</p>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Specialties Tab -->
                                    <div v-show="activeTab === 'specialties'" class="tab-pane">
                                        <h4>Uzmanlık Alanları</h4>
                                        <div v-if="coach.specialties && coach.specialties.length" class="specialties-grid">
                                            <div v-for="specialty in coach.specialties"
                                                 :key="specialty.id"
                                                 class="specialty-card">
                                                <div class="specialty-icon">
                                                    <i :class="specialty.icon || 'fas fa-star'"></i>
                                                </div>
                                                <h6>{{ specialty.name }}</h6>
                                                <p>{{ specialty.description }}</p>
                                            </div>
                                        </div>
                                        <div v-else class="text-muted">
                                            Uzmanlık alanları henüz eklenmemiş.
                                        </div>
                                    </div>

                                    <!-- Education Tab -->
                                    <div v-show="activeTab === 'education'" class="tab-pane">
                                        <h4>Eğitim ve Sertifikalar</h4>

                                        <div v-if="coach.education && coach.education.length" class="education-section mb-4">
                                            <h5>Eğitim</h5>
                                            <div v-for="edu in coach.education"
                                                 :key="edu.id"
                                                 class="education-item">
                                                <div class="education-header">
                                                    <h6>{{ edu.degree }} - {{ edu.field }}</h6>
                                                    <span class="education-year">{{ edu.year }}</span>
                                                </div>
                                                <p class="education-school">{{ edu.school }}</p>
                                                <p v-if="edu.description" class="education-description">{{ edu.description }}</p>
                                            </div>
                                        </div>

                                        <div v-if="coach.certifications && coach.certifications.length" class="certifications-section">
                                            <h5>Sertifikalar</h5>
                                            <div v-for="cert in coach.certifications"
                                                 :key="cert.id"
                                                 class="certification-item">
                                                <div class="certification-header">
                                                    <h6>{{ cert.name }}</h6>
                                                    <span class="certification-year">{{ cert.year }}</span>
                                                </div>
                                                <p class="certification-issuer">{{ cert.issuer }}</p>
                                                <p v-if="cert.description" class="certification-description">{{ cert.description }}</p>
                                            </div>
                                        </div>

                                        <div v-if="(!coach.education || !coach.education.length) && (!coach.certifications || !coach.certifications.length)" class="text-muted">
                                            Eğitim ve sertifika bilgileri henüz eklenmemiş.
                                        </div>
                                    </div>

                                    <!-- Reviews Tab -->
                                    <div v-show="activeTab === 'reviews'" class="tab-pane">
                                        <h4>Müşteri Değerlendirmeleri</h4>

                                        <div v-if="loadingReviews" class="text-center py-4">
                                            <div class="spinner-border spinner-border-sm text-primary"></div>
                                            <p class="mt-2">Değerlendirmeler yükleniyor...</p>
                                        </div>

                                        <div v-else-if="reviews && reviews.length" class="reviews-list">
                                            <div v-for="review in reviews"
                                                 :key="review.id"
                                                 class="review-item">
                                                <div class="review-header">
                                                    <div class="reviewer-info">
                                                        <img :src="review.userAvatar || '/default-avatar.png'"
                                                             :alt="review.userName"
                                                             class="reviewer-avatar"
                                                             @error="handleImageError" />
                                                        <div>
                                                            <h6>{{ review.userName }}</h6>
                                                            <div class="review-rating">
                                                                <i v-for="star in 5"
                                                                   :key="star"
                                                                   class="fas fa-star"
                                                                   :class="{ 'text-warning': star <= review.rating }"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <span class="review-date">{{ formatDate(review.date) }}</span>
                                                </div>
                                                <p class="review-comment">{{ review.comment }}</p>
                                            </div>
                                        </div>

                                        <div v-else class="text-muted">
                                            Henüz değerlendirme yapılmamış.
                                        </div>
                                    </div>

                                    <!-- Blogs Tab -->
                                    <div v-show="activeTab === 'blogs'" class="tab-pane">
                                        <h4>{{ coach.fullName }} Tarafından Yazılan Makaleler</h4>

                                        <div v-if="coach.blogs && coach.blogs.length" class="blogs-grid">
                                            <article v-for="blog in coach.blogs"
                                                     :key="blog.id"
                                                     class="blog-card">
                                                <div class="blog-image">
                                                    <router-link :to="`/kisisel-gelisim/yazilar/${blog.id}/${blog.slug}`">
                                                        <img :src="blog.image || '/default-blog.jpg'"
                                                             :alt="blog.title"
                                                             class="img-fluid"
                                                             @error="handleImageError" />
                                                    </router-link>
                                                </div>
                                                <div class="blog-content">
                                                    <div class="blog-meta">
                                                        <span class="blog-date">{{ formatDate(blog.date) }}</span>
                                                        <span class="blog-category">{{ blog.category }}</span>
                                                    </div>
                                                    <h6>
                                                        <router-link :to="`/kisisel-gelisim/yazilar/${blog.id}/${blog.slug}`">
                                                            {{ blog.title }}
                                                        </router-link>
                                                    </h6>
                                                    <p class="blog-excerpt">{{ blog.excerpt }}</p>
                                                </div>
                                            </article>
                                        </div>

                                        <div v-else class="text-muted">
                                            Henüz makale yazılmamış.
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Sidebar -->
                    <div class="col-lg-4">
                        <div class="coach-sidebar">
                            <!-- Booking Card -->
                            <div class="booking-card">
                                <h5>Randevu Al</h5>
                                <div class="price-info">
                                    <div class="price">
                                        <span class="amount">{{ formatPrice(coach.sessionPrice) }}</span>
                                        <span class="period">/ seans</span>
                                    </div>
                                    <div v-if="coach.discountPrice" class="discount">
                                        <span class="original-price">{{ formatPrice(coach.originalPrice) }}</span>
                                        <span class="discount-badge">%{{ calculateDiscount(coach.originalPrice, coach.sessionPrice) }} İndirim</span>
                                    </div>
                                </div>

                                <div class="session-types mb-3">
                                    <h6>Seans Türleri</h6>
                                    <div v-for="sessionType in coach.sessionTypes || defaultSessionTypes"
                                         :key="sessionType.id"
                                         class="session-type">
                                        <div class="form-check">
                                            <input v-model="selectedSessionType"
                                                   :value="sessionType.id"
                                                   class="form-check-input"
                                                   type="radio"
                                                   :id="`session-${sessionType.id}`" />
                                            <label class="form-check-label" :for="`session-${sessionType.id}`">
                                                <strong>{{ sessionType.name }}</strong>
                                                <span class="session-duration">({{ sessionType.duration }} dk)</span>
                                                <div class="session-price">{{ formatPrice(sessionType.price) }}</div>
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div class="booking-actions">
                                    <router-link :to="`/randevu-takvimi/${coach.id}/${coach.slug}/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}`"
                                                 class="btn btn-primary btn-lg w-100 mb-2">
                                        <i class="fas fa-calendar-alt me-2"></i>
                                        Randevu Al
                                    </router-link>
                                    <button @click="sendMessage"
                                            class="btn btn-outline-primary w-100"
                                            :disabled="isSendingMessage">
                                        <span v-if="isSendingMessage" class="spinner-border spinner-border-sm me-2"></span>
                                        <i v-else class="fas fa-envelope me-2"></i>
                                        Mesaj Gönder
                                    </button>
                                </div>
                            </div>

                            <!-- Contact Info -->
                            <div class="contact-card">
                                <h5>İletişim</h5>
                                <div class="contact-item">
                                    <i class="fas fa-envelope"></i>
                                    <span>{{ coach.email || 'E-posta mevcut değil' }}</span>
                                </div>
                                <div v-if="coach.phone" class="contact-item">
                                    <i class="fas fa-phone"></i>
                                    <span>{{ coach.phone }}</span>
                                </div>
                                <div v-if="coach.website" class="contact-item">
                                    <i class="fas fa-globe"></i>
                                    <a :href="coach.website" target="_blank">{{ coach.website }}</a>
                                </div>
                                <div v-if="coach.socialMedia" class="social-links">
                                    <a v-if="coach.socialMedia.linkedin" :href="coach.socialMedia.linkedin" target="_blank">
                                        <i class="fab fa-linkedin"></i>
                                    </a>
                                    <a v-if="coach.socialMedia.twitter" :href="coach.socialMedia.twitter" target="_blank">
                                        <i class="fab fa-twitter"></i>
                                    </a>
                                    <a v-if="coach.socialMedia.instagram" :href="coach.socialMedia.instagram" target="_blank">
                                        <i class="fab fa-instagram"></i>
                                    </a>
                                </div>
                            </div>

                            <!-- Similar Coaches -->
                            <div v-if="similarCoaches && similarCoaches.length" class="similar-coaches-card">
                                <h5>Benzer Koçlar</h5>
                                <div v-for="similarCoach in similarCoaches"
                                     :key="similarCoach.id"
                                     class="similar-coach-item">
                                    <router-link :to="`/kocluk-ekibimiz/${similarCoach.categoryId}/category/${similarCoach.id}/${similarCoach.fullName.toLowerCase().replace(/\s+/g, '-')}`"
                                                 class="similar-coach-link">
                                        <img :src="similarCoach.profilePhoto || '/default-coach.jpg'"
                                             :alt="similarCoach.fullName"
                                             class="similar-coach-avatar"
                                             @error="handleImageError" />
                                        <div class="similar-coach-info">
                                            <h6>{{ similarCoach.fullName }}</h6>
                                            <p>{{ similarCoach.title }}</p>
                                            <div class="similar-coach-rating">
                                                <div class="stars">
                                                    <i v-for="star in 5"
                                                       :key="star"
                                                       class="fas fa-star"
                                                       :class="{ 'text-warning': star <= similarCoach.rating }"></i>
                                                </div>
                                                <span>{{ similarCoach.rating || 0 }}/5</span>
                                            </div>
                                        </div>
                                    </router-link>
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
    import { ref, reactive, computed, onMounted, watch } from 'vue'
    import { useRoute, useRouter } from 'vue-router'
    import { useAuthStore } from '@/stores/auth'
    import { useToast } from 'vue-toastification'
    import contentService from '@/services/content'
    import dayjs from 'dayjs'

    export default {
        name: 'CoachDetailPage',
        setup() {
            const route = useRoute()
            const router = useRouter()
            const authStore = useAuthStore()
            const toast = useToast()

            // Reactive data
            const coach = ref(null)
            const reviews = ref([])
            const similarCoaches = ref([])
            const loading = ref(true)
            const loadingReviews = ref(false)
            const error = ref(null)
            const activeTab = ref('about')
            const selectedSessionType = ref(null)
            const isSendingMessage = ref(false)

            // Default session types if not provided by API
            const defaultSessionTypes = [
                {
                    id: 1,
                    name: 'Standart Seans',
                    duration: 60,
                    price: 500
                },
                {
                    id: 2,
                    name: 'Yoğun Seans',
                    duration: 90,
                    price: 700
                }
            ]

            // Computed properties
            const isAuthenticated = computed(() => authStore.isAuthenticated)

            // Methods
            const loadCoach = async () => {
                try {
                    loading.value = true
                    error.value = null

                    const coachId = route.params.coachId
                    const response = await contentService.getCoach(coachId)

                    if (!response.data) {
                        throw new Error('Koç bulunamadı.')
                    }

                    coach.value = response.data

                    // Set default session type
                    if (coach.value.sessionTypes && coach.value.sessionTypes.length > 0) {
                        selectedSessionType.value = coach.value.sessionTypes[0].id
                    } else {
                        selectedSessionType.value = defaultSessionTypes[0].id
                    }

                    // Update page title
                    document.title = `${coach.value.fullName} - Koç Profili - PekiYaBen`

                    // Load additional data
                    await Promise.all([
                        loadReviews(),
                        loadSimilarCoaches()
                    ])

                } catch (err) {
                    error.value = err.message || 'Koç bilgileri yüklenirken hata oluştu.'
                    console.error('Coach load error:', err)
                } finally {
                    loading.value = false
                }
            }

            const loadReviews = async () => {
                if (!coach.value) return

                try {
                    loadingReviews.value = true
                    const response = await contentService.getCoachReviews(coach.value.id)
                    reviews.value = response.data || []
                } catch (err) {
                    console.error('Reviews load error:', err)
                } finally {
                    loadingReviews.value = false
                }
            }

            const loadSimilarCoaches = async () => {
                if (!coach.value) return

                try {
                    const response = await contentService.getSimilarCoaches(coach.value.id, coach.value.categoryId)
                    similarCoaches.value = (response.data || []).slice(0, 3)
                } catch (err) {
                    console.error('Similar coaches load error:', err)
                }
            }

            const sendMessage = async () => {
                if (!isAuthenticated.value) {
                    toast.warning('Mesaj göndermek için giriş yapmanız gerekiyor.')
                    router.push({ name: 'Login', query: { returnUrl: route.fullPath } })
                    return
                }

                try {
                    isSendingMessage.value = true

                    // Here you would typically open a message modal or redirect to messaging
                    // For now, we'll show a success message
                    toast.success('Mesaj gönderme sayfasına yönlendiriliyorsunuz...')

                    // You could implement a messaging system here
                    // await messageService.sendMessage(coach.value.id, messageData)

                } catch (err) {
                    toast.error('Mesaj gönderilirken hata oluştu.')
                    console.error('Send message error:', err)
                } finally {
                    isSendingMessage.value = false
                }
            }

            const formatDate = (date) => {
                return dayjs(date).format('DD MMMM YYYY')
            }

            const formatPrice = (price) => {
                if (!price) return '0 ₺'
                return `${price.toLocaleString('tr-TR')} ₺`
            }

            const calculateDiscount = (originalPrice, discountPrice) => {
                if (!originalPrice || !discountPrice) return 0
                return Math.round(((originalPrice - discountPrice) / originalPrice) * 100)
            }

            const handleImageError = (event) => {
                if (event.target.src.includes('default-')) return

                if (event.target.classList.contains('coach-avatar') ||
                    event.target.classList.contains('reviewer-avatar') ||
                    event.target.classList.contains('similar-coach-avatar')) {
                    event.target.src = '/default-coach.jpg'
                } else {
                    event.target.src = '/default-blog.jpg'
                }
            }

            // Lifecycle
            onMounted(async () => {
                await loadCoach()
            })

            // Watch for route changes
            watch(() => route.params.coachId, async (newId, oldId) => {
                if (newId !== oldId) {
                    await loadCoach()
                }
            })

            return {
                // Reactive data
                coach,
                reviews,
                similarCoaches,
                loading,
                loadingReviews,
                error,
                activeTab,
                selectedSessionType,
                isSendingMessage,
                defaultSessionTypes,

                // Computed
                isAuthenticated,

                // Methods
                sendMessage,
                formatDate,
                formatPrice,
                calculateDiscount,
                handleImageError
            }
        }
    }
</script>

<style scoped>
    .coach-profile-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        margin-bottom: 2rem;
    }

    .coach-header {
        border-bottom: 1px solid #eee;
        padding-bottom: 2rem;
        margin-bottom: 2rem;
    }

    .coach-avatar {
        position: relative;
        display: inline-block;
    }

        .coach-avatar img {
            width: 200px;
            height: 200px;
            object-fit: cover;
            border: 4px solid #007bff;
            border-radius: 50%;
        }

    .online-indicator {
        position: absolute;
        bottom: 10px;
        right: 10px;
        color: #28a745;
        font-size: 1.2rem;
        background: white;
        border-radius: 50%;
        padding: 5px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .coach-name {
        font-size: 2.5rem;
        font-weight: 700;
        color: #333;
        margin-bottom: 0.5rem;
    }

    .coach-title {
        font-size: 1.5rem;
        margin-bottom: 1rem;
        color: #007bff;
    }

    .coach-rating {
        margin-bottom: 1.5rem;
    }

    .stars i {
        font-size: 1.2rem;
        color: #ddd;
        margin-right: 2px;
    }

        .stars i.text-warning {
            color: #ffc107 !important;
        }

    .rating-text {
        font-size: 0.9rem;
        color: #666;
    }

    .coach-stats {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 1.5rem;
        margin-top: 1rem;
    }

    .stat-item {
        text-align: center;
    }

    .stat-number {
        font-size: 2rem;
        font-weight: 700;
        color: #007bff;
        margin-bottom: 0.5rem;
    }

    .stat-label {
        color: #666;
        font-size: 0.9rem;
        margin: 0;
    }

    .coach-tabs {
        margin-top: 2rem;
    }

    .nav-tabs {
        border-bottom: 2px solid #dee2e6;
        margin-bottom: 2rem;
    }

        .nav-tabs .nav-link {
            border: none;
            color: #666;
            font-weight: 500;
            padding: 1rem 1.5rem;
            border-radius: 0;
            position: relative;
            transition: all 0.3s ease;
        }

            .nav-tabs .nav-link:hover {
                color: #007bff;
                border-color: transparent;
            }

            .nav-tabs .nav-link.active {
                color: #007bff;
                background: none;
                border: none;
            }

                .nav-tabs .nav-link.active::after {
                    content: '';
                    position: absolute;
                    bottom: -2px;
                    left: 0;
                    right: 0;
                    height: 2px;
                    background: #007bff;
                }

    .tab-content {
        min-height: 300px;
    }

    .about-content {
        line-height: 1.8;
        color: #444;
    }

        .about-content h4,
        .about-content h5 {
            color: #333;
            margin-top: 2rem;
            margin-bottom: 1rem;
        }

    .languages .badge {
        font-size: 0.9rem;
        padding: 0.5rem 0.75rem;
        margin-right: 0.5rem;
        border: 1px solid #ddd;
    }

    .specialties-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        gap: 1.5rem;
        margin-top: 1.5rem;
    }

    .specialty-card {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 1.5rem;
        text-align: center;
        transition: all 0.3s ease;
        border: 1px solid #eee;
    }

        .specialty-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
        }

    .specialty-icon {
        font-size: 2rem;
        color: #007bff;
        margin-bottom: 1rem;
    }

    .specialty-card h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .specialty-card p {
        color: #666;
        line-height: 1.6;
        margin: 0;
    }

    .education-section,
    .certifications-section {
        margin-bottom: 2rem;
    }

    .education-item,
    .certification-item {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 1.5rem;
        margin-bottom: 1rem;
        border-left: 4px solid #007bff;
    }

    .education-header,
    .certification-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 0.5rem;
    }

        .education-header h6,
        .certification-header h6 {
            color: #333;
            font-weight: 600;
            margin: 0;
        }

    .education-year,
    .certification-year {
        background: #007bff;
        color: white;
        padding: 0.25rem 0.5rem;
        border-radius: 15px;
        font-size: 0.8rem;
        font-weight: 500;
    }

    .education-school,
    .certification-issuer {
        color: #666;
        font-weight: 500;
        margin-bottom: 0.5rem;
    }

    .education-description,
    .certification-description {
        color: #555;
        line-height: 1.6;
        margin: 0;
    }

    .reviews-list {
        max-height: 600px;
        overflow-y: auto;
    }

    .review-item {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 1.5rem;
        margin-bottom: 1rem;
        border: 1px solid #eee;
    }

    .review-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 1rem;
    }

    .reviewer-info {
        display: flex;
        align-items: center;
        gap: 1rem;
    }

    .reviewer-avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        object-fit: cover;
    }

    .reviewer-info h6 {
        color: #333;
        margin-bottom: 0.25rem;
    }

    .review-rating {
        margin-top: 0.25rem;
    }

        .review-rating i {
            font-size: 0.9rem;
            color: #ddd;
        }

            .review-rating i.text-warning {
                color: #ffc107 !important;
            }

    .review-date {
        color: #666;
        font-size: 0.9rem;
    }

    .review-comment {
        color: #444;
        line-height: 1.6;
        margin: 0;
    }

    .blogs-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 1.5rem;
        margin-top: 1.5rem;
    }

    .blog-card {
        background: white;
        border-radius: 10px;
        overflow: hidden;
        box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
    }

        .blog-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
        }

    .blog-image img {
        width: 100%;
        height: 200px;
        object-fit: cover;
    }

    .blog-content {
        padding: 1.5rem;
    }

    .blog-meta {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
        font-size: 0.9rem;
    }

    .blog-date {
        color: #666;
    }

    .blog-category {
        background: #007bff;
        color: white;
        padding: 0.25rem 0.5rem;
        border-radius: 12px;
        font-size: 0.8rem;
    }

    .blog-card h6 a {
        color: #333;
        text-decoration: none;
        transition: color 0.3s ease;
    }

        .blog-card h6 a:hover {
            color: #007bff;
        }

    .blog-excerpt {
        color: #666;
        line-height: 1.6;
        margin: 0;
    }

    /* Sidebar Styles */
    .coach-sidebar {
        padding-left: 2rem;
    }

    .booking-card,
    .contact-card,
    .similar-coaches-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        margin-bottom: 2rem;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
    }

        .booking-card h5,
        .contact-card h5,
        .similar-coaches-card h5 {
            color: #333;
            font-weight: 600;
            margin-bottom: 1.5rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #007bff;
        }

    .price-info {
        text-align: center;
        margin-bottom: 2rem;
    }

    .price .amount {
        font-size: 2.5rem;
        font-weight: 700;
        color: #007bff;
    }

    .price .period {
        color: #666;
        font-size: 1rem;
    }

    .discount {
        margin-top: 0.5rem;
    }

    .original-price {
        text-decoration: line-through;
        color: #666;
        font-size: 1.1rem;
    }

    .discount-badge {
        background: #28a745;
        color: white;
        padding: 0.25rem 0.5rem;
        border-radius: 12px;
        font-size: 0.8rem;
        margin-left: 0.5rem;
    }

    .session-types h6 {
        color: #333;
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .session-type {
        margin-bottom: 1rem;
    }

        .session-type .form-check-label {
            cursor: pointer;
            width: 100%;
            padding: 1rem;
            border: 2px solid #eee;
            border-radius: 10px;
            transition: all 0.3s ease;
            display: block;
        }

        .session-type .form-check-input:checked + .form-check-label {
            border-color: #007bff;
            background: rgba(0, 123, 255, 0.1);
        }

        .session-type .form-check-label:hover {
            border-color: #007bff;
        }

    .session-duration {
        color: #666;
        font-size: 0.9rem;
        margin-left: 0.5rem;
    }

    .session-price {
        color: #007bff;
        font-weight: 600;
        margin-top: 0.5rem;
        display: block;
    }

    .booking-actions .btn {
        font-weight: 500;
        border-radius: 25px;
        padding: 0.75rem 1.5rem;
        transition: all 0.3s ease;
    }

        .booking-actions .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
        }

    .contact-item {
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
        padding: 0.75rem;
        background: #f8f9fa;
        border-radius: 8px;
    }

        .contact-item i {
            color: #007bff;
            width: 20px;
            margin-right: 1rem;
        }

        .contact-item a {
            color: #007bff;
            text-decoration: none;
        }

            .contact-item a:hover {
                text-decoration: underline;
            }

    .social-links {
        display: flex;
        gap: 1rem;
        margin-top: 1rem;
    }

        .social-links a {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 40px;
            height: 40px;
            background: #007bff;
            color: white;
            border-radius: 50%;
            text-decoration: none;
            transition: all 0.3s ease;
        }

            .social-links a:hover {
                background: #0056b3;
                transform: translateY(-2px);
                box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
            }

    .similar-coach-item {
        margin-bottom: 1rem;
    }

    .similar-coach-link {
        display: flex;
        align-items: center;
        gap: 1rem;
        padding: 1rem;
        background: #f8f9fa;
        border-radius: 10px;
        text-decoration: none;
        color: inherit;
        transition: all 0.3s ease;
    }

        .similar-coach-link:hover {
            background: #e9ecef;
            transform: translateX(5px);
            color: inherit;
        }

    .similar-coach-avatar {
        width: 60px;
        height: 60px;
        border-radius: 50%;
        object-fit: cover;
    }

    .similar-coach-info h6 {
        color: #333;
        margin-bottom: 0.25rem;
    }

    .similar-coach-info p {
        color: #666;
        font-size: 0.9rem;
        margin-bottom: 0.5rem;
    }

    .similar-coach-rating {
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

        .similar-coach-rating .stars i {
            font-size: 0.8rem;
        }

        .similar-coach-rating span {
            font-size: 0.8rem;
            color: #666;
        }

    /* Responsive Design */
    @media (max-width: 992px) {
        .coach-sidebar {
            padding-left: 0;
            margin-top: 2rem;
        }
    }

    @media (max-width: 768px) {
        .coach-profile-card {
            padding: 1.5rem;
        }

        .coach-header {
            text-align: center;
        }

        .coach-avatar img {
            width: 150px;
            height: 150px;
        }

        .coach-name {
            font-size: 2rem;
        }

        .coach-title {
            font-size: 1.2rem;
        }

        .stat-number {
            font-size: 1.5rem;
        }

        .price .amount {
            font-size: 2rem;
        }

        .nav-tabs .nav-link {
            padding: 0.75rem 1rem;
            font-size: 0.9rem;
        }

        .specialties-grid,
        .blogs-grid {
            grid-template-columns: 1fr;
        }

        .booking-card,
        .contact-card,
        .similar-coaches-card {
            padding: 1.5rem;
        }

        .education-header,
        .certification-header {
            flex-direction: column;
            align-items: flex-start;
        }

        .education-year,
        .certification-year {
            margin-top: 0.5rem;
        }
    }

    @media (max-width: 576px) {
        .coach-stats .row {
            flex-direction: column;
        }

        .coach-stats .col-4 {
            margin-bottom: 1rem;
        }

        .review-header {
            flex-direction: column;
            align-items: flex-start;
        }

        .review-date {
            margin-top: 0.5rem;
        }

        .similar-coach-link {
            flex-direction: column;
            text-align: center;
        }

        .session-type .form-check-label {
            padding: 0.75rem;
            font-size: 0.9rem;
        }
    }

    /* Animation */
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

    .coach-profile-card,
    .booking-card,
    .contact-card,
    .similar-coaches-card {
        animation: fadeInUp 0.6s ease-out;
    }

    .specialty-card,
    .blog-card {
        animation: fadeInUp 0.6s ease-out;
        animation-fill-mode: both;
    }

        .specialty-card:nth-child(2) {
            animation-delay: 0.1s;
        }

        .specialty-card:nth-child(3) {
            animation-delay: 0.2s;
        }

        .specialty-card:nth-child(4) {
            animation-delay: 0.3s;
        }

        .blog-card:nth-child(2) {
            animation-delay: 0.1s;
        }

        .blog-card:nth-child(3) {
            animation-delay: 0.2s;
        }

    /* Loading states */
    .loading-skeleton {
        background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
        background-size: 200% 100%;
        animation: loading 1.5s infinite;
    }

    @keyframes loading {
        0% {
            background-position: 200% 0;
        }

        100% {
            background-position: -200% 0;
        }
    }

    /* Print styles */
    @media print {
        .coach-sidebar,
        .booking-actions,
        .nav-tabs {
            display: none !important;
        }

        .coach-profile-card {
            box-shadow: none;
            border: 1px solid #ddd;
        }
    }

    /* Dark theme support */
    @media (prefers-color-scheme: dark) {
        .coach-profile-card,
        .booking-card,
        .contact-card,
        .similar-coaches-card {
            background: #2d3748;
            color: #e2e8f0;
        }

        .coach-name {
            color: #e2e8f0;
        }

        .specialty-card,
        .education-item,
        .certification-item,
        .review-item,
        .contact-item,
        .similar-coach-link {
            background: #4a5568;
            color: #e2e8f0;
        }

        .blog-card {
            background: #4a5568;
            color: #e2e8f0;
        }
    }

    /* Accessibility improvements */
    @media (prefers-reduced-motion: reduce) {
        .specialty-card,
        .blog-card,
        .similar-coach-link,
        .booking-actions .btn {
            transition: none;
        }

        .coach-profile-card,
        .booking-card,
        .contact-card,
        .similar-coaches-card,
        .specialty-card,
        .blog-card {
            animation: none;
        }
    }

    /* Focus styles for accessibility */
    .nav-tabs .nav-link:focus,
    .form-check-input:focus,
    .btn:focus {
        outline: 2px solid #007bff;
        outline-offset: 2px;
    }

    /* High contrast mode */
    @media (prefers-contrast: high) {
        .nav-tabs .nav-link.active::after {
            background: currentColor;
        }

        .specialty-card:hover,
        .blog-card:hover,
        .similar-coach-link:hover {
            outline: 2px solid currentColor;
        }
    }
</style>