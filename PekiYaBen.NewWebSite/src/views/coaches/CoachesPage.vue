<template>
    <div class="coaches-page">
        <!-- Breadcrumb -->
        <section class="breadcrumb-section bg-primary text-white py-4">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <h2 class="mb-3">Koçluk Ekibimiz</h2>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb bg-transparent mb-0">
                                <li class="breadcrumb-item">
                                    <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                                </li>
                                <li class="breadcrumb-item active text-white">Koçluk Ekibimiz</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </section>

        <!-- Coaches Content -->
        <section class="coaches-content py-5">
            <div class="container">
                <!-- Filters -->
                <div class="row mb-4">
                    <div class="col-12">
                        <div class="coaches-filters">
                            <div class="row align-items-center">
                                <div class="col-md-6">
                                    <div class="search-box">
                                        <div class="input-group">
                                            <input v-model="searchQuery"
                                                   type="text"
                                                   class="form-control"
                                                   placeholder="Koç adı veya uzmanlık alanı ara..."
                                                   @keyup.enter="searchCoaches" />
                                            <button class="btn btn-outline-primary" @click="searchCoaches">
                                                <i class="fas fa-search"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="filter-options">
                                        <select v-model="selectedCategory" @change="filterCoaches" class="form-select me-2">
                                            <option value="">Tüm Kategoriler</option>
                                            <option v-for="category in categories" :key="category.id" :value="category.id">
                                                {{ category.name }}
                                            </option>
                                        </select>
                                        <select v-model="sortBy" @change="sortCoaches" class="form-select">
                                            <option value="name">İsme Göre</option>
                                            <option value="rating">Puana Göre</option>
                                            <option value="experience">Deneyime Göre</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Loading State -->
                <div v-if="loading" class="text-center py-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Yükleniyor...</span>
                    </div>
                    <p class="mt-3">Koçlar yükleniyor...</p>
                </div>

                <!-- Coaches Grid -->
                <div v-else-if="filteredCoaches.length" class="coaches-grid">
                    <div class="row g-4">
                        <div v-for="coach in paginatedCoaches"
                             :key="coach.id"
                             class="col-lg-4 col-md-6">
                            <div class="coach-card">
                                <div class="coach-image">
                                    <router-link :to="`/kocluk-ekibimiz/${coach.categoryId}/category/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}`">
                                        <img :src="coach.profilePhoto || '/default-coach.jpg'"
                                             :alt="coach.fullName"
                                             class="img-fluid"
                                             @error="handleImageError" />
                                    </router-link>
                                    <div v-if="coach.isOnline" class="online-badge">
                                        <i class="fas fa-circle"></i> Çevrimiçi
                                    </div>
                                </div>
                                <div class="coach-info">
                                    <h5>
                                        <router-link :to="`/kocluk-ekibimiz/${coach.categoryId}/category/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}`">
                                            {{ coach.fullName }}
                                        </router-link>
                                    </h5>
                                    <p class="coach-title text-primary">{{ coach.title }}</p>
                                    <div class="coach-rating mb-2">
                                        <div class="stars">
                                            <i v-for="star in 5"
                                               :key="star"
                                               class="fas fa-star"
                                               :class="{ 'text-warning': star <= (coach.rating || 0) }"></i>
                                        </div>
                                        <span class="rating-text ms-2">
                                            {{ coach.rating || 0 }}/5 ({{ coach.reviewCount || 0 }})
                                        </span>
                                    </div>
                                    <p class="coach-description">{{ truncateText(coach.shortDescription, 100) }}</p>
                                    <div class="coach-specialties mb-3">
                                        <span v-for="specialty in coach.topSpecialties?.slice(0, 3)"
                                              :key="specialty"
                                              class="badge bg-light text-dark me-1 mb-1">
                                            {{ specialty }}
                                        </span>
                                    </div>
                                    <div class="coach-actions">
                                        <div class="row">
                                            <div class="col-8">
                                                <router-link :to="`/kocluk-ekibimiz/${coach.categoryId}/category/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}`"
                                                             class="btn btn-primary btn-sm w-100">
                                                    Profili Görüntüle
                                                </router-link>
                                            </div>
                                            <div class="col-4">
                                                <router-link :to="`/randevu-takvimi/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}/${coach.id}/${coach.fullName.toLowerCase().replace(/\s+/g, '-')}`"
                                                             class="btn btn-outline-primary btn-sm w-100"
                                                             title="Randevu Al">
                                                    <i class="fas fa-calendar-alt"></i>
                                                </router-link>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- No Results -->
                <div v-else class="no-results text-center py-5">
                    <i class="fas fa-search fa-3x text-muted mb-3"></i>
                    <h4>Koç bulunamadı</h4>
                    <p class="text-muted">
                        Arama kriterlerinize uygun koç bulunamadı. Lütfen farklı anahtar kelimeler deneyin.
                    </p>
                    <button @click="clearFilters" class="btn btn-primary">
                        Filtreleri Temizle
                    </button>
                </div>

                <!-- Pagination -->
                <div v-if="totalPages > 1" class="pagination-wrapper mt-5">
                    <nav aria-label="Coaches pagination">
                        <ul class="pagination justify-content-center">
                            <li class="page-item" :class="{ disabled: currentPage === 1 }">
                                <button class="page-link"
                                        @click="changePage(currentPage - 1)"
                                        :disabled="currentPage === 1">
                                    Önceki
                                </button>
                            </li>

                            <li v-for="page in visiblePages"
                                :key="page"
                                class="page-item"
                                :class="{ active: page === currentPage }">
                                <button class="page-link" @click="changePage(page)">
                                    {{ page }}
                                </button>
                            </li>

                            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                                <button class="page-link"
                                        @click="changePage(currentPage + 1)"
                                        :disabled="currentPage === totalPages">
                                    Sonraki
                                </button>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
    import { ref, computed, onMounted, watch } from 'vue'
    import { useRoute, useRouter } from 'vue-router'
    import contentService from '@/services/content'

    export default {
        name: 'CoachesPage',
        setup() {
            const route = useRoute()
            const router = useRouter()

            // Reactive data
            const coaches = ref([])
            const categories = ref([])
            const loading = ref(true)
            const searchQuery = ref('')
            const selectedCategory = ref('')
            const sortBy = ref('name')
            const currentPage = ref(1)
            const itemsPerPage = 9

            // Computed properties
            const filteredCoaches = computed(() => {
                let filtered = [...coaches.value]

                // Filter by search query
                if (searchQuery.value) {
                    const query = searchQuery.value.toLowerCase()
                    filtered = filtered.filter(coach =>
                        coach.fullName.toLowerCase().includes(query) ||
                        coach.title.toLowerCase().includes(query) ||
                        coach.shortDescription?.toLowerCase().includes(query) ||
                        coach.topSpecialties?.some(spec => spec.toLowerCase().includes(query))
                    )
                }

                // Filter by category
                if (selectedCategory.value) {
                    filtered = filtered.filter(coach => coach.categoryId == selectedCategory.value)
                }

                // Sort
                filtered.sort((a, b) => {
                    switch (sortBy.value) {
                        case 'rating':
                            return (b.rating || 0) - (a.rating || 0)
                        case 'experience':
                            return (b.experienceYears || 0) - (a.experienceYears || 0)
                        case 'name':
                        default:
                            return a.fullName.localeCompare(b.fullName)
                    }
                })

                return filtered
            })

            const totalPages = computed(() =>
                Math.ceil(filteredCoaches.value.length / itemsPerPage)
            )

            const paginatedCoaches = computed(() => {
                const start = (currentPage.value - 1) * itemsPerPage
                const end = start + itemsPerPage
                return filteredCoaches.value.slice(start, end)
            })

            const visiblePages = computed(() => {
                const pages = []
                const total = totalPages.value
                const current = currentPage.value
                const delta = 2

                for (let i = Math.max(2, current - delta);
                    i <= Math.min(total - 1, current + delta); i++) {
                    pages.push(i)
                }

                if (current - delta > 2) {
                    pages.unshift('...')
                }
                if (current + delta < total - 1) {
                    pages.push('...')
                }

                pages.unshift(1)
                if (total > 1) {
                    pages.push(total)
                }

                return pages.filter((page, index, arr) => arr.indexOf(page) === index)
            })

            // Methods
            const loadCoaches = async () => {
                try {
                    loading.value = true
                    const response = await contentService.getCoaches()
                    coaches.value = response.data || []
                } catch (error) {
                    console.error('Coaches load error:', error)
                } finally {
                    loading.value = false
                }
            }

            const loadCategories = async () => {
                try {
                    const response = await contentService.getCoachCategories()
                    categories.value = response.data || []
                } catch (error) {
                    console.error('Categories load error:', error)
                }
            }

            const searchCoaches = () => {
                currentPage.value = 1
                // Update URL with search query
                const query = { ...route.query }
                if (searchQuery.value) {
                    query.search = searchQuery.value
                } else {
                    delete query.search
                }
                router.push({ query })
            }

            const filterCoaches = () => {
                currentPage.value = 1
                // Update URL with category filter
                const query = { ...route.query }
                if (selectedCategory.value) {
                    query.category = selectedCategory.value
                } else {
                    delete query.category
                }
                router.push({ query })
            }

            const sortCoaches = () => {
                currentPage.value = 1
                // Update URL with sort parameter
                const query = { ...route.query }
                if (sortBy.value !== 'name') {
                    query.sort = sortBy.value
                } else {
                    delete query.sort
                }
                router.push({ query })
            }

            const clearFilters = () => {
                searchQuery.value = ''
                selectedCategory.value = ''
                sortBy.value = 'name'
                currentPage.value = 1
                router.push({ query: {} })
            }

            const changePage = (page) => {
                if (page >= 1 && page <= totalPages.value) {
                    currentPage.value = page
                    window.scrollTo({ top: 0, behavior: 'smooth' })

                    // Update URL with page parameter
                    const query = { ...route.query }
                    if (page > 1) {
                        query.page = page
                    } else {
                        delete query.page
                    }
                    router.push({ query })
                }
            }

            const truncateText = (text, maxLength) => {
                if (!text) return ''
                if (text.length <= maxLength) return text
                return text.substr(0, maxLength) + '...'
            }

            const handleImageError = (event) => {
                event.target.src = '/default-coach.jpg'
            }

            // Initialize from URL parameters
            const initializeFromURL = () => {
                searchQuery.value = route.query.search || ''
                selectedCategory.value = route.query.category || ''
                sortBy.value = route.query.sort || 'name'
                currentPage.value = parseInt(route.query.page) || 1
            }

            // Lifecycle
            onMounted(async () => {
                initializeFromURL()
                await Promise.all([
                    loadCoaches(),
                    loadCategories()
                ])
            })

            // Watch for route changes
            watch(() => route.query, () => {
                initializeFromURL()
            })

            // Reset page when filters change
            watch([searchQuery, selectedCategory, sortBy], () => {
                currentPage.value = 1
            })

            return {
                // Reactive data
                coaches,
                categories,
                loading,
                searchQuery,
                selectedCategory,
                sortBy,
                currentPage,

                // Computed
                filteredCoaches,
                totalPages,
                paginatedCoaches,
                visiblePages,

                // Methods
                searchCoaches,
                filterCoaches,
                sortCoaches,
                clearFilters,
                changePage,
                truncateText,
                handleImageError
            }
        }
    }
</script>

<style scoped>
    .coaches-filters {
        background: white;
        border-radius: 15px;
        padding: 2rem;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        margin-bottom: 2rem;
    }

    .search-box .input-group .form-control {
        border: 2px solid #eee;
        border-radius: 25px 0 0 25px;
        padding: 12px 20px;
    }

    .search-box .input-group .btn {
        border: 2px solid #007bff;
        border-radius: 0 25px 25px 0;
        padding: 12px 20px;
    }

    .filter-options {
        display: flex;
        gap: 1rem;
    }

        .filter-options .form-select {
            border: 2px solid #eee;
            border-radius: 25px;
            padding: 12px 20px;
        }

    .coaches-grid {
        min-height: 600px;
    }

    .coach-card {
        background: white;
        border-radius: 15px;
        overflow: hidden;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
        transition: all 0.3s ease;
        height: 100%;
        display: flex;
        flex-direction: column;
    }

        .coach-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 15px 40px rgba(0, 0, 0, 0.15);
        }

    .coach-image {
        position: relative;
        overflow: hidden;
        height: 250px;
    }

        .coach-image img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.3s ease;
        }

    .coach-card:hover .coach-image img {
        transform: scale(1.05);
    }

    .online-badge {
        position: absolute;
        top: 1rem;
        right: 1rem;
        background: #28a745;
        color: white;
        padding: 0.5rem 1rem;
        border-radius: 20px;
        font-size: 0.8rem;
        font-weight: 500;
    }

        .online-badge i {
            margin-right: 0.25rem;
            animation: pulse 2s infinite;
        }

    @keyframes pulse {
        0% {
            opacity: 1;
        }

        50% {
            opacity: 0.5;
        }

        100% {
            opacity: 1;
        }
    }

    .coach-info {
        padding: 2rem;
        flex: 1;
        display: flex;
        flex-direction: column;
    }

        .coach-info h5 {
            margin-bottom: 0.5rem;
        }

            .coach-info h5 a {
                color: #333;
                text-decoration: none;
                transition: color 0.3s ease;
            }

                .coach-info h5 a:hover {
                    color: #007bff;
                }

    .coach-title {
        font-size: 1rem;
        font-weight: 500;
        margin-bottom: 1rem;
    }

    .coach-rating {
        display: flex;
        align-items: center;
        margin-bottom: 1rem;
    }

    .stars i {
        font-size: 1rem;
        color: #ddd;
        margin-right: 2px;
    }

        .stars i.text-warning {
            color: #ffc107 !important;
        }

    .rating-text {
        font-size: 0.9rem;
        color: #666;
    }

    .coach-description {
        color: #666;
        line-height: 1.6;
        margin-bottom: 1rem;
        flex: 1;
    }

    .coach-specialties {
        margin-bottom: 1.5rem;
    }

        .coach-specialties .badge {
            font-size: 0.75rem;
            padding: 0.5rem 0.75rem;
            border: 1px solid #ddd;
        }

    .coach-actions {
        margin-top: auto;
    }

        .coach-actions .btn {
            border-radius: 25px;
            font-weight: 500;
            transition: all 0.3s ease;
        }

        .coach-actions .btn-primary:hover {
            background: #0056b3;
            transform: translateY(-2px);
        }

        .coach-actions .btn-outline-primary:hover {
            background: #007bff;
            color: white;
            transform: translateY(-2px);
        }

    .no-results {
        background: white;
        border-radius: 15px;
        padding: 3rem;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
    }

    .pagination-wrapper {
        display: flex;
        justify-content: center;
    }

    .pagination .page-link {
        border: none;
        color: #007bff;
        font-weight: 500;
        margin: 0 0.25rem;
        border-radius: 8px;
        padding: 0.75rem 1rem;
        transition: all 0.3s ease;
    }

    .pagination .page-item.active .page-link {
        background: #007bff;
        color: white;
        box-shadow: 0 4px 12px rgba(0, 123, 255, 0.3);
    }

    .pagination .page-link:hover {
        background: #007bff;
        color: white;
        transform: translateY(-2px);
    }

    .pagination .page-item.disabled .page-link {
        color: #6c757d;
        background: #f8f9fa;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .coaches-filters {
            padding: 1.5rem;
        }

            .coaches-filters .row {
                gap: 1rem;
            }

        .filter-options {
            flex-direction: column;
        }

        .coach-image {
            height: 200px;
        }

        .coach-info {
            padding: 1.5rem;
        }

        .coach-actions .row {
            gap: 0.5rem;
        }

        .coach-actions .col-8,
        .coach-actions .col-4 {
            flex: 1;
        }

        .pagination .page-link {
            padding: 0.5rem 0.75rem;
            font-size: 0.9rem;
        }
    }

    @media (max-width: 576px) {
        .coaches-filters .row {
            flex-direction: column;
        }

        .search-box,
        .filter-options {
            width: 100%;
        }

            .search-box .input-group {
                flex-direction: column;
            }

                .search-box .input-group .form-control {
                    border-radius: 25px;
                    margin-bottom: 0.5rem;
                }

                .search-box .input-group .btn {
                    border-radius: 25px;
                }

        .coach-actions .row {
            flex-direction: column;
        }

        .coach-actions .btn {
            margin-bottom: 0.5rem;
        }

        .pagination .page-link {
            padding: 0.5rem;
            margin: 0 0.1rem;
        }

        .no-results {
            padding: 2rem 1rem;
        }
    }

    /* Loading Animation */
    .coach-card-skeleton {
        background: #f8f9fa;
        border-radius: 15px;
        overflow: hidden;
        height: 500px;
    }

    .skeleton-image {
        height: 250px;
        background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
        background-size: 200% 100%;
        animation: loading 1.5s infinite;
    }

    .skeleton-content {
        padding: 2rem;
    }

    .skeleton-line {
        height: 1rem;
        background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
        background-size: 200% 100%;
        animation: loading 1.5s infinite;
        border-radius: 4px;
        margin-bottom: 1rem;
    }

        .skeleton-line.short {
            width: 60%;
        }

        .skeleton-line.medium {
            width: 80%;
        }

    @keyframes loading {
        0% {
            background-position: 200% 0;
        }

        100% {
            background-position: -200% 0;
        }
    }

    /* Entrance Animations */
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

    .coach-card {
        animation: fadeInUp 0.6s ease-out;
        animation-fill-mode: both;
    }

        .coach-card:nth-child(1) {
            animation-delay: 0.1s;
        }

        .coach-card:nth-child(2) {
            animation-delay: 0.2s;
        }

        .coach-card:nth-child(3) {
            animation-delay: 0.3s;
        }

        .coach-card:nth-child(4) {
            animation-delay: 0.4s;
        }

        .coach-card:nth-child(5) {
            animation-delay: 0.5s;
        }

        .coach-card:nth-child(6) {
            animation-delay: 0.6s;
        }

        .coach-card:nth-child(7) {
            animation-delay: 0.7s;
        }

        .coach-card:nth-child(8) {
            animation-delay: 0.8s;
        }

        .coach-card:nth-child(9) {
            animation-delay: 0.9s;
        }

    /* Dark Theme Support */
    @media (prefers-color-scheme: dark) {
        .coaches-filters,
        .coach-card,
        .no-results {
            background: #2d3748;
            color: #e2e8f0;
        }

        .coach-info h5 a {
            color: #e2e8f0;
        }

            .coach-info h5 a:hover {
                color: #63b3ed;
            }

        .coach-specialties .badge {
            background: #4a5568;
            color: #e2e8f0;
            border-color: #4a5568;
        }

        .search-box .input-group .form-control,
        .filter-options .form-select {
            background: #4a5568;
            border-color: #4a5568;
            color: #e2e8f0;
        }
    }

    /* Print Styles */
    @media print {
        .coaches-filters,
        .pagination-wrapper {
            display: none !important;
        }

        .coach-card {
            break-inside: avoid;
            box-shadow: none;
            border: 1px solid #ddd;
        }

        .coach-actions {
            display: none;
        }
    }

    /* High Contrast Mode */
    @media (prefers-contrast: high) {
        .coach-card:hover {
            outline: 3px solid #007bff;
        }

        .coach-specialties .badge {
            border-width: 2px;
        }
    }

    /* Reduced Motion */
    @media (prefers-reduced-motion: reduce) {
        .coach-card,
        .coach-image img,
        .coach-actions .btn,
        .pagination .page-link {
            transition: none;
        }

            .coach-card:hover {
                transform: none;
            }

                .coach-card:hover .coach-image img {
                    transform: none;
                }

        .online-badge i {
            animation: none;
        }

        .coach-card {
            animation: none;
        }
    }

    /* Focus Styles for Accessibility */
    .search-box .form-control:focus,
    .filter-options .form-select:focus,
    .coach-actions .btn:focus,
    .pagination .page-link:focus {
        outline: 2px solid #007bff;
        outline-offset: 2px;
    }
</style>
}