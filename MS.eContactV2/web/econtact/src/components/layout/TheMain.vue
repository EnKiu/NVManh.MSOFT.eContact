<template>
  <div class="main">
    <div class="main-container">
      <!-- <router-view :key="$route.fullPath"></router-view> -->
      <router-view></router-view>
    </div>
  </div>
</template>
<script>
export default {};
</script>

<style scoped>
.main {
  margin-top: 85px;
  width: 100%;
  height: calc(100vh - 85px);
  display: flex;
  justify-content: center;
  padding: 16px;
  box-sizing: border-box;
  overflow-y: auto;
  background-color: #fff;
  z-index: 99;
}

.main-container {
  max-width: 700px;
  height: 100%;
  position: relative;
}
</style>
