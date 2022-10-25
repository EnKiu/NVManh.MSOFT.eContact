<template>
  <div class="progressbar__content">
    <progress :value="progess.Value" :max="progess.Max"></progress>
    <div class="progressbar__info">
      <span v-if="progess.Message">{{ progess.Message }}</span
      ><span v-else>Đang xử lý {{ progess.Value }}/{{ progess.Max }} tệp</span>
    </div>
  </div>
</template>
<script>
// import Enum from "@/scripts/enum";
export default {
  name: "ProgressBarBG",
  props: ["progess"],
  created() {},
  watch: {
    // progress: {
    //   handler(newValue) {
    //     console.log("runbg:", newValue);
    //   },
    //   deep: true,
    // },
  },
};
</script>
<style scoped>
.progressbar__content {
  position: relative;
  color: #fff;
}

.progressbar__info {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  white-space: nowrap;
}

progress {
  height: 30px;
  width: 200px;
  border-radius: 0;
}
</style>
