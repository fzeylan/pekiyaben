
<template>
    <div class="bu-page">
        <section class="breadcrumb-section bg-primary text-white py-4">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <h2 class="mb-3">Bilinçaltı Uygulamaları</h2>
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb bg-transparent mb-0">
                                <li class="breadcrumb-item">
                                    <router-link to="/" class="text-white-50">Ana Sayfa</router-link>
                                </li>
                                <li class="breadcrumb-item active text-white">Bilinçaltı Uygulamaları</li>
                            </ol>
                        </nav>
                    </div>
                </div>
            </div>
        </section>

        <section class="hero-section py-5">
            <div class="container">
                <div class="row align-items-center">
                    <div class="col-lg-6">
                        <h1 class="hero-title">Bilinçaltınızın <span class="text-primary">Gücünü</span> Keşfedin</h1>
                        <p class="hero-description">
                            Bilinçaltı Uygulamaları ile içinizdeki potansiyeli ortaya çıkarın,
                            sınırlayıcı inançlarınızı dönüştürün ve yaşamınızı pozitif yönde değiştirin.
                        </p>
                        <router-link to="/online-kocluk-ve-atolye" class="btn btn-primary btn-lg">
                            Potansiyelinizi Keşfedin
                        </router-link>
                    </div>
                    <div class="col-lg-6">
                        <img src="/services/bu-hero.jpg" alt="Bilinçaltı Uygulamaları" class="img-fluid" />
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<style scoped>
    .breadcrumb-section {
        background: linear-gradient(135deg, #007bff 0%, #0056b3 100%);
    }

    .hero-section {
        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
    }

    .hero-title {
        font-size: 3rem;
        font-weight: 700;
        margin-bottom: 1.5rem;
        line-height: 1.2;
        color: #2c3e50;
    }

    .hero-description {
        font-size: 1.2rem;
        color: #6c757d;
        margin-bottom: 2rem;
        line-height: 1.6;
    }

    .hero-features {
        display: flex;
        gap: 2rem;
        margin-bottom: 2rem;
        flex-wrap: wrap;
    }

    .feature-item {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-weight: 500;
        color: #495057;
    }

        .feature-item i {
            font-size: 1.2rem;
        }

    .btn-lg {
        padding: 0.75rem 2rem;
        font-size: 1.125rem;
        border-radius: 25px;
        font-weight: 600;
        transition: all 0.3s ease;
    }

        .btn-lg:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 25px rgba(0, 123, 255, 0.3);
        }

    /* Responsive */
    @media (max-width: 768px) {
        .hero-title {
            font-size: 2rem;
        }

        .hero-features {
            flex-direction: column;
            gap: 1rem;
        }
    }
</style>