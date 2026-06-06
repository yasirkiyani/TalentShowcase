// Custom JavaScript for Talent Showcasing Platform

document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });

    // Search functionality
    const searchInput = document.querySelector('input[type="search"]');
    const searchResults = document.createElement('div');
    searchResults.className = 'search-results';
    
    if (searchInput) {
        searchInput.parentNode.appendChild(searchResults);
        
        searchInput.addEventListener('input', function() {
            const query = this.value.trim();
            if (query.length > 2) {
                performSearch(query);
            } else {
                searchResults.style.display = 'none';
            }
        });
        
        // Hide search results when clicking outside
        document.addEventListener('click', function(e) {
            if (!searchInput.contains(e.target) && !searchResults.contains(e.target)) {
                searchResults.style.display = 'none';
            }
        });
    }

    // Upload area drag and drop
    const uploadArea = document.querySelector('.upload-area');
    if (uploadArea) {
        uploadArea.addEventListener('dragover', function(e) {
            e.preventDefault();
            this.classList.add('dragover');
        });
        
        uploadArea.addEventListener('dragleave', function(e) {
            e.preventDefault();
            this.classList.remove('dragover');
        });
        
        uploadArea.addEventListener('drop', function(e) {
            e.preventDefault();
            this.classList.remove('dragover');
            handleFiles(e.dataTransfer.files);
        });
        
        uploadArea.addEventListener('click', function() {
            const fileInput = document.createElement('input');
            fileInput.type = 'file';
            fileInput.accept = 'video/*';
            fileInput.multiple = true;
            fileInput.click();
            
            fileInput.addEventListener('change', function() {
                handleFiles(this.files);
            });
        });
    }

    // Like button functionality
    document.querySelectorAll('.like-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const icon = this.querySelector('i');
            const count = this.querySelector('.like-count');
            
            if (icon.classList.contains('bi-heart')) {
                icon.classList.remove('bi-heart');
                icon.classList.add('bi-heart-fill');
                icon.style.color = '#dc3545';
                if (count) {
                    count.textContent = parseInt(count.textContent) + 1;
                }
            } else {
                icon.classList.remove('bi-heart-fill');
                icon.classList.add('bi-heart');
                icon.style.color = '';
                if (count) {
                    count.textContent = parseInt(count.textContent) - 1;
                }
            }
        });
    });

    // Follow button functionality
    document.querySelectorAll('.follow-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            if (this.textContent.trim() === 'Follow') {
                this.textContent = 'Following';
                this.classList.remove('btn-outline-primary');
                this.classList.add('btn-primary');
            } else {
                this.textContent = 'Follow';
                this.classList.remove('btn-primary');
                this.classList.add('btn-outline-primary');
            }
        });
    });

    // Comment submission
    const commentForm = document.querySelector('.comment-form');
    if (commentForm) {
        commentForm.addEventListener('submit', function(e) {
            e.preventDefault();
            const textarea = this.querySelector('textarea');
            const commentText = textarea.value.trim();
            
            if (commentText) {
                addComment(commentText);
                textarea.value = '';
            }
        });
    }

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Navbar scroll effect
    const navbar = document.querySelector('.navbar');
    if (navbar) {
        window.addEventListener('scroll', function() {
            if (window.scrollY > 50) {
                navbar.classList.add('navbar-scrolled');
            } else {
                navbar.classList.remove('navbar-scrolled');
            }
        });
    }

    // Lazy loading for images
    const lazyImages = document.querySelectorAll('img[data-src]');
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });

    lazyImages.forEach(img => imageObserver.observe(img));

    // Video player controls
    const videoPlayers = document.querySelectorAll('video');
    videoPlayers.forEach(video => {
        video.addEventListener('play', function() {
            // Pause other videos when this one starts playing
            videoPlayers.forEach(v => {
                if (v !== video && !v.paused) {
                    v.pause();
                }
            });
        });
    });

    // Form validation
    const forms = document.querySelectorAll('.needs-validation');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Notification system
    window.showNotification = function(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
        notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
        notification.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        document.body.appendChild(notification);
        
        setTimeout(() => {
            notification.remove();
        }, 5000);
    };

    // Loading spinner
    window.showLoader = function() {
        const loader = document.createElement('div');
        loader.id = 'page-loader';
        loader.className = 'position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center';
        loader.style.cssText = 'background: rgba(255,255,255,0.9); z-index: 9999;';
        loader.innerHTML = '<div class="spinner"></div>';
        document.body.appendChild(loader);
    };

    window.hideLoader = function() {
        const loader = document.getElementById('page-loader');
        if (loader) {
            loader.remove();
        }
    };

    // Initialize animations on scroll
    const animateOnScroll = function() {
        const elements = document.querySelectorAll('.fade-in, .slide-in');
        elements.forEach(element => {
            const elementTop = element.getBoundingClientRect().top;
            const elementBottom = element.getBoundingClientRect().bottom;
            
            if (elementTop < window.innerHeight && elementBottom > 0) {
                element.classList.add('animate');
            }
        });
    };

    window.addEventListener('scroll', animateOnScroll);
    animateOnScroll(); // Initial check
});

// Search functionality
function performSearch(query) {
    // Mock search results - replace with actual API call
    const mockResults = [
        { title: 'John Doe - Guitar Performance', type: 'video', url: '#' },
        { title: 'Jane Smith - Dance Tutorial', type: 'video', url: '#' },
        { title: 'Music Community', type: 'community', url: '#' },
        { title: 'Art Contest 2024', type: 'contest', url: '#' }
    ];
    
    const searchResults = document.querySelector('.search-results');
    const filteredResults = mockResults.filter(result => 
        result.title.toLowerCase().includes(query.toLowerCase())
    );
    
    if (filteredResults.length > 0) {
        searchResults.innerHTML = filteredResults.map(result => `
            <div class="search-result-item" onclick="window.location.href='${result.url}'">
                <i class="bi bi-${getIconForType(result.type)} me-2"></i>
                ${result.title}
            </div>
        `).join('');
        searchResults.style.display = 'block';
    } else {
        searchResults.innerHTML = '<div class="search-result-item">No results found</div>';
        searchResults.style.display = 'block';
    }
}

function getIconForType(type) {
    const icons = {
        video: 'play-circle',
        community: 'people',
        contest: 'trophy',
        user: 'person'
    };
    return icons[type] || 'file-earmark';
}

// File upload handler
function handleFiles(files) {
    const uploadProgress = document.querySelector('.upload-progress');
    const progressBar = document.querySelector('.progress-bar-custom');
    
    if (files.length > 0) {
        showLoader();
        
        // Simulate file upload progress
        let progress = 0;
        const interval = setInterval(() => {
            progress += Math.random() * 30;
            if (progress >= 100) {
                progress = 100;
                clearInterval(interval);
                hideLoader();
                showNotification('Files uploaded successfully!', 'success');
            }
            
            if (progressBar) {
                progressBar.style.width = progress + '%';
            }
        }, 500);
        
        // Display file names
        const fileList = document.querySelector('.file-list');
        if (fileList) {
            fileList.innerHTML = Array.from(files).map(file => `
                <div class="file-item d-flex justify-content-between align-items-center p-2 border-bottom">
                    <span><i class="bi bi-file-earmark-play me-2"></i>${file.name}</span>
                    <span class="text-muted">${formatFileSize(file.size)}</span>
                </div>
            `).join('');
        }
    }
}

function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}

// Add comment function
function addComment(text) {
    const commentsContainer = document.querySelector('.comments-container');
    const commentHtml = `
        <div class="comment fade-in">
            <div class="d-flex">
                <img src="https://picsum.photos/seed/user${Date.now()}/40/40" alt="User" class="rounded-circle me-3" width="40" height="40">
                <div class="flex-grow-1">
                    <div class="d-flex justify-content-between">
                        <h6 class="mb-1">Current User</h6>
                        <small class="text-muted">Just now</small>
                    </div>
                    <p class="mb-0">${text}</p>
                    <div class="mt-2">
                        <button class="btn btn-sm btn-link text-muted">
                            <i class="bi bi-heart"></i> Like
                        </button>
                        <button class="btn btn-sm btn-link text-muted">
                            <i class="bi bi-reply"></i> Reply
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    if (commentsContainer) {
        commentsContainer.insertAdjacentHTML('afterbegin', commentHtml);
    }
}

// Share functionality
window.shareContent = function(url, title) {
    if (navigator.share) {
        navigator.share({
            title: title,
            url: url
        }).catch(err => console.log('Error sharing:', err));
    } else {
        // Fallback - copy to clipboard
        navigator.clipboard.writeText(url).then(() => {
            showNotification('Link copied to clipboard!', 'success');
        });
    }
};

// Report content
window.reportContent = function(contentId) {
    if (confirm('Are you sure you want to report this content?')) {
        showNotification('Content reported. Thank you for helping keep our community safe.', 'info');
    }
};

// Filter functionality
window.applyFilters = function() {
    const filters = {
        category: document.querySelector('#categoryFilter')?.value,
        sortBy: document.querySelector('#sortBy')?.value,
        dateRange: document.querySelector('#dateRange')?.value
    };
    
    showLoader();
    
    // Simulate applying filters
    setTimeout(() => {
        hideLoader();
        showNotification('Filters applied successfully!', 'success');
    }, 1000);
};
