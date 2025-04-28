// search.js - site search functionality

document.addEventListener('DOMContentLoaded', function () {
    const searchForm = document.querySelector('.search-form');
    const searchInput = document.getElementById('globalSearch');
    const searchResults = document.querySelector('.search-results');

    if (searchForm && searchInput && searchResults) {
        setupSearch();
    }
});

function setupSearch() {
    const searchInput = document.getElementById('globalSearch');
    const searchResults = document.querySelector('.search-results');

    //Search while typing 
    searchInput.addEventListener('input', debounce(handleSearch, 300));

    // Close search results when clicked outside them
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.search-form')) {
            searchResults.classList.add('d-none');
        }
    });

    // Show/hide search results when focus is on the search field
    searchInput.addEventListener('focus', function () {
        if (this.value.trim() !== '') {
            searchResults.classList.remove('d-none');
        }
    });
}

// Process the search
async function handleSearch() {
    const query = this.value.trim();
    const searchResults = document.querySelector('.search-results');

    if (query === '') {
        searchResults.classList.add('d-none'); return;
    }

    try {
        // You can replace this with an actual AJAX request to the server
        const results = await mockSearch(query);

        if (results.length > 0) {
            renderSearchResults(results);
            searchResults.classList.remove('d-none');
        } else {
            renderNoResults();
            searchResults.classList.remove('d-none');
        }
    } catch (error) {
        console.error('Search error:', error);
        renderSearchError();
    }
}

// Display search results
function renderSearchResults(results) {
    const searchResults = document.querySelector('.search-results');
    let html = '';

    results.forEach(result => {
        html += ` 
<a href="${result.url}" class="dropdown-item search-result-item"> 
<div class="d-flex align-items-center"> 
<div class="me-3"> 
<i class="${result.icon}"></i> 
</div> 
<div> 
<div class="fw-bold">${result.title}</div> 
<small class="text-muted">${result.category}</small> 
</div> 
</div> 
</a> 
`;
    });

    searchResults.innerHTML = html;
}

// Display a message of no results
function renderNoResults() {
    const searchResults = document.querySelector('.search-results');
    searchResults.innerHTML = ` 
<div class="dropdown-item text-muted"> 
No results found for "<strong>${this.value.trim()}</strong>" 
</div> 
`;
}

// Display an error message
function renderSearchError() {
    const searchResults = document.querySelector('.search-results');
    searchResults.innerHTML = ` 
<div class="dropdown-item text-danger"> 
Error occurred while searching. Please try again. 
</div> 
`;
}

// Mock Search (Replace it with an actual AJAX request)
async function mockSearch(query) {
    // This is just a mock - replace it with an actual API call
    return new Promise(resolve => {
        setTimeout(() => {
            const mockResults = [
                {
                    title: 'Introduction to JavaScript',
                    category: 'Programming Course',
                    icon: 'fas fa-book',
                    url: '/courses/javascript-intro'
                },
                {
                    title: 'Advanced CSS Techniques',
                    category: 'Web Design Course',
                    icon: 'fas fa-laptop-code',
                    url: '/courses/advanced-css'
                }
            ].filter(item =>
                item.title.toLowerCase().includes(query.toLowerCase()) ||
                item.category.toLowerCase().includes(query.toLowerCase())
            );

            resolve(mockResults);
        }, 300);
    });
}

// Function to delay requests during write
function debounce(func, wait) {
    let timeout;
    return function () {
        const context = this, args = arguments;
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            func.apply(context, args);
        }, wait);
    };
}