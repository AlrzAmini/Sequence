// wwwroot/js/rating.js
(function () {
    'use strict';

    const ratingLabels = {
        1: '۱ — ضعیف',
        2: '۲ — بد',
        3: '۳ — متوسط رو به پایین',
        4: '۴ — متوسط',
        5: '۵ — قابل قبول',
        6: '۶ — خوب',
        7: '۷ — خوب بود',
        8: '۸ — عالی',
        9: '۹ — شاهکار',
        10: '۱۰ — کامل!'
    };

    document.addEventListener('DOMContentLoaded', function () {
        const starContainers = document.querySelectorAll('.star-rating');

        starContainers.forEach(function (container) {
            const stars = container.querySelectorAll('.star');
            const hiddenInput = document.getElementById('PersonalRating');
            const display = document.getElementById('ratingDisplay');

            if (!hiddenInput) return;

            // Restore saved value
            const savedValue = parseInt(hiddenInput.value) || 0;
            if (savedValue > 0) {
                updateStars(stars, savedValue);
                if (display) display.textContent = ratingLabels[savedValue] || '';
            }

            stars.forEach(function (star) {
                const value = parseInt(star.dataset.value);

                // Hover
                star.addEventListener('mouseenter', function () {
                    updateStars(stars, value, true);
                    if (display) display.textContent = ratingLabels[value] || '';
                });

                // Mouse leave — restore to selected
                star.addEventListener('mouseleave', function () {
                    const selected = parseInt(hiddenInput.value) || 0;
                    updateStars(stars, selected);
                    if (display) display.textContent = selected > 0 ? (ratingLabels[selected] || '') : '';
                });

                // Click
                star.addEventListener('click', function () {
                    hiddenInput.value = value;
                    updateStars(stars, value);
                    if (display) {
                        display.textContent = ratingLabels[value] || '';
                    }

                    // Pulse animation
                    star.classList.add('animate-pulse-star');
                    setTimeout(() => star.classList.remove('animate-pulse-star'), 200);
                });
            });
        });
    });

    function updateStars(stars, value, isHover = false) {
        stars.forEach(function (s) {
            const sVal = parseInt(s.dataset.value);
            if (sVal <= value) {
                s.classList.add('active');
                s.style.color = '#ffd700';
            } else {
                s.classList.remove('active');
                s.style.color = '';
            }
        });
    }
})();