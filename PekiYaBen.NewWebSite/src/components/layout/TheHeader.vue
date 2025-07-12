<template>
    <header class="header">
        <nav class="navbar navbar-expand-lg navbar-light bg-white fixed-top shadow-sm">
            <div class="container">
                <!-- Brand -->
                <router-link to="/" class="navbar-brand">
                    <img src="/assets/img/logo.png" alt="PekiYaBen" class="logo" />
                </router-link>

                <!-- Mobile menu button -->
                <button class="navbar-toggler"
                        type="button"
                        @click="toggleMobileMenu"
                        aria-expanded="false"
                        aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>

                <!-- Navigation -->
                <div class="navbar-collapse" :class="{ show: mobileMenuOpen }">
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <router-link to="/" class="nav-link">Ana Sayfa</router-link>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle"
                               href="#"
                               role="button"
                               @click="toggleDropdown('services')">
                                Hizmetler
                            </a>
                            <ul class="dropdown-menu" :class="{ show: dropdowns.services }">
                                <li>
                                    <router-link to="/kisisel-durum-analizi" class="dropdown-item">
                                        Kişisel Durum Analizi
                                    </router-link>
                                </li>
                                <li>
                                    <router-link to="/kisisel-strateji-belirleme" class="dropdown-item">
                                        Kişisel Strateji Belirleme
                                    </router-link>
                                </li>
                                <li>
                                    <router-link to="/bilincalti-uygulamalari" class="dropdown-item">
                                        Bilinçaltı Uygulamaları
                                    </router-link>
                                </li>
                                <li>
                                    <router-link to="/kocluk-gorusmeleri" class="dropdown-item">
                                        Koçluk Görüşmeleri
                                    </router-link>
                                </li>
                            </ul>
                        </li>
                        <li class="nav-item">
                            <router-link to="/kocluk-ekibimiz" class="nav-link">Ekibimiz</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/online-kocluk-ve-atolye" class="nav-link">Online Koçluk</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/kisisel-gelisim/yazilar" class="nav-link">Blog</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link to="/sikca-sorulanlar" class="nav-link">S.S.S</router-link>
                        </li>
                    </ul>

                    <!-- User menu -->
                    <div class="navbar-nav">
                        <div v-if="isAuthenticated" class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle d-flex align-items-center"
                               href="#"
                               role="button"
                               @click="toggleDropdown('user')">
                                <img :src="userAvatar"
                                     :alt="userFullName"
                                     class="user-avatar me-2"
                                     @error="handleAvatarError" />
                                {{ userFullName }}
                            </a>
                            <ul class="dropdown-menu dropdown-menu-end" :class="{ show: dropdowns.user }">
                                <li>
                                    <router-link to="/hesabim/profilim" class="dropdown-item">
                                        <i class="fas fa-user me-2"></i>Profilim
                                    </router-link>
                                </li>
                                <li>
                                    <router-link to="/hesabim/randevularim" class="dropdown-item">
                                        <i class="fas fa-calendar-alt me-2"></i>Randevularım
                                    </router-link>
                                </li>
                                <li><hr class="dropdown-divider"></li>
                                <li>
                                    <a href="#"
                                       class="dropdown-item text-danger"
                                       @click.prevent="logout">
                                        <i class="fas fa-sign-out-alt me-2"></i>Çıkış Yap
                                    </a>
                                </li>
                            </ul>
                        </div>
                        <div v-else class="nav-item">
                            <router-link to="/hesabim/giris" class="btn btn-outline-primary me-2">
                                Giriş Yap
                            </router-link>
                            <router-link to="/hesabim/kayit" class="btn btn-primary">
                                Kayıt Ol
                            </router-link>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    </header>
</template>

<script>
    import { ref, computed } from 'vue'
    import { useAuthStore } from '@/stores/auth'
    import { useToast } from 'vue-toastification'

    export default {
        name: 'TheHeader',
        setup() {
            const authStore = useAuthStore()
            const toast = useToast()

            const mobileMenuOpen = ref(false)
            const dropdowns = ref({
                services: false,
                user: false
            })

            const isAuthenticated = computed(() => authStore.isAuthenticated)
            const userFullName = computed(() => authStore.userFullName)
            const userAvatar = computed(() => authStore.user?.avatar || '/default-avatar.png')

            const toggleMobileMenu = () => {
                mobileMenuOpen.value = !mobileMenuOpen.value
            }

            const toggleDropdown = (dropdown) => {
                dropdowns.value[dropdown] = !dropdowns.value[dropdown]

                // Close other dropdowns
                Object.keys(dropdowns.value).forEach(key => {
                    if (key !== dropdown) {
                        dropdowns.value[key] = false
                    }
                })
            }

            const logout = async () => {
                try {
                    await authStore.logout()
                    toast.success('Başarıyla çıkış yaptınız.')
                } catch (error) {
                    toast.error('Çıkış yaparken hata oluştu.')
                }
            }

            const handleAvatarError = (event) => {
                event.target.src = '/default-avatar.png'
            }

            return {
                mobileMenuOpen,
                dropdowns,
                isAuthenticated,
                userFullName,
                userAvatar,
                toggleMobileMenu,
                toggleDropdown,
                logout,
                handleAvatarError
            }
        }
    }
</script>

<style scoped>
    .header {
        position: relative;
        z-index: 1000;
    }

    .navbar {
        padding: 1rem 0;
        transition: all 0.3s ease;
    }

    .logo {
        height: 40px;
        width: auto;
    }

    .nav-link {
        font-weight: 500;
        color: #333 !important;
        transition: color 0.3s ease;
    }

        .nav-link:hover,
        .nav-link.router-link-active {
            color: #007bff !important;
        }

    .user-avatar {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        object-fit: cover;
    }

    .dropdown-menu {
        border: none;
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        border-radius: 8px;
    }

    .dropdown-item {
        padding: 0.75rem 1rem;
        transition: all 0.3s ease;
    }

        .dropdown-item:hover {
            background: #f8f9fa;
            color: #007bff;
        }

    @media (max-width: 991px) {
        .navbar-collapse {
            background: white;
            border-radius: 8px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
            margin-top: 1rem;
            padding: 1rem;
        }

        .dropdown-menu {
            position: static;
            box-shadow: none;
            border: 1px solid #eee;
            margin-top: 0.5rem;
        }
    }
</style>