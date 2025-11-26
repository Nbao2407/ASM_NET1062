/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#E11D48', // Rose 600 - Appetite stimulation
          hover: '#BE123C',   // Rose 700 - Hover state
          light: '#FDA4AF',   // Rose 300 - Backgrounds
          dark: '#9F1239',    // Rose 800 - Deep accents
        },
        secondary: {
          DEFAULT: '#F59E0B', // Amber 500 - Warmth, value
          hover: '#D97706',   // Amber 600 - Hover state
          light: '#FCD34D',   // Amber 300 - Highlights
          dark: '#B45309',    // Amber 700 - Deep accents
        },
        accent: {
          success: '#10B981',  // Emerald 500 - Healthy, delivered
          warning: '#F97316',  // Orange 500 - Urgency, limited
          info: '#3B82F6',     // Blue 500 - Information
          danger: '#EF4444',   // Red 500 - Errors, remove
        },
        dark: {
          DEFAULT: '#1F2937', // Gray 800 - Primary text
          light: '#374151',   // Gray 700 - Secondary text
          lighter: '#6B7280', // Gray 500 - Tertiary text
        },
        light: {
          DEFAULT: '#F9FAFB', // Gray 50 - Page background
          hover: '#F3F4F6',   // Gray 100 - Hover, dividers
          border: '#E5E7EB',  // Gray 200 - Borders
        }
      },
      fontFamily: {
        sans: ['Inter', 'sans-serif'],
        heading: ['Outfit', 'sans-serif'],
      },
      // 8px Base Grid System
      spacing: {
        'xs': '4px',    // 8 × 0.5
        'sm': '8px',    // 8 × 1
        'md': '12px',   // 8 × 1.5
        'base': '16px', // 8 × 2
        'lg': '24px',   // 8 × 3
        'xl': '32px',   // 8 × 4
        'xxl': '48px',  // 8 × 6
        '3xl': '64px',  // 8 × 8
        '4xl': '80px',  // 8 × 10
        '5xl': '96px',  // 8 × 12
        '6xl': '128px', // 8 × 16
      },
      fontSize: {
        'xs': ['12px', { lineHeight: '16px' }],      // Labels, fine print
        'sm': ['14px', { lineHeight: '20px' }],      // Small text, descriptions
        'base': ['16px', { lineHeight: '24px' }],    // Body text
        'lg': ['20px', { lineHeight: '28px' }],      // Card titles
        'xl': ['24px', { lineHeight: '32px' }],      // Section titles
        '2xl': ['28px', { lineHeight: '36px' }],     // Large headings
        '3xl': ['32px', { lineHeight: '40px' }],     // Page titles (mobile)
        '4xl': ['36px', { lineHeight: '44px' }],     // Section titles (desktop)
        '5xl': ['48px', { lineHeight: '56px' }],     // Hero titles (mobile)
        '6xl': ['56px', { lineHeight: '64px' }],     // Hero titles (desktop)
        '7xl': ['64px', { lineHeight: '72px' }],     // Extra large headlines
      },
      lineHeight: {
        'tight': '1.2',    // Headlines
        'normal': '1.5',   // Descriptions, body
        'relaxed': '1.6',  // Body text
      },
      borderRadius: {
        'xs': '4px',
        'sm': '8px',
        'base': '12px',
        'lg': '16px',
        'xl': '20px',
        '2xl': '24px',
        'full': '9999px',
      },
      aspectRatio: {
        'food': '4 / 3',    // Food items
        'combo': '16 / 9',  // Combo deals
        'hero': '2 / 1',    // Hero sections
        'square': '1 / 1',  // Icons, avatars
      },
      boxShadow: {
        'xs': '0 1px 2px 0 rgba(0, 0, 0, 0.05)',
        'sm': '0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)',
        'base': '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
        'md': '0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
        'lg': '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
        'xl': '0 25px 50px -12px rgba(0, 0, 0, 0.25)',
        'inner': 'inset 0 2px 4px 0 rgba(0, 0, 0, 0.06)',
        'glow': '0 0 15px rgba(225, 29, 72, 0.3)',
        'glow-lg': '0 0 30px rgba(225, 29, 72, 0.4)',
      },
      transitionDuration: {
        '200': '200ms',
        '300': '300ms',
        '500': '500ms',
        '700': '700ms',
      },
      transitionTimingFunction: {
        'bounce-in': 'cubic-bezier(0.68, -0.55, 0.265, 1.55)',
        'smooth': 'cubic-bezier(0.4, 0, 0.2, 1)',
      },
      keyframes: {
        'bounce-subtle': {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%': { transform: 'translateY(-4px)' },
        },
        'pulse-slow': {
          '0%, 100%': { opacity: '1' },
          '50%': { opacity: '0.7' },
        },
        'shake': {
          '0%, 100%': { transform: 'translateX(0)' },
          '25%': { transform: 'translateX(-4px)' },
          '75%': { transform: 'translateX(4px)' },
        },
        'slide-up': {
          '0%': { transform: 'translateY(10px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        'slide-down': {
          '0%': { transform: 'translateY(-10px)', opacity: '0' },
          '100%': { transform: 'translateY(0)', opacity: '1' },
        },
        'scale-in': {
          '0%': { transform: 'scale(0.9)', opacity: '0' },
          '100%': { transform: 'scale(1)', opacity: '1' },
        },
      },
      animation: {
        'bounce-subtle': 'bounce-subtle 2s ease-in-out infinite',
        'pulse-slow': 'pulse-slow 3s ease-in-out infinite',
        'shake': 'shake 0.5s ease-in-out',
        'slide-up': 'slide-up 0.3s ease-out',
        'slide-down': 'slide-down 0.3s ease-out',
        'scale-in': 'scale-in 0.2s ease-out',
      },
      backdropBlur: {
        'xs': '2px',
        'sm': '4px',
        'md': '8px',
        'lg': '12px',
      },
      zIndex: {
        '60': '60',
        '70': '70',
        '80': '80',
        '90': '90',
        '100': '100',
      },
    },
  },
  plugins: [],
}
