<script>
    $(document).ready(function(){
        // Xử lý sự kiện click trên các thẻ a trong navbar-nav
        $('.navbar-nav .nav-link').click(function () {
            // Loại bỏ lớp active khỏi tất cả các thẻ a trong navbar-nav
            $('.navbar-nav .nav-link').removeClass('active');
            // Thêm lớp active cho thẻ a hiện tại
            $(this).addClass('active');
        });
    });
</script>
