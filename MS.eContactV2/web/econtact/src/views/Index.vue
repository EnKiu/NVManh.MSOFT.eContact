<template>
  <div class="page-not-authenticate">
    <div class="content">
      Đây là website nội bộ dành cho cựu học sinh lớp 10A1, 11A1, 12A1 trường
      THPT Tứ Sơn. Mọi thông tin về nhóm là bảo mật và chỉ dành cho các thành
      viên nội bộ. Nếu bạn muốn sử dụng các tính năng như xem danh bạ, đăng ký
      tham gia sự kiện, xem ảnh và các hoạt động khác. Xin vui lòng Đăng nhập hệ
      thống! Xin lỗi vì sự bất tiện này! Thành viên A1, xin vui lòng Đăng nhập,
      hoặc nếu bạn chưa có tài khoản xin hãy Tạo tài khoản.
    </div>
  </div>
</template>
<script>
export default {
  name: "HomePage",
};
</script>
<style scoped>
.page-not-authenticate{
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background-color: #000;
    z-index: 9999;
    display: flex;
    align-items: center;
    justify-content: center;
}

.content{
    max-width: 300px;
    margin: 24px;
    background-color: #fff;
    border-radius: 4px;
}
</style>