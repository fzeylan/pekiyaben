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
                        <li class="mb-2 d-flex align-items-center">
                            <i class="fas fa-clock me-2 text-primary"></i>
                            <span class="text-muted">Pazartesi - Cuma: 09:00 - 18:00</span>
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
                    <form @submit.prevent="subscribeNewsletter" class="newsletter-form">
                        <div class="input-group">
                            <input v-model="email"
                                   type="email"
                                   class="form-control"
                                   placeholder="E-posta adresiniz"
                                   required
                                   :disabled="isSubscribing" />
                            <button type="submit"
                                    class="btn btn-primary"
                                    :disabled="isSubscribing || !email">
                                <span v-if="isSubscribing" class="spinner-border spinner-border-sm me-1"></span>
                                {{ isSubscribing ? 'Gönderiliyor...' : 'Abone Ol' }}
                            </button>
                        </div>
                    </form>
                </div>
                <div class="col-lg-4 text-lg-end mt-3 mt-lg-0">
                    <!--<div class="footer-badges">
                        <img src="/ssl-badge.png" alt="SSL Sertifikalı" class="badge-img me-2" @error="handleImageError" />
                        <img src="/secure-badge.png" alt="Güvenli Ödeme" class="badge-img" @error="handleImageError" />
                    </div>
                    <div class="app-links mt-3">
                        <a href="#" class="me-2" title="App Store'dan İndir">
                            <img src="/app-store-badge.png" alt="App Store" class="app-badge" @error="handleImageError" />
                        </a>
                        <a href="#" title="Google Play'den İndir">
                            <img src="/google-play-badge.png" alt="Google Play" class="app-badge" @error="handleImageError" />
                        </a>
                    </div>-->
                </div>
            </div>

            <!-- Additional Links -->
            <div class="row border-top border-secondary pt-4 mt-4">
                <div class="col-md-6">
                    <h6 class="text-white mb-3">Yararlı Bağlantılar</h6>
                    <div class="row">
                        <div class="col-sm-6">
                            <ul class="list-unstyled">
                                <li class="mb-1">
                                    <router-link to="/hakkimizda" class="text-muted footer-link small">Hakkımızda</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/iletisim" class="text-muted footer-link small">İletişim</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/kariyer" class="text-muted footer-link small">Kariyer</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/medya" class="text-muted footer-link small">Basında Biz</router-link>
                                </li>
                            </ul>
                        </div>
                        <div class="col-sm-6">
                            <ul class="list-unstyled">
                                <li class="mb-1">
                                    <router-link to="/yardim" class="text-muted footer-link small">Yardım Merkezi</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/akademi" class="text-muted footer-link small">PekiYaBen Akademi</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/etkinlikler" class="text-muted footer-link small">Etkinlikler</router-link>
                                </li>
                                <li class="mb-1">
                                    <router-link to="/referans" class="text-muted footer-link small">Referanslarımız</router-link>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <h6 class="text-white mb-3">Sertifikalar & Üyelikler</h6>
                    <div class="certifications">
                        <!--<img src="/certification-1.png" alt="Sertifika 1" class="cert-img me-2 mb-2" @error="handleImageError" />
                        <img src="/certification-2.png" alt="Sertifika 2" class="cert-img me-2 mb-2" @error="handleImageError" />
                        <img src="/certification-3.png" alt="Sertifika 3" class="cert-img me-2 mb-2" @error="handleImageError" />-->
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
                            <router-link to="/gizlilik-politikasi" class="text-muted small me-3 footer-link">
                                Gizlilik Politikası
                            </router-link>
                            <router-link to="/kullanim-sartlari" class="text-muted small me-3 footer-link">
                                Kullanım Şartları
                            </router-link>
                            <router-link to="/kvkk" class="text-muted small footer-link">
                                KVKK
                            </router-link>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Back to Top Button -->
        <button v-show="showBackToTop"
                @click="scrollToTop"
                class="back-to-top-btn"
                title="Yukarı Çık">
            <i class="fas fa-chevron-up"></i>
        </button>
    </footer>
</template>

<script>
    import { ref, computed, onMounted, onUnmounted } from 'vue'
    import { useToast } from 'vue-toastification'
    import contentService from '@/services/content'

    export default {
        name: 'TheFooter',
        setup() {
            const toast = useToast()

            const email = ref('')
            const isSubscribing = ref(false)
            const showBackToTop = ref(false)

            const currentYear = computed(() => new Date().getFullYear())

            const subscribeNewsletter = async () => {
                if (!email.value.trim()) {
                    toast.warning('Lütfen geçerli bir e-posta adresi girin.')
                    return
                }

                isSubscribing.value = true

                try {
                    await contentService.subscribeNewsletter(email.value.trim())
                    toast.success('Bülten aboneliği başarılı! Teşekkür ederiz.')
                    email.value = ''
                } catch (error) {
                    toast.error('Abonelik sırasında bir hata oluştu. Lütfen tekrar deneyin.')
                    console.error('Newsletter subscription error:', error)
                } finally {
                    isSubscribing.value = false
                }
            }

            const scrollToTop = () => {
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth'
                })
            }

            const handleScroll = () => {
                showBackToTop.value = window.pageYOffset > 300
            }

            const handleImageError = (event) => {
                // Hide broken images gracefully
                event.target.style.display = 'none'
            }

            onMounted(() => {
                window.addEventListener('scroll', handleScroll)
            })

            onUnmounted(() => {
                window.removeEventListener('scroll', handleScroll)
            })

            return {
                email,
                isSubscribing,
                showBackToTop,
                currentYear,
                subscribeNewsletter,
                scrollToTop,
                handleImageError
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
        text-decoration: none;
    }

        .social-links a:hover {
            background-color: #007bff;
            color: white !important;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
        }

    .footer-link {
        text-decoration: none;
        transition: color 0.3s ease;
    }

        .footer-link:hover {
            color: #007bff !important;
        }

    .badge-img,
    .app-badge,
    .cert-img {
        height: 40px;
        width: auto;
        opacity: 0.8;
        transition: opacity 0.3s ease;
        border-radius: 4px;
    }

        .badge-img:hover,
        .app-badge:hover,
        .cert-img:hover {
            opacity: 1;
            transform: scale(1.05);
        }

    .app-badge {
        height: 45px;
    }

    .cert-img {
        height: 35px;
    }

    .newsletter-form .form-control {
        border: none;
        border-radius: 25px 0 0 25px;
        padding: 12px 20px;
        background: rgba(255, 255, 255, 0.95);
    }

        .newsletter-form .form-control:focus {
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
            border-color: #007bff;
            background: white;
        }

    .newsletter-form .btn-primary {
        border-radius: 0 25px 25px 0;
        padding: 12px 25px;
        font-weight: 500;
        transition: all 0.3s ease;
        border: none;
    }

        .newsletter-form .btn-primary:hover {
            transform: translateY(-1px);
            box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
        }

    .back-to-top-btn {
        position: fixed;
        bottom: 30px;
        right: 30px;
        width: 50px;
        height: 50px;
        background: #007bff;
        color: white;
        border: none;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        transition: all 0.3s ease;
        z-index: 1000;
        cursor: pointer;
    }

        .back-to-top-btn:hover {
            background: #0056b3;
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.2);
        }

    .certifications {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
    }

    .input-group {
        max-width: 400px;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .social-links {
            text-align: center;
            margin-top: 1rem;
        }

        .footer-legal-links {
            text-align: center;
            margin-top: 1rem;
        }

        .footer-badges,
        .app-links,
        .certifications {
            text-align: center;
        }

        .newsletter-form .form-control {
            border-radius: 25px;
            margin-bottom: 10px;
        }

        .newsletter-form .btn-primary {
            border-radius: 25px;
            width: 100%;
        }

        .back-to-top-btn {
            bottom: 20px;
            right: 20px;
            width: 45px;
            height: 45px;
        }

        .input-group {
            flex-direction: column;
        }

            .input-group .btn {
                border-radius: 25px !important;
            }
    }

    @media (max-width: 576px) {
        .footer-legal-links a {
            display: block;
            margin: 0.25rem 0 !important;
        }
    }

    /* Dark Theme Support */
    .bg-dark {
        background-color: #1a1a1a !important;
    }

    .bg-secondary {
        background-color: #2d3748 !important;
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

    .footer {
        animation: fadeInUp 0.6s ease-out;
    }

    /* Loading animation for newsletter button */
    .spinner-border-sm {
        width: 1rem;
        height: 1rem;
    }

    /* Custom scrollbar for footer content */
    .footer::-webkit-scrollbar {
        width: 6px;
    }

    .footer::-webkit-scrollbar-track {
        background: #2d3748;
    }

    .footer::-webkit-scrollbar-thumb {
        background: #007bff;
        border-radius: 3px;
    }

        .footer::-webkit-scrollbar-thumb:hover {
            background: #0056b3;
        }

    /* Print styles */
    @media print {
        .footer {
            display: none !important;
        }
    }

    /* High contrast mode support */
    @media (prefers-contrast: high) {
        .footer-link:hover {
            text-decoration: underline;
        }

        .social-links a:hover {
            outline: 2px solid white;
        }
    }

    /* Reduced motion support */
    @media (prefers-reduced-motion: reduce) {
        .social-links a,
        .footer-link,
        .badge-img,
        .app-badge,
        .cert-img,
        .newsletter-form .btn-primary,
        .back-to-top-btn {
            transition: none;
        }

            .back-to-top-btn:hover {
                transform: none;
            }

        .footer {
            animation: none;
        }
    }
</style>