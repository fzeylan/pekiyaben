<template>
  <div class="register-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-5">
      <div class="container">
        <div class="row">
          <div class="col-12 text-center">
            <h2 class="mb-3">Hesap Oluştur</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb justify-content-center bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Kayıt Ol</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Register Form -->
    <section class="register-section py-5">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-8 col-md-10">
            <div class="register-card card shadow-lg border-0">
              <div class="card-body p-5">
                <div class="text-center mb-4">
                  <h3 class="card-title">Yeni Hesap Oluştur</h3>
                  <p class="text-muted">Kişisel gelişim yolculuğunuza başlayın</p>
                </div>

                <form @submit.prevent="handleRegister">
                  <div class="row">
                    <!-- Full Name -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="fullName" class="form-label">Ad Soyad *</label>
                        <div class="input-group">
                          <span class="input-group-text">
                            <i class="fas fa-user"></i>
                          </span>
                          <input
                            id="fullName"
                            v-model="form.fullName"
                            type="text"
                            class="form-control"
                            :class="{ 'is-invalid': errors.fullName }"
                            placeholder="Adınız ve soyadınız"
                            required
                          />
                        </div>
                        <div v-if="errors.fullName" class="invalid-feedback">
                          {{ errors.fullName }}
                        </div>
                      </div>
                    </div>

                    <!-- Email -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="email" class="form-label">E-posta *</label>
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
                            placeholder="E-posta adresiniz"
                            required
                          />
                        </div>
                        <div v-if="errors.email" class="invalid-feedback">
                          {{ errors.email }}
                        </div>
                      </div>
                    </div>

                    <!-- Password -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="password" class="form-label">Şifre *</label>
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
                            placeholder="Şifreniz (min. 6 karakter)"
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
                    </div>

                    <!-- Confirm Password -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="confirmPassword" class="form-label">Şifre Tekrar *</label>
                        <div class="input-group">
                          <span class="input-group-text">
                            <i class="fas fa-lock"></i>
                          </span>
                          <input
                            id="confirmPassword"
                            v-model="form.confirmPassword"
                            :type="showConfirmPassword ? 'text' : 'password'"
                            class="form-control"
                            :class="{ 'is-invalid': errors.confirmPassword }"
                            placeholder="Şifrenizi tekrar giriniz"
                            required
                          />
                          <button
                            type="button"
                            class="btn btn-outline-secondary"
                            @click="toggleConfirmPasswordVisibility"
                          >
                            <i :class="showConfirmPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
                          </button>
                        </div>
                        <div v-if="errors.confirmPassword" class="invalid-feedback">
                          {{ errors.confirmPassword }}
                        </div>
                      </div>
                    </div>

                    <!-- Phone -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="phone" class="form-label">Telefon</label>
                        <div class="input-group">
                          <span class="input-group-text">
                            <i class="fas fa-phone"></i>
                          </span>
                          <input
                            id="phone"
                            v-model="form.phone"
                            type="tel"
                            class="form-control"
                            placeholder="0555 123 45 67"
                          />
                        </div>
                      </div>
                    </div>

                    <!-- Birth Date -->
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label for="birthDate" class="form-label">Doğum Tarihi</label>
                        <div class="input-group">
                          <span class="input-group-text">
                            <i class="fas fa-calendar"></i>
                          </span>
                          <input
                            id="birthDate"
                            v-model="form.birthDate"
                            type="date"
                            class="form-control"
                          />
                        </div>
                      </div>
                    </div>
                  </div>

                  <!-- Agreement Checkbox -->
                  <div class="form-group mb-4">
                    <div class="form-check">
                      <input
                        id="agreement"
                        v-model="form.agreement"
                        type="checkbox"
                        class="form-check-input"
                        :class="{ 'is-invalid': errors.agreement }"
                        required
                      />
                      <label for="agreement" class="form-check-label">
                        <a href="#" class="text-decoration-none">Üyelik Sözleşmesi</a>'ni ve
                        <a href="#" class="text-decoration-none">Gizlilik Politikası</a>'nı okudum, kabul ediyorum *
                      </label>
                    </div>
                    <div v-if="errors.agreement" class="invalid-feedback d-block">
                      {{ errors.agreement }}
                    </div>
                  </div>

                  <!-- Marketing Permission -->
                  <div class="form-group mb-4">
                    <div class="form-check">
                      <input
                        id="marketing"
                        v-model="form.marketingPermission"
                        type="checkbox"
                        class="form-check-input"
                      />
                      <label for="marketing" class="form-check-label">
                        Pazarlama iletişimi almayı kabul ediyorum
                      </label>
                    </div>
                  </div>

                  <!-- Submit Button -->
                  <button
                    type="submit"
                    class="btn btn-primary w-100 mb-3"
                    :disabled="isLoading"
                  >
                    <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    Hesap Oluştur
                  </button>

                  <!-- Login Link -->
                  <div class="text-center">
                    <p class="mb-0">
                      Zaten hesabınız var mı?
                      <router-link to="/hesabim/giris" class="text-decoration-none fw-bold">
                        Giriş yapın
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
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'RegisterPage',
  setup() {
    const router = useRouter()
    const authStore = useAuthStore()
    
    const form = ref({
      fullName: '',
      email: '',
      password: '',
      confirmPassword: '',
      phone: '',
      birthDate: '',
      agreement: false,
      marketingPermission: false
    })
    
    const errors = ref({})
    const showPassword = ref(false)
    const showConfirmPassword = ref(false)
    
    const isLoading = computed(() => authStore.isLoading)
    
    const validateForm = () => {
      errors.value = {}
      
      if (!form.value.fullName || form.value.fullName.trim().length < 2) {
        errors.value.fullName = 'Ad soyad en az 2 karakter olmalıdır'
      }
      
      if (!form.value.email) {
        errors.value.email = 'E-posta adresi gereklidir'
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.value.email)) {
        errors.value.email = 'Geçerli bir e-posta adresi giriniz'
      }
      
      if (!form.value.password) {
        errors.value.password = 'Şifre gereklidir'
      } else if (form.value.password.length < 6) {
        errors.value.password = 'Şifre en az 6 karakter olmalıdır'
      } else if (form.value.password.length > 12) {
        errors.value.password = 'Şifre en fazla 12 karakter olmalıdır'
      }
      
      if (!form.value.confirmPassword) {
        errors.value.confirmPassword = 'Şifre tekrar gereklidir'
      } else if (form.value.password !== form.value.confirmPassword) {
        errors.value.confirmPassword = 'Şifreler eşleşmiyor'
      }
      
      if (!form.value.agreement) {
        errors.value.agreement = 'Üyelik sözleşmesini kabul etmelisiniz'
      }
      
      return Object.keys(errors.value).length === 0
    }
    
    const handleRegister = async () => {
      if (!validateForm()) return
      
      const result = await authStore.register({
        fullName: form.value.fullName,
        email: form.value.email,
        password: form.value.password,
        confirmPassword: form.value.confirmPassword,
        phone: form.value.phone,
        birthDate: form.value.birthDate,
        agreement: form.value.agreement,
        marketingPermission: form.value.marketingPermission
      })
      
      if (result.success) {
        router.push('/hesabim/giris')
      }
    }
    
    const togglePasswordVisibility = () => {
      showPassword.value = !showPassword.value
    }
    
    const toggleConfirmPasswordVisibility = () => {
      showConfirmPassword.value = !showConfirmPassword.value
    }
    
    return {
      form,
      errors,
      showPassword,
      showConfirmPassword,
      isLoading,
      handleRegister,
      togglePasswordVisibility,
      toggleConfirmPasswordVisibility
    }
  }
}
</script>

<style scoped>
.register-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.register-card {
  border-radius: 15px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.register-card:hover {
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

.form-check-input:checked {
  background-color: #007bff;
  border-color: #007bff;
}

.invalid-feedback {
  display: block;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

/* Animation */
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

@media (max-width: 768px) {
  .register-card .card-body {
    padding: 2rem 1.5rem;
  }
}
</style>