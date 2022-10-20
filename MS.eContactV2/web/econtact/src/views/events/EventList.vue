<template>
  <div class="container">
    <div class="toolbar">
      <button class="btn btn--default" @click="addNewEvent">
        <i class="icofont-ui-add"></i> Thêm sự kiện
      </button>
    </div>

    <!-- DANH SÁCH SỰ KIỆN -->
    <div class="event-list">
      <event-item
        v-for="(item, index) in events"
        :key="index"
        :item="item"
        @onRegister="onRegister(item)"
        @onShowList="onShowList(item)"
        @onShowContentDetail="onShowContentDetail(item)"
      ></event-item>
    </div>

    <!-- ĐĂNG KÝ SỰ KIỆN -->
    <event-register
      v-if="showRegister"
      @onClose="showRegister = false"
      @onRegisterSuccess="onCloseRegister"
      v-model:TotalAccompanying="eventRegister.TotalAccompanying"
      v-model:TotalMember="eventRegister.TotalMember"
      :eventRegister="eventRegister"
    ></event-register>

    <!-- DANH SÁCH CHI TIẾT THAM GIA SỰ KIỆN -->
    <event-detail-list
      v-if="showDetailList"
      @onCloseDetail="onCloseDetail"
      @onRegisterFromDetail="onRegister"
      @afterCancelRegisterSuccess="onCancelRegisterSuccess(eventDetail)"
      :eventItem="eventDetailSelected"
    ></event-detail-list>

    <!-- CHI TIẾT SỰ KIỆN -->
    <event-detail
      v-if="showDetail"
      @onClose="showDetail = false"
      @onAddSuccess="onAddEventSuccess"
    ></event-detail>

    <EventItemContent
      v-if="showContentDetail"
      :eventSelected="eventDetailSelected"
      @onCloseEventDetailContent="showContentDetail = false"
    ></EventItemContent>
  </div>
</template>
<script>
import EventItem from "./EventItem.vue";
import EventRegister from "./EventRegister.vue";
import EventDetail from "./EventDetail.vue";
import EventDetailList from "./EventDetailList.vue";
import EventItemContent from "./EventItemContent.vue";
export default {
  name: "EventList",
  components: {
    EventItem,
    EventRegister,
    EventDetailList,
    EventDetail,
    EventItemContent,
  },
  emits: [],
  props: [],
  created() {
    this.loadData();
  },
  methods: {
    addNewEvent() {
      this.showDetail = true;
    },
    onAddEventSuccess() {
      this.loadData();
    },
    onShowList(event) {
      console.log(event);
      this.eventDetailSelected = event;
      this.showDetailList = true;
    },
    onRegister(currentEvent) {
      this.showRegister = true;
      this.eventRegister = currentEvent;
    },
    onShowContentDetail(event) {
      this.showContentDetail = true;
      this.eventDetailSelected = event;
    },
    async onCloseRegister(contactRegister, eventRegister) {
      // Cập nhật lại thông tin event đăng ký:
      this.eventRegister = eventRegister;
      // Đóng form đăng ký
      this.showRegister = false;
      // Hiển thị form chi tiết danh sách đăng ký
      this.eventDetailSelected = eventRegister;
      this.showDetailList = true;
    },
    onCancelRegisterSuccess() {
      this.loadData();
    },
    onCloseDetail() {
      this.showDetailList = false;
    },

    loadData() {
      var baseUrl = process.env.VUE_APP_BASE_URL;
      fetch(baseUrl + "/api/v1/events")
        .then((res) => res.json())
        .then((res) => {
          this.events = res;
        })
        .catch((res) => {
          console.log(res);
        });
    },
  },
  data() {
    return {
      events: [],
      eventRegister: {},
      eventDetailSelected: null,
      showDetail: false,
      showRegister: false,
      showDetailList: false,
      showContentDetail: false,
    };
  },
};
</script>
<style scoped>
.container {
  box-sizing: border-box;
  margin: 0 auto;
  max-width: 350px;
}

.toolbar {
  position: sticky;
  top: 0;
  display: flex;
  align-items: center;
  background-color: #fff;
  justify-content: flex-start;
}

.event-list {
  margin-top: 10px;
}
</style>
