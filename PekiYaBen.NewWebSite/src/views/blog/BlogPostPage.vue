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

        <!-- Error State -->
        <div v-else-if="error" class="container py-5">
            <div class="alert alert-danger" role="alert">
                <h4 class="alert-heading">Hata!</h4>
                <p>{{ error }}</p>
                <hr>
                <router-link to="/kisisel-gelisim/yazilar" class="btn btn-primary">
                    Blog Sayfasına Dön
                </router-link>
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
                            <header class="post-header mb-4">
                                <div class="post-category mb-2">
                                    <span class="badge bg-primary">{{ getCategoryName(post.categoryId) }}</span>
                                </div>
                                <h1 class="post-title mb-3">{{ post.title }}</h1>
                                <div class="post-meta d-flex align-items-center text-muted">
                                    <div class="author-info d-flex align-items-center me-4">
                                        <img :src="post.authorAvatar || '/default-avatar.png'"
                                             :alt="post.authorName"
                                             class="author-avatar me-2"
                                             @error="handleImageError" />
                                        <span>{{ post.authorName }}</span>
                                    </div>
                                    <div class="post-date me-4">
                                        <i class="fas fa-calendar-alt me-1"></i>
                                        {{ formatDate(post.createDate) }}
                                    </div>
                                    <div class="post-views me-4">
                                        <i class="fas fa-eye me-1"></i>
                                        {{ post.viewCount || 0 }} görüntüleme
                                    </div>
                                    <div class="reading-time">
                                        <i class="fas fa-clock me-1"></i>
                                        {{ estimatedReadingTime }} dk okuma
                                    </div>
                                </div>
                            </header>

                            <!-- Featured Image -->
                            <div v-if="post.image" class="post-featured-image mb-4">
                                <img :src="post.image"
                                     :alt="post.title"
                                     class="img-fluid rounded"
                                     @error="handleImageError" />
                            </div>

                            <!-- Post Content -->
                            <div class="post-content" v-html="post.content"></div>

                            <!-- Post Tags -->
                            <div v-if="post.tags && post.tags.length" class="post-tags mt-4 pt-4 border-top">
                                <h6 class="mb-3">Etiketler:</h6>
                                <div class="tags-list">
                                    <span v-for="tag in post.tags"
                                          :key="tag"
                                          class="badge bg-light text-dark me-2 mb-2">
                                        #{{ tag }}
                                    </span>
                                </div>
                            </div>

                            <!-- Social Share -->
                            <div class="social-share mt-4 pt-4 border-top">
                                <h6 class="mb-3">Bu yazıyı paylaş:</h6>
                                <div class="share-buttons">
                                    <a :href="getShareUrl('facebook')"
                                       target="_blank"
                                       class="btn btn-facebook me-2"
                                       title="Facebook'ta Paylaş">
                                        <i class="fab fa-facebook-f"></i>
                                    </a>
                                    <a :href="getShareUrl('twitter')"
                                       target="_blank"
                                       class="btn btn-twitter me-2"
                                       title="Twitter'da Paylaş">
                                        <i class="fab fa-twitter"></i>
                                    </a>
                                    <a :href="getShareUrl('linkedin')"
                                       target="_blank"
                                       class="btn btn-linkedin me-2"
                                       title="LinkedIn'de Paylaş">
                                        <i class="fab fa-linkedin-in"></i>
                                    </a>
                                    <a :href="getShareUrl('whatsapp')"
                                       target="_blank"
                                       class="btn btn-whatsapp me-2"
                                       title="WhatsApp'ta Paylaş">
                                        <i class="fab fa-whatsapp"></i>
                                    </a>
                                    <button @click="copyToClipboard"
                                            class="btn btn-secondary"
                                            title="Linki Kopyala">
                                        <i class="fas fa-link"></i>
                                    </button>
                                </div>
                            </div>

                            <!-- Comments Section -->
                            <div class="comments-section mt-5 pt-4 border-top">
                                <h5 class="mb-4">Yorumlar ({{ comments.length }})</h5>

                                <!-- Comment Form -->
                                <div v-if="isAuthenticated" class="comment-form mb-4">
                                    <form @submit.prevent="submitComment">
                                        <div class="mb-3">
                                            <label for="commentText" class="form-label">Yorumunuz</label>
                                            <textarea id="commentText"
                                                      v-model="newComment.text"
                                                      class="form-control"
                                                      rows="4"
                                                      placeholder="Yorumunuzu yazın..."
                                                      required></textarea>
                                        </div>
                                        <button type="submit"
                                                class="btn btn-primary"
                                                :disabled="isSubmittingComment">
                                            <span v-if="isSubmittingComment" class="spinner-border spinner-border-sm me-2"></span>
                                            Yorum Gönder
                                        </button>
                                    </form>
                                </div>

                                <!-- Login Required Message -->
                                <div v-else class="alert alert-info">
                                    <p class="mb-0">
                                        Yorum yapabilmek için
                                        <router-link to="/hesabim/giris">giriş yapmanız</router-link>
                                        gerekmektedir.
                                    </p>
                                </div>

                                <!-- Comments List -->
                                <div v-if="comments.length" class="comments-list">
                                    <div v-for="comment in comments"
                                         :key="comment.id"
                                         class="comment-item mb-4">
                                        <div class="d-flex">
                                            <img :src="comment.userAvatar || '/default-avatar.png'"
                                                 :alt="comment.userName"
                                                 class="comment-avatar me-3"
                                                 @error="handleImageError" />
                                            <div class="comment-content flex-grow-1">
                                                <div class="comment-header d-flex justify-content-between align-items-start mb-2">
                                                    <div>
                                                        <h6 class="comment-author mb-1">{{ comment.userName }}</h6>
                                                        <small class="text-muted">{{ formatDate(comment.createDate) }}</small>
                                                    </div>
                                                    <div v-if="canDeleteComment(comment)" class="comment-actions">
                                                        <button @click="deleteComment(comment.id)"
                                                                class="btn btn-link btn-sm text-danger p-0"
                                                                title="Yorumu Sil">
                                                            <i class="fas fa-trash"></i>
                                                        </button>
                                                    </div>
                                                </div>
                                                <p class="comment-text mb-0">{{ comment.text }}</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- No Comments -->
                                <div v-else class="text-center py-4">
                                    <i class="fas fa-comments fa-3x text-muted mb-3"></i>
                                    <p class="text-muted">Henüz yorum yapılmamış. İlk yorumu siz yapın!</p>
                                </div>
                            </div>
                        </article>
                    </div>

                    <!-- Sidebar -->
                    <div class="col-lg-4">
                        <div class="blog-sidebar">
                            <!-- Author Info -->
                            <div v-if="post.authorName" class="sidebar-widget author-widget">
                                <h5>Yazar Hakkında</h5>
                                <div class="author-card">
                                    <img :src="post.authorAvatar || '/default-avatar.png'"
                                         :alt="post.authorName"
                                         class="author-image"
                                         @error="handleImageError" />
                                    <h6 class="author-name">{{ post.authorName }}</h6>
                                    <p class="author-title text-muted">{{ post.authorTitle || 'Yazar' }}</p>
                                    <p class="author-bio">
                                        {{ post.authorBio || 'Kişisel gelişim ve koçluk alanında uzman.' }}
                                    </p>
                                    <div v-if="post.coachId" class="author-actions">
                                        <router-link :to="`/kocluk-ekibimiz/${post.categoryId}/category/${post.coachId}/${post.authorName.toLowerCase().replace(/\s+/g, '-')}`"
                                                     class="btn btn-outline-primary btn-sm">
                                            Profili Görüntüle
                                        </router-link>
                                    </div>
                                </div>
                            </div>

                            <!-- Related Posts -->
                            <div class="sidebar-widget">
                                <h5>İlgili Yazılar</h5>
                                <div v-if="loadingRelated" class="text-center py-3">
                                    <div class="spinner-border spinner-border-sm text-primary"></div>
                                </div>
                                <div v-else-if="relatedPosts.length" class="related-posts">
                                    <article v-for="relatedPost in relatedPosts"
                                             :key="relatedPost.id"
                                             class="related-post mb-3">
                                        <div class="row g-2">
                                            <div class="col-4">
                                                <div class="post-thumbnail">
                                                    <router-link :to="`/kisisel-gelisim/yazilar/${relatedPost.id}/${relatedPost.slug}`">
                                                        <img :src="relatedPost.image || '/default-blog.jpg'"
                                                             :alt="relatedPost.title"
                                                             class="img-fluid rounded"
                                                             @error="handleImageError" />
                                                    </router-link>
                                                </div>
                                            </div>
                                            <div class="col-8">
                                                <div class="post-info">
                                                    <h6 class="post-title mb-1">
                                                        <router-link :to="`/kisisel-gelisim/yazilar/${relatedPost.id}/${relatedPost.slug}`"
                                                                     class="text-decoration-none">
                                                            {{ relatedPost.title }}
                                                        </router-link>
                                                    </h6>
                                                    <small class="post-date text-muted">{{ formatDate(relatedPost.createDate) }}</small>
                                                </div>
                                            </div>
                                        </div>
                                    </article>
                                </div>
                                <div v-else class="text-muted">
                                    İlgili yazı bulunamadı.
                                </div>
                            </div>

                            <!-- Popular Posts -->
                            <div class="sidebar-widget">
                                <h5>Popüler Yazılar</h5>
                                <div v-if="loadingPopular" class="text-center py-3">
                                    <div class="spinner-border spinner-border-sm text-primary"></div>
                                </div>
                                <div v-else-if="popularPosts.length" class="popular-posts">
                                    <article v-for="(popularPost, index) in popularPosts"
                                             :key="popularPost.id"
                                             class="popular-post mb-3">
                                        <div class="d-flex">
                                            <div class="post-rank me-3">
                                                <span class="rank-number">{{ index + 1 }}</span>
                                            </div>
                                            <div class="post-info flex-grow-1">
                                                <h6 class="post-title mb-1">
                                                    <router-link :to="`/kisisel-gelisim/yazilar/${popularPost.id}/${popularPost.slug}`"
                                                                 class="text-decoration-none">
                                                        {{ popularPost.title }}
                                                    </router-link>
                                                </h6>
                                                <small class="post-meta text-muted">
                                                    {{ formatDate(popularPost.createDate) }} • {{ popularPost.viewCount }} görüntüleme
                                                </small>
                                            </div>
                                        </div>
                                    </article>
                                </div>
                            </div>

                            <!-- Categories -->
                            <div class="sidebar-widget">
                                <h5>Kategoriler</h5>
                                <div v-if="categories.length" class="categories-list">
                                    <router-link v-for="category in categories"
                                                 :key="category.id"
                                                 :to="`/kisisel-gelisim/yazilar/${category.id}/${category.slug}`"
                                                 class="category-link d-flex justify-content-between align-items-center">
                                        <span>{{ category.name }}</span>
                                        <span class="badge bg-light text-dark">{{ category.postCount }}</span>
                                    </router-link>
                                </div>
                            </div>

                            <!-- Newsletter -->
                            <div class="sidebar-widget newsletter-widget">
                                <h5>Bülten Aboneliği</h5>
                                <p class="text-muted">Yeni yazılarımızdan haberdar olmak için bültenimize abone olun.</p>
                                <form @submit.prevent="subscribeNewsletter">
                                    <div class="mb-3">
                                        <input v-model="newsletterEmail"
                                               type="email"
                                               class="form-control"
                                               placeholder="E-posta adresiniz"
                                               required />
                                    </div>
                                    <button type="submit"
                                            class="btn btn-primary w-100"
                                            :disabled="isSubscribing">
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
    </div>
</template>

<script>
    import { ref, reactive, computed, onMounted, watch } from 'vue'
    import { useRoute, useRouter } from 'vue-router'
    import { useAuthStore } from '@/stores/auth'
    import { useToast } from 'vue-toastification'
    import contentService from '@/services/content'
    import commentService from '@/services/comment'
    import dayjs from 'dayjs'

    export default {
        name: 'BlogPostPage',
        setup() {
            const route = useRoute()
            const router = useRouter()
            const authStore = useAuthStore()
            const toast = useToast()

            // Reactive data
            const post = ref(null)
            const comments = ref([])
            const relatedPosts = ref([])
            const popularPosts = ref([])
            const categories = ref([])
            const loading = ref(true)
            const loadingRelated = ref(false)
            const loadingPopular = ref(false)
            const error = ref(null)
            const isSubmittingComment = ref(false)
            const isSubscribing = ref(false)
            const newsletterEmail = ref('')

            // Comment form
            const newComment = reactive({
                text: ''
            })

            // Computed properties
            const isAuthenticated = computed(() => authStore.isAuthenticated)
            const currentUser = computed(() => authStore.user)

            const estimatedReadingTime = computed(() => {
                if (!post.value?.content) return 0
                const wordsPerMinute = 200
                const wordCount = post.value.content.replace(/<[^>]*>/g, '').split(/\s+/).length
                return Math.ceil(wordCount / wordsPerMinute)
            })

            // Methods
            const loadPost = async () => {
                try {
                    loading.value = true
                    error.value = null

                    const postId = route.params.id
                    const response = await contentService.getBlogPost(postId)

                    if (!response.data) {
                        throw new Error('Yazı bulunamadı.')
                    }

                    post.value = response.data

                    // Update page title
                    document.title = `${post.value.title} - PekiYaBen`

                    // Update meta description
                    const metaDescription = document.querySelector('meta[name="description"]')
                    if (metaDescription) {
                        metaDescription.content = post.value.summary || post.value.title
                    }

                } catch (err) {
                    error.value = err.message || 'Yazı yüklenirken hata oluştu.'
                    console.error('Blog post load error:', err)
                } finally {
                    loading.value = false
                }
            }

            const loadComments = async () => {
                if (!post.value) return

                try {
                    const response = await commentService.getPostComments(post.value.id)
                    comments.value = response.data || []
                } catch (err) {
                    console.error('Comments load error:', err)
                }
            }

            const loadRelatedPosts = async () => {
                if (!post.value) return

                try {
                    loadingRelated.value = true
                    const response = await contentService.getRelatedPosts(post.value.id, post.value.categoryId)
                    relatedPosts.value = response.data || []
                } catch (err) {
                    console.error('Related posts load error:', err)
                } finally {
                    loadingRelated.value = false
                }
            }

            const loadPopularPosts = async () => {
                try {
                    loadingPopular.value = true
                    const response = await contentService.getPopularPosts(5)
                    popularPosts.value = response.data || []
                } catch (err) {
                    console.error('Popular posts load error:', err)
                } finally {
                    loadingPopular.value = false
                }
            }

            const loadCategories = async () => {
                try {
                    const response = await contentService.getBlogCategories()
                    categories.value = response.data || []
                } catch (err) {
                    console.error('Categories load error:', err)
                }
            }

            const submitComment = async () => {
                if (!newComment.text.trim()) return

                try {
                    isSubmittingComment.value = true

                    const commentData = {
                        postId: post.value.id,
                        text: newComment.text.trim()
                    }

                    const response = await commentService.createComment(commentData)

                    if (response.data) {
                        comments.value.unshift(response.data)
                        newComment.text = ''
                        toast.success('Yorumunuz başarıyla gönderildi!')
                    }
                } catch (err) {
                    toast.error('Yorum gönderilirken hata oluştu.')
                    console.error('Comment submit error:', err)
                } finally {
                    isSubmittingComment.value = false
                }
            }

            const deleteComment = async (commentId) => {
                if (!confirm('Bu yorumu silmek istediğinizden emin misiniz?')) return

                try {
                    await commentService.deleteComment(commentId)
                    comments.value = comments.value.filter(c => c.id !== commentId)
                    toast.success('Yorum silindi.')
                } catch (err) {
                    toast.error('Yorum silinirken hata oluştu.')
                    console.error('Comment delete error:', err)
                }
            }

            const canDeleteComment = (comment) => {
                return currentUser.value &&
                    (currentUser.value.id === comment.userId || currentUser.value.role === 'admin')
            }

            const subscribeNewsletter = async () => {
                if (!newsletterEmail.value) return

                try {
                    isSubscribing.value = true
                    await contentService.subscribeNewsletter(newsletterEmail.value)
                    toast.success('Bülten aboneliği başarılı!')
                    newsletterEmail.value = ''
                } catch (err) {
                    toast.error('Bülten aboneliği sırasında hata oluştu.')
                    console.error('Newsletter subscribe error:', err)
                } finally {
                    isSubscribing.value = false
                }
            }

            const formatDate = (date) => {
                return dayjs(date).format('DD MMMM YYYY')
            }

            const getCategoryName = (categoryId) => {
                const category = categories.value.find(c => c.id === categoryId)
                return category?.name || 'Genel'
            }

            const getShareUrl = (platform) => {
                const url = encodeURIComponent(window.location.href)
                const title = encodeURIComponent(post.value.title)

                const urls = {
                    facebook: `https://www.facebook.com/sharer/sharer.php?u=${url}`,
                    twitter: `https://twitter.com/intent/tweet?url=${url}&text=${title}`,
                    linkedin: `https://www.linkedin.com/sharing/share-offsite/?url=${url}`,
                    whatsapp: `https://wa.me/?text=${title} ${url}`
                }

                return urls[platform] || '#'
            }

            const copyToClipboard = async () => {
                try {
                    await navigator.clipboard.writeText(window.location.href)
                    toast.success('Link kopyalandı!')
                } catch (err) {
                    toast.error('Link kopyalanamadı.')
                }
            }

            const handleImageError = (event) => {
                if (event.target.src.includes('default-')) return

                if (event.target.classList.contains('author-avatar') ||
                    event.target.classList.contains('comment-avatar') ||
                    event.target.classList.contains('author-image')) {
                    event.target.src = '/default-avatar.png'
                } else {
                    event.target.src = '/default-blog.jpg'
                }
            }

            // Lifecycle
            onMounted(async () => {
                await loadPost()

                if (post.value) {
                    await Promise.all([
                        loadComments(),
                        loadRelatedPosts(),
                        loadPopularPosts(),
                        loadCategories()
                    ])
                }
            })

            // Watch for route changes
            watch(() => route.params.id, async (newId, oldId) => {
                if (newId !== oldId) {
                    await loadPost()
                    if (post.value) {
                        await Promise.all([
                            loadComments(),
                            loadRelatedPosts()
                        ])
                    }
                }
            })

            return {
                // Reactive data
                post,
                comments,
                relatedPosts,
                popularPosts,
                categories,
                loading,
                loadingRelated,
                loadingPopular,
                error,
                isSubmittingComment,
                isSubscribing,
                newsletterEmail,
                newComment,

                // Computed
                isAuthenticated,
                currentUser,
                estimatedReadingTime,

                // Methods
                submitComment,
                deleteComment,
                canDeleteComment,
                subscribeNewsletter,
                formatDate,
                getCategoryName,
                getShareUrl,
                copyToClipboard,
                handleImageError
            }
        }
    }
</script>

<style scoped>
    .blog-post {
        background: white;
        border-radius: 10px;
        padding: 2rem;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        margin-bottom: 2rem;
    }

    .post-header {
        border-bottom: 1px solid #eee;
        padding-bottom: 1.5rem;
    }

    .post-category .badge {
        font-size: 0.75rem;
        padding: 0.5rem 0.75rem;
        border-radius: 15px;
    }

    .post-title {
        font-size: 2.5rem;
        font-weight: 700;
        color: #333;
        line-height: 1.2;
    }

    .post-meta {
        font-size: 0.9rem;
    }

    .author-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        object-fit: cover;
    }

    .post-featured-image img {
        width: 100%;
        height: 400px;
        object-fit: cover;
    }

    .post-content {
        font-size: 1.1rem;
        line-height: 1.8;
        color: #444;
    }

        .post-content h1,
        .post-content h2,
        .post-content h3,
        .post-content h4,
        .post-content h5,
        .post-content h6 {
            margin-top: 2rem;
            margin-bottom: 1rem;
            color: #333;
        }

        .post-content h2 {
            font-size: 2rem;
            margin-top: 3rem;
        }

        .post-content h3 {
            font-size: 1.5rem;
            margin-top: 2.5rem;
        }

        .post-content h4 {
            font-size: 1.25rem;
            margin-top: 2rem;
        }

        .post-content p {
            margin-bottom: 1.5rem;
        }

        .post-content img {
            max-width: 100%;
            height: auto;
            border-radius: 8px;
            margin: 1.5rem 0;
        }

        .post-content blockquote {
            border-left: 4px solid #007bff;
            padding-left: 1.5rem;
            margin: 2rem 0;
            font-style: italic;
            color: #666;
        }

        .post-content ul,
        .post-content ol {
            margin-bottom: 1.5rem;
            padding-left: 2rem;
        }

        .post-content li {
            margin-bottom: 0.5rem;
            line-height: 1.6;
        }

        .post-content code {
            background: #f8f9fa;
            padding: 0.2rem 0.4rem;
            border-radius: 4px;
            font-size: 0.9rem;
        }

        .post-content pre {
            background: #f8f9fa;
            padding: 1.5rem;
            border-radius: 8px;
            overflow-x: auto;
            margin: 2rem 0;
        }

        .post-content table {
            width: 100%;
            border-collapse: collapse;
            margin: 2rem 0;
        }

        .post-content th,
        .post-content td {
            border: 1px solid #ddd;
            padding: 0.75rem;
            text-align: left;
        }

        .post-content th {
            background: #f8f9fa;
            font-weight: 600;
        }

    .post-tags .badge {
        font-size: 0.8rem;
        padding: 0.5rem 0.75rem;
        border: 1px solid #ddd;
        border-radius: 15px;
    }

    .social-share .share-buttons {
        display: flex;
        gap: 0.5rem;
        flex-wrap: wrap;
    }

    .share-buttons .btn {
        width: 45px;
        height: 45px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.3s ease;
    }

    .btn-facebook {
        background: #3b5998;
        border-color: #3b5998;
        color: white;
    }

    .btn-twitter {
        background: #1da1f2;
        border-color: #1da1f2;
        color: white;
    }

    .btn-linkedin {
        background: #0077b5;
        border-color: #0077b5;
        color: white;
    }

    .btn-whatsapp {
        background: #25d366;
        border-color: #25d366;
        color: white;
    }

    .share-buttons .btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    }

    .comments-section {
        background: #f8f9fa;
        border-radius: 10px;
        padding: 2rem;
    }

    .comment-form textarea {
        border-radius: 10px;
        border: 1px solid #ddd;
        resize: vertical;
    }

    .comment-item {
        background: white;
        border-radius: 10px;
        padding: 1.5rem;
        border: 1px solid #eee;
    }

    .comment-avatar {
        width: 50px;
        height: 50px;
        border-radius: 50%;
        object-fit: cover;
    }

    .comment-author {
        font-weight: 600;
        color: #333;
    }

    .comment-text {
        color: #444;
        line-height: 1.6;
    }

    .blog-sidebar {
        padding-left: 2rem;
    }

    .sidebar-widget {
        background: white;
        border-radius: 10px;
        padding: 1.5rem;
        margin-bottom: 2rem;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    }

        .sidebar-widget h5 {
            color: #333;
            font-weight: 600;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 2px solid #007bff;
        }

    .author-widget .author-card {
        text-align: center;
    }

    .author-image {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        object-fit: cover;
        margin-bottom: 1rem;
    }

    .author-name {
        color: #333;
        font-weight: 600;
        margin-bottom: 0.5rem;
    }

    .author-bio {
        font-size: 0.9rem;
        color: #666;
        line-height: 1.6;
    }

    .related-post,
    .popular-post {
        padding: 1rem 0;
        border-bottom: 1px solid #eee;
    }

        .related-post:last-child,
        .popular-post:last-child {
            border-bottom: none;
            padding-bottom: 0;
        }

        .related-post .post-title,
        .popular-post .post-title {
            font-size: 0.9rem;
            font-weight: 600;
            line-height: 1.4;
        }

            .related-post .post-title a,
            .popular-post .post-title a {
                color: #333;
                transition: color 0.3s ease;
            }

                .related-post .post-title a:hover,
                .popular-post .post-title a:hover {
                    color: #007bff;
                }

    .rank-number {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 30px;
        height: 30px;
        background: #007bff;
        color: white;
        border-radius: 50%;
        font-weight: 600;
        font-size: 0.8rem;
    }

    .categories-list {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .category-link {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0.75rem;
        background: #f8f9fa;
        border-radius: 8px;
        text-decoration: none;
        color: #333;
        transition: all 0.3s ease;
    }

        .category-link:hover {
            background: #007bff;
            color: white;
        }

            .category-link:hover .badge {
                background: white !important;
                color: #007bff !important;
            }

    .newsletter-widget {
        background: linear-gradient(135deg, #007bff, #0056b3);
        color: white;
    }

        .newsletter-widget h5 {
            color: white;
            border-color: rgba(255, 255, 255, 0.3);
        }

        .newsletter-widget .form-control {
            border: none;
            border-radius: 25px;
            padding: 0.75rem 1rem;
        }

        .newsletter-widget .btn-primary {
            background: white;
            border-color: white;
            color: #007bff;
            border-radius: 25px;
            font-weight: 600;
        }

            .newsletter-widget .btn-primary:hover {
                background: #f8f9fa;
                border-color: #f8f9fa;
            }

    /* Responsive Design */
    @media (max-width: 992px) {
        .blog-sidebar {
            padding-left: 0;
            margin-top: 2rem;
        }
    }

    @media (max-width: 768px) {
        .blog-post {
            padding: 1.5rem;
        }

        .post-title {
            font-size: 1.8rem;
        }

        .post-meta {
            gap: 0.5rem;
        }

        .post-featured-image img {
            height: 250px;
        }

        .post-content {
            font-size: 1rem;
        }

        .comments-section {
            padding: 1.5rem;
        }

        .comment-item {
            padding: 1rem;
        }

        .comment-avatar {
            width: 40px;
            height: 40px;
        }

        .sidebar-widget {
            padding: 1rem;
        }

        .share-buttons {
            justify-content: center;
        }
    }

    @media (max-width: 576px) {
        .post-meta {
            flex-direction: column !important;
            align-items: flex-start !important;
        }

        .share-buttons .btn {
            width: 40px;
            height: 40px;
        }
    }

    /* Loading animations */
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

    .blog-post,
    .sidebar-widget {
        animation: fadeInUp 0.6s ease-out;
    }

    /* Print styles */
    @media print {
        .breadcrumb-section,
        .blog-sidebar,
        .social-share,
        .comments-section {
            display: none !important;
        }

        .blog-post {
            box-shadow: none;
            padding: 0;
        }

        .post-content {
            font-size: 12pt;
            line-height: 1.5;
        }
    }

    /* Dark theme support */
    @media (prefers-color-scheme: dark) {
        .blog-post,
        .sidebar-widget,
        .comment-item {
            background: #2d3748;
            color: #e2e8f0;
        }

        .post-title {
            color: #e2e8f0;
        }

        .post-content {
            color: #cbd5e0;
        }

        .comments-section {
            background: #1a202c;
        }

        .category-link {
            background: #4a5568;
            color: #e2e8f0;
        }

            .category-link:hover {
                background: #007bff;
            }
    }

    /* Focus styles for accessibility */
    .comment-form textarea:focus,
    .newsletter-widget .form-control:focus,
    .share-buttons .btn:focus,
    .category-link:focus {
        outline: 2px solid #007bff;
        outline-offset: 2px;
    }

    /* High contrast mode */
    @media (prefers-contrast: high) {
        .category-link:hover,
        .related-post .post-title a:hover,
        .popular-post .post-title a:hover {
            text-decoration: underline;
        }
    }

    /* Reduced motion */
    @media (prefers-reduced-motion: reduce) {
        .blog-post,
        .sidebar-widget,
        .share-buttons .btn,
        .category-link {
            animation: none;
            transition: none;
        }

            .share-buttons .btn:hover {
                transform: none;
            }
    }

    /* Selection styles */
    ::selection {
        background: rgba(0, 123, 255, 0.2);
        color: #333;
    }

    /* Smooth scrolling */
    html {
        scroll-behavior: smooth;
    }
</style>