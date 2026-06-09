// tailwind.config.js
/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Views/**/*.cshtml",
        "./wwwroot/js/**/*.js"
    ],
    darkMode: 'class',
    theme: {
        extend: {
            fontFamily: {
                'persian': ['Vazirmatn', 'IRANSans', 'Tahoma', 'sans-serif'],
            },
            colors: {
                cinema: {
                    dark: '#0a0a0f',
                    card: '#12121a',
                    border: '#1e1e2e',
                    accent: '#f5a623',
                    gold: '#ffd700',
                    red: '#e53935',
                    surface: '#1a1a2e',
                    text: '#e0e0e0',
                    muted: '#8888aa',
                }
            },
            animation: {
                'fade-in': 'fadeIn 0.3s ease-in-out',
                'slide-up': 'slideUp 0.4s ease-out',
                'pulse-star': 'pulseStar 0.2s ease-in-out',
            },
            keyframes: {
                fadeIn: {
                    '0%': { opacity: '0' },
                    '100%': { opacity: '1' }
                },
                slideUp: {
                    '0%': { transform: 'translateY(20px)', opacity: '0' },
                    '100%': { transform: 'translateY(0)', opacity: '1' }
                },
                pulseStar: {
                    '0%, 100%': { transform: 'scale(1)' },
                    '50%': { transform: 'scale(1.3)' }
                }
            }
        },
    },
    plugins: [],
}