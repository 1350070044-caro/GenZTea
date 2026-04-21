// Quick Add to Cart
document.addEventListener('DOMContentLoaded', function() {
    const addButtons = document.querySelectorAll('.add-to-cart-btn');
    addButtons.forEach(btn => {
        btn.addEventListener('click', function() {
            const id = this.dataset.id;
            fetch(`/Cart/AddToCart?id=${id}&quantity=1`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                }
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        title: 'Thành công!',
                        text: 'Đã thêm vào giỏ hàng',
                        icon: 'success',
                        timer: 1500
                    });
                    // Update cart count in navbar
                    location.reload(); // Simple way
                }
            })
            .catch(error => {
                Swal.fire('Lỗi!', 'Không thể thêm món', 'error');
            });
        });
    });
    
    // Navbar scroll effect
    window.addEventListener('scroll', function() {
        const navbar = document.querySelector('.navbar-custom');
        if (window.scrollY > 50) {
            navbar.classList.add('navbar-scrolled');
        } else {
            navbar.classList.remove('navbar-scrolled');
        }
    });
    
    // Search functionality
    const searchInput = document.querySelector('.search-input');
    if (searchInput) {
        searchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                const query = this.value;
                window.location.href = `/Product?search=${encodeURIComponent(query)}`;
            }
        });
    }
});
