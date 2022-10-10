<template>
  <div class="content-dialog">
    <div class="event-content">
      <button class="dialog-content__button" @click="onClose">
        <i class="icofont-close"></i>
      </button>
      <div class="event__title">{{eventSelected.EventName}}</div>
      <div class="event__text"
        v-if="eventSelected.EventContent && eventSelected.EventContent.trim() != ''"
        v-html="eventSelected.EventContent"
      ></div>
      
      <div v-else>(Trống)</div>
      <div class="event__footer">
        <button class="btn btn--default" @click="onClose">Đóng</button>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "EventItemContent",
  props: ["eventSelected"],
  emits: ["onCloseEventDetailContent"],
  created() {
    console.log(this.eventSelected);
  },
  methods: {
    onClose() {
      this.$emit("onCloseEventDetailContent");
    },
  },
  data() {
    return {};
  },
};
</script>
<style scoped>
.event__title{
    font-weight: 700;
    font-size: 16px;
    border-bottom: 1px solid #dedede;
    margin-bottom: 10px;
}

.event__text{
    max-height:calc(100vh - 150px);
    overflow-y: auto;
}

.event__footer{
    display: flex;
    align-items: center;
    justify-content: flex-end;
    margin-top: 10px;
}
.content-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #0000003c;
  display: flex;
  align-items: center;
  justify-content: center;
}
.content-dialog .event-content {
  position: relative;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0px 3px 6px #dedede;
  padding: 24px;
  margin: 10px;
  min-width: 300px;
}
.dialog-content__button {
  position: absolute;
  top: -10px;
  right: -10px;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}
</style>
