<template>
  <div class="event">
    <div class="event__toolbar">
      <button
        v-if="isAdmin"
        class="btn--remove-item"
        @click.prevent="onRemoveEvent"
        title="Xóa sự kiện"
      >
        <i class="icofont-ui-remove"></i>
      </button>
      <button
        v-if="isAdmin"
        class="btn--edit-item"
        @click.prevent="onEditEvent"
        title="Sửa sự kiện"
      >
        <i class="icofont-ui-edit"></i>
      </button>
    </div>
    <div class="event-info event__title">
      <div class="event__text" :class="{ '--cancel': item.IsCancel == true }">
        {{ item.EventName }}
      </div>
    </div>
    <div class="event-info event__status">
      <span v-if="timeLeftInfo != null && item.IsCancel == false" class="time--left"
        >Hạn đăng ký: <span class="time--left-text">{{ timeLeftInfo }}</span></span
      >
      <span v-if="timeLeftInfo == null && item.IsCancel == false" class="time--ended"
        >Đã kết thúc</span
      >
      <span v-if="item.IsCancel == true" class="--cancel">Đã hủy bỏ</span>
    </div>
    <div class="event-info event__status">
      <div class="event__text">
        (
        <span v-if="item.NotRegisted" class="register-status--not">
          <span v-if="timeLeftInfo != null && item.IsCancel == false"
            >Bạn chưa đăng ký tham gia sự kiện này.</span
          >
          <span v-else>Bạn không tham gia sự kiện này.</span>
        </span>

        <span v-else class="register-status--yes">
          <span v-if="timeLeftInfo == null && item.IsCancel == false" class="time--ended"
            >Đã tham dự</span
          >
          <span v-else>Đã đăng ký</span>
        </span>
        )
      </div>
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
      <div class="event__label"><i class="icofont-google-map"></i> Địa điểm:</div>
      <div class="event__text">{{ item.EventPlace }}</div>
    </div>
    <div class="event-info event__spend">
      <div class="event__label"><i class="icofont-money-bag"></i> Kinh phí:</div>
      <div class="event__text">{{ moneyFormat }}</div>
    </div>
    <div class="event-info event__spendtotals">
      <div class="event__label"><i class="icofont-money"></i> Kinh phí đã thu:</div>
      <div class="event__text"><b>{{ moneyTotalFormat }}</b></div>
    </div>
    <div class="event-info event__content">
      <button class="link--show-content" @click="onShowContentDetail">
        <i class="icofont-swoosh-right"></i>Xem nội dung sự kiện
      </button>
    </div>
    <div v-if="timeLeftInfo != null && item.IsCancel == false" class="event__button">
      <button v-if="item.NotRegisted" class="btn btn--default" @click="onRegister">
        <div>Đăng ký tham gia ngay</div>
        <div>
          (Còn <span class="--color-red --bold">{{ timeLeftInfo }}</span
          >)
        </div>
      </button>
      <button v-else class="btn dialog__button--cancel" @click="onCancelRegister">
        <div>Hủy đăng ký</div>
      </button>
    </div>

    <div v-if="item.IsCancel == false" class="event__joinned-number">
      <div v-if="item.TotalMember > 0">
        Có <b>{{ item.TotalMember }}</b> thành viên đăng ký.
        <i class="icofont-swoosh-right"></i>
        <a class="show-list-registers" @click="onShowList">Xem danh sách chi tiết</a>
      </div>
      <span v-else>Không có ai tham gia.</span>
    </div>
  </div>
</template>
<script>
export default {
  name: "EventItem",
  components: {},
  props: ["item", "isAdmin"],
  emits: [
    "onRegister",
    "onShowList",
    "onShowContentDetail",
    "onCancelRegister",
    "onRemoveEvent",
    "onEditEvent",
  ],
  computed: {
    eventDateFormat: function () {
      var eventDate = this.item.EventDate;
      return this.commonJs.formatDate(eventDate);
    },
    timeStartFormat: function () {
      var startTime = this.item.StartTime;
      return this.commonJs.getTime(startTime);
    },
    moneyFormat: function () {
      return this.commonJs.formatMoney(this.item.Spends);
    },
    moneyTotalFormat: function () {
      return this.commonJs.formatMoney(this.item.SpendsTotal);
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
    onShowList() {
      this.$emit("onShowList", this.item);
    },
    onRegister() {
      this.$emit("onRegister", this.item);
    },
    onCancelRegister() {
      this.$emit("onCancelRegister", this.item);
    },
    onShowContentDetail() {
      this.$emit("onShowContentDetail", this.item);
    },
    onRemoveEvent() {
      this.$emit("onRemoveEvent", this.item);
    },
    onEditEvent() {
      this.$emit("onEditEvent", this.item);
    },
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

.event__title {
  font-size: 20px;
  padding-top: unset !important;
}
.event__text.--cancel {
  text-decoration: line-through;
}
.event {
  position: relative;
  padding: 16px;
  border-radius: 4px;
  box-shadow: 0px 0px 10px #404040;
  margin: 0 auto;
}

.event + .event {
  margin-top: 16px;
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

.event__label i {
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
.btn {
  height: 50px;
}

.event__joinned-number {
  margin-top: 10px;
  font-style: italic;
}

.show-list-registers {
  color: #3395ff;
  font-weight: 700;
  cursor: pointer;
}

.show-list-registers:hover,
.show-list-registers:focus,
.link--show-content:hover,
.link--show-content:focus {
  text-decoration: underline;
}

.link--show-content {
  border: unset;
  background: unset;
  color: #3395ff;
  font-weight: 700;
  cursor: pointer;
}

.register-status--not {
  color: #ff0000;
}

.register-status--yes {
  color: #00b87a;
}

.event__toolbar {
  position: absolute;
  top: 0px;
  right: 0px;
}

.event__toolbar button + button {
  margin-top: 4px;
}
.btn--remove-item {
  position: relative;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  z-index: 2;
  color: #ff0000;
  border-color: #ff0000;
  border-style: solid;
}

.btn--edit-item {
  position: relative;
  top: -10px;
  right: -10px;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  z-index: 2;
  color: #005c20;
  border-color: #005c20;
  border-style: solid;
}
</style>
