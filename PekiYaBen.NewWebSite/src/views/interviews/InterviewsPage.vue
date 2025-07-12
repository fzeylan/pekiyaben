<template>
  <div class="interviews-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Online Koçluk ve Atölye</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Online Koçluk</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Hero Section -->
    <section class="hero-section py-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-6">
            <div class="hero-content">
              <h1 class="hero-title">
                Uzaktan Kişisel Gelişim <span class="text-primary">Deneyimi</span>
              </h1>
              <p class="hero-description">
                Evinizin konforundan, uzman koçlarımızla birebir görüşmeler yaparak 
                hedeflerinize daha hızlı ulaşın. Online koçluk seanslarımız ile 
                coğrafi sınırları aşın.
              </p>
              <div class="hero-stats">
                <div class="row">
                  <div class="col-md-4">
                    <div class="stat-item">
                      <h3>500+</h3>
                      <p>Online Seans</p>
                    </div>
                  </div>
                  <div class="col-md-4">
                    <div class="stat-item">
                      <h3>98%</h3>
                      <p>Memnuniyet</p>
                    </div>
                  </div>
                  <div class="col-md-4">
                    <div class="stat-item">
                      <h3>24/7</h3>
                      <p>Destek</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-lg-6">
            <div class="hero-image">
              <img src="/assets/img/online-coaching.jpg" alt="Online Koçluk" class="img-fluid" />
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Services Section -->
    <section class="services-section py-5 bg-light">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Hizmetlerimiz</h2>
          <p class="section-subtitle">
            İhtiyaçlarınıza özel tasarlanmış koçluk hizmetlerimizi keşfedin
          </p>
        </div>

        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>

        <div v-else class="row">
          <div 
            v-for="service in services" 
            :key="service.id" 
            class="col-lg-6 col-xl-4 mb-4"
          >
            <div class="service-card" :class="{ 'featured': service.id === selectedServiceId }">
              <div class="service-image">
                <img 
                  :src="service.image || '/default-service.jpg'" 
                  :alt="service.title"
                  @error="handleImageError"
                />
                <div class="service-overlay">
                  <div class="service-price">
                    {{ formatPrice(service.price) }}
                  </div>
                </div>
              </div>
              
              <div class="service-content">
                <h4 class="service-title">{{ service.title }}</h4>
                <p class="service-description">{{ service.description }}</p>
                
                <div class="service-features">
                  <ul>
                    <li v-for="feature in getServiceFeatures(service)" :key="feature">
                      <i class="fas fa-check text-success me-2"></i>
                      {{ feature }}
                    </li>
                  </ul>
                </div>
                
                <div class="service-meta">
                  <div class="duration">
                    <i class="fas fa-clock me-1"></i>
                    {{ service.duration || '60' }} dakika
                  </div>
                  <div class="format">
                    <i class="fas fa-video me-1"></i>
                    Online
                  </div>
                </div>
                
                <div class="service-actions">
                  <button 
                    @click="viewCoaches(service)"
                    class="btn btn-outline-primary"
                  >
                    Koçları Görüntüle
                  </button>
                  <button 
                    @click="bookService(service)"
                    class="btn btn-primary"
                  >
                    Randevu Al
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- How It Works Section -->
    <section class="how-it-works-section py-5">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Nasıl Çalışır?</h2>
          <p class="section-subtitle">
            Online koçluk sürecimiz 4 basit adımda gerçekleşir
          </p>
        </div>
        
        <div class="row">
          <div class="col-lg-3 col-md-6 mb-4" v-for="(step, index) in howItWorksSteps" :key="index">
            <div class="step-card">
              <div class="step-number">{{ index + 1 }}</div>
              <div class="step-icon">
                <i :class="step.icon"></i>
              </div>
              <h5>{{ step.title }}</h5>
              <p>{{ step.description }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Available Coaches Section -->
    <section v-if="availableCoaches.length > 0" class="coaches-section py-5 bg-light">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Uzman Koçlarımız</h2>
          <p class="section-subtitle">
            Seçtiğiniz hizmet için uygun koçlarımızla tanışın
          </p>
        </div>
        
        <div class="row">
          <div 
            v-for="coach in availableCoaches" 
            :key="coach.id" 
            class="col-lg-4 col-md-6 mb-4"
          >
            <div class="coach-card">
              <div class="coach-image">
                <img 
                  :src="coach.profilePhoto || '/default-coach.jpg'" 
                  :alt="coach.fullName"
                  @error="handleImageError"
                />
              </div>
              
              <div class="coach-info">
                <h5>{{ coach.fullName }}</h5>
                <p class="coach-title">{{ coach.title }}</p>
                <div class="coach-rating">
                  <div class="stars">
                    <i v-for="star in 5" :key="star" 
                       :class="star <= (coach.rating || 0) ? 'fas fa-star' : 'far fa-star'"></i>
                  </div>
                  <span>({{ coach.reviewCount || 0 }} değerlendirme)</span>
                </div>
                <p class="coach-description">{{ truncateText(coach.description, 100) }}</p>
                
                <div class="coach-actions">
                  <button 
                    @click="viewCoachDetail(coach)"
                    class="btn btn-outline-primary btn-sm"
                  >
                    Detaylar
                  </button>
                  <button 
                    @click="bookWithCoach(coach)"
                    class="btn btn-primary btn-sm"
                  >
                    Randevu Al
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- FAQ Section -->
    <section class="faq-section py-5">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Sıkça Sorulan Sorular</h2>
          <p class="section-subtitle">
            Online koçluk hakkında merak ettikleriniz
          </p>
        </div>
        
        <div class="row justify-content-center">
          <div class="col-lg-8">
            <div class="accordion" id="faqAccordion">
              <div 
                v-for="(faq, index) in faqs" 
                :key="faq.id" 
                class="accordion-item"
              >
                <h2 class="accordion-header">
                  <button 
                    class="accordion-button"
                    :class="{ collapsed: activeFaq !== index }"
                    @click="toggleFaq(index)"
                  >
                    {{ faq.question }}
                  </button>
                </h2>
                <div 
                  class="accordion-collapse collapse"
                  :class="{ show: activeFaq === index }"
                >
                  <div class="accordion-body">
                    {{ faq.answer }}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section bg-primary text-white py-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-lg-8">
            <h3>Kişisel Gelişim Yolculuğunuza Bugün Başlayın</h3>
            <p class="mb-0">
              Uzman koçlarımızla online görüşme yaparak hedeflerinize daha hızlı ulaşın.
            </p>
          </div>
          <div class="col-lg-4 text-lg-end">
            <button @click="scrollToServices" class="btn btn-light btn-lg">
              Hizmetleri İncele
            </button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import contentService from '@/services/content'
import coachesService from '@/services/coaches'

export default {
  name: 'InterviewsPage',
  setup() {
    const route = useRoute()
    const router = useRouter()
    
    const services = ref([])
    const availableCoaches = ref([])
    const loading = ref(true)
    const activeFaq = ref(null)
    
    const selectedServiceId = computed(() => {
      return route.params.id ? parseInt(route.params.id) : null
    })
    
    const howItWorksSteps = ref([
      {
        icon: 'fas fa-calendar-check',
        title: 'Hizmet Seçin',
        description: 'İhtiyacınıza uygun koçluk hizmetini ve koçunuzu seçin.'
      },
      {
        icon: 'fas fa-clock',
        title: 'Randevu Alın',
        description: 'Müsait tarih ve saatlerden size uygun olanı seçin.'
      },
      {
        icon: 'fas fa-video',
        title: 'Online Görüşme',
        description: 'Belirlenen tarihte online platformda koçunuzla buluşun.'
      },
      {
        icon: 'fas fa-chart-line',
        title: 'Gelişim Takibi',
        description: 'İlerlemenizi takip edin ve hedeflerinize ulaşın.'
      }
    ])
    
    const faqs = ref([
      {
        id: 1,
        question: 'Online koçluk seansları nasıl gerçekleşir?',
        answer: 'Online koçluk seanslarımız güvenli video konferans platformları üzerinden gerçekleşir. Randevu saatinizde size gönderilen bağlantıya tıklayarak koçunuzla buluşabilirsiniz.'
      },
      {
        id: 2,
        question: 'Teknik gereksinimler nelerdir?',
        answer: 'Sadece internet bağlantısı olan bir bilgisayar, tablet veya telefona ihtiyacınız var. Kamera ve mikrofonun çalışır durumda olması önemlidir.'
      },
      {
        id: 3,
        question: 'Randevumu iptal edebilir miyim?',
        answer: 'Evet, randevunuzu en az 24 saat öncesinden iptal edebilirsiniz. Bu durumda ücret iadesi yapılır.'
      },
      {
        id: 4,
        question: 'Koçluk seansının süresi ne kadar?',
        answer: 'Standart koçluk seanslarımız 60 dakika sürmektedir. İhtiyaca göre daha kısa veya uzun seanslar da düzenlenebilir.'
      },
      {
        id: 5,
        question: 'Koçumla uyum sağlayamazsam ne olur?',
        answer: 'İlk seanstan sonra koçunuzla uyum sağlayamadığınızı düşünüyorsanız, başka bir koça geçiş yapabilirsiniz.'
      }
    ])
    
    const loadServices = async () => {
      try {
        loading.value = true
        const response = await contentService.getProducts()
        services.value = response.filter(product => product.type === 3 && product.status === 1)
        
        // If a specific service is selected, load coaches for that service
        if (selectedServiceId.value) {
          const selectedService = services.value.find(s => s.id === selectedServiceId.value)
          if (selectedService) {
            await loadCoachesForService(selectedService.title)
          }
        }
      } catch (error) {
        console.error('Error loading services:', error)
        services.value = []
      } finally {
        loading.value = false
      }
    }
    
    const loadCoachesForService = async (serviceTitle) => {
      try {
        const response = await coachesService.getCoaches(serviceTitle)
        availableCoaches.value = response.map(coach => ({
          ...coach,
          rating: Math.random() * 2 + 3, // Mock rating
          reviewCount: Math.floor(Math.random() * 50) + 5 // Mock review count
        }))
      } catch (error) {
        console.error('Error loading coaches:', error)
        availableCoaches.value = []
      }
    }
    
    const getServiceFeatures = (service) => {
      // Mock features based on service type
      const features = [
        'Birebir görüşme',
        'Kişiselleştirilmiş plan',
        'İlerleme takibi',
        'Esneklik'
      ]
      return features.slice(0, 3)
    }
    
    const formatPrice = (price) => {
      if (!price) return '0 ₺'
      return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
      }).format(price)
    }
    
    const truncateText = (text, length) => {
      if (!text) return ''
      return text.length > length ? text.substring(0, length) + '...' : text
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-service.jpg'
    }
    
    const viewCoaches = async (service) => {
      await loadCoachesForService(service.title)
      // Scroll to coaches section
      const coachesSection = document.querySelector('.coaches-section')
      if (coachesSection) {
        coachesSection.scrollIntoView({ behavior: 'smooth' })
      }
    }
    
    const bookService = (service) => {
      router.push({
        name: 'InterviewsDetail',
        params: {
          id: service.id,
          title: service.title.toLowerCase().replace(/\s+/g, '-')
        }
      })
    }
    
    const viewCoachDetail = (coach) => {
      const service = services.value.find(s => 
        coach.coachingInfo && coach.coachingInfo.includes(s.title)
      ) || services.value[0]
      
      if (service) {
        router.push({
          name: 'CoachDetail',
          params: {
            id: service.id,
            title: service.title.toLowerCase().replace(/\s+/g, '-'),
            coachId: coach.id,
            coachName: coach.fullName.toLowerCase().replace(/\s+/g, '-')
          }
        })
      }
    }
    
    const bookWithCoach = (coach) => {
      const service = services.value.find(s => 
        coach.coachingInfo && coach.coachingInfo.includes(s.title)
      ) || services.value[0]
      
      if (service) {
        router.push({
          name: 'InterviewCalendar',
          params: {
            id: service.id,
            title: service.title.toLowerCase().replace(/\s+/g, '-'),
            coachId: coach.id,
            coachName: coach.fullName.toLowerCase().replace(/\s+/g, '-')
          }
        })
      }
    }
    
    const toggleFaq = (index) => {
      activeFaq.value = activeFaq.value === index ? null : index
    }
    
    const scrollToServices = () => {
      const servicesSection = document.querySelector('.services-section')
      if (servicesSection) {
        servicesSection.scrollIntoView({ behavior: 'smooth' })
      }
    }
    
    onMounted(() => {
      loadServices()
    })
    
    return {
      services,
      availableCoaches,
      loading,
      activeFaq,
      selectedServiceId,
      howItWorksSteps,
      faqs,
      getServiceFeatures,
      formatPrice,
      truncateText,
      handleImageError,
      viewCoaches,
      bookService,
      viewCoachDetail,
      bookWithCoach,
      toggleFaq,
      scrollToServices
    }
  }
}
</script>

<style scoped>
.interviews-page {
  min-height: 100vh;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.hero-section {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.hero-title {
  font-size: 3rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  line-height: 1.2;
  color: #2c3e50;
}

.hero-description {
  font-size: 1.2rem;
  color: #6c757d;
  margin-bottom: 2rem;
  line-height: 1.6;
}

.hero-stats {
  margin-top: 2rem;
}

.stat-item {
  text-align: center;
  padding: 1rem;
}

.stat-item h3 {
  font-size: 2.5rem;
  font-weight: 700;
  color: #007bff;
  margin-bottom: 0.5rem;
}

.stat-item p {
  color: #6c757d;
  font-weight: 500;
  margin: 0;
}

.hero-image img {
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}

.section-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.section-subtitle {
  font-size: 1.1rem;
  color: #6c757d;
  max-width: 600px;
  margin: 0 auto;
}

.service-card {
  background: white;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.service-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.service-card.featured {
  border: 3px solid #007bff;
  transform: scale(1.02);
}

.service-image {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.service-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.service-card:hover .service-image img {
  transform: scale(1.1);
}

.service-overlay {
  position: absolute;
  top: 1rem;
  right: 1rem;
}

.service-price {
  background: rgba(0, 123, 255, 0.9);
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  font-size: 1.1rem;
}

.service-content {
  padding: 1.5rem;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.service-title {
  font-size: 1.25rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.service-description {
  color: #6c757d;
  line-height: 1.6;
  margin-bottom: 1.5rem;
  flex: 1;
}

.service-features ul {
  list-style: none;
  padding: 0;
  margin-bottom: 1.5rem;
}

.service-features li {
  color: #495057;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
}

.service-meta {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
  font-size: 0.9rem;
  color: #6c757d;
}

.service-actions {
  display: flex;
  gap: 0.5rem;
}

.service-actions .btn {
  flex: 1;
  border-radius: 8px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.service-actions .btn:hover {
  transform: translateY(-2px);
}

.step-card {
  text-align: center;
  padding: 2rem 1rem;
  background: white;
  border-radius: 15px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  height: 100%;
}

.step-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.step-number {
  width: 50px;
  height: 50px;
  background: #007bff;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  font-size: 1.2rem;
  margin: 0 auto 1rem;
}

.step-icon {
  font-size: 2.5rem;
  color: #007bff;
  margin-bottom: 1rem;
}

.step-card h5 {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.step-card p {
  color: #6c757d;
  line-height: 1.6;
  margin: 0;
}

.coach-card {
  background: white;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  height: 100%;
}

.coach-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.coach-image {
  height: 200px;
  overflow: hidden;
}

.coach-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.coach-info {
  padding: 1.5rem;
}

.coach-info h5 {
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.coach-title {
  color: #007bff;
  font-weight: 600;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
}

.coach-rating {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 1rem;
  font-size: 0.85rem;
}

.stars {
  color: #ffc107;
}

.coach-description {
  color: #6c757d;
  font-size: 0.9rem;
  line-height: 1.5;
  margin-bottom: 1.5rem;
}

.coach-actions {
  display: flex;
  gap: 0.5rem;
}

.coach-actions .btn {
  flex: 1;
  border-radius: 8px;
  font-weight: 500;
}

.accordion-item {
  border: 2px solid #e9ecef;
  border-radius: 10px !important;
  margin-bottom: 1rem;
  overflow: hidden;
}

.accordion-button {
  background: white;
  border: none;
  padding: 1.5rem;
  font-weight: 600;
  color: #2c3e50;
  border-radius: 10px !important;
}

.accordion-button:not(.collapsed) {
  background: #f8f9fa;
  color: #007bff;
  box-shadow: none;
}

.accordion-button:focus {
  box-shadow: none;
  border: none;
}

.accordion-body {
  padding: 1.5rem;
  background: #f8f9fa;
  color: #6c757d;
  line-height: 1.6;
}

.cta-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.cta-section h3 {
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.btn-light {
  border-radius: 25px;
  padding: 12px 30px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-light:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(255, 255, 255, 0.3);
}

/* Responsive */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2rem;
  }
  
  .service-actions,
  .coach-actions {
    flex-direction: column;
  }
  
  .service-meta {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .cta-section .row {
    text-align: center;
  }
  
  .cta-section .col-lg-4 {
    margin-top: 1rem;
  }
}

/* Animations */
.service-card,
.step-card,
.coach-card {
  animation: fadeInUp 0.6s ease forwards;
}

.service-card:nth-child(1) { animation-delay: 0.1s; }
.service-card:nth-child(2) { animation-delay: 0.2s; }
.service-card:nth-child(3) { animation-delay: 0.3s; }

.step-card:nth-child(1) { animation-delay: 0.1s; }
.step-card:nth-child(2) { animation-delay: 0.2s; }
.step-card:nth-child(3) { animation-delay: 0.3s; }
.step-card:nth-child(4) { animation-delay: 0.4s; }

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

/* Loading State */
.spinner-border {
  width: 3rem;
  height: 3rem;
}
</style>