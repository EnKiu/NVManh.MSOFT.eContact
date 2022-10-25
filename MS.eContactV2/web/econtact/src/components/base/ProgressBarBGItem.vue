<template>
  <div :title="progress.Name" class="progressbar-bg__content">
    <progress :value="progress.Value" :max="progress.Max"></progress>
    <div class="progressbar-bg__info">
      <span v-if="progress.Message">{{ progress.Message }}</span
      ><span v-else>Đang xử lý {{ progress.Value }}/{{ progress.Max }} tệp</span>
    </div>
  </div>
</template>
<script>
// import Enum from "@/scripts/enum";
export default {
  name: "ProgressBarBG",
  props: ["progress"],
  emits: [],
  created() {
    // this.commonJs.showMessenger({
    //   title: "Thông báo",
    //   msg:
    //     "Quá trình sẽ tiếp tục được xử lý trong ít phút. Hệ thống sẽ thông báo cho bạn khi hoàn thành.",
    //   type: Enum.MsgType.Info,
    //   showCancelButton: false,
    // });
  },
  watch: {
    // progress: {
    //   handler(newValue) {
    //     console.log("runbg:", newValue);
    //   },
    //   deep: true,
    // },
  },
  methods: {},
  data() {
    return {};
  },
};
</script>
<style scoped>
.progressbar-bg__content {
  position: relative;
  color: #fff;
  font-size: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.progressbar-bg__content + .progressbar-bg__content {
  margin-top: 8px;
}
.progressbar-bg__info {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  white-space: nowrap;
}

.progressbar-bg__content progress {
  height: 15px;
  width: 200px;
  border-radius: 0;
}
</style>
