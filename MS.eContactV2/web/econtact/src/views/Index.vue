<template>
  <div class="page-not-authenticate">
    <div class="content">
      <h1>Thông báo</h1>
      <p class="content__text">
        Kể từ 10/10/2022, do vấn đề bảo mật thông tin lớp và của các cá nhân, để có thể sử dụng đầy đủ các tính năng xin vui lòng nhấn <router-link to="/login"><b>đăng nhập</b>.</router-link>
      </p>
      <p class="content__text">
        Nếu bạn chưa có tài khoản xin vui lòng bỏ ra 30s để <router-link to="/register"><b>Tạo tài khoản</b>.</router-link>.
      </p>
      <p class="content__text">
        Website chính thức bổ sung thêm các tính năng, các bạn có thể:
        <ul>
          <li>Tra cứu số điện thoại của nhau.</li>
          <li>Cập nhật số điện thoại của cá nhân.</li>
          <li>Xem/ Đăng ký tham gia các sự kiện của lớp.</li>
          <li>Xem ảnh hoạt động lớp theo các album của cá nhân upload lên.</li>
        </ul>
      </p>
      <p class="content__text">
        Xin vui lòng yên tâm sử dụng Website vì nó được thiết kế bởi Mr Mạnh - an toàn và 100% không có virut.
      </p>
      <p class="content__text">
        Nếu các bạn có bất cứ góp ý nào xin vui lòng nói trực tiếp với Mr Mạnh (0961179969).
      </p>
      <p class="content__text">
        Cám ơn và xin lỗi các bạn vì sự bất tiện này!
      </p>
      <div class="page-button">
        <router-link to="/login" class="btn btn--default btn--link">Đăng nhập</router-link>
        <div> hoặc <router-link to="/register"><b>Tạo tài khoản</b>.</router-link></div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "HomePage",
  props:["showHomePage"]
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
    padding: 0 16px 16px;
    background-color: #fff;
    border-radius: 4px;
}
.page-button{
  display:flex;
  flex-direction: column;
  justify-content: flex-end;
}

.btn--link{
  display: flex;
  align-items: center;
  justify-content: center;
  text-decoration: none;
  margin-bottom: 10px;
}
</style>