// site.js - the main file for the site's general functionality

// Initialize the site when the page loads
document.addEventListener('DOMContentLoaded', function () {
    // Initialize general elements
    initTooltips();
    initModals();
    handleScrollEvents();
    setupBackToTopButton();
});

// Initialize Tooltips
function initTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize Popup Windows (Modals)
function initModals() {
    const myModal = document.getElementById('myModal');
    if (myModal) {
        myModal.addEventListener('show.bs.modal', function (event) {
            // You can add additional functions when the popup appears
        });
    }
}

// Handling Scroll Events
function handleScrollEvents() {
    window.addEventListener('scroll', function () {
        // You can add functions when scrolling
    });
}

// Back to Top Button
function setupBackToTopButton() {
    const backToTopButton = document.getElementById('backToTop');
    if (backToTopButton) {
        window.addEventListener('scroll', function () {
            if (window.pageYOffset > 300) {
                backToTopButton.classList.add('show');
            } else {
                backToTopButton.classList.remove('show');
            }
        });

        backToTopButton.addEventListener('click', function (e) {
            e.preventDefault();
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }
}

//General function for loading dynamic content
async function loadDynamicContent(url, containerId) {
    try {
        const response = await fetch(url);
        const content = await response.text();
        document.getElementById(containerId).innerHTML = content;
    } catch (error) {
        console.error('Error loading content:', error);
    }
}

