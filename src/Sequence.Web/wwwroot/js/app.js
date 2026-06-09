// wwwroot/js/app.js
(function ($) {
    'use strict';

    // ── Global AJAX setup ──────────────────────────────────────────────
    $.ajaxSetup({
        beforeSend: function (xhr) {
            const token = $('input[name="__RequestVerificationToken"]').val();
            if (token) {
                xhr.setRequestHeader('RequestVerificationToken', token);
            }
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
        }
    });

    // ── Loading State ──────────────────────────────────────────────────
    $(document).on('submit', 'form', function () {
        const submitBtn = $(this).find('[type="submit"]');
        if (submitBtn.length && !submitBtn.prop('disabled')) {
            submitBtn.prop('disabled', true);
            const originalText = submitBtn.html();
            submitBtn.data('original-text', originalText);
            submitBtn.html('<span class="inline-block animate-spin mr-2">⏳</span> در حال ارسال...');

            // Re-enable after 10s as safety net
            setTimeout(() => {
                submitBtn.prop('disabled', false);
                submitBtn.html(originalText);
            }, 10000);
        }
    });

    // ── Confirmation dialogs ───────────────────────────────────────────
    $(document).on('click', '[data-confirm]', function (e) {
        const message = $(this).data('confirm');
        if (!confirm(message)) {
            e.preventDefault();
            e.stopPropagation();
        }
    });

    // ── Image lazy loading fallback ────────────────────────────────────
    $(document).on('error', 'img', function () {
        $(this).closest('.movie-card').find('.movie-card-poster').replaceWith(
            '<div class="aspect-[2/3] bg-cinema-surface flex items-center justify-center"><span class="text-4xl">🎬</span></div>'
        );
    });

    // ── Auto-resize textareas ──────────────────────────────────────────
    $('textarea').each(function () {
        this.setAttribute('style', 'height:' + (this.scrollHeight) + 'px;overflow-y:hidden;');
    }).on('input', function () {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });

    // ── RTL number conversion ──────────────────────────────────────────
    window.toPersianNumber = function (num) {
        const persianDigits = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
        return String(num).replace(/[0-9]/g, d => persianDigits[d]);
    };

    // ── Dark mode (always on for this app) ────────────────────────────
    document.documentElement.classList.add('dark');

    console.log('🎬 سکانس initialized');

})(jQuery);