// wwwroot/js/toast.js
(function () {
    'use strict';

    // Auto-dismiss toasts after 4 seconds
    document.addEventListener('DOMContentLoaded', function () {
        const toasts = document.querySelectorAll('.toast');
        toasts.forEach(function (toast) {
            setTimeout(function () {
                toast.style.opacity = '0';
                toast.style.transform = 'translateY(20px)';
                toast.style.transition = 'all 0.4s ease';
                setTimeout(() => toast.remove(), 400);
            }, 4000);
        });
    });

    // Global showToast function
    window.showToast = function (message, type = 'success') {
        const existing = document.querySelectorAll('.toast');
        existing.forEach(t => t.remove());

        const toast = document.createElement('div');
        toast.className = `toast toast-${type} animate-slide-up`;
        toast.innerHTML = type === 'success'
            ? `✅ ${message}`
            : `❌ ${message}`;

        document.body.appendChild(toast);

        setTimeout(function () {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(20px)';
            toast.style.transition = 'all 0.4s ease';
            setTimeout(() => toast.remove(), 400);
        }, 4000);
    };
})();