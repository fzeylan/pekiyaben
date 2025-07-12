<template>
    <div class="not-found-page">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-lg-6 text-center">
                    <div class="error-content">
                        <h1 class="error-code">404</h1>
                        <h2 class="error-title">Sayfa Bulunamadı</h2>
                        <p class="error-message">
                            Aradığınız sayfa bulunamadı. Sayfa silinmiş, taşınmış veya geçici olarak erişilemez durumda olabilir.
                        </p>
                        <div class="error-actions">
                            <router-link to="/" class="btn btn-primary btn-lg me-3">
                                Ana Sayfaya Dön
                            </router-link>
                            <button @click="goBack" class="btn btn-outline-secondary btn-lg">
                                Geri Git
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { useRouter } from 'vue-router'

    export default {
        name: 'NotFoundPage',
        setup() {
            const router = useRouter()

            const goBack = () => {
                if (window.history.length > 1) {
                    router.go(-1)
                } else {
                    router.push('/')
                }
            }

            return {
                goBack
            }
        }
    }
</script>

<style scoped>
    .not-found-page {
        min-height: 80vh;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .error-code {
        font-size: 8rem;
        font-weight: 900;
        color: #007bff;
        margin-bottom: 1rem;
        line-height: 1;
    }

    .error-title {
        font-size: 2.5rem;
        font-weight: 700;
        color: #333;
        margin-bottom: 1rem;
    }

    .error-message {
        font-size: 1.1rem;
        color: #666;
        margin-bottom: 2rem;
        max-width: 500px;
        margin-left: auto;
        margin-right: auto;
    }

    @media (max-width: 768px) {
        .error-code {
            font-size: 6rem;
        }

        .error-title {
            font-size: 2rem;
        }

        .error-actions {
            flex-direction: column;
            gap: 1rem;
        }

            .error-actions .btn {
                margin: 0 !important;
            }
    }
</style>