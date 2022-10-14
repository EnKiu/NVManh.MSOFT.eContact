<template>
  <div class="page-title" style="">Website nội bộ cựu học sinh A1 (2004-2007) - trường thpt tứ sơn</div>
  <nav class="navbar">
    <div class="logo"></div>
    <div class="navbar-list">
        <router-link to="/contacts" class="navbar-item">
            <span class="navbar-item__text"><i class="icofont-contacts"></i> Danh bạ</span>
        </router-link>
         <router-link to="/events" class="navbar-item">
            <span class="navbar-item__text"><i class="icofont-history"></i> Sự kiện</span>
        </router-link>
         <router-link to="/pictures" class="navbar-item">
            <span class="navbar-item__text"><i class="icofont-image"></i> Kho ảnh</span>
        </router-link>
        <router-link v-if="isAuthenticated" to="/account" class="navbar-item">
            <span class="navbar-item__text"><i class="icofont-user-alt-3"></i> Tài khoản</span>
        </router-link>
        <router-link v-else to="/login" class="navbar-item">
            <span class="navbar-item__text"><i class="icofont-login"></i> Đăng nhập</span>
        </router-link>
    </div>
  </nav>
</template>
<script>
export default {
    name:"TheHeader",
    props:["isAuthenticated"]
};
</script>

<style scoped>
.page-title{
    position: fixed;
    top:0;
    width: 100%;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: black;
    color: #fff;
    text-align: center;
    text-transform: uppercase;
    font-weight: 700;
}
.navbar{
    position: fixed;
    top: 40px;
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    height: 40px;
    background-color: #000;
    color: #fff;
    border-top: 1px solid #fff;
}
.navbar-list{
    height: 100%;
    display: flex;
    align-items: center;
}
.navbar-item{
 height: 100%;
 padding: 0 16px;
 display: flex;
 align-items: center;
 justify-content: center;
 font-weight: 700;
}

.navbar-item i{
    font-size: 16px;
    margin-right: 5px;
}
.router-link-active{
    background-color: #656565cc;
}
.navbar a{
    color: #fff;
    text-decoration: none;
}
</style>
