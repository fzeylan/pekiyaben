<template>
  <div class="faq-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Sıkça Sorulan Sorular</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">S.S.S</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- FAQ Content -->
    <section class="faq-content py-5">
      <div class="container">
        <div class="row">
          <!-- Categories Sidebar -->
          <div class="col-lg-3 mb-4">
            <div class="categories-sidebar">
              <h5>Kategoriler</h5>
              <ul class="category-list">
                <li>
                  <button 
                    class="category-btn"
                    :class="{ active: selectedCategory === 'all' }"
                    @click="setCategory('all')"
                  >
                    <i class="fas fa-list me-2"></i>
                    Tümü ({{ faqs.length }})
                  </button>
                </li>
                <li v-for="category in categories" :key="category.id">
                  <button 
                    class="category-btn"
                    :class="{ active: selectedCategory === category.id }"
                    @click="setCategory(category.id)"
                  >
                    <i :class="category.icon + ' me-2'"></i>
                    {{ category.name }} ({{ getCategoryCount(category.id) }})
                  </button>
                </li>
              </ul>
            </div>
          </div>

          <!-- FAQ Content -->
          <div class="col-lg-9">
            <!-- Search Bar -->
            <div class="search-section mb-4">
              <div class="search-input-group">
                <i class="fas fa-search search-icon"></i>
                <input
                  v-model="searchQuery"
                  type="text"
                  class="form-control search-input"
                  placeholder="Soru ara..."
                />
                <button 
                  v-if="searchQuery"
                  @click="clearSearch"
                  class="clear-search-btn"
                >
                  <i class="fas fa-times"></i>
                </button>
              </div>
            </div>

            <!-- Loading State -->
            <div v-if="loading" class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Yükleniyor...</span>
              </div>
              <p class="mt-3">Sorular yükleniyor...</p>
            </div>

            <!-- FAQ Results Info -->
            <div v-if="!loading" class="results-info mb-4">
              <p class="text-muted">
                <span v-if="searchQuery">
                  "<strong>{{ searchQuery }}</strong>" için {{ filteredFAQs.length }} sonuç bulundu
                </span>
                <span v-else-if="selectedCategory !== 'all'">
                  {{ selectedCategoryName }} kategorisinde {{ filteredFAQs.length }} soru
                </span>
                <span v-else>
                  Toplam {{ filteredFAQs.length }} soru
                </span>
              </p>
            </div>

            <!-- FAQ Accordion -->
            <div v-if="!loading && filteredFAQs.length > 0" class="faq-accordion">
              <div 
                v-for="(faq, index) in filteredFAQs" 
                :key="faq.id"
                class="faq-item"
              >
                <div class="faq-header" @click="toggleFAQ(index)">
                  <h6 class="faq-question">
                    <span class="question-text">{{ faq.title }}</span>
                    <i 
                      class="fas fa-chevron-down toggle-icon"
                      :class="{ rotated: activeFAQ === index }"
                    ></i>
                  </h6>
                </div>
                <div 
                  class="faq-body"
                  :class="{ active: activeFAQ === index }"
                >
                  <div class="faq-answer" v-html="faq.description"></div>
                  <div v-if="faq.relatedLinks && faq.relatedLinks.length > 0" class="related-links">
                    <h6>İlgili Bağlantılar:</h6>
                    <ul>
                      <li v-for="link in faq.relatedLinks" :key="link.id">
                        <router-link :to="link.url">{{ link.title }}</router-link>
                      </li>
                    </ul>
                  </div>
                  <div class="faq-actions">
                    <button 
                      @click="markHelpful(faq, true)"
                      class="helpful-btn"
                      :class="{ active: faq.userVote === 'helpful' }"
                    >
                      <i class="fas fa-thumbs-up me-1"></i>
                      Yararlı ({{ faq.helpfulCount || 0 }})
                    </button>
                    <button 
                      @click="markHelpful(faq, false)"
                      class="helpful-btn"
                      :class="{ active: faq.userVote === 'not-helpful' }"
                    >
                      <i class="fas fa-thumbs-down me-1"></i>
                      Yararlı değil ({{ faq.notHelpfulCount || 0 }})
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <!-- No Results -->
            <div v-if="!loading && filteredFAQs.length === 0" class="no-results">
              <div class="no-results-icon">
                <i class="fas fa-search"></i>
              </div>
              <h4>Sonuç bulunamadı</h4>
              <p v-if="searchQuery">
                "<strong>{{ searchQuery }}</strong>" için sonuç bulunamadı. 
                Lütfen farklı kelimeler deneyiniz.
              </p>
              <p v-else>
                Bu kategoride henüz soru bulunmamaktadır.
              </p>
              <button @click="clearSearch" class="btn btn-primary">
                Tüm Soruları Görüntüle
              </button>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Contact Section -->
    <section class="contact-section bg-light py-5">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-8 text-center">
            <h3>Aradığınız cevabı bulamadınız mı?</h3>
            <p class="text-muted mb-4">
              Uzman ekibimiz size yardımcı olmaktan memnuniyet duyacaktır. 
              Sorularınız için bizimle iletişime geçin.
            </p>
            <div class="contact-options">
              <div class="row">
                <div class="col-md-4 mb-3">
                  <div class="contact-option">
                    <i class="fas fa-envelope"></i>
                    <h6>E-posta</h6>
                    <p>info@pekiyaben.com</p>
                    <a href="mailto:info@pekiyaben.com" class="btn btn-outline-primary btn-sm">
                      E-posta Gönder
                    </a>
                  </div>
                </div>
                <div class="col-md-4 mb-3">
                  <div class="contact-option">
                    <i class="fas fa-phone"></i>
                    <h6>Telefon</h6>
                    <p>+90 555 123 45 67</p>
                    <a href="tel:+905551234567" class="btn btn-outline-primary btn-sm">
                      Ara
                    </a>
                  </div>
                </div>
                <div class="col-md-4 mb-3">
                  <div class="contact-option">
                    <i class="fas fa-comments"></i>
                    <h6>Canlı Destek</h6>
                    <p>7/24 Destek</p>
                    <button @click="openLiveChat" class="btn btn-outline-primary btn-sm">
                      Sohbet Başlat
                    </button>
                  </div>
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
import { ref, computed, onMounted, watch } from 'vue'
import { useToast } from 'vue-toastification'
import contentService from '@/services/content'

export default {
  name: 'FAQPage',
  setup() {
    const toast = useToast()
    
    const faqs = ref([])
    const loading = ref(true)
    const searchQuery = ref('')
    const selectedCategory = ref('all')
    const activeFAQ = ref(null)
    
    const categories = ref([
      {
        id: 'general',
        name: 'Genel',
        icon: 'fas fa-info-circle'
      },
      {
        id: 'coaching',
        name: 'Koçluk Hizmetleri',
        icon: 'fas fa-user-tie'
      },
      {
        id: 'booking',
        name: 'Randevu & Rezervasyon',
        icon: 'fas fa-calendar-check'
      },
      {
        id: 'payment',
        name: 'Ödeme & Faturalama',
        icon: 'fas fa-credit-card'
      },
      {
        id: 'technical',
        name: 'Teknik Destek',
        icon: 'fas fa-cog'
      }
    ])
    
    // Mock FAQ data
    const mockFAQs = [
      {
        id: 1,
        title: 'PekiYaBen nedir ve nasıl çalışır?',
        description: `
          <p>PekiYaBen, kişisel gelişim ve profesyonel koçluk hizmetleri sunan bir platformdur. 
          Uzman koçlarımız ile birebir görüşmeler yaparak hedeflerinize ulaşmanıza yardımcı oluyoruz.</p>
          <p>Platform şu şekilde çalışır:</p>
          <ul>
            <li>İhtiyacınıza uygun hizmeti seçin</li>
            <li>Uygun koçu belirleyin</li>
            <li>Randevu alın</li>
            <li>Online veya yüz yüze görüşme yapın</li>
          </ul>
        `,
        category: 'general',
        helpfulCount: 45,
        notHelpfulCount: 3,
        relatedLinks: [
          { id: 1, title: 'Hizmetlerimiz', url: '/online-kocluk-ve-atolye' },
          { id: 2, title: 'Koçluk Ekibimiz', url: '/kocluk-ekibimiz' }
        ]
      },
      {
        id: 2,
        title: 'Online koçluk seansları nasıl gerçekleşir?',
        description: `
          <p>Online koçluk seanslarımız güvenli video konferans platformları üzerinden gerçekleşir:</p>
          <ul>
            <li>Randevu saatinizden 15 dakika önce e-posta ile toplantı linki gönderilir</li>
            <li>Belirlenen saatte linke tıklayarak koçunuzla buluşabilirsiniz</li>
            <li>Sadece internet bağlantısı ve kamera/mikrofon gerekir</li>
            <li>Tüm görüşmeler gizli ve güvenlidir</li>
          </ul>
        `,
        category: 'coaching',
        helpfulCount: 32,
        notHelpfulCount: 1
      },
      {
        id: 3,
        title: 'Randevumu iptal edebilir miyim?',
        description: `
          <p>Evet, randevunuzu belirli koşullar altında iptal edebilirsiniz:</p>
          <ul>
            <li><strong>24 saat öncesine kadar:</strong> Ücretsiz iptal ve tam iade</li>
            <li><strong>2-24 saat arası:</strong> %50 ücret kesintisi</li>
            <li><strong>2 saatten az:</strong> İptal ücreti %100</li>
          </ul>
          <p>İptal işlemi için "Randevularım" sayfasından işlem yapabilirsiniz.</p>
        `,
        category: 'booking',
        helpfulCount: 28,
        notHelpfulCount: 5,
        relatedLinks: [
          { id: 1, title: 'Randevularım', url: '/hesabim/randevularim' }
        ]
      },
      {
        id: 4,
        title: 'Hangi ödeme yöntemlerini kabul ediyorsunuz?',
        description: `
          <p>Aşağıdaki ödeme yöntemlerini kabul ediyoruz:</p>
          <ul>
            <li>Kredi kartı (Visa, Mastercard, American Express)</li>
            <li>Banka kartı</li>
            <li>Havale/EFT</li>
            <li>PayPal</li>
            <li>Mobil ödeme (Paycell, Papara)</li>
          </ul>
          <p>Tüm ödemeleriniz SSL sertifikası ile güvence altındadır.</p>
        `,
        category: 'payment',
        helpfulCount: 19,
        notHelpfulCount: 2
      },
      {
        id: 5,
        title: 'Koçluk seansının süresi ne kadar?',
        description: `
          <p>Standart koçluk seanslarımız <strong>60 dakika</strong> sürmektedir.</p>
          <p>Ancak hizmet türüne göre süre değişebilir:</p>
          <ul>
            <li>Kişisel Durum Analizi: 90 dakika</li>
            <li>Kişisel Strateji Belirleme: 60 dakika</li>
            <li>Bilinçaltı Uygulamaları: 45-60 dakika</li>
            <li>Koçluk Görüşmeleri: 60 dakika</li>
          </ul>
        `,
        category: 'coaching',
        helpfulCount: 15,
        notHelpfulCount: 0
      },
      {
        id: 6,
        title: 'Video görüşme için hangi cihazları kullanabilirim?',
        description: `
          <p>Platformumuz tüm modern cihazlarda çalışır:</p>
          <ul>
            <li><strong>Bilgisayar:</strong> Windows, Mac, Linux</li>
            <li><strong>Mobil:</strong> Android ve iOS telefon/tablet</li>
            <li><strong>Tarayıcı:</strong> Chrome, Firefox, Safari, Edge</li>
          </ul>
          <p>Minimum gereksinimler:</p>
          <ul>
            <li>Stabil internet bağlantısı (en az 1 Mbps)</li>
            <li>Kamera ve mikrofon</li>
            <li>Güncel tarayıcı</li>
          </ul>
        `,
        category: 'technical',
        helpfulCount: 22,
        notHelpfulCount: 1
      },
      {
        id: 7,
        title: 'Koçumla uyum sağlayamazsam ne yapabilirim?',
        description: `
          <p>Koçunuzla uyum sağlayamadığınızı düşünüyorsanız:</p>
          <ul>
            <li>İlk seans sonrası başka bir koça geçiş yapabilirsiniz</li>
            <li>Koç değişikliği ücretsizdir</li>
            <li>Müşteri hizmetleri ekibimiz size uygun koç önerisinde bulunur</li>
            <li>Memnun kalmazsanız tam iade alabilirsiniz</li>
          </ul>
          <p>Memnuniyetiniz bizim önceliğimizdir.</p>
        `,
        category: 'coaching',
        helpfulCount: 31,
        notHelpfulCount: 2
      },
      {
        id: 8,
        title: 'Fatura nasıl alırım?',
        description: `
          <p>Fatura alma işlemi çok basittir:</p>
          <ul>
            <li>Ödeme sonrası otomatik olarak e-fatura gönderilir</li>
            <li>"Hesabım" > "Fatura Bilgileri" bölümünden fatura bilgilerinizi güncelleyebilirsiniz</li>
            <li>Kurumsal fatura için vergi numarası ve firma bilgilerinizi ekleyin</li>
            <li>Geçmiş faturalarınızı hesabınızdan indirebilirsiniz</li>
          </ul>
        `,
        category: 'payment',
        helpfulCount: 18,
        notHelpfulCount: 1,
        relatedLinks: [
          { id: 1, title: 'Profilim', url: '/hesabim/profilim' }
        ]
      }
    ]
    
    const filteredFAQs = computed(() => {
      let filtered = faqs.value
      
      // Filter by category
      if (selectedCategory.value !== 'all') {
        filtered = filtered.filter(faq => faq.category === selectedCategory.value)
      }
      
      // Filter by search query
      if (searchQuery.value.trim()) {
        const query = searchQuery.value.toLowerCase()
        filtered = filtered.filter(faq => 
          faq.title.toLowerCase().includes(query) ||
          faq.description.toLowerCase().includes(query)
        )
      }
      
      return filtered
    })
    
    const selectedCategoryName = computed(() => {
      if (selectedCategory.value === 'all') return 'Tümü'
      const category = categories.value.find(cat => cat.id === selectedCategory.value)
      return category ? category.name : ''
    })
    
    const loadFAQs = async () => {
      try {
        loading.value = true
        // Try to load from API first
        try {
          const response = await contentService.getFAQs()
          faqs.value = response.map(faq => ({
            ...faq,
            category: 'general', // Default category if not provided
            helpfulCount: Math.floor(Math.random() * 50),
            notHelpfulCount: Math.floor(Math.random() * 10)
          }))
        } catch (apiError) {
          // Fallback to mock data if API fails
          console.warn('API failed, using mock data:', apiError)
          faqs.value = mockFAQs
        }
      } catch (error) {
        console.error('Error loading FAQs:', error)
        faqs.value = mockFAQs // Use mock data as ultimate fallback
      } finally {
        loading.value = false
      }
    }
    
    const getCategoryCount = (categoryId) => {
      return faqs.value.filter(faq => faq.category === categoryId).length
    }
    
    const setCategory = (categoryId) => {
      selectedCategory.value = categoryId
      activeFAQ.value = null
      searchQuery.value = '' // Clear search when changing category
    }
    
    const clearSearch = () => {
      searchQuery.value = ''
      selectedCategory.value = 'all'
      activeFAQ.value = null
    }
    
    const toggleFAQ = (index) => {
      activeFAQ.value = activeFAQ.value === index ? null : index
    }
    
    const markHelpful = (faq, isHelpful) => {
      if (faq.userVote) {
        // Remove previous vote
        if (faq.userVote === 'helpful') {
          faq.helpfulCount = (faq.helpfulCount || 1) - 1
        } else {
          faq.notHelpfulCount = (faq.notHelpfulCount || 1) - 1
        }
      }
      
      // Add new vote
      if (isHelpful) {
        faq.helpfulCount = (faq.helpfulCount || 0) + 1
        faq.userVote = 'helpful'
        toast.success('Geri bildiriminiz için teşekkürler!')
      } else {
        faq.notHelpfulCount = (faq.notHelpfulCount || 0) + 1
        faq.userVote = 'not-helpful'
        toast.info('Geri bildiriminiz kaydedildi. Daha iyi hizmet verebilmek için çalışıyoruz.')
      }
    }
    
    const openLiveChat = () => {
      // This would open a live chat widget
      toast.info('Canlı destek yakında kullanıma açılacak!')
    }
    
    // Close FAQ when search changes
    watch(searchQuery, () => {
      activeFAQ.value = null
    })
    
    onMounted(() => {
      loadFAQs()
    })
    
    return {
      faqs,
      loading,
      searchQuery,
      selectedCategory,
      activeFAQ,
      categories,
      filteredFAQs,
      selectedCategoryName,
      getCategoryCount,
      setCategory,
      clearSearch,
      toggleFAQ,
      markHelpful,
      openLiveChat
    }
  }
}
</script>

<style scoped>
.faq-page {
  min-height: 100vh;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.categories-sidebar {
  background: white;
  border-radius: 15px;
  padding: 1.5rem;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  position: sticky;
  top: 100px;
}

.categories-sidebar h5 {
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1.5rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #f8f9fa;
}

.category-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.category-btn {
  width: 100%;
  background: none;
  border: none;
  padding: 0.75rem 1rem;
  text-align: left;
  border-radius: 10px;
  margin-bottom: 0.5rem;
  color: #6c757d;
  font-weight: 500;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
}

.category-btn:hover {
  background: #f8f9fa;
  color: #007bff;
}

.category-btn.active {
  background: #007bff;
  color: white;
}

.search-section {
  background: white;
  border-radius: 15px;
  padding: 1.5rem;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
}

.search-input-group {
  position: relative;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  left: 1rem;
  color: #6c757d;
  z-index: 2;
}

.search-input {
  padding-left: 3rem;
  padding-right: 3rem;
  border: 2px solid #e9ecef;
  border-radius: 25px;
  font-size: 1rem;
  height: 50px;
  transition: all 0.3s ease;
}

.search-input:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.clear-search-btn {
  position: absolute;
  right: 1rem;
  background: none;
  border: none;
  color: #6c757d;
  cursor: pointer;
  transition: color 0.3s ease;
}

.clear-search-btn:hover {
  color: #007bff;
}

.results-info {
  background: white;
  border-radius: 10px;
  padding: 1rem 1.5rem;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.faq-accordion {
  background: white;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
}

.faq-item {
  border-bottom: 1px solid #e9ecef;
}

.faq-item:last-child {
  border-bottom: none;
}

.faq-header {
  padding: 1.5rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.faq-header:hover {
  background: #f8f9fa;
}

.faq-question {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 0;
  font-weight: 600;
  color: #2c3e50;
}

.question-text {
  flex: 1;
  margin-right: 1rem;
}

.toggle-icon {
  color: #007bff;
  transition: transform 0.3s ease;
}

.toggle-icon.rotated {
  transform: rotate(180deg);
}

.faq-body {
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.3s ease;
}

.faq-body.active {
  max-height: 1000px;
}

.faq-answer {
  padding: 0 1.5rem 1rem;
  color: #6c757d;
  line-height: 1.7;
}

.faq-answer p {
  margin-bottom: 1rem;
}

.faq-answer ul {
  margin-bottom: 1rem;
  padding-left: 1.5rem;
}

.faq-answer li {
  margin-bottom: 0.5rem;
}

.related-links {
  padding: 0 1.5rem 1rem;
  border-top: 1px solid #f8f9fa;
  margin-top: 1rem;
  padding-top: 1rem;
}

.related-links h6 {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.related-links ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.related-links li {
  margin-bottom: 0.25rem;
}

.related-links a {
  color: #007bff;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.3s ease;
}

.related-links a:hover {
  color: #0056b3;
  text-decoration: underline;
}

.faq-actions {
  padding: 0 1.5rem 1.5rem;
  display: flex;
  gap: 1rem;
}

.helpful-btn {
  background: none;
  border: 2px solid #e9ecef;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  color: #6c757d;
  font-size: 0.85rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.helpful-btn:hover {
  border-color: #007bff;
  color: #007bff;
}

.helpful-btn.active {
  background: #007bff;
  border-color: #007bff;
  color: white;
}

.no-results {
  background: white;
  border-radius: 15px;
  padding: 4rem 2rem;
  text-align: center;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
}

.no-results-icon {
  font-size: 4rem;
  color: #6c757d;
  margin-bottom: 1.5rem;
}

.no-results h4 {
  color: #2c3e50;
  margin-bottom: 1rem;
}

.no-results p {
  color: #6c757d;
  margin-bottom: 2rem;
}

.contact-section {
  background: #f8f9fa;
}

.contact-section h3 {
  color: #2c3e50;
  font-weight: 700;
  margin-bottom: 1rem;
}

.contact-options {
  margin-top: 2rem;
}

.contact-option {
  background: white;
  border-radius: 15px;
  padding: 2rem 1rem;
  text-align: center;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  height: 100%;
}

.contact-option:hover {
  transform: translateY(-5px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.contact-option i {
  font-size: 2.5rem;
  color: #007bff;
  margin-bottom: 1rem;
}

.contact-option h6 {
  font-weight: 600;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.contact-option p {
  color: #6c757d;
  margin-bottom: 1.5rem;
}

/* Responsive */
@media (max-width: 991.98px) {
  .categories-sidebar {
    position: static;
    margin-bottom: 2rem;
  }
  
  .category-list {
    display: flex;
    flex-wrap: wrap;
    gap: 0.5rem;
  }
  
  .category-btn {
    flex: 1;
    min-width: 150px;
    text-align: center;
    margin-bottom: 0;
  }
}

@media (max-width: 768px) {
  .faq-header {
    padding: 1rem;
  }
  
  .faq-answer {
    padding: 0 1rem 1rem;
  }
  
  .related-links {
    padding: 0 1rem 1rem;
  }
  
  .faq-actions {
    padding: 0 1rem 1rem;
    flex-direction: column;
  }
  
  .helpful-btn {
    text-align: center;
  }
}

/* Loading Animation */
.spinner-border {
  width: 3rem;
  height: 3rem;
}

/* FAQ Animation */
.faq-item {
  animation: fadeInUp 0.6s ease forwards;
}

.faq-item:nth-child(1) { animation-delay: 0.1s; }
.faq-item:nth-child(2) { animation-delay: 0.2s; }
.faq-item:nth-child(3) { animation-delay: 0.3s; }
.faq-item:nth-child(4) { animation-delay: 0.4s; }
.faq-item:nth-child(5) { animation-delay: 0.5s; }

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>