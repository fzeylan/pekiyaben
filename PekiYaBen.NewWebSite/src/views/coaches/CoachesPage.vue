<template>
  <div class="coaches-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Koçluk Ekibimiz</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Ekibimiz</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Filters Section -->
    <section class="filters-section py-4 bg-light">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-md-6">
            <h5 class="mb-0">
              {{ filteredCoaches.length }} koç bulundu
            </h5>
          </div>
          <div class="col-md-6">
            <div class="d-flex gap-3">
              <!-- Service Filter -->
              <select v-model="selectedService" class="form-select" @change="applyFilters">
                <option value="">Tüm Hizmetler</option>
                <option v-for="service in services" :key="service.id" :value="service.title">
                  {{ service.title }}
                </option>
              </select>
              
              <!-- Sort -->
              <select v-model="sortBy" class="form-select" @change="applySorting">
                <option value="name">İsme Göre</option>
                <option value="rating">Puana Göre</option>
                <option value="experience">Deneyime Göre</option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Coaches Grid -->
    <section class="coaches-grid-section py-5">
      <div class="container">
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Yükleniyor...</span>
          </div>
        </div>
        
        <div v-else class="row">
          <div 
            v-for="coach in paginatedCoaches" 
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
                <div class="coach-overlay">
                  <div class="coach-actions">
                    <button 
                      class="btn btn-primary btn-sm"
                      @click="viewCoachDetail(coach)"
                    >
                      Detaylar
                    </button>
                    <button 
                      class="btn btn-success btn-sm"
                      @click="bookAppointment(coach)"
                    >
                      Randevu Al
                    </button>
                  </div>
                </div>
              </div>
              
              <div class="coach-info">
                <h5 class="coach-name">{{ coach.fullName }}</h5>
                <p class="coach-title">{{ coach.title }}</p>
                
                <div class="coach-specialties">
                  <span 
                    v-for="specialty in getCoachSpecialties(coach)" 
                    :key="specialty"
                    class="badge bg-light text-dark me-1"
                  >
                    {{ specialty }}
                  </span>
                </div>
                
                <p class="coach-description">
                  {{ truncateText(coach.description, 100) }}
                </p>
                
                <div class="coach-meta">
                  <div class="rating">
                    <div class="stars">
                      <i v-for="star in 5" :key="star" 
                         :class="star <= coach.rating ? 'fas fa-star' : 'far fa-star'"></i>
                    </div>
                    <span class="rating-text">({{ coach.reviewCount }} değerlendirme)</span>
                  </div>
                  
                  <div class="experience">
                    <i class="fas fa-medal me-1"></i>
                    {{ coach.experience }} yıl deneyim
                  </div>
                </div>
                
                <div class="coach-services">
                  <h6>Hizmetler:</h6>
                  <ul class="services-list">
                    <li v-for="service in getCoachServices(coach)" :key="service">
                      {{ service }}
                    </li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- Pagination -->
        <div v-if="totalPages > 1" class="pagination-wrapper">
          <nav aria-label="Coaches pagination">
            <ul class="pagination justify-content-center">
              <li class="page-item" :class="{ disabled: currentPage === 1 }">
                <button 
                  class="page-link" 
                  @click="changePage(currentPage - 1)"
                  :disabled="currentPage === 1"
                >
                  Önceki
                </button>
              </li>
              
              <li 
                v-for="page in visiblePages" 
                :key="page"
                class="page-item" 
                :class="{ active: page === currentPage }"
              >
                <button class="page-link" @click="changePage(page)">
                  {{ page }}
                </button>
              </li>
              
              <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                <button 
                  class="page-link" 
                  @click="changePage(currentPage + 1)"
                  :disabled="currentPage === totalPages"
                >
                  Sonraki
                </button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import coachesService from '@/services/coaches'
import contentService from '@/services/content'

export default {
  name: 'CoachesPage',
  setup() {
    const router = useRouter()
    
    const coaches = ref([])
    const services = ref([])
    const loading = ref(true)
    const selectedService = ref('')
    const sortBy = ref('name')
    const currentPage = ref(1)
    const itemsPerPage = 6
    
    const filteredCoaches = computed(() => {
      let filtered = coaches.value
      
      if (selectedService.value) {
        filtered = filtered.filter(coach => 
          coach.coachingInfo && coach.coachingInfo.includes(selectedService.value)
        )
      }
      
      return filtered
    })
    
    const sortedCoaches = computed(() => {
      const sorted = [...filteredCoaches.value]
      
      switch (sortBy.value) {
        case 'rating':
          return sorted.sort((a, b) => (b.rating || 0) - (a.rating || 0))
        case 'experience':
          return sorted.sort((a, b) => (b.experience || 0) - (a.experience || 0))
        default:
          return sorted.sort((a, b) => a.fullName.localeCompare(b.fullName))
      }
    })
    
    const totalPages = computed(() => 
      Math.ceil(sortedCoaches.value.length / itemsPerPage)
    )
    
    const paginatedCoaches = computed(() => {
      const start = (currentPage.value - 1) * itemsPerPage
      const end = start + itemsPerPage
      return sortedCoaches.value.slice(start, end)
    })
    
    const visiblePages = computed(() => {
      const pages = []
      const total = totalPages.value
      const current = currentPage.value
      
      if (total <= 7) {
        for (let i = 1; i <= total; i++) {
          pages.push(i)
        }
      } else {
        if (current <= 4) {
          for (let i = 1; i <= 5; i++) {
            pages.push(i)
          }
          pages.push('...')
          pages.push(total)
        } else if (current >= total - 3) {
          pages.push(1)
          pages.push('...')
          for (let i = total - 4; i <= total; i++) {
            pages.push(i)
          }
        } else {
          pages.push(1)
          pages.push('...')
          for (let i = current - 1; i <= current + 1; i++) {
            pages.push(i)
          }
          pages.push('...')
          pages.push(total)
        }
      }
      
      return pages
    })
    
    const loadCoaches = async () => {
      try {
        loading.value = true
        const response = await coachesService.getCoaches()
        
        // Enrich coaches data with mock ratings and experience
        coaches.value = response.map(coach => ({
          ...coach,
          rating: Math.random() * 2 + 3, // Random rating between 3-5
          reviewCount: Math.floor(Math.random() * 50) + 5,
          experience: Math.floor(Math.random() * 15) + 2, // 2-17 years experience
          profilePhoto: coach.profilePhoto ? `/coaches/${coach.profilePhoto}` : null
        }))
      } catch (error) {
        console.error('Error loading coaches:', error)
      } finally {
        loading.value = false
      }
    }
    
    const loadServices = async () => {
      try {
        const response = await contentService.getProducts()
        services.value = response.filter(product => product.type === 3) // Interview type products
      } catch (error) {
        console.error('Error loading services:', error)
      }
    }
    
    const getCoachSpecialties = (coach) => {
      if (!coach.coachingInfo) return []
      return coach.coachingInfo.split(',').map(s => s.trim()).slice(0, 3)
    }
    
    const getCoachServices = (coach) => {
      const allServices = services.value.map(s => s.title)
      return allServices.filter(service => 
        coach.coachingInfo && coach.coachingInfo.includes(service)
      ).slice(0, 4)
    }
    
    const truncateText = (text, length) => {
      if (!text) return ''
      return text.length > length ? text.substring(0, length) + '...' : text
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-coach.jpg'
    }
    
    const applyFilters = () => {
      currentPage.value = 1
    }
    
    const applySorting = () => {
      currentPage.value = 1
    }
    
    const changePage = (page) => {
      if (page >= 1 && page <= totalPages.value) {
        currentPage.value = page
        window.scrollTo({ top: 0, behavior: 'smooth' })
      }
    }
    
    const viewCoachDetail = (coach) => {
      // Find a service to use in the URL (required by the original route structure)
      const service = services.value.find(s => 
        coach.coachingInfo && coach.coachingInfo.includes(s.title)
      ) || services.value[0]
      
      if (service) {
        const serviceTitle = service.title.toLowerCase().replace(/\s+/g, '-')
        const coachName = coach.fullName.toLowerCase().replace(/\s+/g, '-')
        
        router.push({
          name: 'CoachDetail',
          params: {
            id: service.id,
            title: serviceTitle,
            coachId: coach.id,
            coachName: coachName
          }
        })
      }
    }
    
    const bookAppointment = (coach) => {
      // Find a service to use for booking
      const service = services.value.find(s => 
        coach.coachingInfo && coach.coachingInfo.includes(s.title)
      ) || services.value[0]
      
      if (service) {
        const serviceTitle = service.title.toLowerCase().replace(/\s+/g, '-')
        const coachName = coach.fullName.toLowerCase().replace(/\s+/g, '-')
        
        router.push({
          name: 'InterviewCalendar',
          params: {
            id: service.id,
            title: serviceTitle,
            coachId: coach.id,
            coachName: coachName
          }
        })
      }
    }
    
    onMounted(() => {
      loadCoaches()
      loadServices()
    })
    
    return {
      coaches,
      services,
      loading,
      selectedService,
      sortBy,
      currentPage,
      filteredCoaches,
      paginatedCoaches,
      totalPages,
      visiblePages,
      getCoachSpecialties,
      getCoachServices,
      truncateText,
      handleImageError,
      applyFilters,
      applySorting,
      changePage,
      viewCoachDetail,
      bookAppointment
    }
  }
}
</script>

<style scoped>
.coaches-page {
  min-height: 100vh;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.breadcrumb-item + .breadcrumb-item::before {
  color: rgba(255, 255, 255, 0.5);
}

.filters-section {
  border-bottom: 1px solid #dee2e6;
}

.form-select {
  border: 2px solid #e9ecef;
  border-radius: 8px;
  font-size: 0.9rem;
}

.form-select:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
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
  transform: translateY(-8px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.coach-image {
  position: relative;
  height: 250px;
  overflow: hidden;
}

.coach-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.coach-card:hover .coach-image img {
  transform: scale(1.05);
}

.coach-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.coach-card:hover .coach-overlay {
  opacity: 1;
}

.coach-actions {
  display: flex;
  gap: 1rem;
}

.coach-actions .btn {
  border-radius: 20px;
  padding: 8px 20px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.coach-info {
  padding: 1.5rem;
}

.coach-name {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
  color: #2c3e50;
}

.coach-title {
  color: #007bff;
  font-weight: 600;
  margin-bottom: 1rem;
  font-size: 0.95rem;
}

.coach-specialties {
  margin-bottom: 1rem;
}

.coach-specialties .badge {
  font-size: 0.75rem;
  padding: 0.4rem 0.8rem;
  border-radius: 12px;
  margin-bottom: 0.25rem;
}

.coach-description {
  color: #6c757d;
  font-size: 0.9rem;
  line-height: 1.5;
  margin-bottom: 1rem;
}

.coach-meta {
  margin-bottom: 1rem;
}

.rating {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
}

.stars {
  color: #ffc107;
  font-size: 0.9rem;
}

.rating-text {
  font-size: 0.85rem;
  color: #6c757d;
}

.experience {
  font-size: 0.85rem;
  color: #6c757d;
  display: flex;
  align-items: center;
}

.coach-services h6 {
  font-size: 0.9rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: #495057;
}

.services-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.services-list li {
  font-size: 0.85rem;
  color: #6c757d;
  padding: 0.2rem 0;
  position: relative;
  padding-left: 1rem;
}

.services-list li::before {
  content: '•';
  color: #007bff;
  font-weight: bold;
  position: absolute;
  left: 0;
}

.pagination-wrapper {
  margin-top: 3rem;
}

.pagination .page-link {
  border: none;
  color: #007bff;
  font-weight: 500;
  padding: 0.75rem 1rem;
  margin: 0 0.25rem;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.pagination .page-link:hover {
  background-color: #007bff;
  color: white;
  transform: translateY(-2px);
}

.pagination .page-item.active .page-link {
  background-color: #007bff;
  color: white;
  box-shadow: 0 4px 8px rgba(0, 123, 255, 0.3);
}

.pagination .page-item.disabled .page-link {
  color: #6c757d;
  cursor: not-allowed;
}

/* Loading State */
.spinner-border {
  width: 3rem;
  height: 3rem;
}

/* Responsive Design */
@media (max-width: 768px) {
  .filters-section .d-flex {
    flex-direction: column;
    gap: 1rem;
  }
  
  .coach-actions {
    flex-direction: column;
  }
  
  .coach-actions .btn {
    width: 100%;
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

.coach-card {
  animation: fadeInUp 0.6s ease forwards;
}

.coach-card:nth-child(1) { animation-delay: 0.1s; }
.coach-card:nth-child(2) { animation-delay: 0.2s; }
.coach-card:nth-child(3) { animation-delay: 0.3s; }
.coach-card:nth-child(4) { animation-delay: 0.4s; }
.coach-card:nth-child(5) { animation-delay: 0.5s; }
.coach-card:nth-child(6) { animation-delay: 0.6s; }
</style>