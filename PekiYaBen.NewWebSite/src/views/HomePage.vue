<!-- src/views/HomePage.vue -->
<template>
    <div class="home-page">
        <!-- Hero Section -->
        <section class="hero-section bg-primary text-white">
            <div class="container">
                <div class="row align-items-center min-vh-75">
                    <div class="col-lg-6">
                        <h1 class="display-4 fw-bold mb-4">
                            Kişisel Gelişiminizi Bir Üst Seviyeye Taşıyın
                        </h1>
                        <p class="lead mb-4">
                            Uzman koçlarımızla birlikte hedeflerinize ulaşın, potansiyelinizi keşfedin ve hayallerinizi gerçeğe dönüştürün.
                        </p>
                        <div class="hero-buttons">
                            <router-link to="/kocluk-ekibimiz" class="btn btn-light btn-lg me-3">
                                Koçlarımızı Keşfedin
                            </router-link>
                            <router-link to="/online-kocluk-ve-atolye" class="btn btn-outline-light btn-lg">
                                Hemen Başlayın
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="hero-image">
                            <img src="/hero-image.jpg" alt="Kişisel Gelişim" class="img-fluid rounded" />
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Services Section -->
        <section class="services-section py-5">
            <div class="container">
                <div class="row">
                    <div class="col-12 text-center mb-5">
                        <h2 class="section-title">Hizmetlerimiz</h2>
                        <p class="section-subtitle text-muted">
                            Size en uygun koçluk hizmetini bulun
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div class="col-lg-3 col-md-6">
                        <div class="service-card">
                            <div class="service-icon">
                                <i class="fas fa-user-check"></i>
                            </div>
                            <h5>Kişisel Durum Analizi</h5>
                            <p>Mevcut durumunuzu analiz ederek güçlü ve gelişim alanlarınızı belirleyin.</p>
                            <router-link to="/kisisel-durum-analizi" class="btn btn-outline-primary">
                                Detaylar
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6">
                        <div class="service-card">
                            <div class="service-icon">
                                <i class="fas fa-bullseye"></i>
                            </div>
                            <h5>Kişisel Strateji Belirleme</h5>
                            <p>Hedeflerinize ulaşmak için kişiselleştirilmiş stratejiler geliştirin.</p>
                            <router-link to="/kisisel-strateji-belirleme" class="btn btn-outline-primary">
                                Detaylar
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6">
                        <div class="service-card">
                            <div class="service-icon">
                                <i class="fas fa-brain"></i>
                            </div>
                            <h5>Bilinçaltı Uygulamaları</h5>
                            <p>Bilinçaltınızın gücünü keşfederek içsel dönüşümünüzü hızlandırın.</p>
                            <router-link to="/bilincalti-uygulamalari" class="btn btn-outline-primary">
                                Detaylar
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6">
                        <div class="service-card">
                            <div class="service-icon">
                                <i class="fas fa-comments"></i>
                            </div>
                            <h5>Koçluk Görüşmeleri</h5>
                            <p>Bire bir koçluk seanslarıyla kişisel gelişim yolculuğunuzu sürdürün.</p>
                            <router-link to="/kocluk-gorusmeleri" class="btn btn-outline-primary">
                                Detaylar
                            </router-link>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Coaches Section -->
        <section class="coaches-section py-5 bg-light">
            <div class="container">
                <div class="row">
                    <div class="col-12 text-center mb-5">
                        <h2 class="section-title">Uzman Koçluk Ekibimiz</h2>
                        <p class="section-subtitle text-muted">
                            Alanında uzman koçlarımızla tanışın
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div v-for="coach in featuredCoaches"
                         :key="coach.id"
                         class="col-lg-4 col-md-6">
                        <div class="coach-card">
                            <div class="coach-image">
                                <img :src="coach.image" :alt="coach.name" class="img-fluid" />
                            </div>
                            <div class="coach-info">
                                <h5>{{ coach.name }}</h5>
                                <p class="coach-title text-primary">{{ coach.title }}</p>
                                <p class="coach-description">{{ coach.description }}</p>
                                <router-link :to="`/kocluk-ekibimiz/${coach.categoryId}/category/${coach.id}/${coach.slug}`"
                                             class="btn btn-primary">
                                    Profili Görüntüle
                                </router-link>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-4">
                    <div class="col-12 text-center">
                        <router-link to="/kocluk-ekibimiz" class="btn btn-outline-primary btn-lg">
                            Tüm Koçları Görüntüle
                        </router-link>
                    </div>
                </div>
            </div>
        </section>

        <!-- Blog Section -->
        <section class="blog-section py-5">
            <div class="container">
                <div class="row">
                    <div class="col-12 text-center mb-5">
                        <h2 class="section-title">Son Yazılarımız</h2>
                        <p class="section-subtitle text-muted">
                            Kişisel gelişim yolculuğunuzda size rehberlik edecek içerikler
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div v-for="post in latestPosts"
                         :key="post.id"
                         class="col-lg-4 col-md-6">
                        <article class="blog-card">
                            <div class="blog-image">
                                <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`">
                                    <img :src="post.image" :alt="post.title" class="img-fluid" />
                                </router-link>
                            </div>
                            <div class="blog-content">
                                <div class="blog-meta">
                                    <span class="blog-date">{{ formatDate(post.date) }}</span>
                                    <span class="blog-category">{{ post.category }}</span>
                                </div>
                                <h5 class="blog-title">
                                    <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`">
                                        {{ post.title }}
                                    </router-link>
                                </h5>
                                <p class="blog-excerpt">{{ post.excerpt }}</p>
                                <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`"
                                             class="btn btn-outline-primary btn-sm">
                                    Devamını Oku
                                </router-link>
                            </div>
                        </article>
                    </div>
                </div>
                <div class="row mt-4">
                    <div class="col-12 text-center">
                        <router-link to="/kisisel-gelisim/yazilar" class="btn btn-outline-primary btn-lg">
                            Tüm Yazıları Görüntüle
                        </router-link>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import contentService from '@/services/content'
import dayjs from 'dayjs'

export default {
  name: 'HomePage',
  setup() {
    const featuredCoaches = ref([])
    const latestPosts = ref([])
    const loading = ref(false)

    const loadHomeData = async () => {
      try {
        loading.value = true

        // Load featured coaches
        const coachesResponse = await contentService.getCoaches()
        featuredCoaches.value = (coachesResponse.data || []).slice(0, 3)

        // Load latest blog posts
        const postsResponse = await contentService.getBlogPosts({ limit: 3 })
        latestPosts.value = postsResponse.data || []

      } catch (error) {
        console.error('Home data load error:', error)
      } finally {
        loading.value = false
      }
    }

    const formatDate = (date) => {
      return dayjs(date).format('DD MMMM YYYY')
    }

    onMounted(() => {
      loadHomeData()
    })

    return {
      featuredCoaches,
      latestPosts,
      loading,
      formatDate
    }
  }
}
</script>

<style scoped>
    .hero-section {
        background: linear-gradient(135deg, #007bff, #0056b3);
        min-height: 80vh;
        display: flex;
        align-items: center;
    }

    .min-vh-75 {
        min-height: 75vh;
    }

    .service-card,
    .coach-card,
    .blog-card {
        background: white;
        border-radius: 10px;
        padding: 2rem;
        text-align: center;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s ease, box-shadow 0.3s ease;
        height: 100%;
    }

        .service-card:hover,
        .coach-card:hover,
        .blog-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
        }

    .service-icon {
        font-size: 3rem;
        color: #007bff;
        margin-bottom: 1rem;
    }

    .coach-image,
    .blog-image {
        margin-bottom: 1.5rem;
    }

        .coach-image img,
        .blog-image img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 8px;
        }

    .section-title {
        font-size: 2.5rem;
        font-weight: 700;
        color: #333;
        margin-bottom: 1rem;
    }

    .section-subtitle {
        font-size: 1.1rem;
        max-width: 600px;
        margin: 0 auto;
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

    .blog-title a {
        color: #333;
        text-decoration: none;
        transition: color 0.3s ease;
    }

        .blog-title a:hover {
            color: #007bff;
        }

    .blog-excerpt {
        color: #666;
        line-height: 1.6;
        margin-bottom: 1rem;
    }

    /* Responsive */
    @media (max-width: 768px) {
        .hero-section {
            min-height: 60vh;
            text-align: center;
        }

        .hero-buttons {
            flex-direction: column;
            gap: 1rem;
        }

            .hero-buttons .btn {
                margin: 0 !important;
            }

        .section-title {
            font-size: 2rem;
        }

        .service-card,
        .coach-card,
        .blog-card {
            padding: 1.5rem;
        }
    }
</style>