<template>
  <div class="loading">
    <div class="loading-wrapper">
      <div class="loading__icon"></div>
      <div class="loading__text">Đang xử lý, vui lòng đợi...</div>
    </div>
  </div>
</template>
<style scoped>
.loading {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.402);
  z-index: 999;
}

.loading-wrapper {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.loading__icon {
  width: 24px;
  height: 24px;
  border-top: 8px solid #0062CC;
  border-right: 8px solid #0062CC;
  border-left: 8px solid #ffffff;
  border-bottom: 8px solid #ffffff;
  border-radius: 50%;
  -webkit-animation: spin 500ms linear infinite;
  animation: spin 500ms linear infinite;
}

.loading__text {
  color: #ffffff;
  margin-top: 8px;
}

@-webkit-keyframes spin {
  0% {
    -webkit-transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
  }
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>