// =================================================================
// فایل یکپارچه و اصلاح‌شده script.js برای پنل ادمین
// =================================================================

document.addEventListener("DOMContentLoaded", function () {
    console.clear();
    console.log("--- 🏁 شروع اجرای اسکریپت کامل پنل ---");

    const sidebarToggle = document.getElementById('sidebar-toggle');
    const mainWrapper = document.getElementById('main-wrapper');

    // ۱. مدیریت دکمه همبرگری سایدبار و ایجاد اورلی (پوشش تیره موبایل)
    // -------------------------------------------------------------
    if (mainWrapper) {
        // ایجاد داینامیک اورلی در صورتی که در صفحه وجود نداشته باشد
        let overlay = document.querySelector('.sidebar-overlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.classList.add('sidebar-overlay');
            mainWrapper.appendChild(overlay);
        }

        if (sidebarToggle) {
            sidebarToggle.addEventListener('click', function (e) {
                e.preventDefault();
                if (window.innerWidth < 992) {
                    mainWrapper.classList.toggle('sidebar-show');
                } else {
                    mainWrapper.classList.toggle('sidebar-collapsed');
                }
            });
        }

        // بستن سایدبار موبایل با کلیک روی اورلی
        overlay.addEventListener('click', function () {
            mainWrapper.classList.remove('sidebar-show');
        });
    }

    // بستن خودکار سایدبار موبایل در صورت بزرگ شدن صفحه مرورگر
    window.addEventListener('resize', function () {
        if (window.innerWidth >= 992 && mainWrapper) {
            mainWrapper.classList.remove('sidebar-show');
        }
    });

    // ۲. تابع اصلی برای فعال‌سازی آیتم منوی فعال (Active State)
    // -------------------------------------------------------------
    function setActiveMenuItem() {
        const normalizePath = (path) => {
            if (!path) return '';
            let newPath = path.toLowerCase();
            if (newPath.endsWith('/index')) newPath = newPath.slice(0, -6);
            if (newPath.length > 1 && newPath.endsWith('/')) newPath = newPath.slice(0, -1);
            if (newPath === '') return '/';
            return newPath;
        };

        const currentPath = normalizePath(window.location.pathname);
        console.log(`[۱] آدرس نرمال‌شده مرورگر: ${currentPath}`);

        const navLinks = document.querySelectorAll('.sidebar-nav .nav-link');
        let bestMatch = null;
        let maxMatchLength = -1;

        // پاکسازی کلاس‌های قبلی
        navLinks.forEach(link => {
            link.classList.remove('active', 'parent-active');
        });

        // پیدا کردن بهترین تطابق لینک
        navLinks.forEach((link) => {
            if (link.hasAttribute('data-bs-toggle') || !link.href || !link.href.includes('http')) {
                return;
            }

            const linkPath = normalizePath(new URL(link.href).pathname);
            const isMatch = currentPath.startsWith(linkPath);

            if (isMatch && linkPath.length > maxMatchLength) {
                maxMatchLength = linkPath.length;
                bestMatch = link;
            }
        });

        // اعمال کلاس فعال به لینک و والد آن
        if (bestMatch) {
            const matchedPath = normalizePath(new URL(bestMatch.href).pathname);
            console.log(`[۲] بهترین تطابق پیدا شد: ${matchedPath}`);
            bestMatch.classList.add('active');

            const parentSubmenu = bestMatch.closest('.collapse');
            if (parentSubmenu) {
                console.log(`[۳] آیتم در زیرمنو است. باز کردن والد: #${parentSubmenu.id}`);
                parentSubmenu.classList.add('show');

                const parentToggleButton = document.querySelector(`a[data-bs-toggle="collapse"][href="#${parentSubmenu.id}"]`);
                if (parentToggleButton) {
                    parentToggleButton.classList.remove('collapsed');
                    parentToggleButton.classList.add('parent-active');
                    parentToggleButton.setAttribute('aria-expanded', 'true');
                }
            }
        } else {
            console.log("[!] هیچ آیتم منویی با آدرس فعلی مطابقت نداشت.");
        }
    }

    // اجرای تابع فعال‌سازی منو
    setActiveMenuItem();

    // ۳. راه‌اندازی امن دیتاتیبلز (بدون ایجاد خطای تداخل جی‌کوئری)
    // -------------------------------------------------------------
    if (document.getElementById('tblList') && typeof $ !== 'undefined') {
        $('#tblList').DataTable({
            responsive: true,
            "lengthChange": false,
            "pageLength": 10,
            "language": {
                "emptyTable": "هیچ داده‌ای در جدول وجود ندارد",
                "info": "نمایش _START_ تا _END_ از _TOTAL_ ردیف",
                "infoEmpty": "نمایش 0 تا 0 از 0 ردیف",
                "infoFiltered": "(فیلتر شده از مجموع _MAX_ ردیف)",
                "lengthMenu": "نمایش _MENU_ ردیف",
                "loadingRecords": "در حال بارگذاری...",
                "processing": "در حال پردازش...",
                "search": "جستجو:",
                "zeroRecords": "موردی یافت نشد",
                "paginate": {
                    "first": "اولین",
                    "last": "آخرین",
                    "next": "بعدی",
                    "previous": "قبلی"
                }
            }
        });
    }
});
