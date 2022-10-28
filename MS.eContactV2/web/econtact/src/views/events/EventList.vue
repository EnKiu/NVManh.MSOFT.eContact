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
        :isAdmin="isAdmin"
        :key="index"
        :item="item"
        @onRegister="onRegister(item)"
        @onCancelRegister="onCancelRegister(item)"
        @onShowList="onShowList(item)"
        @onShowContentDetail="onShowContentDetail(item)"
        @onRemoveEvent="onRemoveEvent(item)"
        @onEditEvent="onEditEvent(item)"
      ></event-item>
    </div>

    <!-- ĐĂNG KÝ SỰ KIỆN -->
    <event-register
      v-if="showRegister"
      :isAdmin="isAdmin"
      @onClose="showRegister = false"
      @onRegisterSuccess="onCloseRegister"
      v-model:TotalAccompanying="eventRegister.TotalAccompanying"
      v-model:TotalMember="eventRegister.TotalMember"
      :eventRegister="eventRegister"
    ></event-register>

    <!-- DANH SÁCH CHI TIẾT THAM GIA SỰ KIỆN -->
    <event-detail-list
      v-if="showDetailList"
      :isAdmin="isAdmin"
      @onCloseDetail="onCloseDetail"
      @onRegisterFromDetail="onRegister"
      @afterCancelRegisterSuccess="onCancelRegisterSuccess(eventDetail)"
      :eventItem="eventDetailSelected"
    ></event-detail-list>

    <!-- CHI TIẾT SỰ KIỆN -->
    <event-detail
      v-if="showDetail"
      :eventItem="eventDetailSelected"
      :formMode="detailFormMode"
      @onClose="showDetail = false"
      @onSaveSuccess="onSaveSuccess"
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
import Enum from "@/scripts/enum";
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
    var roleValue = localStorage.getItem("userRoleValue");
    if (roleValue == 1) {
      this.isAdmin = true;
    }
    this.loadData();
  },
  methods: {
    addNewEvent() {
      this.showDetail = true;
      this.detailFormMode = Enum.FormMode.ADD;
    },
    onSaveSuccess() {
      this.loadData();
      this.showDetail = false;
      this.detailFormMode = null;
    },
    onShowList(event) {
      this.eventDetailSelected = event;
      this.showDetailList = true;
    },
    onRegister(currentEvent) {
      this.showRegister = true;
      this.eventRegister = currentEvent;
    },
    onCancelRegister(currentEvent) {
      // Hỏi:
      console.log(currentEvent);
      var eventId = currentEvent.EventId;
      this.commonJs.showConfirm(
        "Bạn có chắc chắn muốn hủy tham gia sự kiện này không?",
        () => {
          this.api({
            url: "/api/v1/events/register?eventId=" + eventId,
            method: "DELETE",
          }).then((res) => {
            console.log(res);
            this.loadData();
          });
        }
      );
      // this.showRegister = true;
      this.eventRegister = currentEvent;
    },
    onShowContentDetail(event) {
      this.showContentDetail = true;
      this.eventDetailSelected = event;
    },
    async onCloseRegister(contactRegister, eventRegister) {
      // Cập nhật lại thông tin event đăng ký:
      this.eventRegister = eventRegister;
      eventRegister.NotRegisted = false;
      // Đóng form đăng ký
      this.showRegister = false;
      // Hiển thị form chi tiết danh sách đăng ký
      this.eventDetailSelected = eventRegister;
      this.showDetailList = true;
    },
    onRemoveEvent(event) {
      this.commonJs.showConfirm("Bạn có chắc chắn muốn xóa sự kiện này không?", () => {
        this.api({
          url: "/api/v1/events/" + event.EventId,
          method: "DELETE",
        }).then(() => {
          this.loadData();
        });
      });
    },
    onEditEvent(event) {
      console.log(event);
      this.eventDetailSelected = event;
      this.detailFormMode = Enum.FormMode.UPDATE;
      this.showDetail = true;
    },
    onCancelRegisterSuccess() {
      this.loadData();
    },
    onCloseDetail() {
      this.showDetailList = false;
    },

    loadData() {
      this.api({ url: "/api/v1/events" }).then((res) => {
        this.events = res;
      });
      // fetch(baseUrl + "/api/v1/events")
      //   .then((res) => res.json())
      //   .then((res) => {
      //     this.events = res;
      //   })
      //   .catch((res) => {
      //     console.log(res);
      //   });
    },
  },
  data() {
    return {
      isAdmin: false,
      detailFormMode: null,
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
  display: flex;
  align-items: center;
  background-color: #fff;
  justify-content: flex-start;
}

.event-list {
  margin-top: 10px;
}
</style>
