<template>
    <div class="homepage">
        <!-- Hero Section -->
        <section class="hero-section text-white min-vh-75 d-flex align-items-center">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-lg-6">
                        <h1 class="display-4 fw-bold mb-4">
                            {{ heroContent.title || 'PekiYaBen ile Hayatını Değiştir' }}
                        </h1>
                        <p class="lead mb-4">
                            {{ heroContent.subtitle || 'Uzman koçlarımızla birebir görüşmeler yapın ve kişisel gelişiminizi hızlandırın.' }}
                        </p>
                        <div class="d-flex gap-3 flex-wrap">
                            <router-link to="/kocluk-ekibimiz" class="btn btn-light btn-lg">
                                Koçlarımızı Keşfedin
                            </router-link>
                            <router-link to="/online-kocluk-ve-atolye" class="btn btn-outline-light btn-lg">
                                Hizmetlerimiz
                            </router-link>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="hero-image">
                            <img src="/assets/img/hero-img.jpg" alt="Coaching" class="img-fluid">
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Statistics Section -->
        <section v-if="statistics" class="statistics-section py-5 bg-light">
            <div class="container">
                <div class="row text-center">
                    <div class="col-lg-3 col-md-6 mb-4">
                        <div class="stat-item">
                            <h3 class="stat-number">{{ statistics.coachingHours }}</h3>
                            <p class="stat-label">Koçluk Saati</p>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 mb-4">
                        <div class="stat-item">
                            <h3 class="stat-number">{{ statistics.users }}</h3>
                            <p class="stat-label">Mutlu Kullanıcı</p>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 mb-4">
                        <div class="stat-item">
                            <h3 class="stat-number">{{ statistics.workshopHours }}</h3>
                            <p class="stat-label">Atölye Saati</p>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-6 mb-4">
                        <div class="stat-item">
                            <h3 class="stat-number">{{ statistics.countries }}</h3>
                            <p class="stat-label">Ülke</p>
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
                            Size en uygun koçluk hizmetini seçin
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div v-for="product in featuredProducts"
                         :key="product.id"
                         class="col-lg-3 col-md-6">
                        <div class="service-card h-100">
                            <div class="service-icon mb-3">
                                <img v-if="product.image"
                                     :src="product.image"
                                     :alt="product.title"
                                     class="img-fluid">
                                <i v-else class="fas fa-user-tie fa-2x text-primary"></i>
                            </div>
                            <h5>{{ product.title }}</h5>
                            <p>{{ product.description }}</p>
                            <div class="mt-auto">
                                <p class="price fw-bold text-primary">{{ product.price }}₺</p>
                                <router-link :to="`/online-kocluk-ve-atolye/${product.id}/${product.slug}`"
                                             class="btn btn-primary">
                                    Detayları Görüntüle
                                </router-link>
                            </div>
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
                        <div class="coach-card h-100">
                            <div class="coach-image">
                                <img :src="coach.Image || '/assets/img/default-avatar.jpg'"
                                     :alt="coach.Name"
                                     class="img-fluid">
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
                        <h2 class="section-title">Son Yazılar</h2>
                        <p class="section-subtitle text-muted">
                            Kişisel gelişim yolculuğunuzda size rehberlik edecek içerikler
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div v-for="post in latestContent"
                         :key="post.id"
                         class="col-lg-4 col-md-6">
                        <article class="blog-card h-100">
                            <div class="blog-image">
                                <img :src="post.image || '/images/default-blog.jpg'"
                                     :alt="post.title"
                                     class="img-fluid">
                            </div>
                            <div class="blog-content">
                                <div class="blog-meta mb-2">
                                    <span class="date">{{ formatDate(post.createDate) }}</span>
                                    <span v-if="post.coach" class="author">{{ post.coach.fullName }}</span>
                                </div>
                                <h5>{{ post.title }}</h5>
                                <p>{{ post.summary }}</p>
                                <router-link :to="`/kisisel-gelisim/yazilar/${post.id}/${post.slug}`"
                                             class="btn btn-outline-primary">
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

        <!-- Testimonials Section -->
        <section v-if="testimonials.length" class="testimonials-section py-5 bg-light">
            <div class="container">
                <div class="row">
                    <div class="col-12 text-center mb-5">
                        <h2 class="section-title">Müşteri Yorumları</h2>
                        <p class="section-subtitle text-muted">
                            Bizimle çalışan müşterilerimizin deneyimleri
                        </p>
                    </div>
                </div>
                <div class="row g-4">
                    <div v-for="testimonial in testimonials.slice(0, 3)"
                         :key="testimonial.id"
                         class="col-lg-4 col-md-6">
                        <div class="testimonial-card h-100">
                            <div class="stars mb-3">
                                <i v-for="star in 5"
                                   :key="star"
                                   class="fas fa-star"
                                   :class="star <= testimonial.stars ? 'text-warning' : 'text-muted'"></i>
                            </div>
                            <p class="testimonial-text">"{{ testimonial.comment }}"</p>
                            <div class="testimonial-author mt-auto">
                                <h6>{{ testimonial.fullName }}</h6>
                                <small class="text-muted">{{ testimonial.title }}</small>
                            </div>
                        </div>
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
                <p class="mt-3">İçerik yükleniyor...</p>
            </div>
        </div>
    </div>
</template>

<script>
    import { ref, onMounted } from 'vue'
    import api from '@/services/api'
    import dayjs from 'dayjs'

    export default {
        name: 'HomePage',
        setup() {
            const featuredCoaches = ref([])
            const featuredProducts = ref([])
            const latestContent = ref([])
            const statistics = ref(null)
            const heroContent = ref({})
            const testimonials = ref([])
            const loading = ref(false)

            const loadHomeData = async () => {
                try {
                    loading.value = true

                    await loadFallbackData();

                    //// Load testimonials separately
                    //const testimonialsResponse = await api.get('/api/homepage/testimonials')
                    //testimonials.value = testimonialsResponse.data || []

                } catch (error) {
                    console.error('Home data load error:', error)

                    // Fallback: try to load each section individually
                    await loadFallbackData()
                } finally {
                    loading.value = false
                }
            }

            const loadFallbackData = async () => {
                try {
                    // Load coaches
                    const coachesResponse = await api.get('/coaches')
                    if (coachesResponse.data.Result)
                        featuredCoaches.value = coachesResponse.data.Data || []

                    //// Load products
                    //const productsResponse = await api.get('/api/homepage/products')
                    //featuredProducts.value = productsResponse.data || []

                    //// Load latest content
                    //const contentResponse = await api.get('/api/homepage/latest-content')
                    //latestContent.value = contentResponse.data || []

                    //// Load statistics
                    //const statsResponse = await api.get('/api/homepage/statistics')
                    //statistics.value = statsResponse.data || null

                    //// Load hero content
                    //const heroResponse = await api.get('/api/homepage/hero-content')
                    //heroContent.value = heroResponse.data || {}

                } catch (error) {
                    console.error('Fallback data load error:', error)
                }
            }

            const formatDate = (date) => {
                if (!date) return ''
                return dayjs(date).format('DD MMMM YYYY')
            }

            onMounted(() => {
                loadHomeData()
            })

            return {
                featuredCoaches,
                featuredProducts,
                latestContent,
                statistics,
                heroContent,
                testimonials,
                loading,
                formatDate
            }
        }
    }
</script>

<style scoped>
    /* Hero Section */
    .hero-section {
        background: linear-gradient(135deg, #007bff, #0056b3);
        min-height: 80vh;
        display: flex;
        align-items: center;
    }

    .hero-image img {
        max-width: 100%;
        height: auto;
    }

    /* Statistics Section */
    .statistics-section {
        background: #f8f9fa;
    }

    .stat-item {
        padding: 2rem 1rem;
    }

    .stat-number {
        font-size: 3rem;
        font-weight: 700;
        color: #007bff;
        margin-bottom: 0.5rem;
    }

    .stat-label {
        color: #666;
        font-size: 1.1rem;
        margin: 0;
    }

    /* Common Card Styles */
    .service-card,
    .coach-card,
    .blog-card,
    .testimonial-card {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        text-align: center;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
        display: flex;
        flex-direction: column;
    }

        .service-card:hover,
        .coach-card:hover,
        .blog-card:hover,
        .testimonial-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.15);
        }

    /* Service Cards */
    .service-icon img {
        max-width: 80px;
        height: auto;
    }

    .price {
        font-size: 1.5rem;
        margin: 1rem 0;
    }

    /* Coach Cards */
    .coach-image {
        margin-bottom: 1.5rem;
    }

        .coach-image img {
            width: 120px;
            height: 120px;
            border-radius: 50%;
            object-fit: cover;
            border: 4px solid #007bff;
        }

    .coach-title {
        font-size: 1rem;
        margin-bottom: 1rem;
    }

    .coach-description {
        font-size: 0.9rem;
        color: #666;
        margin-bottom: 1.5rem;
        flex: 1;
    }

    /* Blog Cards */
    .blog-card {
        text-align: left;
        padding: 0;
        overflow: hidden;
    }

    .blog-image {
        height: 200px;
        overflow: hidden;
    }

        .blog-image img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

    .blog-content {
        padding: 1.5rem;
        flex: 1;
        display: flex;
        flex-direction: column;
    }

    .blog-meta {
        font-size: 0.85rem;
        color: #666;
    }

        .blog-meta .date::after {
            content: " • ";
            color: #ccc;
        }

    .blog-card h5 {
        color: #333;
        margin-bottom: 1rem;
        line-height: 1.4;
    }

    .blog-card p {
        color: #666;
        flex: 1;
        margin-bottom: 1.5rem;
    }

    /* Testimonial Cards */
    .testimonial-card {
        text-align: left;
    }

    .stars {
        font-size: 1.1rem;
    }

    .testimonial-text {
        font-style: italic;
        color: #555;
        flex: 1;
    }

    .testimonial-author h6 {
        color: #333;
        margin-bottom: 0.25rem;
    }

    /* Section Titles */
    .section-title {
        font-size: 2.5rem;
        font-weight: 700;
        color: #333;
        margin-bottom: 1rem;
    }

    .section-subtitle {
        font-size: 1.2rem;
        line-height: 1.6;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .hero-section {
            min-height: 60vh;
            text-align: center;
        }

            .hero-section .display-4 {
                font-size: 2rem;
            }

        .stat-number {
            font-size: 2rem;
        }

        .section-title {
            font-size: 2rem;
        }

        .service-card,
        .coach-card {
            margin-bottom: 2rem;
        }
    }

    @media (max-width: 576px) {
        .d-flex.gap-3 {
            flex-direction: column;
        }

        .btn-lg {
            width: 100%;
            margin-bottom: 1rem;
        }
    }
</style>