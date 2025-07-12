<template>
  <div class="login-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-5">
      <div class="container">
        <div class="row">
          <div class="col-12 text-center">
            <h2 class="mb-3">Kullanıcı Girişi</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb justify-content-center bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Giriş Yap</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Login Form -->
    <section class="login-section py-5">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-6 col-md-8">
            <div class="login-card card shadow-lg border-0">
              <div class="card-body p-5">
                <div class="text-center mb-4">
                  <h3 class="card-title">Hesabına Giriş Yap</h3>
                  <p class="text-muted">Lütfen giriş bilgilerinizi giriniz</p>
                </div>

                <form @submit.prevent="handleLogin">
                  <!-- Email Field -->
                  <div class="form-group mb-3">
                    <label for="email" class="form-label">E-posta</label>
                    <div class="input-group">
                      <span class="input-group-text">
                        <i class="fas fa-envelope"></i>
                      </span>
                      <input
                        id="email"
                        v-model="form.email"
                        type="email"
                        class="form-control"
                        :class="{ 'is-invalid': errors.email }"
                        placeholder="E-posta adresinizi giriniz"
                        required
                      />
                    </div>
                    <div v-if="errors.email" class="invalid-feedback">
                      {{ errors.email }}
                    </div>
                  </div>

                  <!-- Password Field -->
                  <div class="form-group mb-4">
                    <label for="password" class="form-label">Şifre</label>
                    <div class="input-group">
                      <span class="input-group-text">
                        <i class="fas fa-lock"></i>
                      </span>
                      <input
                        id="password"
                        v-model="form.password"
                        :type="showPassword ? 'text' : 'password'"
                        class="form-control"
                        :class="{ 'is-invalid': errors.password }"
                        placeholder="Şifrenizi giriniz"
                        required
                      />
                      <button
                        type="button"
                        class="btn btn-outline-secondary"
                        @click="togglePasswordVisibility"
                      >
                        <i :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
                      </button>
                    </div>
                    <div v-if="errors.password" class="invalid-feedback">
                      {{ errors.password }}
                    </div>
                  </div>

                  <!-- Remember Me & Forgot Password -->
                  <div class="d-flex justify-content-between align-items-center mb-4">
                    <div class="form-check">
                      <input
                        id="rememberMe"
                        v-model="form.rememberMe"
                        type="checkbox"
                        class="form-check-input"
                      />
                      <label for="rememberMe" class="form-check-label">
                        Beni hatırla
                      </label>
                    </div>
                    <router-link to="/hesabim/sifremi-unuttum" class="text-decoration-none">
                      Şifremi unuttum
                    </router-link>
                  </div>

                  <!-- Submit Button -->
                  <button
                    type="submit"
                    class="btn btn-primary w-100 mb-3"
                    :disabled="isLoading"
                  >
                    <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    Giriş Yap
                  </button>

                  <!-- Register Link -->
                  <div class="text-center">
                    <p class="mb-0">
                      Hesabınız yok mu?
                      <router-link to="/hesabim/kayit" class="text-decoration-none fw-bold">
                        Kayıt ol
                      </router-link>
                    </p>
                  </div>
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
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'LoginPage',
  setup() {
    const router = useRouter()
    const route = useRoute()
    const authStore = useAuthStore()
    
    const form = ref({
      email: '',
      password: '',
      rememberMe: false
    })
    
    const errors = ref({})
    const showPassword = ref(false)
    
    const isLoading = computed(() => authStore.isLoading)
    
    const validateForm = () => {
      errors.value = {}
      
      if (!form.value.email) {
        errors.value.email = 'E-posta adresi gereklidir'
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.value.email)) {
        errors.value.email = 'Geçerli bir e-posta adresi giriniz'
      }
      
      if (!form.value.password) {
        errors.value.password = 'Şifre gereklidir'
      } else if (form.value.password.length < 6) {
        errors.value.password = 'Şifre en az 6 karakter olmalıdır'
      }
      
      return Object.keys(errors.value).length === 0
    }
    
    const handleLogin = async () => {
      if (!validateForm()) return
      
      const result = await authStore.login({
        email: form.value.email,
        password: form.value.password
      })
      
      if (result.success) {
        const returnUrl = route.query.returnUrl || '/'
        router.push(returnUrl)
      }
    }
    
    const togglePasswordVisibility = () => {
      showPassword.value = !showPassword.value
    }
    
    return {
      form,
      errors,
      showPassword,
      isLoading,
      handleLogin,
      togglePasswordVisibility
    }
  }
}
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.breadcrumb-item + .breadcrumb-item::before {
  color: rgba(255, 255, 255, 0.5);
}

.login-card {
  border-radius: 15px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.login-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1) !important;
}

.form-control {
  border: 2px solid #e9ecef;
  border-radius: 10px;
  padding: 12px 15px;
  font-size: 0.95rem;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #007bff;
  box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.input-group-text {
  background-color: transparent;
  border: 2px solid #e9ecef;
  border-right: none;
  border-radius: 10px 0 0 10px;
  color: #6c757d;
}

.input-group .form-control {
  border-left: none;
  border-radius: 0 10px 10px 0;
}

.input-group .form-control:focus {
  border-left: none;
}

.input-group .form-control:focus + .btn {
  border-color: #007bff;
}

.btn-primary {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
  border: none;
  border-radius: 10px;
  padding: 12px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(0, 123, 255, 0.3);
}

.btn-outline-secondary {
  border: 2px solid #e9ecef;
  border-left: none;
  border-radius: 0 10px 10px 0;
  color: #6c757d;
}

.form-check-input:checked {
  background-color: #007bff;
  border-color: #007bff;
}

.invalid-feedback {
  display: block;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

/* Animation for form elements */
.form-group {
  animation: slideUp 0.6s ease forwards;
  opacity: 0;
  transform: translateY(20px);
}

.form-group:nth-child(1) { animation-delay: 0.1s; }
.form-group:nth-child(2) { animation-delay: 0.2s; }
.form-group:nth-child(3) { animation-delay: 0.3s; }
.form-group:nth-child(4) { animation-delay: 0.4s; }

@keyframes slideUp {
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .login-card .card-body {
    padding: 2rem 1.5rem;
  }
  
  .breadcrumb-section {
    padding: 3rem 0;
  }
}
</style>