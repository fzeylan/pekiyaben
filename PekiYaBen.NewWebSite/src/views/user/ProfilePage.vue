<template>
  <div class="profile-page">
    <!-- Breadcrumb -->
    <section class="breadcrumb-section bg-primary text-white py-4">
      <div class="container">
        <div class="row">
          <div class="col-12">
            <h2 class="mb-3">Profilim</h2>
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0">
                <li class="breadcrumb-item">
                  <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                </li>
                <li class="breadcrumb-item active text-white">Profilim</li>
              </ol>
            </nav>
          </div>
        </div>
      </div>
    </section>

    <!-- Profile Content -->
    <section class="profile-content py-5">
      <div class="container">
        <div class="row">
          <!-- Sidebar -->
          <div class="col-lg-3 mb-4">
            <div class="profile-sidebar">
              <div class="user-info-card">
                <div class="user-avatar">
                  <img 
                    :src="userAvatar" 
                    :alt="userFullName"
                    @error="handleAvatarError"
                  />
                  <button class="avatar-upload-btn" @click="triggerFileUpload">
                    <i class="fas fa-camera"></i>
                  </button>
                  <input 
                    ref="fileInput"
                    type="file" 
                    accept="image/*" 
                    @change="handleAvatarUpload"
                    style="display: none;"
                  />
                </div>
                <h5 class="user-name">{{ userFullName }}</h5>
                <p class="user-email">{{ userEmail }}</p>
              </div>
              
              <nav class="profile-nav">
                <ul class="nav flex-column">
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link" 
                      :class="{ active: activeTab === 'profile' }"
                      @click.prevent="activeTab = 'profile'"
                    >
                      <i class="fas fa-user me-2"></i>
                      Kişisel Bilgiler
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link"
                      :class="{ active: activeTab === 'security' }"
                      @click.prevent="activeTab = 'security'"
                    >
                      <i class="fas fa-lock me-2"></i>
                      Güvenlik
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link"
                      :class="{ active: activeTab === 'invoice' }"
                      @click.prevent="activeTab = 'invoice'"
                    >
                      <i class="fas fa-file-invoice me-2"></i>
                      Fatura Bilgileri
                    </a>
                  </li>
                  <li class="nav-item">
                    <a 
                      href="#" 
                      class="nav-link"
                      :class="{ active: activeTab === 'preferences' }"
                      @click.prevent="activeTab = 'preferences'"
                    >
                      <i class="fas fa-cog me-2"></i>
                      Tercihler
                    </a>
                  </li>
                </ul>
              </nav>
            </div>
          </div>

          <!-- Main Content -->
          <div class="col-lg-9">
            <!-- Personal Information Tab -->
            <div v-show="activeTab === 'profile'" class="tab-content">
              <div class="content-card">
                <h4 class="card-title">Kişisel Bilgiler</h4>
                <form @submit.prevent="updateProfile">
                  <div class="row">
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Ad Soyad *</label>
                        <input
                          v-model="profileForm.fullName"
                          type="text"
                          class="form-control"
                          :class="{ 'is-invalid': errors.fullName }"
                          required
                        />
                        <div v-if="errors.fullName" class="invalid-feedback">
                          {{ errors.fullName }}
                        </div>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">E-posta *</label>
                        <input
                          v-model="profileForm.email"
                          type="email"
                          class="form-control"
                          :class="{ 'is-invalid': errors.email }"
                          required
                        />
                        <div v-if="errors.email" class="invalid-feedback">
                          {{ errors.email }}
                        </div>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Telefon</label>
                        <input
                          v-model="profileForm.phoneNumber"
                          type="tel"
                          class="form-control"
                          placeholder="0555 123 45 67"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Doğum Tarihi</label>
                        <input
                          v-model="profileForm.dateOfBirth"
                          type="date"
                          class="form-control"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Cinsiyet</label>
                        <select v-model="profileForm.gender" class="form-select">
                          <option value="">Seçiniz</option>
                          <option value="1">Erkek</option>
                          <option value="2">Kadın</option>
                          <option value="3">Belirtmek istemiyorum</option>
                        </select>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Şehir</label>
                        <input
                          v-model="profileForm.city"
                          type="text"
                          class="form-control"
                          placeholder="İstanbul"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Meslek</label>
                        <input
                          v-model="profileForm.occupation"
                          type="text"
                          class="form-control"
                          placeholder="Yazılım Geliştirici"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Çalışma Durumu</label>
                        <select v-model="profileForm.occupationStatus" class="form-select">
                          <option value="">Seçiniz</option>
                          <option value="1">Çalışıyor</option>
                          <option value="2">İşsiz</option>
                          <option value="3">Emekli</option>
                          <option value="4">Öğrenci</option>
                        </select>
                      </div>
                    </div>
                  </div>
                  
                  <div class="form-group mb-4">
                    <div class="form-check">
                      <input
                        v-model="profileForm.communicationPermission"
                        type="checkbox"
                        class="form-check-input"
                        id="communicationPermission"
                      />
                      <label class="form-check-label" for="communicationPermission">
                        Pazarlama iletişimi almayı kabul ediyorum
                      </label>
                    </div>
                  </div>
                  
                  <button type="submit" class="btn btn-primary" :disabled="isUpdating">
                    <span v-if="isUpdating" class="spinner-border spinner-border-sm me-2"></span>
                    Güncelle
                  </button>
                </form>
              </div>
            </div>

            <!-- Security Tab -->
            <div v-show="activeTab === 'security'" class="tab-content">
              <div class="content-card">
                <h4 class="card-title">Şifre Değiştir</h4>
                <form @submit.prevent="updatePassword">
                  <div class="form-group mb-3">
                    <label class="form-label">Mevcut Şifre *</label>
                    <input
                      v-model="passwordForm.currentPassword"
                      type="password"
                      class="form-control"
                      :class="{ 'is-invalid': errors.currentPassword }"
                      required
                    />
                    <div v-if="errors.currentPassword" class="invalid-feedback">
                      {{ errors.currentPassword }}
                    </div>
                  </div>
                  
                  <div class="form-group mb-3">
                    <label class="form-label">Yeni Şifre *</label>
                    <input
                      v-model="passwordForm.newPassword"
                      type="password"
                      class="form-control"
                      :class="{ 'is-invalid': errors.newPassword }"
                      required
                    />
                    <div v-if="errors.newPassword" class="invalid-feedback">
                      {{ errors.newPassword }}
                    </div>
                    <div class="form-text">
                      Şifre en az 6 karakter olmalıdır.
                    </div>
                  </div>
                  
                  <div class="form-group mb-4">
                    <label class="form-label">Yeni Şifre Tekrar *</label>
                    <input
                      v-model="passwordForm.confirmPassword"
                      type="password"
                      class="form-control"
                      :class="{ 'is-invalid': errors.confirmPassword }"
                      required
                    />
                    <div v-if="errors.confirmPassword" class="invalid-feedback">
                      {{ errors.confirmPassword }}
                    </div>
                  </div>
                  
                  <button type="submit" class="btn btn-primary" :disabled="isUpdatingPassword">
                    <span v-if="isUpdatingPassword" class="spinner-border spinner-border-sm me-2"></span>
                    Şifreyi Değiştir
                  </button>
                </form>
              </div>
            </div>

            <!-- Invoice Information Tab -->
            <div v-show="activeTab === 'invoice'" class="tab-content">
              <div class="content-card">
                <h4 class="card-title">Fatura Bilgileri</h4>
                <form @submit.prevent="updateInvoiceInfo">
                  <div class="form-group mb-3">
                    <label class="form-label">Fatura Türü *</label>
                    <div class="form-check">
                      <input
                        v-model="invoiceForm.invoiceType"
                        type="radio"
                        class="form-check-input"
                        id="individual"
                        value="1"
                      />
                      <label class="form-check-label" for="individual">
                        Bireysel
                      </label>
                    </div>
                    <div class="form-check">
                      <input
                        v-model="invoiceForm.invoiceType"
                        type="radio"
                        class="form-check-input"
                        id="corporate"
                        value="2"
                      />
                      <label class="form-check-label" for="corporate">
                        Kurumsal
                      </label>
                    </div>
                  </div>
                  
                  <div v-if="invoiceForm.invoiceType === '1'" class="row">
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">TC Kimlik No</label>
                        <input
                          v-model="invoiceForm.personalID"
                          type="text"
                          class="form-control"
                          maxlength="11"
                          placeholder="12345678901"
                        />
                      </div>
                    </div>
                  </div>
                  
                  <div v-if="invoiceForm.invoiceType === '2'" class="row">
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Şirket Adı *</label>
                        <input
                          v-model="invoiceForm.companyName"
                          type="text"
                          class="form-control"
                          :required="invoiceForm.invoiceType === '2'"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Vergi No *</label>
                        <input
                          v-model="invoiceForm.taxNumber"
                          type="text"
                          class="form-control"
                          :required="invoiceForm.invoiceType === '2'"
                        />
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group mb-3">
                        <label class="form-label">Vergi Dairesi *</label>
                        <input
                          v-model="invoiceForm.taxOffice"
                          type="text"
                          class="form-control"
                          :required="invoiceForm.invoiceType === '2'"
                        />
                      </div>
                    </div>
                  </div>
                  
                  <div class="form-group mb-4">
                    <label class="form-label">Fatura Adresi *</label>
                    <textarea
                      v-model="invoiceForm.invoiceAddress"
                      class="form-control"
                      rows="3"
                      required
                    ></textarea>
                  </div>
                  
                  <button type="submit" class="btn btn-primary" :disabled="isUpdatingInvoice">
                    <span v-if="isUpdatingInvoice" class="spinner-border spinner-border-sm me-2"></span>
                    Güncelle
                  </button>
                </form>
              </div>
            </div>

            <!-- Preferences Tab -->
            <div v-show="activeTab === 'preferences'" class="tab-content">
              <div class="content-card">
                <h4 class="card-title">Hesap Tercihleri</h4>
                
                <div class="preference-section">
                  <h6>Bildirimler</h6>
                  <div class="form-check mb-2">
                    <input
                      v-model="preferences.emailNotifications"
                      type="checkbox"
                      class="form-check-input"
                      id="emailNotifications"
                    />
                    <label class="form-check-label" for="emailNotifications">
                      E-posta bildirimleri
                    </label>
                  </div>
                  <div class="form-check mb-2">
                    <input
                      v-model="preferences.smsNotifications"
                      type="checkbox"
                      class="form-check-input"
                      id="smsNotifications"
                    />
                    <label class="form-check-label" for="smsNotifications">
                      SMS bildirimleri
                    </label>
                  </div>
                  <div