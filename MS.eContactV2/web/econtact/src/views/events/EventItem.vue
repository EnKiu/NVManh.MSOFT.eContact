<template>
  <div class="event">
    <div class="event-info event__title">
      <div class="event__text" :class="{ '--cancel': item.IsCancel == true }">
        {{ item.EventName }}
      </div>
    </div>
    <div class="event-info event__status">
      <span
        v-if="timeLeftInfo != null && item.IsCancel == false"
        class="time--left"
        >Đăng ký kết thúc sau
        <span class="time--left-text">{{ timeLeftInfo }}</span></span
      >
      <span
        v-if="timeLeftInfo == null && item.IsCancel == false"
        class="time--ended"
        >Đã kết thúc</span
      >
      <span v-if="item.IsCancel == true" class="--cancel">Đã hủy bỏ</span>
    </div>
    <div class="event-info event__date">
      <div class="event__label"><i class="icofont-clock-time"></i> Ngày:</div>
      <div class="event__text">{{ eventDateFormat }}</div>
    </div>
    <div class="event-info event__time">
      <div class="event__label"><i class="icofont-ui-alarm"></i> Thời gian bắt đầu:</div>
      <div class="event__text">
        {{ timeStartFormat }}
      </div>
    </div>
    <div class="event-info event__place">
      <div class="event__label"><i class="icofont-ui-map"></i> Địa điểm:</div>
      <div class="event__text">{{ item.EventPlace }}</div>
    </div>
    <div class="event-info event__spend">
      <div class="event__label"><i class="icofont-money"></i> Kinh phí:</div>
      <div class="event__text">{{ moneyFormat }}</div>
    </div>
    <div class="event-info event__content">
      {{ item.EventContent }}
    </div>
    <div
      v-if="timeLeftInfo != null && item.IsCancel == false"
      class="event__button"
    >
      <button class="btn btn--default">
        <div>Đăng ký tham gia ngay</div>
        <div>(Còn <span class="--color-red --bold">{{ timeLeftInfo }}</span>)</div>
      </button>
    </div>

    <div v-if="item.IsCancel == false" class="event__joinned-number">Có {{item.JoinnedNumber}} đã tham gia <i class="icofont-swoosh-right"></i> Xem chi tiết</div>
  </div>
</template>
<script>
export default {
  name: "EventItem",
  props: ["item"],
  emits: [],
  computed: {
    eventDateFormat: function () {
      var eventDate = this.item.EventDate;
      console.log(this.commonJs.formatDate(eventDate));
      return this.commonJs.formatDate(eventDate);
    },
    timeStartFormat: function () {
      var startTime = this.item.StartTime;
      return this.commonJs.getTime(startTime);
    },
    moneyFormat: function () {
      return this.commonJs.formatMoney(this.item.Spends);
    },
    calculatorTimeLeft: function () {
      var startTime = new Date(this.item.StartTime);
      if (!this.item.StartTime || startTime == null) {
        return null;
      }
      var timeNow = new Date("2019-02-15");
      var timeLeft = startTime - timeNow;
      if (timeLeft < 0) {
        return null;
      }
      return timeLeft;
    },
    calculatorTimeRegisterLeft: function () {
      return null;
    },
  },
  created() {
    this.interValTime();
  },
  methods: {
    interValTime() {
      this.calculatorTimeInfo();
      var nowTime = new Date();
      var timeStart = new Date(this.item.StartTime);
      if (timeStart > nowTime) {
        setInterval(this.calculatorTimeInfo, 1000);
      }
    },
    calculatorTimeInfo() {
      var self = this;
      //   console.log(this.timeLeftInfo);
      try {
        var currentDate = new Date();
        var value = new Date(self.item.StartTime);
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
      timeLeftInfo: null,
    };
  },
};
</script>
<style scoped>
.time--left {
  color: #3395ff;
}

.time--left-text {
  color: #ff0000;
  font-weight: 700;
}
.time--ended {
  color: #00b87a;
}

.event-info .--cancel {
  color: #ff0000;
}

.event__title{
    font-size: 20px;
    padding-top: unset !important;
}
.event__text.--cancel {
  text-decoration: line-through;
}
.event {
  padding: 24px;
  border-radius: 4px;
  box-shadow: 0 3px 6px #ccc;
}

.event + .event {
  margin-top: 10px;
}
.event-info {
  display: flex;
  align-items: center;
  padding: 4px 0;
}

.event__label {
  width: 150px;
  display: flex;
  align-items: center;
}

.event__label i{
    font-size: 16px;
    margin-right: 10px;
}
.event__title {
  font-weight: 700;
}
.event__date {
}
.event__time {
}
.event__place {
}
.event__spend {
}

.event__content {
}

.event__button {
  display: flex;
  align-items: center;
  justify-content: flex-end;
}
.btn{
    height: 50px;
}

.event__joinned-number{
    margin-top: 10px;
    font-style: italic;
}
</style>