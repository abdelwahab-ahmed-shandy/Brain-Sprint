// notifications.js - Managing Notifications and Alerts

document.addEventListener('DOMContentLoaded', function () {
    initNotifications();
    setupNotificationPolling();
});

// Initialize Notifications
function initNotifications() {
    // Update the number of unread notifications
    updateUnreadCount();

    // Set up listeners for click events
    setupNotificationListeners();
}

// Update the number of unread notifications
async function updateUnreadCount() {
    try {
        // You can replace this with an actual AJAX request to the server
        const unreadCount = await fetchUnreadCount();
        const badgeElements = document.querySelectorAll('.notification-badge');

        badgeElements.forEach(badge => {
            if (unreadCount > 0) {
                badge.textContent = unreadCount;
                badge.classList.remove('d-none');
            } else {
                badge.classList.add('d-none');
            }
        });
    } catch (error) {
        console.error('Failed to update notification count:', error);
    }
}

// Set up a poll for automatic updates
function setupNotificationPolling() {
    // Update every minute (60000 milliseconds)
    setInterval(updateUnreadCount, 60000);
}

// Set up listeners for click events
function setupNotificationListeners() {
    // Notifications label in the navigation bar
    const notificationBell = document.getElementById('notificationsDropdown');
    if (notificationBell) {
        notificationBell.addEventListener('click', function (e) {
            if (e.target.closest('.dropdown-toggle')) {
                loadNotificationDropdown();
            }
        });
    }

    // Mark all notifications as read
    const markAllReadBtn = document.querySelector('.mark-all-read');
    if (markAllReadBtn) {
        markAllReadBtn.addEventListener('click', markAllNotificationsAsRead);
    }
}

// Load the notifications list in the dropdown
async function loadNotificationDropdown() {
    try {
        const notifications = await fetchNotifications(5); // Fetch the last 5 notifications
        const dropdown = document.querySelector('.notification-dropdown .dropdown-menu');

        if (notifications.length > 0) {
            let html = '<li><h6 class="dropdown-header">Recent Notifications</h6></li>';

            notifications.forEach(notification => {
                html += `
                    <li>
                        <a class="dropdown-item notification-item ${notification.unread ? 'unread' : ''}" 
                           href="${notification.url}" data-id="${notification.id}">
                            <div class="d-flex">
                                <div class="notification-icon me-3">
                                    <i class="${notification.icon}"></i>
                                </div>
                                <div>
                                    <div class="notification-title">${notification.title}</div>
                                    <small class="text-muted">${notification.time}</small>
                                </div>
                            </div>
                        </a>
                    </li>
                `;
            });

            html += '<li><hr class="dropdown-divider"></li>';
            html += '<li><a class="dropdown-item text-center" href="/notifications">View All Notifications</a></li>';

            dropdown.innerHTML = html;

            // Set up event listeners for new items
            setupNotificationItemListeners();
        }
    } catch (error) {
        console.error('Failed to load notifications:', error);
    }
}

// Setting up listeners for individual item notification events
function setupNotificationItemListeners() {
    const notificationItems = document.querySelectorAll('.notification-item');

    notificationItems.forEach(item => {
        item.addEventListener('click', function (e) {
            if (!e.target.closest('a')) return;

            const notificationId = this.getAttribute('data-id');
            if (this.classList.contains('unread')) {
                markNotificationAsRead(notificationId);
                this.classList.remove('unread');
            }
        });
    });
}

// Mark a single notification as read
async function markNotificationAsRead(notificationId) {
    try {
        // You can replace this with an actual AJAX request to the server
        await mockMarkAsRead(notificationId);
        updateUnreadCount();
    } catch (error) {
        console.error('Failed to mark notification as read:', error);
    }
}

// Mark all notifications as read
async function markAllNotificationsAsRead() {
    try {
        // You can replace this with an actual AJAX request to the server
        await mockMarkAllAsRead();
        updateUnreadCount();

        // Update the interface
        document.querySelectorAll('.notification-item.unread').forEach(item => {
            item.classList.remove('unread');
        });

        // Display a success message 
        showToast('All notifications marked as read', 'success');
    } catch (error) {
        console.error('Failed to mark all notifications as read:', error);
        showToast('Failed to mark notifications as read', 'error');
    }
}

// Display an alert message
function showToast(message, type = 'info') {
    const toastContainer = document.querySelector('.toast-container');
    const toastId = 'toast-' + Date.now();

    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type}`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    toast.id = toastId;

    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">${message}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;
    `;

    toastContainer.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    // Remove the element after disappearing
    toast.addEventListener('hidden.bs.toast', function () {
        toast.remove();
    });
}

// Simulate fetching the number of unread notifications (replace it with an actual AJAX request)
async function fetchUnreadCount() {
    return new Promise(resolve => {
        setTimeout(() => {
            resolve(3); // Default value for display
        }, 300);
    });
}

// simulate fetching notifications (replace it with an actual AJAX request)
async function fetchNotifications(limit = 5) {
    return new Promise(resolve => {
        setTimeout(() => {
            resolve([
                {
                    id: 1,
                    title: 'New course available',
                    message: 'The course "Advanced React" is now available',
                    icon: 'fas fa-book',
                    time: '2 hours ago',
                    unread: true,
                    url: '/courses/advanced-react'
                },
                {
                    id: 2,
                    title: 'Assignment due',
                    message: 'Your assignment for "JavaScript Basics" is due tomorrow',
                    icon: 'fas fa-tasks',
                    time: '1 day ago',
                    unread: true,
                    url: '/assignments/javascript-basics'
                }
            ].slice(0, limit));
        }, 300);
    });
}

// Simulate a notification mark as read (replace it with an actual AJAX request)
async function mockMarkAsRead(notificationId) {
    return new Promise(resolve => {
        setTimeout(() => {
            console.log(`Notification ${ notificationId } marked as read`);
            resolve();
        }, 300);
    });
}

// Simulate all notifications marked as read (replace it with an actual AJAX request)
async function mockMarkAllAsRead() {
    return new Promise(resolve => {
        setTimeout(() => {
            console.log('All notifications marked as read');
            resolve();
        }, 300);
    });
}