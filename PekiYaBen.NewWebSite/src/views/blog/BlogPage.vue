<template>
  <div class="blog-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Kişisel Gelişim Yazıları</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Blog</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Blog Content -->
    <section class="blog-content py-5">
      <div class="container">
        <div class="row">
          <!-- Sidebar -->
          <div class="col-lg-3 mb-4">
            <div class="blog-sidebar">
              <!-- Search -->
              <div class="sidebar-widget">
                <h5>Arama</h5>
                <form @submit.prevent="handleSearch">
                  <div class="input-group">
                    <input
                      v-model="searchQuery"
                      type="text"
                      class="form-control"
                      placeholder="Yazı ara..."
                    />
                    <button type="submit" class="btn btn-outline-primary">
                      <i class="fas fa-search"></i>
                    </button>
                  </div>
                </form>
              </div>

              <!-- Categories -->
              <div class="sidebar-widget">
                <h5>Kategoriler</h5>
                <ul class="category-list">
                  <li>
                    <button 
                      class="category-btn"
                      :class="{ active: selectedCategory === null }"
                      @click="setCategory(null)"
                    >
                      Tümü ({{ totalPosts }})
                    </button>
                  </li>
                  <li v-for="category in categories" :key="category.id">
                    <button 
                      class="category-btn"
                      :class="{ active: selectedCategory === category.id }"
                      @click="setCategory(category.id)"
                    >
                      {{ category.title }} ({{ category.postCount }})
                    </button>
                  </li>
                </ul>
              </div>

              <!-- Popular Posts -->
              <div class="sidebar-widget">
                <h5>Popüler Yazılar</h5>
                <div class="popular-posts">
                  <article 
                    v-for="post in popularPosts" 
                    :key="post.id"
                    class="popular-post"
                  >
                    <div class="post-thumbnail">
                      <img 
                        :src="post.image || '/default-blog.jpg'" 
                        :alt="post.title"
                        @error="handleImageError"
                      />
                    </div>
                    <div class="post-info">
                      <h6 class="post-title">
                        <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`">
                          {{ post.title }}
                        </router-link>
                      </h6>
                      <span class="post-date">{{ formatDate(post.createDate) }}</span>
                    </div>
                  </article>
                </div>
              </div>

              <!-- Tags -->
              <div class="sidebar-widget">
                <h5>Etiketler</h5>
                <div class="tags-cloud">
                  <button
                    v-for="tag in tags"
                    :key="tag"
                    class="tag-btn"
                    :class="{ active: selectedTags.includes(tag) }"
                    @click="toggleTag(tag)"
                  >
                    {{ tag }}
                  </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Main Content -->
          <div class="col-lg-9">
            <!-- Filter Info -->
            <div class="filter-info mb-4">
              <div class="d-flex justify-content-between align-items-center">
                <div class="results-count">
                  <span v-if="searchQuery">
                    "<strong>{{ searchQuery }}</strong>" için {{ filteredPosts.length }} sonuç
                  </span>
                  <span v-else-if="selectedCategory">
                    {{ getCategoryName(selectedCategory) }} kategorisinde {{ filteredPosts.length }} yazı
                  </span>
                  <span v-else>
                    Toplam {{ filteredPosts.length }} yazı
                  </span>
                </div>
                <div class="sort-options">
                  <select v-model="sortBy" class="form-select" @change="applySorting">
                    <option value="date_desc">En Yeni</option>
                    <option value="date_asc">En Eski</option>
                    <option value="title">Başlığa Göre</option>
                    <option value="popular">Popülerlik</option>
                  </select>
                </div>
              </div>
            </div>

            <!-- Loading State -->
            <div v-if="loading" class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Yükleniyor...</span>
              </div>
              <p class="mt-3">Yazılar yükleniyor...</p>
            </div>

            <!-- Blog Posts Grid -->
            <div v-else-if="paginatedPosts.length > 0" class="blog-posts">
              <div class="row">
                <div 
                  v-for="post in paginatedPosts" 
                  :key="post.id"
                  class="col-md-6 mb-4"
                >
                  <article class="blog-post-card">
                    <div class="post-image">
                      <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`">
                        <img 
                          :src="post.image || '/default-blog.jpg'" 
                          :alt="post.title"
                          @error="handleImageError"
                        />
                      </router-link>
                      <div class="post-category">
                        {{ getCategoryName(post.categoryId) }}
                      </div>
                    </div>
                    
                    <div class="post-content">
                      <div class="post-meta">
                        <span class="post-date">
                          <i class="fas fa-calendar me-1"></i>
                          {{ formatDate(post.createDate) }}
                        </span>
                        <span v-if="post.coachName" class="post-author">
                          <i class="fas fa-user me-1"></i>
                          {{ post.coachName }}
                        </span>
                        <span class="post-reading-time">
                          <i class="fas fa-clock me-1"></i>
                          {{ calculateReadingTime(post.detail) }} dk
                        </span>
                      </div>
                      
                      <h3 class="post-title">
                        <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`">
                          {{ post.title }}
                        </router-link>
                      </h3>
                      
                      <p class="post-summary">{{ post.summary }}</p>
                      
                      <div class="post-tags" v-if="post.tags">
                        <span 
                          v-for="tag in post.tags.slice(0, 3)" 
                          :key="tag"
                          class="tag"
                        >
                          {{ tag }}
                        </span>
                      </div>
                      
                      <div class="post-footer">
                        <router-link 
                          :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`"
                          class="read-more-btn"
                        >
                          Devamını Oku
                          <i class="fas fa-arrow-right ms-1"></i>
                        </router-link>
                        
                        <div class="post-stats">
                          <span class="views">
                            <i class="fas fa-eye me-1"></i>
                            {{ post.views || 0 }}
                          </span>
                          <span class="likes">
                            <i class="fas fa-heart me-1"></i>
                            {{ post.likes || 0 }}
                          </span>
                        </div>
                      </div>
                    </div>
                  </article>
                </div>
              </div>
            </div>

            <!-- No Results -->
            <div v-else-if="!loading" class="no-results">
              <div class="no-results-icon">
                <i class="fas fa-search"></i>
              </div>
              <h4>Sonuç bulunamadı</h4>
              <p v-if="searchQuery">
                "<strong>{{ searchQuery }}</strong>" için sonuç bulunamadı.
                Lütfen farklı kelimeler deneyiniz.
              </p>
              <p v-else>
                Bu kategoride henüz yazı bulunmamaktadır.
              </p>
              <button @click="clearFilters" class="btn btn-primary">
                Tüm Yazıları Görüntüle
              </button>
            </div>

            <!-- Pagination -->
            <div v-if="totalPages > 1" class="pagination-wrapper">
              <nav aria-label="Blog pagination">
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
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import contentService from '@/services/content'

export default {
  name: 'BlogPage',
  setup() {
    const route = useRoute()
    const router = useRouter()
    
    const posts = ref([])
    const categories = ref([])
    const popularPosts = ref([])
    const loading = ref(true)
    const searchQuery = ref(route.query.search || '')
    const selectedCategory = ref(route.params.id ? parseInt(route.params.id) : null)
    const selectedTags = ref([])
    const sortBy = ref('date_desc')
    const currentPage = ref(parseInt(route.params.page) || 1)
    const itemsPerPage = 6
    
    const tags = ref([
      'Motivasyon', 'Başarı', 'Hedefler', 'Zaman Yönetimi', 'Stres Yönetimi',
      'İletişim', 'Liderlik', 'Özgüven', 'Kariyer', 'İlişkiler'
    ])
    
    // Mock data
    const mockCategories = [
      { id: 1, title: 'Kişisel Gelişim', postCount: 15 },
      { id: 2, title: 'Kariyer', postCount: 8 },
      { id: 3, title: 'İlişkiler', postCount: 12 },
      { id: 4, title: 'Sağlık & Yaşam', postCount: 6 },
      { id: 5, title: 'Motivasyon', postCount: 10 }
    ]
    
    const mockPosts = [
      {
        id: 1,
        title: 'Başarılı Olmak İçin Gerekli 5 Alışkanlık',
        summary: 'Başarılı insanların ortak özelliklerini ve alışkanlıklarını keşfedin.',
        detail: 'Başarı tesadüf değildir. Başarılı insanların ortak alışkanlıkları vardır ve bu alışkanlıkları sürekli olarak uygularlar.',
        categoryId: 1,
        coachName: 'Dr. Ayşe Demir',
        createDate: new Date('2024-01-15'),
        image: '/blog/success-habits.jpg',
        slug: 'basarili-olmak-icin-gerekli-5-aliskanlik',
        tags: ['Başarı', 'Alışkanlık', 'Motivasyon'],
        views: 1250,
        likes: 89
      },
      {
        id: 2,
        title: 'Etkili İletişim Teknikleri',
        summary: 'İletişim becerilerinizi geliştirerek daha etkili ilişkiler kurun.',
        detail: 'İyi iletişim becerileri, hem kişisel hem de profesyonel yaşamda başarının anahtarıdır.',
        categoryId: 3,
        coachName: 'Mehmet Kaya',
        createDate: new Date('2024-01-10'),
        image: '/blog/communication.jpg',
        slug: 'etkili-iletisim-teknikleri',
        tags: ['İletişim', 'İlişkiler', 'Sosyal Beceriler'],
        views: 980,
        likes: 67
      },
      {
        id: 3,
        title: 'Zaman Yönetimi Sanatı',
        summary: 'Zamanınızı daha verimli kullanarak üretkenliğinizi artırın.',
        detail: 'Zaman yönetimi, modern yaşamın en önemli becerilerinden biridir.',
        categoryId: 2,
        coachName: 'Zehra Öztürk',
        createDate: new Date('2024-01-05'),
        image: '/blog/time-management.jpg',
        slug: 'zaman-yonetimi-sanati',
        tags: ['Zaman Yönetimi', 'Verimlilik', 'Organizasyon'],
        views: 1450,
        likes: 102
      }
    ]
    
    const filteredPosts = computed(() => {
      let filtered = posts.value
      
      // Filter by category
      if (selectedCategory.value) {
        filtered = filtered.filter(post => post.categoryId === selectedCategory.value)
      }
      
      // Filter by search query
      if (searchQuery.value.trim()) {
        const query = searchQuery.value.toLowerCase()
        filtered = filtered.filter(post => 
          post.title.toLowerCase().includes(query) ||
          post.summary.toLowerCase().includes(query) ||
          post.detail.toLowerCase().includes(query)
        )
      }
      
      // Filter by tags
      if (selectedTags.value.length > 0) {
        filtered = filtered.filter(post => 
          post.tags && post.tags.some(tag => selectedTags.value.includes(tag))
        )
      }
      
      // Sort posts
      switch (sortBy.value) {
        case 'date_asc':
          filtered.sort((a, b) => new Date(a.createDate) - new Date(b.createDate))
          break
        case 'title':
          filtered.sort((a, b) => a.title.localeCompare(b.title))
          break
        case 'popular':
          filtered.sort((a, b) => (b.views || 0) - (a.views || 0))
          break
        default: // date_desc
          filtered.sort((a, b) => new Date(b.createDate) - new Date(a.createDate))
      }
      
      return filtered
    })
    
    const totalPages = computed(() => 
      Math.ceil(filteredPosts.value.length / itemsPerPage)
    )
    
    const paginatedPosts = computed(() => {
      const start = (currentPage.value - 1) * itemsPerPage
      const end = start + itemsPerPage
      return filteredPosts.value.slice(start, end)
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
    
    const totalPosts = computed(() => posts.value.length)
    
    const loadBlogData = async () => {
      try {
        loading.value = true
        
        // Try to load from API
        try {
          const response = await contentService.getBlogPosts(currentPage.value, selectedCategory.value)
          posts.value = response.posts || []
          categories.value = response.categories || mockCategories
        } catch (apiError) {
          console.warn('API failed, using mock data:', apiError)
          posts.value = mockPosts
          categories.value = mockCategories
        }
        
        // Load popular posts
        popularPosts.value = posts.value
          .sort((a, b) => (b.views || 0) - (a.views || 0))
          .slice(0, 5)
          
        // Update category post counts
        categories.value.forEach(category => {
          category.postCount = posts.value.filter(post => post.categoryId === category.id).length
        })
        
      } catch (error) {
        console.error('Error loading blog data:', error)
        posts.value = mockPosts
        categories.value = mockCategories
      } finally {
        loading.value = false
      }
    }
    
    const formatDate = (date) => {
      return new Intl.DateTimeFormat('tr-TR', {
        day: 'numeric',
        month: 'long',
        year: 'numeric'
      }).format(new Date(date))
    }
    
    const calculateReadingTime = (content) => {
      const wordsPerMinute = 200
      const wordCount = content.split(' ').length
      return Math.ceil(wordCount / wordsPerMinute)
    }
    
    const getCategoryName = (categoryId) => {
      const category = categories.value.find(cat => cat.id === categoryId)
      return category ? category.title : 'Genel'
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-blog.jpg'
    }
    
    const handleSearch = () => {
      currentPage.value = 1
      updateURL()
    }
    
    const setCategory = (categoryId) => {
      selectedCategory.value = categoryId
      currentPage.value = 1
      searchQuery.value = ''
      selectedTags.value = []
      updateURL()
    }
    
    const toggleTag = (tag) => {
      const index = selectedTags.value.indexOf(tag)
      if (index > -1) {
        selectedTags.value.splice(index, 1)
      } else {
        selectedTags.value.push(tag)
      }
      currentPage.value = 1
    }
    
    const applySorting = () => {
      currentPage.value = 1
    }
    
    const changePage = (page) => {
      if (page >= 1 && page <= totalPages.value) {
        currentPage.value = page
        updateURL()
        window.scrollTo({ top: 0, behavior: 'smooth' })
      }
    }
    
    const clearFilters = () => {
      searchQuery.value = ''
      selectedCategory.value = null
      selectedTags.value = []
      currentPage.value = 1
      updateURL()
    }
    
    const updateURL = () => {
      const query = {}
      if (searchQuery.value) query.search = searchQuery.value
      
      let path = '/kisisel-gelisim/yazilar'
      if (currentPage.value > 1) {
        path += `/${currentPage.value}`
      }
      if (selectedCategory.value) {
        path += `/${selectedCategory.value}/${getCategoryName(selectedCategory.value).toLowerCase().replace(/\s+/g, '-')}`
      }
      
      router.push({ path, query })
    }
    
    // Watch for route changes
    watch(() => route.params, (newParams) => {
      currentPage.value = parseInt(newParams.page) || 1
      selectedCategory.value = newParams.id ? parseInt(newParams.id) : null
    })
    
    watch(() => route.query, (newQuery) => {
      searchQuery.value = newQuery.search || ''
    })
    
    onMounted(() => {
      loadBlogData()
    })
    
    return {
      posts,
      categories,
      popularPosts,
      loading,
      searchQuery,
      selectedCategory,
      selectedTags,
      sortBy,
      currentPage,
      tags,
      filteredPosts,
      totalPages,
      paginatedPosts,
      visiblePages,
      totalPosts,
      formatDate,
      calculateReadingTime,
      getCategoryName,
      handleImageError,
      handleSearch,
      setCategory,
      toggleTag,
      applySorting,
      changePage,
      clearFilters
    }
  }
}
</script>

<style scoped>
.blog-page {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.blog-sidebar {
  position: sticky;
  top: 100px;
}

.sidebar-widget {
  background: white;
  border-radius: 15px;
  padding: 1.5rem;
  margin-bottom: 2rem;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
}

.sidebar-widget h5 {
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
}

.category-btn:hover {
  background: #f8f9fa;
  color: #007bff;
}

.category-btn.active {
  background: #007bff;
  color: white;
}

.popular-post {
  display: flex;
  gap: 1rem;
  margin-bottom: 1rem;
  padding-bottom: 1rem;
  border-bottom: 1px solid #f8f9fa;
}

.popular-post:last-child {
  border-bottom: none;
  margin-bottom: 0;
}

.post-thumbnail {
  flex-shrink: 0;
  width: 60px;
  height: 60px;
  border-radius: 10px;
  overflow: hidden;
}

.post-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.post-info {
  flex: 1;
}

.post-title {
  margin: 0;
  font-size: 0.9rem;
  font-weight: 600;
}

.post-title a {
  color: #2c3e50;
  text-decoration: none;
  transition: color 0.3s ease;
}

.post-title a:hover {
  color: #007bff;
}

.post-date {
  font-size: 0.8rem;
  color: #6c757d;
}

.tags-cloud {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.tag-btn {
  background: #f8f9fa;
  border: 2px solid #e9ecef;
  border-radius: 20px;
  padding: 0.5rem 1rem;
  font-size: 0.85rem;
  font-weight: 500;
  color: #6c757d;
  cursor: pointer;
  transition: all 0.3s ease;
}

.tag-btn:hover {
  border-color: #007bff;
  color: #007bff;
}

.tag-btn.active {
  background: #007bff;
  border-color: #007bff;
  color: white;
}

.filter-info {
  background: white;
  border-radius: 15px;
  padding: 1.5rem;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
}

.form-select {
  border: 2px solid #e9ecef;
  border-radius: 10px;
  font-size: 0.9rem;
}

.blog-post-card {
  background: white;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  transition: all 0.3s ease;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.blog-post-card:hover {
  transform: translateY(-10px);
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.15);
}

.post-image {
  position: relative;
  height: 200px;
  overflow: hidden;
}

.post-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.blog-post-card:hover .post-image img {
  transform: scale(1.1);
}

.post-category {
  position: absolute;
  top: 1rem;
  left: 1rem;
  background: rgba(0, 123, 255, 0.9);
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
}

.post-content {
  padding: 1.5rem;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.post-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1rem;
  font-size: 0.85rem;
  color: #6c757d;
}

.post-title {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 1rem;
  line-height: 1.3;
}

.post-title a {
  color: #2c3e50;
  text-decoration: none;
  transition: color 0.3s ease;
}

.post-title a:hover {
  color: #007bff;
}

.post-summary {
  color: #6c757d;
  line-height: 1.6;
  margin-bottom: 1rem;
  flex: 1;
}

.post-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

.tag {
  background: #f8f9fa;
  color: #6c757d;
  padding: 0.25rem 0.75rem;
  border-radius: 15px;
  font-size: 0.8rem;
  font-weight: 500;
}

.post-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}

.read-more-btn {
  color: #007bff;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s ease;
}

.read-more-btn:hover {
  color: #0056b3;
  transform: translateX(5px);
}

.post-stats {
  display: flex;
  gap: 1rem;
  font-size: 0.85rem;
  color: #6c757d;
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

/* Responsive */
@media (max-width: 991.98px) {
  .blog-sidebar {
    position: static;
  }
  
  .sidebar-widget {
    margin-bottom: 1rem;
  }
  
  .filter-info .d-flex {
    flex-direction: column;
    gap: 1rem;
  }
}

@media (max-width: 768px) {
  .post-meta {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .post-footer {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }
  
  .popular-post {
    flex-direction: column;
    text-align: center;
  }
  
  .post-thumbnail {
    width: 100%;
    height: 120px;
  }
}

/* Loading Animation */
.spinner-border {
  width: 3rem;
  height: 3rem;
}

/* Card Animation */
.blog-post-card {
  animation: fadeInUp 0.6s ease forwards;
}

.blog-post-card:nth-child(1) { animation-delay: 0.1s; }
.blog-post-card:nth-child(2) { animation-delay: 0.2s; }
.blog-post-card:nth-child(3) { animation-delay: 0.3s; }
.blog-post-card:nth-child(4) { animation-delay: 0.4s; }
.blog-post-card:nth-child(5) { animation-delay: 0.5s; }
.blog-post-card:nth-child(6) { animation-delay: 0.6s; }

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