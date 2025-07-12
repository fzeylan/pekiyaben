<template>
  <footer class="footer bg-dark text-light">
    <div class="container py-5">
      <div class="row">
        <!-- Company Info -->
        <div class="col-lg-4 col-md-6 mb-4">
          <h5 class="text-white mb-3">PekiYaBen</h5>
          <p class="text-muted mb-3">
            Kişisel gelişim ve profesyonel koçluk hizmetleri ile hayallerinizi gerçeğe dönüştürmenize yardımcı oluyoruz.
          </p>
          <div class="social-links">
            <a href="#" class="text-light me-3" title="Facebook">
              <i class="fab fa-facebook-f"></i>
            </a>
            <a href="#" class="text-light me-3" title="Instagram">
              <i class="fab fa-instagram"></i>
            </a>
            <a href="#" class="text-light me-3" title="Twitter">
              <i class="fab fa-twitter"></i>
            </a>
            <a href="#" class="text-light me-3" title="LinkedIn">
              <i class="fab fa-linkedin-in"></i>
            </a>
            <a href="#" class="text-light" title="YouTube">
              <i class="fab fa-youtube"></i>
            </a>
          </div>
        </div>

        <!-- Quick Links -->
        <div class="col-lg-2 col-md-6 mb-4">
          <h6 class="text-white mb-3">Hızlı Linkler</h6>
          <ul class="list-unstyled">
            <li class="mb-2">
              <router-link to="/" class="text-muted footer-link">Ana Sayfa</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/kocluk-ekibimiz" class="text-muted footer-link">Ekibimiz</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/online-kocluk-ve-atolye" class="text-muted footer-link">Online Koçluk</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/kisisel-gelisim/yazilar" class="text-muted footer-link">Blog</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/sikca-sorulanlar" class="text-muted footer-link">S.S.S</router-link>
            </li>
          </ul>
        </div>

        <!-- Services -->
        <div class="col-lg-3 col-md-6 mb-4">
          <h6 class="text-white mb-3">Hizmetlerimiz</h6>
          <ul class="list-unstyled">
            <li class="mb-2">
              <router-link to="/kisisel-durum-analizi" class="text-muted footer-link">Kişisel Durum Analizi</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/kisisel-strateji-belirleme" class="text-muted footer-link">Kişisel Strateji Belirleme</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/bilincalti-uygulamalari" class="text-muted footer-link">Bilinçaltı Uygulamaları</router-link>
            </li>
            <li class="mb-2">
              <router-link to="/kocluk-gorusmeleri" class="text-muted footer-link">Koçluk Görüşmeleri</router-link>
            </li>
          </ul>
        </div>

        <!-- Contact Info -->
        <div class="col-lg-3 col-md-6 mb-4">
          <h6 class="text-white mb-3">İletişim</h6>
          <ul class="list-unstyled">
            <li class="mb-2 d-flex align-items-center">
              <i class="fas fa-envelope me-2 text-primary"></i>
              <a href="mailto:info@pekiyaben.com" class="text-muted footer-link">info@pekiyaben.com</a>
            </li>
            <li class="mb-2 d-flex align-items-center">
              <i class="fas fa-phone me-2 text-primary"></i>
              <a href="tel:+905551234567" class="text-muted footer-link">+90 555 123 45 67</a>
            </li>
            <li class="mb-2 d-flex align-items-start">
              <i class="fas fa-map-marker-alt me-2 text-primary mt-1"></i>
              <span class="text-muted">İstanbul, Türkiye</span>
            </li>
          </ul>
        </div>
      </div>

      <!-- Newsletter -->
      <div class="row border-top border-secondary pt-4 mt-4">
        <div class="col-lg-8">
          <h6 class="text-white mb-3">Bülten Aboneliği</h6>
          <p class="text-muted mb-3">
            Kişisel gelişim ipuçları ve yeniliklerden haberdar olmak için bültenimize abone olun.
          </p>
          <form @submit.prevent="subscribeNewsletter" class="d-flex">
            <input
              v-model="email"
              type="email"
              class="form-control me-2"
              placeholder="E-posta adresiniz"
              required
            />
            <button type="submit" class="btn btn-primary" :disabled="isSubscribing">
              <span v-if="isSubscribing" class="spinner-border spinner-border-sm me-1"></span>
              Abone Ol
            </button>
          </form>
        </div>
        <div class="col-lg-4 text-lg-end mt-3 mt-lg-0">
          <div class="footer-badges">
            <img src="/ssl-badge.png" alt="SSL Sertifikalı" class="badge-img me-2" />
            <img src="/secure-badge.png" alt="Güvenli Ödeme" class="badge-img" />
          </div>
        </div>
      </div>
    </div>

    <!-- Copyright -->
    <div class="bg-secondary">
      <div class="container py-3">
        <div class="row align-items-center">
          <div class="col-md-6">
            <p class="mb-0 text-muted small">
              © {{ currentYear }} PekiYaBen. Tüm hakları saklıdır.
            </p>
          </div>
          <div class="col-md-6 text-md-end">
            <div class="footer-legal-links">
              <a href="#" class="text-muted small me-3 footer-link">Gizlilik Politikası</a>
              <a href="#" class="text-muted small me-3 footer-link">Kullanım Şartları</a>
              <a href="#" class="text-muted small footer-link">KVKK</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </footer>
</template>

<script>
import { ref, computed } from 'vue'
import { useToast } from 'vue-toastification'

export default {
  name: 'TheFooter',
  setup() {
    const toast = useToast()
    
    const email = ref('')
    const isSubscribing = ref(false)
    
    const currentYear = computed(() => new Date().getFullYear())
    
    const subscribeNewsletter = async () => {
      if (!email.value) return
      
      isSubscribing.value = true
      
      try {
        // Simulate API call for newsletter subscription
        await new Promise(resolve => setTimeout(resolve, 1000))
        
        toast.success('Bülten aboneliği başarılı!')
        email.value = ''
      } catch (error) {
        toast.error('Abonelik sırasında bir hata oluştu.')
      } finally {
        isSubscribing.value = false
      }
    }
    
    return {
      email,
      isSubscribing,
      currentYear,
      subscribeNewsletter
    }
  }
}
</script>

<style scoped>
.footer {
  margin-top: auto;
}

.social-links a {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: rgba(255, 255, 255, 0.1);
  transition: all 0.3s ease;
}

.social-links a:hover {
  background-color: #007bff;
  color: white !important;
  transform: translateY(-2px);
}

.footer-link {
  text-decoration: none;
  transition: color 0.3s ease;
}

.footer-link:hover {
  color: #007bff !important;
}

.badge-img {
  height: 40px;
  width: auto;
  opacity: 0.8;
  transition: opacity 0.3s ease;
}

.badge-img:hover {
  opacity: 1;
}

.form-control {
  border: none;
  border-radius: 25px;
  padding: 10px 15px;
}

.form-control:focus {
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
  border-color: #007bff;
}

.btn-primary {
  border-radius: 25px;
  padding: 10px 20px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  transform: translateY(-1px);
  box-shadow: 0 4px 8px rgba(0, 123, 255, 0.3);
}

/* Responsive */
@media (max-width: 768px) {
  .social-links {
    text-align: center;
    margin-top: 1rem;
  }
  
  .footer-legal-links {
    text-align: center;
    margin-top: 1rem;
  }
  
  .footer-badges {
    text-align: center;
  }
}

/* Dark theme support */
.bg-dark {
  background-color: #1a1a1a !important;
}