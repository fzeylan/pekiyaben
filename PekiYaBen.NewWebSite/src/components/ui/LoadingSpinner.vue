<template>
    <div class="loading-overlay">
        <div class="loading-spinner">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Yükleniyor...</span>
            </div>
            <p class="loading-text mt-3">Yükleniyor...</p>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'LoadingSpinner'
    }
</script>

<style scoped>
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(255, 255, 255, 0.9);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
        backdrop-filter: blur(2px);
    }

    .loading-spinner {
        text-align: center;
    }

    .spinner-border {
        width: 3rem;
        height: 3rem;
        border-width: 0.3em;
    }

    .loading-text {
        color: #6c757d;
        font-size: 0.9rem;
        margin: 0;
    }

    /* Animation */
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

    .loading-text {
        animation: pulse 2s infinite;
    }
</style>