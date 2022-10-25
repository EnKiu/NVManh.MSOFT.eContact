<template>
  <div v-if="progressNotBGs.length > 0" class="progressbar">
    <div class="loading-progressbar">
      <div class="loading__icon"><i class="icofont-spinner-alt-1"></i></div>
    </div>
    <ProgressBarNoBG
      v-for="progress in progressNotBGs"
      :progess="progress"
      :key="progress.Id"
    ></ProgressBarNoBG>
  </div>
  <div v-if="progressBGs.length > 0" class="progressbar-bg">
    <ProgressBarBG
      v-for="progress in progressBGs"
      :progress="progress"
      :key="progress.Id"
    ></ProgressBarBG>
  </div>
</template>
<script>
import commonJs from "@/scripts/common";
import Enum from "@/scripts/enum";
import ProgressBarBG from "./ProgressBarBGItem.vue";
import ProgressBarNoBG from "./ProgressBarNoBgItem.vue";
export default {
  name: "ProgressBar",
  components: { ProgressBarBG, ProgressBarNoBG },
  props: ["processList"],
  emits: [],
  created() {
    this.progressBGs = this.processList.filter((e) => e.RunBackground == true);
    this.progressNotBGs = this.processList.filter((e) => !e.RunBackground);
  },
  watch: {
    processList: {
      handler(newValue) {
        this.progressBGs = newValue.filter((e) => e.RunBackground == true);
        this.progressNotBGs = this.processList.filter((e) => !e.RunBackground);
      },
      deep: true,
    },
    runBackground: function (newValue) {
      if (newValue == true) {
        commonJs.showMessenger({
          title: "Thông báo",
          msg:
            "Quá trình sẽ tiếp tục được xử lý trong ít phút. Hệ thống sẽ thông báo cho bạn khi hoàn thành.",
          type: Enum.MsgType.Info,
          showCancelButton: false,
        });
      }
    },
    value: function (newValue) {
      if (newValue == this.max) {
        commonJs.showToast("Đã hoàn thành", Enum.MsgType.Success);
      }
    },
  },
  methods: {},
  data() {
    return {
      progressNotBGs: [],
      progressBGs: [],
    };
  },
};
</script>
<style scoped>
.progressbar {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #00000065;
  z-index: 11510;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.progressbar-bg {
  position: fixed;
  bottom: 8px;
  right: 8px;
  z-index: 11510;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.loading-progressbar {
  position: relative;
}
</style>
