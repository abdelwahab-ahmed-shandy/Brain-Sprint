// theme.js - Dark/Light Theme Management

// Initialize the theme on page load
document.addEventListener('DOMContentLoaded', function () {
    initTheme();
});

// Initialize the theme based on saved preferences
function initTheme() {
    const savedTheme = localStorage.getItem('theme') || 'auto';
    setTheme(savedTheme);

    // Update theme controls
    updateThemeControls(savedTheme);

}

// Change the theme
function setTheme(theme) {
    const htmlElement = document.documentElement;

    if (theme === 'auto') {
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        htmlElement.setAttribute('data-bs-theme', prefersDark ? 'dark' : 'light');
    } else {
        htmlElement.setAttribute('data-bs-theme', theme);
    }

    // Save preferences
    localStorage.setItem('theme', theme);

    // Send an event to update other components
    document.dispatchEvent(new CustomEvent('themeChanged', { detail: theme }));
}

// Update theme controls
function updateThemeControls(theme) {
    const themeSwitchers = document.querySelectorAll('.theme-switcher');

    themeSwitchers.forEach(switcher => {
        switcher.value = theme;
        switcher.addEventListener('change', function () {
            setTheme(this.value);
        });
    });
}

// Listen for system preference changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    if (localStorage.getItem('theme') === 'auto') {
        setTheme('auto');
    }
});

// You can access these functions from anywhere
window.ThemeManager = {
    setTheme,
    getCurrentTheme: () => localStorage.getItem('theme') || 'auto'
};