<template>
  <div class="home-page">
    <!-- Hero Section -->
    <section class="hero-section">
      <div class="hero-bg">
        <div class="container">
          <div class="row align-items-center min-vh-100">
            <div class="col-lg-6">
              <div class="hero-content">
                <h1 class="hero-title">
                  Hayallerinizi <span class="text-primary">Gerçeğe</span> Dönüştürün
                </h1>
                <p class="hero-subtitle">
                  Uzman koçlarımız eşliğinde kişisel gelişim yolculuğunuzda bir adım öne geçin. 
                  Potansiyelinizi keşfedin, hedeflerinize ulaşın.
                </p>
                <div class="hero-buttons">
                  <router-link to="/online-kocluk-ve-atolye" class="btn btn-primary btn-lg me-3">
                    Koçluk Hizmeti Al
                  </router-link>
                  <router-link to="/kocluk-ekibimiz" class="btn btn-outline-primary btn-lg">
                    Ekibimizi Tanıyın
                  </router-link>
                </div>
                <div class="hero-stats">
                  <div class="row">
                    <div class="col-4">
                      <div class="stat-item">
                        <h3>{{ stats.totalUsers }}+</h3>
                        <p>Mutlu Müşteri</p>
                      </div>
                    </div>
                    <div class="col-4">
                      <div class="stat-item">
                        <h3>{{ stats.totalCoaches }}+</h3>
                        <p>Uzman Koç</p>
                      </div>
                    </div>
                    <div class="col-4">
                      <div class="stat-item">
                        <h3>{{ stats.totalSessions }}+</h3>
                        <p>Başarılı Seans</p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-6">
              <div class="hero-image">
                <img src="/hero-image.jpg" alt="Kişisel Gelişim" class="img-fluid" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Services Section -->
    <section class="services-section py-5">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Hizmetlerimiz</h2>
          <p class="section-subtitle">
            Kişisel gelişiminiz için tasarlanmış özel hizmetlerimizi keşfedin
          </p>
        </div>
        <div class="row">
          <div class="col-lg-3 col-md-6 mb-4" v-for="service in services" :key="service.id">
            <div class="service-card">
              <div class="service-icon">
                <i :class="service.icon"></i>
              </div>
              <h4>{{ service.title }}</h4>
              <p>{{ service.description }}</p>
              <router-link :to="service.link" class="btn btn-outline-primary">
                Detaylar
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Coaches Section -->
    <section class="coaches-section bg-light py-5">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Uzman Koçlarımız</h2>
          <p class="section-subtitle">
            Alanında uzman, deneyimli koçlarımızla tanışın
          </p>
        </div>
        <div class="row">
          <div class="col-lg-4 col-md-6 mb-4" v-for="coach in featuredCoaches" :key="coach.id">
            <div class="coach-card">
              <div class="coach-image">
                <img :src="coach.image || '/default-coach.jpg'" :alt="coach.fullName" />
              </div>
              <div class="coach-info">
                <h5>{{ coach.fullName }}</h5>
                <p class="coach-title">{{ coach.title }}</p>
                <p class="coach-description">{{ coach.description }}</p>
                <div class="coach-rating">
                  <div class="stars">
                    <i v-for="star in 5" :key="star" 
                       :class="star <= coach.rating ? 'fas fa-star' : 'far fa-star'"></i>
                  </div>
                  <span>({{ coach.reviewCount }} değerlendirme)</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="text-center">
          <router-link to="/kocluk-ekibimiz" class="btn btn-primary">
            Tüm Koçları Görüntüle
          </router-link>
        </div>
      </div>
    </section>

    <!-- Testimonials Section -->
    <section class="testimonials-section py-5">
      <div class="container">
        <div class="text-center mb-5">
          <h2 class="section-title">Müşteri Yorumları</h2>
          <p class="section-subtitle">
            Bizimle çalışan müşterilerimizin deneyimlerini dinleyin
          </p>
        </div>
        <div class="row">
          <div class="col-lg-4 col-md-6 mb-4" v-for="testimonial in testimonials" :key="testimonial.id">
            <div class="testimonial-card">
              <div class="testimonial-content">
                <div class="quote-icon">
                  <i class="fas fa-quote-left"></i>
                </div>
                <p>"{{ testimonial.comment }}"</p>
                <div class="testimonial-rating">
                  <div class="stars">
                    <i v-for="star in testimonial.stars" :key="star" class="fas fa-star"></i>
                  </div>
                </div>
              </div>
              <div class="testimonial-author">
                <h6>{{ testimonial.fullName }}</h6>
                <small>{{ testimonial.title }}</small>
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
              Uzman koçlarımızla birebir görüşme yaparak hedeflerinize ulaşmanın yollarını keşfedin.
            </p>
          </div>
          <div class="col-lg-4 text-lg-end">
            <router-link to="/online-kocluk-ve-atolye" class="btn btn-light btn-lg">
              Hemen Başla
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Contact Form Section -->
    <section class="contact-section py-5">
      <div class="container">
        <div class="row">
          <div class="col-lg-6">
            <h3>Bizimle İletişime Geçin</h3>
            <p class="text-muted mb-4">
              Sorularınız için bize ulaşın. Size en kısa sürede dönüş yapacağız.
            </p>
            <form @submit.prevent="submitContactForm">
              <div class="row">
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <input
                      v-model="contactForm.fullName"
                      type="text"
                      class="form-control"
                      placeholder="Ad Soyad"
                      required
                    />
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="form-group mb-3">
                    <input
                      v-model="contactForm.email"
                      type="email"
                      class="form-control"
                      placeholder="E-posta"
                      required
                    />
                  </div>
                </div>
              </div>
              <div class="form-group mb-3">
                <input
                  v-model="contactForm.subject"
                  type="text"
                  class="form-control"
                  placeholder="Konu"
                  required
                />
              </div>
              <div class="form-group mb-3">
                <textarea
                  v-model="contactForm.message"
                  class="form-control"
                  rows="5"
                  placeholder="Mesajınız"
                  required
                ></textarea>
              </div>
              <button type="submit" class="btn btn-primary" :disabled="isSubmitting">
                <span v-if="isSubmitting" class="spinner-border spinner-border-sm me-2"></span>
                Gönder
              </button>
            </form>
          </div>
          <div class="col-lg-6">
            <div class="contact-info">
              <div class="contact-item">
                <i class="fas fa-map-marker-alt"></i>
                <div>
                  <h6>Adres</h6>
                  <p>İstanbul, Türkiye</p>
                </div>
              </div>
              <div class="contact-item">
                <i class="fas fa-phone"></i>
                <div>
                  <h6>Telefon</h6>
                  <p>+90 555 123 45 67</p>
                </div>
              </div>
              <div class="contact-item">
                <i class="fas fa-envelope"></i>
                <div>
                  <h6>E-posta</h6>
                  <p>info@pekiyaben.com</p>
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
import { ref, onMounted } from 'vue'
import { useToast } from 'vue-toastification'
import contentService from '@/services/content'
import coachesService from '@/services/coaches'

export default {
  name: 'HomePage',
  setup() {
    const toast = useToast()
    
    const stats = ref({
      totalUsers: 1500,
      totalCoaches: 25,
      totalSessions: 5000
    })
    
    const services = ref([
      {
        id: 1,
        title: 'Kişisel Durum Analizi',
        description: 'Mevcut durumunuzu analiz ederek gelişim alanlarınızı belirleyin.',
        icon: 'fas fa-chart-line',
        link: '/kisisel-durum-analizi'
      },
      {
        id: 2,
        title: 'Kişisel Strateji Belirleme',
        description: 'Hedeflerinize ulaşmak için kişiselleştirilmiş stratejiler geliştirin.',
        icon: 'fas fa-bullseye',
        link: '/kisisel-strateji-belirleme'
      },
      {
        id: 3,
        title: 'Bilinçaltı Uygulamaları',
        description: 'Bilinçaltınızın gücünü keşfederek potansiyelinizi ortaya çıkarın.',
        icon: 'fas fa-brain',
        link: '/bilincalti-uygulamalari'
      },
      {
        id: 4,
        title: 'Koçluk Görüşmeleri',
        description: 'Birebir koçluk seansları ile kişisel gelişiminizi hızlandırın.',
        icon: 'fas fa-comments',
        link: '/kocluk-gorusmeleri'
      }
    ])
    
    const featuredCoaches = ref([])
    const testimonials = ref([])
    
    const contactForm = ref({
      fullName: '',
      email: '',
      subject: '',
      message: ''
    })
    
    const isSubmitting = ref(false)
    
    const loadFeaturedCoaches = async () => {
      try {
        const response = await coachesService.getCoaches()
        featuredCoaches.value = response.slice(0, 3).map(coach => ({
          ...coach,
          rating: 4.5,
          reviewCount: Math.floor(Math.random() * 50) + 10
        }))
      } catch (error) {
        console.error('Error loading coaches:', error)
      }
    }
    
    const loadTestimonials = async () => {
      // Mock testimonials data since we don't have a specific endpoint
      testimonials.value = [
        {
          id: 1,
          fullName: 'Ayşe Demir',
          title: 'Pazarlama Uzmanı',
          comment: 'PekiYaBen sayesinde kariyerimde büyük ilerleme kaydettim. Koçum çok profesyonel ve yardımseverdi.',
          stars: 5
        },
        {
          id: 2,
          fullName: 'Mehmet Kaya',
          title: 'Girişimci',
          comment: 'Kişisel gelişim yolculuğumda bana rehberlik eden harika bir platform. Herkese tavsiye ederim.',
          stars: 5
        },
        {
          id: 3,
          fullName: 'Zehra Öztürk',
          title: 'İnsan Kaynakları Uzmanı',
          comment: 'Bilinçaltı uygulamaları derslerinden sonra kendime olan güvenim arttı. Çok memnunum.',
          stars: 4
        }
      ]
    }
    
    const submitContactForm = async () => {
      isSubmitting.value = true
      
      try {
        await contentService.submitContactForm(contactForm.value)
        toast.success('Mesajınız başarıyla gönderildi!')
        
        // Reset form
        contactForm.value = {
          fullName: '',
          email: '',
          subject: '',
          message: ''
        }
      } catch (error) {
        toast.error('Mesaj gönderilirken bir hata oluştu.')
      } finally {
        isSubmitting.value = false
      }
    }
    
    onMounted(() => {
      loadFeaturedCoaches()
      loadTestimonials()
    })
    
    return {
      stats,
      services,
      featuredCoaches,
      testimonials,
      contactForm,
      isSubmitting,
      submitContactForm
    }
  }
}
</script>

<style scoped>
.hero-section {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
  display: flex;
  align-items: center;
}

.hero-content {
  color: white;
  z-index: 2;
}

.hero-title {
  font-size: 3.5rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  line-height: 1.2;
}

.hero-subtitle {
  font-size: 1.25rem;
  margin-bottom: 2rem;
  opacity: 0.9;
}

.hero-buttons .btn {
  border-radius: 50px;
  padding: 12px 30px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.hero-buttons .btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
}

.hero-stats {
  margin-top: 3rem;
}

.stat-item {
  text-align: center;
}

.stat-item h3 {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 0.5rem;
}

.stat-item p {
  opacity: 0.8;
  margin: 0;
}

.hero-image img {
  border-radius: 20px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
}

.section-title {
  font-size: 2.5rem;
  font-weight: 700;
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
  padding: 2rem;
  border-radius: 15px;
  text-align: center;
  height: 100%;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.service-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.service-icon {
  font-size: 3rem;
  color: #007bff;
  margin-bottom: 1.5rem;
}

.coach-card {
  background: white;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
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

.coach-title {
  color: #007bff;
  font-weight: 600;
  margin-bottom: 0.5rem;
}

.coach-description {
  color: #6c757d;
  font-size: 0.9rem;
  margin-bottom: 1rem;
}

.coach-rating {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.stars {
  color: #ffc107;
}

.testimonial-card {
  background: white;
  padding: 2rem;
  border-radius: 15px;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  height: 100%;
}

.quote-icon {
  font-size: 2rem;
  color: #007bff;
  margin-bottom: 1rem;
}

.testimonial-rating {
  margin: 1rem 0;
}

.testimonial-rating .stars {
  color: #ffc107;
}

.testimonial-author h6 {
  margin-bottom: 0.25rem;
  font-weight: 600;
}

.contact-info {
  padding-left: 2rem;
}

.contact-item {
  display: flex;
  align-items: center;
  margin-bottom: 2rem;
}

.contact-item i {
  font-size: 1.5rem;
  color: #007bff;
  margin-right: 1rem;
  width: 30px;
}

.contact-item h6 {
  margin-bottom: 0.25rem;
  font-weight: 600;
}

.contact-item p {
  margin: 0;
  color: #6c757d;
}

.form-control {
  border: 2px solid #e9ecef;
  border-radius: 10px;
  padding: 12px 15px;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.cta-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

/* Responsive */
@media (max-width: 768px) {
  .hero-title {
    font-size: 2.5rem;
  }
  
  .hero-buttons .btn {
    display: block;
    width: 100%;
    margin-bottom: 1rem;
  }
  
  .contact-info {
    padding-left: 0;
    margin-top: 2rem;
  }
}

/* Animations */
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

.service-card, .coach-card, .testimonial-card {
  animation: fadeInUp 0.6s ease forwards;
}
</style>