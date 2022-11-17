<template>
  <div v-if="events.length > 0" class="news" @click="showNews">
    <button class="close-comment" @click="onClose">
      <i class="icofont-close"></i>
    </button>
    <div class="news__icon"><i class="icofont-hand-right"></i></div>
    <div class="news-content">
      <div class="news__info">THÔNG BÁO HOT: BẤM ĐỂ ĐĂNG KÝ [{{ item.EventName }}]!</div>

      <span
        >Hạn đăng ký: <span class="time--left">{{ timeLeftInfo }}</span></span
      >
    </div>
  </div>
</template>
<script>
import router from "./router";
export default {
  name: "NewsNotice",
  props: [],
  emits: ["onClose"],
  created() {
    this.api({ url: "api/v1/events/event-left-time" }).then((res) => {
      this.events = res;
      if (res && res.length > 0) {
        var lastest = res[0];
        this.item = lastest;
        console.log(lastest.ExpireRegisterDate);
        if (!lastest.ExpireRegisterDate && lastest.EventDate)
          this.item.ExpireRegisterDate = lastest.EventDate;
      }
      this.interValTime();
    });
  },
  methods: {
    onClose() {
      this.$emit("onClose");
      event.stopPropagation();
    },

    showNews() {
      router.push("/events");
      this.$emit("onClose");
    },
    interValTime() {
      this.calculatorTimeInfo();
      var nowTime = new Date();
      var timeStart = new Date(this.item.ExpireRegisterDate);
      if (timeStart > nowTime) {
        setInterval(this.calculatorTimeInfo, 1000);
      }
    },
    calculatorTimeInfo() {
      var self = this;
      //   console.log(this.timeLeftInfo);
      try {
        var currentDate = new Date();
        var value = new Date(self.item.ExpireRegisterDate);
        var timeBetween = value - currentDate;
        if (timeBetween < 0) {
          self.timeLeftInfo = null;
          return;
        }
        var secondsBetween = Math.floor(timeBetween / 1000);
        if (secondsBetween < 60) {
          self.timeLeftInfo = `${secondsBetween} giây.`;
          return;
        }

        var minutesBetween = Math.floor(secondsBetween / 60);
        var seconds = Math.floor(secondsBetween % 60);
        seconds = seconds < 10 ? `0${seconds}` : seconds;
        if (minutesBetween < 60) {
          self.timeLeftInfo = `${minutesBetween} phút ${seconds} giây.`;
          return;
        }

        var hoursBetween = Math.floor(minutesBetween / 60);
        var minutes = Math.floor(minutesBetween % 60);
        minutes = minutes < 10 ? `0${minutes}` : minutes;
        if (hoursBetween < 24) {
          self.timeLeftInfo = `${hoursBetween} giờ ${minutes} phút ${seconds} giây.`;
          return;
        }

        var daysBetween = Math.floor(hoursBetween / 24);
        var hours = Math.floor(hoursBetween % 24);
        hours = hours < 10 ? `0${hours}` : hours;
        if (daysBetween < 30) {
          self.timeLeftInfo = `${daysBetween} ngày ${hours}:${minutes}:${seconds}.`;
          return;
        }

        var monthsBetween = Math.floor(daysBetween / 30);
        var days = Math.floor(daysBetween % 30);
        if (monthsBetween < 12) {
          self.timeLeftInfo = `${monthsBetween} tháng ${days} ngày ${hours}:${minutes}:${seconds}.`;
          return;
        }

        var yearsBetween = Math.floor(monthsBetween / 12);

        if (yearsBetween < 12) {
          self.timeLeftInfo = `${yearsBetween} năm.`;
          return;
        }
      } catch (error) {
        self.timeLeftInfo = "";
      }
    },
  },
  data() {
    return {
      item: {},
      timeLeftInfo: null,
      events: [],
    };
  },
};
</script>
<style scoped>
.close-comment {
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
  color: #ff0000;
  border-color: #ff0000;
  border-style: solid;
}
.news {
  display: flex;
  align-items: center;
  border-radius: 4px;
  padding: 10px;
  position: fixed;
  bottom: 10px;
  right: 10px;
  left: 10px;
  background-color: #fff;
  box-shadow: 0px 0px 10px #404040;
  z-index: 1995;
  background-color: yellow;
}
.news__icon {
  font-size: 24px;
  margin-right: 10px;
  color: #0033ff;
}
.news-content {
}

.news__info {
  font-weight: 700;
  font-size: 16px;
}

.time--left {
  color: #ff0000;
  font-weight: 700;
}
</style>
