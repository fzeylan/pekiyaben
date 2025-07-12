<template>
  <div class="blog-post-page">
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
                  <router-link to="/kisisel-gelisim/yazilar" class="text-white-50">Blog</router-link>
                </li>
                <li class="breadcrumb-item active text-white">{{ post?.title }}</li>
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
        <p class="mt-3">Yazı yükleniyor...</p>
      </div>
    </div>

    <!-- Blog Post Content -->
    <section v-else-if="post" class="blog-post-content py-5">
      <div class="container">
        <div class="row">
          <!-- Main Content -->
          <div class="col-lg-8">
            <article class="blog-post">
              <!-- Post Header -->
              <header class="post-header">
                <div class="post-category">
                  {{ getCategoryName(post.categoryId) }}
                </div>
                <h1 class="post-title">{{ post.title }}</h1>
                
                <div class="post-meta">
                  <div class="author-info">
                    <img 
                      :src="post.authorPhoto || '/default-coach.jpg'" 
                      :alt="post.coachName"
                      class="author-avatar"
                      @error="handleImageError"
                    />
                    <div class="author-details">
                      <span class="author-name">{{ post.coachName || 'PekiYaBen Editörü' }}</span>
                      <span class="author-title">{{ post.authorTitle || 'Editör' }}</span>
                    </div>
                  </div>
                  
                  <div class="post-stats">
                    <span class="post-date">
                      <i class="fas fa-calendar me-1"></i>
                      {{ formatDate(post.createDate) }}
                    </span>
                    <span class="reading-time">
                      <i class="fas fa-clock me-1"></i>
                      {{ calculateReadingTime(post.detail) }} dk okuma
                    </span>
                    <span class="view-count">
                      <i class="fas fa-eye me-1"></i>
                      {{ post.views || 0 }} görüntülenme
                    </span>
                  </div>
                </div>
              </header>

              <!-- Featured Image -->
              <div v-if="post.image" class="featured-image">
                <img 
                  :src="post.image" 
                  :alt="post.title"
                  @error="handleImageError"
                />
              </div>

              <!-- Post Summary -->
              <div v-if="post.summary" class="post-summary">
                <p>{{ post.summary }}</p>
              </div>

              <!-- Post Content -->
              <div class="post-content" v-html="post.detail"></div>

              <!-- Tags -->
              <div v-if="post.tags && post.tags.length > 0" class="post-tags">
                <h6>Etiketler:</h6>
                <div class="tags-list">
                  <span v-for="tag in post.tags" :key="tag" class="tag">
                    {{ tag }}
                  </span>
                </div>
              </div>

              <!-- Social Share -->
              <div class="social-share">
                <h6>Paylaş:</h6>
                <div class="share-buttons">
                  <button @click="shareOnFacebook" class="share-btn facebook">
                    <i class="fab fa-facebook-f"></i>
                    Facebook
                  </button>
                  <button @click="shareOnTwitter" class="share-btn twitter">
                    <i class="fab fa-twitter"></i>
                    Twitter
                  </button>
                  <button @click="shareOnLinkedIn" class="share-btn linkedin">
                    <i class="fab fa-linkedin-in"></i>
                    LinkedIn
                  </button>
                  <button @click="shareOnWhatsApp" class="share-btn whatsapp">
                    <i class="fab fa-whatsapp"></i>
                    WhatsApp
                  </button>
                  <button @click="copyLink" class="share-btn copy" :class="{ copied: linkCopied }">
                    <i class="fas fa-link"></i>
                    {{ linkCopied ? 'Kopyalandı!' : 'Link Kopyala' }}
                  </button>
                </div>
              </div>

              <!-- Like/Reaction -->
              <div class="post-reactions">
                <button 
                  @click="toggleLike"
                  class="like-btn"
                  :class="{ liked: isLiked }"
                >
                  <i class="fas fa-heart"></i>
                  <span>{{ likeCount }}</span>
                </button>
                <button @click="showCommentForm = !showCommentForm" class="comment-btn">
                  <i class="fas fa-comment"></i>
                  <span>Yorum Yap</span>
                </button>
              </div>

              <!-- Comment Form -->
              <div v-if="showCommentForm" class="comment-form">
                <h5>Yorum Yap</h5>
                <form @submit.prevent="submitComment">
                  <div class="row">
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <input
                          v-model="commentForm.name"
                          type="text"
                          class="form-control"
                          placeholder="Adınız"
                          required
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <input
                          v-model="commentForm.email"
                          type="email"
                          class="form-control"
                          placeholder="E-posta adresiniz"
                          required
                        />
                      </div>
                    </div>
                  </div>
                  <div class="form-group mb-3">
                    <textarea
                      v-model="commentForm.comment"
                      class="form-control"
                      rows="4"
                      placeholder="Yorumunuz..."
                      required
                    ></textarea>
                  </div>
                  <button type="submit" class="btn btn-primary" :disabled="isSubmittingComment">
                    <span v-if="isSubmittingComment" class="spinner-border spinner-border-sm me-2"></span>
                    Yorum Gönder
                  </button>
                </form>
              </div>
            </article>
          </div>

          <!-- Sidebar -->
          <div class="col-lg-4">
            <div class="blog-sidebar">
              <!-- Author Card -->
              <div class="sidebar-widget author-card">
                <h5>Yazar Hakkında</h5>
                <div class="author-info">
                  <img 
                    :src="post.authorPhoto || '/default-coach.jpg'" 
                    :alt="post.coachName"
                    class="author-image"
                    @error="handleImageError"
                  />
                  <h6>{{ post.coachName || 'PekiYaBen Editörü' }}</h6>
                  <p class="author-bio">
                    {{ post.authorBio || 'Kişisel gelişim alanında uzman yazar ve koç.' }}
                  </p>
                  <div v-if="post.coachId" class="author-actions">
                    <router-link 
                      :to="`/kocluk-ekibimiz/${post.categoryId}/category/${post.coachId}/${post.coachName.toLowerCase().replace(/\s+/g, '-')}`"
                      class="btn btn-outline-primary btn-sm"
                    >
                      Profili Görüntüle
                    </router-link>
                  </div>
                </div>
              </div>

              <!-- Related Posts -->
              <div class="sidebar-widget">
                <h5>İlgili Yazılar</h5>
                <div class="related-posts">
                  <article 
                    v-for="relatedPost in relatedPosts" 
                    :key="relatedPost.id"
                    class="related-post"
                  >
                    <div class="post-thumbnail">
                      <router-link :to="`/kisisel-gelisim/yazilar/${relatedPost.id}/${relatedPost.slug}`">
                        <img 
                          :src="relatedPost.image || '/default-blog.jpg'" 
                          :alt="relatedPost.title"
                          @error="handleImageError"
                        />
                      </router-link>
                    </div>
                    <div class="post-info">
                      <h6 class="post-title">
                        <router-link :to="`/kisisel-gelisim/yazilar/${relatedPost.id}/${relatedPost.slug}`">
                          {{ relatedPost.title }}
                        </router-link>
                      </h6>
                      <span class="post-date">{{ formatDate(relatedPost.createDate) }}</span>
                    </div>
                  </article>
                </div>
              </div>

              <!-- Popular Tags -->
              <div class="sidebar-widget">
                <h5>Popüler Etiketler</h5>
                <div class="tags-cloud">
                  <router-link
                    v-for="tag in popularTags"
                    :key="tag"
                    :to="`/kisisel-gelisim/yazilar?tag=${tag}`"
                    class="tag-link"
                  >
                    {{ tag }}
                  </router-link>
                </div>
              </div>

              <!-- Newsletter -->
              <div class="sidebar-widget newsletter-widget">
                <h5>Bülten</h5>
                <p>En yeni yazılarımızdan haberdar olmak için bültenimize abone olun.</p>
                <form @submit.prevent="subscribeNewsletter">
                  <div class="form-group mb-3">
                    <input
                      v-model="newsletterEmail"
                      type="email"
                      class="form-control"
                      placeholder="E-posta adresiniz"
                      required
                    />
                  </div>
                  <button type="submit" class="btn btn-primary w-100" :disabled="isSubscribing">
                    <span v-if="isSubscribing" class="spinner-border spinner-border-sm me-2"></span>
                    Abone Ol
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Not Found -->
    <div v-else class="container py-5">
      <div class="text-center">
        <h2>Yazı Bulunamadı</h2>
        <p>Aradığınız yazı mevcut değil veya kaldırılmış olabilir.</p>
        <router-link to="/kisisel-gelisim/yazilar" class="btn btn-primary">
          Blog'a Dön
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useToast } from 'vue-toastification'
import contentService from '@/services/content'

export default {
  name: 'BlogPostPage',
  setup() {
    const route = useRoute()
    const toast = useToast()
    
    const post = ref(null)
    const relatedPosts = ref([])
    const loading = ref(true)
    const showCommentForm = ref(false)
    const isLiked = ref(false)
    const likeCount = ref(0)
    const linkCopied = ref(false)
    const isSubmittingComment = ref(false)
    const isSubscribing = ref(false)
    const newsletterEmail = ref('')
    
    const commentForm = ref({
      name: '',
      email: '',
      comment: ''
    })
    
    const popularTags = ref([
      'Motivasyon', 'Başarı', 'Hedefler', 'Zaman Yönetimi', 
      'Stres Yönetimi', 'İletişim', 'Liderlik', 'Özgüven'
    ])
    
    const categories = ref([
      { id: 1, title: 'Kişisel Gelişim' },
      { id: 2, title: 'Kariyer' },
      { id: 3, title: 'İlişkiler' },
      { id: 4, title: 'Sağlık & Yaşam' },
      { id: 5, title: 'Motivasyon' }
    ])
    
    const loadPost = async () => {
      try {
        loading.value = true
        const postId = route.params.id
        const postTitle = route.params.title
        
        try {
          const response = await contentService.getBlogPost(postId, postTitle)
          post.value = response.post
          relatedPosts.value = response.relatedPosts || []
        } catch (apiError) {
          // Mock data fallback
          console.warn('API failed, using mock data:', apiError)
          post.value = {
            id: parseInt(postId),
            title: 'Başarılı Olmak İçin Gerekli 5 Alışkanlık',
            summary: 'Başarılı insanların ortak özelliklerini ve alışkanlıklarını keşfedin.',
            detail: `
              <p>Başarı tesadüf değildir. Başarılı insanların ortak alışkanlıkları vardır ve bu alışkanlıkları sürekli olarak uygularlar. Bu yazıda, başarılı olmak için gerekli 5 temel alışkanlığı inceleyeceğiz.</p>
              
              <h3>1. Erken Kalkmak</h3>
              <p>Başarılı insanların çoğu günün erken saatlerinde kalkar. Bu, onlara günü planlamak ve en önemli işlere odaklanmak için daha fazla zaman verir.</p>
              
              <h3>2. Sürekli Öğrenme</h3>
              <p>Başarılı insanlar hiçbir zaman öğrenmeyi bırakmazlar. Kitap okuma, podcast dinleme veya yeni beceriler öğrenme gibi aktivitelerle kendilerini sürekli geliştirirler.</p>
              
              <h3>3. Hedef Belirleme</h3>
              <p>Net ve ölçülebilir hedefler belirlemek, başarılı insanların en önemli alışkanlıklarından biridir. Bu hedefler onları motive eder ve yön verir.</p>
              
              <h3>4. Zaman Yönetimi</h3>
              <p>Zamanı verimli kullanmak, başarı için kritik öneme sahiptir. Öncelikleri belirlemek ve dikkat dağıtıcı unsurları elimine etmek bu konuda yardımcı olur.</p>
              
              <h3>5. Sağlıklı Yaşam</h3>
              <p>Fiziksel ve mental sağlık, sürdürülebilir başarının temelidir. Düzenli egzersiz, sağlıklı beslenme ve yeterli uyku başarılı insanların vazgeçilmez alışkanlıklarıdır.</p>
              
              <p>Bu alışkanlıkları hayatınıza entegre ederek, siz de başarı yolunda önemli adımlar atabilirsiniz. Unutmayın, alışkanlık haline getirmek zaman alır, sabırlı olmak önemlidir.</p>
            `,
            categoryId: 1,
            coachName: 'Dr. Ayşe Demir',
            coachId: 1,
            authorTitle: 'Kişisel Gelişim Uzmanı',
            authorBio: 'Kişisel gelişim alanında 10 yılı aşkın deneyime sahip uzman koç.',
            createDate: new Date('2024-01-15'),
            image: '/blog/success-habits.jpg',
            slug: 'basarili-olmak-icin-gerekli-5-aliskanlik',
            tags: ['Başarı', 'Alışkanlık', 'Motivasyon'],
            views: 1250,
            likes: 89
          }
          
          relatedPosts.value = [
            {
              id: 2,
              title: 'Etkili İletişim Teknikleri',
              createDate: new Date('2024-01-10'),
              image: '/blog/communication.jpg',
              slug: 'etkili-iletisim-teknikleri'
            },
            {
              id: 3,
              title: 'Zaman Yönetimi Sanatı',
              createDate: new Date('2024-01-05'),
              image: '/blog/time-management.jpg',
              slug: 'zaman-yonetimi-sanati'
            }
          ]
        }
        
        if (post.value) {
          likeCount.value = post.value.likes || 0
          // Increment view count
          post.value.views = (post.value.views || 0) + 1
        }
      } catch (error) {
        console.error('Error loading post:', error)
        post.value = null
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
      const wordCount = content.replace(/<[^>]*>/g, '').split(' ').length
      return Math.ceil(wordCount / wordsPerMinute)
    }
    
    const getCategoryName = (categoryId) => {
      const category = categories.value.find(cat => cat.id === categoryId)
      return category ? category.title : 'Genel'
    }
    
    const handleImageError = (event) => {
      event.target.src = '/default-blog.jpg'
    }
    
    const shareOnFacebook = () => {
      const url = encodeURIComponent(window.location.href)
      const title = encodeURIComponent(post.value.title)
      window.open(`https://www.facebook.com/sharer/sharer.php?u=${url}&t=${title}`, '_blank', 'width=600,height=400')
    }
    
    const shareOnTwitter = () => {
      const url = encodeURIComponent(window.location.href)
      const title = encodeURIComponent(post.value.title)
      window.open(`https://twitter.com/intent/tweet?url=${url}&text=${title}`, '_blank', 'width=600,height=400')
    }
    
    const shareOnLinkedIn = () => {
      const url = encodeURIComponent(window.location.href)
      window.open(`https://www.linkedin.com/sharing/share-offsite/?url=${url}`, '_blank', 'width=600,height=400')
    }
    
    const shareOnWhatsApp = () => {
      const url = encodeURIComponent(window.location.href)
      const title = encodeURIComponent(post.value.title)
      window.open(`https://wa.me/?text=${title} ${url}`, '_blank')
    }
    
    const copyLink = async () => {
      try {
        await navigator.clipboard.writeText(window.location.href)
        linkCopied.value = true
        toast.success('Link kopyalandı!')
        setTimeout(() => {
          linkCopied.value = false
        }, 3000)
      } catch (error) {
        toast.error('Link kopyalanamadı.')
      }
    }
    
    const toggleLike = () => {
      isLiked.value = !isLiked.value
      likeCount.value += isLiked.value ? 1 : -1
      
      if (isLiked.value) {
        toast.success('Yazıyı beğendiniz!')
      }
    }
    
    const submitComment = async () => {
      if (!commentForm.value.name || !commentForm.value.email || !commentForm.value.comment) {
        toast.error('Lütfen tüm alanları doldurun.')
        return
      }
      
      isSubmittingComment.value = true
      
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1500))
        
        toast.success('Yorumunuz başarıyla gönderildi! Onaylandıktan sonra yayınlanacaktır.')
        
        // Reset form
        commentForm.value = {
          name: '',
          email: '',
          comment: ''
        }
        showCommentForm.value = false
      } catch (error) {
        toast.error('Yorum gönderilirken hata oluştu.')
      } finally {
        isSubmittingComment.value = false
      }
    }
    
    const subscribeNewsletter = async () => {
      if (!newsletterEmail.value) {
        toast.error('Lütfen e-posta adresinizi girin.')
        return
      }
      
      isSubscribing.value = true
      
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 1000))
        
        toast.success('Bülten aboneliği başarılı!')
        newsletterEmail.value = ''
      } catch (error) {
        toast.error('Abonelik sırasında hata oluştu.')
      } finally {
        isSubscribing.value = false
      }
    }
    
    onMounted(() => {
      loadPost()
    })
    
    return {
      post,
      relatedPosts,
      loading,
      showCommentForm,
      isLiked,
      likeCount,
      linkCopied,
      isSubmittingComment,
      isSubscribing,
      newsletterEmail,
      commentForm,
      popularTags,
      formatDate,
      calculateReadingTime,
      getCategoryName,
      handleImageError,
      shareOnFacebook,
      shareOnTwitter,
      shareOnLinkedIn,
      shareOnWhatsApp,
      copyLink,
      toggleLike,
      submitComment,
      subscribeNewsletter
    }
  }
}
</script>

<style scoped>
.blog-post-page {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.blog-post {
  background: white;
  border-radius: 15px;
  padding: 2rem;
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.08);
  margin-bottom: 2rem;
}

.post-header {
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid #e9ecef;
}

.post-category {
  display: inline-block;
  background: #007bff;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  margin-bottom: 1rem;
}

.post-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  line-height: 1.2;
  margin-bottom: 1.5rem;
}

.post-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
}

.author-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.author-avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  object-fit: cover;
}

.author-details .author-name {
  display: block;
  font-weight: 600;
  color: #2c3e50;
}

.author-details .author-title {
  display: block;
  font-size: 0.9rem;
  color: #6c757d;
}

.post-stats {
  display: flex;
  gap: 1.5rem;
  font-size: 0.9rem;
  color: #6c757d;
}

.featured-image {
  margin-bottom: 2rem;
  border-radius: 15px;
  overflow: hidden;
}

.featured-