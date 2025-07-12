<template>
  <header class="header fixed-top">
    <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm">
      <div class="container">
        <!-- Logo -->
        <router-link to="/" class="navbar-brand">
          <img src="/logo.png" alt="PekiYaBen" class="logo" />
        </router-link>

        <!-- Mobile menu button -->
        <button
          class="navbar-toggler"
          type="button"
          @click="toggleMobileMenu"
          :class="{ active: mobileMenuOpen }"
        >
          <span class="navbar-toggler-icon"></span>
        </button>

        <!-- Navigation menu -->
        <div class="navbar-collapse" :class="{ show: mobileMenuOpen }">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <router-link to="/" class="nav-link" @click="closeMobileMenu">
                Ana Sayfa
              </router-link>
            </li>
            <li class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                @click.prevent="toggleDropdown('services')"
                :class="{ show: dropdowns.services }"
              >
                Hizmetlerimiz
              </a>
              <ul class="dropdown-menu" :class="{ show: dropdowns.services }">
                <li>
                  <router-link
                    to="/kisisel-durum-analizi"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    Kişisel Durum Analizi
                  </router-link>
                </li>
                <li>
                  <router-link
                    to="/kisisel-strateji-belirleme"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    Kişisel Strateji Belirleme
                  </router-link>
                </li>
                <li>
                  <router-link
                    to="/bilincalti-uygulamalari"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    Bilinçaltı Uygulamaları
                  </router-link>
                </li>
                <li>
                  <router-link
                    to="/kocluk-gorusmeleri"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    Koçluk Görüşmeleri
                  </router-link>
                </li>
              </ul>
            </li>
            <li class="nav-item">
              <router-link
                to="/online-kocluk-ve-atolye"
                class="nav-link"
                @click="closeMobileMenu"
              >
                Online Koçluk
              </router-link>
            </li>
            <li class="nav-item">
              <router-link
                to="/kocluk-ekibimiz"
                class="nav-link"
                @click="closeMobileMenu"
              >
                Ekibimiz
              </router-link>
            </li>
            <li class="nav-item">
              <router-link
                to="/kisisel-gelisim/yazilar"
                class="nav-link"
                @click="closeMobileMenu"
              >
                Blog
              </router-link>
            </li>
            <li class="nav-item">
              <router-link
                to="/sikca-sorulanlar"
                class="nav-link"
                @click="closeMobileMenu"
              >
                S.S.S
              </router-link>
            </li>
          </ul>

          <!-- User menu -->
          <ul class="navbar-nav ms-auto">
            <li v-if="!isAuthenticated" class="nav-item">
              <router-link
                to="/hesabim/giris"
                class="nav-link"
                @click="closeMobileMenu"
              >
                <i class="fas fa-sign-in-alt me-1"></i>
                Giriş Yap
              </router-link>
            </li>
            <li v-if="!isAuthenticated" class="nav-item">
              <router-link
                to="/hesabim/kayit"
                class="nav-link btn btn-primary text-white px-3 ms-2"
                @click="closeMobileMenu"
              >
                Kayıt Ol
              </router-link>
            </li>
            <li v-if="isAuthenticated" class="nav-item dropdown">
              <a
                class="nav-link dropdown-toggle"
                href="#"
                @click.prevent="toggleDropdown('user')"
                :class="{ show: dropdowns.user }"
              >
                <i class="fas fa-user me-1"></i>
                {{ userFullName }}
              </a>
              <ul class="dropdown-menu" :class="{ show: dropdowns.user }">
                <li>
                  <router-link
                    to="/hesabim/profilim"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    <i class="fas fa-user me-2"></i>
                    Profilim
                  </router-link>
                </li>
                <li>
                  <router-link
                    to="/hesabim/randevularim"
                    class="dropdown-item"
                    @click="closeMobileMenu"
                  >
                    <i class="fas fa-calendar me-2"></i>
                    Randevularım
                  </router-link>
                </li>
                <li><hr class="dropdown-divider" /></li>
                <li>
                  <a
                    href="#"
                    class="dropdown-item"
                    @click.prevent="handleLogout"
                  >
                    <i class="fas fa-sign-out-alt me-2"></i>
                    Çıkış Yap
                  </a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>

<script>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'TheHeader',
  setup() {
    const authStore = useAuthStore()
    
    const mobileMenuOpen = ref(false)
    const dropdowns = ref({
      services: false,
      user: false
    })
    
    const isAuthenticated = computed(() => authStore.isAuthenticated)
    const userFullName = computed(() => authStore.userFullName)
    
    const toggleMobileMenu = () => {
      mobileMenuOpen.value = !mobileMenuOpen.value
      // Close all dropdowns when toggling mobile menu
      Object.keys(dropdowns.value).forEach(key => {
        dropdowns.value[key] = false
      })
    }
    
    const closeMobileMenu = () => {
      mobileMenuOpen.value = false
      Object.keys(dropdowns.value).forEach(key => {
        dropdowns.value[key] = false
      })
    }
    
    const toggleDropdown = (dropdownName) => {
      // Close other dropdowns
      Object.keys(dropdowns.value).forEach(key => {
        if (key !== dropdownName) {
          dropdowns.value[key] = false
        }
      })
      // Toggle current dropdown
      dropdowns.value[dropdownName] = !dropdowns.value[dropdownName]
    }
    
    const handleLogout = () => {
      authStore.logout()
      closeMobileMenu()
    }
    
    // Close dropdowns when clicking outside
    const handleClickOutside = (event) => {
      if (!event.target.closest('.dropdown')) {
        Object.keys(dropdowns.value).forEach(key => {
          dropdowns.value[key] = false
        })
      }
    }
    
    onMounted(() => {
      document.addEventListener('click', handleClickOutside)
    })
    
    onUnmounted(() => {
      document.removeEventListener('click', handleClickOutside)
    })
    
    return {
      mobileMenuOpen,
      dropdowns,
      isAuthenticated,
      userFullName,
      toggleMobileMenu,
      closeMobileMenu,
      toggleDropdown,
      handleLogout
    }
  }
}
</script>

<style scoped>
.header {
  z-index: 1000;
}

.logo {
  height: 40px;
  width: auto;
}

.navbar-toggler {
  border: none;
  padding: 4px 8px;
}

.navbar-toggler:focus {
  box-shadow: none;
}

.dropdown-menu {
  border: none;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  border-radius: 0.375rem;
}

.dropdown-item {
  padding: 0.5rem 1rem;
  transition: all 0.2s ease;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
  color: #007bff;
}

.nav-link {
  font-weight: 500;
  transition: color 0.2s ease;
}

.nav-link:hover {
  color: #007bff !important;
}

.nav-link.router-link-active {
  color: #007bff !important;
  font-weight: 600;
}

/* Mobile menu styles */
@media (max-width: 991.98px) {
  .navbar-collapse {
    background: white;
    border-top: 1px solid #dee2e6;
    margin-top: 1rem;
    padding-top: 1rem;
  }
  
  .dropdown-menu {
    position: static;
    float: none;
    width: 100%;
    margin-top: 0;
    background-color: #f8f9fa;
    border: none;
    box-shadow: none;
  }
  
  .dropdown-menu.show {
    display: block;
  }
  
  .dropdown-item {
    padding-left: 2rem;
  }
}

/* Dropdown animations */
.dropdown-menu {
  transition: all 0.3s ease;
  opacity: 0;
  visibility: hidden;
  transform: translateY(-10px);
}

.dropdown-menu.show {
  opacity: 1;
  visibility: visible;
  transform: translateY(0);
}

/* Button styles */
.btn-primary {
  background-color: #007bff;
  border-color: #007bff;
  border-radius: 20px;
  font-weight: 500;
  transition: all 0.2s ease;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
  transform: translateY(-1px);
}
</style>