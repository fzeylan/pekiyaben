import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

// Import components
import HomePage from '@/views/HomePage.vue'
import LoginPage from '@/views/auth/LoginPage.vue'
import RegisterPage from '@/views/auth/RegisterPage.vue'
import ForgotPasswordPage from '@/views/auth/ForgotPasswordPage.vue'
import ProfilePage from '@/views/user/ProfilePage.vue'
import CoachesPage from '@/views/coaches/CoachesPage.vue'
import CoachDetailPage from '@/views/coaches/CoachDetailPage.vue'
import InterviewsPage from '@/views/interviews/InterviewsPage.vue'
import InterviewCalendarPage from '@/views/interviews/InterviewCalendarPage.vue'
import MyAppointmentsPage from '@/views/user/MyAppointmentsPage.vue'
import BlogPage from '@/views/blog/BlogPage.vue'
import BlogPostPage from '@/views/blog/BlogPostPage.vue'
import FAQPage from '@/views/FAQPage.vue'
import NotFoundPage from '@/views/errors/NotFoundPage.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage,
    meta: { title: 'Ana Sayfa - PekiYaBen' }
  },
  {
    path: '/hesabim/giris',
    name: 'Login',
    component: LoginPage,
    meta: { 
      title: 'Giriş Yap - PekiYaBen',
      requiresGuest: true 
    }
  },
  {
    path: '/hesabim/kayit',
    name: 'Register',
    component: RegisterPage,
    meta: { 
      title: 'Kayıt Ol - PekiYaBen',
      requiresGuest: true 
    }
  },
  {
    path: '/hesabim/sifremi-unuttum',
    name: 'ForgotPassword',
    component: ForgotPasswordPage,
    meta: { 
      title: 'Şifremi Unuttum - PekiYaBen',
      requiresGuest: true 
    }
  },
  {
    path: '/hesabim/profilim',
    name: 'Profile',
    component: ProfilePage,
    meta: { 
      title: 'Profilim - PekiYaBen',
      requiresAuth: true 
    }
  },
  {
    path: '/hesabim/randevularim',
    name: 'MyAppointments',
    component: MyAppointmentsPage,
    meta: { 
      title: 'Randevularım - PekiYaBen',
      requiresAuth: true 
    }
  },
  {
    path: '/kocluk-ekibimiz',
    name: 'Coaches',
    component: CoachesPage,
    meta: { title: 'Koçluk Ekibimiz - PekiYaBen' }
  },
  {
    path: '/kocluk-ekibimiz/:id/:title/:coachId/:coachName',
    name: 'CoachDetail',
    component: CoachDetailPage,
    meta: { title: 'Koç Detayı - PekiYaBen' }
  },
  {
    path: '/online-kocluk-ve-atolye',
    name: 'Interviews',
    component: InterviewsPage,
    meta: { title: 'Online Koçluk ve Atölye - PekiYaBen' }
  },
  {
    path: '/online-kocluk-ve-atolye/:id/:title',
    name: 'InterviewsDetail',
    component: InterviewsPage,
    meta: { title: 'Online Koçluk ve Atölye - PekiYaBen' }
  },
  {
    path: '/randevu-takvimi/:id/:title/:coachId/:coachName',
    name: 'InterviewCalendar',
    component: InterviewCalendarPage,
    meta: { 
      title: 'Randevu Takvimi - PekiYaBen',
      requiresAuth: true 
    }
  },
  {
    path: '/kisisel-gelisim/yazilar',
    name: 'Blog',
    component: BlogPage,
    meta: { title: 'Kişisel Gelişim Yazıları - PekiYaBen' }
  },
  {
    path: '/kisisel-gelisim/yazilar/:page',
    name: 'BlogPaged',
    component: BlogPage,
    meta: { title: 'Kişisel Gelişim Yazıları - PekiYaBen' }
  },
  {
    path: '/kisisel-gelisim/yazilar/:id/:title',
    name: 'BlogPost',
    component: BlogPostPage,
    meta: { title: 'Yazı Detayı - PekiYaBen' }
  },
  {
    path: '/sikca-sorulanlar',
    name: 'FAQ',
    component: FAQPage,
    meta: { title: 'Sıkça Sorulanlar - PekiYaBen' }
  },
  {
    path: '/kisisel-durum-analizi',
    name: 'KDA',
    component: () => import('@/views/services/KDAPage.vue'),
    meta: { title: 'Kişisel Durum Analizi - PekiYaBen' }
  },
  {
    path: '/kisisel-strateji-belirleme',
    name: 'KSB',
    component: () => import('@/views/services/KSBPage.vue'),
    meta: { title: 'Kişisel Strateji Belirleme - PekiYaBen' }
  },
  {
    path: '/bilincalti-uygulamalari',
    name: 'BU',
    component: () => import('@/views/services/BUPage.vue'),
    meta: { title: 'Bilinçaltı Uygulamaları - PekiYaBen' }
  },
  {
    path: '/kocluk-gorusmeleri',
    name: 'KG',
    component: () => import('@/views/services/KGPage.vue'),
    meta: { title: 'Koçluk Görüşmeleri - PekiYaBen' }
  },
  {
    path: '/hata',
    name: 'Error',
    component: NotFoundPage,
    meta: { title: 'Hata - PekiYaBen' }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: NotFoundPage,
    meta: { title: 'Sayfa Bulunamadı - PekiYaBen' }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { top: 0 }
    }
  }
})

// Navigation guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  // Update page title
  if (to.meta.title) {
    document.title = to.meta.title
  }
  
  // Check auth requirements
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({
      name: 'Login',
      query: { returnUrl: to.fullPath }
    })
  } else if (to.meta.requiresGuest && authStore.isAuthenticated) {
    next({ name: 'Home' })
  } else {
    next()
  }
})

export default router