<template>
  <div class="forgot-password-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-5">
      <div class="container">
        <div class="row">
          <div class="col-12 text-center">
            <h2 class="mb-3">Şifremi Unuttum</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb justify-content-center bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item">
                  <router-link to="/hesabim/giris" class="text-white-50">Giriş</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Şifremi Unuttum</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Forgot Password Form -->
    <section class="forgot-password-section py-5">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-5 col-md-7">
            <div class="forgot-password-card card shadow-lg border-0">
              <div class="card-body p-5">
                <div class="text-center mb-4">
                  <div class="forgot-icon mb-3">
                    <i class="fas fa-key"></i>
                  </div>
                  <h3 class="card-title">Şifrenizi mi Unuttunuz?</h3>
                  <p class="text-muted">
                    E-posta adresinizi girin, size şifre sıfırlama bağlantısı gönderelim.
                  </p>
                </div>

                <form v-if="!emailSent" @submit.prevent="handleSubmit">
                  <!-- Email Field -->
                  <div class="form-group mb-4">
                    <label for="email" class="form-label">E-posta Adresi</label>
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

                  <!-- Submit Button -->
                  <button
                    type="submit"
                    class="btn btn-primary w-100 mb-3"
                    :disabled="isLoading"
                  >
                    <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    Şifre Sıfırlama Bağlantısı Gönder
                  </button>

                  <!-- Back to Login -->
                  <div class="text-center">
                    <router-link to="/hesabim/giris" class="text-decoration-none">
                      <i class="fas fa-arrow-left me-1"></i>
                      Giriş sayfasına dön
                    </router-link>
                  </div>
                </form>

                <!-- Success Message -->
                <div v-if="emailSent" class="text-center">
                  <div class="success-icon mb-3">
                    <i class="fas fa-check-circle text-success"></i>
                  </div>
                  <h4 class="text-success mb-3">E-posta Gönderildi!</h4>
                  <p class="text-muted mb-4">
                    <strong>{{ form.email }}</strong> adresine şifre sıfırlama bağlantısı gönderdik.
                    E-posta kutunuzu kontrol edin.
                  </p>
                  <div class="alert alert-info">
                    <small>
                      <i class="fas fa-info-circle me-1"></i>
                      E-postayı görmüyorsanız spam klasörünüzü kontrol edin.
                    </small>
                  </div>
                  <div class="resend-section">
                    <p class="text-muted small mb-2">E-posta gelmediyse:</p>
                    <button
                      type="button"
                      class="btn btn-outline-primary btn-sm"
                      @click="resendEmail"
                      :disabled="isResending || resendCooldown > 0"
                    >
                      <span v-if="isResending" class="spinner-border spinner-border-sm me-1"></span>
                      <span v-if="resendCooldown > 0">
                        Tekrar gönder ({{ resendCooldown }}s)
                      </span>
                      <span v-else>
                        Tekrar gönder
                      </span>
                    </button>
                  </div>
                  <hr class="my-4">
                  <router-link to="/hesabim/giris" class="btn btn-secondary">
                    Giriş sayfasına dön
                  </router-link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, computed, onUnmounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'ForgotPasswordPage',
  setup() {
    const authStore = useAuthStore()
    
    const form = ref({
      email: ''
    })
    
    const errors = ref({})
    const emailSent = ref(false)
    const isResending = ref(false)
    const resendCooldown = ref(0)
    let resendTimer = null
    
    const isLoading = computed(() => authStore.isLoading)
    
    const validateForm = () => {
      errors.value = {}
      
      if (!form.value.email) {
        errors.value.email = 'E-posta adresi gereklidir'
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.value.email)) {
        errors.value.email = 'Geçerli bir e-posta adresi giriniz'
      }
      
      return Object.keys(errors.value).length === 0
    }
    
    const handleSubmit = async () => {
      if (!validateForm()) return
      
      const result = await authStore.forgotPassword(form.value.email)
      
      if (result.success) {
        emailSent.value = true
        startResendCooldown()
      }
    }
    
    const resendEmail = async () => {
      if (resendCooldown.value > 0) return
      
      isResending.value = true
      
      try {
        const result = await authStore.forgotPassword(form.value.email)
        if (result.success) {
          startResendCooldown()
        }
      } finally {
        isResending.value = false
      }
    }
    
    const startResendCooldown = () => {
      resendCooldown.value = 60 // 60 seconds cooldown
      
      resendTimer = setInterval(() => {
        resendCooldown.value--
        if (resendCooldown.value <= 0) {
          clearInterval(resendTimer)
        }
      }, 1000)
    }
    
    onUnmounted(() => {
      if (resendTimer) {
        clearInterval(resendTimer)
      }
    })
    
    return {
      form,
      errors,
      emailSent,
      isResending,
      resendCooldown,
      isLoading,
      handleSubmit,
      resendEmail
    }
  }
}
</script>

<style scoped>
.forgot-password-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.breadcrumb-section {
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
}

.breadcrumb-item + .breadcrumb-item::before {
  color: rgba(255, 255, 255, 0.5);
}

.forgot-password-card {
  border-radius: 15px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.forgot-password-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1) !important;
}

.forgot-icon {
  width: 80px;
  height: 80px;
  margin: 0 auto;
  background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  color: white;
}

.success-icon {
  font-size: 4rem;
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

.btn-outline-primary {
  border: 2px solid #007bff;
  border-radius: 10px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-outline-primary:hover {
  transform: translateY(-1px);
}

.btn-secondary {
  background-color: #6c757d;
  border-color: #6c757d;
  border-radius: 10px;
  padding: 10px 20px;
  font-weight: 500;
}

.resend-section {
  margin: 1.5rem 0;
  padding: 1rem;
  background-color: #f8f9fa;
  border-radius: 10px;
}

.invalid-feedback {
  display: block;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

/* Animation */
.forgot-password-card {
  animation: slideUp 0.6s ease;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsive */
@media (max-width: 768px) {
  .forgot-password-card .card-body {
    padding: 2rem 1.5rem;
  }
  
  .breadcrumb-section {
    padding: 3rem 0;
  }
  
  .forgot-icon {
    width: 60px;
    height: 60px;
    font-size: 1.5rem;
  }
}
</style>