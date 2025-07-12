<template>
  <div class="not-found-page">
    <div class="container">
      <div class="row justify-content-center align-items-center min-vh-100">
        <div class="col-lg-6 col-md-8 text-center">
          <div class="error-content">
            <!-- 404 Animation -->
            <div class="error-animation">
              <div class="error-number">
                <span class="four">4</span>
                <span class="zero">0</span>
                <span class="four">4</span>
              </div>
              <div class="error-icon">
                <i class="fas fa-search"></i>
              </div>
            </div>
            
            <!-- Error Message -->
            <h1 class="error-title">Sayfa Bulunamadı</h1>
            <p class="error-description">
              Aradığınız sayfa mevcut değil veya taşınmış olabilir. 
              Ana sayfaya dönebilir veya aşağıdaki linklerden istediğiniz sayfaya gidebilirsiniz.
            </p>
            
            <!-- Action Buttons -->
            <div class="error-actions">
              <router-link to="/" class="btn btn-primary btn-lg">
                <i class="fas fa-home me-2"></i>
                Ana Sayfaya Dön
              </router-link>
              <button @click="goBack" class="btn btn-outline-secondary btn-lg">
                <i class="fas fa-arrow-left me-2"></i>
                Geri Git
              </button>
            </div>
            
            <!-- Quick Links -->
            <div class="quick-links">
              <h5>Popüler Sayfalar</h5>
              <div class="links-grid">
                <router-link to="/online-kocluk-ve-atolye" class="quick-link">
                  <i class="fas fa-user-tie"></i>
                  <span>Online Koçluk</span>
                </router-link>
                <router-link to="/kocluk-ekibimiz" class="quick-link">
                  <i class="fas fa-users"></i>
                  <span>Koçluk Ekibi</span>
                </router-link>
                <router-link to="/kisisel-gelisim/yazilar" class="quick-link">
                  <i class="fas fa-blog"></i>
                  <span>Blog</span>
                </router-link>
                <router-link to="/sikca-sorulanlar" class="quick-link">
                  <i class="fas fa-question-circle"></i>
                  <span>S.S.S</span>
                </router-link>
              </div>
            </div>
            
            <!-- Search -->
            <div class="error-search">
              <h6>Ne arıyordunuz?</h6>
              <form @submit.prevent="handleSearch" class="search-form">
                <div class="input-group">
                  <input
                    v-model="searchQuery"
                    type="text"
                    class="form-control"
                    placeholder="Arama yapın..."
                  />
                  <button type="submit" class="btn btn-outline-primary">
                    <i class="fas fa-search"></i>
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Background Animation -->
    <div class="bg-animation">
      <div class="floating-shape shape-1"></div>
      <div class="floating-shape shape-2"></div>
      <div class="floating-shape shape-3"></div>
      <div class="floating-shape shape-4"></div>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'NotFoundPage',
  setup() {
    const router = useRouter()
    const searchQuery = ref('')
    
    const goBack = () => {
      if (window.history.length > 1) {
        router.go(-1)
      } else {
        router.push('/')
      }
    }
    
    const handleSearch = () => {
      if (searchQuery.value.trim()) {
        // Redirect to blog with search query
        router.push({
          path: '/kisisel-gelisim/yazilar',
          query: { search: searchQuery.value.trim() }
        })
      }
    }
    
    return {
      searchQuery,
      goBack,
      handleSearch
    }
  }
}
</script>

<style scoped>
.not-found-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  position: relative;
  overflow: hidden;
}

.error-content {
  position: relative;
  z-index: 2;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  padding: 3rem 2rem;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
}

.error-animation {
  margin-bottom: 2rem;
  position: relative;
}

.error-number {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1rem;
}

.error-number span {
  font-size: 6rem;
  font-weight: 900;
  background: linear-gradient(45deg, #007bff, #0056b3);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  animation: bounce 2s infinite;
}

.error-number .zero {
  animation-delay: 0.2s;
}

.error-number .four:last-child {
  animation-delay: 0.4s;
}

.error-icon {
  font-size: 3rem;
  color: #6c757d;
  opacity: 0.7;
  animation: pulse 2s infinite;
}

.error-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.error-description {
  font-size: 1.1rem;
  color: #6c757d;
  margin-bottom: 2rem;
  line-height: 1.6;
}

.error-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
  margin-bottom: 3rem;
  flex-wrap: wrap;
}

.btn {
  border-radius: 25px;
  padding: 0.75rem 2rem;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
}

.quick-links {
  margin-bottom: 2rem;
}

.quick-links h5 {
  color: #2c3e50;
  font-weight: 600;
  margin-bottom: 1.5rem;
}

.links-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.quick-link {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 15px;
  text-decoration: none;
  color: #6c757d;
  transition: all 0.3s ease;
}

.quick-link:hover {
  background: #007bff;
  color: white;
  transform: translateY(-5px);
  box-shadow: 0 10px 25px rgba(0, 123, 255, 0.3);
}

.quick-link i {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
}

.quick-link span {
  font-size: 0.9rem;
  font-weight: 500;
}

.error-search h6 {
  color: #2c3e50;
  font-weight: 600;
  margin-bottom: 1rem;
}

.search-form .form-control {
  border: 2px solid #e9ecef;
  border-radius: 25px 0 0 25px;
  padding: 0.75rem 1rem;
  border-right: none;
}

.search-form .form-control:focus {
  border-color: #007bff;
  box-shadow: none;
}

.search-form .btn {
  border-radius: 0 25px 25px 0;
  border: 2px solid #e9ecef;
  border-left: none;
}

.search-form .btn:hover {
  border-color: #007bff;
  transform: none;
  box-shadow: none;
}

.bg-animation {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 1;
}

.floating-shape {
  position: absolute;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 50%;
  animation: float 6s ease-in-out infinite;
}

.shape-1 {
  width: 100px;
  height: 100px;
  top: 10%;
  left: 10%;
  animation-delay: 0s;
}

.shape-2 {
  width: 150px;
  height: 150px;
  top: 60%;
  right: 10%;
  animation-delay: 2s;
}

.shape-3 {
  width: 80px;
  height: 80px;
  bottom: 20%;
  left: 20%;
  animation-delay: 4s;
}

.shape-4 {
  width: 120px;
  height: 120px;
  top: 30%;
  right: 30%;
  animation-delay: 1s;
}

/* Animations */
@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-20px);
  }
  60% {
    transform: translateY(-10px);
  }
}

@keyframes pulse {
  0% {
    opacity: 0.7;
    transform: scale(1);
  }
  50% {
    opacity: 1;
    transform: scale(1.05);
  }
  100% {
    opacity: 0.7;
    transform: scale(1);
  }
}

@keyframes float {
  0%, 100% {
    transform: translateY(0px) rotate(0deg);
  }
  25% {
    transform: translateY(-20px) rotate(90deg);
  }
  50% {
    transform: translateY(-10px) rotate(180deg);
  }
  75% {
    transform: translateY(-15px) rotate(270deg);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .error-content {
    padding: 2rem 1.5rem;
  }
  
  .error-number span {
    font-size: 4rem;
  }
  
  .error-title {
    font-size: 2rem;
  }
  
  .error-description {
    font-size: 1rem;
  }
  
  .error-actions {
    flex-direction: column;
    align-items: center;
  }
  
  .error-actions .btn {
    width: 100%;
    max-width: 250px;
  }
  
  .links-grid {
    grid-template-columns: repeat(2, 1fr);
  }
  
  .floating-shape {
    display: none;
  }
}

@media (max-width: 576px) {
  .error-number {
    gap: 0.5rem;
  }
  
  .error-number span {
    font-size: 3rem;
  }
  
  .error-title {
    font-size: 1.5rem;
  }
  
  .quick-link {
    padding: 0.75rem;
  }
  
  .quick-link i {
    font-size: 1.25rem;
  }
  
  .quick-link span {
    font-size: 0.8rem;
  }
}

/* Dark mode support */
@media (prefers-color-scheme: dark) {
  .error-content {
    background: rgba(33, 37, 41, 0.95);
    color: #f8f9fa;
  }
  
  .error-title {
    color: #f8f9fa;
  }
  
  .quick-links h5,
  .error-search h6 {
    color: #f8f9fa;
  }
  
  .quick-link {
    background: rgba(108, 117, 125, 0.2);
    color: #adb5bd;
  }
  
  .search-form .form-control {
    background: rgba(255, 255, 255, 0.1);
    border-color: rgba(255, 255, 255, 0.2);
    color: #f8f9fa;
  }
  
  .search-form .form-control::placeholder {
    color: #adb5bd;
  }
}
</style>